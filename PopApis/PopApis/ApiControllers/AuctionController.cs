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
        [HttpGet]
        public IEnumerable<GetAuctionsResult> Get()
        {
            var x = _sqlAdapter.ExecuteStoredProcedureAsync<GetAuctionsResult>("dbo.GetAuctions", new List<StoredProcedureParameter>
            {
                new StoredProcedureParameter { Name="@AuctionTypeId", DbType=SqlDbType.Int, Value=1 }
            });
            return x;
        }

        // GET api/<AuctionController>/5
        [HttpGet("{auctionTypeId}")]
        public IEnumerable<GetAuctionsResult> Get(int auctionTypeId)
        {
            var x = _sqlAdapter.ExecuteStoredProcedureAsync<GetAuctionsResult>("dbo.GetAuctions", new List<StoredProcedureParameter>
            {
                new StoredProcedureParameter { Name="@AuctionTypeId", DbType=SqlDbType.Int, Value=auctionTypeId }
            });
            return x;
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
