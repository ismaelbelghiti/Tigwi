using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageLocalTest
{
  /*
    using NUnit.Framework;

    [TestFixture]
    class UserTest
    {
        const string azureAccountName = "ulyssestorage";
        const string azureAccountKey = "jnfLEhtEGAZ6YzRoYSahJpgUzXL2438grLGeFn/lnhxNJGonwD/jO+7QU2u/UECHeYsF4uigIfXKGsqRbjRsTQ==";
        private IStorage storage;
        
        [SetUp]
        public void InitStorage()
        {
            BlobFactory blobFactory = new BlobFactory(azureAccountName, azureAccountKey);
            blobFactory.InitStorage();

            storage = new Storage(azureAccountName, azureAccountKey);

            Guid userId = storage.User.Create("userThatExists", "userThatExists@gmail.com", new Byte[1]);
            Guid accountId = storage.Account.Create(userId, "accountThatExists", "accountThatExistsDesc");
            storage.User.Create("otherUserThatExists", "otherUserThatExists@gmail.com", new Byte[1]);
            Guid otherAccountId = storage.Account.Create(userId, "otherAccountThatExists", "otherAccountThatExistsDesc");
        }

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
    }
    */    
}
