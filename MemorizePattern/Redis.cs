using System;
using StackExchange.Redis;
using System.Threading;

namespace SimpleCacheExample
{
    public class Redis 
    {
        private readonly ConfigurationOptions _configurationOptions;
        private IDatabase _db;
        private ConnectionMultiplexer _redisConnection;

        public Redis()
        {
            _configurationOptions = new ConfigurationOptions();
            _configurationOptions.EndPoints.Add("localhost",6379);
            _configurationOptions.ConnectRetry = 1;
            _configurationOptions.ConnectTimeout = 500;


        }

        public IDatabase GetDb(int storeId)
        {
            if (_redisConnection == null || !_redisConnection.IsConnected)
                _redisConnection = ConnectionMultiplexer.Connect(_configurationOptions);

            _db = _redisConnection.GetDatabase(storeId);
            
            return _db;
        }
    }
}
