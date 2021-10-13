using System.Data;

namespace PopLibrary
{
    public class StoredProcedureParameter
    {
        public string Name { get; set; }

        public SqlDbType DbType { get; set; }

        public object Value { get; set; }
    }
}
