using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using Tigwi.Storage.Library;
using Tigwi.Storage.Library.Utilities;

namespace Tigwi.Storage.Library
{
    public class Storage : IStorage
    {
        // Children declaration
        IUserStorage user;
        public IUserStorage User { get { return user; } }

        IAccountStorage account;
        public IAccountStorage Account { get { return account; } }
        
        IListStorage list;
        public IListStorage List { get { return list; } }

        IMsgStorage msg;
        public IMsgStorage Msg { get { return msg; } }

        BlobFactory blobFactory;

        // Initialisation
        public Storage(string azureAccountName, string azureKey)
        {
            blobFactory = new BlobFactory(azureAccountName, azureKey);

            // allocate childrens
            user = new UserStorage(blobFactory);
            account = new AccountStorage(blobFactory);
            list = new ListStorage(blobFactory);
            msg = new MsgStorage(blobFactory);
        }
    }
}
