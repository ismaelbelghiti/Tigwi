using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;
using StorageLibrary;
using Tigwi_API.Models;

namespace Tigwi_API.Controllers
{
    public class ModifyUserController : ApiController
    {
        //
        // POST modifyuser/changeemail

        //[Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ChangeEmail(ChangeInfo infos)
        {
            Error error;

            try
            {
                var userId = infos.UserId;
                //TODO: find how to test userId to know if the information was sent or not
                //if (userId == )
                //    userId = Storage.User.GetId(infos.UserLogin);

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

            var stream = new MemoryStream();
            (new XmlSerializer(typeof(Error))).Serialize(stream, error);

            return Content(stream.ToString());
        }

        //
        // POST modifyuser/changeavatar

        //[Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ChangeAvatar(ChangeInfo infos)
        {
            Error error;

            try
            {
                var userId = infos.UserId;
                //TODO: find how to test userId to know if the information was sent or not
                //if (userId == )
                //    userId = Storage.User.GetId(infos.UserLogin);

                //TODO: come back to this when storage will hav implemented change avatar method
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

            var stream = new MemoryStream();
            (new XmlSerializer(typeof(Error))).Serialize(stream, error);

            return Content(stream.ToString());
        }

        //
        // POST modifyuser/changepassword

        //[Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ChangePassword(ChangePassword infos)
        {
            Error error;

            try
            {
                var userId = infos.UserId;
                //TODO: find how to test userId to know if the information was sent or not
                //if (userId == )
                //    userId = Storage.User.GetId(infos.UserLogin);

                //TODO: implement old password checking
                //Check the old password
               // var password = Storage.User.GetPassword(userId);
               // if (password != infos.OldPassword)
               //     return whatsnecessary (raise an error)

                //Set the password
                Storage.User.SetPassword(userId, infos.NewPassword);

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
        // POST modifyuser/createaccount

        //[Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateAccount(CreateAccount infos)
        {
            Answer answer;

            try
            {
                var userId = infos.UserId;
                //TODO: find how to test userId to know if the information was sent or not
                //if (userId == )
                //    userId = Storage.User.GetId(infos.UserLogin);

                //Create the account and get the infos
                var accountId = Storage.Account.Create(userId, infos.AccountName, infos.Description);
                var accountInfo = Storage.Account.GetInfo(accountId);
               
                // Result
                answer = new Answer( new Account(accountId, accountInfo.Name, accountInfo.Description) );

            }
            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                answer = new Answer( new Error(exception.Code.ToString()));
            }

            var stream = new MemoryStream();
            (new XmlSerializer(typeof(Error))).Serialize(stream, answer);

            return Content(stream.ToString());
        }

    }
}
