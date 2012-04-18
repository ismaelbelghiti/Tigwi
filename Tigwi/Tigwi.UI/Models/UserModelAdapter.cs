// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserModelAdapter.cs" company="">
//   
// </copyright>
// <summary>
//   The user model adapter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Tigwi.UI.Models
{
    using System;
    using System.Collections.Generic;

    using StorageLibrary;

    /// <summary>
    /// The user model adapter.
    /// </summary>
    public class UserModelAdapter : UserModel
    {
        #region Constants and Fields

        /// <summary>
        /// The accounts.
        /// </summary>
        private readonly AccountCollectionAdapter accounts;

        /// <summary>
        /// The id fetcher.
        /// </summary>
        private readonly Func<Guid> idFetcher;

        /// <summary>
        /// The storage.
        /// </summary>
        private readonly IStorage storage;

        /// <summary>
        /// The avatar.
        /// </summary>
        private string avatar;

        /// <summary>
        /// The email.
        /// </summary>
        private string email;

        /// <summary>
        /// The id.
        /// </summary>
        private Guid id;

        /// <summary>
        /// The login.
        /// </summary>
        private string login;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UserModelAdapter"/> class. 
        /// </summary>
        /// <param name="storage">
        /// The storage.
        /// </param>
        /// <param name="idFetcher">
        /// The id fetcher.
        /// </param>
        public UserModelAdapter(IStorage storage, Func<Guid> idFetcher)
        {
            this.storage = storage;
            this.idFetcher = idFetcher;
            this.accounts = new AccountCollectionAdapter(this.Storage, () => this.Id);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets Accounts.
        /// </summary>
        public override ICollection<AccountModel> Accounts
        {
            get
            {
                return this.accounts;
            }
        }

        /// <summary>
        /// Gets or sets Avatar.
        /// </summary>
        public override string Avatar
        {
            get
            {
                this.FetchInfos();
                return this.avatar;
            }

            set
            {
                this.avatar = value;
                this.InfosUpdated = true;
            }
        }

        /// <summary>
        /// Gets or sets Email.
        /// </summary>
        public override string Email
        {
            get
            {
                this.FetchInfos();
                return this.email;
            }

            set
            {
                this.email = value;
                this.InfosUpdated = true;
            }
        }

        /// <summary>
        /// Gets Id.
        /// </summary>
        public override Guid Id
        {
            get
            {
                if (!this.IdFetched)
                {
                    this.id = this.IdFetcher();
                    this.IdFetched = true;
                }

                return this.id;
            }
        }

        /// <summary>
        /// Gets Login.
        /// </summary>
        public override string Login
        {
            get
            {
                // Login might be cached
                if (this.login == null)
                {
                    this.FetchInfos();
                }

                return this.login;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether IdFetched.
        /// </summary>
        protected bool IdFetched { get; set; }

        /// <summary>
        /// Gets IdFetcher.
        /// </summary>
        protected Func<Guid> IdFetcher
        {
            get
            {
                return this.idFetcher;
            }
        }

        /// <summary>
        /// Gets Storage.
        /// </summary>
        protected IStorage Storage
        {
            get
            {
                return this.storage;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether InfosUpdated.
        /// </summary>
        private bool InfosUpdated { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// The save.
        /// </summary>
        internal override void Save()
        {
            if (this.InfosUpdated)
            {
                this.Storage.User.SetInfo(this.Id, this.Email);
            }
        }

        /// <summary>
        /// The fetch infos.
        /// </summary>
        private void FetchInfos()
        {
            IUserInfo userInfo = this.Storage.User.GetInfo(this.Id);
            this.login = this.login ?? userInfo.Login;
            this.email = this.email ?? userInfo.Email;
            this.avatar = this.avatar ?? userInfo.Avatar;
        }

        #endregion

        /// <summary>
        /// The account collection adapter.
        /// </summary>
        private class AccountCollectionAdapter : CollectionAdapter<AccountModel>
        {
            #region Constants and Fields

            /// <summary>
            /// The storage.
            /// </summary>
            private readonly IStorage storage;

            /// <summary>
            /// The account id fetcher.
            /// </summary>
            private readonly Func<Guid> userIdFetcher;

            /// <summary>
            /// The account id.
            /// </summary>
            private Guid userId;

            /// <summary>
            /// The account id fetched.
            /// </summary>
            private bool userIdFetched;

            #endregion

            #region Constructors and Destructors

            /// <summary>
            /// Initializes a new instance of the <see cref="AccountCollectionAdapter"/> class. 
            /// </summary>
            /// <param name="storage">
            /// The storage.
            /// </param>
            /// <param name="userIdFetcher">
            /// The user Id Fetcher.
            /// </param>
            public AccountCollectionAdapter(IStorage storage, Func<Guid> userIdFetcher)
            {
                this.storage = storage;
                this.userIdFetcher = userIdFetcher;
            }

            #endregion

            #region Properties

            /// <summary>
            /// Gets Storage.
            /// </summary>
            protected IStorage Storage
            {
                get
                {
                    return this.storage;
                }
            }

            /// <summary>
            /// Gets AccountId.
            /// </summary>
            protected Guid UserId
            {
                get
                {
                    if (!this.userIdFetched)
                    {
                        this.userId = this.userIdFetcher();
                        this.userIdFetched = true;
                    }

                    return this.userId;
                }
            }

            #endregion

            #region Public Methods and Operators

            /// <summary>
            /// The save.
            /// </summary>
            public override void Save()
            {
                foreach (AccountModel account in this.CollectionAdded)
                {
                    account.Save();
                    this.Storage.Account.Add(account.Id, this.UserId);
                }

                foreach (AccountModel account in this.CollectionRemoved)
                {
                    account.Save();
                    this.Storage.Account.Remove(account.Id, this.UserId);
                }

                this.CollectionAdded.Clear();
                this.CollectionRemoved.Clear();
            }

            #endregion

            #region Methods

            /// <summary>
            /// The fetch id collection.
            /// </summary>
            /// <returns>
            /// </returns>
            protected override ICollection<Guid> FetchIdCollection()
            {
                return this.Storage.User.GetAccounts(this.userId);
            }

            /// <summary>
            /// The get model.
            /// </summary>
            /// <param name="id">
            /// The id.
            /// </param>
            /// <returns>
            /// </returns>
            protected override AccountModel GetModel(Guid id)
            {
                return new StorageAccountModel(this.Storage, () => id);
            }

            #endregion
        }
    }
}