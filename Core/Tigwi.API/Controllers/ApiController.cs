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
            var accountList = new List<Account>();
            for (var k = 0; k < size; k++)
            {
                var accountId = hashAccounts.First();
                var accountInfo = storage.Account.GetInfo(accountId);
                var account = new Account(accountId, accountInfo.Name, accountInfo.Description);
                accountList.Add(account);
                hashAccounts.Remove(accountId);
            }

            return new Accounts(accountList);  
        }

        protected static Lists BuildListsFromGuidCollection(ICollection<Guid> hashLists, int size, IStorage storage )
        {
            var lists = new List<ListApi>();
            for (var k = 0; k < size; k++)
            {
                var listId = hashLists.First();
                var list = new ListApi(listId, storage.List.GetInfo(listId).Name);
                lists.Add(list);
                hashLists.Remove(listId);
            }

            return new Lists(lists);  
        }

        protected static Users BuilUserListFormGuidCollection(ICollection<Guid> hashUsers, int size, IStorage storage)
        {
            var users = new List<User>();
            for (var k = 0; k < size; k++)
            {
                var userId = hashUsers.First();
                var user = new User(storage.User.GetInfo(userId), userId);
                users.Add(user);
                hashUsers.Remove(userId);
            }

            return new Users(users); 
        }
        
        protected IStorage Storage;

    }
}
