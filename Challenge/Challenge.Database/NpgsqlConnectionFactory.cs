using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Challenge.Database
{
    public static class NpgsqlConnectionFactory
    {

        public static IDbConnection GetConnection(string connectionString) => new NpgsqlConnection(connectionString);
    }
}
