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
        const string azureAccountName = "ulyssestorage";
        const string azureAccountKey = "fc2HTyfP0m2r3zlNYmMc3Pjvbfmy63ovoCP9Zkz0yoyuId3AeyrTswLcye2VDr3hzDvAQbdeKUlXBX3lFTcNWQ==";

        [SetUp]
        public void InitStorage()
        {
            bool UseStorageTmp = false;
            if (UseStorageTmp)
                storage = new StorageTmp();
            else
            {
                BlobFactory blobFactory = new BlobFactory(azureAccountName, azureAccountKey);
                blobFactory.InitStorage();
                storage = new Storage(azureAccountName, azureAccountKey);
            }

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
            HashSet<Guid> users = storage.Account.GetUsers(Guid.NewGuid());
        }

        [Test]
        public void GetUsersNormalBehaviour()
        {
            Guid userId = storage.User.GetId("userThatExists");
            Guid accountId = storage.Account.GetId("accountThatExists");
            HashSet<Guid> users = storage.Account.GetUsers(accountId);
            Assert.IsTrue(users.Contains(userId));
        }

        #endregion

        #region Guid GetAdminId(Guid accountId)

        [Test]
        [ExpectedException(typeof(AccountNotFound))]
        public void GetAdminIdAccountNotFound()
        {
            storage.Account.GetAdminId(new Guid());
        }

        [Test]
        public void GetAdminIdNormalBehaviour()
        {
             Guid userId = storage.User.GetId("userThatExists");
             Guid accountId = storage.Account.GetId("accountThatExists");
             Guid admin = storage.Account.GetAdminId(accountId);
             Assert.AreEqual(admin, userId);
        }

        #endregion

        #region void SetAdminId(Guid accountId, Guid userId)

        [Test]
        [ExpectedException(typeof(AccountNotFound))]
        public void SetAdminIdAccountNotFound()
        {
            Guid userId = storage.User.GetId("userThatExists");
            Guid accountId = storage.Account.GetId("accountThatDoesntExist");
            storage.Account.SetAdminId(accountId, userId);
        }

        [Test]
        [ExpectedException(typeof(UserNotFound))]
        public void SetAdminIdUserNotFound()
        {
            Guid userId = storage.User.GetId("userThatDoesntExist");
            Guid accountId = storage.Account.GetId("accountThatExists");
            storage.Account.SetAdminId(accountId, userId);
        }

        [Test]
        public void SetAdminIdNormalBehaviour()
        {
            Guid userId = storage.User.GetId("userThatExists");
            Guid accountId = storage.Account.GetId("accountThatExists");
        }

        #endregion

        #region void Add(Guid accountId, Guid userId)

        [Test]
        [ExpectedException(typeof(AccountNotFound))]
        public void AddAccountNotFound()
        {
            Guid otherUserId = storage.User.GetId("otherUserThatExists");
            storage.Account.Add(otherUserId, otherUserId);
        }

        [Test]
        [ExpectedException(typeof(UserNotFound))]
        public void AddUserNotFound()
        {
            Guid accountId = storage.Account.GetId("accountThatExists");
            Guid otherUserId = storage.User.GetId("otherUserThatDoesnotExists");
            storage.Account.Add(accountId, otherUserId);
        }

        [Test]
        public void AddNormalBehaviour()
        {
            Guid accountId = storage.Account.GetId("accountThatExists");
            Guid otherUserId = storage.User.GetId("otherUserThatExists");
            storage.Account.Add(accountId, otherUserId);
            HashSet<Guid> users = storage.Account.GetUsers(accountId);
            Assert.IsTrue(users.Contains(otherUserId));
            HashSet<Guid> accounts = storage.User.GetAccounts(otherUserId);
            Assert.IsTrue(accounts.Contains(accountId));
        }

        #endregion

        #region void Remove(Guid accountId, Guid userId)

        [Test]
        [ExpectedException(typeof(UserIsAdmin))]
        public void RemoveUserIsAdmin()
        {
            Guid userId = storage.User.GetId("userThatExists");
            Guid accountId = storage.Account.GetId("accountThatExists");
            storage.Account.Remove(accountId, userId);
        }

        [Test]
        public void RemoveWhenWrongAccountId()
        {
            Guid userId = storage.User.GetId("userThatExists");
            storage.Account.Remove(Guid.NewGuid(), userId);
        }

        [Test]
        public void RemoveWhenWrongUserId()
        {
            Guid accountId = storage.Account.GetId("accountThatExists");
            storage.Account.Remove(accountId, Guid.NewGuid());
        }

        [Test]
        public void RemoveNormalBehaviour()
        {
            Guid accountId = storage.Account.GetId("accountThatExists");
            Guid otherUserId = storage.User.GetId("otherUserThatExists");
            storage.Account.Add(accountId, otherUserId);
            storage.Account.Remove(accountId, otherUserId);
            HashSet<Guid> users = storage.Account.GetUsers(accountId);
            Assert.IsTrue(!users.Contains(otherUserId));
            HashSet<Guid> accounts = storage.User.GetAccounts(otherUserId);
            Assert.IsTrue(!accounts.Contains(accountId));
        }

        #endregion

        #region Guid Create(Guid adminId, string name, string description)

        [Test]

        [ExpectedException(typeof(UserNotFound))]
        public void CreateUserNotFound()
        {
            storage.Account.Create(Guid.NewGuid(), "accountThatDoesntExists", "description2");
        }

        [Test]
        [ExpectedException(typeof(AccountAlreadyExists))]
        public void CreateAccountAlreadyExists()
        {
            Guid userId = storage.User.GetId("userThatExists");
            storage.Account.Create(userId, "accountThatExists", "description2");
        }

        // test normal behavior done while doing init

        #endregion

        #region void Delete(Guid accountId)

        [Test]
        public void DeleteWithWrongAccountId()
        {
            storage.Account.Delete(new Guid());
        }

        [Test]
        [ExpectedException(typeof(AccountNotFound))]
        public void DeleteNormalBehaviour()
        {
            Guid otherAccountId = storage.Account.GetId("otherAccountThatExists");
            storage.Account.Delete(otherAccountId);
            storage.Account.GetId("otherAccountThatExists");
        }

        #endregion

    }
}
