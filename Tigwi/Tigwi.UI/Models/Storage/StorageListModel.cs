﻿namespace Tigwi.UI.Models.Storage
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using StorageLibrary;

    #endregion

    public class StorageListModel : StorageEntityModel
    {
        #region Constants and Fields

        private string description;

        private AccountCollectionAdapter followers;

        private bool isPersonal;

        private bool isPrivate;

        private AccountCollectionAdapter members;

        private string name;

        #endregion

        #region Constructors and Destructors

        public StorageListModel(IStorage storage, IStorageContext storageContext, Guid listId)
            : base(storage, storageContext, listId)
        {
        }

        #endregion

        #region Public Properties

        public string Description
        {
            get
            {
                if (!this.UpdatedDescription)
                {
                    this.Populate();
                }

                return this.description;
            }

            set
            {
                this.UpdatedDescription = true;
                this.description = value;
            }
        }

        public ICollection<StorageAccountModel> Followers
        {
            get
            {
                return this.followers
                       ??
                       (this.followers =
                        new AccountCollectionAdapter(
                            this.Storage, 
                            this.StorageContext, 
                            this, 
                            () => this.Storage.List.GetFollowingAccounts(this.Id), 
                            account =>
                                {
                                    account.InternalAllFollowedLists.CacheAdd(this);
                                    if (!this.IsPrivate)
                                    {
                                        account.InternalPublicFollowedLists.CacheAdd(this);
                                    }
                                },
                            account =>
                                {
                                    account.InternalAllFollowedLists.CacheRemove(this);
                                    if (!this.IsPrivate)
                                    {
                                        account.InternalPublicFollowedLists.CacheRemove(this);
                                    }
                                }));
            }
        }

        public bool IsPersonal
        {
            get
            {
                this.Populate();
                return this.isPersonal;
            }
        }

        public bool IsPrivate
        {
            get
            {
                if (!this.UpdatedPrivacy)
                {
                    this.Populate();
                }

                return this.isPrivate;
            }

            set
            {
                this.UpdatedPrivacy = true;
                this.isPrivate = value;
            }
        }

        public ICollection<StorageAccountModel> Members
        {
            get
            {
                return this.members
                       ??
                       (this.members =
                        new AccountCollectionAdapter(
                            this.Storage, 
                            this.StorageContext, 
                            this, 
                            () => this.Storage.List.GetAccounts(this.Id), 
                            account => account.InternalMemberOfLists.CacheAdd(this), 
                            account => account.InternalMemberOfLists.CacheRemove(this)));
            }
        }

        public string Name
        {
            get
            {
                if (!this.UpdatedName)
                {
                    this.Populate();
                }

                return this.name;
            }

            set
            {
                this.UpdatedName = true;
                this.name = value;
            }
        }

        #endregion

        #region Properties

        protected override bool InfosUpdated
        {
            get
            {
                return this.UpdatedName || this.UpdatedPrivacy || this.UpdatedDescription;
            }

            set
            {
                this.UpdatedName = this.UpdatedPrivacy = this.UpdatedDescription = value;
            }
        }

        private bool UpdatedDescription { get; set; }

        private bool UpdatedName { get; set; }

        private bool UpdatedPrivacy { get; set; }

        #endregion

        #region Public Methods and Operators

        public ICollection<StoragePostModel> PostsAfter(DateTime date, int maximum = 100)
        {
            var msgCollection = this.Storage.Msg.GetListsMsgFrom(new HashSet<Guid> { this.Id }, date, maximum);
            return
                new List<StoragePostModel>(msgCollection.Select(msg => new StoragePostModel(this.StorageContext, msg)))
                    .AsReadOnly();
        }

        public ICollection<StoragePostModel> PostsBefore(DateTime date, int maximum = 100)
        {
            var msgCollection = this.Storage.Msg.GetListsMsgFrom(new HashSet<Guid> { this.Id }, date, maximum);
            return
                new List<StoragePostModel>(msgCollection.Select(msg => new StoragePostModel(this.StorageContext, msg)))
                    .AsReadOnly();
        }

        internal override void Repopulate()
        {
            var infos = this.Storage.List.GetInfo(this.Id);
            this.isPersonal = infos.IsPersonnal;
            if (!this.UpdatedPrivacy)
            {
                this.isPrivate = infos.IsPrivate;
            }

            if (!this.UpdatedName)
            {
                this.name = infos.Name;
            }

            if (!this.UpdatedDescription)
            {
                this.description = infos.Description;
            }

            this.Populated = true;
        }

        internal override void Save()
        {
            if (this.InfosUpdated)
            {
                this.Storage.List.SetInfo(this.Id, this.Name, this.Description, this.IsPrivate);
            }

            if (this.members != null)
            {
                this.members.Save();
            }

            if (this.followers != null)
            {
                this.followers.Save();
            }
        }

        #endregion

        internal class AccountCollectionAdapter : StorageEntityCollection<StorageListModel, StorageAccountModel>
        {
            #region Constructors and Destructors

            public AccountCollectionAdapter(
                IStorage storage, 
                IStorageContext storageContext, 
                StorageListModel parent, 
                Func<ICollection<Guid>> fetchIdCollection, 
                Action<StorageAccountModel> reverseAdd, 
                Action<StorageAccountModel> reverseRemove)
                : base(storage, storageContext, parent, fetchIdCollection)
            {
                this.GetModel = storageContext.Accounts.Find;
                this.DoReverseAdd = reverseAdd;
                this.DoReverseRemove = reverseRemove;
            }

            #endregion

            #region Properties

            protected Action<StorageAccountModel> DoReverseAdd { get; set; }

            protected Action<StorageAccountModel> DoReverseRemove { get; set; }

            #endregion

            #region Public Methods and Operators

            internal override void Save()
            {
                foreach (var accountAdded in this.CollectionAdded.Where(item => item.Value).Select(item => item.Key))
                {
                    this.Storage.List.Add(this.Parent.Id, accountAdded.Id);
                }

                foreach (var accountRemoved in this.CollectionRemoved.Where(item => item.Value).Select(item => item.Key))
                {
                    this.Storage.List.Remove(this.Parent.Id, accountRemoved.Id);
                }

                this.CollectionAdded.Clear();
                this.CollectionRemoved.Clear();
            }

            #endregion

            #region Methods

            protected override void ReverseAdd(StorageAccountModel item)
            {
                this.DoReverseAdd(item);
            }

            protected override void ReverseRemove(StorageAccountModel item)
            {
                this.DoReverseRemove(item);
            }

            #endregion
        }
    }
}