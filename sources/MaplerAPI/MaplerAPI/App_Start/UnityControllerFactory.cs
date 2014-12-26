using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;

namespace MaplerAPI.App_Start
{
    public class UnityControllerFactory : IControllerFactory
    {
        private readonly IUnityContainer _container;
        private readonly IControllerFactory _defaultControllerFactory;

        public UnityControllerFactory(IUnityContainer container, IControllerFactory defaultControllerFactory)
        {
            _container = container;
            _defaultControllerFactory = defaultControllerFactory;
        }

        public IController CreateController(System.Web.Routing.RequestContext requestContext, string controllerName)
        {
            //throw new NotImplementedException();
            return _defaultControllerFactory.CreateController(requestContext, controllerName);
        }

        public System.Web.SessionState.SessionStateBehavior GetControllerSessionBehavior(System.Web.Routing.RequestContext requestContext, string controllerName)
        {
            //throw new NotImplementedException();
            return _defaultControllerFactory.GetControllerSessionBehavior(requestContext, controllerName);
        }

        public void ReleaseController(IController controller)
        {
            //throw new NotImplementedException();
            _defaultControllerFactory.ReleaseController(controller);
        }
    }
}