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
        const string azureAccountKey = "73IH549/+HNKuXjF8LcIT2ad4v+acch8gn2Gm0GN1w9tRHFZGTm/fFOr8BRPV/1gHBKJ/1hLeB5IMdNGHv4VIA==";
        private Guid _listId;
        private Guid _listIdPrivate;
        private Guid _userId;
        private Guid _accountId, _otherAccountId;
        private const string Hw = "Hello world!";
        private DateTime _date;

        [SetUp]
        public void InitStorage()
        {
            var blobFactory = new BlobFactory(azureAccountName, azureAccountKey);
            blobFactory.InitStorage();
            storage = new Storage(azureAccountName, azureAccountKey);

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
        public void PostNormalBehaviour()
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
        public void CopyNormalBehaviour()
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
        public void RemoveNormalBehaviour()
        {
            var messageId = storage.Msg.Post(_accountId, Hw);
            storage.Msg.Remove(messageId);
        }

        [Test]
        public void RemoveNormalBehaviourWithUnexistantMessage()
        {
            storage.Msg.Remove(new Guid());
        }

        #endregion

        #region List<IMessage> GetListsMsgFrom(HashSet<Guid> listsId, DateTime firstMsgDate, int msgNumber)

        [Test]
        public void GetListsMsgFromNormalBehaviour()
        {
            var lists = new HashSet<Guid> { _listId };
            var messages = storage.Msg.GetListsMsgFrom(lists, _date, 333);
            Assert.AreEqual(messages.Count(), 0);
        }

        [Test]    
        public void GetListsMsgFromNormalBehaviourIncludingPrivate()
        {
            var lists = new HashSet<Guid> { _listId, _listIdPrivate };
            var messages = storage.Msg.GetListsMsgFrom(lists, _date, 333);
            Assert.AreEqual(messages.Count(), 0);
        }

        [Test]
        public void GetListsMsgFromNormalBehaviourPrivate()
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
        public void GetListsMsgFromNormalBehaviourTestPost()
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
        public void GetListsMsgFromNormalBehaviourTestCopy()
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
        public void GetListsMsgFromNormalBehaviourOnDates()
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
        public void GetListsMsgFromNormalBehaviourOnNumbers()
        {
            var messageId = storage.Msg.Post(_accountId, Hw);
            storage.Msg.Copy(_otherAccountId, messageId);

            var lists = new HashSet<Guid> { storage.List.GetPersonalList(_otherAccountId), storage.List.GetPersonalList(_accountId) };
            var messages = storage.Msg.GetListsMsgFrom(lists, _date, 1); //There are 2 messages available
            Assert.AreEqual(messages.Count(), 1); //We asked one, we got one
        }

        [Test]
        //Checks that Remove and GetListsMsg work well altogether
        public void GetListsMsgFromNormalBehaviourTestRemove()
        {
            var messageId = storage.Msg.Post(_accountId, Hw);
            storage.Msg.Remove(messageId);

            var lists = new HashSet<Guid> { storage.List.GetPersonalList(_accountId) };
            var messages = storage.Msg.GetListsMsgFrom(lists, _date, 333);
            Assert.AreEqual(messages.Count(), 0);
        }


        [Test]
        //Copied message shouldn't be deleted
        public void GetListsMsgFromNormalBehaviourTestCopyDelete()
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
        public void GetListsMsgFromNormalBehaviourOnBigNumbers()
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
        public void GetListsMsgToNormalBehaviour()
        {
            var lists = new HashSet<Guid> { _listId };
            var messages = storage.Msg.GetListsMsgTo(lists, _date, 333);
            Assert.AreEqual(messages.Count(), 0);
        }

        [Test]
        public void GetListsMsgToNormalBehaviourIncludingPrivate()
        {
            var lists = new HashSet<Guid> { _listId, _listIdPrivate };
            var messages = storage.Msg.GetListsMsgTo(lists, _date, 333);
            Assert.AreEqual(messages.Count(), 0);
        }

        [Test]
        public void GetListsMsgToNormalBehaviourPrivate()
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
        public void GetListsMsgToNormalBehaviourTest1()
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
        public void GetListsMsgToNormalBehaviourTest2()
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
        public void GetListsMsgToNormalBehaviourOnDates()
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
        public void GetListsMsgToNormalBehaviourOnNumbers()
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
        public void GetListsMsgToNormalBehaviourTest3()
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
        public void GetListsMsgToNormalBehaviourTest4()
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
        public void GetListsMsgToNormalBehaviourOnBigNumbers()
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
        public void TagNormalBehaviour()
        {
            var messageId = storage.Msg.Post(_accountId, Hw);
            storage.Msg.Tag(_accountId, messageId);
        }

        [Test]
        public void TagNormalBehaviour2()
        {
            var messageId = storage.Msg.Post(_accountId, Hw);
            storage.Msg.Tag(_otherAccountId, messageId);
        }

        [Test]
        public void TagNormalBehaviourAlreadyTagged()
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
        public void UntagNormalBehaviour()
        {
            var messageId = storage.Msg.Post(_accountId, Hw);
            storage.Msg.Tag(_accountId, messageId);

            storage.Msg.Untag(_accountId,messageId);
        }

        [Test]
        public void UntagNormalBehaviour2()
        {
            var messageId = storage.Msg.Post(_accountId, Hw);
            storage.Msg.Tag(_otherAccountId, messageId);

            storage.Msg.Untag(_accountId, messageId);
        }

        [Test]
        public void UntagNormalBehaviourNotTagged()
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
        public void GetTaggedFromNormalBehaviour()
        {
            var messages = storage.Msg.GetTaggedFrom(_accountId, _date, 444);
            Assert.AreEqual(messages.Count(), 0);
        }

        [Test]
        public void GetTaggedFromNormalBehaviour2()
        {
            var messageId = storage.Msg.Post(_accountId, Hw);
            storage.Msg.Tag(_accountId,messageId);

            var messages = storage.Msg.GetTaggedFrom(_accountId, _date, 444);
            Assert.AreEqual(messages.Count(), 1);
            Assert.AreEqual(messages[0].Id, messageId);
        }

        [Test]
        public void GetTaggedFromNormalBehaviour3()
        {
            var messageId = storage.Msg.Post(_accountId, Hw);
            storage.Msg.Tag(_accountId, messageId);
            storage.Msg.Untag(_accountId,messageId);

            var messages = storage.Msg.GetTaggedFrom(_accountId, _date, 444);
            Assert.AreEqual(messages.Count(), 0);
        }

        [Test]
        public void GetTaggedFromNormalBehaviour4()
        {
            var messageId = storage.Msg.Post(_accountId, Hw);
            storage.Msg.Tag(_otherAccountId, messageId);

            var messages = storage.Msg.GetTaggedFrom(_otherAccountId, _date, 444);
            Assert.AreEqual(messages.Count(), 1);
            Assert.AreEqual(messages[0].Id, messageId);
        }

        [Test]
        public void GetTaggedFromNormalBehaviour45()
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
        public void GetTaggedFromNormalBehaviourOnDate()
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
        public void GetTaggedToNormalBehaviour()
        {
            var messages = storage.Msg.GetTaggedTo(_accountId, _date, 444);
            Assert.AreEqual(messages.Count(), 0);
        }

        [Test]
        public void GetTaggedToNormalBehaviour2()
        {
            var messageId = storage.Msg.Post(_accountId, Hw);
            storage.Msg.Tag(_accountId, messageId);
            _date = DateTime.Now;

            var messages = storage.Msg.GetTaggedTo(_accountId, _date, 444);
            Assert.AreEqual(messages.Count(), 1);
            Assert.AreEqual(messages[0].Id, messageId);
        }

        [Test]
        public void GetTaggedToNormalBehaviour3()
        {
            var messageId = storage.Msg.Post(_accountId, Hw);
            storage.Msg.Tag(_accountId, messageId);
            storage.Msg.Untag(_accountId, messageId);
            _date = DateTime.Now;

            var messages = storage.Msg.GetTaggedTo(_accountId, _date, 444);
            Assert.AreEqual(messages.Count(), 0);
        }

        [Test]
        public void GetTaggedToNormalBehaviour4()
        {
            var messageId = storage.Msg.Post(_accountId, Hw);
            storage.Msg.Tag(_otherAccountId, messageId);
            _date = DateTime.Now;

            var messages = storage.Msg.GetTaggedTo(_otherAccountId, _date, 444);
            Assert.AreEqual(messages.Count(), 1);
            Assert.AreEqual(messages[0].Id, messageId);
        }

        [Test]
        public void GetTaggedToNormalBehaviour45()
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
        public void GetTaggedToNormalBehaviourOnDate()
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

        #region Guid GetLastMessages(Guid accountId, string content)

        [Test]
        public void GetLastMessagesNormalBehaviour()
        {
            Guid id = storage.Msg.Post(_accountId, Hw);
            List<IMessage> lastmessages = storage.Msg.GetLastMessages();
            Assert.Contains(storage.Msg.GetMessage(id),lastmessages);
        }

        #endregion
    }        
}
