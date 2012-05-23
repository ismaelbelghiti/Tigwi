using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tigwi.Storage.Library;
using Tigwi.Storage.Library.Utilities;
using Tigwi.Storage.Library.DoubleMetaphone;

namespace Tigwi.Storage.Library
{
    public class AccountStorage : IAccountStorage
    {
        BlobFactory blobFactory;

        // Constuctor
        public AccountStorage(BlobFactory blobFactory)
        {
            this.blobFactory = blobFactory;
        }

        // Interface implementation
        public Guid GetId(string name)
        {
            return blobFactory.AIdByName(name).GetIfExists(new AccountNotFound());
        }

        public IAccountInfo GetInfo(Guid accountId)
        {
            return blobFactory.AInfo(accountId).GetIfExists(new AccountNotFound());
        }

        public void SetInfo(Guid accountId, string description)
        {
            Blob<AccountInfo> bInfo = blobFactory.AInfo(accountId);
            AccountInfo info = bInfo.GetIfExists(new AccountNotFound());
            info.Description = description;
            if (!bInfo.SetIfExists(info))
                throw new AccountNotFound();
        }

        public HashSet<Guid> GetUsers(Guid accountId)
        {
            return blobFactory.AUsers(accountId).GetIfExists(new AccountNotFound());
        }

        public Guid GetAdminId(Guid accountId)
        {
            return blobFactory.AAdminId(accountId).GetIfExists(new AccountNotFound());
        }

        public void SetAdminId(Guid accountId, Guid userId)
        {
            //Blob<Guid> bAdmin = new Blob<Guid>(connexion.accountContainer, Path.A_ADMINID + accountId);
            Blob<HashSet<Guid>> bUserAccounts = blobFactory.UAccountsData(userId);

            using (blobFactory.UAccountsLock(userId))
            {
                HashSet<Guid> userAccounts = bUserAccounts.Get();
                // we check if the user is already on this account
                if (!userAccounts.Contains(accountId))
                {
                    if (!blobFactory.AUsers(accountId).AddWithRetry(userId))
                        throw new AccountNotFound();

                    userAccounts.Add(accountId);
                    bUserAccounts.Set(userAccounts);
                }

                if (!blobFactory.AAdminId(accountId).SetIfExists(userId))
                {
                    userAccounts.Remove(accountId);
                    bUserAccounts.Set(userAccounts);
                    throw new AccountNotFound();
                }
            }
        }

        public void Add(Guid accountId, Guid userId)
        {
            using (blobFactory.UAccountsLock(userId))
            {
                if (!blobFactory.AUsers(accountId).AddWithRetry(userId))
                    throw new AccountNotFound();

                blobFactory.UAccountsData(userId).Add(accountId);
            }
        }

        public void Remove(Guid accountId, Guid userId)
        {
            try
            {
                using (blobFactory.UAccountsLock(userId))
                {
                    if (blobFactory.AAdminId(accountId).GetIfExists(new AccountNotFound()).Equals(userId))
                        throw new UserIsAdmin();

                    blobFactory.AUsers(accountId).RemoveWithRetry(userId);
                    blobFactory.UAccountsData(userId).Remove(accountId);
                }

            }
            catch (UserNotFound) { }
            catch (AccountNotFound) { }
            
        }

        public bool ReserveAccountName(string accountName)
        {
            Blob<Guid> bIdByName = blobFactory.AIdByName(accountName);
            if (!bIdByName.SetIfNotExists(Guid.Empty))
                return false;
            return true;
        }

        public Guid Create(Guid adminId, string name, string description, bool bypassNameReservation = false)
        {
            Blob<Guid> bIdByName = blobFactory.AIdByName(name);
            // lock the name
            if (!bypassNameReservation)
            {
                if (!bIdByName.SetIfNotExists(Guid.Empty))
                    throw new AccountAlreadyExists();
            }

            Guid accountId = Guid.NewGuid();         

            // TODO : we could do it without a lock - or at least store the data before
            using (blobFactory.UAccountsLock(adminId))
            {
                Guid personnalListId = Guid.NewGuid();

                // store the data
                HashSet<Guid> users = new HashSet<Guid>();
                users.Add(adminId);
                blobFactory.AUsers(accountId).Set(users);

                HashSet<Guid> followedByAll = new HashSet<Guid>();
                followedByAll.Add(personnalListId);
                blobFactory.LFollowedByAll(accountId).Set(followedByAll);

                blobFactory.AInfo(accountId).Set(new AccountInfo(name, description));
                blobFactory.AAdminId(accountId).Set(adminId);
                blobFactory.LOwnedListsPublic(accountId).Set(new HashSet<Guid>());
                blobFactory.LOwnedListsPrivate(accountId).Set(new HashSet<Guid>());
                blobFactory.LFollowedListsData(accountId).Set(new HashSet<Guid>());
                blobFactory.LFollowedByPublic(accountId).Set(new HashSet<Guid>());
                blobFactory.LFollowedListsLockInit(accountId);
                blobFactory.MTaggedMessages(accountId).Init();
                blobFactory.AAutocompletion().Add(new KeyValuePair<string, string>(name.GenerateDoubleMetaphone(), name));

                // Setup the personnal list
                blobFactory.LPersonnalList(accountId).Set(personnalListId);
                blobFactory.LInfo(personnalListId).Set(new ListInfo("", "", true, true));
                blobFactory.LOwner(personnalListId).Set(accountId);
                blobFactory.MListMessages(personnalListId).Init();
                blobFactory.LFollowedAccounts(personnalListId).Set(new Dictionary<Guid,bool>());

                // we finish by unlocking the name
                bIdByName.Set(accountId);

                // we make this account accessible
                blobFactory.UAccountsData(adminId).Add(accountId);
            }

            return accountId;
        }

        public void Delete(Guid accountId)
        {
            // TODO : implement
            throw new NotImplementedException();
        }

        public HashSet<string> Autocompletion(string name, int maxNameNumber)
        {
            return blobFactory.AAutocompletion().GetWithPrefix(name.GenerateDoubleMetaphone(), maxNameNumber);
        }
    }
}
