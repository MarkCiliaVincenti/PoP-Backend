using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PopLibrary.Stripe
{
    public class StripeAdapter
    {
        private readonly HttpClient _httpClient;
        private readonly StripeSettings _stripeSettings;

        public StripeAdapter(
            IHttpClientFactory httpClientFactory,
            StripeSettings stripeSettings)
        {
            _httpClient = httpClientFactory.CreateClient();
            _stripeSettings = stripeSettings;
            StripeConfiguration.ApiKey = stripeSettings.Key;
        }

        public string GetOrCreateCustomerForEmail(string email)
        {
            var service = new CustomerService();
            var customers = service.List();
            if (customers.Any(c => c.Email == email))
            {
                return customers.First(c => c.Email == email).Id;
            }
            var options = new CustomerCreateOptions
            {
                Email = email
            };
            Customer customer = service.Create(options);
            return customer.Id;
        }

        public string CreateInvoiceItem(string customerId, decimal amount, string description)
        {
            var options = new InvoiceItemCreateOptions
            {
                Customer = customerId,
                Amount = Convert.ToInt64(amount) * 100,
                Currency = "usd",
                Description = description
            };
            var service = new InvoiceItemService();
            InvoiceItem invoiceItem = service.Create(options);
            return invoiceItem.Id;
        }

        public string CreateInvoice(string customerId)
        {
            var options = new InvoiceCreateOptions
            {
                Customer = customerId,
                CollectionMethod = "send_invoice",
                DaysUntilDue = 7,
                Metadata = new Dictionary<string, string> { { "chargeOrigin", "gala" } }
            };
            var service = new InvoiceService();
            try
            {
                Invoice invoice = service.Create(options);
                return invoice.Id;
            }
            catch
            {
                return "-1";
            }
        }
    }
}
