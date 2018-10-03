using System;
using System.Collections.Generic;
using StackExchange.Redis;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SimpleCacheExample
{
    public class RedisJsonSerializer
    {
        // By default, JSON.NET will always use Int64/Double when deserializing numbers
        // since there isn't an easy way to detect the proper number size. However,
        // because NHibernate does casting to the correct number type, it will fail.
        // Adding the type to the serialize object is what the "TypeNameHandling.All"
        // option does except that it doesn't include numbers.
        private class KeepNumberTypesConverter : JsonConverter
        {
            // We shouldn't have to account for Nullable<T> because the serializer
            // should see them as null.
            private static readonly ISet<Type> numberTypes = new HashSet<Type>(new[]
            {
                typeof(Byte), typeof(SByte),
                typeof(UInt16), typeof(UInt32), typeof(UInt64),
                typeof(Int16), typeof(Int32), typeof(Int64),
                typeof(Single), typeof(Double), typeof(Decimal),typeof(Guid)
            });

            public override bool CanConvert(Type objectType)
            {
                return numberTypes.Contains(objectType);
            }

            // JSON.NET will deserialize a value with the explicit type when 
            // the JSON object exists with $type/$value properties. So, we 
            // don't need to implement reading.
            public override bool CanRead { get { return false; } }

            public override bool CanWrite { get { return true; } }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                // CanRead is false.
                throw new NotImplementedException();
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("$type");
                var typeName = value.GetType().FullName;
                writer.WriteValue(typeName);
                writer.WritePropertyName("$value");
                writer.WriteValue(value);
                writer.WriteEndObject();
            }
        }

        private class CustomContractResolver : DefaultContractResolver
        {


            protected override JsonObjectContract CreateObjectContract(Type objectType)
            {
                var result = base.CreateObjectContract(objectType);

                return result;
            }

            protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
            {

                return base.CreateProperties(type, memberSerialization);
            }
        }

        private readonly JsonSerializerSettings settings;

        public RedisJsonSerializer()
        {
            this.settings = new JsonSerializerSettings();
            settings.TypeNameHandling = TypeNameHandling.All;
            settings.Converters.Add(new KeepNumberTypesConverter());
            settings.ContractResolver = new CustomContractResolver();
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