using System;
using System.Web.Mvc;
using Tigwi.Storage.Library;
using Tigwi.API.Models;

namespace Tigwi.API.Controllers
{
    public class InfoUserByIdController : InfoUserController
    {

        //
        // GET : /infouserbyid/maininfo/{userId}
        public ActionResult MainInfo(Guid userId)
        {
            Answer output;

            try
            {
                output = AnswerMainInfo(userId);
            }

            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                output = new Answer(new Error(exception.Code.ToString()));
            }

            return Serialize(output);
        }

        
        //
        // GET : /infouserbyid/authorizedaccounts/{userId}/{number}
        public ActionResult AuthorizedAccounts(Guid userId, int number)
        {
            Answer output;

            try
            {
                output = AnswerAuthorizedAccounts(userId, number);
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
