using System;
using System.IO;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Xml.Serialization;
using System.Linq;
using StorageLibrary;
using Tigwi_API.Models;

namespace Tigwi_API.Controllers
{
    public class InfoListController : ApiController
    {
        //
        // GET : /infolist/subscriptions/{idOfList}/{number}

        public ActionResult Subscriptions(Guid idOfList, int number)
        {
            Answer output;

            try
            {
                // get accounts followed by the given list 
                var followedAccounts = Storage.List.GetAccounts(idOfList);

                var numberToReturn = Math.Min(number, followedAccounts.Count);
                var followedAccountsToReturn = BuildAccountListFromGuidCollection(followedAccounts, numberToReturn, Storage);

                output = new Answer(followedAccountsToReturn);
            }

            catch (StorageLibException exception)
            {
                output = new Answer(new Error(exception.Code.ToString()));
            }

            var stream = new MemoryStream();
            (new XmlSerializer(typeof(Answer))).Serialize(stream, output);

            return Content(stream.ToString());
        }

        //
        // GET : /infolist/subscribers/{idOfList}/{number}

        public ActionResult Subscribers(Guid idOfList, int number)
        {
            Answer output;

            try
            {
                // get accounts following a given list 
                var listSuscriberAccounts = Storage.List.GetFollowingAccounts(idOfList);

                var numberToReturn = Math.Min(number, listSuscriberAccounts.Count);
                var listSuscribersOutputToReturn = BuildAccountListFromGuidCollection(listSuscriberAccounts, numberToReturn, Storage);

                output = new Answer(listSuscribersOutputToReturn);
            }
            catch (StorageLibException exception)
            {
                output = new Answer(new Error(exception.Code.ToString()));
            }

            var stream = new MemoryStream();
            (new XmlSerializer(typeof(Answer))).Serialize(stream, output);

            return Content(stream.ToString());
        }


        //
        // GET : /infolist/owner/{idOfList}

        public ActionResult Owner(Guid idOfList)
        {
            Answer output;

            try
            {
                // get accounts following a given list 
                var ownerId = Storage.List.GetOwner(idOfList);
                var ownerInfo = Storage.Account.GetInfo(ownerId);
                var ownerToReturn = new Account(ownerId, ownerInfo.Name, ownerInfo.Description);
                output = new Answer(ownerToReturn);
            }

            catch (StorageLibException exception)
            {
                output = new Answer(new Error(exception.Code.ToString()));
            }

            var stream = new MemoryStream();
            (new XmlSerializer(typeof(Answer))).Serialize(stream, output);

            return Content(stream.ToString());
        }

        //
        // GET : /infolist/messages/{idOfList}/{number}

        public ActionResult Messages(Guid idOfList, int number)
        {
            Answer output;

            try
            {
                // get lasts messages from list defined by idOfList
                var listMsgs = Storage.Msg.GetListsMsgTo(new HashSet<Guid> { idOfList }, DateTime.Now, number);

                // convert, looking forward XML serialization
                var listMsgsOutput = new Messages(listMsgs, Storage);

                output = new Answer(listMsgsOutput);
            }
            catch (StorageLibException exception)
            {
                output = new Answer(new Error(exception.Code.ToString()));
            }

            var stream = new MemoryStream();
            (new XmlSerializer(typeof(Answer))).Serialize(stream, output);

            return Content(stream.ToString());
        }

    }
}
