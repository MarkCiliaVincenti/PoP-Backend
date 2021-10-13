using Microsoft.AspNetCore.Mvc;
using PopLibrary;
using PopLibrary.SqlModels;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PopApis
{
    [Route("api/auctioncontroller")]
    [ApiController]
    public class AuctionController : ControllerBase
    {
        private SqlAdapter _sqlAdapter;
        public AuctionController(SqlAdapter sqlAdapter)
        {
            _sqlAdapter = sqlAdapter;
        }

        // GET: api/<AuctionController>
        /// <summary>
        /// Gets all auctions.
        /// </summary>
        [HttpGet]
        public IEnumerable<GetAuctionsResult> GetAuctions()
        {
            var results = _sqlAdapter.ExecuteStoredProcedureAsync<GetAuctionsResult>("dbo.GetAuctions");
            return results;
        }

        // GET api/<AuctionController>/5
        /// <summary>
        /// Gets all auctions with type ID equal to <paramref name="auctionTypeId"/>.
        /// </summary>
        [HttpGet("{auctionTypeId}")]
        public IEnumerable<GetAuctionsResult> GetAuctionById(int auctionTypeId)
        {
            var result = _sqlAdapter.ExecuteStoredProcedureAsync<GetAuctionsResult>("dbo.GetAuctions", new List<StoredProcedureParameter>
            {
                new StoredProcedureParameter { Name="@AuctionTypeId", DbType=SqlDbType.Int, Value=auctionTypeId }
            });
            return result;
        }

        // GET api/<AuctionController>/highestbid/2
        /// <summary>
        /// Gets all auctions with type ID equal to <paramref name="auctionId"/>.
        /// </summary>
        [HttpGet("highestbid/{auctionId}")]
        public IEnumerable<GetAuctionBidResult> GetHighestBidOnAuction(int auctionId)
        {
            var result = _sqlAdapter.ExecuteStoredProcedureAsync<GetAuctionBidResult>("dbo.GetHighestBid", new List<StoredProcedureParameter>
            {
                new StoredProcedureParameter { Name="@AuctionId", DbType=SqlDbType.Int, Value=auctionId }
            });
            return result;
        }

        // POST api/<AuctionController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<AuctionController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AuctionController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
