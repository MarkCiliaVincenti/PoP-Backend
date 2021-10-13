using Microsoft.AspNetCore.Mvc;
using PopApis.Models;
using PopLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace PopApis.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountingController : ControllerBase
    {
        private SqlAdapter _sqlAdapter;

        AuctionViewModel[] auctions = new AuctionViewModel[]{
         new AuctionViewModel { AuctionType = AuctionType.Live, AuctionTime = DateTime.Now, AuctionName = "testAuction1", HighestBid =
             new BidViewModel { BidId = 100, BidAmount = 200000, GuestId = 23498, PaidStatus = false}},
         new AuctionViewModel { AuctionType = AuctionType.Silent, AuctionTime = DateTime.Now, AuctionName = "testAuction2", HighestBid =
             new BidViewModel { BidId = 234, BidAmount = 20, GuestId = 25345, PaidStatus = true}}};

        public AccountingController(SqlAdapter sqlAdapter)
        {
            _sqlAdapter = sqlAdapter;
        }

        public IEnumerable<AuctionViewModel> GetAllAuctions()
        {
            return auctions;
        }
        
        // GET: api/<AccountingController>/nameExample
        [HttpGet]
        public IActionResult GetAuction(string auctionName)
        {
            var auction = auctions.FirstOrDefault((a) => a.AuctionName == auctionName);
            return auction == null ? NotFound() : Ok(auction);
        }



        /*        // GET: api/<AccountingController>
                [HttpGet]
                public IEnumerable<string> Get()
                {
                    return new string[] { "value1", "value2" };
                }

                // GET api/<AccountingController>/5
                [HttpGet("{id}")]
                public string Get(int id)
                {
                    return "value";
                }

                // POST api/<AccountingController>
                [HttpPost]
                public void Post([FromBody] string value)
                {
                }

                // PUT api/<AccountingController>/5
                [HttpPut("{id}")]
                public void Put(int id, [FromBody] string value)
                {
                }

                // DELETE api/<AccountingController>/5
                [HttpDelete("{id}")]
                public void Delete(int id)
                {
                }
        */
    }
}
