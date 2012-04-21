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
    public class UserRepositoryTest
    {
        protected IStorage Storage { get; set; }

        #region SetUp / TearDown

        [SetUp]
        public void Init()
        {
            var istorage = new StorageTmp();
            this.Storage = istorage;
            this.Repository = (new StorageContext(istorage)).Users;
        }

        protected IUserRepository Repository { get; set; }

        [TearDown]
        public void Dispose()
        { }

        #endregion

        #region Tests

        [Test]
        public void TestCreate()
        {
            var repository = this.Repository;

            var account = repository.Create("Elarnon", "cbasile06@gmail.com");

            Assert.That(account, Is.Not.Null);
            Assert.That(account.Login, Is.EqualTo("Elarnon"));
            Assert.That(account.Email, Is.EqualTo("cbasile06@gmail.com"));
            Assert.That(account.Avatar, Is.Null);
        }

        [Test]
        public void TestFindUnicity()
        {
            var repository = this.Repository;

            var account = repository.Create("Elarnon", "cbasile06@gmail.com");

            Assert.That(repository.Find(account.Id), Is.EqualTo(account));
            Assert.That(repository.Find(account.Login), Is.EqualTo(account));
        }

        [Test]
        public void TestDelete()
        {
            var repository = this.Repository;

            var account = repository.Create("Elarnon", "cbasile06@gmail.com");

            Assert.Throws<NotImplementedException>(() => repository.Delete(account));
        }

        #endregion
    }
}
