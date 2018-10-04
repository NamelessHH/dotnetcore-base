using System;
using System.Reflection;
using Castle.Windsor;

namespace thehinc.ioc
{
    public static class IoCContainer
    {
        /// <summary>
        /// Main Container to be used in a resolving attribute
        /// </summary>
        public static IWindsorContainer Container
        {
            get
            {
                return _container ?? (_container = new WindsorContainer());
            }
            set
            {
                _container = value;
            }
        }
        private static IWindsorContainer _container;

        /// <summary>
        /// Method to be called in Constructor so I don'\t have to change the signature everytime I need a new object somewhere
        /// </summary>
        /// <param name="myObject"></param>
        /// <typeparam name="T"></typeparam>
        public static void InjectProperties<T>(this T myObject)
        {
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                var dependencyAttribute = property.GetDependencyAttibute();
                if (dependencyAttribute != null && property.GetValue(myObject) == null)
                {
                    if (dependencyAttribute.OverrideName == null && dependencyAttribute.ConstructorDictionary == null)
                    {
                        property.SetValue(myObject, Container.Resolve(property.PropertyType));
                    }
                    else if (dependencyAttribute.ConstructorDictionary == null) //We have only an OverrideName
                        property.SetValue(myObject, Container.Resolve(dependencyAttribute.OverrideName, property.PropertyType));
                    else if (dependencyAttribute.OverrideName == null) //We have only a constructor Doctionary
                        property.SetValue(myObject, Container.Resolve(property.PropertyType, dependencyAttribute.ConstructorDictionary));
                    else //We have both
                        property.SetValue(myObject, Container.Resolve(dependencyAttribute.OverrideName, property.PropertyType, dependencyAttribute.ConstructorDictionary));

                }

            }
        }
        /// <summary>
        /// Method for seeing a property has the Require Injection Attribute
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        private static RequiresInjectionAttribute GetDependencyAttibute(this PropertyInfo property)
        {
            object[] attrs = property.GetCustomAttributes(true);
            foreach (object attr in attrs)
            {
                RequiresInjectionAttribute authAttr = attr as RequiresInjectionAttribute;
                if (authAttr != null)
                    return authAttr;
            }
            return null;
        }
    }
}