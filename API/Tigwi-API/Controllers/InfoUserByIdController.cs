using System;
using System.IO;
using System.Web.Mvc;
using System.Xml.Serialization;
using StorageLibrary;
using Tigwi_API.Models;

namespace Tigwi_API.Controllers
{
    public class InfoUserByIdController : InfoUserController
    {

        //
        // GET : /infouser/maininformations/{userId}
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

            // a stream is needed for serialization
            var stream = new MemoryStream();
            (new XmlSerializer(typeof(Answer))).Serialize(stream, output);

            return Content(stream.ToString());
        }

        
        //
        // GET : /infouser/authorizedaccounts/{userId}/{number}
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

            // a stream is needed for serialization
            var stream = new MemoryStream();
            (new XmlSerializer(typeof(Answer))).Serialize(stream, output);

            return Content(stream.ToString());
        }
    }
}
