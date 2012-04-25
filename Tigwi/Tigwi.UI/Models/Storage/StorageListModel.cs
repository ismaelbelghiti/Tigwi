using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tigwi.UI.Models.Storage
{
    using StorageLibrary;

    public class StorageListModel : StorageEntityModel
    {
        public StorageListModel(IStorage storage, IStorageContext storageContext, Guid listId)
            : base(storage, storageContext, listId)
        {
        }

        public ICollection<StoragePostModel> PostsAfter(DateTime date, int maximum = 100)
        {
            var postsAfter = new List<StoragePostModel>(maximum);
            var msgCollection = this.Storage.Msg.GetListsMsgFrom(new HashSet<Guid> { this.Id }, date, maximum);
            postsAfter.AddRange(msgCollection.Select(msg => new StoragePostModel(this.StorageContext, msg)));
            return postsAfter;
        }

        public ICollection<StoragePostModel> Posts
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public ICollection<StorageAccountModel> Members
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public ICollection<StorageAccountModel> Followers
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        #region Overrides of StorageEntityModel

        protected override bool InfosUpdated
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override void Save()
        {
            throw new NotImplementedException();
        }

        public override void Repopulate()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}