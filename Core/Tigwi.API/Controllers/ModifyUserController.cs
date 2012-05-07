// WARNING : All these methods are (unfortunately) a little useless and maybe not good in an API

using System;
using System.Web.Mvc;
using System.Xml.Serialization;
using Tigwi.Storage.Library;
using Tigwi.API.Models;

namespace Tigwi.API.Controllers
{
    public class ModifyUserController : ApiController
    {
        //
        // POST modifyuser/changeemail

        //[Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ChangeEmail()
        {
            Error error;

            try
            {
                var infos = (ChangeInfo) (new XmlSerializer(typeof (ChangeInfo))).Deserialize(Request.InputStream);

                if (infos.UserId == null && infos.UserLogin == null)
                    error = new Error("UserId or UserLogin missing");
                else if (infos.Info == null) // TODO ? More checkings on email
                    error = new Error("Info missing");
                else
                {
                    try
                    {
                        var userId = infos.UserId ?? Storage.User.GetId(infos.UserLogin);

                        //Set the mail address
                        Storage.User.SetInfo(userId, infos.Info);

                        // Result is an empty error XML element
                        error = new Error();
                    }
                    catch (StorageLibException exception)
                    {
                        // Result is an non-empty error XML element
                        error = new Error(exception.Code.ToString());
                    }
                }
            }
            catch (InvalidOperationException exception)
            {
                error = new Error(exception.Message + " " + exception.InnerException.Message);
            }

            return Serialize(new Answer(error));
        }

        //
        // POST modifyuser/changeavatar

        //[Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ChangeAvatar()
        {
            Error error;

            try
            {
                var infos = (ChangeInfo) (new XmlSerializer(typeof (ChangeInfo))).Deserialize(Request.InputStream);

                if (infos.UserId == null && infos.UserLogin == null)
                    error = new Error("UserId or UserLogin missing");
                else if (infos.Info == null) // TODO ? More checkings on avatar
                    error = new Error("Info missing");
                else
                {
                    try
                    {
                        //var userId = infos.UserId ?? Storage.User.GetId(infos.UserLogin);

                        //TODO: come back to this when storage will have implemented change avatar method
                        //Set the mail address
                        //Storage.User.ChangeAvatar(userId, infos.Info);

                        // Result is an empty error XML element
                        error = new Error();
                    }
                    catch (StorageLibException exception)
                    {
                        // Result is an non-empty error XML element
                        error = new Error(exception.Code.ToString());
                    }
                }
            }
            catch (InvalidOperationException exception)
            {
                error = new Error(exception.Message + " " + exception.InnerException.Message);
            }

            return Serialize(new Answer(error));
        }

        //
        // POST modifyuser/changepassword

        //[Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ChangePassword()
        {
            Error error;

            try
            {
                var infos =
                    (ChangePassword) (new XmlSerializer(typeof (ChangePassword))).Deserialize(Request.InputStream);

                if (infos.UserId == null && infos.UserLogin == null)
                    error = new Error("UserId or UserLogin missing");
                else if (infos.NewPassword == null)
                    error = new Error("NewPassword missing");
                else if (infos.OldPassword == null)
                    error = new Error("OldPassword missing");
                else
                {
                    try
                    {
                        var userId = infos.UserId ?? Storage.User.GetId(infos.UserLogin);

                        //TODO: implement old password checking
                        //Check the old password
                        // var password = Storage.User.GetPassword(userId);
                        // if (password != infos.OldPassword)
                        //     return whatsnecessary (raise an error)

                        //Set the password
                        // TODO : use the new version Storage.User.SetPassword(userId, infos.NewPassword);

                        // Result is an empty error XML element
                        error = new Error();
                    }
                    catch (StorageLibException exception)
                    {
                        // Result is an non-empty error XML element
                        error = new Error(exception.Code.ToString());
                    }
                }
            }
            catch (InvalidOperationException exception)
            {
                error = new Error(exception.Message + " " + exception.InnerException.Message);
            }

            return Serialize(new Answer(error));
        }

        //
        // POST modifyuser/createaccount

        //[Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateAccount()
        {
            Answer output;

            try
            {
                var infos = (CreateAccount) (new XmlSerializer(typeof (CreateAccount))).Deserialize(Request.InputStream);

                if (infos.UserId == null && infos.UserLogin == null)
                    output = new Answer(new Error("UserId or UserLogin missing"));
                else if (infos.AccountName == null)
                    output = new Answer(new Error("AccountName missing"));
                else if (infos.Description == null) // TODO ? More checkings on description
                    output = new Answer(new Error("Description missing"));
                else
                {
                    try
                    {
                        var userId = infos.UserId ?? Storage.User.GetId(infos.UserLogin);

                        // Create the account
                        var accountId = Storage.Account.Create(userId, infos.AccountName, infos.Description);
                        // TODO : give the rights to the user

                        // Result
                        output = new Answer(new ObjectCreated(accountId));

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

    }
}
