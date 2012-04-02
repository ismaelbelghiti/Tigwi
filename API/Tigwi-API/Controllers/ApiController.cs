using System.IO;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Xml.Serialization;
using StorageLibrary;
using Tigwi_API.Models;

namespace Tigwi_API.Controllers
{
    public class ApiController : Controller
    {
        //
        // GET: /usertimeline/{name}/{numberOfMessages}

        public ActionResult UserTimeline(string name, int numberOfMessages)
        {
            // TODO : handle errors more precisely

            IStorage storage = new Storage("",""); // connexion

            MessageList listMsgsOutput;

            try
            {
                int accountId = storage.User.GetId(name);
                // get lasts messages from user name
                var listMsgs = storage.Msg.GetListsMsgTo(new HashSet<int> {accountId}, int.MaxValue, numberOfMessages);
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
        // GET : /usersubscriptions/{name}/{numberOfSubscriptions}

        public ActionResult UserSubscriptionsList(string name, int numberOfSubscriptions)
        {
            // TODO : handle errors

            IStorage storage = new Storage("", ""); // connexion

            int accountId = storage.Account.GetId(name);
            // get lasts followers of user name 's list

            //TODO: use right methode once implemented
            HashSet<int> listsFollowing = storage.List.GetFollowingLists(accountId);
            UserList listUsers = new UserList();

            //TODO: implement initialization of listUsers


            // convert, looking forward serialization
            
            // a stream is needed for serialization
            var stream = new MemoryStream();
            var result = new FileStreamResult(stream, "xml"); // is "xml" the right contentType ??

            var serialize = new XmlSerializer(typeof(UserList));
            serialize.Serialize(stream, listUsers);

            stream.Flush(); // is it necessary ??
            return result;
        }

        //
        // GET : /usersubscribers/{name}/{numberOfSusbscribers}

        public ActionResult UserSubscribersList(string name, int numberOfSubscribers)
        {
            // TODO : handle errors

            IStorage storage = new Storage("", ""); // connexion

            int accountId = storage.Account.GetId(name);
            // get lasts followers of user name 's list

            //TODO: use right methode once implemented
            HashSet<int> listsFollowing = storage.List.GetAccountFollowedLists(accountId, false);
            UserList listUsers = new UserList();

            //TODO: implement initialization of listUsers


            // convert, looking forward serialization

            // a stream is needed for serialization
            var stream = new MemoryStream();
            var result = new FileStreamResult(stream, "xml"); // is "xml" the right contentType ??

            var serialize = new XmlSerializer(typeof(UserList));
            serialize.Serialize(stream, listUsers);

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
                int accountId = storage.Account.GetId(subscribe.User);
                int subsciptionId = storage.Account.GetId(subscribe.Subscription);

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
