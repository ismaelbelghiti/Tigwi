using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using StorageLibrary.Utilities;

namespace StorageLibrary
{
    // TODO : change this class to private
	public class BlobFactory
	{
        // Cloud Connection
        private CloudBlobContainer userContainer;
        private CloudBlobContainer accountContainer;
        private CloudBlobContainer listContainer;
        private CloudBlobContainer msgContainer;

        public BlobFactory(string azureAccountName, string azureKey)
        {
            // initialize Azure Account
            StorageCredentialsAccountAndKey storageKey = new StorageCredentialsAccountAndKey(azureAccountName, azureKey);
            CloudStorageAccount azureAccount = new CloudStorageAccount(storageKey, true);

            // Create blob containers
            CloudBlobClient blobClient = azureAccount.CreateCloudBlobClient();

            userContainer = blobClient.GetContainerReference("user");
            accountContainer = blobClient.GetContainerReference("account");
            listContainer = blobClient.GetContainerReference("list");
            msgContainer = blobClient.GetContainerReference("msg");
        }

        public void InitStorage()
        {
            userContainer.CreateIfNotExist();
            ClearContainer(userContainer);

            accountContainer.CreateIfNotExist();
            ClearContainer(accountContainer);

            listContainer.CreateIfNotExist();
            ClearContainer(listContainer);

            msgContainer.CreateIfNotExist();
            ClearContainer(msgContainer);
        }

        void ClearContainer(CloudBlobContainer c)
        {
            BlobRequestOptions opt = new BlobRequestOptions();
            opt.UseFlatBlobListing = true;
            foreach (CloudBlob blob in c.ListBlobs(opt))
                blob.Delete();
        }

        // User Blobs
        public Blob<Guid> UIdByLogin(string login)
        {
            Guid loginHash = Hasher.Hash(login);
            return new Blob<Guid>(userContainer, U_IDBYLOGIN + loginHash);
        }

        public Blob<IUserInfo> UInfo(Guid userId)
        {
            return new Blob<IUserInfo>(userContainer, U_INFO + userId);
        }

        public Blob<Byte[]> UPassword(Guid userId)
        {
            return new Blob<Byte[]>(userContainer, U_PASSWORD + userId);
        }

        public Mutex UAccountsLock(Guid userId)
        {
            return new Mutex(userContainer, U_ACCOUNTS + userId + LOCK, new UserNotFound());
        }

        public void UAccountsLockInit(Guid userId)
        {
            Mutex.Init(userContainer, U_ACCOUNTS + userId + LOCK);
        }

        public HashSetBlob<Guid> UAccountsData(Guid userId)
        {
            return new HashSetBlob<Guid>(userContainer, U_ACCOUNTS + userId + DATA);
        }

        // account Blobs
        public Blob<Guid> AIdByName(string name)
        {
            return new Blob<Guid>(accountContainer, A_IDBYNAME + Hasher.Hash(name));
        }

        public Blob<IAccountInfo> AInfo(Guid accountId)
        {
            return new Blob<IAccountInfo>(accountContainer, A_INFO + accountId);
        }

        public HashSetBlob<Guid> AUsers(Guid accountId)
        {
            return new HashSetBlob<Guid>(accountContainer, A_USERS + accountId);
        }

        public Blob<Guid> AAdminId(Guid accountId)
        {
            return new Blob<Guid>(accountContainer, A_ADMINID + accountId);
        }

        // list blobs
        public Blob<IListInfo> LInfo(Guid listId)
        {
            return new Blob<IListInfo>(listContainer, L_INFO + listId);
        }

        public Blob<Guid> LPersonnalList(Guid accountId)
        {
            return new Blob<Guid>(listContainer, L_PERSO + accountId);
        }

        public Blob<Guid> LOwner(Guid listId)
        {
            return new Blob<Guid>(listContainer, L_OWNER + listId);
        }

        public HashSetBlob<Guid> LOwnedListsPublic(Guid accountId)
        {
            return new HashSetBlob<Guid>(listContainer, L_OWNEDLISTS + accountId + PUBLIC);
        }

        public HashSetBlob<Guid> LOwnedListsPrivate(Guid accountId)
        {
            return new HashSetBlob<Guid>(listContainer, L_OWNEDLISTS + accountId + PRIVATE);
        }

        public Mutex LFollowedListsLock(Guid accountId)
        {
            return new Mutex(listContainer, L_FOLLOWEDLISTS + accountId + LOCK, new AccountNotFound());
        }

        public void LFollowedListsLockInit(Guid accountId)
        {
            Mutex.Init(listContainer, L_FOLLOWEDLISTS + accountId + LOCK);
        }

        public HashSetBlob<Guid> LFollowedListsData(Guid accountId)
        {
            return new HashSetBlob<Guid>(listContainer, L_FOLLOWEDLISTS + accountId + DATA);
        }

        public HashSetBlob<Guid> LFollowingAccounts(Guid listId)
        {
            return new HashSetBlob<Guid>(listContainer, L_FOLLOWINGACCOUNTS + listId);
        }

        public HashSetBlob<Guid> LFollowedByPublic(Guid accountId)
        {
            return new HashSetBlob<Guid>(listContainer, L_FOLLOWEDBY + accountId + PUBLIC);
        }

        public HashSetBlob<Guid> LFollowedByAll(Guid accountId)
        {
            return new HashSetBlob<Guid>(listContainer, L_FOLLOWEDBY + accountId + ALL);
        }

        public Mutex LFollowedAccountLock(Guid listId)
        {
            return new Mutex(listContainer, L_FOLLOWEDACCOUNTS + listId + LOCK, new ListNotFound());
        }

        public void LFollowedAccountLockInit(Guid listId)
        {
            Mutex.Init(listContainer, L_FOLLOWEDACCOUNTS + listId + LOCK);
        }

        public HashSetBlob<Guid> LFollowedAccountsData(Guid listId)
        {
            return new HashSetBlob<Guid>(listContainer, L_FOLLOWEDACCOUNTS + listId + DATA);
        }

        // msg blobs
        public MsgSetBlobPack MListMessages(Guid listId)
        {
            return new MsgSetBlobPack(msgContainer, M_LISTMESSAGES + listId);
        }

        public Blob<IMessage> MMessage(Guid messageId)
        {
            return new Blob<IMessage>(msgContainer, M_MESSAGE + messageId);
        }

        public MsgSetBlobPack MTaggedMessages(Guid AccountId)
        {
            return new MsgSetBlobPack(msgContainer, M_TAGGEDMESSAGES + AccountId);
        }

        // paths
        const string DATA = "/data";
        const string LOCK = "/lock";
        const string PUBLIC = "/public";
        const string PRIVATE = "/private";
        const string ALL = "/all";

        const string U_INFO = "info/";
        const string U_ACCOUNTS = "accounts/";
        const string U_IDBYLOGIN = "idbylogin/";
        const string U_PASSWORD = "password/";

        const string A_IDBYNAME = "idbyname/";
        const string A_INFO = "info/";
        const string A_USERS = "users/";
        const string A_ADMINID = "adminid/";

        const string L_INFO = "info/";
        const string L_PERSO = "personnallist/";
        const string L_OWNER = "owner/";
        const string L_OWNEDLISTS = "ownedlists/";
        const string L_FOLLOWEDLISTS = "followedlists/";
        const string L_FOLLOWINGACCOUNTS = "followingaccounts/";
        const string L_FOLLOWEDBY = "followedby/";
        const string L_FOLLOWEDACCOUNTS = "followedaccounts/";

        const string M_LISTMESSAGES = "listmessages/";
        const string M_MESSAGE = "message/";
        const string M_TAGGEDMESSAGES = "taggedmessages/";
	}
}
