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

    using Tigwi.UI.Models;
    using Tigwi.UI.Models.Storage;

    [TestFixture]
    public class AccountRepositoryTest
    {
        protected IStorageContext Storage { get; set; }

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
        public void TestCreate()
        {
            var storage = this.Storage;
            var user = storage.Users.Create("Elarnon", "cbasile06@gmail.com", new byte[0]);

            var account = storage.Accounts.Create(user, "ElarnonAccount", "Elarnon's account");

            Assert.That(account.Name, Is.EqualTo("ElarnonAccount"));
            Assert.That(account.Admin, Is.EqualTo(user));
            Assert.That(account.Description, Is.EqualTo("Elarnon's account"));
            Assert.That(account.Users.Contains(user), Is.True);
        }

        [Test]
        public void TestFindUnicity()
        {
            var storage = this.Storage;
            var user = storage.Users.Create("Elarnon", "cbasile06@gmail.com", new byte[0]);
            var account = storage.Accounts.Create(user, "ElarnonAccount", "Elarnon's account");

            Assert.That(storage.Accounts.Find(account.Id), Is.EqualTo(account));
            Assert.That(storage.Accounts.Find(account.Name), Is.EqualTo(account));
        }

        [Test]
        public void TestNameUnicity()
        {
            var storage = this.Storage;
            var user = storage.Users.Create("Elarnon", "cbasile06@gmail.com", new byte[0]);
            storage.Accounts.Create(user, "ElarnonAccount", "Elarnon's account");

            Assert.Throws<DuplicateAccountException>(() => storage.Accounts.Create(user, "ElarnonAccount", "."));
            Assert.DoesNotThrow(() => storage.Accounts.Create(user, "AnAccount", "Elarnon's account"));
        }

        #endregion
    }
}
