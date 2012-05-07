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
        // POST : /modifylist/subscribeaccount/

        //[Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SubscribeAccount()
        {
            Error error;

            try
            {
                var subscribeAccount =
                    (ListAndAccount) (new XmlSerializer(typeof (ListAndAccount))).Deserialize(Request.InputStream);

                if (subscribeAccount.Account == null)
                    error = new Error("Account missing");
                else if (subscribeAccount.List == null)
                    error = new Error("List missing");
                else
                {
                    try
                    {
                        var accountId = Storage.Account.GetId(subscribeAccount.Account);
                        Storage.List.Add(subscribeAccount.List.GetValueOrDefault(), accountId);

                        // Result is an empty error XML element
                        error = new Error();
                    }
                    catch (StorageLibException exception)
                    {
                        // Result is an non-empty error XML element
                        error = new Error(exception.Code.ToString());
                    }
                }
            }
            catch (InvalidOperationException exception)
            {
                error = new Error(exception.Message + " " + exception.InnerException.Message);
            }

            return Serialize(new Answer(error));
        }

        //
        // POST : /modifylist/unsubscribeaccount/

        //[Authorize]
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
                    try
                    {
                        var accountId = Storage.Account.GetId(unsubscribeAccount.Account);
                        Storage.List.Remove(unsubscribeAccount.List.GetValueOrDefault(), accountId);

                        // Result is an empty error XML element
                        error = new Error();
                    }
                    catch (StorageLibException exception)
                    {
                        // Result is an non-empty error XML element
                        error = new Error(exception.Code.ToString());
                    }
                }
            }
            catch (InvalidOperationException exception)
            {
                error = new Error(exception.Message + " " + exception.InnerException.Message);
            }

            return Serialize(new Answer(error));
        }

        // WARNING : The following methods are redundant with the ones in ModifyAccount (which is the right place)

        /*
        //
        // POST : /modifylist/followlist/

        //[Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult FollowList()
        {
            Error error;

            try
            {
                var followList =
                    (ListAndAccount) (new XmlSerializer(typeof (ListAndAccount))).Deserialize(Request.InputStream);

                if (followList.Account == null)
                    error = new Error("Account missing");
                else if (followList.List == null)
                    error = new Error("List missing");
                else
                {
                    try
                    {
                        var accountId = Storage.Account.GetId(followList.Account);
                        Storage.List.Follow(followList.List.GetValueOrDefault(), accountId);

                        // Result is an empty error XML element
                        error = new Error();
                    }
                    catch (StorageLibException exception)
                    {
                        // Result is an non-empty error XML element
                        error = new Error(exception.Code.ToString());
                    }
                }
            }
            catch (InvalidOperationException exception)
            {
                error = new Error(exception.Message + " " + exception.InnerException.Message);
            }

            return Serialize(new Answer(error));
        }
        */

        /*
        //
        // POST : /modifylist/unfollowlist/

        //[Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UnfollowList()
        {
            Error error;

            try
            {
                var unfollowList =
                    (ListAndAccount) (new XmlSerializer(typeof (ListAndAccount))).Deserialize(Request.InputStream);

                if (unfollowList.Account == null)
                    error = new Error("Account missing");
                else if (unfollowList.List == null)
                    error = new Error("List missing");
                else
                {
                    try
                    {
                        var accountId = Storage.Account.GetId(unfollowList.Account);
                        Storage.List.Unfollow(unfollowList.List.GetValueOrDefault(), accountId);

                        // Result is an empty error XML element
                        error = new Error();
                    }
                    catch (StorageLibException exception)
                    {
                        // Result is an non-empty error XML element
                        error = new Error(exception.Code.ToString());
                    }
                }
            }
            catch (InvalidOperationException exception)
            {
                error = new Error(exception.Message + " " + exception.InnerException.Message);
            }

            return Serialize(new Answer(error));
        }
        */
    }
}
