using System;

namespace PopLibrary.SqlModels
{
    public class Customer
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string StripeCustomerId { get; set; }

        public DateTime Created { get; set; }
    }
}
