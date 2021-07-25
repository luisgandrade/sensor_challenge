using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Challenge.Services
{
    public class RedisAdapter
    {
        private ConnectionMultiplexer _connection;

        public RedisAdapter(string connection)
        {
            _connection = ConnectionMultiplexer.Connect(connection);
        }

    }
}
