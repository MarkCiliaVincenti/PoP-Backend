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
            var options = new CustomerCreateOptions
            {
                Email = email
            };
            var service = new CustomerService();
            Customer customer = service.Create(options);
            return customer.Id;
        }

        public string GetOrCreateInvoiceItem(string customerId, decimal amount, string description)
        {
            var options = new InvoiceItemCreateOptions
            {
                Customer = customerId,
                Amount = Convert.ToInt64(amount),
                Currency = "usd",
                Description = description
            };
            var service = new InvoiceItemService();
            InvoiceItem invoiceItem = service.Create(options);
            return invoiceItem.Id;
        }
    }
}
