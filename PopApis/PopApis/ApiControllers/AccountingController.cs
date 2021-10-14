using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PopLibrary;
using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using Stripe;
using System.Threading.Tasks;

namespace PopApis.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Admin")]
    public class AccountingController : ControllerBase
    {
        private SqlAdapter _sqlAdapter;
        public AccountingController(SqlAdapter sqlAdapter)
        {
            _sqlAdapter = sqlAdapter;
        }

        // GET api/<AccountingController>/allIDs/
        /// <summary>
        /// For calcuating totals of bids.
        /// Gets bid auction IDs of all auctions between <paramref name="startDate"/ and <paramref name="endDate">.
        /// </summary>
        [HttpGet("ids/{startDate}/{endDate}")]
        public IEnumerable<int> GetAllBidAuctionIDs(DateTime startDate, DateTime endDate)
        {
            var result = _sqlAdapter.ExecuteStoredProcedure<int>("dbo.GetAllAuctionIDs", new List<StoredProcedureParameter>
            {
                new StoredProcedureParameter { Name = "@StartDate", DbType = SqlDbType.DateTime, Value = startDate },
                new StoredProcedureParameter { Name = "@EndDate", DbType = SqlDbType.DateTime, Value = endDate }
            });
            return result;
        }

        // GET api/<AccountingController>/allDonations/
        /// <summary>
        /// For calcuating totals of donations.
        /// Gets all donation amounts between <paramref name="startDate"/ and <paramref name="endDate">.
        /// </summary>
        [HttpGet("donations/{startDate}/{endDate}")]
        public IEnumerable<int> GetAllDonationAmounts(DateTime startDate, DateTime endDate)
        {
            var result = _sqlAdapter.ExecuteStoredProcedure<int>("dbo.GetAllDonationAmounts", new List<StoredProcedureParameter>
            {
                new StoredProcedureParameter { Name = "@StartDate", DbType = SqlDbType.DateTime, Value = startDate },
                new StoredProcedureParameter { Name = "@EndDate", DbType = SqlDbType.DateTime, Value = endDate }
            });
            return result;
        }

        const string endpointSecret = "whsec_j7Fn816sxt8rBFqNbkhaytfrhmY9JhQK";
        [AllowAnonymous]
        [HttpPost("webhook")]
        public async Task<IActionResult> Index()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], endpointSecret, 300, false);

                // Handle the event
                if (stripeEvent.Type == Events.PaymentIntentSucceeded)
                {
                    Console.WriteLine("hi");

                    PaymentIntent paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
                    var email = paymentIntent.Customer.Email;
                    var stripeCustomerId = paymentIntent.Customer.Id;
                    var amount = paymentIntent.Amount % 100;
                    var auctionId = "";
                    paymentIntent.Metadata.TryGetValue("auctionId", out auctionId);

                    var customerId = _sqlAdapter.ExecuteStoredProcedure<int>("dbo.AddOrUpdateCustomer", new List<StoredProcedureParameter>
                    {
                        new StoredProcedureParameter { Name="@Email", DbType=SqlDbType.NVarChar, Value=email },
                        new StoredProcedureParameter { Name="@StripeCustomerId", DbType=SqlDbType.NVarChar, Value=stripeCustomerId },
                    });

                    _sqlAdapter.ExecuteStoredProcedure<int>("dbo.AddOrUpdatePayment", new List<StoredProcedureParameter>
                    {
                        new StoredProcedureParameter { Name="@AuctionId", DbType=SqlDbType.Int, Value=auctionId },
                        new StoredProcedureParameter { Name="@CustomerId", DbType=SqlDbType.Int, Value=customerId },
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
