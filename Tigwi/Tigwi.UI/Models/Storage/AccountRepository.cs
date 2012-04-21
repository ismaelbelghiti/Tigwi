// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AccountSet.cs" company="">
//   
// </copyright>
// <summary>
//   The i account set.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Tigwi.UI.Models.Storage
{
    using System;
    using System.Collections.Generic;

    using StorageLibrary;

    /// <summary>
    /// The account set.
    /// </summary>
    public class AccountRepository : IAccountRepository
    {
        #region Constants and Fields

        /// <summary>
        /// The storage obj.
        /// </summary>
        private readonly IStorage storageObj;

        private readonly IStorageContext storageContext;

        protected IDictionary<Guid, StorageAccountModel> Accounts { get; set; }

        #endregion

        #region Constructors and Destructors

        public AccountRepository(IStorage storageObj, IStorageContext storageContext)
        {
            this.storageObj = storageObj;
            this.storageContext = storageContext;
            this.Accounts = new Dictionary<Guid, StorageAccountModel>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets StorageObj.
        /// </summary>
        protected IStorage StorageObj
        {
            get
            {
                return this.storageObj;
            }
        }

        protected IStorageContext StorageContext
        {
            get
            {
                return this.storageContext;
            }
        }

        #endregion

        #region Public Methods and Operators

        public StorageAccountModel Create(StorageUserModel user, string name, string description)
        {
            try
            {
                var id = this.StorageObj.Account.Create(user.Id, name, description);
                return this.Find(id);
            }
            catch (UserNotFound)
            {
                throw;
            }
            catch (AccountAlreadyExists)
            {
                throw;
            }
        }

        public void Delete(StorageAccountModel account)
        {
            this.StorageObj.Account.Delete(account.Id);
            this.Accounts.Remove(account.Id);
        }

        public StorageAccountModel Find(Guid id)
        {
            StorageAccountModel account;
            if (!this.Accounts.TryGetValue(id, out account))
            {
                account = new StorageAccountModel(this.StorageObj, this.StorageContext, id);
                this.Accounts.Add(id, account);
            }

            return account;
        }

        public StorageAccountModel Find(string name)
        {
            var id = this.StorageObj.Account.GetId(name);
            return this.Find(id);
        }

        #endregion
    }
}