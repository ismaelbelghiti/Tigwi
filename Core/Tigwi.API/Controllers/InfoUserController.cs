using System.Web.Mvc;
using Tigwi.API.Models;
using Tigwi.Auth;
using Tigwi.Storage.Library;

namespace Tigwi.API.Controllers
{
    public class InfoUserController : ApiController
    {
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
                    var userId = (new ApiKeyAuth(Storage, keyCookie.Value)).Authenticate();
                    var userInfo = Storage.User.GetInfo(userId);
                    var userToReturn = new User(userInfo, userId);
                    output = new Answer(userToReturn);
                }
            }
            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                output = new Answer(new Error(exception.Code.ToString()));
            }
            catch (AuthFailedException)
            {
                output = new Answer(new Error("Authentication failed"));
            }

            return Serialize(output);
        }
    }
}
