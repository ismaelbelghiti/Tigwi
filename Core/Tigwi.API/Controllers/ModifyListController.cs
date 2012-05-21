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

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SubscribeAccount()
        {
            Error error;

            try
            {
                var subscribeAccount =
                    (ListAndAccount)(new XmlSerializer(typeof(ListAndAccount))).Deserialize(Request.InputStream);

                if (subscribeAccount.AccountName == null && subscribeAccount.AccountId == null)
                    error = new Error("Account missing");
                else if (subscribeAccount.List == null)
                    error = new Error("List missing");
                else
                {
                    var ownerId = Storage.List.GetOwner(subscribeAccount.List.GetValueOrDefault());
                    // Check if the user is authenticated and has rights
                    var authentication = Authorized(ownerId);

                    if (authentication.HasRights)
                    {
                        var accountId = subscribeAccount.AccountId ?? Storage.Account.GetId(subscribeAccount.AccountName);
                        Storage.List.Add(subscribeAccount.List.GetValueOrDefault(), accountId);

                        // Result is an empty error XML element
                        error = new Error();
                    }
                    else
                        error = new Error(authentication.ErrorMessage());
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

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UnsubscribeAccount()
        {
            Error error;

            try
            {
                var unsubscribeAccount =
                    (ListAndAccount) (new XmlSerializer(typeof (ListAndAccount))).Deserialize(Request.InputStream);

                if (unsubscribeAccount.AccountName == null && unsubscribeAccount.AccountId == null)
                    error = new Error("Account missing");
                else if (unsubscribeAccount.List == null)
                    error = new Error("List missing");
                else
                {
                    var ownerId = Storage.List.GetOwner(unsubscribeAccount.List.GetValueOrDefault());
                    // Check if the user is authenticated and has rights
                    var authentication = Authorized(ownerId);

                    if (authentication.HasRights)
                    {
                        var accountId = unsubscribeAccount.AccountId ?? Storage.Account.GetId(unsubscribeAccount.AccountName);
                        Storage.List.Remove(unsubscribeAccount.List.GetValueOrDefault(), accountId);

                        // Result is an empty error XML element
                        error = new Error();
                    }
                    else
                        error = new Error(authentication.ErrorMessage());
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
