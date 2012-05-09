using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Tigwi.Storage.Library;

namespace StorageTest
{
    class MessageTest
    {
        // Use other informations!
        public IStorage storage;
        const string azureAccountName = "ulyssestorage";
        const string azureAccountKey = "fc2HTyfP0m2r3zlNYmMc3Pjvbfmy63ovoCP9Zkz0yoyuId3AeyrTswLcye2VDr3hzDvAQbdeKUlXBX3lFTcNWQ==";
        private Guid listId;
        private Guid listIdPrivate;

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
            listId = storage.List.Create(accountId, "listThatExists", "This list exists", false);
            listIdPrivate = storage.List.Create(accountId, "listThatExistsPrivate", "This list exists and is private", true);
        }

        #region List<IMessage> GetListsMsgFrom(HashSet<Guid> listsId, DateTime firstMsgDate, int msgNumber)

        [Test]
        public void GetListsMsgFromNormalBehavior()
        {
            var lists = new HashSet<Guid> {listId};
            var messages = storage.Msg.GetListsMsgFrom(lists, DateTime.Now, 333);
            Assert.AreEqual(messages, new List<Message>());
        }

        public void GetListsMsgFromNormalBehaviorIncludingPrivate()
        {
            var lists = new HashSet<Guid> {listId, listIdPrivate};
            var messages = storage.Msg.GetListsMsgFrom(lists, DateTime.Now, 333);
            Assert.AreEqual(messages, new List<Message>());
        }

        public void GetListsMsgFromNormalBehaviorPrivate()
        {
            var lists = new HashSet<Guid> {listIdPrivate};
            var messages = storage.Msg.GetListsMsgFrom(lists, DateTime.Now, 333);
            Assert.AreEqual(messages, new List<Message>());
        }

        [Test]
        [ExpectedException(typeof(ListNotFound))]
        public void GetListsMsgFromListsNotFound()
        {
            var lists = new HashSet<Guid> { new Guid() };
            var messages = storage.Msg.GetListsMsgFrom(lists, DateTime.Now, 333);
        }

        //TODO: tests on date that necessitate an initialisation => necessitate createMsg tested

        //TODO: test on number



        #endregion
    }
}
