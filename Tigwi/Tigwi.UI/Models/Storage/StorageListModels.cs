using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tigwi.UI.Models.Storage
{
    using System.Collections;

    // TODO: extend StorageEntityCollection
    public class StorageListModels : IListModels
    {
        public StorageListModels(StorageContext context, IEnumerable<Guid> ids)
        {
            this.InternalStorage = context;
            this.InternalDictionary = new Dictionary<Guid, StorageListModel>();
            foreach (var id in ids)
            {
                this.InternalDictionary.Add(id, context.InternalLists.InternalFind(id));
            }
        }

        protected IDictionary<Guid, StorageListModel> InternalDictionary { get; set; } 

        protected StorageContext InternalStorage { get; set; }

        #region Implementation of IEnumerable

        public IEnumerator<IListModel> GetEnumerator()
        {
            return this.InternalDictionary.Select(item => item.Value).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region Implementation of ICollection<IListModel>

        public void Add(IListModel item)
        {
            this.InternalDictionary.Add(
                item.Id, item as StorageListModel ?? this.InternalStorage.InternalLists.InternalFind(item.Id));
        }

        public void Clear()
        {
            this.InternalDictionary.Clear();
        }

        public bool Contains(IListModel item)
        {
            return this.InternalDictionary.ContainsKey(item.Id);
        }

        public void CopyTo(IListModel[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }

            if (array.Length + arrayIndex > this.InternalDictionary.Count)
            {
                throw new ArgumentOutOfRangeException("arrayIndex");
            }

            var i = arrayIndex;
            foreach (var model in this.InternalDictionary.Values)
            {
                array[i++] = model;
            }
        }

        public bool Remove(IListModel item)
        {
            return this.InternalDictionary.Remove(item.Id);
        }

        public int Count
        {
            get
            {
                return this.InternalDictionary.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return this.InternalDictionary.IsReadOnly;
            }
        }

        #endregion

        #region Implementation of IListModels

        public ICollection<Guid> Ids
        {
            get
            {
                return this.InternalDictionary.Keys;
            }
        }

        public ICollection<IPostModel> PostsAfter(DateTime date, int maximum = 100)
        {
            var msgCollection = this.InternalStorage.StorageObj.Msg.GetListsMsgFrom(
                new HashSet<Guid>(this.Ids), date, maximum);
            return
                new List<IPostModel>(msgCollection.Select(msg => new StoragePostModel(this.InternalStorage, msg)))
                    .AsReadOnly();
        }

        public ICollection<IPostModel> PostsBefore(DateTime date, int maximum = 100)
        {
            var msgCollection = this.InternalStorage.StorageObj.Msg.GetListsMsgTo(
                new HashSet<Guid>(this.Ids), date, maximum);
            return
                new List<IPostModel>(msgCollection.Select(msg => new StoragePostModel(this.InternalStorage, msg)))
                    .AsReadOnly();
        }

        #endregion
    }
}