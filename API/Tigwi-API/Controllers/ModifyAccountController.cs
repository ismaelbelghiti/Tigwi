using System;
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
            Error error;

            try
            {
                var accountId = msg.AccountId;
                //TODO: find how to test accountId
                //if (accountId == )
                //    accountId = Storage.Account.GetId(msg.AccountName);

                Storage.Msg.Post(accountId, msg.Message.Content);

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
        // POST : modifyaccount/delete

        //[Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(MsgToDelete msg)
        {
            Error error;

            try
            {
                //TODO: find out how to use this information, if necessary (?)
                var accountId = msg.AccountId;
                //TODO: find how to test accountId
                //if (accountId == )
                //    accountId = Storage.Account.GetId(msg.AccountName);

                Storage.Msg.Remove(msg.MessageId);

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
        // POST : /modifyaccount/suscribelist

        //[Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SubscribeList(SubscribeList subscribe)
        {
            Error error;

            try
            {
                var accountId = Storage.Account.GetId(subscribe.Account);

                Storage.List.Follow(subscribe.Subscription, accountId);

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
        // POST /modifyaccount/createlist

        //[Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateList(CreateList listCreation)
        {
            Error error;

            try
            {
                var accountId = Storage.Account.GetId(listCreation.Account);
                var listToCreate = listCreation.ListInfo;

                Storage.List.Create(accountId, listToCreate.Name, listToCreate.Description,
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
            (new XmlSerializer(typeof (Error))).Serialize(stream, error);

            return Content(stream.ToString());
        }

        //
        // POST modifyaccount/changedescription

        //[Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ChangeDescription(ChangeDescription infos)
        {
            Error error;

            try
            {
                var accountId = infos.AccountId;
                //TODO: find how to test accountId to know if the information was sent or not
                //if (accountId == )
                //    accountId = Storage.Account.GetId(infos.AccountName);

                //Set the informations
                Storage.Account.SetInfo(accountId, infos.Description);

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
        // POST modifyaccount/adduser

        //[Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult RemoveUser(AccountUser infos)
        {
            Error error;

            try
            {
                var accountId = infos.AccountId;
                //TODO: find how to test accountId to know if the information was sent or not
                //if (accountId == )
                //    accountId = Storage.Account.GetId(infos.AccountName);

                var userId = infos.UserId;
                //TODO: find how to test userId to know if the information was sent or not
                //if (userId == )
                //   userId = Storage.User.GetId(infos.UserLogin);

                //Set the informations
                Storage.Account.Add(accountId, userId);

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
        // POST modifyaccount/adduser

        //[Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddUser(AccountUser infos)
        {
            Error error;

            try
            {
                var accountId = infos.AccountId;
                //TODO: find how to test accountId to know if the information was sent or not
                //if (accountId == )
                //    accountId = Storage.Account.GetId(infos.AccountName);

                var userId = infos.UserId;
                //TODO: find how to test userId to know if the information was sent or not
                //if (userId == )
                //   userId = Storage.User.GetId(infos.UserLogin);

                //Set the informations
                Storage.Account.Remove(accountId, userId);

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
    

    }
}
