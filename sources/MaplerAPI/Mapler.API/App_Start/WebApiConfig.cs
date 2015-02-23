using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;
using DataPersistance.Facade;
using Mapler.API.Security;
using Mapler.DataPersistance.Models;
using System.Web.Http.Routing;
using System.Net.Http;
using System.Web.Http.ExceptionHandling;
using Elmah.Contrib.WebApi;
using Mapler.API.CORS;

namespace Mapler.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            SetupLogging(config);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.EnableCors(new CorsPolicyProviderAttribute());

            //config.Routes.MapHttpRoute(
            //    name: "CustomPostAction",
            //    routeTemplate: "api/{controller}/extras/{action}",
            //    defaults: null,
            //    constraints: new { action = @"[a-zA-Z]+" }
            //);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional },
                constraints: new { id = new GuidConstraint(true) }
            );
            
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

            config.DependencyResolver = UnityConfig.CreateContainer();
        }

        private static void SetupLogging(HttpConfiguration config)
        {
            // The 'ElmahHandleErrorApiAttribute' attribute on controllers is used as well
            config.Services.Add(typeof(IExceptionLogger), new ElmahExceptionLogger());
        }
    }

    public class GuidConstraint : IHttpRouteConstraint
    {
        private readonly bool _allowEmpty;
        public GuidConstraint(bool allowEmpty)
        {
            _allowEmpty = allowEmpty;
        }

        public bool Match(HttpRequestMessage request, IHttpRoute route, string parameterName, IDictionary<string, object> values,
                          HttpRouteDirection routeDirection)
        {
            if (_allowEmpty && values.ContainsKey(parameterName) && string.Empty == values[parameterName].ToString())
                return true;

            if (values.ContainsKey(parameterName))
            {
                string stringValue = values[parameterName] as string;

                if (!string.IsNullOrEmpty(stringValue))
                {
                    Guid guidValue;

                    return Guid.TryParse(stringValue, out guidValue) && (guidValue != Guid.Empty);
                }
            }

            return false;
        }
    }
}
