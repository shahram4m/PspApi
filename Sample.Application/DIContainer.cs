namespace Sample.Application
{
    using System;
    using Microsoft.Practices.Unity;

    /// <summary>
    /// IoC Container.
    /// </summary>
    public class DIContainer
    {
        private static IUnityContainer defaultInstance;

        /// <summary>
        /// Gets the IoC container.
        /// </summary>
        public static IUnityContainer Default
        {
            get { return defaultInstance ?? (defaultInstance = new UnityContainer()); }
        }
    }
}