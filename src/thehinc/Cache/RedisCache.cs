using System;
using Newtonsoft.Json;
using StackExchange.Redis;
using thehinc.core;

namespace thehinc.Cache
{

    public class RedisCache : ICache
    {
        #region Constants        
        /// The identifier type separator
        /// </summary>
        private readonly string _idTypeSeparator = "_";

        /// <summary>
        /// Gets the TTL in minutes from the config file with a default of 15 min
        /// </summary>
        /// <value>
        /// The TTL in minutes
        /// </value>
        public static int MinutesTtl => 15;
        /// <summary>
        /// The default TTL
        /// </summary>
        private readonly TimeSpan _defaultTtl = TimeSpan.FromMinutes(MinutesTtl);
        private readonly int _databaseNumber = 1;
        #endregion Constants

        /// <summary>
        /// The cache
        /// </summary>
        public IDatabase Cache { get; set; }

        public RedisCache(string Endpoint)
        {
            Cache = ConnectionMultiplexer.Connect(Endpoint).GetDatabase(_databaseNumber);

        }

        public T Get<T>(string id) where T : class, new()
        {

            return Cache.KeyExists(id) ? JsonConvert.DeserializeObject<T>(Cache.StringGet(id)) : null;
        }

        public object Get(string id, string propertyName)
        {
            return Cache.KeyExists(getPropertyId(id, propertyName)) ? JsonConvert.DeserializeObject(Cache.StringGet(getPropertyId(id, propertyName))) : null;
        }

        public T Get<T>(string id, string propertyName) where T : class, new()
        {
            return Cache.KeyExists(getPropertyId(id, propertyName)) ? JsonConvert.DeserializeObject<T>(Cache.StringGet(getPropertyId(id, propertyName))) : null;
        }

        public bool HasAnyValue(string id)
        {
            return Cache.KeyExists(id);
        }

        public void Set<T>(string id, T value, TimeSpan? ttl = null, bool overwrite = true) where T : class, new()
        {
            if (!ttl.HasValue)
                ttl = _defaultTtl;
            Cache.StringSet(id, JsonConvert.SerializeObject(value), ttl, overwrite ? When.Always : When.NotExists);
        }

        public void Set(string id, object value, TimeSpan? ttl = null, bool overwrite = true)
        {
            if (!ttl.HasValue)
                ttl = _defaultTtl;
            Cache.StringSet(id, JsonConvert.SerializeObject(value), ttl, overwrite ? When.Always : When.NotExists);
        }

        public void Set(string id, string propertyName, object value, TimeSpan? ttl = null, bool overwrite = true)
        {
            if (!ttl.HasValue)
                ttl = _defaultTtl;
            Cache.StringSet(getPropertyId(id, propertyName),
                JsonConvert.SerializeObject(value), ttl, overwrite ? When.Always : When.NotExists);
        }


        /// <summary>
        /// creates a property ID from an ID and a property name
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        private string getPropertyId(string id, string propertyName)
        {
            return $"{id}{_idTypeSeparator}{propertyName}";
        }

    }
}