using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using System.Linq;
using System.Xml.Serialization;
using Tigwi.Auth;
using Tigwi.Storage.Library;
using Tigwi.API.Models;

namespace Tigwi.API.Controllers
{
    public abstract class ApiController : Controller
    {

        // Initialize storage when instanciating a controller
        protected ApiController ()
        {
            Storage = new Storage.Library.Storage("__AZURE_STORAGE_ACCOUNT_NAME", "__AZURE_STORAGE_ACCOUNT_KEY");
        }


        // General methods used in any controller

        protected Authentication Authorized(Guid account)
        {
            // Key must be sent in a cookie
            var keyCookie = Request.Cookies.Get("key");
            
            // Check if the cookie exist
            var authentication = new Authentication {Tried = keyCookie != null};

            if (keyCookie != null)
            {
                try
                {
                    var user = (new ApiKeyAuth(Storage, keyCookie.Value)).Authenticate();
                    var users = Storage.Account.GetUsers(account);
                    authentication.HasRights = users.Contains(user);

                }
                catch (AuthFailedException)
                {
                    authentication.Failed = true;
                }
            }

            return authentication;
        }

        protected ContentResult Serialize(Answer output)
        {
            // a stream is needed for serialization
            var stream = new MemoryStream();
            (new XmlSerializer(typeof(Answer))).Serialize(stream, output);

            stream.Position = 0;
            return Content((new StreamReader(stream)).ReadToEnd());
        }

        // Methods to build the lists to return
        protected static Accounts AccountsFromGuidCollection(ICollection<Guid> hashAccounts, int size, IStorage storage)
        {
            var accounts = from accountId in hashAccounts.Take(size)
                           let accountInfo = storage.Account.GetInfo(accountId)
                           select new Account(accountId, accountInfo.Name, accountInfo.Description);

            return new Accounts(accounts.ToList());  
        }

        protected static Lists ListsFromGuidCollection(ICollection<Guid> hashLists, int size, IStorage storage )
        {
            var lists = from listId in hashLists.Take(size)
                        select new ListApi(listId, storage.List.GetInfo(listId).Name);

            return new Lists(lists.ToList());
        }

        protected IStorage Storage;

    }

    public class Authentication
    {
        public bool Tried { get; set; }
        public bool Failed { get; set; } // Cannot failed if not tried
        public bool HasRights { get; set; }

        public string ErrorMessage()
        {
            if (!Tried)
                return "No key cookie was sent";
            if (Failed)
                return "Authentication failed";
            return !HasRights ? "User hasn't rights on this account" : null;
        }
    }
}
