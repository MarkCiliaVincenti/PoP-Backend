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

        public List<T> ExecuteStoredProcedure<T>(
            string procedureName,
            IReadOnlyCollection<StoredProcedureParameter> parameters = null)
        {
            var ret = new List<T>();

            using (SqlConnection con = new SqlConnection(_sqlSettings.PopDbConnectionString))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand(procedureName, con);
                if (parameters != null && parameters.Any())
                {
                    foreach (var param in parameters)
                    {
                        cmd.Parameters.Add(param.Name, param.DbType).Value = param.Value;
                    }
                }
                cmd.CommandType = CommandType.StoredProcedure;

                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    ret = JsonConvert.DeserializeObject<List<T>>(SqlDatoToJson(rdr));
                }
            }

            return ret;
        }

        public void ExecuteStoredProcedure(
            string procedureName,
            IReadOnlyCollection<StoredProcedureParameter> parameters = null)
        {
            using (SqlConnection con = new SqlConnection(_sqlSettings.PopDbConnectionString))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand(procedureName, con);
                if (parameters != null && parameters.Any())
                {
                    foreach (var param in parameters)
                    {
                        cmd.Parameters.Add(param.Name, param.DbType).Value = param.Value;
                    }
                }
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader rdr = cmd.ExecuteReader();
            }

            return;
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
