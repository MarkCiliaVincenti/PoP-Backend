using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PopLibrary;
using PopLibrary.SqlModels;
using System;
using System.Collections.Generic;
using System.Data;

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

        // GET api/accounting/ids
        // GET api/accounting/ids?startDate=10/13/2021&endDate=10/14/2021
        /// <summary>
        /// For calcuating totals of bids.
        /// Gets bid auction IDs of all auctions between <paramref name="startDate"/ and <paramref name="endDate">.
        /// </summary>
        [HttpGet("ids")]
        public IEnumerable<GetAuctionIdResult> GetAllBidAuctionIDs([FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            var result = _sqlAdapter.ExecuteStoredProcedure<GetAuctionIdResult>("dbo.GetAllAuctionIDs", new List<StoredProcedureParameter>
            {
                new StoredProcedureParameter { Name = "@StartDate", DbType = SqlDbType.DateTime, Value = startDate },
                new StoredProcedureParameter { Name = "@EndDate", DbType = SqlDbType.DateTime, Value = endDate }
            });
            return result;
        }

        // GET api/accounting/donations
        // GET api/accounting/donations?startDate=10/13/2021&endDate=10/14/2021
        /// <summary>
        /// For calcuating totals of donations.
        /// Gets all donation amounts between <paramref name="startDate"/ and <paramref name="endDate">.
        /// </summary>
        [HttpGet("donations")]
        public IEnumerable<GetDonationAmountResult> GetAllDonationAmounts([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var result = _sqlAdapter.ExecuteStoredProcedure<GetDonationAmountResult>("dbo.GetAllDonationAmounts", new List<StoredProcedureParameter>
            {
                new StoredProcedureParameter { Name = "@StartDate", DbType = SqlDbType.DateTime, Value = startDate },
                new StoredProcedureParameter { Name = "@EndDate", DbType = SqlDbType.DateTime, Value = endDate }
            });
            return result;
        }
    }
}
