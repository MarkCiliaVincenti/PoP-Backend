using Microsoft.AspNetCore.Mvc;
using PopApis.Models;
using System;
using System.Collections.Generic;
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
        public AccountingController(FinalizeOptions finalizeOptions)
        {
            _finalizeOptions = finalizeOptions;
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
            if (key != _finalizeOptions.FinalizeKey)
            {
                return "Bad key";
            }
            return "Finalize operation successful";
        }
    }
}
