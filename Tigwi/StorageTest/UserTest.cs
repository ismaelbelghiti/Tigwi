using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using StorageLibrary;

namespace StorageTest
{
    [TestFixture]
    public class UserTest
    {
        const string azureAccountName = "ulyssestorage";
        const string azureAccountKey = "jnfLEhtEGAZ6YzRoYSahJpgUzXL2438grLGeFn/lnhxNJGonwD/jO+7QU2u/UECHeYsF4uigIfXKGsqRbjRsTQ==";

        public IStorage storage;

        [SetUp]
        public void InitStorage()
        {
            bool UseStorageTmp = true;
            if (UseStorageTmp)
                storage = new StorageTmp();
            else
            {
                BlobFactory blobFactory = new BlobFactory(azureAccountName, azureAccountKey);
                blobFactory.InitStorage();
                storage = new Storage(azureAccountName, azureAccountKey);
            }

            Guid userId = storage.User.Create("userThatExists", "userThatExists@gmail.com", new Byte[1]);
            Guid accountId = storage.Account.Create(userId, "accountThatExists", "accountThatExistsDesc");
            storage.User.Create("otherUserThatExists", "otherUserThatExists@gmail.com", new Byte[1]);
            Guid otherAccountId = storage.Account.Create(userId, "otherAccountThatExists", "otherAccountThatExistsDesc");
        }

        #region Guid GetId(string login)
 
        [Test]
        [ExpectedException(typeof(UserNotFound))]
        public void GetIdUserNotFound()
        {
            storage.User.GetId("userThatDontExists");
        }

        [Test]
        public void GetIdNormalBehaviour()
        {
            storage.User.GetId("userThatExists");
        }

        #endregion

        #region IUserInfo GetInfo(Guid userId)
                    
        [Test]
        [ExpectedException(typeof(UserNotFound))]
        public void GetInfoUserNotFound()
        {
            IUserInfo userInfo = storage.User.GetInfo(new Guid());
        }

        [Test]
        public void GetInfoNormalBehaviour()
        {
            IUserInfo userinfo = storage.User.GetInfo(storage.User.GetId("userThatExists"));
            Assert.AreEqual(userinfo.Login, "userThatExists");
            Assert.AreEqual(userinfo.Email, "userThatExists@gmail.com");
        }    

        #endregion

        #region void SetInfo(Guid userId, string email)

        [Test]
        [ExpectedException(typeof(UserNotFound))]
        public void SetInfoUserNotFound()
        {
            storage.User.SetInfo(Guid.NewGuid(), "babar@celeste.com");
        }

        [Test]
        public void SetInfoNormalBehaviour()
        {
            Guid userid = storage.User.GetId("userThatExists");
            storage.User.SetInfo(userid, "userThatExists@notgmail.com");
            IUserInfo newuserinfo = storage.User.GetInfo(userid);
            Assert.AreEqual(newuserinfo.Email, "userThatExists@notgmail.com");
            storage.User.SetInfo(userid, "userThatExists@gmail.com");
        }



        #endregion

        #region HashSet<Guid> GetAccounts(Guid userId)
        [Test]
        [ExpectedException(typeof(UserNotFound))]
        public void GetAccountsUserNotFound()
        {
            storage.User.GetAccounts(new Guid());
        }
                
        [Test]
        public void GetAccountsUserNormalBehaviour()
        {
            Guid userid = storage.User.GetId("userThatExists");
            Guid accountid = storage.Account.GetId("accountThatExists");
            HashSet<Guid> accounts = storage.User.GetAccounts(userid);
            Assert.IsTrue(accounts.Contains(accountid));
        }       
        #endregion

        #region Guid Create(string login, string email, Byte[] password)

        [Test]
        [ExpectedException(typeof(UserAlreadyExists))]
        public void CreateUserAlreadyExists()
        {
            storage.User.Create("userThatExists", "bidon@test2.com", new Byte[1]);
        }

        //test normal behaviour : Done with "userThatExists"

        #endregion

        #region void Delete(Guid userId)

        [Test]
        [ExpectedException(typeof(UserIsAdmin))]
        public void DeleteUserIsAdmin()
        {
            storage.User.Delete(storage.User.GetId("userThatExists"));
        }

        [Test]
        [ExpectedException(typeof(UserNotFound))]
        public void DeleteNormalBehaviour()
        {
            Guid userTempId = storage.User.Create("userTemp", "bidon@test2.com", new Byte[1]);
            storage.User.Delete(userTempId);
            storage.User.GetId("userTemp");
        }

        #endregion

        #region Guid GetIdByOpenIdUri(string openIdUri)

        [Test]
        [ExpectedException(typeof(UserNotFound))]
        public void GetIdByOpenIdUriUserNotFound()
        {

        }

        [Test]
        public void GetIdByOpenIdUriNormalBehaviour()
        {

        }

        #endregion

        #region void AssociateOpenIdUri(Guid userId, string openIdUri)

        [Test]
        [ExpectedException(typeof(UserNotFound))]
        public void AssociateOpenIdUriUserNotFound()
        {

        }

        [Test]
        [ExpectedException(typeof(OpenIdUriDuplicated))]
        public void AssociateOpenIdUriOpenIdUriDuplicated()
        {

        }

        [Test]
        public void AssociateOpenIdUriNormalBehaviour()
        {

        }

        #endregion

        #region ListOpenIdUris(Guid userId)

        [Test]
        [ExpectedException(typeof(UserNotFound))]
        public void ListOpenIdUrisUserNotFound()
        {

        }

        [Test]
        public void ListOpenIdUrisNormalBehaviour()
        {

        }

        #endregion

        #region void DeassociateOpenIdUri(Guid userId, string openIdUri)

        [Test]
        [ExpectedException(typeof(UserNotFound))]
        public void DeassociateOpenIdUriUserNotFound()
        {

        }

        [Test]
        [ExpectedException(typeof(OpenIdUriNotAssociated))]
        public void DeassociateOpenIdUriOpenIdUriNotAssociated()
        {

        }

        [Test]
        public void DeassociateOpenIdUriNormalBehaviour()
        {

        }

        #endregion

        #region Byte[] GetPassword(Guid userId)

        [Test]
        [ExpectedException(typeof(UserNotFound))]
        public void GetPasswordUserNotFound()
        {
            storage.User.GetPassword(new Guid());
        }

        [Test]
        public void GetPasswordNormalBehaviour()
        {
            Byte[] pass = storage.User.GetPassword(storage.User.GetId("userThatExists"));
            Assert.AreEqual(pass, new Byte[1]);
        }

        #endregion

        #region void SetPassword(Guid userId, Byte[] password)

        [Test]
        [ExpectedException(typeof(UserNotFound))]
        public void SetPasswordUserNotFound()
        {
            storage.User.SetPassword(Guid.NewGuid(), new Byte[1]);
        }

        [Test]
        public void SetPasswordNormalBehaviour()
        {
            storage.User.SetPassword(storage.User.GetId("userThatExists"), new Byte[1]);
            Byte[] pass = storage.User.GetPassword(storage.User.GetId("userThatExists"));
            Assert.AreEqual(pass, new Byte[1]);
        }

        #endregion
    }
}
