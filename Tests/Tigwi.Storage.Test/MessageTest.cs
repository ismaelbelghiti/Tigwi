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
        private Guid _listId;
        private Guid _listIdPrivate;
        private Guid _userId;
        private Guid _accountId, _otherAccountId;
        private const string Hw = "Hello world!";
        private DateTime _date;

        [SetUp]
        public void InitStorage()
        {
            bool UseStorageTmp = false;
            if (UseStorageTmp)
                storage = new StorageTmp();
            else
            {
                var blobFactory = new BlobFactory(azureAccountName, azureAccountKey);
                blobFactory.InitStorage();
                storage = new Storage(azureAccountName, azureAccountKey);
            }

            _userId = storage.User.Create("userThatExists", "userThatExists@gmail.com", new Byte[1]);
            _accountId = storage.Account.Create(_userId, "accountThatExists", "accountThatExistsDesc");
            storage.User.Create("otherUserThatExists", "otherUserThatExists@gmail.com", new Byte[1]);
            _otherAccountId = storage.Account.Create(_userId, "otherAccountThatExists", "otherAccountThatExistsDesc");
            _listId = storage.List.Create(_accountId, "listThatExists", "This list exists", false);
            _listIdPrivate = storage.List.Create(_accountId, "listThatExistsPrivate", "This list exists and is private", true);
            _date = DateTime.Now;
        }

        #region Guid Post(Guid accountId, string content)

        [Test]
        public void PostNormalBehavior()
        {
            storage.Msg.Post(_accountId, Hw);
        }

        [Test]
        [ExpectedException(typeof(AccountNotFound))]
        public void PostAccountNotFound()
        {
            storage.Msg.Post(new Guid(), "will never be posted");
        }

        //TODO: use correct exception when documented
        /*
        [Test]
        [ExpectedException(typeof(MessageTooLong))]
        public void PostMessageTooLong()
        {
            var msgTooLong =
                "I have a dream that one day this nation will rise up and live out the true meaning of its creed. We hold these truths to be self-evident that all men are created equal. I have a dream that one day out in the red hills of Georgia the sons of former slaves and the sons of former slave owners will be able to sit down together at the table of brotherhood. I have a dream that one day even the state of Mississippi, a state sweltering with the heat of oppression, will be transformed into an oasis of freedom and justice.";
            storage.Msg.Post(accountId, msgTooLong);
        }
        */

        //TODO: necessary?
        /*
        [Test]
        [ExpectedException(typeof(EmptyMessage))]
        public void PostAccountEmptyMEssage()
        {
            storage.Msg.Post(new Guid(), "");
        }
        */

        #endregion

        #region Guid Copy(Guid accountId, Guid msgId);

        [Test]
        public void CopyNormalBehavior()
        {
            var messageId = storage.Msg.Post(_accountId, Hw);
            storage.Msg.Copy(_otherAccountId, messageId);
        }

        [Test]
        [ExpectedException(typeof(AccountNotFound))]
        public void CopyAccountNotFound()
        {
            var messageId = storage.Msg.Post(_accountId, Hw);
            storage.Msg.Copy(new Guid(), messageId);
        }

        [Test]
        [ExpectedException(typeof(MessageNotFound))]
        public void CopyMessageNotFound()
        {
            storage.Msg.Copy(_otherAccountId, new Guid());
        }

        #endregion

        #region void Remove(Guid id);

        [Test]
        public void RemoveNormalBehavior()
        {
            var messageId = storage.Msg.Post(_accountId, Hw);
            storage.Msg.Remove(messageId);
        }

        [Test]
        public void RemoveNormalBehaviorWithUnexistantMessage()
        {
            storage.Msg.Remove(new Guid());
        }

        #endregion

        #region List<IMessage> GetListsMsgFrom(HashSet<Guid> listsId, DateTime firstMsgDate, int msgNumber)

        [Test]
        public void GetListsMsgFromNormalBehavior()
        {
            var lists = new HashSet<Guid> { _listId };
            var messages = storage.Msg.GetListsMsgFrom(lists, _date, 333);
            Assert.AreEqual(messages.Count(), 0);
        }

        [Test]    
        public void GetListsMsgFromNormalBehaviorIncludingPrivate()
        {
            var lists = new HashSet<Guid> { _listId, _listIdPrivate };
            var messages = storage.Msg.GetListsMsgFrom(lists, _date, 333);
            Assert.AreEqual(messages.Count(), 0);
        }

        [Test]
        public void GetListsMsgFromNormalBehaviorPrivate()
        {
            var lists = new HashSet<Guid> { _listIdPrivate };
            var messages = storage.Msg.GetListsMsgFrom(lists, _date, 333);
            Assert.AreEqual(messages.Count(), 0);
        }

        [Test]
        [ExpectedException(typeof(ListNotFound))]
        public void GetListsMsgFromListsNotFound()
        {
            var lists = new HashSet<Guid> { new Guid() };
            storage.Msg.GetListsMsgFrom(lists, _date, 333);
        }

        //TODO: use correct exception when documented
        /*
        [Test]
        [ExpectedException(typeof(WrongNumber))]
        public void GetListsFromWrongNumber()
        {
            var lists = new HashSet<Guid> { listId };
            storage.Msg.GetListsMsgFrom(lists, date, -1);
        }
        */

        [Test]
        //Checks that GetListsMsg and Post work well altogether
        public void GetListsMsgFromNormalBehaviorTestPost()
        {
            var messageId = storage.Msg.Post(_accountId, Hw);

            var lists = new HashSet<Guid> { storage.List.GetPersonalList(_accountId) };
            var messages = storage.Msg.GetListsMsgFrom(lists, _date, 333);
            Assert.AreEqual(messages.Count(), 1);

            var message = messages[0];
            Assert.AreEqual(message.Id, messageId);
            Assert.AreEqual(message.Content, Hw);
        }


        [Test]
        //Checks that GetListsMsg and Copy work well altogether
        public void GetListsMsgFromNormalBehaviorTestCopy()
        {
            var messageId = storage.Msg.Post(_accountId, Hw);
            storage.Msg.Copy(_otherAccountId, messageId);

            var lists = new HashSet<Guid> { storage.List.GetPersonalList(_otherAccountId), storage.List.GetPersonalList(_accountId) };
            var messages = storage.Msg.GetListsMsgFrom(lists, _date, 333);
            Assert.AreEqual(messages.Count(), 2);

            var message1 = messages[0];
            var message2 = messages[1];

            Assert.AreEqual(message1.Content, message2.Content);
            Assert.AreNotEqual(message1.Id, message2.Id);
        }

        [Test]
        public void GetListsMsgFromNormalBehaviorOnDates()
        {
            var messageId = storage.Msg.Post(_accountId, Hw);
            _date = DateTime.Now;
            var copiedMessageId = storage.Msg.Copy(_otherAccountId, messageId);

            var lists = new HashSet<Guid> { storage.List.GetPersonalList(_otherAccountId), storage.List.GetPersonalList(_accountId) };

            var messages = storage.Msg.GetListsMsgFrom(lists, _date, 333);
            Assert.AreEqual(messages.Count(), 1);
            Assert.AreEqual(messages[0].Id, copiedMessageId);
        }

        [Test]
        public void GetListsMsgFromNormalBehaviorOnNumbers()
        {
            var messageId = storage.Msg.Post(_accountId, Hw);
            storage.Msg.Copy(_otherAccountId, messageId);

            var lists = new HashSet<Guid> { storage.List.GetPersonalList(_otherAccountId), storage.List.GetPersonalList(_accountId) };
            var messages = storage.Msg.GetListsMsgFrom(lists, _date, 1); //There are 2 messages available
            Assert.AreEqual(messages.Count(), 1); //We asked one, we got one
        }

        [Test]
        //Checks that Remove and GetListsMsg work well altogether
        public void GetListsMsgFromNormalBehaviorTestRemove()
        {
            var messageId = storage.Msg.Post(_accountId, Hw);
            storage.Msg.Remove(messageId);

            var lists = new HashSet<Guid> { storage.List.GetPersonalList(_accountId) };
            var messages = storage.Msg.GetListsMsgFrom(lists, _date, 333);
            Assert.AreEqual(messages.Count(), 0);
        }


        [Test]
        //Copied message shouldn't be deleted
        public void GetListsMsgFromNormalBehaviorTestCopyDelete()
        {
            var messageId = storage.Msg.Post(_accountId, Hw);
            var copiedMessageId = storage.Msg.Copy(_otherAccountId, messageId);

            var lists = new HashSet<Guid> { storage.List.GetPersonalList(_otherAccountId) };
            var messages = storage.Msg.GetListsMsgFrom(lists, _date, 333);
            Assert.AreEqual(messages.Count(), 1);
            Assert.AreEqual(messages[0].Id, copiedMessageId);
        }


        [Test]
        //Big test on numbers
        public void GetListsMsgFromNormalBehaviorOnBigNumbers()
        {
            int k;
            for (k = 0; k < 600; k++)
            {
                storage.Msg.Post(_accountId, Hw);
            }

            var lists = new HashSet<Guid> { storage.List.GetPersonalList(_accountId) };
            var messages = storage.Msg.GetListsMsgFrom(lists, _date, 333);
            Assert.AreEqual(messages.Count(), 333);
            foreach (var message in messages)
            {
                Assert.LessOrEqual(_date.CompareTo(message.Date), 0);
            }
        }

        #endregion

        #region List<IMessage> GetListsMsgTo(HashSet<Guid> listsId, DateTime firstMsgDate, int msgNumber)

        [Test]
        public void GetListsMsgToNormalBehavior()
        {
            var lists = new HashSet<Guid> { _listId };
            var messages = storage.Msg.GetListsMsgTo(lists, _date, 333);
            Assert.AreEqual(messages.Count(), 0);
        }

        [Test]
        public void GetListsMsgToNormalBehaviorIncludingPrivate()
        {
            var lists = new HashSet<Guid> { _listId, _listIdPrivate };
            var messages = storage.Msg.GetListsMsgTo(lists, _date, 333);
            Assert.AreEqual(messages.Count(), 0);
        }

        [Test]
        public void GetListsMsgToNormalBehaviorPrivate()
        {
            var lists = new HashSet<Guid> { _listIdPrivate };
            var messages = storage.Msg.GetListsMsgTo(lists, _date, 333);
            Assert.AreEqual(messages.Count(), 0);
        }

        [Test]
        [ExpectedException(typeof(ListNotFound))]
        public void GetListsMsgToListsNotFound()
        {
            var lists = new HashSet<Guid> { new Guid() };
            storage.Msg.GetListsMsgTo(lists, _date, 333);
        }

        //TODO: use correct exception when documented
        /*
        [Test]
        [ExpectedException(typeof(WrongNumber))]
        public void GetListsToWrongNumber()
        {
            var lists = new HashSet<Guid> { listId };
            storage.Msg.GetListsMsgTo(lists, date, -1);
        }
        */

        [Test]
        //Checks that GetListsMsg and Post work well altogether
        public void GetListsMsgToNormalBehaviorTest1()
        {
            var messageId = storage.Msg.Post(_accountId, Hw);
            _date = DateTime.Now;

            var lists = new HashSet<Guid> { storage.List.GetPersonalList(_accountId) };
            var messages = storage.Msg.GetListsMsgTo(lists, _date, 333);
            Assert.AreEqual(messages.Count(), 1);

            var message = messages[0];
            Assert.AreEqual(message.Id, messageId);
            Assert.AreEqual(message.Content, Hw);
        }


        [Test]
        //Checks that GetListsMsg and copy work well altogether
        public void GetListsMsgToNormalBehaviorTest2()
        {
            var messageId = storage.Msg.Post(_accountId, Hw);
            storage.Msg.Copy(_otherAccountId, messageId);
            _date = DateTime.Now;

            var lists = new HashSet<Guid> { storage.List.GetPersonalList(_otherAccountId), storage.List.GetPersonalList(_accountId) };
            var messages = storage.Msg.GetListsMsgTo(lists, _date, 333);
            Assert.AreEqual(messages.Count(), 2);

            var message1 = messages[0];
            var message2 = messages[1];

            Assert.AreEqual(message1.Content, message2.Content);
            Assert.AreNotEqual(message1.Id, message2.Id);
        }

        [Test]
        public void GetListsMsgToNormalBehaviorOnDates()
        {
            var messageId = storage.Msg.Post(_accountId, Hw);
            _date = DateTime.Now;
            storage.Msg.Copy(_otherAccountId, messageId);

            var lists = new HashSet<Guid> { storage.List.GetPersonalList(_otherAccountId), storage.List.GetPersonalList(_accountId) };

            var messages = storage.Msg.GetListsMsgTo(lists, _date, 333);
            Assert.AreEqual(messages.Count(), 1);
            Assert.AreEqual(messages[0].Id, messageId);
        }

        [Test]
        public void GetListsMsgToNormalBehaviorOnNumbers()
        {
            var messageId = storage.Msg.Post(_accountId, Hw);
            storage.Msg.Copy(_otherAccountId, messageId);
            _date = DateTime.Now;

            var lists = new HashSet<Guid> { storage.List.GetPersonalList(_otherAccountId), storage.List.GetPersonalList(_accountId) };
            var messages = storage.Msg.GetListsMsgTo(lists, _date, 1); //There are 2 messages available
            Assert.AreEqual(messages.Count(), 1); //We asked one, we got one
        }

        [Test]
        //Checks that remove and GetListsMsg work well altogether
        public void GetListsMsgToNormalBehaviorTest3()
        {
            var messageId = storage.Msg.Post(_accountId, Hw);
            storage.Msg.Remove(messageId);
            _date = DateTime.Now;

            var lists = new HashSet<Guid> { storage.List.GetPersonalList(_accountId) };
            var messages = storage.Msg.GetListsMsgTo(lists, _date, 333);
            Assert.AreEqual(messages.Count(), 0);
        }


        [Test]
        //Copied message shouldn't be deleted
        public void GetListsMsgToNormalBehaviorTest4()
        {
            var messageId = storage.Msg.Post(_accountId, Hw);
            var copiedMessageId = storage.Msg.Copy(_otherAccountId, messageId);
            _date = DateTime.Now;

            var lists = new HashSet<Guid> { storage.List.GetPersonalList(_otherAccountId) };
            var messages = storage.Msg.GetListsMsgTo(lists, _date, 333);
            Assert.AreEqual(messages.Count(), 1);
            Assert.AreEqual(messages[0].Id, copiedMessageId);
        }


        [Test]
        //Big test on numbers
        public void GetListsMsgToNormalBehaviorOnBigNumbers()
        {
            int k;
            for (k = 0; k < 600; k++)
            {
                storage.Msg.Post(_accountId, Hw);
            }
            _date = DateTime.Now;

            var lists = new HashSet<Guid> { storage.List.GetPersonalList(_accountId) };
            var messages = storage.Msg.GetListsMsgTo(lists, _date, 333);
            Assert.AreEqual(messages.Count(), 333);
            foreach (var message in messages)
            {
                Assert.GreaterOrEqual(_date.CompareTo(message.Date), 0);
            }
        }

        #endregion

        #region void Tag(Guid accountId, Guid msgId);

        [Test]
        public void TagNormalBehavior()
        {
            var messageId = storage.Msg.Post(_accountId, Hw);
            storage.Msg.Tag(_accountId, messageId);
        }

        [Test]
        public void TagNormalBehavior2()
        {
            var messageId = storage.Msg.Post(_accountId, Hw);
            storage.Msg.Tag(_otherAccountId, messageId);
        }

        [Test]
        public void TagNormalBehaviorAlreadyTagged()
        {
            var messageId = storage.Msg.Post(_accountId, Hw);
            storage.Msg.Tag(_accountId,messageId);

            storage.Msg.Tag(_accountId, messageId);
        }

        [Test]
        [ExpectedException(typeof(AccountNotFound))]
        public void TagAccountNotFound()
        {
            var messageId = storage.Msg.Post(_accountId, Hw);
            storage.Msg.Tag(new Guid(), messageId);
        }

        [Test]
        [ExpectedException(typeof(MessageNotFound))]
        public void TagMessageNotFound()
        {
            storage.Msg.Tag(_accountId, new Guid());
        }

        #endregion

        #region void Untag(Guid accountId, Guid msgId);

        [Test]
        public void UntagNormalBehavior()
        {
            var messageId = storage.Msg.Post(_accountId, Hw);
            storage.Msg.Tag(_accountId, messageId);

            storage.Msg.Untag(_accountId,messageId);
        }

        [Test]
        public void UntagNormalBehavior2()
        {
            var messageId = storage.Msg.Post(_accountId, Hw);
            storage.Msg.Tag(_otherAccountId, messageId);

            storage.Msg.Untag(_accountId, messageId);
        }

        [Test]
        public void UntagNormalBehaviorNotTagged()
        {
            var messageId = storage.Msg.Post(_accountId, Hw);
            storage.Msg.Untag(_accountId, messageId);
        }

        [Test]
        public void UntagAccountNotFound()
        {
            var messageId = storage.Msg.Post(_accountId, Hw);
            storage.Msg.Tag(_accountId, messageId);

            storage.Msg.Untag(new Guid(), messageId);
        }

        [Test]
        public void UntagMessageNotFound()
        {
            storage.Msg.Untag(_accountId, new Guid());
        }

        #endregion

        #region List<IMessage> GetTaggedFrom(Guid accoundId, DateTime firstMsgDate, int msgNumber);

        [Test]
        public void GetTaggedFromNormalBehavior()
        {
            var messages = storage.Msg.GetTaggedFrom(_accountId, _date, 444);
            Assert.AreEqual(messages.Count(), 0);
        }

        [Test]
        public void GetTaggedFromNormalBehavior2()
        {
            var messageId = storage.Msg.Post(_accountId, Hw);
            storage.Msg.Tag(_accountId,messageId);

            var messages = storage.Msg.GetTaggedFrom(_accountId, _date, 444);
            Assert.AreEqual(messages.Count(), 1);
            Assert.AreEqual(messages[0].Id, messageId);
        }

        [Test]
        public void GetTaggedFromNormalBehavior3()
        {
            var messageId = storage.Msg.Post(_accountId, Hw);
            storage.Msg.Tag(_accountId, messageId);
            storage.Msg.Untag(_accountId,messageId);

            var messages = storage.Msg.GetTaggedFrom(_accountId, _date, 444);
            Assert.AreEqual(messages.Count(), 0);
        }

        [Test]
        public void GetTaggedFromNormalBehavior4()
        {
            var messageId = storage.Msg.Post(_accountId, Hw);
            storage.Msg.Tag(_otherAccountId, messageId);

            var messages = storage.Msg.GetTaggedFrom(_otherAccountId, _date, 444);
            Assert.AreEqual(messages.Count(), 1);
            Assert.AreEqual(messages[0].Id, messageId);
        }

        [Test]
        public void GetTaggedFromNormalBehavior45()
        {
            var messageId = storage.Msg.Post(_accountId, Hw);
            var messageId2 = storage.Msg.Post(_otherAccountId, Hw);
            storage.Msg.Tag(_otherAccountId, messageId);
            storage.Msg.Tag(_otherAccountId, messageId2);

            var messages = storage.Msg.GetTaggedFrom(_otherAccountId, _date, 444);
            Assert.AreEqual(messages.Count(), 2);
        }

        [Test]
        [ExpectedException(typeof(AccountNotFound))]
        public void GetTaggedFromAccountNotFound()
        {
            storage.Msg.GetTaggedFrom(new Guid(), _date, 444);
        }

        [Test]
        public void GetTaggedFromNormalBehaviorOnDate()
        {
            var messageId = storage.Msg.Post(_accountId, Hw);
            _date = DateTime.Now;
            var messageId2 = storage.Msg.Post(_accountId, Hw);
            storage.Msg.Tag(_accountId, messageId);
            storage.Msg.Tag(_accountId,messageId2);

            var messages = storage.Msg.GetTaggedFrom(_accountId, _date, 444);
            Assert.AreEqual(messages.Count(), 1);
            Assert.AreEqual(messages[0].Id, messageId2);
        }

        #endregion

        #region List<IMessage> GetTaggedTo(Guid accoundId, DateTime firstMsgDate, int msgNumber);

        [Test]
        public void GetTaggedToNormalBehavior()
        {
            var messages = storage.Msg.GetTaggedTo(_accountId, _date, 444);
            Assert.AreEqual(messages.Count(), 0);
        }

        [Test]
        public void GetTaggedToNormalBehavior2()
        {
            var messageId = storage.Msg.Post(_accountId, Hw);
            storage.Msg.Tag(_accountId, messageId);
            _date = DateTime.Now;

            var messages = storage.Msg.GetTaggedTo(_accountId, _date, 444);
            Assert.AreEqual(messages.Count(), 1);
            Assert.AreEqual(messages[0].Id, messageId);
        }

        [Test]
        public void GetTaggedToNormalBehavior3()
        {
            var messageId = storage.Msg.Post(_accountId, Hw);
            storage.Msg.Tag(_accountId, messageId);
            storage.Msg.Untag(_accountId, messageId);
            _date = DateTime.Now;

            var messages = storage.Msg.GetTaggedTo(_accountId, _date, 444);
            Assert.AreEqual(messages.Count(), 0);
        }

        [Test]
        public void GetTaggedToNormalBehavior4()
        {
            var messageId = storage.Msg.Post(_accountId, Hw);
            storage.Msg.Tag(_otherAccountId, messageId);
            _date = DateTime.Now;

            var messages = storage.Msg.GetTaggedTo(_otherAccountId, _date, 444);
            Assert.AreEqual(messages.Count(), 1);
            Assert.AreEqual(messages[0].Id, messageId);
        }

        [Test]
        public void GetTaggedToNormalBehavior45()
        {
            var messageId = storage.Msg.Post(_accountId, Hw);
            var messageId2 = storage.Msg.Post(_otherAccountId, Hw);
            storage.Msg.Tag(_otherAccountId, messageId);
            storage.Msg.Tag(_otherAccountId, messageId2);
            _date = DateTime.Now;

            var messages = storage.Msg.GetTaggedTo(_otherAccountId, _date, 444);
            Assert.AreEqual(messages.Count(), 2);
        }

        [Test]
        [ExpectedException(typeof(AccountNotFound))]
        public void GetTaggedToAccountNotFound()
        {
            storage.Msg.GetTaggedTo(new Guid(), _date, 444);
        }

        [Test]
        public void GetTaggedToNormalBehaviorOnDate()
        {
            var messageId = storage.Msg.Post(_accountId, Hw);
            _date = DateTime.Now;
            var messageId2 = storage.Msg.Post(_accountId, Hw);
            storage.Msg.Tag(_accountId, messageId);
            storage.Msg.Tag(_accountId, messageId2);

            var messages = storage.Msg.GetTaggedTo(_accountId, _date, 444);
            Assert.AreEqual(messages.Count(), 1);
            Assert.AreEqual(messages[0].Id, messageId);
        }

        #endregion
    }        
}
