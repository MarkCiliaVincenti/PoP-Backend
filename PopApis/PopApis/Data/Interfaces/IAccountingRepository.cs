using PopApis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PopApis.Data.Interfaces
{
    /// <summary>
    /// Interface for <see cref="AccountingRepository"/>
    /// </summary>
    public interface IAccountingRepository
    {
        /// <summary>
        /// Gets relevant auction and bid info given Auction ID.
        /// </summary>
        /// The <param name="auctionID"> for the auction this information is for.</param>
        /// <returns></returns>
        public AuctionViewModel getAuctionInfoById(int auctionID);

        /// <summary>
        /// Gets total amount of all silent and live auctions within a given time period.
        /// </summary>
        /// The <param name="startDate"> of the time range.</param>
        /// The <param name="endDate"> of the time range.</param>
        /// <returns></returns>
        public decimal getTotalBidAmount(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Gets total amount of all donations within a given time period.
        /// </summary>
        /// <param name="startDate"> of the time range.</param>
        /// <param name="endDate"> of the time range.</param>
        /// <returns></returns>
        public decimal getTotalDonationAmount(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Gets total amount of all auctions and donations within a given time period.
        /// </summary>
        /// <param name="startDate"> of the time range.</param>
        /// <param name="endDate"> of the time range.</param>
        public decimal getTotal(DateTime startDate, DateTime endDate);



    }
}
