using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tigwi.Storage.Library;
using Microsoft.WindowsAzure.StorageClient;

namespace StorageLocalTest
{
    class Program
    {
        const string azureAccountName = "ulyssestorage";
        const string azureAccountKey = "fc2HTyfP0m2r3zlNYmMc3Pjvbfmy63ovoCP9Zkz0yoyuId3AeyrTswLcye2VDr3hzDvAQbdeKUlXBX3lFTcNWQ==";

        static void Main(string[] args)
        {
            Console.WriteLine("Clearing previous data");
            BlobFactory blobFactory = new BlobFactory(azureAccountName, azureAccountKey);
            blobFactory.InitStorage();

            Console.WriteLine("Init connexions");
            Storage storage = new Storage(azureAccountName, azureAccountKey);

            storage.Account.Autocompletion("zebingn1", 10);
            storage.Account.Autocompletion("blackgold", 10);
            storage.Account.Autocompletion("ulysse", 10);
            storage.Account.Autocompletion("babar", 10);

            Console.ReadLine();

        }
    }
}
