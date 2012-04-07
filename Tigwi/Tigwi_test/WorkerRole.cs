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
                storage.User.SetInfo(userid, "test", "email@test2.com");
                Trace.WriteLine("Error, User.SetInfo does not detect existing user");
            }
            catch (UserAlreadyExists)
            {
                ;
            }

            try
            {
                storage.User.SetInfo(userid, "settest", "email@test.com");
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
                storage.Account.SetInfo(accountid, "test", "descriptiontest2");
                Trace.WriteLine("Error, User.SetInfo does not detect existing user");
            }
            catch (UserAlreadyExists)
            {
                ;
            }

            try
            {
                storage.Account.SetInfo(accountid, "nametest", "descriptiontest");
                IAccountInfo newaccountinfo = storage.Account.GetInfo(accountid);
                if (newaccountinfo.Name != "nametest" || newaccountinfo.Description != "descriptiontest")
                    Trace.WriteLine("Error, Account.SetInfo");
            }
            catch (UserNotFound)
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

            Guid admin = storage.Account.GetAdminId(accountid);
            if (admin != userid)
                Trace.WriteLine("Error, Account.GetAdmin");

            storage.Account.SetAdminId(accountid, useridbis);
            admin = storage.Account.GetAdminId(accountid);
            if (admin != useridbis)
                Trace.WriteLine("Error, Account.SetAdmin");


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
