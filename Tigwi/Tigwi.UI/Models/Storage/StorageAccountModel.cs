namespace Tigwi.UI.Models.Storage
{
    #region

    using System;
    using System.Collections.Generic;

    using StorageLibrary;

    #endregion

    public class StorageAccountModel
    {
        #region Constants and Fields

        private ListCollectionAdapter allFollowedLists;

        private ListCollectionAdapter allOwnedLists;

        private ListCollectionAdapter memberLists;

        private ListCollectionAdapter publicFollowedLists;

        private ListCollectionAdapter publicOwnedLists;

        private UserCollectionAdapter users;

        private string name;

        private string description;

        private StorageUserModel admin;

        #endregion

        #region Constructors and Destructors

        public StorageAccountModel(IStorage storage, IStorageContext storageContext, Guid accountId)
        {
            this.Storage = storage;
            this.StorageContext = storageContext;
            this.Id = accountId;
        }

        public StorageAccountModel(IStorage storage, IStorageContext storageContext, string name)
            : this(storage, storageContext, storage.Account.GetId(name))
        {
        }

        #endregion

        #region Public Properties

        public StorageUserModel Admin
        {
            get
            {
                return this.admin ?? (this.admin = this.StorageContext.Users.Find(this.Storage.Account.GetAdminId(this.Id)));
            }

            set
            {
                this.admin = value;
                this.AdminUpdated = true;
            }
        }

        public ICollection<ListModel> AllFollowedLists
        {
            get
            {
                return this.allFollowedLists
                       ??
                       (this.allFollowedLists =
                        new ListCollectionAdapter(
                            this.Storage, this.Id, () => this.Storage.List.GetAccountFollowedLists(this.Id, true)));
            }
        }

        public ICollection<ListModel> AllOwnedLists
        {
            get
            {
                return this.allOwnedLists
                       ??
                       (this.allOwnedLists =
                        new ListCollectionAdapter(
                            this.Storage, this.Id, () => this.Storage.List.GetAccountOwnedLists(this.Id, true)));
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
                this.InfosUpdated = true;
            }
        }

        public Guid Id { get; private set; }

        public ICollection<ListModel> MemberLists
        {
            get
            {
                return this.memberLists
                       ??
                       (this.memberLists =
                        new ListCollectionAdapter(
                            this.Storage, this.Id, () => this.Storage.List.GetFollowingLists(this.Id)));
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

        public ListModel PersonalList
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public ICollection<ListModel> PublicFollowedLists
        {
            get
            {
                return this.publicFollowedLists
                       ??
                       (this.publicFollowedLists =
                        new ListCollectionAdapter(
                            this.Storage, this.Id, () => this.Storage.List.GetAccountFollowedLists(this.Id, false)));
            }
        }

        public ICollection<ListModel> PublicOwnedLists
        {
            get
            {
                return this.publicOwnedLists
                       ??
                       (this.publicOwnedLists =
                        new ListCollectionAdapter(
                            this.Storage, this.Id, () => this.Storage.List.GetAccountOwnedLists(this.Id, false)));
            }
        }

        public ICollection<StorageUserModel> Users
        {
            get
            {
                return this.users ?? (this.users = new UserCollectionAdapter(this.Storage, this.StorageContext, this.Id));
            }
        }

        #endregion

        #region Properties

        private bool AdminUpdated { get; set; }

        private bool InfosFetched { get; set; }

        private bool InfosUpdated { get; set; }

        private IStorage Storage { get; set; }

        private IStorageContext StorageContext { get; set; }

        #endregion

        #region Public Methods and Operators

        public void Save()
        {
            if (this.InfosUpdated)
            {
                this.Storage.Account.SetInfo(this.Id, this.Description);
            }

            if (this.AdminUpdated)
            {
                this.Admin.Save();
                this.Storage.Account.SetAdminId(this.Id, this.Admin.Id);
            }

            this.users.Save();
        }

        #endregion

        #region Methods

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
            // Fetch infos
            IAccountInfo accountInfo = this.Storage.Account.GetInfo(this.Id);

            // TODO: shit may happen
            this.name = accountInfo.Name;
            this.description = accountInfo.Description;

            this.InfosFetched = true;
        }

        #endregion

        private abstract class AccountMemberCollection<T> : CollectionAdapter<T>
        {
            #region Constants and Fields

            private readonly IStorage storage;

            private readonly IStorageContext storageContext;

            #endregion

            #region Constructors and Destructors

            protected AccountMemberCollection(IStorage storage, IStorageContext storageContext, Guid accountId)
            {
                this.storage = storage;
                this.storageContext = storageContext;
                this.AccountId = accountId;
            }

            #endregion

            #region Properties

            protected Guid AccountId { get; private set; }

            protected IStorage Storage
            {
                get
                {
                    return this.storage;
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
        }

        private class ListCollectionAdapter : AccountMemberCollection<ListModel>
        {
            #region Constants and Fields

            private readonly Func<ICollection<Guid>> idCollectionFetcher;

            #endregion

            #region Constructors and Destructors

            public ListCollectionAdapter(
                IStorage storage, Guid accountId, Func<ICollection<Guid>> idCollectionFetcher)
                : base(storage, null, accountId)
            {
                this.idCollectionFetcher = idCollectionFetcher;
            }

            #endregion

            #region Properties

            private Func<ICollection<Guid>> IdCollectionFetcher
            {
                get
                {
                    return this.idCollectionFetcher;
                }
            }

            #endregion

            #region Public Methods and Operators

            public override void Save()
            {
                foreach (var item in this.CollectionAdded)
                {
                    this.Storage.List.Add(item.Id, this.AccountId);
                }

                foreach (var item in this.CollectionRemoved)
                {
                    // TODO: check we are not removing the personal list ?
                    this.Storage.List.Remove(item.Id, this.AccountId);
                }
            }

            #endregion

            #region Methods

            protected override ICollection<Guid> FetchIdCollection()
            {
                return this.IdCollectionFetcher();
            }

            protected override ListModel GetModel(Guid id)
            {
                throw new NotImplementedException();

                // return new ListModelAdapter(this.storage, () => id);
            }

            #endregion
        }

        private class UserCollectionAdapter : AccountMemberCollection<StorageUserModel>
        {
            #region Constructors and Destructors

            public UserCollectionAdapter(IStorage storage, IStorageContext storageContext, Guid accountId)
                : base(storage, storageContext, accountId)
            {
            }

            #endregion

            #region Public Methods and Operators

            public override void Save()
            {
                foreach (var item in this.CollectionAdded)
                {
                    this.Storage.Account.Add(this.AccountId, item.Id);
                }

                foreach (var item in this.CollectionRemoved)
                {
                    // TODO: check we are not removing the admin (?)
                    this.Storage.Account.Remove(this.AccountId, item.Id);
                }
            }

            #endregion

            #region Methods

            protected override ICollection<Guid> FetchIdCollection()
            {
                return this.Storage.Account.GetUsers(this.AccountId);
            }

            protected override StorageUserModel GetModel(Guid id)
            {
                return this.StorageContext.Users.Find(id);
            }

            #endregion
        }
    }
}