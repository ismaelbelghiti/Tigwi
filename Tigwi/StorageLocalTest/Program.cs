using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StorageCommon;
using StorageLibrary;
using Microsoft.WindowsAzure.StorageClient;

namespace StorageLocalTest
{
    class Program
    {
        static void TestUser(IStorage storage)
        {
            Console.WriteLine("TestUser");

            #region Guid GetId(string login)
                // "UserNotFound" if no user has this login

                // test exception "UserNotFound"
                try
                {
                    Guid noneid = storage.User.GetId("userThatDontExists");
                    Console.WriteLine("Error, User.GetId return an id where it shouldn't.");
                }
                catch (UserNotFound) { }
                catch (Exception) { Console.WriteLine("Unhandled exception in storage.User.GetId"); }
                Console.Write(".");

                //test normal behaviour

                try
                {
                    Guid id = storage.User.GetId("userThatExists");
                }
                catch (UserNotFound) { Console.WriteLine("Error, User.GetId does not found an Id."); }
                catch (Exception) { Console.WriteLine("Unhandled exception in storage.User.GetId"); }
                Console.Write(".");
            #endregion

            #region IUserInfo GetInfo(Guid userId)
                //"UserNotFound">if no user has this ID

                // test exception "UserNotFound"

                try
                {
                    IUserInfo userInfo = storage.User.GetInfo(new Guid());
                    Console.WriteLine("Error, User.GetInfo return an info where it shouldn't.");
                }
                catch (UserNotFound) { }
                catch (Exception) { Console.WriteLine("Unhandled exception in storage.User.GetInfo"); }
                Console.Write(".");

                //test normal behaviour

                try
                {
                    IUserInfo userinfo = storage.User.GetInfo(storage.User.GetId("userThatExists"));
                    if (userinfo.Login != "userThatExists" || userinfo.Email != "userThatExists@gmail.com")
                        Console.WriteLine("Error, User.GetInfo");
                }
                catch (UserNotFound) { Console.WriteLine("Error, User.GetInfo does not found an Id."); }
                catch (Exception) { Console.WriteLine("Unhandled exception in storage.User.GetInfo"); }
                Console.Write(".");
            #endregion
            
            #region void SetInfo(Guid userId, string email)
                //"UserNotFound">if no user has this ID

                // test exception "UserNotFound"

                try
                {
                    storage.User.SetInfo(Guid.NewGuid(), "babar@celeste.com");
                    Console.WriteLine("Error, User.SetInfo does not raise UserNotFound.");
                }
                catch (UserNotFound) { }
                catch (Exception) { Console.WriteLine("Unhandled exception in storage.User.SetInfo"); }
                Console.Write(".");

                //test normal behaviour

                try
                {
                    Guid userid = storage.User.GetId("userThatExists");
                    storage.User.SetInfo(userid, "userThatExists@notgmail.com");
                    IUserInfo newuserinfo = storage.User.GetInfo(userid);
                    if (newuserinfo.Email != "userThatExists@notgmail.com")
                        Console.WriteLine("Error, User.SetInfo");
                    storage.User.SetInfo(userid, "userThatExists@gmail.com");
                }
                catch (UserNotFound) { Console.WriteLine("Error, User.Set/GetInfo does not found an Id."); }
                catch (Exception) { Console.WriteLine("Unhandled exception in storage.User.GetId/SetInfo/GetInfo"); }
                Console.Write(".");
            #endregion

            #region HashSet<Guid> GetAccounts(Guid userId)
                //"UserNotFound">if no user has this ID

                // test exception "UserNotFound"
                try
                {
                    Guid userid = storage.User.GetId("userThatExists");
                    Guid accountid = storage.Account.GetId("accountThatExists");
                    HashSet<Guid> accounts = storage.User.GetAccounts(userid);
                    if (!accounts.Contains(accountid))
                        Console.WriteLine("Error, User.GetAccounts");
                }
                catch (UserNotFound) { }
                catch (Exception) { Console.WriteLine("Unhandled exception in storage.User.GetId/GetAccounts"); }
                Console.Write(".");

                //test normal behaviour
                try
                {
                    Guid userid = storage.User.GetId("userThatExists");
                    Guid accountid = storage.Account.GetId("accountThatExists");
                    HashSet<Guid> accounts = storage.User.GetAccounts(userid);
                    if (!accounts.Contains(accountid))
                        Console.WriteLine("Error, User.GetAccounts");
                }
                catch (UserNotFound) { Console.WriteLine("Error, User.GetAccounts"); }
                catch (Exception) { Console.WriteLine("Unhandled exception in storage.User.GetId/GetAccounts"); }
                Console.Write(".");
            #endregion

            #region Guid Create(string login, string email, string password)
                //"UserAlreadyExists">if the login is already used

                //test exception "UserAlreadyExists"

                try
                {
                    Guid sameuser = storage.User.Create("userThatExists", "bidon@test2.com", "pass");
                    Console.WriteLine("Error, User.Create does not detect existing user");
                }
                catch (UserAlreadyExists) { }
                catch (Exception) { Console.WriteLine("Unhandled exception in storage.User.Create"); }
                Console.Write(".");

                //test normal behaviour : Done with "userThatExists"


                //void Delete(Guid userId); -> UserIsAdmin

                // test exception "UserIsAdmin"

                try
                {
                    storage.User.Delete(storage.User.GetId("userThatExists"));
                    Console.WriteLine("Error, User.Delete does not detect an admin user");
                }
                catch (UserIsAdmin) { }
                catch (Exception) { Console.WriteLine("Unhandled exception in storage.User.GetId/Delete"); }

                //test normal behaviour

                try
                {
                    Guid userTempId = storage.User.Create("userTemp", "bidon@test2.com", "pass");
                    storage.User.Delete(userTempId);
                    storage.User.GetId("userTemp");
                    Console.WriteLine("Error, User.Delete does not delete");
                }
                catch (UserNotFound) { }
                catch (Exception) { Console.WriteLine("Unhandled exception in storage.User.Create/Delete/GetId"); }
                Console.Write(".");
            #endregion

            #region string GetPassword(Guid userID)
                //UserNotFound

                // test exception "UserNotFound"

                try
                {
                    storage.User.GetPassword(new Guid());
                    Console.WriteLine("Error, User.GetPassword does not detect existing user");
                }
                catch (UserNotFound) { }
                catch (Exception) { Console.WriteLine("Unhandled exception in storage.User.GetPassword"); }
                Console.Write(".");

                //test normal behaviour

                try
                {
                    string pass = storage.User.GetPassword(storage.User.GetId("userThatExists"));
                    if (pass != "userThatExistsPass")
                        Console.WriteLine("Error, User.GetPassword does not get the good password");
                }
                catch (UserNotFound) { Console.WriteLine("Error, User.GetPassword unable to find the id"); }
                catch (Exception) { Console.WriteLine("Unhandled exception in storage.User.GetPassword"); }
                Console.Write(".");
            #endregion

            #region void SetPassword(Guid userID, string password)
                //"UserNotFound"

                // test exception "UserNotFound"

                try
                {
                    storage.User.SetPassword(Guid.NewGuid(), "dontexistspass");
                    Console.WriteLine("Error, SetPassword doed not raise UserNotFound");
                }
                catch (UserNotFound) { }
                catch (Exception) { Console.WriteLine("Unhandled exception in storage.User.SetPassword"); }

                //test normal behaviour with valid pass

                try
                {
                    storage.User.SetPassword(storage.User.GetId("userThatExists"), "babar");
                    string pass = storage.User.GetPassword(storage.User.GetId("userThatExists"));
                    if (pass != "babar")
                        Console.WriteLine("Error, User.SetPassword does not set the good password");
                    storage.User.SetPassword(storage.User.GetId("userThatExists"), "userThatExistsPass");
                }
                catch (UserNotFound) { Console.WriteLine("Error, User.SetPassword unable to find the id"); }
                catch (Exception) { Console.WriteLine("Unhandled exception in storage.User.SetPassword/GetPassword"); }
                Console.Write(".");
            #endregion

            Console.WriteLine("\n");

        }

        static void TestAccounts(IStorage storage)
        {

            //Guid GetId(string name); -> "AccountNotFound">if no account has this name

            // test exception "AccountNotFound"

            try
            {
                Guid noneid = storage.Account.GetId("accountThatDoesntExist");
                Console.WriteLine("Error, Account.GetId return an id where it shouldn't.");
            }
            catch (AccountNotFound) { }

            //test normal behaviour

            try
            {
                Guid id = storage.Account.GetId("accountThatExists");
            }
            catch (AccountNotFound) { Console.WriteLine("Error, Account.GetId does not find id."); }
            Console.Write(".");

            //IAccountInfo GetInfo(Guid accountId); -> "AccountNotFound">if no account has this ID

            // test exception "AccountNotFound"

            try
            {
                IAccountInfo accountInfo = storage.Account.GetInfo(new Guid());
                Console.WriteLine("Error, Account.GetInfo doesnot raise AccountNotFound.");
            }
            catch (AccountNotFound) { }
            Console.Write(".");

            //test normal behaviour

            try
            {
                Guid accountid = storage.Account.GetId("accountThatExists");
                IAccountInfo accountinfo = storage.Account.GetInfo(accountid);
                if (accountinfo.Name != "accountThatExists" || accountinfo.Description != "accountThatExistsDesc")
                    Console.WriteLine("Error, Account.GetInfo");
            }
            catch (UserNotFound) { Console.WriteLine("Error, Account.GetInfo does not found a new Id."); }
            Console.Write(".");


            //void SetInfo(Guid accountId, string description); -> "AccountNotFound">if no account has ID=accountID>

            // test exception "AccountNotFound"

            try
            {
                storage.Account.SetInfo(new Guid(), "");
                Console.WriteLine("Error, Account.SetInfo does not raise AccountNotFound");
            }
            catch (AccountNotFound) { }

            //test normal behaviour

            try
            {
                Guid accountId = storage.Account.GetId("accountThatDoesntExist");
                storage.Account.SetInfo(accountId, "accountThatExistsBadDesc");
                IAccountInfo newaccountinfo = storage.Account.GetInfo(accountId);
                if (newaccountinfo.Description != "accountThatExistsBadDesc")
                    Console.WriteLine("Error, Account.SetInfo");
                storage.User.SetInfo(accountId, "accountThatExistsDesc");
            }
            catch (AccountNotFound) { Console.WriteLine("Error, Account.SetInfo does not find an id"); }

            //HashSet<Guid> GetUsers(Guid accountId); -> "AccountNotFound"

            // test exception "AccountNotFound"
            try
            {
                Guid accountId = storage.Account.GetId("accountThatExists");
                HashSet<Guid> users = storage.Account.GetUsers(accountId);
                Console.WriteLine("Error, Account.GetUsers does not raise AccountNotFound");
            }
            catch (AccountNotFound) { }
            Console.Write(".");

            //test normal behaviour
            try
            {
                Guid userId = storage.User.GetId("userThatExists");
                Guid accountId = storage.Account.GetId("accountThatExists");
                HashSet<Guid> users = storage.Account.GetUsers(accountId);
                if (!users.Contains(userId))
                    Console.WriteLine("Error, Account.GetUsers");
            }
            catch (AccountNotFound) { Console.WriteLine("Error, Account.GetUsers does not find an id"); }
            Console.Write(".");


            //Guid GetAdminId(Guid accountId); -> "AccountNotFound"

            // test exception "AccountNotFound"

            try
            {
                storage.Account.GetAdminId(new Guid());
                Console.WriteLine("Account.GetAdminId does not raise AccountNotFound");
            }
            catch (AccountNotFound) { }

            //test normal behaviour

            try
            {
                Guid userId = storage.User.GetId("userThatExists");
                Guid accountId = storage.Account.GetId("accountThatExists");
                Guid admin = storage.Account.GetAdminId(accountId);
                if (admin != userId)
                    Console.WriteLine("Error, Account.GetAdminId");
                Console.Write(".");
            }
            catch (AccountNotFound) { Console.WriteLine("Error, Account.GetAdminId"); }

            //void SetAdminId(Guid accountId, Guid userId); -> "AccountNotFound">if no account has this ID
            //                                                  "UserNotFound">if no user has this ID

            // test exception "AccountNotFound"
            try
            {
                Guid userId = storage.User.GetId("userThatExists");
                Guid accountId = storage.Account.GetId("accountThatDoesntExist");
                storage.Account.SetAdminId(accountId, userId);
                Console.WriteLine("Error, Account.SetAdminId does raise AccountNotFound");
            }
            catch (AccountNotFound) { }

            // test exception "UserNotFound"

            try
            {
                Guid userId = storage.User.GetId("userThatDoesntExist");
                Guid accountId = storage.Account.GetId("accountThatExists");
                storage.Account.SetAdminId(accountId, userId);
                Console.WriteLine("Error, Account.SetAdminId does raise AccountNotFound");
            }
            catch (AccountNotFound) { }

            //test normal behaviour

            try
            {
                Guid userId = storage.User.GetId("userThatExists");
                Guid accountId = storage.Account.GetId("accountThatExists");
                storage.Account.SetAdminId(accountId, userId);

            }
            catch { Console.WriteLine("Error, Account.SetAdminId does not find account id or user id"); }



            //void Add(Guid accountId, Guid userId); -> "AccountNotFound">if no account has this id
            //                                          "UserNotFound">if no user has this id

            // test exception "AccountNotFound"

            try
            {
                Guid accountId = storage.Account.GetId("accountThatDoesnotExists");
                Guid otherUserId = storage.User.GetId("otherUserThatExists");
                storage.Account.Add(accountId, otherUserId);
                Console.WriteLine("Error, Account.Add does not raise AccountNotFound");
            }
            catch (AccountNotFound) { }
            Console.Write(".");

            // test exception "UserNotFound"

            try
            {
                Guid accountId = storage.Account.GetId("accountThatExists");
                Guid otherUserId = storage.User.GetId("otherUserThatDoesnotExists");
                storage.Account.Add(accountId, otherUserId);
                Console.WriteLine("Error, Account.Add does not raise UsertNotFound");
            }
            catch (UserNotFound) { }
            Console.Write(".");

            //test normal behaviour
            try
            {
                Guid accountId = storage.Account.GetId("accountThatExists");
                Guid otherUserId = storage.User.GetId("otherUserThatExists");
                storage.Account.Add(accountId, otherUserId);
                HashSet<Guid> users = storage.Account.GetUsers(accountId);
                if (!users.Contains(otherUserId))
                    Console.WriteLine("Error, Account.Add");
                HashSet<Guid> accounts = storage.User.GetAccounts(otherUserId);
                if (!accounts.Contains(accountId))
                    Console.WriteLine("Error, Account.Add");

            }
            catch { Console.WriteLine("Error, Account.Add raised an exception where it should not"); }
            Console.Write(".");

            //void Remove(Guid accountId, Guid userId); -> "UserIsAdmin">if you try to remove the administrator from the user groups

            // test exception "UserIsAdmin"

            try
            {
                Guid userId = storage.User.GetId("userThatExists");
                Guid accountId = storage.Account.GetId("accountThatExists");
                storage.Account.Remove(accountId, userId);
                Console.WriteLine("Error, Account.Remove doesn't detect admin");
            }
            catch (UserIsAdmin) { }
            Console.Write(".");

            //test with wrong accountId

            try
            {
                Guid userId = storage.User.GetId("userThatExists");
                Guid accountId = storage.Account.GetId("accountThatDoesntExists");
                storage.Account.Remove(accountId, userId);
            }
            catch (AccountNotFound) { Console.WriteLine("Error, Account.Remove raised AccountNotFound where it should not"); }
            Console.Write(".");

            //test with wrong userId

            try
            {
                Guid userId = storage.User.GetId("userThatDoesnotExists");
                Guid accountId = storage.Account.GetId("accountThatExists");
                storage.Account.Remove(accountId, userId);
            }
            catch (AccountNotFound) { Console.WriteLine("Error, Account.Remove raised UserNotFound where it should not"); }
            Console.Write(".");

            //test normal behaviour

            try
            {
                Guid accountId = storage.Account.GetId("accountThatExists");
                Guid otherUserId = storage.User.GetId("otherUserThatExists");
                storage.Account.Add(accountId, otherUserId);
                storage.Account.Remove(accountId, otherUserId);
                HashSet<Guid> users = storage.Account.GetUsers(accountId);
                if (users.Contains(otherUserId))
                    Console.WriteLine("Error, Account.Add");
                HashSet<Guid> accounts = storage.User.GetAccounts(otherUserId);
                if (accounts.Contains(accountId))
                    Console.WriteLine("Error, Account.Add");
            }
            catch { Console.WriteLine("Error, Account.Remove "); }


            //Guid Create(Guid adminId, string name, string description); -> "UserNotFound">if no account has this ID
            //                                                            -> "AccountAlreadyExists">if the name is already used

            // test exception "UserNotFound"

            try
            {
                Guid userId = storage.User.GetId("userThatExists");
                storage.Account.Create(userId, "acountThatExists", "description2");
                Console.WriteLine("Error, Account.Create does not raise UserNotFound");
            }
            catch (UserNotFound) { }
            Console.Write(".");

            // test exception "AccountAlreadyExists"

            try
            {
                Guid userId = storage.User.GetId("userThatExists");
                storage.Account.Create(userId, "acountThatExists", "description2");
                Console.WriteLine("Error, Account.Create does not detect existing account");
            }
            catch (AccountAlreadyExists) { }
            Console.Write(".");


            //test normal behaviour : already done with accountThatExists

            //void Delete(Guid accountId);

            //test with wrong accountId

            try
            {
                storage.Account.Delete(new Guid());
            }
            catch { Console.WriteLine("Error, Account.Delete raise exception where it should not"); }
            Console.Write(".");

            //test normal behaviour

            try
            {
                Guid otherAccountId = storage.Account.GetId("otherAccountThatExists");
                storage.Account.Delete(otherAccountId);
                try
                {
                    storage.Account.GetId("otherAccountThatExists");
                    Console.WriteLine("Error, Account.Delete does not delete");
                }
                catch { }
                storage.Account.Create(storage.User.GetId("userThatExists"), "otherAccountThatExists", "otherAccountThatExistsDesc");
            }
            catch { }
            Console.Write(".");

        }

        static void ClearContainer(CloudBlobContainer c)
        {
            BlobRequestOptions opt = new BlobRequestOptions();
            opt.UseFlatBlobListing = true;
            foreach (CloudBlob blob in c.ListBlobs(opt))
                blob.Delete();
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Init connexions");
            Storage storage = new Storage("ulyssestorage", "S9IlG8caJ1BJpA3B3DOV7KF6VxU8an2W5N5m4Y5ZcL1rt7ljoRzcXAOw6xRc8pn8f9XNAOpyqCcXdJShj95onA==");

            Console.WriteLine("Clearing previous data");
            ClearContainer(storage.connexion.userContainer);
            ClearContainer(storage.connexion.accountContainer);
            ClearContainer(storage.connexion.listContainer);
            ClearContainer(storage.connexion.msgContainer);

            Console.WriteLine("Filling with initial data");
            //storage.InitWithStupidData();
            //storage.afficheDebug();
            Guid userId = storage.User.Create("userThatExists", "userThatExists@gmail.com", "userThatExistsPass");
            Guid accountId = storage.Account.Create(userId, "accountThatExists", "accountThatExistsDesc");
            storage.Account.Add(accountId, userId);

            Console.WriteLine("Init ok");

            TestUser(storage);
            TestAccounts(storage);

            Console.ReadLine();

        }
    }
}
