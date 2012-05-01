namespace StorageLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class UserStorageTmp : IUserStorage
    {
        #region Constants and Fields

        private readonly Dictionary<string, Guid> idFromLogin;

        private readonly Dictionary<Guid, FullUserInfo> infoFromId;

        private readonly StorageTmp storage;

        #endregion

        #region Constructors and Destructors

        public UserStorageTmp(StorageTmp storage)
        {
            this.storage = storage;
            this.idFromLogin = new Dictionary<string, Guid>();
            this.infoFromId = new Dictionary<Guid, FullUserInfo>();
        }

        #endregion

        #region Public Methods and Operators

        public Guid Create(string login, string email, string password)
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

            return id;
        }

        public void Delete(Guid user_id)
        {
            throw new NotImplementedException();
        }

        public HashSet<Guid> GetAccounts(Guid userId)
        {
            return this.GetFullInfo(userId).Accounts;
        }

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

        public string GetPassword(Guid userID)
        {
            throw new NotImplementedException();
        }

        public void SetInfo(Guid userId, string email)
        {
            var userInfo = this.GetFullInfo(userId);
            userInfo.UserInfo.Email = email;
        }

        public void SetPassword(Guid userID, string password)
        {
            throw new NotImplementedException();
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

        #endregion

        #region Methods

        internal FullUserInfo GetFullInfo(Guid id)
        {
            FullUserInfo userInfo;
            if (this.infoFromId.TryGetValue(id, out userInfo))
            {
                return userInfo;
            }

            throw new UserNotFound();
        }

        #endregion

        internal class FullUserInfo
        {
            #region Public Properties

            public HashSet<Guid> Accounts { get; set; }

            public Guid Id { get; set; }

            public IUserInfo UserInfo { get; set; }

            #endregion
        }
    }

    public class AccountStorageTmp : IAccountStorage
    {
        #region Constants and Fields

        private readonly Dictionary<string, Guid> idFromName;

        private readonly Dictionary<Guid, FullAccountInfo> infoFromId;

        private readonly StorageTmp motherStorage;

        #endregion

        #region Constructors and Destructors

        public AccountStorageTmp(StorageTmp storage)
        {
            this.motherStorage = storage;
            this.infoFromId = new Dictionary<Guid, FullAccountInfo>();
            this.idFromName = new Dictionary<string, Guid>();
        }

        #endregion

        #region Public Methods and Operators

        public void Add(Guid accountId, Guid userId)
        {
            var accountInfo = this.GetFullInfo(accountId);
            var userInfo = this.motherStorage.user.GetFullInfo(userId);

            accountInfo.Users.Add(userId);
            userInfo.Accounts.Add(accountId);
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

            if (this.idFromName.ContainsKey(name))
            {
                throw new AccountAlreadyExists();
            }

            this.infoFromId.Add(id, accountInfo);
            this.idFromName.Add(name, id);
            adminInfos.Accounts.Add(id);
            accountInfo.PersonalListId = this.motherStorage.list.CreateList(
                id, "personal list", "personal list", false, true);

            return id;
        }

        public void Delete(Guid accountId)
        {
            try
            {
                var accountInfo = this.GetFullInfo(accountId);

                // Remove lists
                // Warning: might throw :-(
                foreach (var list in accountInfo.MemberOfLists.Select(id => this.motherStorage.list.GetFullInfo(id)))
                {
                    if (list.OwnerId == accountId)
                    {
                        this.motherStorage.List.Delete(list.Id);
                    }
                    else
                    {
                        this.motherStorage.List.Remove(list.Id, accountId);
                    }
                }

                foreach (var list in accountInfo.FollowerOfLists)
                {
                    this.motherStorage.List.Unfollow(list, accountId);
                }

                var userInfo = this.motherStorage.user.GetFullInfo(accountInfo.AdminId);
                userInfo.Accounts.Remove(accountId);

                // Last for being able to GetFullInfo() from other functions while deleting
                this.infoFromId.Remove(accountId);
                this.idFromName.Remove(accountInfo.AccountInfo.Name);
            }
            catch (StorageLibException)
            {
                // Must not fail
            }
        }

        public HashSet<Guid> GetAccounts(Guid userId)
        {
            return this.motherStorage.User.GetAccounts(userId);
        }

        public Guid GetAdminId(Guid accountId)
        {
            return this.GetFullInfo(accountId).AdminId;
        }

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

        public HashSet<Guid> GetUsers(Guid accountId)
        {
            return this.GetFullInfo(accountId).Users;
        }

        public void Remove(Guid userId, Guid accountId)
        {
            throw new NotImplementedException();
        }

        public void SetAdminId(Guid accountId, Guid userId)
        {
            var accountInfo = this.GetFullInfo(accountId);
            var userInfo = this.motherStorage.user.GetFullInfo(userId);

            accountInfo.AdminId = accountId;
            this.Add(accountId, userId);
        }

        public void SetInfo(Guid Id, string description)
        {
            var accountInfo = this.GetFullInfo(Id).AccountInfo;
            accountInfo.Description = description;
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

        #endregion

        #region Methods

        internal FullAccountInfo GetFullInfo(Guid accountId)
        {
            FullAccountInfo accountInfo;
            if (this.infoFromId.TryGetValue(accountId, out accountInfo))
            {
                return accountInfo;
            }

            throw new AccountNotFound();
        }

        #endregion

        internal class FullAccountInfo
        {
            #region Public Properties

            public IAccountInfo AccountInfo { get; set; }

            public Guid AdminId { get; set; }

            public HashSet<Guid> FollowerOfLists { get; set; }

            public Guid Id { get; set; }

            public HashSet<Guid> MemberOfLists { get; set; }

            public Guid PersonalListId { get; set; }

            public HashSet<Guid> Users { get; set; }

            #endregion
        }
    }

    public class ListStorageTmp : IListStorage
    {
        #region Constants and Fields

        private readonly Dictionary<Guid, FullListInfo> infoFromId;

        private readonly StorageTmp motherStorage;

        #endregion

        #region Constructors and Destructors

        public ListStorageTmp(StorageTmp storage)
        {
            this.motherStorage = storage;
            this.infoFromId = new Dictionary<Guid, FullListInfo>();
        }

        #endregion

        #region Public Methods and Operators

        public void Add(Guid listId, Guid accountId)
        {
            FullListInfo listInfo = this.GetFullInfo(listId);
            AccountStorageTmp.FullAccountInfo accountInfo = this.motherStorage.account.GetFullInfo(accountId);

            listInfo.Members.Add(accountId);
            accountInfo.MemberOfLists.Add(listId);
        }

        public Guid Create(Guid ownerId, string name, string description, bool isPrivate)
        {
            return this.CreateList(ownerId, name, description, isPrivate, false);
        }

        public void Delete(Guid ownerId)
        {
            throw new NotImplementedException();
        }

        public void Follow(Guid listId, Guid accountId)
        {
            FullListInfo listInfo = this.GetFullInfo(listId);
            AccountStorageTmp.FullAccountInfo accountInfo = this.motherStorage.account.GetFullInfo(accountId);

            listInfo.Followers.Add(accountId);
            accountInfo.FollowerOfLists.Add(listId);
        }

        public HashSet<Guid> GetAccountFollowedLists(Guid accountId, bool withPrivate)
        {
            throw new NotImplementedException();
        }

        public HashSet<Guid> GetAccountOwnedLists(Guid accountId, bool withPrivate)
        {
            throw new NotImplementedException();
        }

        public HashSet<Guid> GetAccounts(Guid listId)
        {
            return this.GetFullInfo(listId).Members;
        }

        public HashSet<Guid> GetFollowingAccounts(Guid listId)
        {
            throw new NotImplementedException();
        }

        public HashSet<Guid> GetFollowingLists(Guid accountId)
        {
            throw new NotImplementedException();
        }

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

        public void SetInfo(Guid listId, string name, string description, bool isPrivate)
        {
            IListInfo listInfo = this.GetFullInfo(listId).ListInfo;

            if (listInfo.IsPersonnal)
            {
                throw new IsPersonnalList();
            }

            listInfo.Name = name;
            listInfo.Description = description;
            listInfo.IsPrivate = isPrivate;
        }

        public void Unfollow(Guid listId, Guid accountId)
        {
            FullListInfo listInfo = this.GetFullInfo(listId);
            AccountStorageTmp.FullAccountInfo accountInfo = this.motherStorage.account.GetFullInfo(accountId);

            listInfo.Followers.Remove(accountId);
            accountInfo.FollowerOfLists.Remove(listId);
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

        #endregion

        #region Methods

        internal Guid CreateList(Guid ownerId, string name, string description, bool isPrivate, bool isPersonal)
        {
            Guid id = Guid.NewGuid();
            AccountStorageTmp.FullAccountInfo accountInfo = this.motherStorage.account.GetFullInfo(ownerId);
            var listInfo = new FullListInfo
                {
                    Id = id, 
                    Followers = new HashSet<Guid> { ownerId }, 
                    ListInfo = new ListInfo(name, description, isPrivate, isPersonal), 
                    Members = new HashSet<Guid> { ownerId },
                    Messages = new List<IMessage>(),
                    OwnerId = ownerId
                };

            accountInfo.MemberOfLists.Add(id);
            accountInfo.FollowerOfLists.Add(id);
            this.infoFromId.Add(id, listInfo);

            return id;
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

        #endregion

        internal class FullListInfo
        {
            #region Public Properties

            public HashSet<Guid> Followers { get; set; }

            public Guid Id { get; set; }

            public IListInfo ListInfo { get; set; }

            public HashSet<Guid> Members { get; set; }

            public List<IMessage> Messages { get; set; }

            public Guid OwnerId { get; set; }

            #endregion
        }
    }

    public class MsgStorageTmp : IMsgStorage
    {
        #region Constants and Fields

        private readonly SortedDictionary<Guid, FullMessageInfo> infoFromId;

        private readonly StorageTmp motherStorage;

        #endregion

        #region Constructors and Destructors

        public MsgStorageTmp(StorageTmp storage)
        {
            this.motherStorage = storage;
            this.infoFromId = new SortedDictionary<Guid, FullMessageInfo>();
        }

        #endregion

        #region Public Methods and Operators

        public Guid Copy(Guid accountId, Guid msgId)
        {
            // TODO: THATS NOT HOW RETWEET MUST WORK. DEFINITELY NOT.
            FullMessageInfo message = this.GetFullInfo(msgId);
            AccountStorageTmp.FullAccountInfo account = this.motherStorage.account.GetFullInfo(accountId);
            return this.Post(accountId, message.MessageInfo.Content);
        }

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
            Guid id = Guid.NewGuid();
            AccountStorageTmp.FullAccountInfo accountInfo = this.motherStorage.account.GetFullInfo(accountId);
            var message = new Message(id, accountId, accountInfo.AccountInfo.Name, string.Empty, DateTime.Now, content);

            foreach (var listId in accountInfo.MemberOfLists)
            {
                List<IMessage> messages = this.motherStorage.list.GetFullInfo(listId).Messages;
                messages.Add(message);
            }

            return id;
        }

        public void Remove(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Tag(Guid accountId, Guid msgId)
        {
            throw new NotImplementedException();
        }

        public void Untag(Guid accoundId, Guid msgId)
        {
            throw new NotImplementedException();
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

        #endregion

        #region Methods

        internal FullMessageInfo GetFullInfo(Guid messageId)
        {
            FullMessageInfo messageInfo;
            if (this.infoFromId.TryGetValue(messageId, out messageInfo))
            {
                return messageInfo;
            }

            throw new MessageNotFound();
        }

        #endregion

        internal class FullMessageInfo
        {
            #region Public Properties

            public Guid Id { get; set; }

            public IMessage MessageInfo { get; set; }

            #endregion
        }
    }

    public class StorageTmp : IStorage
    {
        #region Constants and Fields

        public AccountStorageTmp account;

        public ListStorageTmp list;

        public MsgStorageTmp msg;

        public UserStorageTmp user;

        #endregion

        #region Constructors and Destructors

        public StorageTmp()
        {
            this.user = new UserStorageTmp(this);
            this.account = new AccountStorageTmp(this);
            this.list = new ListStorageTmp(this);
            this.msg = new MsgStorageTmp(this);
        }

        #endregion

        #region Public Properties

        public IAccountStorage Account
        {
            get
            {
                return this.account;
            }
        }

        public IListStorage List
        {
            get
            {
                return this.list;
            }
        }

        public IMsgStorage Msg
        {
            get
            {
                return this.msg;
            }
        }

        public IUserStorage User
        {
            get
            {
                return this.user;
            }
        }

        #endregion

        #region Public Methods and Operators

        public void InitWithStupidData()
        {
            this.User.Create("Ismael", "isma.proj@gmail.com", string.Empty);
            this.Account.Create(this.User.GetId("Ismael"), "La vie d'Isma", "Suivez un mec cool ...");
            this.Account.Create(this.User.GetId("Ismael"), "Algorea", "Suivez les news d'Algorea ...");
            this.Account.Add(this.Account.GetId("La vie d'Isma"), this.User.GetId("Ismael"));
            this.Account.Add(this.Account.GetId("Algorea"), this.User.GetId("Ismael"));

            this.User.Create("Ulysse", "ulysse.beaugnon@ens.fr", string.Empty);
            this.Account.Create(this.User.GetId("Ulysse"), "Ulysse a la plage", string.Empty);
            this.Account.Create(this.User.GetId("Ulysse"), "Ulysse a la montagne", string.Empty);
            this.Account.Add(this.Account.GetId("Ulysse a la plage"), this.User.GetId("Ulysse"));
            this.Account.Add(this.Account.GetId("Ulysse a la montagne"), this.User.GetId("Ulysse"));

            this.User.Create("Basile", "basile@ens.fr", string.Empty);
            this.Account.Create(this.User.GetId("Basile"), "Basile le surfeur", string.Empty);
            this.Account.Add(this.Account.GetId("Basile le surfeur"), this.User.GetId("Basile"));

            this.Account.Add(this.Account.GetId("La vie d'Isma"), this.User.GetId("Ulysse"));
            this.Account.Add(this.Account.GetId("La vie d'Isma"), this.User.GetId("Basile"));

            Guid idListIsmaFriends = this.List.Create(
                this.Account.GetId("Ismael"), "Les amis d'Ismael", "Les amis d'Isma sont cools...", false);
            this.List.Add(idListIsmaFriends, this.Account.GetId("La vie d'Isma"));
            this.List.Add(idListIsmaFriends, this.Account.GetId("Ulysse a la plage"));
            this.List.Add(idListIsmaFriends, this.Account.GetId("Basile le surfeur"));

            this.Msg.Post(this.Account.GetId("La vie d'Isma"), "Je suis alle a un concert");
            this.Msg.Post(this.Account.GetId("La vie d'Isma"), "J'ai mange une pomme");
            this.Msg.Post(this.Account.GetId("La vie d'Isma"), "Ce soir c'est ravioli");

            this.Msg.Post(this.Account.GetId("Ulysse a la plage"), "Je bronze");
            this.Msg.Post(this.Account.GetId("Ulysse a la plage"), "La mer est magnifique");
            this.Msg.Post(this.Account.GetId("Ulysse a la plage"), "J'ai fait un super chateau de sable");

            this.List.GetPersonalList(this.Account.GetId("La vie d'Isma"));
            this.List.GetPersonalList(this.Account.GetId("Algorea"));
            this.List.GetPersonalList(this.Account.GetId("Ulysse a la montagne"));
            this.List.GetPersonalList(this.Account.GetId("Basile le surfeur"));
        }

        public void afficheDebug()
        {
            this.user.afficheDebug();
            this.account.afficheDebug();
            this.list.afficheDebug();
            this.msg.afficheDebug();
        }

        #endregion
    }

    internal class Program
    {
        #region Methods

        private static void Main(string[] args)
        {
            var db = new StorageTmp();
            db.InitWithStupidData();
            db.afficheDebug();

            Console.ReadLine();
        }

        #endregion
    }
}