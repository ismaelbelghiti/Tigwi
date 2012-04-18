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
            //TODO : use appropriate storage connexion
            IStorage storage = new StorageTmp(); // connexion
            Error error;

            try
            {
                var accountId = storage.Account.GetId(msg.Account);

                storage.Msg.Post(accountId, msg.Message.Content);

                // Result is an empty error XML element
                error = new Error();
            }
            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                error = new Error(exception.Code.ToString());
            }

            var stream = new MemoryStream();
            (new XmlSerializer(typeof (Error))).Serialize(stream, error);

            return Content(stream.ToString());
        }

        //
        // POST : /modify/accountsuscribelist

        //[Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AccountSubscribeList(SubscribeList subscribe)
        {
            //TODO : use appropriate storage connexion
            IStorage storage = new StorageTmp(); // connexion
            Error error;

            try
            {
                var accountId = storage.Account.GetId(subscribe.Account);

                storage.List.Follow(subscribe.Subscription, accountId);

                // Result is an empty error XML element
                error = new Error();
            }
            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                error = new Error(exception.Code.ToString());
            }

            var stream = new MemoryStream();
            (new XmlSerializer(typeof(Error))).Serialize(stream, error);

            return Content(stream.ToString());
        }

        //
        // POST /modify/createlist

        //[Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateList(CreateList listCreation)
        {
            //TODO : use appropriate storage connexion
            IStorage storage = new StorageTmp(); // connexion
            Error error;

            try
            {
                var accountId = storage.Account.GetId(listCreation.Account);
                var listToCreate = listCreation.ListInfo;

                storage.List.Create(accountId,listToCreate.Name, listToCreate.Description,
                                    listToCreate.IsPrivate);

                // Result is an empty error XML element
                error = new Error();
            }
            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                error = new Error(exception.Code.ToString());
            }

            var stream = new MemoryStream();
            (new XmlSerializer(typeof(Error))).Serialize(stream, error);

            return Content(stream.ToString());
        }

        //
        // POST : /modify/listsubscribeaccount/

        //[Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ListSubscribeAccount(ListSubscribe listSubscribe)
        {
             //TODO : use appropriate storage connexion
            IStorage storage = new StorageTmp(); // connexion
            Error error;

            try
            {
                var accountId = storage.Account.GetId(listSubscribe.Subscription);
                storage.List.Add(listSubscribe.List, accountId);
                
                // Result is an empty error XML element
                error = new Error();
            }
            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                error = new Error(exception.Code.ToString());
            }

            var stream = new MemoryStream();
            (new XmlSerializer(typeof(Error))).Serialize(stream, error);

            return Content(stream.ToString());
        }
        
    }
}
