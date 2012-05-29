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
    public class StorageContextTest
    {
        const string azureAccountName = "ulyssestorage";
        const string azureAccountKey = "fc2HTyfP0m2r3zlNYmMc3Pjvbfmy63ovoCP9Zkz0yoyuId3AeyrTswLcye2VDr3hzDvAQbdeKUlXBX3lFTcNWQ==";

        #region SetUp / TearDown

        [SetUp]
        public void Init()
        { }

        [TearDown]
        public void Dispose()
        { }

        #endregion

        #region Tests

        [Test]
        public void RepositoriesAreNotNull()
        {
            var storageContext = new StorageContext(new Storage(azureAccountName, azureAccountKey));

            Assert.That(storageContext.Accounts, Is.Not.Null);
            Assert.That(storageContext.Lists, Is.Not.Null);
            Assert.That(storageContext.Posts, Is.Not.Null);
            Assert.That(storageContext.Users, Is.Not.Null);
        }

        #endregion
    }
}
