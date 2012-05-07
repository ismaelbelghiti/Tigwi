namespace Tigwi.UI.Models.Storage
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Tigwi.Storage.Library;

    #endregion

    public class StorageAccountModel : StorageEntityModel, IAccountModel
    {
        #region Constants and Fields

        private readonly ListCollectionAdapter allFollowedLists;

        private readonly ListCollectionAdapter allOwnedLists;

        private readonly ListCollectionAdapter memberLists;

        private readonly ListCollectionAdapter publicFollowedLists;

        private readonly ListCollectionAdapter publicOwnedLists;

        private readonly UserCollectionAdapter users;

        private StorageUserModel admin;

        private string avatar;

        private string description;

        private string name;

        private StorageListModel personalList;

        #endregion

        #region Constructors and Destructors

        public StorageAccountModel(IStorage storage, StorageContext storageContext, Guid accountId)
            : base(storage, storageContext, accountId)
        {
            // Users
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

        public IUserModel Admin
        {
            get
            {
                // Admin's initialization involves storage calls, so let's be lazy
                return this.admin
                       ??
                       (this.admin =
                        this.StorageContext.InternalUsers.InternalFind(this.Storage.Account.GetAdminId(this.Id)));
            }

            set
            {
                this.admin = value as StorageUserModel ?? this.StorageContext.InternalUsers.InternalFind(value.Id);
                this.AdminUpdated = true;
            }
        }

        public IListModelCollection AllFollowedLists
        {
            get
            {
                return this.allFollowedLists;
            }
        }

        public IListModelCollection AllOwnedLists
        {
            get
            {
                return this.allOwnedLists;
            }
        }

        public string Avatar
        {
            get
            {
                if (!this.Prefetched)
                {
                    this.Populate();
                }

                return this.avatar;
            }

            set
            {
                this.avatar = value;
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

        public IListModelCollection MemberOfLists
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
                if (!this.Prefetched)
                {
                    this.Populate();
                }

                return this.name;
            }
        }

        public IListModel PersonalList
        {
            get
            {
                return this.InternalPersonalList;
            }
        }

        public IListModelCollection PublicFollowedLists
        {
            get
            {
                return this.publicFollowedLists;
            }
        }

        public IListModelCollection PublicOwnedLists
        {
            get
            {
                return this.publicOwnedLists;
            }
        }

        public ICollection<IUserModel> Users
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

        internal StorageListModel InternalPersonalList
        {
            get
            {
                return this.personalList
                       ??
                       (this.personalList =
                        this.StorageContext.InternalLists.InternalFind(this.Storage.List.GetPersonalList(this.Id)));
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

        protected bool Prefetched { get; set; }

        private bool AdminUpdated { get; set; }

        private bool DescriptionUpdated { get; set; }

        #endregion

        #region Public Methods and Operators

        public StorageAccountModel PopulateWith(string name, string avatar)
        {
            this.Prefetched = true;
            this.name = name;
            this.avatar = avatar;
            return this;
        }

        #endregion

        #region Methods

        internal override void Repopulate()
        {
            // TODO: repopulate adapters ?
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
            if (this.Deleted)
            {
                this.Storage.Account.Delete(this.Id);
            }
            else
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
                this.allFollowedLists.Save();
                this.allOwnedLists.Save();
                this.memberLists.Save();
                this.publicFollowedLists.Save();
                this.publicOwnedLists.Save();
                if (this.personalList != null)
                {
                    this.personalList.Save();
                }
            }
        }

        private ListCollectionAdapter MakeListCollection(Func<ICollection<Guid>> func)
        {
            return new ListCollectionAdapter(this.Storage, this.StorageContext, this, func);
        }

        #endregion

        internal class ListCollectionAdapter :
            StorageEntityCollection<StorageAccountModel, StorageListModel, IListModel>, IListModelCollection
        {
            #region Constructors and Destructors

            public ListCollectionAdapter(
                IStorage storage, 
                StorageContext storageContext, 
                StorageAccountModel account, 
                Func<ICollection<Guid>> idCollectionFetcher)
                : base(storage, storageContext, account, idCollectionFetcher, list => list.Id)
            {
                this.GetModel = storageContext.InternalLists.InternalFind;
            }

            #endregion

            #region Methods

            internal override void Save()
            {
                if (!this.Parent.Deleted)
                {
                    foreach (
                        var list in
                            this.CollectionAdded.Where(item => item.Value && !item.Key.Deleted).Select(item => item.Key)
                        )
                    {
                        this.Storage.List.Add(list.Id, this.Parent.Id);
                    }

                    foreach (
                        var list in
                            this.CollectionRemoved.Where(item => item.Value && !item.Key.Deleted).Select(
                                item => item.Key))
                    {
                        // TODO: check we are not removing the personal list ?
                        this.Storage.List.Remove(list.Id, this.Parent.Id);
                    }
                }

                this.CollectionAdded.Clear();
                this.CollectionRemoved.Clear();
            }

            protected override void ReverseAdd(StorageListModel item)
            {
                // TODO item.CachedMembers.CacheAdd(this.Parent);
            }

            protected override void ReverseRemove(StorageListModel item)
            {
                // TODO item.CachedMembers.CacheRemove(this.Parent);
            }

            #endregion

            #region Implementation of IListModelCollection

            public ICollection<Guid> Ids
            {
                get
                {
                    // TODO: berk berk berk
                    var ids = new HashSet<Guid>(this.InternalCollection.Select(model => model.Id));
                    return ids;
                }
            }

            public ICollection<IPostModel> PostsAfter(DateTime date, int maximum = 100)
            {
                var msgCollection = this.Parent.StorageContext.StorageObj.Msg.GetListsMsgFrom(
                new HashSet<Guid>(this.Ids), date, maximum);
                return
                    new List<IPostModel>(msgCollection.Select(msg => new StoragePostModel(this.Parent.StorageContext, msg)))
                        .AsReadOnly();
            }

            public ICollection<IPostModel> PostsBefore(DateTime date, int maximum = 100)
            {
                var msgCollection = this.Parent.StorageContext.StorageObj.Msg.GetListsMsgTo(
                new HashSet<Guid>(this.Ids), date, maximum);
                return
                    new List<IPostModel>(msgCollection.Select(msg => new StoragePostModel(this.Parent.StorageContext, msg)))
                        .AsReadOnly();
            }

            #endregion
        }

        internal class UserCollectionAdapter :
            StorageEntityCollection<StorageAccountModel, StorageUserModel, IUserModel>
        {
            #region Constructors and Destructors

            public UserCollectionAdapter(IStorage storage, StorageContext storageContext, StorageAccountModel account)
                : base(storage, storageContext, account, () => storage.Account.GetUsers(account.Id), user => user.Id)
            {
                this.GetModel = storageContext.InternalUsers.InternalFind;
            }

            #endregion

            #region Methods

            internal override void Save()
            {
                foreach (var user in this.CollectionAdded.Where(item => item.Value).Select(item => item.Key))
                {
                    this.Storage.Account.Add(this.Parent.Id, user.Id);
                }

                foreach (var user in this.CollectionRemoved.Where(item => item.Value).Select(item => item.Key))
                {
                    // TODO: check we are not removing the admin (?)
                    this.Storage.Account.Remove(this.Parent.Id, user.Id);
                }

                this.CollectionAdded.Clear();
                this.CollectionRemoved.Clear();
            }

            protected override void ReverseAdd(StorageUserModel item)
            {
                item.InternalAccounts.CacheAdd(this.Parent);
            }

            protected override void ReverseRemove(StorageUserModel item)
            {
                item.InternalAccounts.CacheRemove(this.Parent);
            }

            #endregion
        }
    }
}