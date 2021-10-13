using System;

namespace PopLibrary.SqlModels
{
    public class GetAuctionsResult
    {
        public int Id { get; set; }

        public int AuctionTypeId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Restrictions { get; set; }

        public bool IsActive { get; set; }

        public decimal Amount { get; set; }

        public DateTime Created { get; set; }

        public string ImageUrl { get; set; }
    }
}
