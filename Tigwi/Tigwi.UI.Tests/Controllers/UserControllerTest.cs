using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NUnit.Framework;
using Tigwi.UI;
using Tigwi.UI.Controllers;

namespace Tigwi.UI.Tests.Controllers
{
    using StorageLibrary;

    using Tigwi.UI.Models.Storage;
    using Tigwi.UI.Models.User;

    [TestFixture]
    public class UserControllerTest
    {
        [Test]
        public void Register()
        {
            // Arrange
            var storage = new StorageContext(new StorageTmp());
            var controller = new UserController(storage);

            var model = new RegisterViewModel
                { Login = "Elarnon", Email = "cbasile06@gmail.com", Password = "pass", PasswordConfirmation = "pass" };

            var result = controller.Register(model) as RedirectToRouteResult;
        }
    }
}
