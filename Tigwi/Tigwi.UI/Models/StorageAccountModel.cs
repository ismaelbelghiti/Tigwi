// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StorageAccountModel.cs" company="">
//   
// </copyright>
// <summary>
//   The account model proxy.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Tigwi.UI.Models
{
    using System;
    using System.Collections.Generic;

    using StorageLibrary;

    /// <summary>
    /// The account model proxy.
    /// </summary>
    public class StorageAccountModel : AccountModel
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StorageAccountModel"/> class. 
        /// </summary>
        /// <param name="storage">
        /// The storage.
        /// </param>
        /// <param name="getId">
        /// The get id.
        /// </param>
        public StorageAccountModel(IStorage storage, Func<Guid> getId)
        {
            this.Storage = storage;
            this.IdFetcher = getId;
            base.Admin = new UserModelAdapter(this.Storage, () => this.Storage.Account.GetAdminId(this.Id));
            this.SpecializedUsers = new UserCollectionAdapter(this.Storage, () => this.Id);
            this.SpecializedPublicOwnedLists = new ListCollectionAdapter(
                this.Storage, () => this.Id, () => this.Storage.List.GetAccountOwnedLists(this.Id, false));
            this.SpecializedAllOwnedLists = new ListCollectionAdapter(
                this.Storage, () => this.Id, () => this.Storage.List.GetAccountOwnedLists(this.Id, true));
            this.SpecializedPublicFollowedLists = new ListCollectionAdapter(
                this.Storage, () => this.Id, () => this.Storage.List.GetAccountFollowedLists(this.Id, false));
            this.SpecializedAllFollowedLists = new ListCollectionAdapter(
                this.Storage, () => this.Id, () => this.Storage.List.GetAccountFollowedLists(this.Id, true));
            this.SpecializedMemberLists = new ListCollectionAdapter(
                this.Storage, () => this.Id, () => this.Storage.List.GetFollowingLists(this.Id));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StorageAccountModel"/> class. 
        /// </summary>
        /// <param name="storage">
        /// The account storage.
        /// </param>
        /// <param name="id">
        /// The id.
        /// </param>
        public StorageAccountModel(IStorage storage, Guid id)
            : this(storage, () => id)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StorageAccountModel"/> class. 
        /// </summary>
        /// <param name="storage">
        /// The account storage.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        public StorageAccountModel(IStorage storage, string name)
            : this(storage, () => storage.Account.GetId(name))
        {
            // Cache name
            base.Name = name;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets Admin.
        /// </summary>
        public override UserModel Admin
        {
            set
            {
                base.Admin = value;
                this.AdminUpdated = true;
            }
        }

        /// <summary>
        /// Gets AllFollowedLists.
        /// </summary>
        public override ICollection<ListModel> AllFollowedLists
        {
            get
            {
                return this.SpecializedAllFollowedLists;
            }
        }

        /// <summary>
        /// Gets AllOwnedLists.
        /// </summary>
        public override ICollection<ListModel> AllOwnedLists
        {
            get
            {
                return this.SpecializedAllOwnedLists;
            }
        }

        /// <summary>
        /// Gets or sets Description.
        /// </summary>
        public override string Description
        {
            get
            {
                this.FetchInfos();
                return base.Description;
            }

            set
            {
                base.Description = value;
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
                    base.Id = this.IdFetcher();
                    this.IdFetched = true;
                }

                return base.Id;
            }
        }

        /// <summary>
        /// Gets MemberLists.
        /// </summary>
        public override ICollection<ListModel> MemberLists
        {
            get
            {
                return this.SpecializedMemberLists;
            }
        }

        /// <summary>
        /// Gets Name.
        /// </summary>
        public override string Name
        {
            get
            {
                // Name might be cached
                if (base.Name == null)
                {
                    this.FetchInfos();
                }

                return base.Name;
            }
        }

        /// <summary>
        /// Gets PublicFollowedLists.
        /// </summary>
        public override ICollection<ListModel> PublicFollowedLists
        {
            get
            {
                return this.SpecializedPublicFollowedLists;
            }
        }

        /// <summary>
        /// Gets PublicOwnedLists.
        /// </summary>
        public override ICollection<ListModel> PublicOwnedLists
        {
            get
            {
                return this.SpecializedPublicOwnedLists;
            }
        }

        /// <summary>
        /// Gets Users.
        /// </summary>
        public override ICollection<UserModel> Users
        {
            get
            {
                return this.SpecializedUsers;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether AdminUpdated.
        /// </summary>
        private bool AdminUpdated { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IdFetched.
        /// </summary>
        private bool IdFetched { get; set; }

        /// <summary>
        /// Gets or sets IdFetcher.
        /// </summary>
        private Func<Guid> IdFetcher { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Fetched.
        /// </summary>
        private bool InfosFetched { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether DescriptionUpdated.
        /// </summary>
        private bool InfosUpdated { get; set; }

        /// <summary>
        /// Gets or sets SpecializedAllFollowedLists.
        /// </summary>
        private ListCollectionAdapter SpecializedAllFollowedLists { get; set; }

        /// <summary>
        /// Gets or sets SpecializedAllOwnedLists.
        /// </summary>
        private ListCollectionAdapter SpecializedAllOwnedLists { get; set; }

        /// <summary>
        /// Gets or sets SpecializedMemberLists.
        /// </summary>
        private ListCollectionAdapter SpecializedMemberLists { get; set; }

        /// <summary>
        /// Gets or sets SpecializedPublicFollowedLists.
        /// </summary>
        private ListCollectionAdapter SpecializedPublicFollowedLists { get; set; }

        /// <summary>
        /// Gets or sets SpecializedLists.
        /// </summary>
        private ListCollectionAdapter SpecializedPublicOwnedLists { get; set; }

        /// <summary>
        /// Gets or sets SpecializedUsers.
        /// </summary>
        private UserCollectionAdapter SpecializedUsers { get; set; }

        /// <summary>
        /// Gets or sets AccountStorage.
        /// </summary>
        private IStorage Storage { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The save.
        /// </summary>
        public override void Save()
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

            this.SpecializedUsers.Save();
        }

        #endregion

        #region Methods

        /// <summary>
        /// The fetch infos.
        /// </summary>
        private void FetchInfos()
        {
            if (this.InfosFetched)
            {
                return;
            }

            // Fetch infos
            // Notice it can fetch the Id as a side effect
            IAccountInfo accountInfo = this.Storage.Account.GetInfo(this.Id);

            // Don't forget shit that happens when updates where made
            base.Name = base.Name ?? accountInfo.Name;
            base.Description = base.Description ?? accountInfo.Description;

            this.InfosFetched = true;
        }

        #endregion

        /// <summary>
        /// The account member collection.
        /// </summary>
        /// <typeparam name="T">
        /// The inner member type.
        /// </typeparam>
        private abstract class AccountMemberCollection<T> : CollectionAdapter<T>
        {
            #region Constants and Fields

            /// <summary>
            /// The account id fetcher.
            /// </summary>
            private readonly Func<Guid> accountIdFetcher;

            /// <summary>
            /// The storage.
            /// </summary>
            private readonly IStorage storage;

            /// <summary>
            /// The account id.
            /// </summary>
            private Guid accountId;

            /// <summary>
            /// The account id fetched.
            /// </summary>
            private bool accountIdFetched;

            #endregion

            #region Constructors and Destructors

            /// <summary>
            /// Initializes a new instance of the <see cref="AccountMemberCollection{T}"/> class. 
            /// Initializes a new instance of the <see cref="AccountMemberCollection{T}"/> class. 
            /// Initializes a new instance of the <see cref="AccountMemberCollection{T}"/> class. 
            /// Initializes a new instance of the <see cref="AccountMemberCollection{T}"/> class. 
            /// Initializes a new instance of the <see cref="AccountMemberCollection"/> class. 
            /// Initializes a new instance of the <see cref="UserCollectionAdapter"/> class. 
            /// </summary>
            /// <param name="storage">
            /// The storage.
            /// </param>
            /// <param name="accountIdFetcher">
            /// The account id fetcher.
            /// </param>
            protected AccountMemberCollection(IStorage storage, Func<Guid> accountIdFetcher)
            {
                this.storage = storage;
                this.accountIdFetcher = accountIdFetcher;
            }

            #endregion

            #region Properties

            /// <summary>
            /// Gets AccountId.
            /// </summary>
            protected Guid AccountId
            {
                get
                {
                    if (!this.accountIdFetched)
                    {
                        this.accountId = this.accountIdFetcher();
                        this.accountIdFetched = true;
                    }

                    return this.accountId;
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

            #endregion
        }

        /// <summary>
        /// The list collection adapter.
        /// </summary>
        private class ListCollectionAdapter : AccountMemberCollection<ListModel>
        {
            #region Constants and Fields

            /// <summary>
            /// The id collection fetcher.
            /// </summary>
            private readonly Func<ICollection<Guid>> idCollectionFetcher;

            #endregion

            #region Constructors and Destructors

            /// <summary>
            /// Initializes a new instance of the <see cref="ListCollectionAdapter"/> class. 
            /// Initializes a new instance of the <see cref="ListCollectionAdapter"/> class. 
            /// Initializes a new instance of the <see cref="ListCollectionAdapter"/> class. 
            /// Initializes a new instance of the <see cref="ListCollectionAdapter"/> class. 
            /// Initializes a new instance of the <see cref="ListCollectionAdapter"/> class. 
            /// Initializes a new instance of the <see cref="ListCollectionAdapter"/> class. 
            /// </summary>
            /// <param name="storage">
            /// The storage.
            /// </param>
            /// <param name="accountIdFetcher">
            /// The account id fetcher.
            /// </param>
            /// <param name="idCollectionFetcher">
            /// The id Collection Fetcher.
            /// </param>
            public ListCollectionAdapter(
                IStorage storage, Func<Guid> accountIdFetcher, Func<ICollection<Guid>> idCollectionFetcher)
                : base(storage, accountIdFetcher)
            {
                this.idCollectionFetcher = idCollectionFetcher;
            }

            #endregion

            #region Properties

            /// <summary>
            /// Gets IdCollectionFetcher.
            /// </summary>
            private Func<ICollection<Guid>> IdCollectionFetcher
            {
                get
                {
                    return this.idCollectionFetcher;
                }
            }

            #endregion

            #region Public Methods and Operators

            /// <summary>
            /// The save.
            /// </summary>
            public override void Save()
            {
                foreach (ListModel item in this.CollectionAdded)
                {
                    this.Storage.List.Add(item.Id, this.AccountId);
                }

                foreach (ListModel item in this.CollectionRemoved)
                {
                    // TODO: check we are not removing the personal list ?
                    this.Storage.List.Remove(item.Id, this.AccountId);
                }
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
                return this.IdCollectionFetcher();
            }

            /// <summary>
            /// The create model.
            /// </summary>
            /// <param name="id">
            /// The id.
            /// </param>
            /// <returns>
            /// </returns>
            protected override ListModel GetModel(Guid id)
            {
                throw new NotImplementedException();

                // return new ListModelAdapter(this.storage, () => id);
            }

            #endregion
        }

        /// <summary>
        /// The user collection adapter.
        /// </summary>
        private class UserCollectionAdapter : AccountMemberCollection<UserModel>
        {
            #region Constructors and Destructors

            /// <summary>
            /// Initializes a new instance of the <see cref="UserCollectionAdapter"/> class. 
            /// Initializes a new instance of the <see cref="UserCollectionAdapter"/> class. 
            /// Initializes a new instance of the <see cref="UserCollectionAdapter"/> class. 
            /// Initializes a new instance of the <see cref="UserCollectionAdapter"/> class. 
            /// Initializes a new instance of the <see cref="UserCollectionAdapter"/> class. 
            /// Initializes a new instance of the <see cref="UserCollectionAdapter"/> class. 
            /// </summary>
            /// <param name="storage">
            /// The storage.
            /// </param>
            /// <param name="accountIdFetcher">
            /// The account id fetcher.
            /// </param>
            public UserCollectionAdapter(IStorage storage, Func<Guid> accountIdFetcher)
                : base(storage, accountIdFetcher)
            {
            }

            #endregion

            #region Public Methods and Operators

            /// <summary>
            /// The save.
            /// </summary>
            public override void Save()
            {
                foreach (UserModel item in this.CollectionAdded)
                {
                    this.Storage.Account.Add(this.AccountId, item.Id);
                }

                foreach (UserModel item in this.CollectionRemoved)
                {
                    // TODO: check we are not removing the admin (?)
                    this.Storage.Account.Remove(this.AccountId, item.Id);
                }
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
                return this.Storage.Account.GetUsers(this.AccountId);
            }

            /// <summary>
            /// The create model.
            /// </summary>
            /// <param name="id">
            /// The id.
            /// </param>
            /// <returns>
            /// </returns>
            protected override UserModel GetModel(Guid id)
            {
                return new UserModelAdapter(this.Storage, () => id);
            }

            #endregion
        }
    }
}