using Microsoft.AspNetCore.Mvc;
using PopApis.Models;
using PopLibrary;
using PopLibrary.Helpers;
using PopLibrary.SqlModels;
using PopLibrary.Stripe;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PopApis.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
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

        // GET: api/<AccountingController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<AccountingController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<AccountingController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<AccountingController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AccountingController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
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
            var customers = _sqlAdapter.ExecuteStoredProcedureAsync<Customer>("dbo.GetCustomers");
            foreach (var customer in customers)
            {
                var stripeCustomerId = _stripeAdapter.GetOrCreateCustomerForEmail(customer.Email);
                _sqlAdapter.ExecuteStoredProcedureAsync("dbo.AddOrUpdateStripeCustomerId", new List<StoredProcedureParameter>
                {
                    new StoredProcedureParameter { Name = "@CustomerId", DbType = SqlDbType.Int, Value = customer.Id },
                    new StoredProcedureParameter { Name = "@StripeCustomerId", DbType = SqlDbType.NVarChar, Value = stripeCustomerId }
                });
            }
            // 5. Get payments and create invoice items for each payment that is incomplete, updating the payment row accordingly
            var incompletePayments = _sqlAdapter.ExecuteStoredProcedureAsync<GetPaymentResult>("dbo.GetPayments")
                .Where(payment => !payment.Complete);
            foreach (var payment in incompletePayments)
            {
                var invoiceItemId = _stripeAdapter.CreateInvoiceItem(
                    payment.StripeCustomerId,
                    payment.Amount,
                    payment.Description);
                _sqlAdapter.ExecuteStoredProcedureAsync("dbo.AddOrUpdatePayment", new List<StoredProcedureParameter>
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
            var customerStripeIds = (_sqlAdapter.ExecuteStoredProcedureAsync<Customer>("dbo.GetCustomers"))
                .Select(customer => customer.StripeCustomerId)
                .Distinct();
            foreach (var customerStripeId in customerStripeIds)
            {
                var invoiceId = _stripeAdapter.CreateInvoice(
                    customerStripeId);
                customerToInvoiceMap.Add(customerStripeId, invoiceId);
            }
            var allPayments = _sqlAdapter.ExecuteStoredProcedureAsync<GetPaymentResult>("dbo.GetPayments");
            foreach (var payment in allPayments)
            {
                var invoiceId = customerToInvoiceMap[payment.StripeCustomerId];
                _sqlAdapter.ExecuteStoredProcedureAsync("dbo.AddOrUpdatePayment", new List<StoredProcedureParameter>
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
    }
}
