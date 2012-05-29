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
        const string azureAccountKey = "73IH549/+HNKuXjF8LcIT2ad4v+acch8gn2Gm0GN1w9tRHFZGTm/fFOr8BRPV/1gHBKJ/1hLeB5IMdNGHv4VIA==";

        static void Main(string[] args)
        {
            Console.WriteLine("Clearing previous data");
            BlobFactory blobFactory = new BlobFactory(azureAccountName, azureAccountKey);
            blobFactory.InitStorage();

            Console.WriteLine("Init connexions");
            Storage storage = new Storage(azureAccountName, azureAccountKey);

            Guid userId = storage.User.Create("tagada", "taga@poulpe.fr", new byte[1]);
            Guid account1ID = storage.Account.Create(userId, "account1", "");
            Guid account2ID = storage.Account.Create(userId, "account2", "");
            Guid account3ID = storage.Account.Create(userId, "account3", "");
            Guid listID = storage.List.Create(account1ID, "list", "", false);
            HashSet<Guid> listSet = new HashSet<Guid>();
            listSet.Add(listID);
            storage.List.Add(listID, account2ID);
            storage.Msg.Post(account2ID, "A2 M1");
            storage.Msg.Post(account3ID, "A3 M1");
            storage.List.Add(listID, account3ID);

            List<IMessage> messages = storage.Msg.GetListsMsgTo(listSet, DateTime.MaxValue, 10);

            Console.ReadLine();

        }
    }
}
