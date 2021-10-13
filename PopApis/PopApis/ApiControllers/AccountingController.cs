using Microsoft.AspNetCore.Mvc;
using PopLibrary;
using System;
using System.Collections.Generic;
using System.Data;

namespace PopApis.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountingController : ControllerBase
    {
        private SqlAdapter _sqlAdapter;

        public AccountingController(SqlAdapter sqlAdapter)
        {
            _sqlAdapter = sqlAdapter;
        }

        // GET api/<AccountingController>/allIDs/
        /// <summary>
        /// For calcuating totals of bids.
        /// Gets bid auction IDs of all auctions between <paramref name="startDate"/ and <paramref name="endDate">.
        /// </summary>
        [HttpGet("allIDs/{startDate}/{endDate}")]
        public IEnumerable<int> GetAllBidAuctionIDs(DateTime startDate, DateTime endDate)
        {
            var result = _sqlAdapter.ExecuteStoredProcedureAsync<int>("dbo.GetAllAuctionIDs", new List<StoredProcedureParameter>
            {
                new StoredProcedureParameter { Name = "@StartDate", DbType = SqlDbType.DateTime, Value = startDate },
                new StoredProcedureParameter { Name = "@EndDate", DbType = SqlDbType.DateTime, Value = endDate }
            });
            return result;
        }

        // GET api/<AccountingController>/allDonations/
        /// <summary>
        /// For calcuating totals of donations.
        /// Gets all donation amounts between <paramref name="startDate"/ and <paramref name="endDate">.
        /// </summary>
        [HttpGet("allDonations/{startDate}/{endDate}")]
        public IEnumerable<int> GetAllDonationAmounts(DateTime startDate, DateTime endDate)
        {
            var result = _sqlAdapter.ExecuteStoredProcedureAsync<int>("dbo.GetAllDonationAmounts", new List<StoredProcedureParameter>
            {
                new StoredProcedureParameter { Name = "@StartDate", DbType = SqlDbType.DateTime, Value = startDate },
                new StoredProcedureParameter { Name = "@EndDate", DbType = SqlDbType.DateTime, Value = endDate }
            });
            return result;
        }
    }
}
