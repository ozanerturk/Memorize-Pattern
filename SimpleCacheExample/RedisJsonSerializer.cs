using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using StackExchange.Redis;
using System.Drawing;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SimpleCacheExample
{
    public class RedisJsonSerializer
    {
       
        private readonly JsonSerializerSettings settings;

        public RedisJsonSerializer()
        {
            this.settings = new JsonSerializerSettings();
            settings.TypeNameHandling = TypeNameHandling.All;
        }

        public RedisValue Serialize(object value)
        {
            if (value == null) return RedisValue.Null;

            var result = JsonConvert.SerializeObject(value, Formatting.None, settings);
            return result;
        }

        public object Deserialize(RedisValue value)
        {
            if (value.IsNull) return null;

            var result = JsonConvert.DeserializeObject(value, settings);
            return result;
        }
    }
}
