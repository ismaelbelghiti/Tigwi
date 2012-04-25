namespace Tigwi.UI.Models.Storage
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using StorageLibrary;

    #endregion

    public class StorageAccountModel : StorageEntityModel
    {
        #region Constants and Fields

        private readonly ListCollectionAdapter allFollowedLists;

        private readonly ListCollectionAdapter allOwnedLists;

        private readonly ListCollectionAdapter memberLists;

        private readonly ListCollectionAdapter publicFollowedLists;

        private readonly ListCollectionAdapter publicOwnedLists;

        private readonly UserCollectionAdapter users;

        private StorageUserModel admin;

        private string description;

        private string name;

        #endregion

        #region Constructors and Destructors

        public StorageAccountModel(IStorage storage, IStorageContext storageContext, Guid accountId)
            : base(storage, storageContext, accountId)
        {
            this.users = new UserCollectionAdapter(this.Storage, this.StorageContext, this);

            // Bunch of list adapters
            this.allFollowedLists =
                this.MakeListCollection(() => this.Storage.List.GetAccountFollowedLists(this.Id, true));
            this.allOwnedLists = this.MakeListCollection(() => this.Storage.List.GetAccountOwnedLists(this.Id, true));
            this.memberLists = this.MakeListCollection(() => this.Storage.List.GetFollowingLists(this.Id));
            this.publicFollowedLists =
                this.MakeListCollection(() => this.Storage.List.GetAccountFollowedLists(this.Id, false));
            this.publicOwnedLists = this.MakeListCollection(
                () => this.Storage.List.GetAccountOwnedLists(this.Id, false));
        }

        #endregion

        #region Public Properties

        public StorageUserModel Admin
        {
            get
            {
                // Admin's initialization involves storage calls, so let's be lazy
                return this.admin
                       ?? (this.admin = this.StorageContext.Users.Find(this.Storage.Account.GetAdminId(this.Id)));
            }

            set
            {
                this.admin = value;
                this.AdminUpdated = true;
            }
        }

        public ICollection<StorageListModel> AllFollowedLists
        {
            get
            {
                return this.allFollowedLists;
            }
        }

        public ICollection<StorageListModel> AllOwnedLists
        {
            get
            {
                return this.allOwnedLists;
            }
        }

        public string Description
        {
            get
            {
                this.Populate();
                return this.description;
            }

            set
            {
                this.description = value;
                this.DescriptionUpdated = true;
            }
        }

        public ICollection<StorageListModel> MemberOfLists
        {
            get
            {
                return this.memberLists;
            }
        }

        public string Name
        {
            get
            {
                this.Populate();
                return this.name;
            }
        }

        public StorageListModel PersonalList
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public ICollection<StorageListModel> PublicFollowedLists
        {
            get
            {
                return this.publicFollowedLists;
            }
        }

        public ICollection<StorageListModel> PublicOwnedLists
        {
            get
            {
                return this.publicOwnedLists;
            }
        }

        public ICollection<StorageUserModel> Users
        {
            get
            {
                return this.users;
            }
        }

        #endregion

        #region Properties

        internal ListCollectionAdapter InternalAllFollowedLists
        {
            get
            {
                return this.allFollowedLists;
            }
        }

        internal ListCollectionAdapter InternalAllOwnedLists
        {
            get
            {
                return this.allOwnedLists;
            }
        }

        internal ListCollectionAdapter InternalMemberOfLists
        {
            get
            {
                return this.memberLists;
            }
        }

        internal ListCollectionAdapter InternalPersonalList
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        internal ListCollectionAdapter InternalPublicFollowedLists
        {
            get
            {
                return this.publicFollowedLists;
            }
        }

        internal ListCollectionAdapter InternalPublicOwnedLists
        {
            get
            {
                return this.publicOwnedLists;
            }
        }

        internal UserCollectionAdapter InternalUsers
        {
            get
            {
                return this.users;
            }
        }

        protected override bool InfosUpdated
        {
            get
            {
                return this.DescriptionUpdated;
            }

            set
            {
                this.DescriptionUpdated = value;
            }
        }

        private bool AdminUpdated { get; set; }

        private bool DescriptionUpdated { get; set; }

        #endregion

        #region Methods

        internal override void Repopulate()
        {
            // Fetch infos
            var accountInfo = this.Storage.Account.GetInfo(this.Id);

            this.name = accountInfo.Name;

            if (!this.DescriptionUpdated)
            {
                this.description = accountInfo.Description;
            }

            this.Populated = true;
        }

        internal override void Save()
        {
            if (this.InfosUpdated)
            {
                this.Storage.Account.SetInfo(this.Id, this.Description);
            }

            if (this.AdminUpdated)
            {
                this.Storage.Account.SetAdminId(this.Id, this.Admin.Id);
            }

            this.users.Save();
        }

        private ListCollectionAdapter MakeListCollection(Func<ICollection<Guid>> func)
        {
            return new ListCollectionAdapter(this.Storage, this.StorageContext, this, func);
        }

        #endregion

        internal class ListCollectionAdapter : StorageEntityCollection<StorageAccountModel, StorageListModel>
        {
            #region Constructors and Destructors

            public ListCollectionAdapter(
                IStorage storage, 
                IStorageContext storageContext, 
                StorageAccountModel account, 
                Func<ICollection<Guid>> idCollectionFetcher)
                : base(storage, storageContext, account, idCollectionFetcher)
            {
                this.GetModel = storageContext.Lists.Find;
            }

            #endregion

            #region Methods

            internal override void Save()
            {
                foreach (var list in this.CollectionAdded.Where(item => item.Value).Select(item => item.Key))
                {
                    this.Storage.List.Add(list.Id, this.Parent.Id);
                }

                foreach (var list in this.CollectionRemoved.Where(item => item.Value).Select(item => item.Key))
                {
                    // TODO: check we are not removing the personal list ?
                    this.Storage.List.Remove(list.Id, this.Parent.Id);
                }

                foreach (var list in this.CachedCollection)
                {
                    list.Save();
                }

                this.CollectionAdded.Clear();
                this.CollectionRemoved.Clear();
            }

            protected override void ReverseAdd(StorageListModel item)
            {
                // item.CachedMembers.CacheAdd(this.Parent);
            }

            protected override void ReverseRemove(StorageListModel item)
            {
                // item.CachedMembers.CacheRemove(this.Parent);
            }

            #endregion
        }

        internal class UserCollectionAdapter : StorageEntityCollection<StorageAccountModel, StorageUserModel>
        {
            #region Constructors and Destructors

            public UserCollectionAdapter(IStorage storage, IStorageContext storageContext, StorageAccountModel account)
                : base(storage, storageContext, account, () => storage.Account.GetUsers(account.Id))
            {
                this.GetModel = storageContext.Users.Find;
            }

            #endregion

            #region Methods

            internal override void Save()
            {
                foreach (var user in this.CollectionAdded.Where(item => item.Value).Select(item => item.Key))
                {
                    user.Save();
                    this.Storage.Account.Add(this.Parent.Id, user.Id);
                }

                foreach (var user in this.CollectionRemoved.Where(item => item.Value).Select(item => item.Key))
                {
                    // TODO: check we are not removing the admin (?)
                    user.Save();
                    this.Storage.Account.Remove(this.Parent.Id, user.Id);
                }

                this.CollectionAdded.Clear();
                this.CollectionRemoved.Clear();
            }

            protected override void ReverseAdd(StorageUserModel item)
            {
                item.Accounts.Add(this.Parent);
            }

            protected override void ReverseRemove(StorageUserModel item)
            {
                item.Accounts.Remove(this.Parent);
            }

            #endregion
        }
    }
}