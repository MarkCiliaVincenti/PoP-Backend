using PopLibrary.SqlModels;
using System.Collections.Generic;
using System.Data;

namespace PopLibrary.Data
{
    public class AuctionData
    {
        private SqlAdapter _sqlAdapter;

        public AuctionData(SqlAdapter sqlAdapter)
        {
            _sqlAdapter = sqlAdapter;
        }

        public IEnumerable<Auction> GetAuctions()
        {
            return _sqlAdapter.ExecuteStoredProcedure<Auction>("dbo.GetAuctions");
        }

        public void AddOrUpdateAuction(Auction itemToUpdate)
        {
            var parameters = new List<StoredProcedureParameter>();
            if (itemToUpdate.Id != null)
            {
                parameters.Add(new StoredProcedureParameter { Name = "@AuctionId", DbType = SqlDbType.Int, Value = itemToUpdate.Id });
            }

            parameters.AddRange(new List<StoredProcedureParameter>{
                    new StoredProcedureParameter { Name = "@AuctionTypeId", DbType = SqlDbType.Int, Value = itemToUpdate.AuctionTypeId},
                    new StoredProcedureParameter { Name = "@Title", DbType = SqlDbType.NVarChar, Value = itemToUpdate.Title},
                    new StoredProcedureParameter { Name = "@Description", DbType = SqlDbType.NVarChar, Value = itemToUpdate.Description },
                    new StoredProcedureParameter { Name = "@Restrictions", DbType = SqlDbType.NVarChar, Value = itemToUpdate.Restrictions },
                    new StoredProcedureParameter { Name = "@IsActive", DbType = SqlDbType.Bit, Value = itemToUpdate.IsActive },
                    new StoredProcedureParameter { Name = "@Amount", DbType = SqlDbType.Decimal, Value = itemToUpdate.Amount },                    
                    new StoredProcedureParameter { Name = "@Created", DbType = SqlDbType.DateTime, Value = itemToUpdate.Created },
                    new StoredProcedureParameter { Name = "@ImageUrl", DbType = SqlDbType.NVarChar, Value = itemToUpdate.ImageUrl },
                }
            );

            _sqlAdapter.ExecuteStoredProcedure<int>("dbo.AddOrUpdateAuction", parameters);
        }

        public void DeleteAuction(int auctionId)
        {
            var parameters = new List<StoredProcedureParameter>() {
                new StoredProcedureParameter { Name = "@AuctionId", DbType = SqlDbType.Int, Value = auctionId }
            };

            _sqlAdapter.ExecuteStoredProcedure<int>("dbo.DeleteAuction", parameters);
        }
    }
}
