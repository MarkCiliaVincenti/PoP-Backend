using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopLibrary.SqlModels
{
    public class AuctionBid
    {
        public int? Id { get; set; }

        public int? AuctionId { get; set; }

        public decimal Amount { get; set; }

        public string Email { get; set; }

        public DateTime? Timestamp { get; set; }

    }
}

