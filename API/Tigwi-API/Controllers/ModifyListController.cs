using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;
using Tigwi_API.Models;
using StorageLibrary;

namespace Tigwi_API.Controllers
{
    public class ModifyListController : ApiController
    {
        //
        // POST : /modify/listsubscribeaccount/

        //[Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ListSubscribeAccount(ListSubscribe listSubscribe)
        {
            //TODO : use appropriate storage connexion
            IStorage storage = new StorageTmp(); // connexion
            Error error;

            try
            {
                var accountId = storage.Account.GetId(listSubscribe.Subscription);
                storage.List.Add(listSubscribe.List, accountId);

                // Result is an empty error XML element
                error = new Error();
            }
            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                error = new Error(exception.Code.ToString());
            }

            var stream = new MemoryStream();
            (new XmlSerializer(typeof(Error))).Serialize(stream, error);

            return Content(stream.ToString());
        }

    }
}
