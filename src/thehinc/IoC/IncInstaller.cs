using Amazon.SimpleSystemsManagement;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using thehinc.Cache;
using thehinc.Config;
using thehinc.core;

namespace thehinc.IoC
{
    public class IncInstaller : IWindsorInstaller
    {

        /// <summary>
        /// Registers the contracts with their implementations
        /// </summary>
        /// <param name="container"></param>
        /// <param name="store"></param>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {

            //Cache
            container.Register (Component.For<ICache> ().ImplementedBy<RedisCache> ());

            //Data
            container.Register (Component.For<IValueRetriever> ().ImplementedBy<StorageValueRetriever> ());
            
            //AWS
            container.Register (Component.For<IAmazonSimpleSystemsManagement> ().ImplementedBy<AmazonSimpleSystemsManagementClient> ());
        }

    }
}