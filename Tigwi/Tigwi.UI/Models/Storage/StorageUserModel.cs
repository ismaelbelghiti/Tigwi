namespace Tigwi.UI.Models.Storage
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using StorageLibrary;

    #endregion

    /// <summary>
    /// The user model adapter.
    /// </summary>
    public class StorageUserModel : StorageEntityModel, IUserModel
    {
        #region Constants and Fields

        private readonly AccountCollectionAdapter accounts;

        private string avatar;

        private string email;

        private string login;

        #endregion

        #region Constructors and Destructors

        public StorageUserModel(IStorage storage, StorageContext storageContext, Guid id)
            : base(storage, storageContext, id)
        {
            this.accounts = new AccountCollectionAdapter(storage, storageContext, this);
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

        internal AccountCollectionAdapter InternalAccounts
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

        internal class AccountCollectionAdapter : StorageEntityCollection<StorageUserModel, StorageAccountModel, IAccountModel>
        {
            #region Constructors and Destructors

            public AccountCollectionAdapter(IStorage storage, StorageContext storageContext, StorageUserModel user)
                : base(storage, storageContext, user, () => storage.User.GetAccounts(user.Id), account => account.Id)
            {
                this.GetModel = storageContext.InternalAccounts.InternalFind;
            }

            #endregion

            #region Public Methods and Operators

            internal override void Save()
            {
                if (!this.Parent.Deleted)
                {
                    // TODO: catch exceptions (?)
                    foreach (var account in this.CollectionAdded.Where(item => item.Value && !item.Key.Deleted).Select(item => item.Key))
                    {
                        this.Storage.Account.Add(account.Id, this.Parent.Id);
                    }

                    foreach (var account in this.CollectionRemoved.Where(item => item.Value && !item.Key.Deleted).Select(item => item.Key))
                    {
                        this.Storage.Account.Remove(account.Id, this.Parent.Id);
                    }
                }

                this.CollectionAdded.Clear();
                this.CollectionRemoved.Clear();
            }

            protected override void ReverseAdd(StorageAccountModel item)
            {
                item.InternalUsers.CacheAdd(this.Parent);
            }

            protected override void ReverseRemove(StorageAccountModel item)
            {
                item.InternalUsers.CacheRemove(this.Parent);
            }

            #endregion
        }
    }
}
