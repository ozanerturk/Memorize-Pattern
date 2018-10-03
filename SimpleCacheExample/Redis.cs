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
            _configurationOptions.EndPoints.Add("localhost");
            _configurationOptions.ConnectRetry = 1;

            int? timeout = 5000;
            if (timeout != null) //Configte timeout değeri tanımlanmış ise o değer kullanılır. Default: 5000 ms.
                _configurationOptions.ConnectTimeout = timeout.Value;
            else
                _configurationOptions.ConnectTimeout = 500;


        }

        public IDatabase GetDb(int storeId)
        {
            //Try catch kullanım sebebi InternalExfException tipindeki hataların özel olarak loglanması ve monitor edilmesi ihtiyacı
            //Üstteki public metotlar hata fırlatabilirler, bunu handle etmek burayı çağıran yerin sorumluluğu
            try
            {
                if (_redisConnection == null || !_redisConnection.IsConnected)
                    _redisConnection = ConnectionMultiplexer.Connect(_configurationOptions);

                _db = _redisConnection.GetDatabase(storeId);
            }
            catch (Exception)
            {

            }

            return _db;
        }
    }
}