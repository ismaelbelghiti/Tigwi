using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NUnit.Framework;

namespace Tigwi.UI.Tests.Models
{
    using Moq;

    using StorageLibrary;

    using Tigwi.UI.Models.Storage;

    [TestFixture]
    public class StorageUserModelTest
    {
        protected Mock<IStorage> MockStorage { get; set; }

        protected StorageUserModel UserModel { get; set; }
        #region SetUp / TearDown

        [SetUp]
        public void Init()
        {
            var mock = new Mock<IStorage>();
            this.MockStorage = mock;

            var istorage = mock.Object;
            var guid = istorage.User.Create("Elarnon", "cbasile06@gmail.com");

            this.UserModel = new StorageUserModel(istorage, guid);
        }

        [TearDown]
        public void Dispose()
        { }

        #endregion

        #region Tests

        [Test]
        public void TestGetProperties()
        {
            var storageUserModel = this.UserModel;

            // Assert.That(storageUserModel.Id, Is.EqualTo(this.Id));
            Assert.That(storageUserModel.Login, Is.EqualTo("Elarnon"));
            Assert.That(storageUserModel.Email, Is.EqualTo("cbasile06@gmail.com"));
            Assert.That(storageUserModel.Avatar, Is.Null);
        }

        [Test]
        public void TestSave()
        {
            var storageUserModel = this.UserModel;
            storageUserModel.Email = "basile.clement@ens.fr";

            storageUserModel.Save();

            var otherModel = new StorageUserModel(this.MockStorage.Object, storageUserModel.Id);
            Assert.That(otherModel.Email, Is.EqualTo("basile.clement@ens.fr"));
        }

        [Test]
        public void TestSetEmail()
        {
            var storageUserModel = this.UserModel;
            storageUserModel.Email = "basile.clement@ens.fr";

            Assert.That(storageUserModel.Email, Is.EqualTo("basile.clement@ens.fr"));
        }

        #endregion
    }
}
