using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StorageLibrary;

namespace SetUp
{
    class Program
    {
        const string azureAccountName = "";
        const string azureAccountKey = "";
 
        // Lauch this program to reinit the storage
        static void Main(string[] args)
        {
            BlobFactory blobFactory = new BlobFactory(azureAccountName, azureAccountKey);
            blobFactory.InitStorage();
        }
    }
}
