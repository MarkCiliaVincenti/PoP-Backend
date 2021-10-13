using PopApis.Data.Interfaces;
using PopApis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace PopApis.Data
{
    /// <summary>
    /// Implementation of <see cref="IAccountingRepository"/>
    /// </summary>
    public class AccountingRepository : IAccountingRepository
    {
        private AuctionController _auctionController;

        public AccountingRepository(AuctionController auctionController) {
            _auctionController = auctionController;
        }

        public AuctionViewModel getAuctionInfoById(int auctionID)
        {
            var auctionDetails = _auctionController.GetAuctionById(auctionID);
            var auctionHighestBid = _auctionController.GetHighestBidOnAuction(auctionID);
            return new AuctionViewModel
            {
                AuctionId = auctionID,
                AuctionName = auctionDetails.FirstOrDefault().Title,
                AuctionTime = auctionDetails.FirstOrDefault().Created,
                AuctionType = auctionDetails.FirstOrDefault().AuctionTypeId,
                HighestBid = new BidViewModel
                {
                    BidAmount = auctionHighestBid.FirstOrDefault().Amount,
                    BidId = auctionHighestBid.FirstOrDefault().Id,
                    GuestId = auctionHighestBid.FirstOrDefault().Email,
                    PaidStatus = auctionHighestBid.FirstOrDefault().Amount <= 100
                }
            };
        }
    }
}
