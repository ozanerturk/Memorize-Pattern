using StackExchange.Redis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace SimpleCacheExample
{
    public static class CacheSystem
    {

        public static Hashtable Cache = new Hashtable();//could be replace with redis :)

        public static IEnumerable<TSource> Cacheble<TSource>(this IQueryable<TSource> source)
        {
            Redis redis = new Redis();
            var db = redis.GetDb(1);
           
            RedisJsonSerializer serialize = new RedisJsonSerializer();

            var key = source.ToString();

            var redisValue =db.StringGet(key);
            if (!string.IsNullOrEmpty(redisValue))
            {
                Console.WriteLine("Cache Hit");
                var result = serialize.Deserialize(redisValue);
                return ((IEnumerable<TSource>)result).AsEnumerable();
            }
            else
            {
                Console.WriteLine("Cache Miss");
                Console.WriteLine("Caching...");
                var result = source.AsEnumerable();
                redisValue = serialize.Serialize(result);
                db.StringSet( key, redisValue);
                return result;
            }
        }

    }
}
