using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using System.Linq;
using System.Xml.Serialization;
using Tigwi.Storage.Library;
using Tigwi.API.Models;

namespace Tigwi.API.Controllers
{
    public abstract class ApiController : Controller
    {

        // Initialize storage when instanciating a controller
        protected ApiController ()
        {
            Storage = new Storage.Library.Storage("sefero", "GU0GjvcPoXKzDFgBSPFbWmCPQrIRHAT6fholbMnxtteY5vQVgYTcWKk/25i/F4m9MFoGHXNf4oYgeAKo+mFO5Q==");
        }

        protected bool CheckAuthentification (string key)
        {
            return true;
        }

        protected ContentResult Serialize(Answer output)
        {
            // a stream is needed for serialization
            var stream = new MemoryStream();
            (new XmlSerializer(typeof(Answer))).Serialize(stream, output);

            stream.Position = 0;
            return Content((new StreamReader(stream)).ReadToEnd());
        }

        // Methods to build lists used in any controller
        // TODO : modify to use foreach or LINQ
        protected static Accounts BuildAccountListFromGuidCollection(ICollection<Guid> hashAccounts, int size, IStorage storage)
        {
            var accounts = from accountId in hashAccounts.Take(size)
                              let accountInfo = storage.Account.GetInfo(accountId)
                              select new Account(accountId, accountInfo.Name, accountInfo.Description);

            return new Accounts(accounts.ToList());  
        }

        protected static Lists BuildListsFromGuidCollection(ICollection<Guid> hashLists, int size, IStorage storage )
        {
            var lists = from listId in hashLists.Take(size)
                        select new ListApi(listId, storage.List.GetInfo(listId).Name);

            return new Lists(lists.ToList());
        }

        protected static Users BuilUserListFormGuidCollection(ICollection<Guid> hashUsers, int size, IStorage storage)
        {
            var users = from userId in hashUsers.Take(size)
                        select new User(storage.User.GetInfo(userId), userId);

            return new Users(users.ToList()); 
        }
        
        protected IStorage Storage;

    }
}
