using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using System.Linq;
using System.Xml.Serialization;
using StorageLibrary;
using Tigwi_API.Models;

namespace Tigwi_API.Controllers
{
    public abstract class ApiController : Controller
    {
        protected ApiController ()
        {
            // TODO : give the actual connexion informations
            Storage = new Storage("__AZURE_STORAGE_ACCOUNT_NAME", "__AZURE_STORAGE_ACCOUNT_KEY");
        }

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

        public ActionResult Createuser(NewUser user)
        {
            Answer answer;

            try
            {
                var userId = Storage.User.Create(user.Login, user.Email, user.Password);

                // Result is an empty error XML element
                answer = new Answer(new ObjectCreated(userId));
            }
            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                answer = new Answer(new Error(exception.Code.ToString()));
            }

            var stream = new MemoryStream();
            (new XmlSerializer(typeof(Error))).Serialize(stream, answer);

            return Content(stream.ToString());
        }

        protected IStorage Storage;

    }
}
