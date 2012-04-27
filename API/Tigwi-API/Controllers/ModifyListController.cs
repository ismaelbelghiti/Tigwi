using System;
using System.IO;
using System.Web.Mvc;
using System.Xml.Serialization;
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
        public ActionResult SubscribeAccount(SubscribeAccount subscribeAccount)
        {
            Error error;

            try
            {
                var accountId = Storage.Account.GetId(subscribeAccount.Subscription);
                Storage.List.Add(subscribeAccount.List, accountId);

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

        //
        // POST : /modifylist/unsubscribeaccount/{idOfList}/{accountId}

        //[Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UnsubscribeAccount(Guid idOfList, Guid accountId)
        {
            Error error;

            try
            {
                Storage.List.Add(idOfList, accountId);

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


        //
        // POST : /modifylist/followlist/{idOfList}/{accountId}

        //[Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult FollowList(Guid idOfList, Guid accountId)
        {
            Error error;

            try
            {
                Storage.List.Follow(idOfList, accountId);

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

        //
        // POST : /modifylist/unfollowlist/{idOfList}/{accountId}

        //[Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UnfollowList(Guid idOfList, Guid accountId)
        {
            Error error;

            try
            {
                Storage.List.Unfollow(idOfList, accountId);

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
