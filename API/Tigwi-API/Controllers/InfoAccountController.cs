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
            Answer output;

            try
            {
                // get lasts messages from user name
                var accountId = storage.Account.GetId(accountName);
                var personalListId = storage.List.GetPersonalList(accountId);
                var listMsgs = storage.Msg.GetListsMsgTo(new HashSet<Guid> { personalListId }, DateTime.Now, number);

                // convert, looking forward XML serialization
                var listMsgsOutput = new Messages(listMsgs, storage);
                output = new Answer(listMsgsOutput);
            }

            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                output = new Answer(new Error(exception.Code.ToString()));
            }

            // a stream is needed for serialization
            var stream = new MemoryStream();
            (new XmlSerializer(typeof(Answer))).Serialize(stream, output);

            return Content(stream.ToString());
        }

        //
        // GET : /infoaccount/subscriberaccounts/{accountName}/{number}

        public ActionResult SubscriberAccounts(string accountName, int number)
        {
            //TODO : use appropriate storage connexion
            IStorage storage = new StorageTmp(); // connexion
            Answer output;

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

                output = new Answer(accountListToReturn);
            }

            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                output = new Answer(new Error(exception.Code.ToString()));
            }

            // a stream is needed for serialization
            var stream = new MemoryStream();
            (new XmlSerializer(typeof(Answer))).Serialize(stream, output);

            return Content(stream.ToString());
        }

        private ContentResult SubscriptionsEitherPublicOrAll(string accountName, int numberOfSubscriptions, bool withPrivate)
        {
            //TODO : use appropriate storage connexion
            IStorage storage = new StorageTmp(); // connexion
            Answer output;

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

                output = new Answer(accountListToReturn);
            }

            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                output = new Answer(new Error(exception.Code.ToString()));
            }

            // a stream is needed for serialization
            var stream = new MemoryStream();
            (new XmlSerializer(typeof(Answer))).Serialize(stream, output);

            return Content(stream.ToString());
        }

        //
        // GET : /infoaccount/publiclysubscribedaccounts/{accountName}/{number}
        // or publicsubscriptions ??

        public ActionResult PubliclySubscribedAccounts(string accountName, int number)
        {
            return SubscriptionsEitherPublicOrAll(accountName, number, false);
        }


        //
        // GET : /infoaccount/subscribedaccounts/{accountName}/{number}
        // [Authorize]
        public ActionResult SubscribedAccounts(string accountName, int number)
        {
            return SubscriptionsEitherPublicOrAll(accountName, number, true);
        }


        
        private ActionResult SubscribedListsEitherPublicOrAll(string accountName, int numberofLists, bool withPrivate)
        {
            //TODO : use appropriate storage connexion
            IStorage storage = new StorageTmp(); // connexion
            Answer output;

            try
            {
                // get the public lists followed by the given account

                var accountId = storage.Account.GetId(accountName);
                var followedLists = storage.List.GetAccountFollowedLists(accountId, withPrivate);


                // Get as many subscriptions as possible (maximum: numberOfSubscriptions)
                var size = Math.Min(followedLists.Count, numberofLists);
                var listsToReturn = BuildListsFromGuidCollection(followedLists, size, storage);

                output = new Answer(listsToReturn);
            }

            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                output = new Answer(new Error(exception.Code.ToString()));
            }

            // a stream is needed for serialization
            var stream = new MemoryStream();
            (new XmlSerializer(typeof(Answer))).Serialize(stream, output);

            return Content(stream.ToString());
        }

        
        //
        // GET : /infoaccount/subscribedpubliclists/{accountName}/{number}
        // or publicsubscriptions but subcribedpublic isn't correct

        public ActionResult SubscribedPublicLists(string accountName, int number)
        {
            return SubscribedListsEitherPublicOrAll(accountName, number, false);
        }

        //
        // GET : /infoaccount/subscribedlists/{accountName}/{number}
        // or subscriptions but subscribed isn't correct

        //[Authorize]
        public ActionResult SubscribedLists(string accountName, int number)
        {
            return SubscribedListsEitherPublicOrAll(accountName, number, true);
        }

        //
        // GET : /infoaccount/subscribers/{accountName}/{number}
        // To clarify : subscriberlists ??

        public ActionResult Subscribers(string name, int numberOfSubscribers)
        {
            //TODO : use appropriate storage connexion
            IStorage storage = new StorageTmp(); // connexion
            Answer output;

            try
            {
                var accountId = storage.Account.GetId(name);

                // get lasts followers of user name 's list
                var followingLists = storage.List.GetFollowingLists(accountId);

                // Get as many subscribers as possible (maximum: number)
                var size = Math.Min(followingLists.Count, numberOfSubscribers);
                var accountListToReturn = BuildAccountListFromGuidCollection(followingLists, size, storage);

                output = new Answer(accountListToReturn);
            }

            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                output = new Answer(new Error(exception.Code.ToString()));
            }

            // a stream is needed for serialization
            var stream = new MemoryStream();
            (new XmlSerializer(typeof(Answer))).Serialize(stream, output);

            return Content(stream.ToString());
        }

        private ActionResult OwnedListsEitherPublicOrAll(string accountName, int numberOfLists, bool withPrivate)
        {
            //TODO : use appropriate storage connexion
            IStorage storage = new StorageTmp(); // connexion
            Answer output;

            try
            {
                // get the public lists owned by the given account

                var accountId = storage.Account.GetId(accountName);
                var ownedLists = storage.List.GetAccountOwnedLists(accountId, withPrivate);


                // Get as many subscriptions as possible (maximum: numberOfSubscriptions)
                var size = Math.Min(ownedLists.Count, numberOfLists);
                var listsToReturn = BuildListsFromGuidCollection(ownedLists, size, storage);

                output = new Answer(listsToReturn);
            }

            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                output = new Answer(new Error(exception.Code.ToString()));
            }

            // a stream is needed for serialization
            var stream = new MemoryStream();
            (new XmlSerializer(typeof(Answer))).Serialize(stream, output);

            return Content(stream.ToString());
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
