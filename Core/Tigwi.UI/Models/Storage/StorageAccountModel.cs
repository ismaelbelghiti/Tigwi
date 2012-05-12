namespace Tigwi.UI.Models.Storage
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    using Tigwi.Storage.Library;

    public class StorageAccountModel : StorageEntityModel, IAccountModel
    {
        #region Constants and Fields

        private readonly ListCollectionAdapter allFollowedLists;

        private readonly ListCollectionAdapter allOwnedLists;

        private readonly ListCollectionAdapter memberOfLists;

        private readonly ListCollectionAdapter publicFollowedLists;

        private readonly ListCollectionAdapter publicOwnedLists;

        private readonly StorageEntityCollection<StorageUserModel, IUserModel> users;

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
            this.users = new StorageEntityCollection<StorageUserModel, IUserModel>(this.StorageContext)
                {
                    FetchIdCollection = () => storage.Account.GetUsers(accountId),
                    GetId = user => user.Id,
                    GetModel = storageContext.InternalUsers.InternalFind,
                    ReverseAdd = user => user.InternalAccounts.CacheAdd(this),
                    ReverseRemove = user => user.InternalAccounts.CacheRemove(this),
                    SaveAdd = user => storage.Account.Add(this.Id, user.Id),
                    SaveRemove = user => storage.Account.Remove(this.Id, user.Id),
                };

            this.memberOfLists = new ListCollectionAdapter(this.StorageContext)
                {
                    FetchIdCollection = () => storage.List.GetFollowingLists(accountId),
                    GetId = list => list.Id,
                    GetModel = storageContext.InternalLists.InternalFind,
                    ReverseAdd = list => list.InternalMembers.CacheAdd(this),
                    ReverseRemove = list => list.InternalMembers.CacheRemove(this),
                    SaveAdd = list => storage.List.Add(list.Id, this.Id),
                    SaveRemove = list => storage.List.Remove(list.Id, this.Id),
                };

            this.allFollowedLists =
                new ListCollectionAdapter(storageContext)
                    {
                        FetchIdCollection = () => storage.List.GetAccountFollowedLists(accountId, true),
                        GetId = list => list.Id,
                        GetModel = storageContext.InternalLists.InternalFind,
                        ReverseAdd = list =>
                            {
                                // TODO: shouldn't call storage
                                list.InternalFollowers.CacheAdd(this);
                                if (!list.IsPrivate)
                                {
                                    this.InternalPublicFollowedLists.CacheAdd(list);
                                }
                            },
                        ReverseRemove = list =>
                            {
                                // TODO: shouldn't call storage
                                list.InternalFollowers.CacheRemove(this);
                                if (!list.IsPrivate)
                                {
                                    this.InternalPublicFollowedLists.CacheRemove(list);
                                }
                            },
                        SaveAdd = list => storage.List.Follow(list.Id, this.Id),
                        SaveRemove = list => storage.List.Unfollow(list.Id, this.Id),
                    };

            this.publicFollowedLists =
                new ListCollectionAdapter(storageContext)
                    {
                        FetchIdCollection = () => storage.List.GetAccountFollowedLists(accountId, true),
                        GetId = list => list.Id,
                        GetModel = storageContext.InternalLists.InternalFind,
                        ReverseAdd = list => Contract.Assert(false),
                        ReverseRemove = list => Contract.Assert(false),
                        SaveAdd = list => Contract.Assert(false),
                        SaveRemove = list => Contract.Assert(false),
                    };

            this.allOwnedLists =
                new ListCollectionAdapter(storageContext)
                    {
                        FetchIdCollection = () => storage.List.GetAccountOwnedLists(accountId, true),
                        GetId = list => list.Id,
                        GetModel = storageContext.InternalLists.InternalFind,
                        ReverseAdd = list => Contract.Assert(false),
                        ReverseRemove = list => Contract.Assert(false),
                        SaveAdd = list => Contract.Assert(false),
                        SaveRemove = list => Contract.Assert(false),
                    };

            this.publicOwnedLists =
                new ListCollectionAdapter(storageContext)
                {
                    FetchIdCollection = () => storage.List.GetAccountOwnedLists(accountId, false),
                    GetId = list => list.Id,
                    GetModel = storageContext.InternalLists.InternalFind,
                    ReverseAdd = list => Contract.Assert(false),
                    ReverseRemove = list => Contract.Assert(false),
                    SaveAdd = list => Contract.Assert(false),
                    SaveRemove = list => Contract.Assert(false),
                };
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

        public IListModelEnumerable AllOwnedLists
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
                return this.memberOfLists;
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

        public IListModelEnumerable PublicFollowedLists
        {
            get
            {
                return this.publicFollowedLists;
            }
        }

        public IListModelEnumerable PublicOwnedLists
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
                return this.memberOfLists;
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

        internal StorageEntityCollection<StorageUserModel, IUserModel> InternalUsers
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
                this.memberOfLists.Save();
                if (this.personalList != null)
                {
                    this.personalList.Save();
                }
            }
        }

        #endregion

        internal class ListCollectionAdapter :
            StorageEntityCollection<StorageListModel, IListModel>, IListModelCollection
        {
            public ListCollectionAdapter(StorageContext storageContext)
                : base(storageContext)
            {
            }

            #region Implementation of IListModelCollection

            public IEnumerable<Guid> Ids
            {
                get
                {
                    return this.InternalCollection.Keys;
                }
            }

            public IEnumerable<IPostModel> PostsAfter(DateTime date, int maximum = 100)
            {
                var msgCollection = this.Storage.Msg.GetListsMsgFrom(
                new HashSet<Guid>(this.Ids), date, maximum);
                return
                    new List<IPostModel>(msgCollection.Select(msg => new StoragePostModel(this.StorageContext, msg)))
                        .AsReadOnly();
            }

            public IEnumerable<IPostModel> PostsBefore(DateTime date, int maximum = 100)
            {
                var msgCollection = this.Storage.Msg.GetListsMsgTo(
                new HashSet<Guid>(this.Ids), date, maximum);
                return
                    new List<IPostModel>(msgCollection.Select(msg => new StoragePostModel(this.StorageContext, msg)))
                        .AsReadOnly();
            }

            #endregion
        }
    }
}