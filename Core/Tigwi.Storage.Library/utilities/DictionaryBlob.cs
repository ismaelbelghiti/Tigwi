using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;

namespace Tigwi.Storage.Library.Utilities
{
    public class DictionaryBlob<T> : Blob<Dictionary<T,bool>>
    {
        public DictionaryBlob(CloudBlobContainer container, string blobName) : base(container, blobName) { }

        public bool AddWithRetry(T item)
        {
            Dictionary<T,bool> set;

            do
            {
                try
                {
                    set = base.Get();
                }
                catch { return false; }

                set.Add(item,false);
                

            } while (!base.TrySet(set));

            return true;
        }

        public void Add(T item)
        {
            Dictionary<T,bool> set = base.Get();
            set.Add(item,false);
            base.Set(set);
        }

        public void Remove(T item)
        {
            Dictionary<T, bool> set = base.Get();
            set.Remove(item);
            base.Set(set);
        }

        public bool RemoveWithRetry(T item)
        {
            Dictionary<T, bool> set;

            do
            {
                try
                {
                    set = base.Get();
                }
                catch { return false; }

                set.Remove(item);

            } while (!base.TrySet(set));

            return true;
        }

        public bool AddIfNotInWithRetry(T item, Exception e)
        {
            Dictionary<T, bool> set;
            do
            {
                set = base.GetIfExists(e);
                if (set.ContainsKey(item))
                    return false;
                set.Add(item,false);
            } while (!base.TrySet(set));

            return true;
        }

        public bool SetBool(T item,bool boolean)
        {
            Dictionary<T,bool> set;
            do
            {
                try
                {
                    set = base.Get();
                }
                catch { return false; }
                set[item] = boolean;
            } while (!base.TrySet(set));
            return true;
        }
    }
}
