using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Tigwi.Storage.Library;

namespace StorageTest
{
    [TestFixture]
    class ListTest
    {
        public IStorage storage;
        const string azureAccountName = "ulyssestorage";
        const string azureAccountKey = "73IH549/+HNKuXjF8LcIT2ad4v+acch8gn2Gm0GN1w9tRHFZGTm/fFOr8BRPV/1gHBKJ/1hLeB5IMdNGHv4VIA==";

        private Guid accountId;
        private Guid listIdThatExists;
        private Guid otherAccountId;

        [SetUp]
        public void InitStorage()
        {
            BlobFactory blobFactory = new BlobFactory(azureAccountName, azureAccountKey);
            blobFactory.InitStorage();
            storage = new Storage(azureAccountName, azureAccountKey);

            Guid userId = storage.User.Create("userThatExists", "userThatExists@gmail.com", new Byte[1]);
            accountId = storage.Account.Create(userId, "accountThatExists", "accountThatExistsDesc");
            storage.User.Create("otherUserThatExists", "otherUserThatExists@gmail.com", new Byte[1]);
            otherAccountId = storage.Account.Create(userId, "otherAccountThatExists", "otherAccountThatExistsDesc");
            listIdThatExists = storage.List.Create(accountId, "listThatExists", "Yeah", false);
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
            Guid idPersList = storage.List.GetPersonalList(accountId);
            storage.List.SetInfo(idPersList, "babar", "babar", false);
        }

        [Test]
        public void SetInfoNormalBehaviourDescription()
        {
            storage.List.SetInfo(listIdThatExists, "listThatExists", "Babar", false);
            Assert.AreEqual(storage.List.GetInfo(listIdThatExists).Description, "Babar");
        }

        [Test]
        public void SetInfoNormalBehaviourPrivacy()
        {
            storage.List.SetInfo(listIdThatExists, "listThatExists", "Yeah", true);
            Assert.IsTrue(storage.List.GetInfo(listIdThatExists).IsPrivate);

            storage.List.SetInfo(listIdThatExists,"listThatExists","Yeah",false);
            Assert.IsFalse(storage.List.GetInfo(listIdThatExists).IsPrivate);
        }

        [Test]
        public void SetInfoNormalBehaviourName()
        {
            storage.List.SetInfo(listIdThatExists, "newNameListThatExists", "Yeah", false);
            Assert.AreEqual(storage.List.GetInfo(listIdThatExists).Name, "newNameListThatExists");
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
            Assert.AreEqual(ownerId, accountId);
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
        [Ignore("Not implemented")]
        public void DeleteIsPersonnalList()
        {
            storage.List.Delete(storage.List.GetPersonalList(storage.Account.GetId("accountThatExists")));
        }

        [Test]
        [Ignore("Not implemented")]
        public void DeleteWithWrondId()
        {
            storage.List.Delete(new Guid());
        }

        [Test]
        [ExpectedException(typeof(ListNotFound))]
        [Ignore("Not implemented")]
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

        #region Hashset<Guid> GetAccounts(Guid listId)

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

        #region void SetMain(Guid listId, Guid accountId, bool isMain)

        [Test]
        [ExpectedException(typeof(ListNotFound))]
        public void SetMainListNotFound()
        {
            storage.List.SetMain(new Guid(),new Guid(), true);
        }

        [Test]
        public void SetMainNormalBehaviour()
        {
            Guid accountThatExistsId = storage.Account.GetId("accountThatExists");
            storage.List.SetMain(listIdThatExists, accountThatExistsId, true);
            Assert.IsTrue(storage.List.GetMainAccounts(listIdThatExists).Contains(accountThatExistsId));
        }

        #endregion

        #region Hashset<Guid> GetMainAccounts(Guid listId)

        [Test]
        [ExpectedException(typeof(ListNotFound))]
        public void GetMainAccountsListNotFound()
        {
            storage.List.GetMainAccounts(new Guid());
        }

        [Test]
        public void GetMainAccountsNormalBehaviour()
        {
            Guid accountThatExistsId = storage.Account.GetId("accountThatExists");
            storage.List.SetMain(listIdThatExists, accountThatExistsId, true);
            Assert.IsTrue(storage.List.GetMainAccounts(listIdThatExists).Contains(storage.Account.GetId("accountThatExists")));
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
            storage.List.Remove(listIdThatExists, accountId);
            HashSet<Guid> accounts = storage.List.GetAccounts(listIdThatExists);
            Assert.IsTrue(!accounts.Contains(accountId));
            storage.List.Add(listIdThatExists, accountId);
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
            HashSet<Guid> ownedLists = storage.List.GetAccountOwnedLists(accountId, false);
            Assert.AreEqual(ownedLists.Count(), 1);
            Assert.IsTrue(ownedLists.Contains(listIdThatExists));
        }

        [Test]
        public void GetAccounOwnedListsNormalBehaviourWithPrivates()
        {
            var privateListId = storage.List.Create(accountId, "private", "this list is private", true);
            
            HashSet<Guid> ownedLists = storage.List.GetAccountOwnedLists(accountId, true);
            Assert.AreEqual(ownedLists.Count(), 2);
            Assert.IsTrue(ownedLists.Contains(privateListId)); 
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
            HashSet<Guid> followedLists = storage.List.GetAccountFollowedLists(otherAccountId, false);
            Assert.AreEqual(followedLists.Count(), 1);
            Assert.IsTrue(followedLists.Contains(listIdThatExists));
        }

        [Test]
        public void GetAccountFollowedListsNormalBehaviourWithPrivate()
        {
            var privateListId = storage.List.Create(accountId, "privateList", "This list is private", true);
            storage.List.Follow(privateListId,otherAccountId);

            HashSet<Guid> followedLists = storage.List.GetAccountFollowedLists(otherAccountId, true);
            Assert.AreEqual(followedLists.Count(), 2);
            Assert.IsTrue(followedLists.Contains(listIdThatExists));
            Assert.IsTrue(followedLists.Contains(privateListId));
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
            HashSet<Guid> followingLists = storage.List.GetFollowingLists(accountId);
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
            Assert.IsTrue(followingAccounts.Contains(accountId));
        }
        #endregion
    }
}
