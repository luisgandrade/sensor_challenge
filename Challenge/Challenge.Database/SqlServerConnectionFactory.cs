using System.Data;
using System.Data.SqlClient;

namespace Challenge.Database
{
    public static class SqlServerConnectionFactory
    {

        public static IDbConnection GetConnection(string connectionString) => new SqlConnection(connectionString);
    }
}
