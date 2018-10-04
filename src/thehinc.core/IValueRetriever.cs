using System.Collections.Generic;
using System.Threading.Tasks;
using thehinc.core.Data;

namespace thehinc.core
{
    /// <summary>
    /// Contract for an object to retrieve a value from multiple possible locations
    /// </summary>
    public interface IValueRetriever {
        /// <summary>
        /// Gets value from a heirarchy of storage locations
        /// </summary>
        /// <param name="locations"></param>
        /// <returns></returns>
        string GetValueMulti (Dictionary<ValueStorageType, string> locations);

        /// <summary>
        /// Gets value from a single of storage location
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        string GetValue (ValueStorageType storagelocation, string name);

        /// <summary>
        /// gets a collection of values given a string name
        /// </summary>
        /// <param name="storagelocation">the storage locatrion, must bee ssm unencrypted or encryted</param>
        /// <param name="name">The prepended key value</param>
        /// <returns>a collection of Name,Value Pairs</returns>
        Dictionary<string, string> GetValues (ValueStorageType storagelocation, string name);

        /// <summary>
        /// Gets value from a heirarchy of storage locations
        /// </summary>
        /// <param name="locations"></param>
        /// <returns></returns>
        Task<string> GetValueMultiAsync (Dictionary<ValueStorageType, string> locations);

        /// <summary>
        /// Gets value from a single of storage location
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        Task<string> GetValueAsync (ValueStorageType storagelocation, string name);

        /// <summary>
        /// gets a collection of values given a string name
        /// </summary>
        /// <param name="storagelocation">the storage locatrion, must bee ssm unencrypted or encryted</param>
        /// <param name="name">The prepended key value</param>
        /// <returns>a collection of Name,Value Pairs</returns>

        Task<Dictionary<string, string>> GetValuesAsync (ValueStorageType storagelocation, string name);
    }
}