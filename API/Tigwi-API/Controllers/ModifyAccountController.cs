using System;
using System.Web.Mvc;
using System.Xml.Serialization;
using StorageLibrary;
using Tigwi_API.Models;

namespace Tigwi_API.Controllers
{
    public class ModifyAccountController : ApiController
    {
        //
        // POST : /modifyaccount/write

        //[Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Write()
        {
            Answer output;

            try
            {
                var msg = (MsgToWrite) (new XmlSerializer(typeof (MsgToWrite))).Deserialize(Request.InputStream);

                if (msg.AccountId == null && msg.AccountName == null)
                    output = new Answer(new Error("AccountId or AccountName missing"));
                else if (msg.Message == null) // TODO ? check more on message (size for example)
                    output = new Answer(new Error("Message missing"));
                else
                {
                    try
                    {
                        var accountId = msg.AccountId ?? Storage.Account.GetId(msg.AccountName);

                        var msgId = Storage.Msg.Post(accountId, msg.Message.Content);

                        //Result
                        output = new Answer(new ObjectCreated(msgId));
                    }
                    catch (StorageLibException exception)
                    {
                        // Result is an non-empty error XML element
                        output = new Answer(new Error(exception.Code.ToString()));
                    }
                }
            }
            catch (InvalidOperationException exception)
            {
                output = new Answer(new Error(exception.Message + " " + exception.InnerException.Message));
            }

            return Serialize(output);
        }

        // Is copying like retweeting ?
        // POST : /modifyaccount/copy

        //[Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Copy()
        {
            Answer output;

            try
            {
                var msg = (CopyMsg) (new XmlSerializer(typeof (CopyMsg))).Deserialize(Request.InputStream);

                if (msg.AccountId == null && msg.AccountName == null)
                    output = new Answer(new Error("AccountId or AccountName missing"));
                else if (msg.MessageId == null)
                    output = new Answer(new Error("MessageId missing"));
                else
                {
                    try
                    {
                        var accountId = msg.AccountId ?? Storage.Account.GetId(msg.AccountName);

                        var msgId = Storage.Msg.Copy(accountId, msg.MessageId.GetValueOrDefault());

                        //Result
                        output = new Answer(new ObjectCreated(msgId));
                    }
                    catch (StorageLibException exception)
                    {
                        // Result is an non-empty error XML element
                        output = new Answer(new Error(exception.Code.ToString()));
                    }
                }
            }
            catch (InvalidOperationException exception)
            {
                output = new Answer(new Error(exception.Message + " " + exception.InnerException.Message));
            }

            return Serialize(output);
        }

        // TODO : continue adding checkings beyond this point

        //
        // POST : /modifyaccount/delete

        //[Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete()
        {
            Error error;

            var msg = (MsgToDelete)(new XmlSerializer(typeof(MsgToDelete))).Deserialize(Request.InputStream);

            try
            {
                //TODO: find out how to use this information, if necessary (?)
                //var accountId = msg.AccountId ?? Storage.Account.GetId(msg.AccountName);

                Storage.Msg.Remove(msg.MessageId.GetValueOrDefault());

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
        // POST : /modifyaccount/tag

        //[Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Tag()
        {
            Error error;

            var msg = (Tag)(new XmlSerializer(typeof(Tag))).Deserialize(Request.InputStream);

            try
            {
                var accountId = msg.AccountId ?? Storage.Account.GetId(msg.AccountName);

                Storage.Msg.Tag(accountId, msg.MessageId.GetValueOrDefault());

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
        // POST : /modifyaccount/untag

        //[Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Untag()
        {
            Error error;

            var msg = (Untag)(new XmlSerializer(typeof(Untag))).Deserialize(Request.InputStream);

            try
            {
                var accountId = msg.AccountId ?? Storage.Account.GetId(msg.AccountName);

                Storage.Msg.Untag(accountId, msg.MessageId.GetValueOrDefault());

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
        // POST : /modifyaccount/subscribelist

        //[Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SubscribeList()
        {
            Error error;

            var subscribe = (SubscribeList)(new XmlSerializer(typeof(SubscribeList))).Deserialize(Request.InputStream);

            try
            {
                var accountId = subscribe.AccountId ?? Storage.Account.GetId(subscribe.AccountName);

                Storage.List.Follow(subscribe.Subscription.GetValueOrDefault(), accountId);

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
        public ActionResult CreateList()
        {
            Answer output;

            var listCreation = (CreateList)(new XmlSerializer(typeof(CreateList))).Deserialize(Request.InputStream);

            try
            {
                var accountId = listCreation.AccountId ?? Storage.Account.GetId(listCreation.AccountName);
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
        // POST /modifyaccount/changedescription

        //[Authorize]
        // According to spec, authentication must check that the user is the administrator of the account
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ChangeDescription()
        {
            Error error;

            var infos = (ChangeDescription)(new XmlSerializer(typeof(ChangeDescription))).Deserialize(Request.InputStream);

            try
            {
                var accountId = infos.AccountId ?? Storage.Account.GetId(infos.AccountName);

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

        // WARNING : are the following methods really necessary in an API ?

        /*
        //
        // POST : /modifyaccount/removeuser

        //[Authorize]
        // According to spec, authentication must check that the user is the administrator of the account
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult RemoveUser()
        {
            Error error;

            var infos = (RemoveUser)(new XmlSerializer(typeof(RemoveUser))).Deserialize(Request.InputStream);

            try
            {
                var accountId = infos.AccountId ?? Storage.Account.GetId(infos.AccountName);

                var userId = infos.UserId ?? Storage.User.GetId(infos.UserLogin);

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
        */
         
        /*
        //
        // POST : /modifyaccount/adduser

        //[Authorize]
        // According to spec, authentication must check that the user is the administrator of the account
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddUser()
        {
            Error error;

            var infos = (AddUser)(new XmlSerializer(typeof(AddUser))).Deserialize(Request.InputStream);

            try
            {
                var accountId = infos.AccountId ?? Storage.Account.GetId(infos.AccountName);

                var userId = infos.UserId ?? Storage.User.GetId(infos.UserLogin);

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
        */

        /*
        //
        // POST : /modifyaccount/changeadmin

        //[Authorize]
        // According to spec, authentication must check that the user is the administrator of the account
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ChangeAdmin()
        {
            Error error;

            var infos = (ChangeAdministrator)(new XmlSerializer(typeof(ChangeAdministrator))).Deserialize(Request.InputStream);

            try
            {
                var accountId = infos.AccountId ?? Storage.Account.GetId(infos.AccountName);

                var userId = infos.UserId ?? Storage.User.GetId(infos.UserLogin);

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
        */
    }
}
