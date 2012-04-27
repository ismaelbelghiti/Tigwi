using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using System.Xml.Serialization;
using StorageLibrary;
using Tigwi_API.Models;

namespace Tigwi_API.Controllers
{
    public class InfoUserController : ApiController
    {
        //
        //GET infouser/maininformations/{userLogin}
        public ActionResult MainInfo(string userLogin)
        {
            Answer output;

            try
            {
                // get the informations on the account

                var userId = Storage.User.GetId(userLogin);
                var userInfo = Storage.User.GetInfo(userId);
                var userToReturn = new User(userInfo, userId);
                output = new Answer(userToReturn);
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
        //GET infouser/maininformations/{userId}
        public ActionResult MainInfo(Guid userId)
        {
            Answer output;

            try
            {
                // get the informations on the account
                var userInfo = Storage.User.GetInfo(userId);
                var userToReturn = new User(userInfo, userId);
                output = new Answer(userToReturn);
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
        // GET : /infoaccount/subscribedaccounts/{userLogin}/{number}
        public ActionResult SubscribedAccounts(string userLogin, int number)
        {
            Answer output;

            try
            {
                // get the public lists followed by the given account

                var userId = Storage.Account.GetId(userLogin);
                var authorizedAccounts = Storage.User.GetAccounts(userId);
                var authorizedAccountsInList = new HashSet<Guid>();
                foreach (var followedList in authorizedAccounts)
                {
                    authorizedAccountsInList.UnionWith(Storage.List.GetAccounts(followedList));
                }

                // Get as many subscriptions as possible (maximum: numberOfSubscriptions)
                var size = Math.Min(authorizedAccountsInList.Count, number);
                var authorizedAccountsToReturn = BuildAccountListFromGuidCollection(authorizedAccountsInList, size, Storage);

                output = new Answer(authorizedAccountsToReturn);
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
