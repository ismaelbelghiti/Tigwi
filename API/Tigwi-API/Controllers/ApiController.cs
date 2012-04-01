using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        public FileStreamResult UserTimeline(string name, int numberOfMessages)
        {
            // TODO : process errors

            IStorage storage = new Storage("",""); // connexion

            var id = storage.User.GetId(name);
            // get lasts messages from user name
            var listMsgs = storage.Msg.GetListsMsgTo(new HashSet<int> {id}, int.MaxValue, numberOfMessages);
            // convert, looking forward serialization
            var listMsgsOutput = new MessageList(listMsgs);

            // a stream is needed for serialization
            var stream = new MemoryStream();
            var result = new FileStreamResult(stream, "xml"); // is "xml" the right contentType ??

            var serialize = new XmlSerializer(typeof (MessageList));
            serialize.Serialize(stream,listMsgs);

            stream.Flush(); // is it necessary ??
            return result;
        }

        //
        // GET : /usersubscriptions/{name}/{numberOfSubscriptions}

        public ActionResult UserSubscriptionsList(string name, int numberOfSubscriptions)
        {
            // TODO : process errors

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
            // TODO : process errors

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
        public ActionResult WritePost(/*objet convenable*/)
        {
            throw new NotImplementedException();
        }

        //
        // POST : /suscribe

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Suscribe(/*objet convenable*/)
        {
            throw new NotImplementedException();
        }


    }
}
