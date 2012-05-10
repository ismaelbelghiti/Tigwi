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

        protected bool CheckAuthentication(string key, Guid account)
        {
            try
            {
                var user = (new ApiKeyAuth(Storage, key)).Authenticate();
                var users = Storage.Account.GetUsers(account);
                return users.Contains(user);
            }
            catch (AuthFailedException)
            {
                return false;
            }
        }

        // TODO : look at IDisposable
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
}
