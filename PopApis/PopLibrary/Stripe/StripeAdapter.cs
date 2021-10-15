using StripeSDK = Stripe;
using Stripe.Checkout;
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
            StripeSDK.StripeConfiguration.ApiKey = stripeSettings.Key;
        }

        public string GetWebhookSecret()
        {
            return _stripeSettings.WebhookSecret;
        }

        public string GetOrCreateCustomerForEmail(string email)
        {
            var service = new StripeSDK.CustomerService();
            var customers = service.List();
            if (customers.Any(c => c.Email == email))
            {
                return customers.First(c => c.Email == email).Id;
            }
            var options = new StripeSDK.CustomerCreateOptions
            {
                Email = email
            };
            StripeSDK.Customer customer = service.Create(options);
            return customer.Id;
        }

        public string CreateInvoiceItem(string customerId, decimal amount, string description)
        {
            var options = new StripeSDK.InvoiceItemCreateOptions
            {
                Customer = customerId,
                Amount = Convert.ToInt64(amount) * 100,
                Currency = "usd",
                Description = description
            };
            var service = new StripeSDK.InvoiceItemService();
            StripeSDK.InvoiceItem invoiceItem = service.Create(options);
            return invoiceItem.Id;
        }

        public string CreateInvoice(string customerId)
        {
            var options = new StripeSDK.InvoiceCreateOptions
            {
                Customer = customerId,
                CollectionMethod = "send_invoice",
                DaysUntilDue = 7,
                Metadata = new Dictionary<string, string> { { "chargeOrigin", "gala" } }
            };
            var service = new StripeSDK.InvoiceService();
            try
            {
                StripeSDK.Invoice invoice = service.Create(options);
                return invoice.Id;
            }
            catch
            {
                return "-1";
            }
        }

        public string CreateSession(string email, decimal amount, int auctionId)
        {
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        Name = "Gift",
                        Description = "Pencils of Promise Gala",
                        Images = new List<string> { "https://files.stripe.com/files/f_live_xdy93ZhMy8ouxq1KzsZYziYH" },
                        Currency = "usd",
                        Amount = Convert.ToInt64(amount) * 100,
                        Quantity = 1,
                    },
                },
                PaymentMethodTypes = new List<string>
                {
                  "card",
                },
                PaymentIntentData = new SessionPaymentIntentDataOptions()
                {
                    Metadata = new Dictionary<string, string>()
                    {
                        { "chargeOrigin", "gala" },
                        { "auction_id", auctionId.ToString() },
                        { "source", "gala" },
                        { "customer_email", email.ToLower() },
                    }
                },
                BillingAddressCollection = "auto",
                SuccessUrl = "https://live.pencilsofpromise.org/thankyou",
                CancelUrl = "https://live.pencilsofpromise.org/",
            };
            var service = new SessionService();
            var session = service.Create(options);
            return session.Id;
        }
    }
}
