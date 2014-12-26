using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;
using MaplerAPI.App_Start;
using Microsoft.Practices.Unity.Mvc;

namespace MaplerAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

            RegisterRoutes(config);
            SetupIocContainer(config);
        }

        private static void SetupIocContainer(HttpConfiguration config)
        {
            var container = UnityConfig.GetConfiguredContainer();
            config.DependencyResolver = new UnityHttpDependencyResolver(container);
        }

        private static void RegisterRoutes(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

    }
}
