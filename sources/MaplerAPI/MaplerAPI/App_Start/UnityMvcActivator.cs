using System.Linq;
using System.Web.Mvc;
using Microsoft.Practices.Unity.Mvc;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(MaplerAPI.App_Start.UnityWebActivator), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(MaplerAPI.App_Start.UnityWebActivator), "Shutdown")]

namespace MaplerAPI.App_Start
{
    /// <summary>Provides the bootstrapping for integrating Unity with ASP.NET MVC.</summary>
    public static class UnityWebActivator
    {
        /// <summary>Integrates Unity when the application starts.</summary>
        public static void Start() 
        {
            //var container = UnityConfig.GetConfiguredContainer();

            //FilterProviders.Providers.Remove(FilterProviders.Providers.OfType<FilterAttributeFilterProvider>().First());
            //FilterProviders.Providers.Add(new UnityFilterAttributeFilterProvider(container));

            //DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            //var defaultControllerFactory = ControllerBuilder.Current.GetControllerFactory();
            //ControllerBuilder.Current.SetControllerFactory(new UnityControllerFactory(container, defaultControllerFactory));


            // TODO: Uncomment if you want to use PerRequestLifetimeManager
            // Microsoft.Web.Infrastructure.DynamicModuleHelper.DynamicModuleUtility.RegisterModule(typeof(UnityPerRequestHttpModule));
        }

        /// <summary>Disposes the Unity _container when the application is shut down.</summary>
        public static void Shutdown()
        {
            //var container = UnityConfig.GetConfiguredContainer();
            //container.Dispose();
        }
    }
}