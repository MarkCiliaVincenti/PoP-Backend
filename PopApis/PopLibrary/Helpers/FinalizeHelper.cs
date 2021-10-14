using PopLibrary.SqlModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopLibrary.Helpers
{
    public class FinalizeHelper
    {
        private readonly SqlAdapter _sqlAdapter;
        public FinalizeHelper(SqlAdapter sqlAdapter)
        {
            _sqlAdapter = sqlAdapter;
        }

        public void IngestAuctionResultsToPaymentTable(int auctionTypeId)
        {
            var auctions = _sqlAdapter.ExecuteStoredProcedure<GetAuctionsResult>("dbo.GetAuctions");
            var auctionResults = _sqlAdapter.ExecuteStoredProcedure<GetAuctionBidResult>("dbo.GetHighestBidForAllAuctionsOfType", new List<StoredProcedureParameter>
            {
                new StoredProcedureParameter { Name="@AuctionTypeId", DbType=SqlDbType.Int, Value=auctionTypeId }
            });
            foreach (var auctionResult in auctionResults)
            {
                // Check customers table, if no customer, create one
                var customer = _sqlAdapter.ExecuteStoredProcedure<Customer>("dbo.AddOrUpdateCustomer", new List<StoredProcedureParameter>
                {
                    new StoredProcedureParameter { Name="@Email", DbType=SqlDbType.NVarChar, Value=auctionResult.Email }
                }).FirstOrDefault();
                var auctionDescription = auctions
                    .Where(auction => auctionResult.AuctionId == auction.Id).
                    Select(a => a.Description)
                    .FirstOrDefault();
                _sqlAdapter.ExecuteStoredProcedure("dbo.AddOrUpdatePayment", new List<StoredProcedureParameter>
                {
                    new StoredProcedureParameter { Name="@AuctionId", DbType=SqlDbType.Int, Value=auctionResult.AuctionId },
                    new StoredProcedureParameter { Name="@CustomerId", DbType=SqlDbType.Int, Value=customer.Id },
                    new StoredProcedureParameter { Name="@Complete", DbType=SqlDbType.Bit, Value=0 },
                    new StoredProcedureParameter { Name="@Amount", DbType=SqlDbType.Decimal, Value=auctionResult.Amount },
                    new StoredProcedureParameter { Name="@Description", DbType=SqlDbType.NVarChar, Value=auctionDescription }
                });
            }
        }
    }
}
