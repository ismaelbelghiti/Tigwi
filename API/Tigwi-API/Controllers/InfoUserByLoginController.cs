using System.Web.Mvc;
using StorageLibrary;
using Tigwi_API.Models;

namespace Tigwi_API.Controllers
{
    public class InfoUserByLoginController : InfoUserController
    {

        //
        // GET : /infouser/maininformations/{userLogin}
        public ActionResult MainInfo(string userLogin)
        {
            Answer output;

            try
            {
                output = AnswerMainInfo(Storage.User.GetId(userLogin));
            }

            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                output = new Answer(new Error(exception.Code.ToString()));
            }

            return Serialize(output);
        }

        
        //
        // GET : /infouser/authorizedaccounts/{userLogin}/{number}
        public ActionResult AuthorizedAccounts(string userLogin, int number)
        {
            Answer output;

            try
            {
                output = AnswerAuthorizedAccounts(Storage.Account.GetId(userLogin), number);
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
