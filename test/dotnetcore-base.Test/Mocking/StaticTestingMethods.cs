using System;
using Castle.Windsor;
using NSubstitute;
using thehinc.ioc;

namespace dotnetcore_base.Test.Mocking
{

    public class StaticTestingMethods
    {
        public static void IoCEnableMocking()
        {
            var container = Substitute.For<IWindsorContainer>();
            container.Resolve(Arg.Any<Type>()).Returns((x) => Substitute.For(new[] { x.ArgAt<Type>(0) }, null));
            container.Resolve(Arg.Any<string>(), Arg.Any<Type>()).Returns((x) => Substitute.For(new[] { x.ArgAt<Type>(1) }, null));
            IoCContainer.Container = container;
        }
    }
}