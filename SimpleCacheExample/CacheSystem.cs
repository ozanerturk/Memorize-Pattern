using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SimpleCacheExample
{
    public static class CacheSystem
    {
        public static Hashtable Cache = new Hashtable();//could be replace with redis :)

        public static IEnumerable<TSource> Cacheble<TSource>(this IQueryable<TSource> source)
        {
            var key = source.ToString();
            if (Cache.ContainsKey(key))
            {
                Console.WriteLine("Cache Hit");
                return ((IEnumerable<TSource>)Cache[key]).AsEnumerable();
            }
            else
            {
                Console.WriteLine("Cache Miss");
                Console.WriteLine("Caching...");
                var result = source.AsEnumerable();
                Cache.Add(key, result);
                return result;
            }
        }

    }
}
