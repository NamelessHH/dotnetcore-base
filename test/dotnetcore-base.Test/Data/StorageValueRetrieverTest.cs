using System;
using System.Collections.Generic;
using Amazon.SimpleSystemsManagement.Model;
using dotnetcore_base.Test.Mocking;
using NSubstitute;
using thehinc.Config;
using thehinc.core.Data;
using Xunit;

namespace dotnetcore_base.Test.Data
{
    public class StorageValueRetrieverTest {
        [Fact]
        public async void TestRetrieveSingleSsmEncrypted () {
            StaticTestingMethods.IoCEnableMocking ();
            var testObj = new StorageValueRetriever ();
            var expectedResponse = "expectedReturn";
            var response = new GetParameterResponse () { Parameter = new Parameter () { Value = expectedResponse } };

            var keyPair = new KeyValuePair<ValueStorageType, string> (ValueStorageType.SystemManagerParameterStoreEncrypted, "ssm/key/string");
            testObj.StoreManager.GetParameterAsync (Arg.Any<GetParameterRequest> ()).Returns (response);

            var actual = await testObj.GetValueAsync (keyPair.Key, keyPair.Value);

            Assert.Equal (expectedResponse, actual);
            await testObj.StoreManager.Received ().GetParameterAsync (Arg.Is<GetParameterRequest> (x => x.WithDecryption == true));
        }

        [Fact]
        public async void TestRetrieveSingleSsmUnencrypted () {
            StaticTestingMethods.IoCEnableMocking ();
            var testObj = new StorageValueRetriever ();
            var expectedResponse = "expectedReturn";
            var response = new GetParameterResponse () { Parameter = new Parameter () { Value = expectedResponse } };

            var keyPair = new KeyValuePair<ValueStorageType, string> (ValueStorageType.SystemManagerParameterStoreUnencrypted, "ssm/key/string");
            testObj.StoreManager.GetParameterAsync (Arg.Any<GetParameterRequest> ()).Returns (response);

            var actual = await testObj.GetValueAsync (keyPair.Key, keyPair.Value);;

            Assert.Equal (expectedResponse, actual);
            await testObj.StoreManager.Received ().GetParameterAsync (Arg.Is<GetParameterRequest> (x => x.WithDecryption == false));
        }

        [Fact]
        public async void TestRetrieveSingleEnvironment () {
            StaticTestingMethods.IoCEnableMocking ();
            var testObj = new StorageValueRetriever ();
            var envName = "TestEnvName";
            var envValue = "TestEnvValue";
            Environment.SetEnvironmentVariable (envName, envValue);

            var keyPair = new KeyValuePair<ValueStorageType, string> (ValueStorageType.EnvironmentVariable, envName);

            var actual = await testObj.GetValueAsync (keyPair.Key, keyPair.Value);

            Assert.Equal (envValue, actual);
        }

        [Fact]
        public async void TestMultiRetrievalAllHaveValues () {
            StaticTestingMethods.IoCEnableMocking ();
            var testObj = new StorageValueRetriever ();
            var envName = "TestEnvName";
            var envValue = "TestEnvValue";
            var expectedResponse = "expectedReturn";
            Environment.SetEnvironmentVariable (envName, envValue);
            var response = new GetParameterResponse () { Parameter = new Parameter () { Value = expectedResponse } };
            testObj.StoreManager.GetParameterAsync (Arg.Any<GetParameterRequest> ()).Returns (response);

            var multiKeyPair = new Dictionary<ValueStorageType, string> () { { ValueStorageType.SystemManagerParameterStoreUnencrypted, "ssm/key/string" }, { ValueStorageType.EnvironmentVariable, envName }
                };

            var actual = await testObj.GetValueMultiAsync (multiKeyPair);

            Assert.Equal (expectedResponse, actual);
            await testObj.StoreManager.Received ().GetParameterAsync (Arg.Is<GetParameterRequest> (x => x.WithDecryption == false));
        }

        [Fact]
        public async void TestMultiRetrievalWithNull () {
            StaticTestingMethods.IoCEnableMocking ();
            var testObj = new StorageValueRetriever ();
            var envName = "TestEnvName";
            var envValue = "TestEnvValue";
            Environment.SetEnvironmentVariable (envName, envValue);
            var response = new GetParameterResponse () { Parameter = new Parameter () { Value = null } };
            testObj.StoreManager.GetParameterAsync (Arg.Any<GetParameterRequest> ()).Returns (response);

            var multiKeyPair = new Dictionary<ValueStorageType, string> () { { ValueStorageType.SystemManagerParameterStoreUnencrypted, "ssm/key/string" }, { ValueStorageType.EnvironmentVariable, envName }
                };

            var actual = await testObj.GetValueMultiAsync (multiKeyPair);

            Assert.Equal (envValue, actual);
        }

        [Fact]
        public async void TestRetrieveDictionarySsmEncrypted () {
            StaticTestingMethods.IoCEnableMocking ();
            var testObj = new StorageValueRetriever ();
            var expectedName = "expectedName";
            var expectedResponseValue = "expectedResponseValue";
            var expectedResponse = new Dictionary<string, string> () { { expectedName, expectedResponseValue } };
            var response = new GetParametersByPathResponse () {
                Parameters = new List<Parameter> () {
                new Parameter () { Name = expectedName, Value = expectedResponseValue }
                }
            };

            var keyPair = new KeyValuePair<ValueStorageType, string> (ValueStorageType.SystemManagerParameterStoreEncrypted, "ssm/key/string");
            testObj.StoreManager.GetParametersByPathAsync (Arg.Any<GetParametersByPathRequest> ()).Returns (response);

            var actual = await testObj.GetValuesAsync (keyPair.Key, keyPair.Value);

            Assert.Equal (expectedResponse, actual);
            await testObj.StoreManager.Received ().GetParametersByPathAsync (Arg.Is<GetParametersByPathRequest> (x => x.WithDecryption == true));
        }

        [Fact]
        public async void TestRetrieveDictionarySsmUnencrypted () {
            StaticTestingMethods.IoCEnableMocking ();
            var testObj = new StorageValueRetriever ();
            var expectedName = "expectedName";
            var expectedResponseValue = "expectedResponseValue";
            var expectedResponse = new Dictionary<string, string> () { { expectedName, expectedResponseValue } };
            var response = new GetParametersByPathResponse () {
                Parameters = new List<Parameter> () {
                new Parameter () { Name = expectedName, Value = expectedResponseValue }
                }
            };

            var keyPair = new KeyValuePair<ValueStorageType, string> (ValueStorageType.SystemManagerParameterStoreUnencrypted, "ssm/key/string");
            testObj.StoreManager.GetParametersByPathAsync (Arg.Any<GetParametersByPathRequest> ()).Returns (response);

            var actual = await testObj.GetValuesAsync (keyPair.Key, keyPair.Value);;

            Assert.Equal (expectedResponse, actual);
            await testObj.StoreManager.Received ().GetParametersByPathAsync (Arg.Is<GetParametersByPathRequest> (x => x.WithDecryption == false));
        }
    }
}