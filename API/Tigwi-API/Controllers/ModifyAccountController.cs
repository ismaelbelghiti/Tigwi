using System.IO;
using System.Web.Mvc;
using System.Xml.Serialization;
using StorageLibrary;
using Tigwi_API.Models;

namespace Tigwi_API.Controllers
{
    public class ModifyAccountController : ApiController
    {
        //
        // POST : modifyaccount/write

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
        // POST : /modifyaccount/suscribelist

        //[Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SubscribeList(SubscribeList subscribe)
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
        // POST /modifyaccount/createlist

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
        
    }
}
