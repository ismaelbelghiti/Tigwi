using System;
using System.Web.Mvc;
using System.Xml.Serialization;
using Tigwi.Storage.Library;
using Tigwi.API.Models;

namespace Tigwi.API.Controllers
{
    public class MessageController : ApiController
    {
        //
        // POST : /message/write

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Write()
        {
            Answer output;

            try
            {
                var msg = (MsgToWrite) (new XmlSerializer(typeof (MsgToWrite))).Deserialize(Request.InputStream);

                if (msg.AccountId == null && msg.AccountName == null)
                    output = new Answer(new Error("AccountId or AccountName missing"));
                else if (msg.Message == null)
                    output = new Answer(new Error("Message missing"));
                else if (msg.Message.Length > 140)
                    output = new Answer(new Error("Message must not exceed 140 characters"));
                else
                {
                    var accountId = msg.AccountId ?? Storage.Account.GetId(msg.AccountName);

                    // Check if the user is authenticated and has rights
                    var authentication = Authorized(accountId);
                    if (authentication.HasRights)
                    {
                        var msgId = Storage.Msg.Post(accountId, msg.Message);

                        // Result
                        output = new Answer(new NewObject(msgId));
                    }
                    else
                        output = new Answer(new Error(authentication.ErrorMessage()));
                }
            }
            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                output = new Answer(new Error(exception.Code.ToString()));
            }
            catch (InvalidOperationException exception)
            {
                output = new Answer(new Error(exception.Message + " " + exception.InnerException.Message));
            }

            return Serialize(output);
        }


        // POST : /message/copy

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
                    var accountId = msg.AccountId ?? Storage.Account.GetId(msg.AccountName);

                    // Check if the user is authenticated and has rights
                    var authentication = Authorized(accountId);
                    if (authentication.HasRights)
                    {
                        var msgId = Storage.Msg.Copy(accountId, msg.MessageId.GetValueOrDefault());

                        //Result
                        output = new Answer(new NewObject(msgId));
                    }
                    else
                        output = new Answer(new Error(authentication.ErrorMessage()));
                }
            }
            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                output = new Answer(new Error(exception.Code.ToString()));
            }
            catch (InvalidOperationException exception)
            {
                output = new Answer(new Error(exception.Message + " " + exception.InnerException.Message));
            }

            return Serialize(output);
        }


        //
        // POST : /message/delete
        
        // TODO : Authentication when a method to get the owner is provided
        // Note : This is not implemented by storage
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete()
        {
            Error error;

            try
            {
                var msg = (MsgToDelete) (new XmlSerializer(typeof (MsgToDelete))).Deserialize(Request.InputStream);

                if (msg.MessageId == null)
                    error = new Error("MessageId missing");
                else
                {
                    //var ownerId = Storage.Msg.

                    Storage.Msg.Remove(msg.MessageId.GetValueOrDefault());

                    // Result is an empty error XML element
                    error = new Error();
                }
            }
            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                error = new Error(exception.Code.ToString());
            }
            catch (InvalidOperationException exception)
            {
                error = new Error(exception.Message + " " + exception.InnerException.Message);
            }

            return Serialize(new Answer(error));
        }
        

        //
        // POST : /message/tag

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Tag()
        {
            Error error;

            try
            {
                var msg = (Tag) (new XmlSerializer(typeof (Tag))).Deserialize(Request.InputStream);

                if (msg.AccountId == null && msg.AccountName == null)
                    error = new Error("AccountId or AccountName missing");
                else if (msg.MessageId == null)
                    error = new Error("MessageId missing");
                else
                {
                    var accountId = msg.AccountId ?? Storage.Account.GetId(msg.AccountName);

                    // Check if the user is authenticated and has rights
                    var authentication = Authorized(accountId);
                    if (authentication.HasRights)
                    {
                        Storage.Msg.Tag(accountId, msg.MessageId.GetValueOrDefault());

                        //Result is an empty error
                        error = new Error();
                    }
                    else
                        error = new Error(authentication.ErrorMessage());                    
                }
            }
            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                error = new Error(exception.Code.ToString());
            }
            catch (InvalidOperationException exception)
            {
                error = new Error(exception.Message + " " + exception.InnerException.Message);
            }

            return Serialize(new Answer(error));
        }

        
        //
        // POST : /message/untag

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Untag()
        {
            Error error;

            try
            {
                var msg = (Untag) (new XmlSerializer(typeof (Untag))).Deserialize(Request.InputStream);

                if (msg.AccountId == null && msg.AccountName == null)
                    error = new Error("AccountId or AccountName missing");
                else if (msg.MessageId == null)
                    error = new Error("MessageId missing");
                else
                {
                    var accountId = msg.AccountId ?? Storage.Account.GetId(msg.AccountName);

                    // Check if the user is authenticated and has rights
                    var authentication = Authorized(accountId);
                    if (authentication.HasRights)
                    {
                        Storage.Msg.Untag(accountId, msg.MessageId.GetValueOrDefault());
                        //Result is an empty error
                        error = new Error();
                    }
                    else
                        error = new Error(authentication.ErrorMessage());

                }
            }
            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                error = new Error(exception.Code.ToString());
            }
            catch (InvalidOperationException exception)
            {
                error = new Error(exception.Message + " " + exception.InnerException.Message);
            }

            return Serialize(new Answer(error));
        }

    }
}
