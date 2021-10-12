using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PopApis.Models
{
    public class BidViewModel
    {
        // The Bid's ID number.
        public int BidId { get; set; }

        // The amount of money bidded.
        public double BidAmount { get; set; }

        // The ID of the guest who submitted the bid.
        public int GuestId { get; set; }

        // Whether or not the bid has been paid.
        public bool PaidStatus { get; set; }
    }
}
