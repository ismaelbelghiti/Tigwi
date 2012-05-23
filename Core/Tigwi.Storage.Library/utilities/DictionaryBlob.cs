using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;

namespace Tigwi.Storage.Library.Utilities
{

    public class DictionaryBlob<S,T> : Blob<Dictionary<S,T>>
    {
        public DictionaryBlob(CloudBlobContainer container, string blobName) : base(container, blobName) { }

        public DictionaryBlob(CloudBlob blob) : base(blob) { }

        public bool AddWithRetry(S key, T value)
        {
            Dictionary<S, T> dict;

            do
            {
                try
                {
                    dict = base.Get();
                }
                catch { return false; }

                dict.Add(key, value);
                

            } while (!base.TrySet(dict));

            return true;
        }

        public void Add(S key, T value)
        {
            Dictionary<S,T> dict = base.Get();
            dict.Add(key, value);
            base.Set(dict);
        }

        public void Remove(S key)
        {
            Dictionary<S,T> dict = base.Get();
            dict.Remove(key);
            base.Set(dict);
        }

        public bool RemoveWithRetry(S key)
        {
            Dictionary<S,T> dict;

            do
            {
                try
                {
                    dict = base.Get();
                }
                catch { return false; }

                dict.Remove(key);

            } while (!base.TrySet(dict));

            return true;
        }


        public bool AddIfNotInWithRetry(S key, T value, Exception e)
        {
            Dictionary<S, T> dict;
            do
            {
                dict = base.GetIfExists(e);
                if (dict.ContainsKey(key))
                    return false;
                dict.Add(key,value);
            } while (!base.TrySet(dict));

            return true;
        }

    }

    public class DictionaryBlob<T> : DictionaryBlob<T, bool>
    {
        public DictionaryBlob(CloudBlobContainer container, string blobName) : base(container, blobName) { }

        public bool AddWithRetry(T item)
        {
            return this.AddWithRetry(item, false);
        }

        public void Add(T item)
        {
            this.Add(item, false);
        }
        public bool AddIfNotInWithRetry(T item, Exception e)
        {
            return this.AddIfNotInWithRetry(item, false, e);
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
