using System;
using System.Web.Mvc;
using Tigwi_API.Models;
using StorageLibrary;

namespace Tigwi_API.Controllers
{
    public class ModifyListController : ApiController
    {
        //
        // POST : /modifylist/subscribeaccount/

        //[Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SubscribeAccount(ListAndAccount subscribeAccount)
        {
            Error error;

            try
            {
                var accountId = Storage.Account.GetId(subscribeAccount.Account);
                Storage.List.Add(subscribeAccount.List, accountId);

                // Result is an empty error XML element
                error = new Error();
            }
            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                error = new Error(exception.Code.ToString());
            }

            return Serialize(new Answer(error));
        }

        //
        // POST : /modifylist/unsubscribeaccount/

        //[Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UnsubscribeAccount(ListAndAccount unsubscribeAccount)
        {
            Error error;

            try
            {
                var accountId = Storage.Account.GetId(unsubscribeAccount.Account);
                Storage.List.Remove(unsubscribeAccount.List, accountId);

                // Result is an empty error XML element
                error = new Error();
            }
            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                error = new Error(exception.Code.ToString());
            }

            return Serialize(new Answer(error));
        }


        //
        // POST : /modifylist/followlist/

        //[Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult FollowList(ListAndAccount followList)
        {
            Error error;

            try
            {
                var accountId = Storage.Account.GetId(followList.Account);
                Storage.List.Follow(followList.List, accountId);

                // Result is an empty error XML element
                error = new Error();
            }
            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                error = new Error(exception.Code.ToString());
            }

            return Serialize(new Answer(error));
        }

        //
        // POST : /modifylist/unfollowlist/

        //[Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UnfollowList(ListAndAccount unfollowList)
        {
            Error error;

            try
            {
                var accountId = Storage.Account.GetId(unfollowList.Account);
                Storage.List.Unfollow(unfollowList.List, accountId);

                // Result is an empty error XML element
                error = new Error();
            }
            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                error = new Error(exception.Code.ToString());
            }

            return Serialize(new Answer(error));
        }

    }
}
