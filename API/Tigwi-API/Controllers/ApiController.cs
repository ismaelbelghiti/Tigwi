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
    public class ApiController : Controller
    {

        // Initialize storage when instanciating a controller
        public ApiController ()
        {
            Storage = new Storage("__AZURE_STORAGE_ACCOUNT_NAME", "__AZURE_STORAGE_ACCOUNT_KEY");
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

        // General methods

        //
        // POST : /createuser
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateUser()
        {
            Answer output;

            try
            {
                var user = (NewUser) (new XmlSerializer(typeof (NewUser))).Deserialize(Request.InputStream);
                
                if (user.Login == null)
                    output = new Answer(new Error("Login missing"));
                else if (user.Email == null) // TODO ? More checkings on email
                    output = new Answer(new Error("Email missing"));
                else if (user.Password == null) // TODO ? More checkings on password
                    output = new Answer(new Error("Password missing"));
                else
                {
                    try
                    {
                        // TODO : use new version 
                        //var userId = Storage.User.Create(user.Login, user.Email, user.Password);
                        var userId = new Guid(); 

                        // Result is an empty error XML element
                        output = new Answer(new ObjectCreated(userId));
                    }
                    catch (StorageLibException exception)
                    {
                        // Result is an non-empty error XML element
                        output = new Answer(new Error(exception.Code.ToString()));
                    }
                }
            }
            catch(InvalidOperationException exception)
            {
                output = new Answer(new Error(exception.Message + " " + exception.InnerException.Message));
            }

            return Serialize(output);
        }

        protected IStorage Storage;

    }
}
