using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using StorageLibrary;

namespace StorageTest
{
    [TestFixture]
    class AccountTest
    {
        public IStorage storage;

        [SetUp]
        public void InitStorage()
        {
            storage = new StorageTmp();
            Guid userId = storage.User.Create("userThatExists", "userThatExists@gmail.com", new Byte[1]);
            Guid accountId = storage.Account.Create(userId, "accountThatExists", "accountThatExistsDesc");
            storage.User.Create("otherUserThatExists", "otherUserThatExists@gmail.com", new Byte[1]);
            Guid otherAccountId = storage.Account.Create(userId, "otherAccountThatExists", "otherAccountThatExistsDesc");
        }

        #region Guid GetId(string name)

        [Test]
        [ExpectedException(typeof(AccountNotFound))]
        public void GetIdAccountNotFound()
        {
            storage.Account.GetId("accountThatDoesntExist");
        }

        [Test]
        public void GetIdNormalBehaviour()
        {
            storage.Account.GetId("accountThatExists");
        }

        #endregion

        #region IAccountInfo GetInfo(Guid accountId)

        [Test]
        [ExpectedException(typeof(AccountNotFound))]
        public void GetInfoAccountNotFound()
        {
            storage.Account.GetInfo(Guid.NewGuid());
        }

        [Test]
        public void GetInfoNormalBehaviour()
        {
            Guid accountid = storage.Account.GetId("accountThatExists");
            IAccountInfo accountinfo = storage.Account.GetInfo(accountid);
            Assert.AreEqual(accountinfo.Name, "accountThatExists");
            Assert.AreEqual(accountinfo.Description, "accountThatExistsDesc");
        }

        #endregion

        #region void SetInfo(Guid accountId, string description)

        [Test]
        [ExpectedException(typeof(AccountNotFound))]
        public void SetInfoAccountNotFound()
        {
            storage.Account.SetInfo(new Guid(), "");
        }

        [Test]
        public void SetInfoNormalBehaviour()
        {
            Guid accountId = storage.Account.GetId("accountThatExists");
            storage.Account.SetInfo(accountId, "accountThatExistsBadDesc");
            IAccountInfo newaccountinfo = storage.Account.GetInfo(accountId);
            Assert.AreEqual(newaccountinfo.Description,"accountThatExistsBadDesc") ;
            storage.Account.SetInfo(accountId, "accountThatExistsDesc");
        }

        #endregion

        #region HashSet<Guid> GetUsers(Guid accountId)

        [Test]
        [ExpectedException(typeof(AccountNotFound))]
        public void GetUsersAccountNotFound()
        {

        }

        [Test]
        public void GetUsersNormalBehaviour()
        {

        }

        #endregion

        #region Guid GetAdminId(Guid accountId)

        [Test]
        [ExpectedException(typeof(AccountNotFound))]
        public void GetAdminIdAccountNotFound()
        {

        }

        [Test]
        public void GetAdminIdNormalBehaviour()
        {

        }

        #endregion

        #region void SetAdminId(Guid accountId, Guid userId)

        [Test]
        [ExpectedException(typeof(AccountNotFound))]
        public void SetAdminIdAccountNotFound()
        {

        }

        [Test]
        [ExpectedException(typeof(UserNotFound))]
        public void SetAdminIdUserNotFound()
        {

        }

        [Test]
        public void SetAdminIdNormalBehaviour()
        {

        }

        #endregion


        #region void Add(Guid accountId, Guid userId)

        [Test]
        [ExpectedException(typeof(AccountNotFound))]
        public void AddAccountNotFound()
        {

        }

        [Test]
        [ExpectedException(typeof(UserNotFound))]
        public void AddUserNotFound()
        {

        }

        [Test]
        public void AddNormalBehaviour()
        {

        }

        #endregion

        #region void Remove(Guid accountId, Guid userId)

        [Test]
        [ExpectedException(typeof(UserIsAdmin))]
        public void RemoveUserIsAdmin()
        {

        }

        [Test]
        public void RemoveNormalBehaviour()
        {

        }

        #endregion

        #region Guid Create(Guid adminId, string name, string description)

        [Test]

        [ExpectedException(typeof(UserNotFound))]
        public void CreateUserNotFound()
        {

        }

        [Test]
        [ExpectedException(typeof(AccountAlreadyExists))]
        public void CreateAccountAlreadyExists()
        {

        }

        [Test]
        public void CreateNormalBehaviour()
        {

        }

        #endregion


        #region void Delete(Guid accountId)

        [Test]
        public void DeleteNormalBehaviour()
        {

        }

        #endregion

    }
}
