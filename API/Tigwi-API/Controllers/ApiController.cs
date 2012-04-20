using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using StorageLibrary;
using Tigwi_API.Models;

namespace Tigwi_API.Controllers
{
    public abstract class ApiController : Controller
    {
        protected ApiController ()
        {
            // TODO : give the actual connexion informations
            Storage = new Storage("devstoreaccount1","Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==");
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
                var user = new User(storage.User.GetInfo(userId));
                users.Add(user);
                hashUsers.Remove(userId);
            }

            return new Users(users); 
        }

        protected IStorage Storage;

    }
}
