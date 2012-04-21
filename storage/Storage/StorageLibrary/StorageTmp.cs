using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageLibrary
{

    public class UserStorageTmp : IUserStorage
    {
        internal class FullUserInfo
        {
            public Guid Id { get; set; }

            public IUserInfo UserInfo { get; set; }

            public HashSet<Guid> Accounts { get; set; } 
        }

        private readonly Dictionary<Guid, FullUserInfo> infoFromId;

        private readonly Dictionary<string, Guid> idFromLogin;

        private readonly StorageTmp storage;

        public Guid GetId(string login)
        {
            Guid id;
            if (this.idFromLogin.TryGetValue(login, out id))
            {
                return id;
            }

            throw new UserNotFound();
        }

        public IUserInfo GetInfo(Guid userId)
        {
            return this.GetFullInfo(userId).UserInfo;
        }

        public void SetInfo(Guid userId, string email)
        {
            var userInfo = this.GetFullInfo(userId);
            userInfo.UserInfo.Email = email;
        }

        public HashSet<Guid> GetAccounts(Guid userId)
        {
            throw new NotImplementedException();
            // return this.motherStorage.account.GetAccounts(userId);
        }

        public Guid Create(string login, string email)
        {
            if (this.idFromLogin.ContainsKey(login))
            {
                throw new UserAlreadyExists();
            }

            var id = Guid.NewGuid();
            var userInfo = new FullUserInfo
                {
                    Id = id,
                    UserInfo = new UserInfo(login: login, email: email, avatar: string.Empty),
                    Accounts = new HashSet<Guid>()
                };
            this.idFromLogin.Add(login, id);
            this.infoFromId.Add(id, userInfo);
            this.storage.Account.Create(id, login, "Main account");

            return id;
        }

        public void Delete(Guid user_id)
        {
            throw new NotImplementedException();
        }

        public UserStorageTmp(StorageTmp storage)
        {
            this.storage = storage;
            this.idFromLogin = new Dictionary<string, Guid>();
            this.infoFromId = new Dictionary<Guid, FullUserInfo>();
        }

        public void afficheDebug()
        {
            Console.WriteLine("Printing users...");
            foreach (var userInfo in this.infoFromId)
            {
                Console.Write("    ");
                Console.Write(userInfo.Key);
                Console.Write(" : ");
                Console.WriteLine(userInfo.Value.UserInfo.Login);
            }
            Console.WriteLine("Users printed.");
        }

        internal FullUserInfo GetFullInfo(Guid id)
        {
            FullUserInfo userInfo;
            if (this.infoFromId.TryGetValue(id, out userInfo))
            {
                return userInfo;
            }

            throw new UserNotFound();
        }
    }

    public class AccountStorageTmp : IAccountStorage
    {
        internal class FullAccountInfo
        {
            public Guid Id { get; set; }

            public IAccountInfo AccountInfo { get; set; }

            public HashSet<Guid> Users { get; set; }

            public Guid AdminId { get; set; }

            public Guid PersonalListId { get; set; }

            public HashSet<Guid> MemberOfLists { get; set; }

            public HashSet<Guid> FollowerOfLists { get; set; } 
        }

        private readonly Dictionary<Guid, FullAccountInfo> infoFromId;

        private readonly Dictionary<string, Guid> idFromName;

        private readonly StorageTmp motherStorage;

        public Guid GetId(string name)
        {
            Guid id;
            if (this.idFromName.TryGetValue(name, out id))
            {
                return id;
            }

            throw new AccountNotFound();
        }

        public IAccountInfo GetInfo(Guid accountId)
        {
            return this.GetFullInfo(accountId).AccountInfo;
        }

        public void SetInfo(Guid Id, string description)
        {
            var accountInfo = this.GetFullInfo(Id).AccountInfo;
            accountInfo.Description = description;
        }

        public HashSet<Guid> GetUsers(Guid accountId)
        {
            return this.GetFullInfo(accountId).Users;
        }

        public HashSet<Guid> GetAccounts(Guid userId)
        {
            return this.motherStorage.User.GetAccounts(userId);
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
            var accountInfo = this.GetFullInfo(accountId);
            var userInfo = this.motherStorage.user.GetFullInfo(userId);

            accountInfo.Users.Add(userId);
            userInfo.Accounts.Add(accountId);
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
            var id = Guid.NewGuid();
            var adminInfos = this.motherStorage.user.GetFullInfo(adminId);
            var accountInfo = new FullAccountInfo
                {
                    Id = id,
                    AccountInfo = new AccountInfo(name, description),
                    Users = new HashSet<Guid> { adminId },
                    AdminId = adminId,
                    FollowerOfLists = new HashSet<Guid>(),
                    MemberOfLists = new HashSet<Guid>()
                };

            this.infoFromId.Add(id, accountInfo);
            this.idFromName.Add(name, id);
            adminInfos.Accounts.Add(id);
            accountInfo.PersonalListId = this.motherStorage.list.CreateList(
                id, "personal list", "personal list", false, true);

            return id;
        }

        public void Delete(Guid accountId)
        {
            throw new NotImplementedException();
        }

        public AccountStorageTmp(StorageTmp storage)
        {
            this.motherStorage = storage;
            this.infoFromId = new Dictionary<Guid, FullAccountInfo>();
            this.idFromName = new Dictionary<string, Guid>();
        }

        public void afficheDebug()
        {
            Console.WriteLine("Printing accounts...");
            foreach (var item in this.infoFromId)
            {
                Console.Write("  ");
                Console.WriteLine(item.Value.AccountInfo.Name);
                foreach (var userId in item.Value.Users)
                {
                    Console.Write("    ");
                    Console.WriteLine(this.motherStorage.User.GetInfo(userId).Login);
                }
            }

            Console.WriteLine("Accounts printed.");
        }

        internal FullAccountInfo GetFullInfo(Guid accountId)
        {
            FullAccountInfo accountInfo;
            if (this.infoFromId.TryGetValue(accountId, out accountInfo))
            {
                return accountInfo;
            }

            throw new AccountNotFound();
        }
    }

    public class ListStorageTmp : IListStorage
    {
        internal class FullListInfo
        {
            public List<IMessage> Messages { get; set; }

            public Guid Id { get; set; }

            public IListInfo ListInfo { get; set; }

            public Guid OwnerId { get; set; }

            public HashSet<Guid> Members { get; set; }

            public HashSet<Guid> Followers { get; set; }
        }

        private readonly Dictionary<Guid, FullListInfo> infoFromId; 

         private readonly StorageTmp motherStorage;

         public IListInfo GetInfo(Guid listId)
         {
             return this.GetFullInfo(listId).ListInfo;
         }

         public Guid GetOwner(Guid listId)
         {
             return this.GetFullInfo(listId).OwnerId;
         }

         public Guid GetPersonalList(Guid accountId)
         {
             return this.motherStorage.account.GetFullInfo(accountId).PersonalListId;
         }

         public void SetInfo(Guid listId, string name, string description, bool isPrivate)
         {
             var listInfo = this.GetFullInfo(listId).ListInfo;

             if (listInfo.IsPersonnal)
             {
                 throw new IsPersonnalList();
             }

             listInfo.Name = name;
             listInfo.Description = description;
             listInfo.IsPrivate = isPrivate;
         }

         public Guid Create(Guid ownerId, string name, string description, bool isPrivate)
         {
             return this.CreateList(ownerId, name, description, isPrivate, false);
         }

        internal Guid CreateList(Guid ownerId, string name, string description, bool isPrivate, bool isPersonal)
        {
            var id = Guid.NewGuid();
            var accountInfo = this.motherStorage.account.GetFullInfo(ownerId);
            var listInfo = new FullListInfo
                 {
                     Id = id,
                     Followers = new HashSet<Guid> { ownerId },
                     ListInfo = new ListInfo(name, description, isPrivate, isPersonal),
                     Members = new HashSet<Guid> { ownerId },
                     OwnerId = ownerId
                 };

            accountInfo.MemberOfLists.Add(id);
            accountInfo.FollowerOfLists.Add(id);
            this.infoFromId.Add(id, listInfo);

            return id;
         }

         public void Delete(Guid ownerId)
         {
             throw new NotImplementedException();
         }

         public void Follow(Guid listId, Guid accountId)
         {
             var listInfo = this.GetFullInfo(listId);
             var accountInfo = this.motherStorage.account.GetFullInfo(accountId);

             listInfo.Followers.Add(accountId);
             accountInfo.FollowerOfLists.Add(listId);
         }

         public void Unfollow(Guid listId, Guid accountId)
         {
             var listInfo = this.GetFullInfo(listId);
             var accountInfo = this.motherStorage.account.GetFullInfo(accountId);

             listInfo.Followers.Remove(accountId);
             accountInfo.FollowerOfLists.Remove(listId);
         }


         public HashSet<Guid> GetAccounts(Guid listId)
         {
             return this.GetFullInfo(listId).Members;
         }

         public void Add(Guid listId, Guid accountId)
         {
             var listInfo = this.GetFullInfo(listId);
             var accountInfo = this.motherStorage.account.GetFullInfo(accountId);

             listInfo.Members.Add(accountId);
             accountInfo.MemberOfLists.Add(listId);
         }

         public void Remove(Guid listId, Guid accountId)
         {
             try
             {
                 this.GetFullInfo(listId).Members.Remove(accountId);
             }
             catch (ListNotFound)
             {
             }

             try
             {
                 this.motherStorage.account.GetFullInfo(accountId).MemberOfLists.Remove(listId);
             }
             catch (AccountNotFound)
             {
             }
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
             this.motherStorage = storage;
             this.infoFromId = new Dictionary<Guid, FullListInfo>();
         }

         public void afficheDebug()
         {
             Console.WriteLine("Printing lists...");
             foreach (var item in this.infoFromId)
             {
                 Console.Write("  ");
                 Console.WriteLine(item.Value.ListInfo.Name);
                 foreach (var accountId in item.Value.Members)
                 {
                     Console.Write("    ");
                     Console.WriteLine(this.motherStorage.Account.GetInfo(accountId).Name);
                 }
             }

             Console.WriteLine("Printed lists.");
         }

        internal FullListInfo GetFullInfo(Guid id)
        {
            FullListInfo listInfo;
            if (this.infoFromId.TryGetValue(id, out listInfo))
            {
                return listInfo;
            }

            throw new ListNotFound();
        }
     }

    public class MsgStorageTmp : IMsgStorage
    {
        internal class FullMessageInfo
        {
            public Guid Id { get; set; }

            public IMessage MessageInfo { get; set; }
        }

        private readonly SortedDictionary<Guid, FullMessageInfo> infoFromId;
  
        private StorageTmp motherStorage;
        private SortedDictionary<Guid,List<Guid>> dico_idaccount_listIdMess;
        private SortedDictionary<Guid, Message> dico_idMess_mess;

        public List<IMessage> GetListsMsgFrom(HashSet<Guid> listsId, DateTime firstMsgDate, int msgNumber)
        {
            var messages = new List<IMessage>();
            foreach (var listId in listsId)
            {
                messages.AddRange(
                    this.motherStorage.list.GetFullInfo(listId).Messages.Where(msg => msg.Date > firstMsgDate));
            }

            messages.Sort((msg1, msg2) => DateTime.Compare(msg1.Date, msg2.Date));

            return messages.Take(msgNumber).ToList();
        }

        public List<IMessage> GetListsMsgTo(HashSet<Guid> listsId, DateTime lastMsgDate, int msgNumber)
        {
            var messages = new List<IMessage>();
            foreach (var listId in listsId)
            {
                messages.AddRange(
                    this.motherStorage.list.GetFullInfo(listId).Messages.Where(msg => msg.Date < lastMsgDate));
            }

            // Note the reversing of the parameters because we supposedly want the *lasts* messages first
            messages.Sort((msg1, msg2) => DateTime.Compare(msg2.Date, msg1.Date));

            return messages.Take(msgNumber).ToList();
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
            var id = Guid.NewGuid();
            var accountInfo = this.motherStorage.account.GetFullInfo(accountId);
            var message = new Message(
                id, accountId, accountInfo.AccountInfo.Name, string.Empty, DateTime.Now, content);

            foreach (var listId in accountInfo.MemberOfLists)
            {
                var messages = this.motherStorage.list.GetFullInfo(listId).Messages;
                messages.Add(message);
            }

            return id;
        }


        public Guid Copy(Guid accountId, Guid msgId)
        {
            // TODO: THATS NOT HOW RETWEET MUST WORK. DEFINITELY NOT.
            var message = this.GetFullInfo(msgId);
            var account = this.motherStorage.account.GetFullInfo(accountId);
            return this.Post(accountId, message.MessageInfo.Content);
        }

        public void Remove(Guid id)
        {
            throw new NotImplementedException();
        }

        public MsgStorageTmp(StorageTmp storage)
        {
            this.motherStorage = storage;
            this.infoFromId = new SortedDictionary<Guid, FullMessageInfo>();
        }

        public void afficheDebug()
        {
            Console.WriteLine("Printing messages...");
            foreach (var item in this.infoFromId)
            {
                Console.Write("  ");
                Console.Write(this.motherStorage.Account.GetInfo(item.Value.MessageInfo.PosterId).Name);
                Console.Write(" : ");
                Console.Write(item.Value.MessageInfo.Content);
            }
            Console.WriteLine("Messages printed.");
        }

        internal FullMessageInfo GetFullInfo(Guid messageId)
        {
            FullMessageInfo messageInfo;
            if (this.infoFromId.TryGetValue(messageId, out messageInfo))
            {
                return messageInfo;
            }

            throw new MessageNotFound();
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
            var db = new StorageTmp();
            db.InitWithStupidData();
            db.afficheDebug();

            Console.ReadLine();
        }
    }
}
