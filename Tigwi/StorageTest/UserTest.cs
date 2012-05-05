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
        public IStorage storage;

        [SetUp]
        public void InitStorage()
        {
            storage = new StorageTmp();
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

        }

        [Test]
        public void CreateNormalBehaviour()
        {

        }

        #endregion

        #region void Delete(Guid userId)

        [Test]
        [ExpectedException(typeof(UserIsAdmin))]
        public void DeleteUserIsAdmin()
        {

        }

        [Test]
        public void DeleteNormalBehaviour()
        {

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

        }

        [Test]
        public void GetPasswordNormalBehaviour()
        {

        }

        #endregion

        #region void SetPassword(Guid userId, Byte[] password)

        [Test]
        [ExpectedException(typeof(UserNotFound))]
        public void SetPasswordUserNotFound()
        {

        }

        [Test]
        public void SetPasswordNormalBehaviour()
        {

        }

        #endregion
    }
}
