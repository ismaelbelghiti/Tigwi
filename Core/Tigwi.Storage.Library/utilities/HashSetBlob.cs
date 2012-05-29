#region copyright
// Copyright (c) 2012, TIGWI
// All rights reserved.
// Distributed under  BSD 2-Clause license
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;

namespace Tigwi.Storage.Library.Utilities
{
    public class HashSetBlob<T> : Blob<HashSet<T>>
    {
        public HashSetBlob(CloudBlobContainer container, string blobName) : base(container, blobName) { }

        public HashSetBlob(CloudBlob blob) : base(blob) { }

        public bool AddWithRetry(T item)
        {
            HashSet<T> set;

            do
            {
                try
                {
                    set = base.Get();
                }
                catch { return false; }

                set.Add(item);
                

            } while (!base.TrySet(set));

            return true;
        }

        public void Add(T item)
        {
            HashSet<T> set = base.Get();
            set.Add(item);
            base.Set(set);
        }

        public void Remove(T item)
        {
            HashSet<T> set = base.Get();
            set.Remove(item);
            base.Set(set);
        }

        public bool RemoveWithRetry(T item)
        {
            HashSet<T> set;

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
            HashSet<T> set;
            do
            {
                set = base.GetIfExists(e);
                if (set.Contains(item))
                    return false;
                set.Add(item);
            } while (!base.TrySet(set));

            return true;
        }
    }
}
