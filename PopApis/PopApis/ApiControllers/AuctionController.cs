using AsyncKeyedLock;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PopApis.Models;
using PopLibrary;
using PopLibrary.SqlModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PopApis
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "User")]
    public class AuctionController : ControllerBase
    {
        private static AsyncKeyedLocker<int> _asyncKeyedLocker = new AsyncKeyedLocker<int>(o =>
        {
            o.PoolSize = 20;
            o.PoolInitialFill = 1;
        });
        private SqlAdapter _sqlAdapter;
        public AuctionController(SqlAdapter sqlAdapter)
        {
            _sqlAdapter = sqlAdapter;
        }

        // GET: api/Auction
        /// <summary>
        /// Gets all auctions.
        /// </summary>
        [HttpGet]
        public IEnumerable<GetAuctionsResult> GetAuctions()
        {
            var results = _sqlAdapter.ExecuteStoredProcedure<GetAuctionsResult>("dbo.GetAuctions");
            return results;
        }

        // GET api/Auction/5
        /// <summary>
        /// Gets all auctions with type ID equal to <paramref name="auctionTypeId"/>.
        /// </summary>
        [HttpGet("{auctionTypeId}")]
        public IEnumerable<GetAuctionsResult> GetAuctionById(int auctionTypeId)
        {
            var result = _sqlAdapter.ExecuteStoredProcedure<GetAuctionsResult>("dbo.GetAuctions", new List<StoredProcedureParameter>
            {
                new StoredProcedureParameter { Name="@AuctionTypeId", DbType=SqlDbType.Int, Value=auctionTypeId }
            });
            return result;
        }

        // GET api/Auction/highestbidoftype/1
        /// <summary>
        /// Gets highest bid information for all auctions of type <paramref name="auctionTypeId"/>.
        /// </summary>
        [HttpGet("highestbidoftype/{auctionTypeId}")]
        public IEnumerable<GetAuctionBidResult> GetHighestBidForAllAuctionsOfType(int auctionTypeId)
        {
            var result = _sqlAdapter.ExecuteStoredProcedure<GetAuctionBidResult>("dbo.GetHighestBidForAllAuctionsOfType", new List<StoredProcedureParameter>
            {
                new StoredProcedureParameter { Name="@AuctionTypeId", DbType=SqlDbType.Int, Value=auctionTypeId }
            });
            return result;
        }

        // GET api/Auction/highestbid/2
        /// <summary>
        /// Gets highest bid information for auction with type ID equal to <paramref name="auctionId"/>.
        /// </summary>
        [HttpGet("highestbid/{auctionId}")]
        public IEnumerable<GetAuctionBidResult> GetHighestBidOnAuction(int auctionId)
        {
            return GetHighestBidByAuctionId(auctionId);
        }

        // POST api/Auction/bid/1
        [HttpPost]
        [Route("bid/{auctionId}")]
        public async Task<ActionResult<string>> Bid([FromBody] Bid request, int auctionId)
        {
            if (request.Amount <= 0)
            {
                return BadRequest("The amount should be at least $1.");
            }

            if (auctionId <= 0)
            {
                return BadRequest("The auction Id must be a valid one.");
            }

            if (string.IsNullOrWhiteSpace(request.Email))
            {
                return BadRequest("The customer email should be valid.");
            }

            // This is used to avoid race condition when 2 users are bidding the some auction id at the same time
            using (await _asyncKeyedLocker.LockAsync(auctionId).ConfigureAwait(false))
            {
                try
                {
                    var highestBids = GetHighestBidByAuctionId(auctionId).ToList();

                    if (highestBids.Count > 0)
                    {
                        var highestBidAmout = highestBids.First().Amount;
                        if (request.Amount <= highestBidAmout)
                        {
                            return Conflict($"Your bid amount should be greater than ${highestBidAmout}.");
                        }
                    }

                    _sqlAdapter.ExecuteStoredProcedure<GetAuctionBidResult>("dbo.AddOrUpdateAuctionBid", new List<StoredProcedureParameter>
                    {
                        new StoredProcedureParameter { Name="@AuctionId", DbType=SqlDbType.Int, Value=auctionId },
                        new StoredProcedureParameter { Name="@Amount", DbType=SqlDbType.Decimal, Value=request.Amount },
                        new StoredProcedureParameter { Name="@Email", DbType=SqlDbType.NVarChar, Value=request.Email },
                    });

                    return Ok($"You successfully bid ${request.Amount} for auction Id {auctionId}");
                }
                catch (Exception ex)
                {
                    var errorMessage = "Error calling Bid API";
                    if (ex.Message.Contains("conflicted with the FOREIGN KEY", StringComparison.OrdinalIgnoreCase))
                    {
                        return BadRequest($"{errorMessage}: There's no auction Id found. Please make sure the auction Id exist.");
                    }

                    return StatusCode(StatusCodes.Status500InternalServerError, $"{errorMessage}: {ex.Message}");
                }
            }
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