using System;
using System.Linq;
using System.Runtime.Caching;
using thehinc.core;

namespace thehinc.Cache
{
    public class LocalCache : ICache
    {
        #region Constants        
        /// <summary>
        /// The cache
        /// </summary>
        private System.Runtime.Caching.MemoryCache _cache = System.Runtime.Caching.MemoryCache.Default;
        /// <summary>
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
        #endregion Constants

        #region Public Methods        
        /// <summary>
        /// Gets the object with the correct Id and Type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public T Get<T>(string id) where T : class, new()
        {
            return BuildType<T>(id);
        }

        /// <summary>
        /// Gets the specified property for identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public object Get(string id, string propertyName)
        {
            return _cache.Get(getPropertyId(id, propertyName));
        }

        /// <summary>
        /// Gets the specified property for identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public T Get<T>(string id, string propertyName) where T : class, new()
        {
            return _cache.Get(getPropertyId(id, propertyName)) as T;
        }

        /// <summary>
        /// Determines whether the specified id has any values in the casche at all
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// <c>true</c> if if the cache has any value for the id; otherwise, <c>false</c>.
        /// </returns>
        public bool HasAnyValue(string id)
        {
            return _cache.Any(x => x.Key.StartsWith($"{id}{_idTypeSeparator}"));
        }

        /// <summary>
        /// Sets the value for the specified ID and Type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">The identifier.</param>
        /// <param name="value">The value.</param>
        /// <param name="ttl">The Time to live for a an object.</param>
        /// <param name="overwrite">whether to overwrite an existing value or not</param>
        /// <returns></returns>
        public void Set<T>(string id, T value, TimeSpan? ttl = null, bool overwrite = true) where T : class, new()
        {
            if (!ttl.HasValue)
                ttl = _defaultTtl;

            SetAllProperties(id, typeof(T), value, getCacheItemPolicy(ttl.Value), overwrite);
        }

        /// <summary>
        /// Sets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        /// <param name="ttl">The TTL.</param>
        /// <param name="overwrite">if set to <c>true</c> [overwrite].</param>
        public void Set(string id, string propertyName, object value, TimeSpan? ttl = null, bool overwrite = true)
        {
            if (!ttl.HasValue)
                ttl = _defaultTtl;

            if (overwrite)
                _cache.Set(getPropertyId(id, propertyName), value, getCacheItemPolicy(ttl.Value));
            else
                _cache.Add(getPropertyId(id, propertyName), value, getCacheItemPolicy(ttl.Value));
        }
        /// <summary>
        /// Sets the value for the specified ID and Type
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="value">The value.</param>
        /// <param name="ttl">The TTL.</param>
        /// <param name="overwrite">if set to <c>true</c> will overwrite current values in the cache.</param>
        /// <returns>true if it sets a value, false otherwise</returns>
        public void Set(string id, object value, TimeSpan? ttl = null, bool overwrite = true)
        {
            if (!ttl.HasValue)
                ttl = _defaultTtl;

            SetAllProperties(id, value.GetType(), value, getCacheItemPolicy(ttl.Value), overwrite);
        }
        #endregion Public Methods

        #region Private Methods         
        /// <summary>
        /// Gets the cache item policy.
        /// </summary>
        /// <param name="ttl">The TTL.</param>
        /// <returns></returns>
        private CacheItemPolicy getCacheItemPolicy(TimeSpan ttl)
        {
            return new CacheItemPolicy()
            {
                Priority = CacheItemPriority.Default,
                SlidingExpiration = ttl
            };
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

        /// <summary>
        /// Sets all properties of an object for a specific type
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        /// <param name="ttl">The TTL.</param>
        /// <param name="overwrite">if set to <c>true</c> [overwrite].</param>
        private void SetAllProperties(string id, Type type, object value, CacheItemPolicy ttl, bool overwrite)
        {
            var properties = type.GetProperties();
            foreach (var prop in properties)
            {
                var propValue = prop.GetValue(value);
                if (propValue != null)
                    SetSingleProperty(id, prop.Name, propValue, ttl, overwrite);
            }
            var fields = type.GetFields();
            foreach (var field in fields)
            {
                var fieldValue = field.GetValue(value);
                if (fieldValue != null)
                    SetSingleProperty(id, field.Name, fieldValue, ttl, overwrite);
            }

        }

        /// <summary>
        /// Sets a single property for an Id
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        /// <param name="ttl">The TTL.</param>
        /// <param name="overwrite">if set to <c>true</c> [overwrite].</param>
        private void SetSingleProperty(string id, string propertyName, object value, CacheItemPolicy ttl, bool overwrite)
        {
            if (overwrite)
                _cache.Set(getPropertyId(id, propertyName), value, ttl);
            else
                _cache.Add(getPropertyId(id, propertyName), value, ttl);
        }

        /// <summary>
        /// Builds a type of objects for a specific ID with the values in the cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        private T BuildType<T>(string id) where T : class, new()
        {
            var type = typeof(T);
            var returnValue = new T();
            var properties = type.GetProperties();
            foreach (var prop in properties.Where(x => x.CanWrite))
            {
                var propValue = _cache.Get(getPropertyId(id, prop.Name));
                if (propValue != null && propValue.GetType() == prop.PropertyType)
                    prop.SetValue(returnValue, propValue);
            }
            var fields = type.GetFields();
            foreach (var field in fields)
            {
                var fieldValue = _cache.Get(getPropertyId(id, field.Name));
                if (fieldValue != null && fieldValue.GetType() == field.FieldType)
                    field.SetValue(returnValue, fieldValue);
            }

            return returnValue;
        }
        #endregion Private Methods


    }
}