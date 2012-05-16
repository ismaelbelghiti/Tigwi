using System;
using System.Web.Mvc;
using Tigwi.API.Models;
using Tigwi.Storage.Library;

namespace Tigwi.API.Controllers
{
    public class InfoUserController : ApiController
    {

        // TODO : add "need authentication" in specs
        // TODO : add authentication
        //
        // GET : /user/maininfo
        // The user you get the info depend on who you are according to authentication
        /*
        public ActionResult MainInfo()
        {
            Answer output;

            try
            {
                var userId = ;
                var userInfo = Storage.User.GetInfo(userId);
                var userToReturn = new User(userInfo, userId);
                output = new Answer(userToReturn);
            }

            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                output = new Answer(new Error(exception.Code.ToString()));
            }

            return Serialize(output);
        }*/
    }
}
