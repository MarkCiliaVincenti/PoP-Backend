using System.Collections.Generic;
using System.Data;
using PopLibrary.SqlModels;

namespace PopLibrary.Data
{
    public class EventData
    {

        private SqlAdapter _sqlAdapter;
        public EventData(SqlAdapter sqlAdapter)
        {
            _sqlAdapter = sqlAdapter;
        }
        public IEnumerable<Event> GetEvents()
        {
            return _sqlAdapter.ExecuteStoredProcedureAsync<Event>("dbo.GetEvents");
        }

        public void AddOrUpdateEvent(Event itemToUpdate)
        {
            var parameters = new List<StoredProcedureParameter>();
            if (itemToUpdate.Id != null)
            {
                parameters.Add(new StoredProcedureParameter { Name = "@EventId", DbType = SqlDbType.Int, Value = itemToUpdate.Id });
            }
            parameters.AddRange(new List<StoredProcedureParameter>{
                    new StoredProcedureParameter { Name = "@Name", DbType = SqlDbType.NVarChar, Value = itemToUpdate.Name },
                    new StoredProcedureParameter { Name = "@Description", DbType = SqlDbType.NVarChar, Value = itemToUpdate.Description },
                    new StoredProcedureParameter { Name = "@StartDate", DbType = SqlDbType.DateTime, Value = itemToUpdate.StartDate },
                    new StoredProcedureParameter { Name = "@EndDate", DbType = SqlDbType.DateTime, Value = itemToUpdate.EndDate },
                    new StoredProcedureParameter { Name = "@Goal", DbType = SqlDbType.Decimal, Value = itemToUpdate.Goal },
                    new StoredProcedureParameter { Name = "@Type", DbType = SqlDbType.NVarChar, Value = itemToUpdate.Type },
                    new StoredProcedureParameter { Name = "@BaseAmount", DbType = SqlDbType.Decimal, Value = itemToUpdate.BaseAmount },
                    new StoredProcedureParameter { Name = "@Venue", DbType = SqlDbType.NVarChar, Value = itemToUpdate.Venue },
                    new StoredProcedureParameter { Name = "@IsActive", DbType = SqlDbType.Bit, Value = itemToUpdate.IsActive },
                    new StoredProcedureParameter { Name = "@Created", DbType = SqlDbType.DateTime, Value = itemToUpdate.Created }
                    }
            );

            _sqlAdapter.ExecuteStoredProcedureAsync<int>("dbo.AddOrUpdateEvent", parameters);
        }
    }
}
