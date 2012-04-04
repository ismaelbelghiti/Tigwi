using System;
using System.IO;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Xml.Serialization;
using System.Linq;
using System.Text;
using StorageLibrary;
using Tigwi_API.Models;

namespace Tigwi_API.Controllers
{
    public class ApiController : Controller
    {
        private UserList BuildUserListFromAccountsHashSet (HashSet<Guid> hashAccounts, int size, IStorage storage )
        {
            var userList = new List<UserApi>();
            int k;
            for (k = 0; k < size; k++)
            {
                var accountId = hashAccounts.First();
                var user = new UserApi(accountId, storage.Account.GetInfo(accountId).Name);
                userList.Add(user);
            }

            return new UserList(userList);  
        }
        
        //
        // GET: /usertimeline/{name}/{numberOfMessages}

        public ActionResult UserTimeline(string name, int numberOfMessages)
        {
            // TODO : handle errors more precisely

            IStorage storage = new Storage("",""); // connexion

            MessageList listMsgsOutput;

            try
            {
                var accountId = storage.User.GetId(name);
                // get lasts messages from user name
                var listMsgs = storage.Msg.GetListsMsgTo(new HashSet<Guid> {accountId}, DateTime.Now , numberOfMessages);
                // convert, looking forward serialization
                listMsgsOutput = new MessageList(listMsgs);
            }
            catch // for the moment, all errors result in sending an empty list as a result
            {
                listMsgsOutput = new MessageList();
            }
            // a stream is needed for serialization
            var stream = new MemoryStream();
            var result = new FileStreamResult(stream, "xml"); // is "xml" the right contentType ??
            // Maybe a ContentResult would be more adequate.

            var serialize = new XmlSerializer(typeof (MessageList));
            serialize.Serialize(stream,listMsgsOutput);

            stream.Flush(); // is it necessary ??
            return result;
        }

        //
        // GET : /usersubscribers/{name}/{numberOfSubscribers}

        public ActionResult UserSubscribersList(string name, int numberOfSubscribers)
        {
            // TODO : handle errors

            IStorage storage = new Storage("", ""); // connexion

            var accountId = storage.Account.GetId(name);
         
            // get lasts followers of user name 's list
            var followingLists = storage.List.GetFollowingLists(accountId);
            var hashFollowers = new HashSet<Guid>();
            foreach (var followingList in followingLists)
            {
                hashFollowers.UnionWith(storage.List.GetFollowingAccounts(followingList));
            }

            // convert, looking forward serialization
            var sizeHash = hashFollowers.Count;
            var size = sizeHash<numberOfSubscribers ? sizeHash : numberOfSubscribers;

                // Get as many subscribers as possible (maximum: numberOfSubscibers)
                var userListToReturn = BuildUserListFromAccountsHashSet(hashFollowers, size, storage);   

            // a stream is needed for serialization
            var stream = new MemoryStream();
            var result = new FileStreamResult(stream, "xml"); // is "xml" the right contentType ??

            var serialize = new XmlSerializer(typeof(UserList));
            serialize.Serialize(stream, userListToReturn);

            stream.Flush(); // is it necessary ??
            return result;
        }

        //
        // GET : /usersubscriptions/{name}/{numberOfSubscriptions}

        public ActionResult UserSubscriptionsList(string name, int numberOfSubscriptions)
        {
            // TODO : handle errors

            IStorage storage = new Storage("", ""); // connexion

            var accountId = storage.Account.GetId(name);
            // get public lists followed by user name

            var followedLists = storage.List.GetAccountFollowedLists(accountId, false);
            var accountsInLists = new HashSet<Guid>();
            foreach (var followedList in followedLists)
            {
                accountsInLists.UnionWith(storage.List.GetAccounts(followedList));
            }

            // convert, looking forward serialization
            var sizeHash = accountsInLists.Count;
            var size = sizeHash < numberOfSubscriptions ? sizeHash : numberOfSubscriptions;
                
                // Get as many subscriptions as possible (maximum: numberOfSubscibers)
                var userListToReturn = BuildUserListFromAccountsHashSet(accountsInLists, size, storage);  
           
            // a stream is needed for serialization
            var stream = new MemoryStream();
            var result = new FileStreamResult(stream, "xml"); // is "xml" the right contentType ??

            var serialize = new XmlSerializer(typeof(UserList));
            serialize.Serialize(stream, userListToReturn);

            stream.Flush(); // is it necessary ??
            return result;
        }

        //
        // POST : /write

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult WritePost(MsgToWrite msg)
        {
            // TODO: handle errors (can happen at any time)

            IStorage storage = new Storage("", ""); // connexion

            ContentResult result;

            try
            {
                var accountId = storage.Account.GetId(msg.User);

                storage.Msg.Post(accountId, msg.Message.Content);

                // Result is an empty error XML element
                var stream = new MemoryStream();
                (new XmlSerializer(typeof (Error))).Serialize(stream, new Error());
                result = Content(stream.ToString());
            }
            catch // exceptions storage can throw description needed
            {
                // Result is an non-empty error XML element
                var stream = new MemoryStream();
                (new XmlSerializer(typeof(Error))).Serialize(stream, new Error(42));
                result = Content(stream.ToString());
            }

            return result;
        }

        //
        // POST : /suscribe

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Subscribe(Subscribe subscribe)
        {
            // TODO: handle errors (can happen at any time)

            IStorage storage = new Storage("", ""); // connexion

            ContentResult result;

            try
            {
                var accountId = storage.Account.GetId(subscribe.User);
                var subsciptionId = storage.Account.GetId(subscribe.Subscription);

                storage.List.Follow(accountId, subsciptionId); // accountId follow subscriptionId, right ?

                // Result is an empty error XML element
                var stream = new MemoryStream();
                (new XmlSerializer(typeof(Error))).Serialize(stream, new Error());
                result = Content(stream.ToString());
            }
            catch
            {
                // Result is an non-empty error XML element
                var stream = new MemoryStream();
                (new XmlSerializer(typeof(Error))).Serialize(stream, new Error(42));
                result = Content(stream.ToString());
            }
            return result;
        }


    }
}
