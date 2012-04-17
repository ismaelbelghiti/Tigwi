using System;
using System.IO;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Xml.Serialization;
using StorageLibrary;
using Tigwi_API.Models;

namespace Tigwi_API.Controllers
{
    public class InfoAccountController : ApiController
    {

        //
        // GET: /infoaccount/messages/{accountName}/{number}

        public ActionResult Messages(string accountName, int number)
        {

            // TODO : give the actual connexion informations
            IStorage storage = new StorageTmp(); // connexion
            ContentResult result;

            try
            {
                // get lasts messages from user name
                var accountId = storage.Account.GetId(accountName);
                var personalListId = storage.List.GetPersonalList(accountId);
                var listMsgs = storage.Msg.GetListsMsgTo(new HashSet<Guid> { personalListId }, DateTime.Now, number);

                // convert, looking forward XML serialization
                var listMsgsOutput = new Messages(listMsgs, storage);

                // a stream is needed for serialization
                var stream = new MemoryStream();

                (new XmlSerializer(typeof(Messages))).Serialize(stream, listMsgsOutput);

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
        // GET : /infoaccount/subscribers/{accountName}/{number}

        public ActionResult SubscribersAccounts(string accountName, int number)
        {

            IStorage storage = new StorageTmp(); // connexion
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

                // Get as many subscribers as possible (maximum: number)
                var size = Math.Min(hashFollowers.Count, number);
                var accountListToReturn = BuildAccountListFromGuidCollection(hashFollowers, size, storage);

                // a stream is needed for serialization
                var stream = new MemoryStream();

                (new XmlSerializer(typeof(Accounts))).Serialize(stream, accountListToReturn);

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

        private ContentResult SubscriptionsEitherPublicOrAll(string accountName, int numberOfSubscriptions, bool withPrivate)
        {

            IStorage storage = new StorageTmp(); // connexion
            ContentResult result;

            try
            {
                // get the public lists followed by the given account

                var accountId = storage.Account.GetId(accountName);
                var followedLists = storage.List.GetAccountFollowedLists(accountId, withPrivate);
                var accountsInLists = new HashSet<Guid>();
                foreach (var followedList in followedLists)
                {
                    accountsInLists.UnionWith(storage.List.GetAccounts(followedList));
                }

                // Get as many subscriptions as possible (maximum: numberOfSubscriptions)
                var size = Math.Min(accountsInLists.Count, numberOfSubscriptions);
                var accountListToReturn = BuildAccountListFromGuidCollection(accountsInLists, size, storage);

                // a stream is needed for serialization
                var stream = new MemoryStream();

                (new XmlSerializer(typeof(Accounts))).Serialize(stream, accountListToReturn);

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
        // GET : /infoaccount/publicsubscriptionsaccounts/{accountName}/{number}

        public ActionResult PublicSubscriptionsAccounts(string accountName, int number)
        {
            return SubscriptionsEitherPublicOrAll(accountName, number, false);
        }


        //
        // GET : /infoaccount/subscriptionaccounts/{accountName}/{number}
        // [Authorize]
        public ActionResult SubscriptionsAccounts(string accountName, int number)
        {
            return SubscriptionsEitherPublicOrAll(accountName, number, true);
        }


        
        private ActionResult SubscribedListsEitherPublicOrAll(string accountName, int numberofLists, bool withPrivate)
        {
            IStorage storage = new StorageTmp(); // connexion
            ContentResult result;

            try
            {
                // get the public lists followed by the given account

                var accountId = storage.Account.GetId(accountName);
                var followedLists = storage.List.GetAccountFollowedLists(accountId, withPrivate);


                // Get as many subscriptions as possible (maximum: numberOfSubscriptions)
                var size = Math.Min(followedLists.Count, numberofLists);
                var listsToReturn = BuildListsFromGuidCollection(followedLists, size, storage);

                // a stream is needed for serialization
                var stream = new MemoryStream();

                (new XmlSerializer(typeof(Accounts))).Serialize(stream, listsToReturn);

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
        // GET : /infoaccount/subscribedpubliclists/{accountName}/{number}

        public ActionResult SubscribedPublic(string accountName, int number)
        {
            return SubscribedListsEitherPublicOrAll(accountName, number, false);
        }

        //
        // GET : /infoaccount/subscribedlists/{accountName}/{number}

        //[Authorize]
        public ActionResult Subscribed(string accountName, int number)
        {
            return SubscribedListsEitherPublicOrAll(accountName, number, true);
        }

        //
        //Get : /infoaccount/subscribers/{accountName}/{number}

        public ActionResult Subscribers(string name, int numberOfSubscribers)
        {
            IStorage storage = new StorageTmp(); // connexion
            ContentResult result;

            try
            {
                var accountId = storage.Account.GetId(name);

                // get lasts followers of user name 's list
                var followingLists = storage.List.GetFollowingLists(accountId);

                // Get as many subscribers as possible (maximum: number)
                var size = Math.Min(followingLists.Count, numberOfSubscribers);
                var accountListToReturn = BuildAccountListFromGuidCollection(followingLists, size, storage);

                // a stream is needed for serialization
                var stream = new MemoryStream();

                (new XmlSerializer(typeof(Accounts))).Serialize(stream, accountListToReturn);

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

        private ActionResult OwnedListsEitherPublicOrAll(string accountName, int numberOfLists, bool withPrivate)
        {
            IStorage storage = new StorageTmp(); // connexion
            ContentResult result;

            try
            {
                // get the public lists owned by the given account

                var accountId = storage.Account.GetId(accountName);
                var ownedLists = storage.List.GetAccountOwnedLists(accountId, withPrivate);


                // Get as many subscriptions as possible (maximum: numberOfSubscriptions)
                var size = Math.Min(ownedLists.Count, numberOfLists);
                var listsToReturn = BuildListsFromGuidCollection(ownedLists, size, storage);

                // a stream is needed for serialization
                var stream = new MemoryStream();

                (new XmlSerializer(typeof(Accounts))).Serialize(stream, listsToReturn);

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
        // GET : infoaccount/ownedpubliclists/{accountName}/{number}

        public ActionResult OwnedPublicLists(string accountName, int number)
        {
            return OwnedListsEitherPublicOrAll(accountName, number, false);
        }

        //
        // GET : /infoaccount/ownedlists/{accountName}/{number}

        //[Authorize]
        public ActionResult OwnedLists(string accountName, int number)
        {
            return OwnedListsEitherPublicOrAll(accountName, number, true);
        }

    }
}
