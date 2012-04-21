namespace Tigwi.UI.Models.Storage
{
    #region

    using System;
    using System.Collections.Generic;

    using StorageLibrary;

    #endregion

    /// <summary>
    /// The user model adapter.
    /// </summary>
    public class StorageUserModel
    {
        #region Constants and Fields

        private AccountCollectionAdapter accounts;

        private string avatar;

        private string email;

        private string login;

        #endregion

        #region Constructors and Destructors

        public StorageUserModel(IStorage storage, Guid id)
        {
            this.Storage = storage;
            this.Id = id;
        }

        #endregion

        #region Public Properties

        public ICollection<StorageAccountModel> Accounts
        {
            get
            {
                // TODO: use an AccountRepository
                return this.accounts ?? (this.accounts = new AccountCollectionAdapter(this.Storage, this.Id));
            }
        }

        public string Avatar
        {
            get
            {
                if (!this.AvatarUpdated)
                {
                    this.Populate();
                }

                return this.avatar;
            }

            set
            {
                this.avatar = value;
                this.AvatarUpdated = true;
            }
        }

        public string Email
        {
            get
            {
                if (!this.EmailUpdated)
                {
                    this.Populate();
                }

                return this.email;
            }

            set
            {
                this.email = value;
                this.EmailUpdated = true;
            }
        }

        public Guid Id { get; private set; }

        public string Login
        {
            get
            {
                this.Populate();
                return this.login;
            }
        }

        #endregion

        #region Properties

        protected bool AvatarUpdated { get; set; }

        protected bool EmailUpdated { get; set; }

        protected bool IdFetched { get; set; }

        protected bool InfosFetched { get; set; }

        protected bool InfosUpdated
        {
            get
            {
                return this.AvatarUpdated || this.EmailUpdated;
            }

            set
            {
                this.AvatarUpdated = value;
                this.EmailUpdated = value;
            }
        }

        protected IStorage Storage { get; private set; }

        #endregion

        #region Methods

        internal void Save()
        {
            if (this.InfosUpdated)
            {
                this.Storage.User.SetInfo(this.Id, this.Email);
                this.InfosUpdated = false;
            }

            var accountCollectionAdapter = this.accounts;
            if (accountCollectionAdapter != null)
            {
                accountCollectionAdapter.Save();
            }
        }

        public void Populate()
        {
            if (this.InfosFetched)
            {
                return;
            }

            this.Repopulate();
        }

        public void Repopulate()
        {
            var userInfo = this.Storage.User.GetInfo(this.Id);
            this.login = userInfo.Login;
            if (!this.EmailUpdated)
            {
                this.email = userInfo.Email;
            }

            if (!this.AvatarUpdated)
            {
                this.avatar = userInfo.Avatar;
            }

            this.InfosFetched = true;
        }

        #endregion

        private class AccountCollectionAdapter : CollectionAdapter<StorageAccountModel>
        {
            #region Constants and Fields

            private readonly IStorage storage;

            #endregion

            #region Constructors and Destructors

            public AccountCollectionAdapter(IStorage storage, Guid userId)
            {
                this.storage = storage;
                this.UserId = userId;
            }

            #endregion

            #region Properties

            protected IStorage Storage
            {
                get
                {
                    return this.storage;
                }
            }

            protected Guid UserId { get; private set; }

            #endregion

            #region Public Methods and Operators

            public override void Save()
            {
                // TODO: catch exceptions.
                foreach (var account in this.CollectionAdded)
                {
                    // Account must be saved in the database prior to adding
                    // TODO: really ?
                    account.Save();
                    this.Storage.Account.Add(account.Id, this.UserId);
                }

                foreach (var account in this.CollectionRemoved)
                {
                    account.Save();
                    this.Storage.Account.Remove(account.Id, this.UserId);
                }

                this.CollectionAdded.Clear();
                this.CollectionRemoved.Clear();
            }

            #endregion

            #region Methods

            protected override ICollection<Guid> FetchIdCollection()
            {
                return this.Storage.User.GetAccounts(this.UserId);
            }

            protected override StorageAccountModel GetModel(Guid id)
            {
                // TODO: Use an AccountRepository
                return new StorageAccountModel(this.Storage, id);
            }

            #endregion
        }
    }
}