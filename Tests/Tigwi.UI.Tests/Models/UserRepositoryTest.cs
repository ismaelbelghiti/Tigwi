using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NUnit.Framework;

namespace Tigwi.UI.Tests.Models
{
    using System.IO;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;

    using Tigwi.Storage.Library;

    using Tigwi.UI.Models;
    using Tigwi.UI.Models.Storage;

    [TestFixture]
    public class UserRepositoryTest
    {
        protected IStorageContext Storage { get; set; }

        #region SetUp / TearDown

        [SetUp]
        public void Init()
        {
            this.Storage = new StorageContext(new StorageTmp());
            this.Repository = this.Storage.Users;
        }

        protected IUserRepository Repository { get; set; }

        [TearDown]
        public void Dispose()
        { }

        #endregion

        #region Tests

        [Serializable]
        public class Test
        {
            public Guid UserId { get; set; }

            public Guid AccountId { get; set; }
        }

        [Test]
        public void MyTest()
        {
            var test = new Test { UserId = Guid.NewGuid(), AccountId = Guid.NewGuid() };

            Console.WriteLine(test.UserId);
            Console.WriteLine(test.AccountId);

            var formatter = new BinaryFormatter();
            var stream = new MemoryStream();

            formatter.Serialize(stream, test);

            var encodedStr = Convert.ToBase64String(stream.ToArray());
            Console.WriteLine(encodedStr);
            var decodedArr = Convert.FromBase64String(encodedStr);
            var stream2 = new MemoryStream(decodedArr);
            var deserialized = (new BinaryFormatter()).Deserialize(stream2) as Test;

            Console.WriteLine(deserialized.UserId);
            Console.WriteLine(deserialized.AccountId);
        }

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
        public void TestCreateCreatesAccount()
        {
            var storage = this.Storage;
            var user = storage.Users.Create("Elarnon", "cbasile06@gmail.com");

            Assert.DoesNotThrow(() => storage.Accounts.Find("Elarnon"));
            Assert.That(storage.Accounts.Find("Elarnon").Admin, Is.EqualTo(user));
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
        public void TestNameUnicity()
        {
            var storage = this.Storage;
            var user = storage.Users.Create("Elarnon", "cbasile06@gmail.com");

            Assert.Throws<DuplicateUserException>(() => storage.Users.Create("Elarnon", "mail"));
            Assert.DoesNotThrow(() => storage.Users.Create("Login", "cbasile06@gmail.com"));
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
