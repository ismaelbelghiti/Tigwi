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
        public ActionResult ChangeEmail()
        {
            var infos = (ChangeInfo)(new XmlSerializer(typeof(ChangeInfo))).Deserialize(Request.InputStream);

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

            return Serialize(new Answer(error));
        }

        //
        // POST modifyuser/changeavatar

        //[Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ChangeAvatar()
        {
            var infos = (ChangeInfo)(new XmlSerializer(typeof(ChangeInfo))).Deserialize(Request.InputStream);

            Error error;

            try
            {
                // var userId = infos.UserId;
                //TODO: find how to test userId to know if the information was sent or not
                // if (userId == )
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

            return Serialize(new Answer(error));
        }

        //
        // POST modifyuser/changepassword

        //[Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ChangePassword()
        {
            var infos = (ChangePassword)(new XmlSerializer(typeof(ChangePassword))).Deserialize(Request.InputStream);

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

            return Serialize(new Answer(error));
        }

        //
        // POST modifyuser/createaccount

        //[Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateAccount()
        {
            var infos = (CreateAccount)(new XmlSerializer(typeof(CreateAccount))).Deserialize(Request.InputStream);

            Answer output;

            try
            {
                var userId = infos.UserId;
                //TODO: find how to test userId to know if the information was sent or not
                //if (userId == )
                //    userId = Storage.User.GetId(infos.UserLogin);

                //Create the account
                var accountId = Storage.Account.Create(userId, infos.AccountName, infos.Description);
               
                // Result
                output = new Answer( new ObjectCreated(accountId) );

            }
            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                output = new Answer( new Error(exception.Code.ToString()));
            }

            return Serialize(output);
        }

    }
}
