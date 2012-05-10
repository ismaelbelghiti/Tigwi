using System;
using System.Web.Mvc;
using Tigwi.API.Models;
using Tigwi.Storage.Library;

namespace Tigwi.API.Controllers
{
    public abstract class InfoUserController : ApiController
    {

        //
        // GET : /user/maininfo/{userLogin}
        // GET : /user/maininfo/login={userLogin}
        // GET : /user/maininfo/id={userId}
        public ActionResult MainInfo(string userLogin, Guid? userId)
        {
            Answer output;

            try
            {
                var realId = userId ?? Storage.User.GetId(userLogin);
                var userInfo = Storage.User.GetInfo(realId);
                var userToReturn = new User(userInfo, realId);
                output = new Answer(userToReturn);
            }

            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                output = new Answer(new Error(exception.Code.ToString()));
            }

            return Serialize(output);
        }      
    }
}
