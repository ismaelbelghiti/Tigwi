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

    using Tigwi.UI.Models;
    using Tigwi.UI.Models.Storage;

    [TestFixture]
    public class StorageUserModelTest
    {
        protected IUserModel UserModel { get; set; }

        protected IStorageContext Storage { get; set; }

        #region SetUp / TearDown

        [SetUp]
        public void Init()
        {
            this.Storage = new StorageContext(new StorageTmp());
        }

        [TearDown]
        public void Dispose()
        { }

        #endregion

        #region Tests

        [Test]
        public void TestGetProperties()
        {
            var storage = this.Storage;

            var user = storage.Users.Create("Elarnon", "cbasile06@gmail.com");

            Assert.That(user.Login, Is.EqualTo("Elarnon"));
            Assert.That(user.Email, Is.EqualTo("cbasile06@gmail.com"));
            Assert.That(user.Avatar, Is.Null);
        }

        [Test]
        public void TestSetEmail()
        {
            var storage = this.Storage;
            var user = storage.Users.Create("Elarnon", "cbasile06@gmail.com");

            user.Email = "basile.clement@ens.fr";
            Assert.That(user.Email, Is.EqualTo("basile.clement@ens.fr"));

            user.Save();

            user.Repopulate();
            Assert.That(user.Email, Is.EqualTo("basile.clement@ens.fr"));
        }

        [Test]
        public void TestAccountsLink()
        {
            var storage = this.Storage;
            var user = storage.Users.Create("Elarnon", "cbasile06@gmail.com");
            // user.CreateAcccount ?
            var account = storage.Accounts.Create(user, "ElarnonAccount", "Elarnon's account");

            Assert.That(account.Admin, Is.EqualTo(user));
            Assert.That(user.Accounts.Contains(account), Is.True);
            Assert.That(account.Users.Contains(user), Is.True);

            user.Accounts.Remove(account);

            Assert.That(user.Accounts.Contains(account), Is.False);
            Assert.That(account.Users.Contains(user), Is.False);
        }

        #endregion
    }
}
