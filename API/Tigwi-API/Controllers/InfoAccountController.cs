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
            Answer output;

            try
            {
                // get lasts messages from account accoutName
                var accountId = Storage.Account.GetId(accountName);
                var personalListId = Storage.List.GetPersonalList(accountId);
                var listMsgs = Storage.Msg.GetListsMsgTo(new HashSet<Guid> { personalListId }, DateTime.Now, number);

                // convert, looking forward XML serialization
                var listMsgsOutput = new Messages(listMsgs, Storage);
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
        // GET: /infoaccount/messages/{accountId}/{number}
        public ActionResult Messages(Guid accountId, int number)
        {
            Answer output;

            try
            {
                // get lasts messages from account accoutName
                var personalListId = Storage.List.GetPersonalList(accountId);
                var listMsgs = Storage.Msg.GetListsMsgTo(new HashSet<Guid> { personalListId }, DateTime.Now, number);

                // convert, looking forward XML serialization
                var listMsgsOutput = new Messages(listMsgs, Storage);
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
        // GET: /infoaccount/taggedmessages/{accountName}/{number}
        public ActionResult TaggedMessages(string accountName, int number)
        {
            Answer output;

            try
            {
                // get lasts tagged messages from account accountName
                var accountId = Storage.Account.GetId(accountName);
                var listMsgs = Storage.Msg.GetTaggedTo(accountId, DateTime.Now, number);

                // convert, looking forward XML serialization
                var listMsgsOutput = new Messages(listMsgs, Storage);
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
        // GET: /infoaccount/taggedmessages/{accountId}/{number}
        public ActionResult TaggedMessages(Guid accountId, int number)
        {
            Answer output;

            try
            {
                // get lasts messages from user name
                var listMsgs = Storage.Msg.GetTaggedTo(accountId, DateTime.Now, number);

                // convert, looking forward XML serialization
                var listMsgsOutput = new Messages(listMsgs, Storage);
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
            Answer output;

            try
            {
                var accountId = Storage.Account.GetId(accountName);

                // get lasts followers of user name 's list
                var followingLists = Storage.List.GetFollowingLists(accountId);
                var hashFollowers = new HashSet<Guid>();
                foreach (var followingList in followingLists)
                {
                    hashFollowers.UnionWith(Storage.List.GetFollowingAccounts(followingList));
                }

                // Get as many subscribers as possible (maximum: number)
                var size = Math.Min(hashFollowers.Count, number);
                var accountListToReturn = BuildAccountListFromGuidCollection(hashFollowers, size, Storage);

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
        // GET : /infoaccount/subscriberaccounts/{accountId}/{number}
        public ActionResult SubscriberAccounts(Guid accountId, int number)
        {
            Answer output;

            try
            {
                // get lasts followers of user name 's list
                var followingLists = Storage.List.GetFollowingLists(accountId);
                var hashFollowers = new HashSet<Guid>();
                foreach (var followingList in followingLists)
                {
                    hashFollowers.UnionWith(Storage.List.GetFollowingAccounts(followingList));
                }

                // Get as many subscribers as possible (maximum: number)
                var size = Math.Min(hashFollowers.Count, number);
                var accountListToReturn = BuildAccountListFromGuidCollection(hashFollowers, size, Storage);

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

        //To be used in following methods
        private ContentResult SubscriptionsEitherPublicOrAll(string accountName, int numberOfSubscriptions, bool withPrivate)
        {
            Answer output;

            try
            {
                // get the public lists followed by the given account

                var accountId = Storage.Account.GetId(accountName);
                var followedLists = Storage.List.GetAccountFollowedLists(accountId, withPrivate);
                var accountsInLists = new HashSet<Guid>();
                foreach (var followedList in followedLists)
                {
                    accountsInLists.UnionWith(Storage.List.GetAccounts(followedList));
                }

                // Get as many subscriptions as possible (maximum: numberOfSubscriptions)
                var size = Math.Min(accountsInLists.Count, numberOfSubscriptions);
                var accountListToReturn = BuildAccountListFromGuidCollection(accountsInLists, size, Storage);

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
        private ContentResult SubscriptionsEitherPublicOrAll(Guid accountId, int numberOfSubscriptions, bool withPrivate)
        {
            Answer output;

            try
            {
                // get the public lists followed by the given account
                var followedLists = Storage.List.GetAccountFollowedLists(accountId, withPrivate);
                var accountsInLists = new HashSet<Guid>();
                foreach (var followedList in followedLists)
                {
                    accountsInLists.UnionWith(Storage.List.GetAccounts(followedList));
                }

                // Get as many subscriptions as possible (maximum: numberOfSubscriptions)
                var size = Math.Min(accountsInLists.Count, numberOfSubscriptions);
                var accountListToReturn = BuildAccountListFromGuidCollection(accountsInLists, size, Storage);

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
        public ActionResult PubliclySubscribedAccounts(string accountName, int number)
        {
            return SubscriptionsEitherPublicOrAll(accountName, number, false);
        }

        //
        // GET : /infoaccount/publiclysubscribedaccounts/{accountId}/{number}
        public ActionResult PubliclySubscribedAccounts(Guid accountId, int number)
        {
            return SubscriptionsEitherPublicOrAll(accountId, number, false);
        }

        //
        // GET : /infoaccount/subscribedaccounts/{accountName}/{number}

        // [Authorize]
        public ActionResult SubscribedAccounts(string accountName, int number)
        {
            return SubscriptionsEitherPublicOrAll(accountName, number, true);
        }

        //
        // GET : /infoaccount/subscribedaccounts/{accountId}/{number}

        // [Authorize]
        public ActionResult SubscribedAccounts(Guid accountId, int number)
        {
            return SubscriptionsEitherPublicOrAll(accountId, number, true);
        }

        //to be used in following methods
        private ActionResult SubscribedListsEitherPublicOrAll(string accountName, int numberofLists, bool withPrivate)
        {
            Answer output;

            try
            {
                // get the public lists followed by the given account

                var accountId = Storage.Account.GetId(accountName);
                var followedLists = Storage.List.GetAccountFollowedLists(accountId, withPrivate);


                // Get as many subscriptions as possible (maximum: numberOfSubscriptions)
                var size = Math.Min(followedLists.Count, numberofLists);
                var listsToReturn = BuildListsFromGuidCollection(followedLists, size, Storage);

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
        private ActionResult SubscribedListsEitherPublicOrAll(Guid accountId, int numberofLists, bool withPrivate)
        {
            Answer output;

            try
            {
                // get the public lists followed by the given account

                var followedLists = Storage.List.GetAccountFollowedLists(accountId, withPrivate);


                // Get as many subscriptions as possible (maximum: numberOfSubscriptions)
                var size = Math.Min(followedLists.Count, numberofLists);
                var listsToReturn = BuildListsFromGuidCollection(followedLists, size, Storage);

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
        public ActionResult SubscribedPublicLists(string accountName, int number)
        {
            return SubscribedListsEitherPublicOrAll(accountName, number, false);
        }

        //
        // GET : /infoaccount/subscribedpubliclists/{accountId}/{number}
        public ActionResult SubscribedPublicLists(Guid accountId, int number)
        {
            return SubscribedListsEitherPublicOrAll(accountId, number, false);
        }

        //
        // GET : /infoaccount/subscribedlists/{accountName}/{number}

        //[Authorize]
        public ActionResult SubscribedLists(string accountName, int number)
        {
            return SubscribedListsEitherPublicOrAll(accountName, number, true);
        }

        //
        // GET : /infoaccount/subscribedlists/{accountId}/{number}

        //[Authorize]
        public ActionResult SubscribedLists(Guid accountId, int number)
        {
            return SubscribedListsEitherPublicOrAll(accountId, number, true);
        }

        //
        // GET : /infoaccount/subscriberLists/{accountName}/{number}
        public ActionResult SubscriberLists(string name, int numberOfSubscribers)
        {
            Answer output;

            try
            {
                var accountId = Storage.Account.GetId(name);

                // get lasts followers of user name 's list
                var followingLists = Storage.List.GetFollowingLists(accountId);

                // Get as many subscribers as possible (maximum: number)
                var size = Math.Min(followingLists.Count, numberOfSubscribers);
                var accountListToReturn = BuildAccountListFromGuidCollection(followingLists, size, Storage);

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
        // GET : /infoaccount/subscriberLists/{accountName}/{number}
        public ActionResult SubscriberLists(Guid accountId, int numberOfSubscribers)
        {
            Answer output;

            try
            {
                // get lasts followers of user name 's list
                var followingLists = Storage.List.GetFollowingLists(accountId);

                // Get as many subscribers as possible (maximum: number)
                var size = Math.Min(followingLists.Count, numberOfSubscribers);
                var accountListToReturn = BuildAccountListFromGuidCollection(followingLists, size, Storage);

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

        //to be used in following methods
        private ActionResult OwnedListsEitherPublicOrAll(string accountName, int numberOfLists, bool withPrivate)
        {
            Answer output;

            try
            {
                // get the public lists owned by the given account

                var accountId = Storage.Account.GetId(accountName);
                var ownedLists = Storage.List.GetAccountOwnedLists(accountId, withPrivate);


                // Get as many subscriptions as possible (maximum: numberOfSubscriptions)
                var size = Math.Min(ownedLists.Count, numberOfLists);
                var listsToReturn = BuildListsFromGuidCollection(ownedLists, size, Storage);

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
        private ActionResult OwnedListsEitherPublicOrAll(Guid accountId, int numberOfLists, bool withPrivate)
        {
            Answer output;

            try
            {
                // get the public lists owned by the given account
                var ownedLists = Storage.List.GetAccountOwnedLists(accountId, withPrivate);


                // Get as many subscriptions as possible (maximum: numberOfSubscriptions)
                var size = Math.Min(ownedLists.Count, numberOfLists);
                var listsToReturn = BuildListsFromGuidCollection(ownedLists, size, Storage);

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
        // GET : infoaccount/ownedpubliclists/{accountId}/{number}
        public ActionResult OwnedPublicLists(Guid accountId, int number)
        {
            return OwnedListsEitherPublicOrAll(accountId, number, false);
        }

        //
        // GET : /infoaccount/ownedlists/{accountName}/{number}

        //[Authorize]
        public ActionResult OwnedLists(string accountName, int number)
        {
            return OwnedListsEitherPublicOrAll(accountName, number, true);
        }

        //
        // GET : /infoaccount/ownedlists/{accountId}/{number}

        //[Authorize]
        public ActionResult OwnedLists(Guid accountId, int number)
        {
            return OwnedListsEitherPublicOrAll(accountId, number, true);
        }

        //
        //GET infoaccount/main/{accountName}
        
        //[Authorize]
        public ActionResult MainInfo(string accountName)
        {
            Answer output;

            try
            {
                // get the informations of the given account

                var accountId = Storage.Account.GetId(accountName);
                var accountInfo = Storage.Account.GetInfo(accountId);
                var accountToReturn = new Account(accountId, accountName,accountInfo.Description);
                output = new Answer(accountToReturn);
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
        //GET infoaccount/main/{accountName}
        public ActionResult MainInfo(Guid accountId)
        {
            Answer output;

            try
            {
                // get the informations of the given account
                var accountInfo = Storage.Account.GetInfo(accountId);
                var accountToReturn = new Account(accountId, accountInfo.Name, accountInfo.Description);
                output = new Answer(accountToReturn);
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
        // GET : infoaccount/users/{accountName}/{number}
        //[Authorize] (?)
        public ActionResult UsersAllowed(string accountName, int number)
        {
            Answer output;

            try
            {
                var accountId = Storage.Account.GetId(accountName);

                // get users posting on this account
                var users = Storage.Account.GetUsers(accountId);

                // Get as many users as possible (maximum: number)
                var size = Math.Min(users.Count, number);
                var userListToReturn = BuilUserListFormGuidCollection(users, size, Storage);

                output = new Answer(userListToReturn);
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
        // GET : infoaccount/users/{accountID}/{number}
        //[Authorize] (?)
        public ActionResult UsersAllowed(Guid accountId, int number)
        {
            Answer output;

            try
            {
                // get users posting on this account
                var users = Storage.Account.GetUsers(accountId);

                // Get as many users as possible (maximum: number)
                var size = Math.Min(users.Count, number);
                var userListToReturn = BuilUserListFormGuidCollection(users, size, Storage);

                output = new Answer(userListToReturn);
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
        // GET : infoaccount/administrator/{accountName}

        //[Authorize]
        public ActionResult Administrator(string accountName)
        {
            Answer output;

            try
            {
                var accountId = Storage.Account.GetId(accountName);

                // get account's administrator
                var adminId = Storage.Account.GetAdminId(accountId);
                var adminInfo = Storage.User.GetInfo(adminId);
                var admin = new User(adminInfo, Storage.User.GetId(adminInfo.Login));

                output = new Answer(admin);
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
        // GET : infoaccount/administrator/{accountID}

        //[Authorize]
        public ActionResult Administrator(Guid accountId)
        {
            Answer output;

            try
            {
                // get account's administrator
                var adminId = Storage.Account.GetAdminId(accountId);
                var adminInfo = Storage.User.GetInfo(adminId);
                var admin = new User(adminInfo, Storage.User.GetId(adminInfo.Login));

                output = new Answer(admin);
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
    }
}
