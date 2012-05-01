using System.Web.Mvc;
using StorageLibrary;
using Tigwi_API.Models;

namespace Tigwi_API.Controllers
{
    public class InfoUserByLoginController : InfoUserController
    {

        // TEST
        // GET: /infouser/test/{userLogin}/{number}
        public string Test(string userLogin, int number)
        {
            return userLogin + " " + number;
        }

        //
        // GET : /infouser/maininfo/{userLogin}
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
