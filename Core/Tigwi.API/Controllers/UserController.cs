using System.Web.Mvc;
using Tigwi.API.Models;
using Tigwi.Auth;
using Tigwi.Storage.Library;
using System.Xml.Serialization;
using System;
using System.Linq;
using System.Web;


namespace Tigwi.API.Controllers
{
    public class UserController : ApiController
    {
        // TODO : handle authentication correctly
        //
        // GET : /user/maininfo
        // The user you get the info depend on who you are according to authentication
        public ActionResult MainInfo()
        {
            Answer output;

            try
            {
                // Key must be sent in a cookie
                var keyCookie = Request.Cookies.Get("key");

                if (keyCookie == null)
                    output = new Answer(new Error("No key cookie was sent"));
                else
                {
                    var userId = (new ApiKeyAuth(Storage, new Guid(keyCookie.Value))).Authenticate();
                    var userInfo = Storage.User.GetInfo(userId);
                    var userToReturn = new User(userInfo, userId);
                    output = new Answer(userToReturn);
                }
            }
            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                output = new Answer(new Error(exception.Code.ToString()));

                // In the case of a "not found" exception we change the HTTP status
                if (exception.Code == StrgLibErr.MessageNotFound || exception.Code == StrgLibErr.ListNotFound || exception.Code == StrgLibErr.UserNotFound || exception.Code == StrgLibErr.AccountNotFound)
                    Response.StatusCode = 404;
            }
            catch (AuthFailedException)
            {
                output = new Answer(new Error("Authentication failed"));
            }

            return Serialize(output);
        }


        //
        // POST : user/generatekey
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GenerateKey()
        {
            try
            {
                var idParameters = (Identity)(new XmlSerializer(typeof(Identity))).Deserialize(Request.InputStream);
                var userId = idParameters.UserId ?? Storage.User.GetId(idParameters.UserLogin);

                // We catch the real password to verify if both are the same
                var hashedPassword = Storage.User.GetPassword(userId);
                var otherHash = PasswordAuth.HashPassword(idParameters.Password);

                // If both passwords are the same, we can generate the key
                if (hashedPassword.SequenceEqual(otherHash)) 
                {
                    var key = Storage.User.GenerateApiKey(userId, idParameters.ApplicationName);
                    Response.SetCookie(new HttpCookie("key=" + key));
                }
                else 
                    throw new AuthFailedException();                
            }
            catch (UserNotFound)
            {
                // In the case of a "not found" exception we change the HTTP status
                Response.StatusCode = 404;
            }
            catch (InvalidOperationException)
            {
                Response.StatusCode = 403; // TODO : check if this is the right code
            }

            return new EmptyResult();
        }
    }
}
