using PopApis.ApiControllers;
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
        private AccountingController _accountingController;

        public AccountingRepository(AuctionController auctionController, AccountingController accountingController) {
            _auctionController = auctionController;
            _accountingController = accountingController;
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public decimal getTotalBidAmount(DateTime startDate, DateTime endDate)
        {
            var allIds = _accountingController.GetAllBidAuctionIDs(startDate, endDate);
            List<decimal> highestBidAmounts = new();
            highestBidAmounts.AddRange(allIds.Select(i => _auctionController.GetHighestBidOnAuction(i).FirstOrDefault().Amount));
            return highestBidAmounts.Sum();
        }

        /// <inheritdoc/>
        public decimal getTotalDonationAmount(DateTime startDate, DateTime endDate)
        {
            var allDonationAmounts = _accountingController.GetAllDonationAmounts(startDate, endDate);
            return allDonationAmounts.Sum();            
        }

        /// <inheritdoc/>
        public decimal getTotal(DateTime startDate, DateTime endDate)
        {
            return this.getTotalBidAmount(startDate, endDate) + this.getTotalDonationAmount(startDate, endDate);
        }
    }
}
