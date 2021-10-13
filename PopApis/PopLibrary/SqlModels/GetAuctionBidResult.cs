using System;

namespace PopLibrary.SqlModels
{
    public class GetAuctionBidResult
    {
        public int Id { get; set; }

        public int AuctionId { get; set; }

        public decimal Amount { get; set; }

        public string Email { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
