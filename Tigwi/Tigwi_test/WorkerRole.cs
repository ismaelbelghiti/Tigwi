using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.IO;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using StorageLibrary;
using StorageCommon;

namespace Tigwi_test
{
    public class WorkerRole : RoleEntryPoint
    {
        public override void Run()
        {
            // This is a sample worker implementation. Replace with your logic.
            Trace.WriteLine("$projectname$ entry point called", "Information");

            Debug.Listeners.Add(new TextWriterTraceListener(File.Create(
                                                "StorageTestResult.txt")));
            
            // Test code here
            Storage storage = new Storage("devstoreaccount1", "Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==");

            try
            {
                Guid noneid = storage.User.GetId("test");
                Trace.WriteLine("Error, User.GetId return an id where it shouldn't.");
            }
            catch (UserNotFound)
            {
                ;
            }

            try
            {
                Guid noneid = storage.Account.GetId("test");
                Trace.WriteLine("Error, Account.GetId return an id where it shouldn't.");
            }
            catch (UserNotFound)
            {
                ;
            }


            Guid userid = storage.User.Create("test", "bidon@test.com");
            try 
            {
                Guid sameid = storage.User.GetId("test");
                if (userid != sameid) Trace.WriteLine("Error, User.GetId");
            }
            catch (UserNotFound)
            {
                Trace.WriteLine("Error, User.GetId does not found a new Id.");
            }

            try
            {
                Guid sameuser = storage.User.Create("test", "bidon@test2.com");
                Trace.WriteLine("Error, User.Create does not detect existing user");
            }
            catch (UserAlreadyExists)
            {
                ;
            }
            
            try
            {
                IUserInfo userinfo = storage.User.GetInfo(userid);
                if (userinfo.Login != "test" || userinfo.Email != "bidon@test.com")
                    Trace.WriteLine("Error, User.GetInfo");
            }
            catch (UserNotFound)
            {
                Trace.WriteLine("Error, User.GetInfo does not found a new Id.");
            }

            try
            {
                storage.User.SetInfo(userid, "email@test2.com");
                Trace.WriteLine("Error, User.SetInfo does not detect existing user");
            }
            catch (UserAlreadyExists)
            {
                ;
            }

            try
            {
                storage.User.SetInfo(userid, "email@test.com");
                IUserInfo newuserinfo = storage.User.GetInfo(userid);
                if (newuserinfo.Login != "settest" || newuserinfo.Email != "email@test.com")
                    Trace.WriteLine("Error, User.SetInfo");
            }
            catch (UserNotFound)
            {
                Trace.WriteLine("Error, User.GetInfo does not found an Id.");
            }

            Guid accountid = storage.Account.Create(userid, "test", "description");
            try
            {    
                Guid sameaccountid = storage.Account.GetId("test");
                if (accountid != sameaccountid) Trace.WriteLine("Error, Account.GetId");
            }
            catch (AccountNotFound)
            {
                Trace.WriteLine("Error, Account.GetId does not found a new Id.");
            }

            try
            {
                Guid sameaccount = storage.Account.Create(userid, "test", "description2");
                Trace.WriteLine("Error, Account.Create does not detect existing user");
            }
            catch (AccountAlreadyExists)
            {
                ;
            }

            try
            {
                IAccountInfo accountinfo = storage.Account.GetInfo(accountid);
                if (accountinfo.Name != "test" || accountinfo.Description != "description")
                    Trace.WriteLine("Error, Account.GetInfo");
            }
            catch (UserNotFound)
            {
                Trace.WriteLine("Error, Account.GetInfo does not found a new Id.");
            }

            try
            {
                storage.Account.SetInfo(accountid, "descriptiontest2");
                Trace.WriteLine("Error, User.SetInfo does not detect existing user");
            }
            catch (UserAlreadyExists)
            {
                ;
            }

            try
            {
                storage.Account.SetInfo(accountid, "descriptiontest");
                IAccountInfo newaccountinfo = storage.Account.GetInfo(accountid);
                if (newaccountinfo.Name != "nametest" || newaccountinfo.Description != "descriptiontest")
                    Trace.WriteLine("Error, Account.SetInfo");
            }
            catch (AccountNotFound)
            {
                Trace.WriteLine("Error, Account.GetInfo does not found a new Id.");
            }
            HashSet<Guid> accounts = storage.User.GetAccounts(userid);
            if (! accounts.Contains(accountid))
                Trace.WriteLine("Error, User.GetAccounts");

            HashSet<Guid> users = storage.Account.GetUsers(accountid);
            if (! users.Contains(userid))
                Trace.WriteLine("Error, Account.GetUsers");


            Guid useridbis = storage.User.Create("user2", "user@mail.com");
            storage.Account.Add(accountid, useridbis);
            if (!users.Contains(useridbis))
                Trace.WriteLine("Error, Account.Add");

            Guid accountidbis = storage.Account.Create(useridbis, "account2", "description2");

            Guid admin = storage.Account.GetAdminId(accountid);
            if (admin != userid)
                Trace.WriteLine("Error, Account.GetAdmin");

            storage.Account.SetAdminId(accountid, useridbis);
            admin = storage.Account.GetAdminId(accountid);
            if (admin != useridbis)
                Trace.WriteLine("Error, Account.SetAdmin");

            try
            {
                Guid personalid = storage.List.GetPersonalList(accountid);
                Guid ownerid = storage.List.GetOwner(personalid);
                if (ownerid != accountid)
                    Trace.WriteLine("Error, List.GetOwner doesn't give the right id");
                IListInfo personalinfo = storage.List.GetInfo(personalid);
                if ( ! personalinfo.IsPrivate)
                    Trace.WriteLine("Error, personnal list is not personal");
                storage.List.SetInfo(personalid, "test", "test", true);
                Trace.WriteLine("Error, List.SetInfo doesn't detect personal list");
            }
            catch (AccountNotFound)
            {
                Trace.WriteLine("Error, List.GetPersonalList doesn't find an existing account");
            }
            catch (ListNotFound)
            {
                Trace.WriteLine("Error, List.GetOwner doesn't find an existing list");
            }
            catch (IsPersonnalList)
            { ;}

            try
            {
                Guid listid = storage.List.Create(accountid, "listtest", "listdescription", false);
                Guid ownerbis = storage.List.GetOwner(listid);
                if (ownerbis != accountid)
                    Trace.WriteLine("Error, List.GetOwner returns a wrong id");

                HashSet<Guid> listowned = storage.List.GetAccountOwnedLists(accountid, true);
                if (!listowned.Contains(listid))
                    Trace.WriteLine("Error, GetAccountOwnedLists doesn't find an owned list or Create doesn't add it");
                listowned = storage.List.GetAccountOwnedLists(accountid, false);
                if (listowned.Contains(listid))
                    Trace.WriteLine("Error, GetAccountOwnedLists returns private lists where it shouldn't");

                IListInfo listinfo = storage.List.GetInfo(listid);
                if (listinfo.Name != "listtest" || listinfo.Description != "listdescription" ||
                    listinfo.IsPrivate || listinfo.IsPersonnal)
                    Trace.WriteLine("Error, List.GetInfo returns wrong infos");
                storage.List.SetInfo(listid, "test", "test", true);
                listinfo = storage.List.GetInfo(listid);
                if (listinfo.Name != "test" || listinfo.Description != "test" ||
                    (!listinfo.IsPrivate) || listinfo.IsPersonnal)
                    Trace.WriteLine("Error, List.SetInfo doesn't set infos");

                storage.List.Follow(listid, accountidbis);
                HashSet<Guid> listfollowed = storage.List.GetAccountFollowedLists(accountidbis, true);
                if (!listfollowed.Contains(listid))
                    Trace.WriteLine("Error, GetAccountFollowedLists doesn't find a followed list or Follow doesn't add it");
                listfollowed = storage.List.GetAccountFollowedLists(accountidbis, false);
                if (listfollowed.Contains(listid))
                    Trace.WriteLine("Error, GetAccountFollowedLists returns private lists where it shouldn't");

                HashSet<Guid> followingaccounts = storage.List.GetFollowingAccounts(listid);
                if (!followingaccounts.Contains(accountidbis))
                    Trace.WriteLine("Error, GetFollowingAccounts doesn't find a following account");

                storage.List.Add(listid, accountidbis);
                HashSet<Guid> listaccounts = storage.List.GetAccounts(listid);
                if (!listaccounts.Contains(accountidbis))
                    Trace.WriteLine("Error, List.GetAccounts doesn't find a followed list or List.Add doesn't add it");

                storage.List.Remove(listid, accountidbis);
                listaccounts = storage.List.GetAccounts(listid);
                if (listaccounts.Contains(accountidbis))
                    Trace.WriteLine("Error, List.Remove doesn't remove an account from a list");
            }
            catch (ListNotFound)
            {
                Trace.WriteLine("Error, an existing list cannot be found");
            }
            catch (AccountNotFound)
            {
                Trace.WriteLine("Error, an existing account cannot be found");
            }

            try
            {
                Guid listid = storage.List.Create(accountid, "list", "list", false);
                storage.List.Delete(listid);
                IListInfo nonexistent = storage.List.GetInfo(listid);
                Trace.WriteLine("Error, List.Delete doesn't delete the list");
            }
            catch (ListNotFound)
            { ;}

            try
            {
                storage.Account.Remove(accountid, useridbis);
                Trace.WriteLine("Error, Account.Remove doesn't detect admin");
            }
            catch (UserIsAdmin)
            { ;}

            storage.Account.Remove(accountid, userid);
            storage.Account.Delete(accountid);
            storage.User.Delete(userid);
            storage.User.Delete(useridbis);
            try
            {
                Guid nonexistent = storage.Account.GetId("nametest");
                Trace.WriteLine("Error, Account.Delete does not delete.");
            }
            catch (AccountNotFound)
            { ;}
            try
            {
                Guid nonexistent = storage.User.GetId("settest");
                Trace.WriteLine("Error, User.Delete does not delete.");
            }
            catch (UserNotFound)
            { ;}
            // End test code

            while (true)
            {
                Thread.Sleep(10000);
                Trace.WriteLine("Working", "Information");
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            return base.OnStart();
        }
    }
}
