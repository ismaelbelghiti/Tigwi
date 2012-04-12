using System;
using System.IO;
using System.Web.Mvc;
using System.Xml.Serialization;
using StorageLibrary;
using Tigwi_API.Models;

namespace Tigwi_API.Controllers
{
    public class ModifyController : ApiController
    {
        //
        // POST : modify/write

        //[Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Write(MsgToWrite msg)
        {
            IStorage storage = new StorageTmp(); // connexion

            ContentResult result;

            try
            {
                var accountId = storage.Account.GetId(msg.Account);

                storage.Msg.Post(accountId, msg.Message.Content);

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
        // POST : /modify/accountsuscribelist

        //[Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AccountSubscribeList(SubscribeList subscribe)
        {
            IStorage storage = new StorageTmp(); // connexion

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
        // POST /modify/createlist

        //[Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateList(/*to be implemented)*/)
        {
            //TODO : implement this
            throw new NotImplementedException();
        }

        //
        // POST : /modify/listsubscribeaccount/

        //[Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ListSubscribeAccount(/*to be implemented*/)
        {
            //TODO: implement this
            throw new NotImplementedException();
        }

    }
}
