using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using StorageLibrary;

namespace StorageTest
{
    [TestFixture]
    class ListTest
    {
        public IStorage storage;
        const string azureAccountName = "ulyssestorage";
        const string azureAccountKey = "jnfLEhtEGAZ6YzRoYSahJpgUzXL2438grLGeFn/lnhxNJGonwD/jO+7QU2u/UECHeYsF4uigIfXKGsqRbjRsTQ==";

        Guid listIdThatExists; 

        [SetUp]
        public void InitStorage()
        {
            bool UseStorageTmp = true;
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
            listIdThatExists = storage.List.Create(storage.Account.GetId("accountThatExists"), "listThatExists", "Yeah", false);
            storage.List.Add(listIdThatExists, accountId);
            storage.List.Add(listIdThatExists, otherAccountId);
            storage.List.Follow(listIdThatExists, accountId);
            storage.List.Follow(listIdThatExists, otherAccountId);
        }

        #region IListInfo GetInfo(Guid listId)

        [Test]
        [ExpectedException(typeof(ListNotFound))]
        public void GetInfoListNotFound()
        {
            storage.List.GetInfo(new Guid());
        }

        [Test]
        public void GetInfoNormalBehaviour()
        {
            IListInfo listInfo = storage.List.GetInfo(listIdThatExists);
            Assert.AreEqual(listInfo.Name,"listThatExists") ;
        }

        #endregion

        #region void SetInfo(Guid listId, string name, string description, bool isPrivate)

        [Test]
        [ExpectedException(typeof(ListNotFound))]
        public void SetInfoListNotFound()
        {
            storage.List.SetInfo(new Guid(), "babar", "babar is cool", false);
        }

        [Test]
        [ExpectedException(typeof(IsPersonnalList))]
        public void SetInfoIsPersonnalList()
        {
            Guid idPersList = storage.List.GetPersonalList(storage.Account.GetId("accountThatExists"));
            storage.List.SetInfo(idPersList, "babar", "babar", false);
        }

        [Test]
        public void SetInfoNormalBehaviour()
        {
            storage.List.SetInfo(listIdThatExists, "listThatExists", "Babar", false);
            Assert.AreEqual(storage.List.GetInfo(listIdThatExists).Description, "Babar");
        }

        #endregion

        #region Guid GetOwner(Guid listId)

        [Test]
        [ExpectedException(typeof(ListNotFound))]
        public void GetOwnerListNotFound()
        {
            storage.List.GetOwner(new Guid());
        }

        [Test]
        public void GetOwnerNormalBehaviour()
        {
            Guid ownerId = storage.List.GetOwner(listIdThatExists);
            Assert.AreEqual(ownerId, storage.Account.GetId("accountThatExists"));
        }

        #endregion

        #region Guid GetPersonalList(Guid accountId)

        [Test]
        [ExpectedException(typeof(AccountNotFound))]
        public void GetPersonalListAccountNotFound()
        {
            storage.List.Create(new Guid(), "babar", "babar", false);
        }

        //test normal beahaviour : already tested

        #endregion

        #region Guid Create(Guid ownerId, string name, string description, bool isPrivate)

        [Test]
        [ExpectedException(typeof(AccountNotFound))]
        public void CreateAccountNotFound()
        {
            storage.List.Create(new Guid(), "babar", "babar", false);
        }

        //test normal beahaviour : already tested

        #endregion

        #region void Delete(Guid id)

        [Test]
        [ExpectedException(typeof(IsPersonnalList))]
        public void DeleteIsPersonnalList()
        {
            storage.List.Delete(storage.List.GetPersonalList(storage.Account.GetId("accountThatExists")));
        }

        [Test]
        public void DeleteWithWrondId()
        {
            storage.List.Delete(new Guid());
        }

        [Test]
        [ExpectedException(typeof(ListNotFound))]
        public void DeleteNormalBehaviour()
        {
            Guid listTempId = storage.List.Create(storage.Account.GetId("accountThatExists"), "babar", "babar", false);
            storage.List.Delete(listTempId);
            storage.List.GetInfo(listTempId);
        }

        #endregion

        #region void Follow(Guid listId, Guid accountId)

        [Test]
        [ExpectedException(typeof(AccountNotFound))]
        public void FollowAccountNotFound()
        {
            storage.List.Follow(listIdThatExists, new Guid());
        }

        [Test]
        [ExpectedException(typeof(ListNotFound))]
        public void FollowListNotFound()
        {
            storage.List.Follow(new Guid(), storage.Account.GetId("accountThatExists"));
        }

        //test normal beahaviour : already tested

        #endregion

        #region void Unfollow(Guid listId, Guid accountId)

        [Test]
        [ExpectedException(typeof(AccountNotFound))]
        public void UnfollowAccountNotFound()
        {
            storage.List.Unfollow(listIdThatExists, new Guid());
        }

        [Test]
        [ExpectedException(typeof(ListNotFound))]
        public void UnfollowListNotFound()
        {
            storage.List.Unfollow(new Guid(), storage.Account.GetId("accountThatExists"));
        }

        [Test]
        [ExpectedException(typeof(AccountIsOwner))]
        public void UnfollowAccountIsOwner()
        {
            storage.List.Unfollow(listIdThatExists, storage.Account.GetId("accountThatExists"));
        }

        [Test]
        public void UnfollowNormalBehaviour()
        {
            Guid otherAccountId = storage.Account.GetId("otherAccountThatExists");
            storage.List.Unfollow(listIdThatExists, otherAccountId);
            HashSet<Guid> listsFollowed = storage.List.GetAccountFollowedLists(otherAccountId, false);
            HashSet<Guid> followers = storage.List.GetFollowingAccounts(listIdThatExists);

            Assert.IsTrue(!listsFollowed.Contains(otherAccountId));
            Assert.IsTrue(!followers.Contains(listIdThatExists));
            storage.List.Follow(listIdThatExists, otherAccountId);
        }

        #endregion

        #region HashSet<Guid> GetAccounts(Guid listId)

        [Test]
        [ExpectedException(typeof(ListNotFound))]
        public void GetAccountsListNotFound()
        {
            storage.List.GetAccounts(new Guid());
        }

        [Test]
        public void GetAccountsNormalBehaviour()
        {
            Assert.IsTrue(storage.List.GetAccounts(listIdThatExists).Contains(storage.Account.GetId("accountThatExists")));
        }

        #endregion

        #region void Add(Guid listId, Guid accountId)

        [Test]
        [ExpectedException(typeof(ListNotFound))]
        public void AddListNotFound()
        {
            storage.List.Add(Guid.NewGuid(), storage.Account.GetId("accountThatExists"));
        }

        [Test]
        [ExpectedException(typeof(AccountNotFound))]
        public void AddAccountNotFound()
        {
            storage.List.Add(listIdThatExists, Guid.NewGuid());
        }

        //test normal beahaviour : already tested

        #endregion

        #region void Remove(Guid listId, Guid accountId)

        [Test]
        public void RemoveNormalBehaviour()
        {
            Guid accountThatExistsId = storage.Account.GetId("accountThatExists");
            storage.List.Remove(listIdThatExists, accountThatExistsId);
            HashSet<Guid> accounts = storage.List.GetAccounts(listIdThatExists);
            Assert.IsTrue(!accounts.Contains(accountThatExistsId));
            storage.List.Add(listIdThatExists, accountThatExistsId);
        }

        #endregion

        #region HashSet<Guid> GetAccountOwnedLists(Guid accountId, bool withPrivate)

        [Test]
        [ExpectedException(typeof(AccountNotFound))]
        public void GetAccountOwnedListsAccountNotFound()
        {
            storage.List.GetAccountOwnedLists(Guid.NewGuid(), false);
        }

        [Test]
        [ExpectedException(typeof(AccountNotFound))]
        public void GetAccountOwnedListsAccountNotFoundBis()
        {
            storage.List.GetAccountOwnedLists(Guid.NewGuid(), true);
        }

        [Test]
        public void GetAccountOwnedListsNormalBehaviour()
        {
            HashSet<Guid> ownedLists = storage.List.GetAccountOwnedLists(storage.Account.GetId("accountThatExists"), false);
            Assert.IsTrue(ownedLists.Contains(listIdThatExists));
        }

        #endregion

        #region HashSet<Guid> GetAccountFollowedLists(Guid accountId, bool withPrivate)

        [Test]
        [ExpectedException(typeof(AccountNotFound))]
        public void GetAccountFollowedListsAccountNotFound()
        {
            storage.List.GetAccountFollowedLists(new Guid(), false);
        }

        [Test]
        [ExpectedException(typeof(AccountNotFound))]
        public void GetAccountFollowedListsAccountNotFoundBis()
        {
            storage.List.GetAccountFollowedLists(new Guid(), true);
        }

        [Test]
        public void GetAccountFollowedListsNormalBehaviour()
        {
            HashSet<Guid> followedList = storage.List.GetAccountFollowedLists(storage.Account.GetId("otherAccountThatExists"), false);
            Assert.IsTrue(followedList.Contains(listIdThatExists));
        }

        #endregion

        #region HashSet<Guid> GetFollowingLists(Guid accountId)

        [Test]
        [ExpectedException(typeof(AccountNotFound))]
        public void GetFollowingListsAccountNotFound()
        {
            storage.List.GetFollowingLists(Guid.NewGuid());
        }

        [Test]
        public void GetFollowingListsNormalBehaviour()
        {
            HashSet<Guid> followingLists = storage.List.GetFollowingLists(storage.Account.GetId("accountThatExists"));
            Assert.IsTrue(followingLists.Contains(listIdThatExists));
        }

        #endregion

        #region HashSet<Guid> GetFollowingAccounts(Guid listId)

        [Test]
        [ExpectedException(typeof(ListNotFound))]
        public void GetFollowingAccountsListNotFound()
        {
            storage.List.GetFollowingAccounts(new Guid());
        }

        [Test]
        public void GetFollowingAccountsNormalBehaviour()
        {
            HashSet<Guid> followingAccounts = storage.List.GetFollowingAccounts(listIdThatExists);
            Assert.IsTrue(followingAccounts.Contains(storage.Account.GetId("accountThatExists")));
        }
        #endregion
    }
}
