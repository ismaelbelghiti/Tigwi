using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using StorageLibrary;
using StorageLibrary.Utilities;

namespace StorageLibrary
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


        public StrgConnexion connexion;
        BlobFactory blobFactory;

        // Initialisation
        public Storage(string azureAccountName, string azureKey)
        {
            connexion = new StrgConnexion(azureAccountName, azureKey);  // TODO : only the blobFactory should be needed
            blobFactory = new BlobFactory(azureAccountName, azureKey);

            // allocate childrens
            user = new UserStorage(blobFactory);
            account = new AccountStorage(connexion, blobFactory);
            list = new ListStorage(connexion, blobFactory);
            msg = new MsgStorage(connexion, blobFactory);
        }
    }
}
