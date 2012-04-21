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
        protected StorageUserModel UserModel { get; set; }

        protected IStorageContext Storage { get; set; }

        #region SetUp / TearDown

        [SetUp]
        public void Init()
        {
            var istorage = new StorageTmp();
            this.Storage = new StorageContext(istorage);

            this.UserModel = this.Storage.Users.Create("Elarnon", "cbasile06@gmail.com");
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
        public void TestSetEmail()
        {
            var storageUserModel = this.UserModel;
            storageUserModel.Email = "basile.clement@ens.fr";

            Assert.That(storageUserModel.Email, Is.EqualTo("basile.clement@ens.fr"));
        }

        [Test]
        public void TestSaveEmail()
        {
            var storageUserModel = this.UserModel;
            storageUserModel.Email = "basile.clement@ens.fr";

            storageUserModel.Save();

            storageUserModel.Repopulate();
            Assert.That(storageUserModel.Email, Is.EqualTo("basile.clement@ens.fr"));

            var otherModel = this.Storage.Users.Find(storageUserModel.Id);
            Assert.That(otherModel.Email, Is.EqualTo("basile.clement@ens.fr"));
        }

        [Test]
        public void TestAddAccounts()
        {
            var user = this.UserModel;

            var account = this.Storage.Accounts.Create(user, "Koukou", "A stupid account.");

            Assert.That(user.Accounts.Contains(account), Is.True);
        }

        #endregion
    }
}
