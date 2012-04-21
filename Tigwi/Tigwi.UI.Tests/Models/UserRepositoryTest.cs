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
        protected Mock<IStorage> MockStorage { get; set; }

        #region SetUp / TearDown

        [SetUp]
        public void Init()
        {
            var mock = new Mock<IStorage>();
            this.MockStorage = mock;

            var map = new Dictionary<Guid, IUserInfo>();
            var loginMap = new Dictionary<string, Guid>();

            mock.Setup(storage => storage.User.Create(It.IsAny<string>(), It.IsAny<string>())).Returns(
                (string login, string email) =>
                    {
                        var id = Guid.NewGuid();
                        var mockUser = new Mock<IUserInfo>();
                        mockUser.SetupAllProperties();
                        var userInfo = mockUser.Object;
                        userInfo.Email = email;
                        userInfo.Login = login;
                        if (map.ContainsKey(id))
                        {
                            throw new UserAlreadyExists();
                        }

                        map.Add(id, userInfo);
                        loginMap.Add(login, id);

                        return id;
                    });

            mock.Setup(storage => storage.User.Delete(It.IsAny<Guid>())).Callback<Guid>(
                id =>
                    {
                        IUserInfo userInfo;
                        if (!map.TryGetValue(id, out userInfo))
                        {
                            return;
                        }

                        loginMap.Remove(userInfo.Login);
                        map.Remove(id);
                    });

            mock.Setup(storage => storage.User.GetId(It.IsAny<string>())).Returns(
                (string login) =>
                    {
                        Guid id;
                        if (loginMap.TryGetValue(login, out id))
                        {
                            return id;
                        }

                        throw new UserNotFound();
                    });

            mock.Setup(storage => storage.User.GetInfo(It.IsAny<Guid>())).Returns(
                (Guid id) =>
                    {
                        IUserInfo userInfo;
                        if (map.TryGetValue(id, out userInfo))
                        {
                            return userInfo;
                        }

                        throw new UserNotFound();
                    });

            this.Repository = new UserRepository(mock.Object);
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
            repository.Delete(account);

            Assert.Throws<UserNotFound>(() => repository.Find("Elarnon"));
        }

        #endregion
    }
}
