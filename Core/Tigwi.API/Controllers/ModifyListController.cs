using System;
using System.Web.Mvc;
using System.Xml.Serialization;
using Tigwi.API.Models;
using Tigwi.Storage.Library;

namespace Tigwi.API.Controllers
{
    public class ModifyListController : ApiController
    {
        //
        // POST : /list/subscribeaccount/

        // TODO : Authorize
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SubscribeAccount()
        {
            Error error;

            try
            {
                var subscribeAccount =
                    (ListAndAccount)(new XmlSerializer(typeof(ListAndAccount))).Deserialize(Request.InputStream);

                if (subscribeAccount.Account == null)
                    error = new Error("Account missing");
                else if (subscribeAccount.List == null)
                    error = new Error("List missing");
                else
                {
                    var accountId = Storage.Account.GetId(subscribeAccount.Account);
                    Storage.List.Add(subscribeAccount.List.GetValueOrDefault(), accountId);

                    // Result is an empty error XML element
                    error = new Error();
                }
            }
            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                error = new Error(exception.Code.ToString());
            }
            catch (InvalidOperationException exception)
            {
                error = new Error(exception.Message + " " + exception.InnerException.Message);
            }

            return Serialize(new Answer(error));
        }

        //
        // POST : /list/unsubscribeaccount/

        // TODO : Authorize
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UnsubscribeAccount()
        {
            Error error;

            try
            {
                var unsubscribeAccount =
                    (ListAndAccount) (new XmlSerializer(typeof (ListAndAccount))).Deserialize(Request.InputStream);

                if (unsubscribeAccount.Account == null)
                    error = new Error("Account missing");
                else if (unsubscribeAccount.List == null)
                    error = new Error("List missing");
                else
                {
                    var accountId = Storage.Account.GetId(unsubscribeAccount.Account);
                    Storage.List.Remove(unsubscribeAccount.List.GetValueOrDefault(), accountId);

                    // Result is an empty error XML element
                    error = new Error();
                }
            }
            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                error = new Error(exception.Code.ToString());
            }
            catch (InvalidOperationException exception)
            {
                error = new Error(exception.Message + " " + exception.InnerException.Message);
            }

            return Serialize(new Answer(error));
        }
        
    }
}
