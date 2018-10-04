namespace thehinc.core.Data
{
    /// <summary>
    /// enum for use with value retriever to roll through different storage areas to retrieve a value
    /// </summary>
    public enum ValueStorageType
    {
        ActualValue = 0,
        ConfigurationFile = 1,
        SystemManagerParameterStoreEncrypted = 2,
        EnvironmentVariable = 3,
        SystemManagerParameterStoreUnencrypted = 4
    }
}