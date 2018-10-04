using System;

namespace thehinc.core
{
    /// <summary>
    /// Contract for a cache
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        T Get<T>(string id) where T : class, new();

        /// <summary>
        /// Gets the specified property for identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        object Get(string id, string propertyName);

        /// <summary>
        /// Gets the property for the specified identifier.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">The identifier.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        T Get<T>(string id, string propertyName) where T : class, new();

        /// <summary>
        /// Sets the value for the specified ID and Type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">The identifier.</param>
        /// <param name="value">The value.</param>
        /// <param name="ttl">The Time to live for a an object.</param>
        /// <param name="overwrite">if set to <c>true</c> will overwrite current values in the cache.</param>
        /// <returns>true if it sets a value, false otherwise</returns>
        void Set<T>(string id, T value, TimeSpan? ttl = null, bool overwrite = true) where T : class, new();

        /// <summary>
        /// Sets the value for the specified ID and Type
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="value">The value.</param>
        /// <param name="ttl">The TTL.</param>
        /// <param name="overwrite">if set to <c>true</c> will overwrite current values in the cache.</param>
        /// <returns>true if it sets a value, false otherwise</returns>
        void Set(string id, object value, TimeSpan? ttl = null, bool overwrite = true);

        /// <summary>
        /// Sets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        /// <param name="ttl">The TTL.</param>
        /// <param name="overwrite">if set to <c>true</c> [overwrite].</param>
        /// <returns></returns>
        void Set(string id, string propertyName, object value, TimeSpan? ttl = null, bool overwrite = true);

        /// <summary>
        /// Determines whether [has any value] [the specified identifier].
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        ///   <c>true</c> if [has any value] [the specified identifier]; otherwise, <c>false</c>.
        /// </returns>
        bool HasAnyValue(string id);
    }
}