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
    public class StorageContextTest
    {
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
            var mockStorage = new Mock<IStorage>();
            var storageContext = new StorageContext(mockStorage.Object);

            Assert.That(storageContext.Accounts, Is.Not.Null);
            Assert.That(storageContext.Lists, Is.Not.Null);
            Assert.That(storageContext.Posts, Is.Not.Null);
            Assert.That(storageContext.Users, Is.Not.Null);
        }

        #endregion
    }
}
