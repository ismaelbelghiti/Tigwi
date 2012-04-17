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
        protected static Accounts BuildAccountListFromGuidCollection(ICollection<Guid> hashAccounts, int size, IStorage storage)
        {
            var accountList = new List<Account>();
            for (var k = 0; k < size; k++)
            {
                var accountId = hashAccounts.First();
                var account = new Account(accountId, storage.Account.GetInfo(accountId).Name);
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
     
    }
}
