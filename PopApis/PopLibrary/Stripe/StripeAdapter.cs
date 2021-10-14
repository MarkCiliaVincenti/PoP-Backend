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
        public StripeAdapter(
            IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public string GetOrCreateCustomerForEmail(string email)
        {
            StripeConfiguration.ApiKey = "sk_test_51JkGVHDElXhh7M42Fo8vSv9bozntSKa458CzLF1dnnaM92t3oVgsLW8Sial62Ra5t1Z6UqN7T1AAcIMQ0BGouwWe00YPt2hKJo";

            var options = new CustomerCreateOptions
            {
                Email = email
            };
            var service = new CustomerService();
            Customer customer = service.Create(options);
            return customer.Id;
        }

        public string GetOrCreateInvoiceItem(string email)
        {
            StripeConfiguration.ApiKey = "sk_test_51JkGVHDElXhh7M42Fo8vSv9bozntSKa458CzLF1dnnaM92t3oVgsLW8Sial62Ra5t1Z6UqN7T1AAcIMQ0BGouwWe00YPt2hKJo";

            var options = new CustomerCreateOptions
            {
                Email = email
            };
            var service = new CustomerService();
            Customer customer = service.Create(options);
            return customer.Id;
        }
    }
}
