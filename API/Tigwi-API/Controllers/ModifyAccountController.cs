using System;
using System.Web.Mvc;
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
            Answer output;

            try
            {
                var accountId = msg.AccountId;
                if (accountId == new Guid("default") )
                    accountId = Storage.Account.GetId(msg.AccountName);

                var msgId = Storage.Msg.Post(accountId, msg.Message.Content);

                //Result
                output = new Answer(new ObjectCreated(msgId));
            }
            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                output = new Answer(new Error(exception.Code.ToString()));
            }

            return Serialize(output);
        }

        //
        // POST : modifyaccount/copy

        //[Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Copy(CopyMsg msg)
        {
            Answer output;

            try
            {
                var accountId = msg.AccountId;
                if (accountId == new Guid("default"))
                    accountId = Storage.Account.GetId(msg.AccountName);

                var msgId = Storage.Msg.Copy(accountId, msg.MessageId);

                //Result
                output = new Answer(new ObjectCreated(msgId));
            }
            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                output = new Answer(new Error(exception.Code.ToString()));
            }

            return Serialize(output);
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
               // var accountId = msg.AccountId;
               // if (accountId == new Guid("default"))
               //     accountId = Storage.Account.GetId(msg.AccountName);

                Storage.Msg.Remove(msg.MessageId);

                // Result is an empty error XML element
                error = new Error();
            }
            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                error = new Error(exception.Code.ToString());
            }

            return Serialize(new Answer(error));
        }

        //
        // POST : modifyaccount/tag

        //[Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Tag(Tag msg)
        {
            Error error;

            try
            {
                var accountId = msg.AccountId;
                if (accountId == new Guid("default"))
                    accountId = Storage.Account.GetId(msg.AccountName);

              Storage.Msg.Tag(accountId, msg.MessageId);

                //Result is an empty error
                error = new Error();
            }
            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                error = new Error(exception.Code.ToString());
            }

            return Serialize(new Answer(error));
        }

        //
        // POST : modifyaccount/tag

        //[Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Untag(Untag msg)
        {
            Error error;

            try
            {
                var accountId = msg.AccountId;
                if (accountId == new Guid("default"))
                    accountId = Storage.Account.GetId(msg.AccountName);

                Storage.Msg.Untag(accountId, msg.MessageId);

                //Result is an empty error
                error = new Error();
            }
            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                error = new Error(exception.Code.ToString());
            }

            return Serialize(new Answer(error));
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
                var accountId = Storage.Account.GetId(subscribe.AccountName);

                Storage.List.Follow(subscribe.Subscription, accountId);

                // Result is an empty error XML element
                error = new Error();
            }
            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                error = new Error(exception.Code.ToString());
            }

            return Serialize(new Answer(error));
        }

        //
        // POST /modifyaccount/createlist

        //[Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateList(CreateList listCreation)
        {
            Answer output;

            try
            {
                var accountId = Storage.Account.GetId(listCreation.Account);
                var listToCreate = listCreation.ListInfo;

                var listId = Storage.List.Create(accountId, listToCreate.Name, listToCreate.Description,
                                    listToCreate.IsPrivate);

                // Result is an empty error XML element
                output = new Answer(new ObjectCreated(listId));
            }
            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                output = new Answer(new Error(exception.Code.ToString()));
            }

            return Serialize(output);
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
                if (accountId == new Guid("default"))
                    accountId = Storage.Account.GetId(infos.AccountName);

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

            return Serialize(new Answer(error));
        }

        //
        // POST modifyaccount/adduser

        //[Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult RemoveUser(RemoveUser infos)
        {
            Error error;

            try
            {
                var accountId = infos.AccountId;
                if (accountId == new Guid("default"))
                    accountId = Storage.Account.GetId(infos.AccountName);

                var userId = infos.UserId;
                if (userId == new Guid("default") )
                   userId = Storage.User.GetId(infos.UserLogin);

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

            return Serialize(new Answer(error));
        }

        //
        // POST modifyaccount/adduser

        //[Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddUser(AddUser infos)
        {
            Error error;

            try
            {
                var accountId = infos.AccountId;
                if (accountId == new Guid("default"))
                    accountId = Storage.Account.GetId(infos.AccountName);

                var userId = infos.UserId;
                if (userId == new Guid("default"))
                    userId = Storage.User.GetId(infos.UserLogin);

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

            return Serialize(new Answer(error));
        }


        //
        // POST modifyaccount/changeadmin

        //[Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ChangeAdmin(ChangeAdministrator infos)
        {
            Error error;

            try
            {
                var accountId = infos.AccountId;
                if (accountId == new Guid("default"))
                    accountId = Storage.Account.GetId(infos.AccountName);

                var userId = infos.UserId;
                if (userId == new Guid("default"))
                    userId = Storage.User.GetId(infos.UserLogin);

                //Set the informations
                Storage.Account.SetAdminId(accountId, userId);

                // Result is an empty error XML element
                error = new Error();
            }
            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                error = new Error(exception.Code.ToString());
            }

            return Serialize(new Answer(error));
        }
    }
}
