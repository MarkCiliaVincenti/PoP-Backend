using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace PopLibrary
{
    public class SqlAdapter
    {
        private SqlSettings _sqlSettings;
        public SqlAdapter(SqlSettings sqlSettings)
        {
            _sqlSettings = sqlSettings;
        }

        public List<T> ExecuteStoredProcedureAsync<T>(
            string procedureName,
            IReadOnlyCollection<(string name, SqlDbType sqlDbType, object value)> parameters = null)
        {
            var ret = new List<T>();

            using (SqlConnection con = new SqlConnection(_sqlSettings.PopDbConnectionString))
            {
                SqlCommand cmd = new SqlCommand(procedureName, con);
                if (parameters != null && parameters.Any())
                {
                    parameters.Select(param => cmd.Parameters.Add(param.name, param.sqlDbType).Value = param.value);
                }
                cmd.CommandType = CommandType.StoredProcedure;

                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    ret = JsonConvert.DeserializeObject<List<T>>(SqlDatoToJson(rdr));
                }
            }

            return ret;
        }

        private string SqlDatoToJson(SqlDataReader dataReader)
        {
            var dataTable = new DataTable();
            dataTable.Load(dataReader);
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(dataTable);
            return JSONString;
        }
    }
}
