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

        // Initialisation
        public Storage(string azureAccountName, string azureKey)
        {
            connexion = new StrgConnexion(azureAccountName, azureKey);

            // allocate childrens
            user = new UserStorage(connexion);
            account = new AccountStorage(connexion);
            list = new ListStorage(connexion);
            msg = new MsgStorage(connexion);
        }
    }
}
