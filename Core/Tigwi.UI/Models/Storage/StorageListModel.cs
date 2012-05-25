namespace Tigwi.UI.Models.Storage
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Tigwi.Storage.Library;

    #endregion

    public class StorageListModel : StorageEntityModel, IListModel
    {
        #region Constants and Fields

        private readonly StorageEntityCollection<StorageAccountModel, IAccountModel> followers;

        private readonly StorageEntityCollection<StorageAccountModel, IAccountModel> members;

        private StorageAccountModel owner;

        private string description;

        private bool isPersonal;

        private bool isPrivate;

        private string name;

        #endregion

        #region Constructors and Destructors

        public StorageListModel(IStorage storage, StorageContext storageContext, Guid listId)
            : base(storage, storageContext, listId)
        {
            this.followers = new StorageEntityCollection<StorageAccountModel, IAccountModel>(storageContext)
                {
                    FetchIdCollection = () => storage.List.GetFollowingAccounts(listId), 
                    GetId = account => account.Id, 
                    GetModel = storageContext.InternalAccounts.InternalFind, 
                    ReverseAdd = account =>
                        {
                            // TODO: this shouldn't involve storage calls
                            account.InternalAllFollowedLists.CacheAdd(this);
                            if (!this.IsPrivate)
                            {
                                account.InternalPublicFollowedLists.CacheAdd(this);
                            }
                        }, 
                    ReverseRemove = account =>
                        {
                            // TODO: this shouldn't involve storage calls
                            account.InternalAllFollowedLists.CacheRemove(this);
                            if (!this.IsPrivate)
                            {
                                account.InternalPublicFollowedLists.CacheRemove(this);
                            }
                        }, 
                    SaveAdd = account => storage.List.Follow(listId, account.Id), 
                    SaveRemove = account => storage.List.Unfollow(listId, account.Id)
                };

            this.members = new StorageEntityCollection<StorageAccountModel, IAccountModel>(storageContext)
                {
                    FetchIdCollection = () => storage.List.GetAccounts(listId), 
                    GetId = account => account.Id, 
                    GetModel = storageContext.InternalAccounts.InternalFind, 
                    ReverseAdd = account => account.InternalMemberOfLists.CacheAdd(this), 
                    ReverseRemove = account => account.InternalMemberOfLists.CacheRemove(this), 
                    SaveAdd = account => storage.List.Add(listId, account.Id), 
                    SaveRemove = account => storage.List.Remove(listId, account.Id)
                };
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
                if (value == this.description)
                {
                    return;
                }

                this.UpdatedDescription = true;
                this.description = value;
            }
        }

        public ICollection<IAccountModel> Followers
        {
            get
            {
                return this.followers;
            }
        }

        public IAccountModel Owner
        {
            get
            {
                return this.owner
                       ??
                       (this.owner =
                        this.StorageContext.InternalAccounts.InternalFind(this.Storage.List.GetOwner(this.Id)));
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
                if (this.isPrivate == value)
                {
                    return;
                }

                this.UpdatedPrivacy = true;
                this.isPrivate = value;
            }
        }

        public ICollection<IAccountModel> Members
        {
            get
            {
                return this.members;
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
                if (this.name != value)
                {
                    return;
                }

                this.UpdatedName = true;
                this.name = value;
            }
        }

        #endregion

        #region Properties

        internal StorageEntityCollection<StorageAccountModel, IAccountModel> InternalFollowers
        {
            get
            {
                return this.followers;
            }
        }

        internal StorageEntityCollection<StorageAccountModel, IAccountModel> InternalMembers
        {
            get
            {
                return this.members;
            }
        }

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

        public IEnumerable<IPostModel> PostsAfter(DateTime date, int maximum = 100)
        {
            var msgCollection = this.Storage.Msg.GetListsMsgFrom(new HashSet<Guid> { this.Id }, date, maximum);
            return new List<IPostModel>(msgCollection.Select(msg => new StoragePostModel(this.StorageContext, msg)));
        }

        public IEnumerable<IPostModel> PostsBefore(DateTime date, int maximum = 100)
        {
            var msgCollection = this.Storage.Msg.GetListsMsgTo(new HashSet<Guid> { this.Id }, date, maximum);
            return new List<IPostModel>(msgCollection.Select(msg => new StoragePostModel(this.StorageContext, msg)));
        }

        #endregion

        #region Methods

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

        internal override bool Save()
        {
            var success = true;

            if (this.Deleted)
            {
                this.Storage.List.Delete(this.Id);
            }
            else
            {
                if (this.InfosUpdated)
                {
                    try
                    {
                        this.Storage.List.SetInfo(this.Id, this.Name, this.Description, this.IsPrivate);
                    }
                    catch (StorageLibException)
                    {
                        success = false;
                    }
                }

                if (this.members != null)
                {
                    success &= this.members.Save();
                }

                if (this.followers != null)
                {
                    success &= this.followers.Save();
                }
            }

            return success;
        }

        #endregion
    }
}