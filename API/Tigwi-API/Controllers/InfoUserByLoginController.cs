using System.IO;
using System.Web.Mvc;
using System.Xml.Serialization;
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

            // a stream is needed for serialization
            var stream = new MemoryStream();
            (new XmlSerializer(typeof(Answer))).Serialize(stream, output);

            return Content(stream.ToString());
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

            // a stream is needed for serialization
            var stream = new MemoryStream();
            (new XmlSerializer(typeof(Answer))).Serialize(stream, output);

            return Content(stream.ToString());
        }
    }
}
