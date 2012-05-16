namespace Tigwi.UI.Models.Storage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Tigwi.Storage.Library;

    /// <summary>
    /// The user model adapter.
    /// </summary>
    public class StorageUserModel : StorageEntityModel, IUserModel
    {
        #region Constants and Fields

        private readonly StorageEntityCollection<StorageAccountModel, IAccountModel> accounts;

        private string avatar;

        private string email;

        private string login;

        #endregion

        #region Constructors and Destructors

        public StorageUserModel(IStorage storage, StorageContext storageContext, Guid id)
            : base(storage, storageContext, id)
        {
            this.accounts = new StorageEntityCollection<StorageAccountModel, IAccountModel>(storageContext)
                {
                    FetchIdCollection = () => storage.User.GetAccounts(id),
                    GetId = account => account.Id,
                    GetModel = storageContext.InternalAccounts.InternalFind,
                    ReverseAdd = account => account.InternalUsers.CacheAdd(this),
                    ReverseRemove = account => account.InternalUsers.CacheRemove(this),
                    SaveAdd = account => storage.Account.Add(account.Id, id),
                    SaveRemove = account => storage.Account.Remove(account.Id, id),
                };
        }

        #endregion

        #region Public Properties

        public ICollection<IAccountModel> Accounts
        {
            get
            {
                return this.accounts;
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

        public string Login
        {
            get
            {
                this.Populate();
                return this.login;
            }
        }

        public string Password
        {
            set
            {
                this.Storage.User.SetPassword(this.Id, Tigwi.Auth.PasswordAuth.HashPassword(value));
            }
        }

        internal StorageEntityCollection<StorageAccountModel, IAccountModel> InternalAccounts
        {
            get
            {
                return this.accounts;
            }
        }

        #endregion

        #region Properties

        protected bool AvatarUpdated { get; set; }

        protected bool EmailUpdated { get; set; }

        protected bool IdFetched { get; set; }

        protected override bool InfosUpdated
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

        #endregion

        #region Methods

        internal override void Save()
        {
            if (this.Deleted)
            {
                this.Storage.User.Delete(this.Id);
            }
            else
            {
                if (this.InfosUpdated)
                {
                    this.Storage.User.SetInfo(this.Id, this.Email);
                    this.InfosUpdated = false;
                }

                if (this.accounts != null)
                {
                    this.accounts.Save();
                }
            }
        }

        internal override void Repopulate()
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

            this.Populated = true;
        }

        #endregion
    }
}
