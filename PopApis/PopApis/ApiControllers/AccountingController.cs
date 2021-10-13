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

        // total bid
        // GET api/<AccountingController>/all/
        /// <summary>
        /// Gets final bid amount of all auctions between <paramref name="startDate"/ and <paramref name="endDate">.
        /// </summary>
        [HttpGet("all/{startDate}/{endDate}")]
        public IEnumerable<decimal> GetAllAuctionAmounts(DateTime startDate, DateTime endDate)
        {
            var result = _sqlAdapter.ExecuteStoredProcedureAsync<decimal>("dbo.GetAllAuctionAmounts", new List<StoredProcedureParameter>
            {
                new StoredProcedureParameter { Name = "@StartDate", DbType = SqlDbType.DateTime, Value = startDate },
                new StoredProcedureParameter { Name = "@EndDate", DbType = SqlDbType.DateTime, Value = endDate }
            });
            return result;
        }

        // total donation
    }
}
