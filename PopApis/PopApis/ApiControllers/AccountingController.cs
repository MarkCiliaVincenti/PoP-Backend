
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PopApis.Models;
using PopLibrary;
using PopLibrary.Helpers;
using PopLibrary.SqlModels;
using PopLibrary.Stripe;
using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using Stripe;
using System.Threading.Tasks;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PopApis.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "User")]
    public class AccountingController : ControllerBase
    {
        private readonly FinalizeOptions _finalizeOptions;
        private readonly SqlAdapter _sqlAdapter;
        private readonly FinalizeHelper _finalizeHelper;
        private readonly StripeAdapter _stripeAdapter;

        public AccountingController(
            FinalizeOptions finalizeOptions,
            SqlAdapter sqlAdapter,
            FinalizeHelper finalizeHelper,
            StripeAdapter stripeAdapter)
        {
            _finalizeOptions = finalizeOptions;
            _sqlAdapter = sqlAdapter;
            _finalizeHelper = finalizeHelper;
            _stripeAdapter = stripeAdapter;
        }

        // GET api/accounting/info/1
        /// <summary>
        /// Returns accounting details for an auction.
        /// Gets information for auction with ID <paramref name="auctionID"/>.
        /// </summary>
        [HttpGet("info")]
        public AuctionViewModel getAuctionInfoById(int auctionID)
        {
            var auctionDetails = this.GetAuctionById(auctionID);
            var auctionHighestBid = this.GetHighestBidByAuctionId(auctionID);
            return new AuctionViewModel
            {
                AuctionId = auctionID,
                AuctionName = auctionDetails.FirstOrDefault().Title,
                AuctionTime = auctionDetails.FirstOrDefault().Created,
                AuctionType = auctionDetails.FirstOrDefault().AuctionTypeId,
                HighestBid = new BidViewModel
                {
                    BidAmount = auctionHighestBid.FirstOrDefault().Amount,
                    BidId = auctionHighestBid.FirstOrDefault().Id,
                    GuestId = auctionHighestBid.FirstOrDefault().Email,
                    PaidStatus = auctionHighestBid.FirstOrDefault().Amount <= 100
                }
            };
        }

        // GET api/accounting/bids
        // GET api/accounting/bids?startDate=10/13/2021&endDate=10/14/2021
        /// <summary>
        /// Returns the total amount from silent and live auctions.
        /// Returns total of all highest bids between dates <paramref name="startDate"/> and <paramref name="endDate"/>.
        /// </summary>
        [HttpGet("bids")]
        public decimal getTotalBidAmount([FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            var allIds = this.GetAllBidAuctionIDs(startDate, endDate);
            List<decimal> highestBidAmounts = new();
            highestBidAmounts.AddRange(allIds.Select(i => this.GetHighestBidByAuctionId(i.Id).FirstOrDefault().Amount));
            return highestBidAmounts.Sum();
        }

        // GET api/accounting/donations
        // GET api/accounting/donations?startDate=10/13/2021&endDate=10/14/2021
        /// <summary>
        /// Returns the total amount from all donations.
        /// Returns total of donations between dates <paramref name="startDate"/> and <paramref name="endDate"/>.
        /// </summary>
        [HttpGet("donations")]
        public decimal getTotalDonationAmount([FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            var allDonationAmounts = this.GetAllDonationAmounts(startDate, endDate);
            decimal total = 0;
            var sum = allDonationAmounts.Select(d => total + d.Amount);
            return total;
        }

        // GET api/accounting/total
        // GET api/accounting/total?startDate=10/13/2021&endDate=10/14/2021
        /// <summary>
        /// Returns the total amount from all auctions and donations.
        /// Returns total amount raised between dates <paramref name="startDate"/> and <paramref name="endDate"/>.
        /// </summary>
        [HttpGet("total")]
        public decimal getTotal([FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            return this.getTotalBidAmount(startDate, endDate) + this.getTotalDonationAmount(startDate, endDate);
        }

        private IEnumerable<GetAuctionIdResult> GetAllBidAuctionIDs([FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            var result = _sqlAdapter.ExecuteStoredProcedure<GetAuctionIdResult>("dbo.GetAllAuctionIDs", new List<StoredProcedureParameter>
            {
                new StoredProcedureParameter { Name = "@StartDate", DbType = SqlDbType.DateTime, Value = startDate },
                new StoredProcedureParameter { Name = "@EndDate", DbType = SqlDbType.DateTime, Value = endDate }
            });
            return result;
        }

        private IEnumerable<GetDonationAmountResult> GetAllDonationAmounts([FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            var result = _sqlAdapter.ExecuteStoredProcedure<GetDonationAmountResult>("dbo.GetAllDonationAmounts", new List<StoredProcedureParameter>
            {
                new StoredProcedureParameter { Name = "@StartDate", DbType = SqlDbType.DateTime, Value = startDate },
                new StoredProcedureParameter { Name = "@EndDate", DbType = SqlDbType.DateTime, Value = endDate }
            });
            return result;
        }

        private IEnumerable<GetAuctionsResult> GetAuctionById(int auctionTypeId)
        {
            var result = _sqlAdapter.ExecuteStoredProcedure<GetAuctionsResult>("dbo.GetAuctions", new List<StoredProcedureParameter>
            {
                new StoredProcedureParameter { Name="@AuctionTypeId", DbType=SqlDbType.Int, Value=auctionTypeId }
            });
            return result;
        }

        private IEnumerable<GetAuctionBidResult> GetHighestBidByAuctionId(int auctionId)
        {
            return _sqlAdapter.ExecuteStoredProcedure<GetAuctionBidResult>("dbo.GetHighestBid", new List<StoredProcedureParameter>
            {
                new StoredProcedureParameter { Name="@AuctionId", DbType=SqlDbType.Int, Value=auctionId }
            });
        }

        // POST api/<AccountingController>
        [HttpPost("finalize")]
        public string PostFinalize([FromBody] string key)
        {
            if (key != _finalizeOptions.FinalizeKey)
            {
                return "Bad key";
            }

            // 1. All outstanding donations already in Payment table
            // 2. Ingest silent auction highest bidders to Payment table
            _finalizeHelper.IngestAuctionResultsToPaymentTable((int)AuctionType.Silent);
            // 3. Ingest silent auction highest bidders to Payment table
            _finalizeHelper.IngestAuctionResultsToPaymentTable((int)AuctionType.Live);
            // 4. For each customer, update their stripe ID using stripe's returned value
            var customers = _sqlAdapter.ExecuteStoredProcedure<PopLibrary.SqlModels.Customer>("dbo.GetCustomers");
            foreach (var customer in customers)
            {
                var stripeCustomerId = _stripeAdapter.GetOrCreateCustomerForEmail(customer.Email);
                _sqlAdapter.ExecuteStoredProcedure("dbo.AddOrUpdateStripeCustomerId", new List<StoredProcedureParameter>
                {
                    new StoredProcedureParameter { Name = "@CustomerId", DbType = SqlDbType.Int, Value = customer.Id },
                    new StoredProcedureParameter { Name = "@StripeCustomerId", DbType = SqlDbType.NVarChar, Value = stripeCustomerId }
                });
            }
            // 5. Get payments and create invoice items for each payment that is incomplete, updating the payment row accordingly
            var incompletePayments = _sqlAdapter.ExecuteStoredProcedure<GetPaymentResult>("dbo.GetPayments")
                .Where(payment => !payment.Complete);
            foreach (var payment in incompletePayments)
            {
                var invoiceItemId = _stripeAdapter.CreateInvoiceItem(
                    payment.StripeCustomerId,
                    payment.Amount,
                    payment.Description);
                _sqlAdapter.ExecuteStoredProcedure("dbo.AddOrUpdatePayment", new List<StoredProcedureParameter>
                {
                    new StoredProcedureParameter { Name = "@PaymentId", DbType = SqlDbType.Int, Value = payment.Id },
                    new StoredProcedureParameter { Name = "@AuctionId", DbType = SqlDbType.Int, Value = payment.AuctionId },
                    new StoredProcedureParameter { Name = "@CustomerId", DbType = SqlDbType.Int, Value = payment.CustomerId },
                    new StoredProcedureParameter { Name = "@Complete", DbType = SqlDbType.Bit, Value = payment.Complete },
                    new StoredProcedureParameter { Name = "@StripeInvoiceItemId", DbType = SqlDbType.NVarChar, Value = invoiceItemId },
                    new StoredProcedureParameter { Name = "@Created", DbType = SqlDbType.DateTime, Value = payment.Created },
                    new StoredProcedureParameter { Name = "@Amount", DbType = SqlDbType.Decimal, Value = payment.Amount },
                    new StoredProcedureParameter { Name = "@Description", DbType = SqlDbType.Text, Value = payment.Description}
                });
            }
            // 6. Get all customer unique IDs and create one invoice for each one, saving mapping in dict. Then, updating the payment row accordingly for each.
            Dictionary<string, string> customerToInvoiceMap = new Dictionary<string, string>();
            var customerStripeIds = (_sqlAdapter.ExecuteStoredProcedure<PopLibrary.SqlModels.Customer>("dbo.GetCustomers"))
                .Select(customer => customer.StripeCustomerId)
                .Distinct();
            foreach (var customerStripeId in customerStripeIds)
            {
                var invoiceId = _stripeAdapter.CreateInvoice(
                    customerStripeId);
                customerToInvoiceMap.Add(customerStripeId, invoiceId);
            }
            var allPayments = _sqlAdapter.ExecuteStoredProcedure<GetPaymentResult>("dbo.GetPayments");
            foreach (var payment in allPayments)
            {
                var invoiceId = customerToInvoiceMap[payment.StripeCustomerId];
                _sqlAdapter.ExecuteStoredProcedure("dbo.AddOrUpdatePayment", new List<StoredProcedureParameter>
                {
                    new StoredProcedureParameter { Name = "@PaymentId", DbType = SqlDbType.Int, Value = payment.Id },
                    new StoredProcedureParameter { Name = "@AuctionId", DbType = SqlDbType.Int, Value = payment.AuctionId },
                    new StoredProcedureParameter { Name = "@CustomerId", DbType = SqlDbType.Int, Value = payment.CustomerId },
                    new StoredProcedureParameter { Name = "@Complete", DbType = SqlDbType.Bit, Value = payment.Complete },
                    new StoredProcedureParameter { Name = "@StripeInvoiceItemId", DbType = SqlDbType.NVarChar, Value = payment.StripeInvoiceItemId },
                    new StoredProcedureParameter { Name = "@StripeInvoiceId", DbType = SqlDbType.NVarChar, Value = invoiceId },
                    new StoredProcedureParameter { Name = "@Created", DbType = SqlDbType.DateTime, Value = payment.Created },
                    new StoredProcedureParameter { Name = "@Amount", DbType = SqlDbType.Decimal, Value = payment.Amount },
                    new StoredProcedureParameter { Name = "@Description", DbType = SqlDbType.Text, Value = payment.Description}
                });
            }
            return "Finalize operation successful";
        }

        // POST api/accounting/pledge
        /// <summary>
        /// Puts an incomplete payment in payments table.
        /// </summary>
        [HttpPost("pledge")]
        public string Pledge([FromBody] PledgeBody body)
        {
            var customer = _sqlAdapter.ExecuteStoredProcedure<PopLibrary.SqlModels.Customer>("dbo.AddOrUpdateCustomer", new List<StoredProcedureParameter>
            {
                new StoredProcedureParameter { Name="@Email", DbType=SqlDbType.NVarChar, Value=body.email }
            }).FirstOrDefault();
            _sqlAdapter.ExecuteStoredProcedure("dbo.AddOrUpdatePayment", new List<StoredProcedureParameter>
            {
                new StoredProcedureParameter { Name = "@AuctionId", DbType = SqlDbType.Int, Value = body.auctionId },
                new StoredProcedureParameter { Name = "@CustomerId", DbType = SqlDbType.Int, Value = customer.Id },
                new StoredProcedureParameter { Name = "@Complete", DbType = SqlDbType.Bit, Value = 0 },
                new StoredProcedureParameter { Name = "@Amount", DbType = SqlDbType.Decimal, Value = body.amount },
                new StoredProcedureParameter { Name = "@Description", DbType = SqlDbType.Text, Value = body.description}
            });

            return $"successfully created pledge for {body.amount}";
        }

        // POST api/accounting/session
        /// <summary>
        /// Create stripe checkout session and return ID.
        /// </summary>
        [HttpPost("session")]
        public SessionResponse Session([FromBody] SessionBody body)
        {
            var sessionId = _stripeAdapter.CreateSession(
                body.Email,
                body.Amount,
                body.AuctionId);

            return new SessionResponse() { Id = sessionId };
        }

        public class SessionBody
        {
            public string Email { get; set; }

            public int AuctionId { get; set; }

            public decimal Amount { get; set; }
        }

        public class SessionResponse
        {
            public string Id { get; set; }
        }

        public class PledgeBody
        {
            public string email { get; set; }
            public decimal amount { get; set; }
            public int auctionId { get; set; }
            public string description { get; set; }
        }

        [AllowAnonymous]
        [HttpPost("webhook")]
        public async Task<IActionResult> Index()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], _stripeAdapter.GetWebhookSecret(), 300, false);

                // Handle the event
                if (stripeEvent.Type == Events.PaymentIntentSucceeded)
                {
                    PaymentIntent paymentIntent = (PaymentIntent)stripeEvent.Data.Object;

                    if (string.IsNullOrEmpty(paymentIntent.InvoiceId))
                    {
                        return BadRequest();
                    }

                    var email = paymentIntent.Customer.Email;
                    var stripeCustomerId = paymentIntent.Customer.Id;
                    var amount = paymentIntent.Amount % 100;
                    string auctionId = "";
                    int auctionIdInt;
                    paymentIntent.Metadata.TryGetValue("auctionId", out auctionId);
                    if (string.IsNullOrEmpty(auctionId))
                    {
                        auctionIdInt = 0;
                    }
                    else
                    {
                        auctionIdInt = int.Parse(auctionId);
                    }

                    var customerId = _sqlAdapter.ExecuteStoredProcedure<int>("dbo.AddOrUpdateCustomer", new List<StoredProcedureParameter>
                    {
                        new StoredProcedureParameter { Name="@Email", DbType=SqlDbType.NVarChar, Value=email },
                        new StoredProcedureParameter { Name="@StripeCustomerId", DbType=SqlDbType.NVarChar, Value=stripeCustomerId },
                    });

                    _sqlAdapter.ExecuteStoredProcedure<int>("dbo.AddOrUpdatePayment", new List<StoredProcedureParameter>
                    {
                        new StoredProcedureParameter { Name="@AuctionId", DbType=SqlDbType.Int, Value=auctionIdInt },
                        new StoredProcedureParameter { Name="@CustomerId", DbType=SqlDbType.Int, Value=customerId },
                        new StoredProcedureParameter { Name="@Complete", DbType=SqlDbType.Bit, Value=1 },
                        new StoredProcedureParameter { Name="@Amount", DbType=SqlDbType.Decimal, Value=amount },
                    });

                }

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest();
            }
        }
    }
}
