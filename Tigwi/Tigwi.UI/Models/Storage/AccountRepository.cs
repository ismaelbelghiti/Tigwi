namespace Tigwi.UI.Models.Storage
{
    #region

    using System;

    using StorageLibrary;

    #endregion

    public class AccountRepository : StorageEntityRepository<StorageAccountModel>, IAccountRepository
    {
        #region Constructors and Destructors

        public AccountRepository(IStorage storageObj, IStorageContext storageContext)
            : base(storageObj, storageContext)
        {
        }

        #endregion

        #region Public Methods and Operators

        public IAccountModel Create(StorageUserModel user, string name, string description)
        {
            try
            {
                var id = this.Storage.Account.Create(user.Id, name, description);
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

        public void Delete(IAccountModel interfaceAccount)
        {
            // TODO: fixme
            var account = this.InternalFind(interfaceAccount.Id);
            account.MarkDeleted();
            this.Storage.Account.Delete(account.Id);
            this.EntitiesMap.Remove(account.Id);
        }

        public IAccountModel Find(Guid id)
        {
            return this.InternalFind(id);
        }

        public IAccountModel Find(string name)
        {
            try
            {
                var id = this.Storage.Account.GetId(name);
                return this.Find(id);
            }
            catch (AccountNotFound ex)
            {
                throw new AccountNotFoundException(name, ex);
            }
        }

        #endregion

        #region Methods

        internal void SaveChanges()
        {
            foreach (var account in this.EntitiesMap)
            {
                account.Value.Save();
            }
        }

        protected StorageAccountModel InternalFind(Guid id)
        {
            StorageAccountModel account;
            if (!this.EntitiesMap.TryGetValue(id, out account))
            {
                account = new StorageAccountModel(this.Storage, this.StorageContext, id);
                this.EntitiesMap.Add(id, account);
            }

            return account;
        }

        #endregion
    }
}