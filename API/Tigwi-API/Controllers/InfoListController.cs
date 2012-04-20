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
                // get accounts following a given list 
                list listFollowedAccounts = storage.List.GetAccounts(idOfList);
                var numberToReturn = Math.Min(number, listFollowedAccounts.count);
                List<user> listFollowedAccountsOutput = listFollowedAccounts.GetRange(numberToReturn, 1);

                output = new Answer(listFollowedAccountsOutput);
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
                list listSuscribersAccount = storage.List.GetFollowingAccounts(idOfList);
                var numberToReturn = Math.Min(number, listSuscribersAccount.count);
                List<user> listSuscribersOutput = listSuscribersAccount.GetRange(numberToReturn, 1);

                output = new Answer(listSuscribersOutput);
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
                var owner = storage.List.GetOwner(idOfList);

                output = new Answer(owner);
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
                list listMsgs = storage.Msg.GetListsMsgTo(new HashSet<Guid> { idOfList }, DateTime.Now, number);

                // convert, looking forward XML serialization
                var listMsgsOutput = new Messages(listMsgs, storage);

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
