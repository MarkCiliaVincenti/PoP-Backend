using Microsoft.AspNetCore.Mvc;
using PopApis.Models;
using PopLibrary;
using PopLibrary.Helpers;
using PopLibrary.SqlModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PopApis.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountingController : ControllerBase
    {
        private readonly FinalizeOptions _finalizeOptions;
        private readonly SqlAdapter _sqlAdapter;
        private readonly FinalizeHelper _finalizeHelper;

        public AccountingController(
            FinalizeOptions finalizeOptions,
            SqlAdapter sqlAdapter,
            FinalizeHelper finalizeHelper)
        {
            _finalizeOptions = finalizeOptions;
            _sqlAdapter = sqlAdapter;
            _finalizeHelper = finalizeHelper;
        }

        // GET: api/<AccountingController>
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

        // POST api/<AccountingController>
        [HttpPost("finalize")]
        public string PostFinalize([FromBody] string key)
        {
            // TODO: encrypt
            if (key != _finalizeOptions.FinalizeKey)
            {
                return "Bad key";
            }
            // 1. All outstanding donations already in Payment table
            // 2. Ingest silent auction highest bidders to Payment table
            _finalizeHelper.IngestAuctionResultsToPaymentTable((int)AuctionType.Silent);
            // 3. Ingest silent auction highest bidders to Payment table
            _finalizeHelper.IngestAuctionResultsToPaymentTable((int)AuctionType.Live);
            return "Finalize operation successful";
        }
    }
}
