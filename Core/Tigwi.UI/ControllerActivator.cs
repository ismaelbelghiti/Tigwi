using System;
using System.Collections.Generic;
namespace Tigwi.UI
{
    using System.Web.Mvc;
    using System.Web.Routing;

    using Tigwi.Storage.Library;
    using Tigwi.UI.Controllers;
    using Tigwi.UI.Models;

    public class ControllerActivator : IControllerActivator
    {
        private IStorage rawStorage;

        public ControllerActivator(IStorage storage)
        {
            this.rawStorage = storage;
        }

        #region Implementation of IControllerActivator

        public IController Create(RequestContext requestContext, Type controllerType)
        {
            if (typeof(HomeController).IsAssignableFrom(controllerType))
            {
                return Activator.CreateInstance(controllerType, this.rawStorage) as HomeController;
            }

            return Activator.CreateInstance(controllerType) as IController;
        }

        #endregion
    }
}