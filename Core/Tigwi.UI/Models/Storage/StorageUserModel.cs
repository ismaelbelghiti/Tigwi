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

        private Guid mainAccountId;

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

        public Guid MainAccountId
        {
            get
            {
                if (!this.MainAccountIdUpdated)
                {
                    this.Populate();
                }

                return this.mainAccountId;
            }

            set
            {
                this.mainAccountId = value;
                this.MainAccountIdUpdated = true;
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

        protected bool MainAccountIdUpdated { get; set; }

        protected bool IdFetched { get; set; }

        protected override bool InfosUpdated
        {
            get
            {
                return this.AvatarUpdated || this.EmailUpdated || this.MainAccountIdUpdated;
            }

            set
            {
                this.AvatarUpdated = value;
                this.EmailUpdated = value;
                this.MainAccountIdUpdated = value;
            }
        }

        #endregion

        #region Methods

        internal override bool Save()
        {
            bool success = true;

            if (this.Deleted)
            {
                try
                {
                    this.Storage.User.Delete(this.Id);
                }
                catch (StorageLibException)
                {
                    success = false;
                }
            }
            else
            {
                if (this.InfosUpdated)
                {
                    try
                    {
                        this.Storage.User.SetInfo(this.Id, this.Email, this.mainAccountId);
                        this.InfosUpdated = false;
                    }
                    catch (StorageLibException)
                    {
                        success = false;
                    }
                }

                if (this.accounts != null)
                {
                    success &= this.accounts.Save();
                }
            }

            return success;
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

            if (!this.MainAccountIdUpdated)
            {
                this.mainAccountId = userInfo.MainAccountId;
            }

            this.Populated = true;
        }

        #endregion
    }
}
