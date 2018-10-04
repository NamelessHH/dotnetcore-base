using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using thehinc.data.Contracts;

namespace thehinc.data.IoC
{
    public class InventoryInstaller: IWindsorInstaller
    {

        /// <summary>
        /// Registers the contracts with their implementations
        /// </summary>
        /// <param name="container"></param>
        /// <param name="store"></param>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            //Cache
            container.Register (Component.For<IInventoryContext> ().ImplementedBy<InventoryContext> ());

        }

    }
}