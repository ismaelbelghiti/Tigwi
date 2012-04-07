using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageLibrary
{

    public class UserStorageTmp : IUserStorage
    {
        private Dictionary<Guid, UserInfo> dico_id_info;
        private Dictionary<string, Guid> dico_login_id;
        private StorageTmp motherStorage;
        int next_free_id = 0;

        public Guid GetId(string login)
        {
            Guid res;
            dico_login_id.TryGetValue(login, out res);
            return res;
        }

        public IUserInfo GetInfo(Guid user_id)
        {
            UserInfo res;
            dico_id_info.TryGetValue(user_id, out res);
            return res;
        }

        public void SetInfo(Guid userId, string email)
        {
            try
            {
                dico_id_info[userId].Email = email;
            }
            catch (Exception)
            {
                throw new UserNotFound();
            }
        }

        public HashSet<Guid> GetAccounts(Guid userId)
        {
            return motherStorage.account.GetAccounts(userId);
        }

        public Guid Create(string login, string email)
        {
            dico_id_info.Add( new Guid(next_free_id, 0, 0, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }), new UserInfo(login, email));
            dico_login_id.Add(login, new Guid(next_free_id, 0, 0, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }));
            next_free_id++;
            return  new Guid(next_free_id - 1, 0, 0, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 });
        }

        public void Delete(Guid user_id)
        {
            throw new NotImplementedException();
        }

        public UserStorageTmp(StorageTmp storage)
        {
            motherStorage = storage;
            dico_id_info = new Dictionary<Guid, UserInfo>();
            dico_login_id = new Dictionary<string, Guid>();
        }

        public void afficheDebug()
        {
            Console.WriteLine("Affichage des users");
            for(int id = 0; id < next_free_id; id++)
            {
                IUserInfo myUserInfo = GetInfo( new Guid(id, 0, 0, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }));
                Console.Write("   ");
                Console.Write(id);
                Console.Write(" : ");
                Console.WriteLine(myUserInfo.Login);
            }
            Console.WriteLine("");
        }
    }

    public class AccountStorageTmp : IAccountStorage
    {
        private Dictionary<Guid, IAccountInfo> dico_id_info;
        private Dictionary<string, Guid> dico_name_id;
        private Dictionary<Guid, HashSet<Guid>> dico_id_users;
        private Dictionary<Guid, HashSet<Guid>> dico_user_idaccounts;
        private int next_free_id = 0;
        private StorageTmp motherStorage;

        public Guid GetId(string name)
        {
            Guid res;
            dico_name_id.TryGetValue(name, out res);
            return res;
        }

        public IAccountInfo GetInfo(Guid accountId)
        {
            IAccountInfo res;
            dico_id_info.TryGetValue(accountId, out res);
            return res;
        }

        public void SetInfo(Guid Id, string name, string description)
        {
            dico_id_info.Add(Id, new AccountInfo(name, description));
        }

        public HashSet<Guid> GetUsers(Guid accountId)
        {
            HashSet<Guid> res;
            dico_id_users.TryGetValue(accountId, out res);
            return res;
        }

        public HashSet<Guid> GetAccounts(Guid userId)
        {
            HashSet<Guid> res;
            dico_user_idaccounts.TryGetValue(userId, out res);
            return res;
        }

        public Guid GetAdminId(Guid accountId)
        {
            throw new NotImplementedException();
        }

        public void SetAdminId(Guid accountId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public void Add(Guid accountId, Guid userId)
        {
            HashSet<Guid> setUsers;
            if(!dico_id_users.TryGetValue(accountId, out setUsers))
            {
                setUsers = new HashSet<Guid>();
                dico_id_users.Add(accountId, setUsers);
            }
            setUsers.Add(userId);

            HashSet<Guid> setAccounts;
            if (!dico_user_idaccounts.TryGetValue(userId, out setAccounts))
            {
                setAccounts = new HashSet<Guid>();
                dico_user_idaccounts.Add(userId, setAccounts);
            }
            setAccounts.Add(accountId);
        }

        public void Remove(Guid userId, Guid accountId)
        {
            throw new NotImplementedException();
            //HashSet<int> setUsers;
            //dico_id_users.TryGetValue(accountId, out setUsers);
            //setUsers.Remove(userId);

            //HashSet<int> setAccounts;
            //dico_user_idaccounts.TryGetValue(userId, out setAccounts);
            //setAccounts.Remove(accountId);
        }

        public Guid Create(Guid adminId, string name, string description)
        {
            dico_id_info.Add(new Guid(next_free_id, 0, 0, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }), new AccountInfo(name, description));
            dico_name_id.Add(name, new Guid(next_free_id, 0, 0, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }));
            next_free_id++;
            return new Guid(next_free_id - 1, 0, 0, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 });
        }

        public void Delete(Guid accountId) { }

        public AccountStorageTmp(StorageTmp storage)
        {
            motherStorage = storage;
            dico_id_info = new Dictionary<Guid, IAccountInfo>();
            dico_name_id = new Dictionary<string, Guid>();
            dico_user_idaccounts = new Dictionary<Guid, HashSet<Guid>>();
            dico_id_users = new Dictionary<Guid, HashSet<Guid>>();
        }

        public void afficheDebug()
        {
            Console.WriteLine("Affichage des comptes");
            foreach (KeyValuePair<Guid, HashSet<Guid>> kvp in dico_id_users)
            {
                Console.Write("  ");
                Console.WriteLine( (GetInfo(kvp.Key)).Name);
                foreach(Guid idUser in kvp.Value)
                {
                   Console.Write("     ");
                   Console.WriteLine((motherStorage.User.GetInfo(idUser)).Login);
                }
            }
            Console.WriteLine("");
        }

    }

    public class ListStorageTmp : IListStorage
     {
         private int next_free_id = 0;
         private StorageTmp motherStorage;
         private Dictionary<Guid, IListInfo> dico_id_info;
         private Dictionary<Guid, Guid> dico_id_idowner;
         private Dictionary<Guid, HashSet<Guid>> dico_id_idaccounts;
         private Dictionary<Guid, Guid> dico_idaccount_idperslist;
         private Dictionary<Guid, HashSet<Guid>> dico_idaccount_idownedlists;
         private Dictionary<Guid, HashSet<Guid>> dico_id_idfollowers;
         private Dictionary<Guid, HashSet<Guid>> dico_idaccount_idFollowedLists;

         public IListInfo GetInfo(Guid listId)
         {
             IListInfo res;
             dico_id_info.TryGetValue(listId, out res);
             return res;
         }

         public Guid GetOwner(Guid listId)
         {
             Guid res;
             dico_id_idowner.TryGetValue(listId, out res);
             return res;
         }

         public Guid GetPersonalList(Guid accountId)
         {
             Guid idPersList;
             if (!dico_idaccount_idperslist.TryGetValue(accountId, out idPersList))
             {
                 idPersList = Create(accountId, "Liste perso de " + motherStorage.Account.GetInfo(accountId).Name, "", false);
                 dico_idaccount_idperslist.Add(accountId, idPersList);
                 Add(idPersList, accountId);
             }
             return idPersList;
         }

         public void SetInfo(Guid listId, string name, string description, bool isPrivate)
         {
              dico_id_info.Add(listId, new ListInfo(name, description, isPrivate));
         }

         public Guid Create(Guid ownerId, string name, string description, bool isPrivate)
         {
              Guid newGuid = new Guid(next_free_id, 0, 0, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 });
              dico_id_info.Add(newGuid, new ListInfo(name, description, isPrivate));
              dico_id_idowner.Add(newGuid, ownerId);

             HashSet<Guid> setOwnedListsId;
             if (!dico_idaccount_idownedlists.TryGetValue(ownerId, out setOwnedListsId))
             {
                 setOwnedListsId = new HashSet<Guid>();
                 dico_idaccount_idownedlists.Add(ownerId, setOwnedListsId);
             }
             setOwnedListsId.Add(newGuid);

             next_free_id++;
             return new Guid(next_free_id - 1, 0, 0, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 });
         }

         public void Delete(Guid ownerId)
         {
             throw new NotImplementedException();
         }

         public void Follow(Guid listId, Guid accountId)
         {
             HashSet<Guid> setFollowers;
             if (!dico_id_idfollowers.TryGetValue(listId, out setFollowers))
             {
                 setFollowers = new HashSet<Guid>();
                 dico_id_idfollowers.Add(listId, setFollowers);
             }
             setFollowers.Add(accountId);

             HashSet<Guid> setFollowedLists;
             if (!dico_idaccount_idFollowedLists.TryGetValue(listId, out setFollowedLists))
             {
                 setFollowedLists = new HashSet<Guid>();
                 dico_idaccount_idFollowedLists.Add(accountId, setFollowedLists);
             }
             setFollowedLists.Add(listId);
         }

         public void Unfollow(Guid listId, Guid accountId)
         {
             throw new NotImplementedException();
         }


         public HashSet<Guid> GetAccounts(Guid listId)
         {
             HashSet<Guid> res;
             dico_id_idaccounts.TryGetValue(listId, out res);
             return res;
         }

         public void Add(Guid listId, Guid accountId)
         {
             HashSet<Guid> setAccounts;
             if (!dico_id_idaccounts.TryGetValue(listId, out setAccounts))
             {
                 setAccounts = new HashSet<Guid>();
                 dico_id_idaccounts.Add(listId, setAccounts);
             }
             setAccounts.Add(accountId);
         }

         public void Remove(Guid listId, Guid accountId)
         {
             throw new NotImplementedException();
         }

         public HashSet<Guid> GetAccountOwnedLists(Guid accountId, bool withPrivate)
         {
             throw new NotImplementedException();
         }

         public HashSet<Guid> GetAccountFollowedLists(Guid accountId, bool withPrivate)
         {
             throw new NotImplementedException();
         }

         public HashSet<Guid> GetFollowingLists(Guid accountId)
         {
             throw new NotImplementedException();
         }

         public HashSet<Guid> GetFollowingAccounts(Guid listId)
         {
             throw new NotImplementedException();
         }

         public ListStorageTmp(StorageTmp storage)
         {
            motherStorage = storage;
            dico_id_info = new Dictionary<Guid, IListInfo>();
            dico_id_idaccounts = new Dictionary<Guid, HashSet<Guid>>();
            dico_id_idowner = new Dictionary<Guid, Guid>();
            dico_idaccount_idperslist = new Dictionary<Guid, Guid>();
            dico_idaccount_idownedlists = new Dictionary<Guid, HashSet<Guid>>();
            dico_id_idfollowers = new Dictionary<Guid, HashSet<Guid>>();
            dico_idaccount_idFollowedLists = new Dictionary<Guid,
HashSet<Guid>>();
         }

         public void afficheDebug()
         {
             Console.WriteLine("Affichage des listes");
             foreach (KeyValuePair<Guid, HashSet<Guid>> kvp in dico_id_idaccounts)
             {
                 Console.Write("  ");
                 Console.WriteLine((GetInfo(kvp.Key)).Name);
                 foreach (Guid idAccount in kvp.Value)
                 {
                     Console.Write("     ");
                     Console.WriteLine((motherStorage.Account.GetInfo(idAccount)).Name);
                 }
             }
             Console.WriteLine("");

         }

     }

    public class MsgStorageTmp : IMsgStorage
    {
        private int nextFreeMsgId = 0;
        private StorageTmp motherStorage;
        private SortedDictionary<Guid,List<Guid>> dico_idaccount_listIdMess;
        private SortedDictionary<Guid, Message> dico_idMess_mess;

        public List<IMessage> GetListsMsgFrom(HashSet<Guid> listsId, DateTime firstMsgDate, int msgNumber)
        {
            List<IMessage> listMess= new List<IMessage>();
            int nbMess = 0;
            for(int idMess = 0; idMess < nextFreeMsgId && nbMess < msgNumber; idMess++)
            {
                Message curMess;
                dico_idMess_mess.TryGetValue(new Guid(idMess, 0, 0, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }), out curMess);
                bool trouve = false;
                foreach(Guid idList in listsId)
                {
                    if((motherStorage.List.GetAccounts(idList)).Contains(curMess.PosterId))
                        trouve = true;
                }
                if (trouve && curMess.Date > firstMsgDate )
                {
                   listMess.Add(curMess);
                   nbMess++;
                }
            }
            return listMess;
        }

        public List<IMessage> GetListsMsgTo(HashSet<Guid> listsId, DateTime lastMsgDate, int msgNumber)
        {
            List<IMessage> listMess= new List<IMessage>();
            int nbMess = 0;
            for(int idMess = nextFreeMsgId - 1; idMess >= 0  && nbMess < msgNumber; idMess--)
            {
                Message curMess;
                dico_idMess_mess.TryGetValue(new Guid(idMess, 0, 0, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }), out curMess);
                bool trouve = false;
                foreach(Guid idList in listsId)
                {
                    if((motherStorage.List.GetAccounts(idList)).Contains(curMess.PosterId))
                       trouve = true;
                }
                if (trouve && curMess.Date < lastMsgDate )
                {
                   listMess.Add(curMess);
                   nbMess++;
                }
            }
            return listMess;
        }

        public void Tag(Guid accountId, Guid msgId)
        {
            throw new NotImplementedException();
        }

        public void Untag(Guid accoundId, Guid msgId)
        {
            throw new NotImplementedException();
        }

        public List<IMessage> GetTaggedFrom(Guid accoundId, DateTime firstMsgDate, int msgNumber)
        {
            throw new NotImplementedException();
        }

        public List<IMessage> GetTaggedTo(Guid accountId, DateTime lastMsgDate, int msgNumber)
        {
            throw new NotImplementedException();
        }

        public Guid Post(Guid accountId, string content)
        {
            if (!dico_idaccount_listIdMess.ContainsKey(accountId))
                dico_idaccount_listIdMess.Add(accountId, new List<Guid>());

            List<Guid> list_mess;
            dico_idaccount_listIdMess.TryGetValue(accountId, out list_mess);
            list_mess.Add(new Guid(nextFreeMsgId, 0, 0, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }));

            Message curMessage = new Message(new Guid(nextFreeMsgId, 0, 0, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }), accountId, new DateTime(0), content);
            dico_idMess_mess.Add(new Guid(nextFreeMsgId, 0, 0, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }), curMessage);

            nextFreeMsgId++;
            return new Guid(nextFreeMsgId - 1, 0, 0, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 });
        }


        public Guid Copy(Guid accountId, Guid msgId)
        {
            Message curMess;
            dico_idMess_mess.TryGetValue(msgId, out curMess);
            return Post(accountId, curMess.Content);
        }

        public void Remove(Guid id)
        {
            throw new NotImplementedException();
        }

        public MsgStorageTmp(StorageTmp storage)
        {
            dico_idaccount_listIdMess = new SortedDictionary<Guid,List<Guid>>();
            dico_idMess_mess = new SortedDictionary<Guid, Message>();

            motherStorage = storage;
        }

        public void afficheDebug()
        {
            Console.WriteLine("Affichage des messages");
            foreach (KeyValuePair<Guid, List<Guid>> kvp in dico_idaccount_listIdMess)
            {
                Console.Write("  ");
                Console.WriteLine((motherStorage.Account.GetInfo(kvp.Key)).Name);
                foreach (Guid idMessage in kvp.Value)
                {
                    Console.Write("     ");
                    Message mesCur;
                    dico_idMess_mess.TryGetValue(idMessage, out mesCur);
                    Console.WriteLine(mesCur.Content);
                }
            }
            Console.WriteLine("");
        }

    }

    public class StorageTmp : IStorage
    {
        public UserStorageTmp user;
        public AccountStorageTmp account;
        public ListStorageTmp list;
        public MsgStorageTmp msg;

        public IUserStorage User { get { return user; } }
        public IAccountStorage Account { get { return account; } }
        public IListStorage List { get { return list; } }
        public IMsgStorage Msg { get { return msg; } }

        public void InitWithStupidData()
        {
            User.Create("Ismael", "isma.proj@gmail.com");
            Account.Create(User.GetId("Ismael"), "La vie d'Isma", "Suivez un mec cool ...");
            Account.Create(User.GetId("Ismael"), "Algorea", "Suivez les news d'Algorea ...");
            Account.Add(Account.GetId("La vie d'Isma"), User.GetId("Ismael"));
            Account.Add(Account.GetId("Algorea"), User.GetId("Ismael"));

            User.Create("Ulysse", "ulysse.beaugnon@ens.fr");
            Account.Create(User.GetId("Ulysse"), "Ulysse a la plage", "");
            Account.Create(User.GetId("Ulysse"), "Ulysse a la montagne", "");
            Account.Add(Account.GetId("Ulysse a la plage"), User.GetId("Ulysse"));
            Account.Add(Account.GetId("Ulysse a la montagne"), User.GetId("Ulysse"));

            User.Create("Basile", "basile@ens.fr");
            Account.Create(User.GetId("Basile"), "Basile le surfeur", "");
            Account.Add(Account.GetId("Basile le surfeur"), User.GetId("Basile"));

            Account.Add(Account.GetId("La vie d'Isma"), User.GetId("Ulysse"));
            Account.Add(Account.GetId("La vie d'Isma"), User.GetId("Basile"));

            Guid idListIsmaFriends = List.Create(Account.GetId("Ismael"), "Les amis d'Ismael", "Les amis d'Isma sont cools...", false);
            List.Add(idListIsmaFriends, Account.GetId("La vie d'Isma"));
            List.Add(idListIsmaFriends, Account.GetId("Ulysse a la plage"));
            List.Add(idListIsmaFriends, Account.GetId("Basile le surfeur"));

            Msg.Post(Account.GetId("La vie d'Isma"), "Je suis alle a un concert");
            Msg.Post(Account.GetId("La vie d'Isma"), "J'ai mange une pomme");
            Msg.Post(Account.GetId("La vie d'Isma"), "Ce soir c'est ravioli");

            Msg.Post(Account.GetId("Ulysse a la plage"), "Je bronze");
            Msg.Post(Account.GetId("Ulysse a la plage"), "La mer est magnifique");
            Msg.Post(Account.GetId("Ulysse a la plage"), "J'ai fait un super chateau de sable");

            List.GetPersonalList(Account.GetId("La vie d'Isma"));
            List.GetPersonalList(Account.GetId("Algorea"));
            List.GetPersonalList(Account.GetId("Ulysse a la montagne"));
            List.GetPersonalList(Account.GetId("Basile le surfeur"));
        }

        public StorageTmp()
        {
            user = new UserStorageTmp(this);
            account = new AccountStorageTmp(this);
            list = new ListStorageTmp(this);
            msg = new MsgStorageTmp(this);
        }

        public void afficheDebug()
        {
            user.afficheDebug();
            account.afficheDebug();
            list.afficheDebug();
            msg.afficheDebug();
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            StorageTmp DB = new StorageTmp();
            DB.InitWithStupidData();
            DB.afficheDebug();

            Console.ReadLine();
        }
    }
}
