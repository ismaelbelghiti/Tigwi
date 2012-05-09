using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NUnit.Framework;
using Tigwi.UI;
using Tigwi.UI.Controllers;
using Rhino.Mocks;

namespace Tigwi.UI.Tests.Controllers
{
    using System.Web;
    using System.Web.Security;

    using MvcContrib.TestHelper;
    using MvcContrib.TestHelper.Fakes;

    using Tigwi.Storage.Library;

    using Tigwi.UI.Models;
    using Tigwi.UI.Models.Storage;
    using Tigwi.UI.Models.User;

    [TestFixture]
    public class UserControllerTest
    {
        protected IStorageContext Storage { get; set; }

        protected UserController Controller { get; set; }

        protected TestControllerBuilder Builder { get; set; }

        [SetUp]
        public void Init()
        {
            var builder = new TestControllerBuilder();
            builder.HttpContext.Request.Expect(r => r.Cookies).Return(new HttpCookieCollection());
            builder.HttpContext.Response.Expect(r => r.Cookies).Return(new HttpCookieCollection());
            var storage = new StorageContext(new StorageTmp());
            var controller = builder.CreateController<UserController>(storage);

            this.Builder = builder;
            this.Storage = storage;
            this.Controller = controller;
        }

        [Test]
        public void Register()
        {
            // Initialization stuff
            var controller = this.Controller;
            var storage = this.Storage;
            var model = new RegisterViewModel { Login = "Elarnon", Email = "cbasile06@gmail.com", Password = "pass", ConfirmPassword = "pass" };

            // Actually call the method
            var result = controller.Register(model);

            // Register must redirect to the "Welcome" action on success
            result.AssertActionRedirect().ToAction("Welcome");

            // User must be created and correctly populated
            Assert.DoesNotThrow(() => storage.Users.Find("Elarnon"));
            var user = storage.Users.Find("Elarnon");
            Assert.That(user.Login, Is.EqualTo("Elarnon"));
            Assert.That(user.Email, Is.EqualTo("cbasile06@gmail.com"));

            // Verify a cookie is set
            var cookie = this.Builder.HttpContext.Response.Cookies[FormsAuthentication.FormsCookieName];
            Assert.NotNull(cookie);
            Assert.DoesNotThrow(() => FormsAuthentication.Decrypt(cookie.Value));
        }

        [Test]
        public void DoubleRegisterFails()
        {
            // Initialization stuff
            var controller = this.Controller;
            var firstModel = new RegisterViewModel
                { Login = "Elarnon", Email = "cbasile06@gmail.com", Password = "pass", ConfirmPassword = "pass" };
            var secondModel = new RegisterViewModel
                { Login = "Elarnon", Email = "basile.clement@ens.fr", Password = "ssap", ConfirmPassword = "ssap" };

            // Register two users with same login
            controller.Register(firstModel);
            var result = controller.Register(secondModel);

            // The result must be the view with the same argument as the controller's, and the ModelState must be invalid.
            result.AssertViewRendered().WithViewData<RegisterViewModel>().ShouldEqual(secondModel, "Unexpected model value.");
            Assert.IsFalse(controller.ModelState.IsValid);
            Assert.IsFalse(controller.ModelState.IsValidField("Login"));
        }
    }
}
