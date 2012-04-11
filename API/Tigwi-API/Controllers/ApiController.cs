using System;
using System.IO;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Xml.Serialization;
using System.Linq;
using StorageLibrary;
using Tigwi_API.Models;

namespace Tigwi_API.Controllers
{
    public class ApiController : Controller
    {
        private static AccountList BuildAccountListFromAccountsHashSet (HashSet<Guid> hashAccounts, int size, IStorage storage )
        {
            var accountList = new List<Account>();
            for (var k = 0; k < size; k++)
            {
                var accountId = hashAccounts.First();
                var account = new Account(accountId, storage.Account.GetInfo(accountId).Name);
                accountList.Add(account);
                hashAccounts.Remove(accountId);
            }

            return new AccountList(accountList);  
        }
        
        //
        // GET: /accountmessages/{name}/{numberOfMessages}

        public ActionResult AccountMessages(string accountName, int numberOfMessages)
        {
            // TODO : give the actual connexion informations
            IStorage storage = new Storage("",""); // connexion
            ContentResult result;

            try
            {
                // get lasts messages from user name
                var accountId = storage.Account.GetId(accountName);
                var personalListId = storage.List.GetPersonalList(accountId);
                var listMsgs = storage.Msg.GetListsMsgTo(new HashSet<Guid> {personalListId}, DateTime.Now , numberOfMessages);

                // convert, looking forward XML serialization
                var listMsgsOutput = new MessageList(listMsgs);

                // a stream is needed for serialization
                var stream = new MemoryStream();

                (new XmlSerializer(typeof(MessageList))).Serialize(stream, listMsgsOutput);

                result = Content(stream.ToString());
            }
            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                var stream = new MemoryStream();
                (new XmlSerializer(typeof(Error))).Serialize(stream, new Error(exception.Code.ToString()));
                result = Content(stream.ToString());
            }

            return result;
        }

        //
        // GET : /accountsubscribers/{accountName}/{numberOfSubscribers}

        public ActionResult AccountSubscribersList(string accountName, int numberOfSubscribers)
        {
            IStorage storage = new Storage("", ""); // connexion
            ContentResult result;

            try
            {
                var accountId = storage.Account.GetId(accountName);

                // get lasts followers of user name 's list
                var followingLists = storage.List.GetFollowingLists(accountId);
                var hashFollowers = new HashSet<Guid>();
                foreach (var followingList in followingLists)
                {
                    hashFollowers.UnionWith(storage.List.GetFollowingAccounts(followingList));
                }

                // Get as many subscribers as possible (maximum: numberOfSubscibers)
                var size = Math.Min(hashFollowers.Count, numberOfSubscribers);
                var accountListToReturn = BuildAccountListFromAccountsHashSet(hashFollowers, size, storage);

                // a stream is needed for serialization
                var stream = new MemoryStream();

                (new XmlSerializer(typeof (AccountList))).Serialize(stream, accountListToReturn);

                result = Content(stream.ToString());
            }
            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                var stream = new MemoryStream();
                (new XmlSerializer(typeof(Error))).Serialize(stream, new Error(exception.Code.ToString()));
                result = Content(stream.ToString());
            }

            return result;
        }

        //
        // GET : /accountsubscriptions/{accountName}/{numberOfSubscriptions}

        public ActionResult AccountSubscriptionsList(string accountName, int numberOfSubscriptions)
        {
            IStorage storage = new Storage("", ""); // connexion
            ContentResult result;

            try
            {
                // get the public lists followed by the given account
                // TODO : if the user is authorized, get also the private lists
                var accountId = storage.Account.GetId(accountName);
                var followedLists = storage.List.GetAccountFollowedLists(accountId, false);
                var accountsInLists = new HashSet<Guid>();
                foreach (var followedList in followedLists)
                {
                    accountsInLists.UnionWith(storage.List.GetAccounts(followedList));
                }

                // Get as many subscriptions as possible (maximum: numberOfSubscriptions)
                var size = Math.Min(accountsInLists.Count, numberOfSubscriptions);
                var accountListToReturn = BuildAccountListFromAccountsHashSet(accountsInLists, size, storage);

                // a stream is needed for serialization
                var stream = new MemoryStream();

                (new XmlSerializer(typeof (AccountList))).Serialize(stream, accountListToReturn);

                result = Content(stream.ToString());
            }
            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                var stream = new MemoryStream();
                (new XmlSerializer(typeof(Error))).Serialize(stream, new Error(exception.Code.ToString()));
                result = Content(stream.ToString());
            }

            return result;
        }

        //
        // GET : accountsubscribedpubliclists/{accountName}/{numberOfLists}

        public ActionResult AccountSubscribedPublicLists(string accountName, int numberofLists)
        {
            //TODO: implement this
            throw new NotImplementedException();
        }

        //
        // GET : accountsubscribedlists/{accountName}/{numberOfLists}

        public ActionResult AccountSubscribedLists(string accountName, int numberOfLists)
        {
            //TODO: implement this
            throw new NotImplementedException();
        }

        //
        // GET : accountpubliclists/{accountName}/{numberOfLists}

        public ActionResult AccountPublicLists(string accountName, int numberOfList)
        {
            //TODO: implement this
            throw new NotImplementedException();
        }

        //
        // GET : accountlists/{accountName}/{numberOfLists}

        public ActionResult AccountLists(string accountName, int numberOfList)
        {
            //TODO: implement this
            throw new NotImplementedException();
        }

        //
        // GET : listsubscriptions/{idOfList}/{numberOfSubscriptions}

        public ActionResult ListSubscriptions(Guid idOfList, int numberOfSubscriptions)
        {
            //TODO: Implement this
            throw new NotImplementedException();
        }
        
        //
        // GET : listsubscribers/{idOfList}/{numberOfSubscribers}

        public ActionResult ListSubscribers(Guid idOfList, int numberOfSubscribers)
        {
            //TODO: Implement this
            throw new NotImplementedException();
        }

        //
        // GET : listowner/{idOfList}

        public ActionResult ListOwner(Guid idOfList)
        {
            //TODO: implement this
            throw new NotImplementedException();
        }

        //
        // GET : listtimeline/{idOfList}/{numberOfMessages}

        public ActionResult ListTimeline(Guid idOfList, int numberOfMessages)
        {
            //TODO: implement this
            throw new NotImplementedException();
        }
        
        //
        // POST : /write

        //[Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult WritePost(MsgToWrite msg)
        {
            IStorage storage = new Storage("", ""); // connexion

            ContentResult result;

            try
            {
                var accountId = storage.Account.GetId(msg.Account);

                storage.Msg.Post(accountId, msg.Message.Content);

                // Result is an empty error XML element
                var stream = new MemoryStream();
                (new XmlSerializer(typeof (Error))).Serialize(stream, new Error());
                result = Content(stream.ToString());
            }
            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                var stream = new MemoryStream();
                (new XmlSerializer(typeof(Error))).Serialize(stream, new Error(exception.Code.ToString()));
                result = Content(stream.ToString());
            }

            return result;
        }

        //
        // POST : /accountsuscribelist

        //[Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AccountSubscribeList(SubscribeList subscribe)
        {
            IStorage storage = new Storage("", ""); // connexion

            ContentResult result;

            try
            {
                var accountId = storage.Account.GetId(subscribe.Account);

                storage.List.Follow(subscribe.Subscription, accountId);

                // Result is an empty error XML element
                var stream = new MemoryStream();
                (new XmlSerializer(typeof(Error))).Serialize(stream, new Error());
                result = Content(stream.ToString());
            }
            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                var stream = new MemoryStream();
                (new XmlSerializer(typeof(Error))).Serialize(stream, new Error(exception.Code.ToString()));
                result = Content(stream.ToString());
            }
            return result;
        }

        //
        // POST /createlist

        //[Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateList(/*to be implemented)*/)
        {
            //TODO : implement this
            throw new NotImplementedException();
        }

        //
        // POST : listsubscribeaccount/

        //[Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ListSubscribeAccount(/*to be implemented*/)
        {
            //TODO: implement this
            throw new NotImplementedException();
        }


    }
}
