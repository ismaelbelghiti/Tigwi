#region copyright
// Copyright (c) 2012, TIGWI
// All rights reserved.
// Distributed under  BSD 2-Clause license
#endregion
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

        private IDictionary<Guid, string> apiKeys;

        private string avatar;

        private string email;

        private StorageAccountModel mainAccount;

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

        public IDictionary<Guid, string> ApiKeys
        {
            get
            {
                if (!this.ApiKeysPopulated)
                {
                    this.PopulateApiKeys();
                }
                return this.apiKeys;
            }
        }

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

        public IAccountModel MainAccount
        {
            get
            {
                if (!this.MainAccountUpdated)
                {
                    this.Populate();
                }

                return this.mainAccount;
            }

            set
            {
                // TODO: consistency check
                this.mainAccount = value is StorageAccountModel
                                       ? value as StorageAccountModel
                                       : this.StorageContext.InternalAccounts.InternalFind(value.Id);
                this.MainAccountUpdated = true;
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
                this.Storage.User.SetPassword(this.Id, Auth.PasswordAuth.HashPassword(value));
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

        protected bool MainAccountUpdated { get; set; }

        protected bool IdFetched { get; set; }

        protected bool ApiKeysPopulated { get; set; }

        protected override bool InfosUpdated
        {
            get
            {
                return this.AvatarUpdated || this.EmailUpdated || this.MainAccountUpdated;
            }

            set
            {
                this.AvatarUpdated = value;
                this.EmailUpdated = value;
                this.MainAccountUpdated = value;
            }
        }

        #endregion

        #region Methods

        public Guid GenerateApiKey(string applicationName)
        {
            if (!this.ApiKeysPopulated)
            {
              this.PopulateApiKeys();
            }
            Guid newKey = this.Storage.User.GenerateApiKey(this.Id, applicationName);
            this.apiKeys.Add(newKey, applicationName);
            return newKey;
        }

        public void DeactivateApiKey(Guid apiKey)
        {
            // TODO : Better error checking
            if (!this.ApiKeysPopulated)
            {
              this.PopulateApiKeys();
            }

            if (!this.apiKeys.ContainsKey(apiKey)) return;

            this.Storage.User.DeactivateApiKey(this.Id, apiKey);
            this.apiKeys.Remove(apiKey);
        }

        internal void PopulateApiKeys()
        {
            this.apiKeys = this.Storage.User.ListApiKeys(this.Id);
            this.ApiKeysPopulated = true;
        }

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
                        this.Storage.User.SetInfo(this.Id, this.Email, this.mainAccount.Id);
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

            if (!this.MainAccountUpdated)
            {
                this.mainAccount = this.StorageContext.InternalAccounts.InternalFind(userInfo.MainAccountId);
            }

            this.Populated = true;
        }

        #endregion
    }
}
