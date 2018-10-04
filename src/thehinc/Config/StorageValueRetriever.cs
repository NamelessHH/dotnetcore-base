using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using thehinc.core;
using thehinc.core.Data;
using thehinc.ioc;

namespace thehinc.Config
{
    public class StorageValueRetriever : IValueRetriever {

        [RequiresInjection]
        public IAmazonSimpleSystemsManagement StoreManager { get; set; }

        public StorageValueRetriever () {
            this.InjectProperties ();
        }
        /// <summary>
        /// retruns a value based on different possible locations for the value
        /// </summary>
        /// <param name="locations">The locations.</param>
        /// <returns></returns>
        public string GetValueMulti (Dictionary<ValueStorageType, string> locations) {
            foreach (var item in locations) {
                if (!string.IsNullOrEmpty (item.Value))
                    return GetValue (item.Key, item.Value);

            }

            return null;
        }

        private GetParameterRequest _getRequestEncrypted;
        private GetParameterRequest getRequestEncrypted {
            get {
                if (_getRequestEncrypted == null) {
                    _getRequestEncrypted = new GetParameterRequest ();
                    _getRequestEncrypted.WithDecryption = true;
                }
                return _getRequestEncrypted;
            }
        }

        private GetParameterRequest _getRequestUnencrypted;
        private GetParameterRequest getRequestUnencrypted {
            get {
                if (_getRequestUnencrypted == null) {
                    _getRequestUnencrypted = new GetParameterRequest ();
                    _getRequestUnencrypted.WithDecryption = false;
                }
                return _getRequestUnencrypted;
            }
        }

        private GetParametersByPathRequest _getMultiRequestEncrypted;
        private GetParametersByPathRequest getMultiRequestEncrypted {
            get {
                if (_getMultiRequestEncrypted == null) {
                    _getMultiRequestEncrypted = new GetParametersByPathRequest ();
                    _getMultiRequestEncrypted.WithDecryption = true;
                }
                return _getMultiRequestEncrypted;
            }
        }

        private GetParametersByPathRequest _getMultiRequestUnencrypted;
        private GetParametersByPathRequest getMultiRequestUnencrypted {
            get {
                if (_getMultiRequestUnencrypted == null) {
                    _getMultiRequestUnencrypted = new GetParametersByPathRequest ();
                    _getMultiRequestUnencrypted.WithDecryption = false;
                }
                return _getMultiRequestUnencrypted;
            }
        }
        /// <summary>
        /// Gets the individual value of the specified location
        /// This should probably be abstracted out into another object
        /// </summary>
        /// <param name="populatedLocation">The populated location.</param>
        /// <returns></returns>
        public string GetValue (ValueStorageType storagelocation, string name) {
            switch (storagelocation) {
                case ValueStorageType.ActualValue:
                    return name;
                case ValueStorageType.EnvironmentVariable:
                    return Environment.GetEnvironmentVariable (name);
                case ValueStorageType.SystemManagerParameterStoreEncrypted:
                    getRequestEncrypted.Name = name;
                    return StoreManager.GetParameterAsync (getRequestEncrypted).GetAwaiter ().GetResult ().Parameter.Value;
                case ValueStorageType.SystemManagerParameterStoreUnencrypted:
                    getRequestEncrypted.Name = name;
                    return StoreManager.GetParameterAsync (getRequestUnencrypted).GetAwaiter ().GetResult ().Parameter.Value;
                default:
                    return null;
            }

        }

        /// <summary>
        /// Gets the value associated with one of the keyvalue pair locations the first one where something is found
        /// </summary>
        /// <param name="populatedLocation"></param>
        /// <returns></returns>
        public async Task<string> GetValueMultiAsync (Dictionary<ValueStorageType, string> locations) {
            string returnVal = null;
            foreach (var item in locations) {
                if (!string.IsNullOrEmpty (item.Value))
                    returnVal = await GetValueAsync (item.Key, item.Value);
                if (!string.IsNullOrEmpty (returnVal))
                    return returnVal;
            }
            return null;
        }

        /// <summary>
        /// Gets the value associated with the keyvalue pair location
        /// </summary>
        /// <param name="populatedLocation"></param>
        /// <returns></returns>
        public async Task<string> GetValueAsync (ValueStorageType storagelocation, string name) {
            switch (storagelocation) {
                case ValueStorageType.ActualValue:
                    return name;
                case ValueStorageType.EnvironmentVariable:
                    return Environment.GetEnvironmentVariable (name);
                case ValueStorageType.SystemManagerParameterStoreEncrypted:
                    getRequestEncrypted.Name = name;
                    return (await StoreManager.GetParameterAsync (getRequestEncrypted)).Parameter.Value;
                case ValueStorageType.SystemManagerParameterStoreUnencrypted:
                    getRequestEncrypted.Name = name;
                    return (await StoreManager.GetParameterAsync (getRequestUnencrypted)).Parameter.Value;
                default:
                    return null;
            }
        }

        public Dictionary<string, string> GetValues (ValueStorageType storagelocation, string name) {
            switch (storagelocation) {
                case ValueStorageType.SystemManagerParameterStoreEncrypted:
                    getMultiRequestEncrypted.Path = name;
                    return StoreManager.GetParametersByPathAsync (getMultiRequestEncrypted).GetAwaiter ().GetResult ().Parameters.ToDictionary (x => x.Name, x => x.Value);
                case ValueStorageType.SystemManagerParameterStoreUnencrypted:
                    getMultiRequestUnencrypted.Path = name;
                    return StoreManager.GetParametersByPathAsync (getMultiRequestUnencrypted).GetAwaiter ().GetResult ().Parameters.ToDictionary (x => x.Name, x => x.Value);
                default:
                    return null;
            }
        }

        public async Task<Dictionary<string, string>> GetValuesAsync (ValueStorageType storagelocation, string name) {
            switch (storagelocation) {
                case ValueStorageType.SystemManagerParameterStoreEncrypted:
                    getMultiRequestEncrypted.Path = name;
                    return (await StoreManager.GetParametersByPathAsync (getMultiRequestEncrypted)).Parameters.ToDictionary (x => x.Name, x => x.Value);
                case ValueStorageType.SystemManagerParameterStoreUnencrypted:
                    getMultiRequestUnencrypted.Path = name;
                    return (await StoreManager.GetParametersByPathAsync (getMultiRequestUnencrypted)).Parameters.ToDictionary (x => x.Name, x => x.Value);
                default:
                    return null;
            }
        }
    }
}