using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Tigwi.API.Models;
using Tigwi.Storage.Library;

namespace Tigwi.API.Controllers
{
    public abstract class InfoAccountController : ApiController
    {

        //
        // GET: /infoaccount/messages/{accountName}/{number}
        // GET: /infoaccount/messages/name={accountName}/{number}
        // GET: /infoaccount/messages/id={accountId}/{number}
        public ActionResult Messages(string accountName, Guid? accountId, int number)
        {
            Answer output;

            try
            {
                var realId = accountId ?? Storage.Account.GetId(accountName);

                // get lasts messages from account accoutName
                var personalListId = Storage.List.GetPersonalList(realId);
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

            return Serialize(output);
        }

        // TODO : The following method isn't documented.
        
        //
        // GET: /infoaccount/taggedmessages/{accountName}/{number}
        // GET: /infoaccount/taggedmessages/name={accountName}/{number}
        // GET: /infoaccount/taggedmessages/id={accountId}/{number}
        public ActionResult TaggedMessages(string accountName, Guid? accountId, int number)
        {
            Answer output;

            try
            {
                var realId = accountId ?? Storage.Account.GetId(accountName);

                // get lasts messages from user name
                var listMsgs = Storage.Msg.GetTaggedTo(realId, DateTime.Now, number);

                // convert, looking forward XML serialization
                var listMsgsOutput = new Messages(listMsgs, Storage);
                output = new Answer(listMsgsOutput);
            }

            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                output = new Answer(new Error(exception.Code.ToString()));
            }

            return Serialize(output);
        }


        //
        // GET : /infoaccount/subscriberaccounts/{accountName}/{number}
        // GET: /infoaccount/subscriberaccounts/name={accountName}/{number}
        // GET: /infoaccount/subscriberaccounts/id={accountId}/{number}
        public ActionResult SubscribersAccounts(string accountName, Guid? accountId, int number)
        {
            Answer output;

            try
            {
                var realId = accountId ?? Storage.Account.GetId(accountName);

                // get lasts followers of user name 's list
                var followingLists = Storage.List.GetFollowingLists(realId);
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

            return Serialize(output);
        }


        // TODO : need authentication when withPrivate = true
        
        //
        // GET : /infoaccount/subscribedaccounts/{accountName}/{number}
        // GET: /infoaccount/subscribedaccounts/name={accountName}/{number}
        // GET: /infoaccount/subscribedaccounts/id={accountId}/{number}
        public ActionResult SubscribedAccounts(string accountName, Guid? accountId, int number, bool withPrivate)
        {
            Answer output;

            try
            {
                var realId = accountId ?? Storage.Account.GetId(accountName);

                // get the public lists followed by the given account
                var followedLists = Storage.List.GetAccountFollowedLists(realId, withPrivate);
                var accountsInLists = new HashSet<Guid>();
                foreach (var followedList in followedLists)
                {
                    accountsInLists.UnionWith(Storage.List.GetAccounts(followedList));
                }

                // Get as many subscriptions as possible (maximum: numberOfSubscriptions)
                var size = Math.Min(accountsInLists.Count, number);
                var accountListToReturn = BuildAccountListFromGuidCollection(accountsInLists, size, Storage);

                output = new Answer(accountListToReturn);
            }

            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                output = new Answer(new Error(exception.Code.ToString()));
            }

            return Serialize(output);
        }


        // TODO : need authentication when withPrivate = true

        //
        // GET : /infoaccount/subscribedlists/{accountName}/{number}
        // GET: /infoaccount/subscribedlists/name={accountName}/{number}
        // GET: /infoaccount/subscribedlists/id={accountId}/{number}
        public ActionResult SubscribedListsEitherPublicOrAll(string accountName, Guid? accountId, int number, bool withPrivate)
        {
            Answer output;

            try
            {
                var realId = accountId ?? Storage.Account.GetId(accountName);

                // get the public lists followed by the given account
                var followedLists = Storage.List.GetAccountFollowedLists(realId, withPrivate);

                // Get as many subscriptions as possible (maximum: numberOfSubscriptions)
                var size = Math.Min(followedLists.Count, number);
                var listsToReturn = BuildListsFromGuidCollection(followedLists, size, Storage);

                output = new Answer(listsToReturn);
            }

            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                output = new Answer(new Error(exception.Code.ToString()));
            }

            return Serialize(output);
        }


        //
        // GET : /infoaccount/subscriberlists/{accountName}/{number}
        // GET: /infoaccount/subscriberlists/name={accountName}/{number}
        // GET: /infoaccount/subscriberlists/id={accountId}/{number}
        public ActionResult SubscriberLists(string accountName, Guid? accountId, int number)
        {
            Answer output;

            try
            {
                var realId = accountId ?? Storage.Account.GetId(accountName);

                // get lasts followers of user name 's list
                var followingLists = Storage.List.GetFollowingLists(realId);

                // Get as many subscribers as possible (maximum: number)
                var size = Math.Min(followingLists.Count, number);
                var accountListToReturn = BuildAccountListFromGuidCollection(followingLists, size, Storage);

                output = new Answer(accountListToReturn);
            }

            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                output = new Answer(new Error(exception.Code.ToString()));
            }

            return Serialize(output);
        }


        // TODO : need authentication when withPrivate = true

        //
        // GET : /infoaccount/ownedlists/{accountName}/{number}
        // GET: /infoaccount/ownedlists/name={accountName}/{number}
        // GET: /infoaccount/ownedlists/id={accountId}/{number}
        public ActionResult OwnedListsEitherPublicOrAll(string accountName, Guid? accountId, int number, bool withPrivate)
        {
            Answer output;

            try
            {
                var realId = accountId ?? Storage.Account.GetId(accountName);

                // get the public lists owned by the given account
                var ownedLists = Storage.List.GetAccountOwnedLists(realId, withPrivate);

                // Get as many subscriptions as possible (maximum: numberOfSubscriptions)
                var size = Math.Min(ownedLists.Count, number);
                var listsToReturn = BuildListsFromGuidCollection(ownedLists, size, Storage);

                output = new Answer(listsToReturn);
            }

            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                output = new Answer(new Error(exception.Code.ToString()));
            }

            return Serialize(output);
        }


        //
        // GET : /infoaccount/maininfo/{accountName}
        // GET: /infoaccount/maininfo/name={accountName}/{number}
        // GET: /infoaccount/maininfo/id={accountId}/{number}
        public ActionResult MainInfo(string accountName, Guid? accountId)
        {
            Answer output;

            try
            {
                var realId = accountId ?? Storage.Account.GetId(accountName);

                // get the informations of the given account
                var accountInfo = Storage.Account.GetInfo(realId);
                var accountToReturn = new Account(realId, accountInfo.Name, accountInfo.Description);
                output = new Answer(accountToReturn);
            }

            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                output = new Answer(new Error(exception.Code.ToString()));
            }

            return Serialize(output);
        }

    }
}
