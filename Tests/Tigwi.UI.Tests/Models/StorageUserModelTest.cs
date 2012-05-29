#region copyright
// Copyright (c) 2012, TIGWI
// All rights reserved.
// Distributed under  BSD 2-Clause license
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NUnit.Framework;

namespace Tigwi.UI.Tests.Models
{
    using Tigwi.Storage.Library;

    using Tigwi.UI.Models.Storage;

    [TestFixture]
    public class StorageUserModelTest
    {
        protected StorageUserModel UserModel { get; set; }

        protected StorageContext Storage { get; set; }

        const string azureAccountName = "ulyssestorage";
        const string azureAccountKey = "fc2HTyfP0m2r3zlNYmMc3Pjvbfmy63ovoCP9Zkz0yoyuId3AeyrTswLcye2VDr3hzDvAQbdeKUlXBX3lFTcNWQ==";


        #region SetUp / TearDown

        [SetUp]
        public void Init()
        {
            this.Storage = new StorageContext(new Storage(azureAccountName, azureAccountKey));
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

            var user = storage.Users.Create("Elarnon", "cbasile06@gmail.com", new byte[0]);

            Assert.That(user.Login, Is.EqualTo("Elarnon"));
            Assert.That(user.Email, Is.EqualTo("cbasile06@gmail.com"));
            Assert.That(user.Avatar, Is.Null);
        }

        [Test]
        public void TestSetEmail()
        {
            var storage = this.Storage;
            var user = storage.Users.Create("Elarnon", "cbasile06@gmail.com", new byte[0]);

            user.Email = "basile.clement@ens.fr";
            Assert.That(user.Email, Is.EqualTo("basile.clement@ens.fr"));

            storage.SaveChanges();
            Assert.That(user.Email, Is.EqualTo("basile.clement@ens.fr"));
        }

        [Test]
        public void TestAccountsLink()
        {
            var storage = this.Storage;
            var user = storage.Users.Create("Elarnon", "cbasile06@gmail.com", new byte[0]);
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
