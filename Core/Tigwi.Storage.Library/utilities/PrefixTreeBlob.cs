using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;

namespace Tigwi.Storage.Library.Utilities
{
    class PrefixTreeBlob
    {
        CloudBlobDirectory dir;

        public PrefixTreeBlob(CloudBlobContainer container, string path)
        {
            dir = container.GetDirectoryReference(path);
        }

        public void Add(KeyValuePair<string, string> p)
        {
            HashSetBlob<KeyValuePair<string, string>> rootBlob = new HashSetBlob<KeyValuePair<string, string>>(dir.GetBlobReference("root"));
            rootBlob.AddWithRetry(p);
        }

        public void Init()
        {
            Blob<HashSet<KeyValuePair<string, string>>> rootBlob = new Blob<HashSet<KeyValuePair<string, string>>>(dir.GetBlobReference("root"));
            rootBlob.Set(new HashSet<KeyValuePair<string, string>>());
        }

        public HashSet<string> GetWithPrefix(string prefix, int maxSize)
        {
            Blob<HashSet<KeyValuePair<string, string>>> rootBlob = new Blob<HashSet<KeyValuePair<string, string>>>(dir.GetBlobReference("root"));
            HashSet<KeyValuePair<string, string>> set = rootBlob.Get();
            set.RemoveWhere((p) => p.Key.StartsWith(prefix));
            return new HashSet<string>(set.Select((p) => p.Value));
        }
    }
}