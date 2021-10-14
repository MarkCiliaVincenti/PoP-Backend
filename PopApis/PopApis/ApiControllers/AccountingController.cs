using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PopApis.Models;
using PopLibrary;
using PopLibrary.SqlModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace PopApis.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Admin")]
    public class AccountingController : ControllerBase
    {
        private SqlAdapter _sqlAdapter;
        public AccountingController(SqlAdapter sqlAdapter)
        {
            _sqlAdapter = sqlAdapter;
        }

        // GET api/accounting/info/1
        /// <summary>
        /// Returns accounting details for an auction.
        /// Gets information for auction with ID <paramref name="auctionID"/>.
        /// </summary>
        [HttpGet("info")]
        public AuctionViewModel getAuctionInfoById(int auctionID)
        {
            var auctionDetails = this.GetAuctionById(auctionID);
            var auctionHighestBid = this.GetHighestBidByAuctionId(auctionID);
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

        // GET api/accounting/bids
        // GET api/accounting/bids?startDate=10/13/2021&endDate=10/14/2021
        /// <summary>
        /// Returns the total amount from silent and live auctions.
        /// Returns total of all highest bids between dates <paramref name="startDate"/> and <paramref name="endDate"/>.
        /// </summary>
        [HttpGet("bids")]
        public decimal getTotalBidAmount([FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            var allIds = this.GetAllBidAuctionIDs(startDate, endDate);
            List<decimal> highestBidAmounts = new();
            highestBidAmounts.AddRange(allIds.Select(i => this.GetHighestBidByAuctionId(i.Id).FirstOrDefault().Amount));
            return highestBidAmounts.Sum();
        }

        // GET api/accounting/donations
        // GET api/accounting/donations?startDate=10/13/2021&endDate=10/14/2021
        /// <summary>
        /// Returns the total amount from all donations.
        /// Returns total of donations between dates <paramref name="startDate"/> and <paramref name="endDate"/>.
        /// </summary>
        [HttpGet("donations")]
        public decimal getTotalDonationAmount([FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            var allDonationAmounts = this.GetAllDonationAmounts(startDate, endDate);
            decimal total = 0;
            var sum = allDonationAmounts.Select(d => total + d.Amount);
            return total;
        }

        // GET api/accounting/total
        // GET api/accounting/total?startDate=10/13/2021&endDate=10/14/2021
        /// <summary>
        /// Returns the total amount from all auctions and donations.
        /// Returns total amount raised between dates <paramref name="startDate"/> and <paramref name="endDate"/>.
        /// </summary>
        [HttpGet("total")]
        public decimal getTotal([FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            return this.getTotalBidAmount(startDate, endDate) + this.getTotalDonationAmount(startDate, endDate);
        }

        private IEnumerable<GetAuctionIdResult> GetAllBidAuctionIDs([FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            var result = _sqlAdapter.ExecuteStoredProcedure<GetAuctionIdResult>("dbo.GetAllAuctionIDs", new List<StoredProcedureParameter>
            {
                new StoredProcedureParameter { Name = "@StartDate", DbType = SqlDbType.DateTime, Value = startDate },
                new StoredProcedureParameter { Name = "@EndDate", DbType = SqlDbType.DateTime, Value = endDate }
            });
            return result;
        }
    
        private IEnumerable<GetDonationAmountResult> GetAllDonationAmounts([FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            var result = _sqlAdapter.ExecuteStoredProcedure<GetDonationAmountResult>("dbo.GetAllDonationAmounts", new List<StoredProcedureParameter>
            {
                new StoredProcedureParameter { Name = "@StartDate", DbType = SqlDbType.DateTime, Value = startDate },
                new StoredProcedureParameter { Name = "@EndDate", DbType = SqlDbType.DateTime, Value = endDate }
            });
            return result;
        }

        private IEnumerable<GetAuctionsResult> GetAuctionById(int auctionTypeId)
        {
            var result = _sqlAdapter.ExecuteStoredProcedure<GetAuctionsResult>("dbo.GetAuctions", new List<StoredProcedureParameter>
            {
                new StoredProcedureParameter { Name="@AuctionTypeId", DbType=SqlDbType.Int, Value=auctionTypeId }
            });
            return result;
        }

        private IEnumerable<GetAuctionBidResult> GetHighestBidByAuctionId(int auctionId)
        {
            return _sqlAdapter.ExecuteStoredProcedure<GetAuctionBidResult>("dbo.GetHighestBid", new List<StoredProcedureParameter>
            {
                new StoredProcedureParameter { Name="@AuctionId", DbType=SqlDbType.Int, Value=auctionId }
            });
        }
    }
}
