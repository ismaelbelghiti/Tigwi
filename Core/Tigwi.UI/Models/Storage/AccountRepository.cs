#region copyright
// Copyright (c) 2012, TIGWI
// All rights reserved.
// Distributed under  BSD 2-Clause license
#endregion
namespace Tigwi.UI.Models.Storage
{
    using System;
    using System.Linq;

    using Tigwi.Storage.Library;

    /// <summary>
    /// An implementation of the <see cref="IAccountRepository" /> interface for Azure Storage.
    /// </summary>
    public class AccountRepository : StorageEntityRepository<StorageAccountModel>, IAccountRepository
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountRepository"/> class. 
        /// </summary>
        /// <param name="storageContext">
        /// The storage context.
        /// </param>
        public AccountRepository(StorageContext storageContext)
            : base(storageContext)
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Create a new account with the given owner.
        /// </summary>
        /// <param name="user">
        /// The new accountt's owner.
        /// </param>
        /// <param name="name">
        /// The new account's name.
        /// </param>
        /// <param name="description">
        /// The new account's description.
        /// </param>
        /// <returns>
        /// An <see cref="IAccountModel"/> representing the newly created account.
        /// </returns>
        /// <exception cref="UserNotFoundException">
        /// When the given user no longer exists in the context.
        /// </exception>
        /// <exception cref="DuplicateAccountException">
        /// When there is already an account with the given name in the context.
        /// </exception>
        public IAccountModel Create(IUserModel user, string name, string description)
        {
            // TODO: move this to IUserModel ?
            try
            {
                // Delegate to the API, then find the model.
                // TODO: prepopulate
                Guid id = this.Storage.Account.Create(user.Id, name, description);
                return this.Find(id);
            }
            catch (UserNotFound ex)
            {
                throw new UserNotFoundException(user.Id, ex);
            }
            catch (AccountAlreadyExists ex)
            {
                throw new DuplicateAccountException(name, ex);
            }
        }

        /// <summary>
        /// Delete an account model.
        /// </summary>
        /// <param name="interfaceAccount">
        /// The account model to delete.
        /// </param>
        public void Delete(IAccountModel interfaceAccount)
        {
            // TODO: fixme
            StorageAccountModel account = interfaceAccount as StorageAccountModel
                                          ?? this.InternalFind(interfaceAccount.Id);
            account.MarkDeleted();

            // Should we do this now ?
            this.Storage.Account.Delete(account.Id);
            this.EntitiesMap.Remove(account.Id);
        }

        /// <summary>
        /// Find an account by its ID.
        /// </summary>
        /// <param name="id">
        /// The account's id.
        /// </param>
        /// <returns>
        /// A lazy <see cref="IAccountModel"/> representing the account.
        /// </returns>
        public IAccountModel Find(Guid id)
        {
            // We only need a different type
            return this.InternalFind(id);
        }

        /// <summary>
        /// Find an account by its name.
        /// </summary>
        /// <param name="name">
        /// The account's name.
        /// </param>
        /// <returns>
        /// A lazy <see cref="IAccountModel"/> representing the account.
        /// </returns>
        /// <exception cref="AccountNotFoundException">
        /// When there is no account with the given name.
        /// </exception>
        public IAccountModel Find(string name)
        {
            IAccountModel account;

            if (!this.TryFind(name, out account))
            {
                throw new AccountNotFoundException(name, null);
            }

            return account;
        }

        public bool TryFind(string name, out IAccountModel account)
        {
            try
            {
                var id = this.Storage.Account.GetId(name);
                account = this.Find(id);
                return true;
            }
            catch (AccountNotFound)
            {
                account = null;
                return false;
            }
        }

        public bool Exists(string name)
        {
            try
            {
                this.Storage.Account.GetId(name);
                return true;
            }
            catch (AccountNotFound)
            {
                return false;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Find an account by its ID and returns it with its true type, <see cref="StorageAccountModel" />.
        /// </summary>
        /// <param name="id">
        /// The account's id.
        /// </param>
        /// <returns>
        /// A lazy <see cref="StorageAccountModel" /> reprsenting the account.
        /// </returns>
        internal StorageAccountModel InternalFind(Guid id)
        {
            StorageAccountModel account;

            // Try to hit the cache
            if (!this.EntitiesMap.TryGetValue(id, out account))
            {
                account = new StorageAccountModel(this.Storage, this.StorageContext, id);
                this.EntitiesMap.Add(id, account);
            }

            return account;
        }

        /// <summary>
        /// Commit the changes.
        /// </summary>
        internal bool SaveChanges()
        {
            // Fold, yay !
            return this.EntitiesMap.Aggregate(true, (current, account) => current & account.Value.Save());
        }

        #endregion
    }
}