// -----------------------------------------------------------------------
// <copyright file="MockStorage.cs" company="ENS Paris">
// BSD
// </copyright>
// -----------------------------------------------------------------------

namespace Tigwi.UI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Tigwi.Storage.Library;

    /// <summary>
    /// A mock implementation of IStorage. Not tested, there can be bugs !
    /// </summary>
    public class MockStorage : IStorage
    {
        public MockStorage()
        {
            this.UserStorage = new MockUserStorage(this);
            this.AccountStorage = new MockAccountStorage(this);
            this.ListStorage = new MockListStorage(this);
            this.MsgStorage = new MockMsgStorage(this);
        }

        #region Implementation of IStorage

        public IUserStorage User
        {
            get
            {
                return this.UserStorage;
            }
        }

        public IAccountStorage Account
        {
            get
            {
                return this.AccountStorage;
            }
        }

        public IListStorage List
        {
            get
            {
                return this.ListStorage;
            }
        }

        public IMsgStorage Msg
        {
            get
            {
                return this.MsgStorage;
            }
        }

        #endregion

        protected MockUserStorage UserStorage { get; set; }

        protected MockAccountStorage AccountStorage { get; set; }

        protected MockListStorage ListStorage { get; set; }

        protected MockMsgStorage MsgStorage { get; set; }

        protected class MockUser : IUserInfo
        {
            public Guid Id { get; set; }

            #region Implementation of IUserInfo

            public string Login { get; set; }

            public string Avatar { get; set; }

            public string Email { get; set; }

            #endregion

            public ISet<Guid> Accounts { get; set; }

            public ISet<string> OpenIdUri { get; set; }

            public byte[] Password { get; set; }
        }

        protected class MockUserStorage : IUserStorage
        {
            public MockUserStorage(MockStorage storage)
            {
                this.Storage = storage;
                this.IdFromLogin = new Dictionary<string, Guid>();
                this.UserFromId = new Dictionary<Guid, MockUser>();
                this.IdFromOpenId = new Dictionary<string, Guid>();
            }

            protected IDictionary<string, Guid> IdFromLogin { get; set; }

            protected IDictionary<Guid, MockUser> UserFromId { get; set; }

            protected IDictionary<string, Guid> IdFromOpenId { get; set; }

            protected MockStorage Storage { get; set; }

            #region Implementation of IUserStorage

            public Guid GetId(string login)
            {
                Guid guid;

                if (!this.IdFromLogin.TryGetValue(login, out guid))
                {
                    throw new UserNotFound();
                }

                return guid;
            }

            public IUserInfo GetInfo(Guid userId)
            {
                return this.GetMock(userId);
            }

            public MockUser GetMock(Guid userId)
            {
                MockUser user;

                if (!this.UserFromId.TryGetValue(userId, out user))
                {
                    throw new UserNotFound();
                }

                return user;
            }

            public void SetInfo(Guid userId, string email)
            {
                this.GetMock(userId).Email = email;
            }

            public HashSet<Guid> GetAccounts(Guid userId)
            {
                return new HashSet<Guid>(this.GetMock(userId).Accounts);
            }

            public Guid Create(string login, string email, byte[] password)
            {
                if (this.IdFromLogin.ContainsKey(login))
                {
                    throw new UserAlreadyExists();
                }

                var id = Guid.NewGuid();
                this.IdFromLogin.Add(login, id);
                this.UserFromId.Add(
                    id,
                    new MockUser
                        {
                            Accounts = new HashSet<Guid>(),
                            Avatar = string.Empty,
                            Email = email,
                            Login = login,
                            Id = id,
                            Password = password,
                            OpenIdUri = new HashSet<string>()
                        });

                return id;
            }

            public void Delete(Guid userId)
            {
                MockUser user;
                if (!this.UserFromId.TryGetValue(userId, out user))
                {
                    return;
                }

                if (user.Accounts.Select(id => this.Storage.AccountStorage.GetAdminId(id)).Any(admin => admin == userId))
                {
                    throw new UserIsAdmin();
                }

                foreach (var account in user.Accounts.Select(this.Storage.AccountStorage.GetMock))
                {
                    account.Users.Remove(userId);
                }

                this.IdFromLogin.Remove(user.Login);
                this.UserFromId.Remove(userId);
                foreach (var oidUri in user.OpenIdUri)
                {
                    this.IdFromOpenId.Remove(oidUri);
                }
            }

            public Guid GetIdByOpenIdUri(string openIdUri)
            {
                Guid id;

                if (!this.IdFromOpenId.TryGetValue(openIdUri, out id))
                {
                    throw new UserNotFound();
                }

                return id;
            }

            public void AssociateOpenIdUri(Guid userId, string openIdUri)
            {
                if (this.IdFromOpenId.ContainsKey(openIdUri))
                {
                    throw new OpenIdUriDuplicated();
                }

                var user = this.GetMock(userId);
                user.OpenIdUri.Add(openIdUri);
                this.IdFromOpenId.Add(openIdUri, userId);
            }

            public HashSet<string> ListOpenIdUris(Guid userId)
            {
                return new HashSet<string>(this.GetMock(userId).OpenIdUri);
            }

            public void DeassociateOpenIdUri(Guid userId, string openIdUri)
            {
                Guid associated;

                this.GetMock(userId).OpenIdUri.Remove(openIdUri);

                if (!this.IdFromOpenId.TryGetValue(openIdUri, out associated))
                {
                    return;
                }

                if (associated != userId)
                {
                    throw new OpenIdUriNotAssociated();
                }

                this.IdFromOpenId.Remove(openIdUri);
            }

            public byte[] GetPassword(Guid userId)
            {
                return this.GetMock(userId).Password;
            }

            public void SetPassword(Guid userId, byte[] password)
            {
                this.GetMock(userId).Password = password;
            }

            #endregion
        }

        protected class MockAccount : IAccountInfo
        {
            #region Implementation of IAccountInfo

            public string Name { get; set; }

            public string Description { get; set; }

            #endregion

            public ISet<Guid> Users { get; set; }

            public Guid Admin { get; set; }

            public Guid PersonalList { get; set; }

            public ISet<Guid> AllFollowedLists { get; set; }

            public ISet<Guid> PublicFollowedLists { get; set; }

            public ISet<Guid> AllOwnedLists { get; set; }

            public ISet<Guid> PublicOwnedLists { get; set; }

            public ISet<Guid> MemberOfLists { get; set; }

            public ISet<Guid> Messages { get; set; }

            public ISet<Guid> TaggedMessages { get; set; }
        }

        protected class MockAccountStorage : IAccountStorage
        {
            public MockAccountStorage(MockStorage storage)
            {
                this.Storage = storage;
                this.IdFromName = new Dictionary<string, Guid>();
                this.AccountFromId = new Dictionary<Guid, MockAccount>();
            }

            public IDictionary<string, Guid> IdFromName { get; set; }

            public IDictionary<Guid, MockAccount> AccountFromId { get; set; }

            protected MockStorage Storage { get; set; }

            #region Implementation of IAccountStorage

            public Guid GetId(string name)
            {
                Guid id;

                if (!this.IdFromName.TryGetValue(name, out id))
                {
                    throw new AccountNotFound();
                }

                return id;
            }

            public IAccountInfo GetInfo(Guid accountId)
            {
                return this.GetMock(accountId);
            }

            public MockAccount GetMock(Guid accountId)
            {
                MockAccount account;

                if (!this.AccountFromId.TryGetValue(accountId, out account))
                {
                    throw new AccountNotFound();
                }

                return account;
            }

            public void SetInfo(Guid accountId, string description)
            {
                this.GetMock(accountId).Description = description;
            }

            public HashSet<Guid> GetUsers(Guid accountId)
            {
                return new HashSet<Guid>(this.GetMock(accountId).Users);
            }

            public Guid GetAdminId(Guid accountId)
            {
                return this.GetMock(accountId).Admin;
            }

            public void SetAdminId(Guid accountId, Guid userId)
            {
                this.GetMock(accountId).Admin = userId;
            }

            public void Add(Guid accountId, Guid userId)
            {
                var account = this.GetMock(accountId);
                var user = this.Storage.UserStorage.GetMock(userId);

                account.Users.Add(userId);
                user.Accounts.Add(accountId);
            }

            public void Remove(Guid accountId, Guid userId)
            {
                try
                {
                    var account = this.GetMock(accountId);
                    var user = this.Storage.UserStorage.GetMock(userId);

                    if (account.Admin == user.Id)
                    {
                        throw new UserIsAdmin();
                    }

                    account.Users.Remove(userId);
                    user.Accounts.Remove(accountId);
                }
                catch (UserNotFound)
                {
                }
                catch (AccountNotFound)
                {
                }
            }

            public Guid Create(Guid adminId, string name, string description, bool bypassNameReservation = false)
            {
                if (bypassNameReservation)
                    throw new NotImplementedException("Account name reservation has not been implemented in MockStorage");

                var user = this.Storage.UserStorage.GetMock(adminId);

                if (this.IdFromName.ContainsKey(name))
                {
                    throw new AccountAlreadyExists();
                }

                var id = Guid.NewGuid();
                user.Accounts.Add(id);
                this.IdFromName.Add(name, id);
                var personalList = new MockList
                    {
                        Description = "Personal list of " + name,
                        Followers = new HashSet<Guid> { id },
                        Name = name,
                        IsPersonnal = true,

                        // TODO: ?
                        IsPrivate = true,
                        Members = new HashSet<Guid> { id },
                        Owner = id,
                        Messages = new List<Guid>()
                    };
                this.Storage.ListStorage.ListFromId.Add(id, personalList);
                var account = new MockAccount
                    {
                        Admin = adminId,
                        Description = description,
                        Name = name,
                        Users = new HashSet<Guid> { adminId },

                        // TODO: Is the personal list here ?
                        AllFollowedLists = new HashSet<Guid> { id },
                        AllOwnedLists = new HashSet<Guid> { id },
                        MemberOfLists = new HashSet<Guid> { id },
                        PersonalList = id,
                        PublicFollowedLists = new HashSet<Guid>(),
                        Messages = new HashSet<Guid>(),
                        TaggedMessages = new HashSet<Guid>(),
                        PublicOwnedLists = new HashSet<Guid>()
                    };
                this.AccountFromId.Add(id, account);

                return id;
            }

            public void Delete(Guid accountId)
            {
                try
                {
                    var account = this.GetMock(accountId);
                    foreach (var userId in account.Users)
                    {
                        this.Storage.UserStorage.GetMock(userId).Accounts.Remove(accountId);
                    }

                    foreach (var msgId in account.Messages)
                    {
                        this.Storage.MsgStorage.Remove(msgId);
                    }

                    foreach (var taggedMsg in account.TaggedMessages)
                    {
                        this.Storage.MsgStorage.GetMock(taggedMsg).TaggedBy.Remove(accountId);
                    }

                    foreach (var listId in account.AllFollowedLists)
                    {
                        try
                        {
                            this.Storage.ListStorage.Unfollow(listId, accountId);
                        }
                        catch (AccountIsOwner)
                        {
                        }
                    }
                    
                    // Choosing the easy way
                    account.AllOwnedLists.Add(account.PersonalList);
                    foreach (var listId in account.AllOwnedLists)
                    {
                        this.Storage.ListStorage.Remove(listId, accountId);
                    }

                    this.IdFromName.Remove(account.Name);
                    this.AccountFromId.Remove(accountId);
                }
                catch (AccountNotFound)
                {
                }
            }

            public bool ReserveAccountName(string accountName)
            {
                throw new NotImplementedException("ReserveAccountName has not been implemented in MockStorage");
            }

            #endregion
        }

        protected class MockList : IListInfo
        {
            #region Implementation of IListInfo

            public string Name { get; set; }

            public string Description { get; set; }

            public bool IsPrivate { get; set; }

            public bool IsPersonnal { get; set; }

            #endregion

            public Guid Owner { get; set; }

            public ISet<Guid> Members { get; set; }

            public ISet<Guid> Followers { get; set; }

            public IList<Guid> Messages { get; set; }
        }

        protected class MockListStorage : IListStorage
        {
            public MockListStorage(MockStorage storage)
            {
                this.Storage = storage;
                this.ListFromId = new Dictionary<Guid, MockList>();
            }

            public IDictionary<Guid, MockList> ListFromId { get; set; }

            protected MockStorage Storage { get; set; }

            #region Implementation of IListStorage

            public IListInfo GetInfo(Guid listId)
            {
                return this.GetMock(listId);
            }

            public MockList GetMock(Guid listId)
            {
                MockList list;

                if (!this.ListFromId.TryGetValue(listId, out list))
                {
                    throw new ListNotFound();
                }

                return list;
            }

            public void SetInfo(Guid listId, string name, string description, bool isPrivate)
            {
                var list = this.GetMock(listId);
                if (list.IsPersonnal)
                {
                    throw new IsPersonnalList();
                }

                list.Name = name;
                list.Description = description;
                if (isPrivate == list.IsPrivate)
                {
                    return;
                }

                list.IsPrivate = isPrivate;

                MockAccount owner = this.Storage.AccountStorage.GetMock(list.Owner);

                if (isPrivate)
                {
                    owner.PublicOwnedLists.Remove(listId);
                }
                else
                {
                    owner.PublicOwnedLists.Add(listId);
                }

                foreach (var accountId in list.Followers)
                {
                    var account = this.Storage.AccountStorage.GetMock(accountId);

                    if (isPrivate)
                    {
                        account.PublicFollowedLists.Remove(listId);
                        if (!list.Members.Contains(accountId))
                        {
                            account.AllFollowedLists.Remove(listId);
                        }

                        list.Followers.IntersectWith(list.Members);
                    }
                    else
                    {
                        account.PublicFollowedLists.Add(listId);
                    }
                }
            }

            public Guid GetOwner(Guid listId)
            {
                return this.GetMock(listId).Owner;
            }

            public Guid GetPersonalList(Guid accountId)
            {
                return this.Storage.AccountStorage.GetMock(accountId).PersonalList;
            }

            public Guid Create(Guid ownerId, string name, string description, bool isPrivate)
            {
                var account = this.Storage.AccountStorage.GetMock(ownerId);
                var list = new MockList
                    {
                        Description = description,
                        IsPersonnal = false,
                        IsPrivate = isPrivate,
                        Name = name,
                        Owner = ownerId,
                        Members = new HashSet<Guid> { ownerId },
                        Followers = new HashSet<Guid> { ownerId },
                        Messages = new List<Guid>()
                    };
                var id = Guid.NewGuid();
                account.AllFollowedLists.Add(id);
                if (!isPrivate)
                {
                    account.PublicOwnedLists.Add(id);
                    account.PublicFollowedLists.Add(id);
                }

                account.AllOwnedLists.Add(id);
                account.MemberOfLists.Add(id);
                this.ListFromId.Add(id, list);

                return id;
            }

            public void Delete(Guid id)
            {
                MockList list;

                if (!this.ListFromId.TryGetValue(id, out list))
                {
                    return;
                }

                if (list.IsPersonnal)
                {
                    throw new IsPersonnalList();
                }

                foreach (var follower in list.Followers.Select(followerId => this.Storage.AccountStorage.GetMock(followerId)))
                {
                    follower.AllFollowedLists.Remove(id);
                    follower.PublicFollowedLists.Remove(id);
                }

                foreach (var member in list.Members.Select(memberId => this.Storage.AccountStorage.GetMock(memberId)))
                {
                    member.MemberOfLists.Remove(id);
                }

                var owner = this.Storage.AccountStorage.GetMock(list.Owner);
                owner.AllOwnedLists.Remove(id);
                owner.PublicFollowedLists.Remove(id);

                this.ListFromId.Remove(id);
            }

            public void Follow(Guid listId, Guid accountId)
            {
                var account = this.Storage.AccountStorage.GetMock(accountId);
                var list = this.GetMock(listId);

                list.Followers.Add(accountId);
                account.AllFollowedLists.Add(listId);
                if (!list.IsPrivate)
                {
                    account.PublicFollowedLists.Add(listId);
                }
            }

            public void Unfollow(Guid listId, Guid accountId)
            {
                var account = this.Storage.AccountStorage.GetMock(accountId);
                var list = this.GetMock(listId);

                if (accountId == list.Owner)
                {
                    throw new AccountIsOwner();
                }

                list.Followers.Remove(accountId);
                account.AllFollowedLists.Remove(listId);
                account.PublicFollowedLists.Remove(listId);
            }

            public HashSet<Guid> GetAccounts(Guid listId)
            {
                return new HashSet<Guid>(this.GetMock(listId).Members);
            }

            public void Add(Guid listId, Guid accountId)
            {
                var account = this.Storage.AccountStorage.GetMock(accountId);
                var list = this.GetMock(listId);

                list.Members.Add(accountId);
                account.MemberOfLists.Add(listId);

                foreach (var msg in account.Messages)
                {
                    list.Messages.Add(msg);
                }

                list.Messages = new List<Guid>(list.Messages.OrderBy(msg => this.Storage.MsgStorage.GetMock(msg).Date));
            }

            public void Remove(Guid listId, Guid accountId)
            {
                var account = this.Storage.AccountStorage.GetMock(accountId);
                var list = this.GetMock(listId);

                account.MemberOfLists.Remove(listId);
                list.Members.Remove(accountId);

                foreach (var msg in account.Messages)
                {
                    list.Messages.Remove(msg);
                }
            }

            public HashSet<Guid> GetAccountOwnedLists(Guid accountId, bool withPrivate)
            {
                return
                    new HashSet<Guid>(
                        withPrivate
                            ? this.Storage.AccountStorage.GetMock(accountId).AllOwnedLists
                            : this.Storage.AccountStorage.GetMock(accountId).PublicOwnedLists);
            }

            public HashSet<Guid> GetAccountFollowedLists(Guid accountId, bool withPrivate)
            {
                return
                    new HashSet<Guid>(
                        withPrivate
                            ? this.Storage.AccountStorage.GetMock(accountId).AllFollowedLists
                            : this.Storage.AccountStorage.GetMock(accountId).PublicFollowedLists);
            }

            public HashSet<Guid> GetFollowingLists(Guid accountId)
            {
                return new HashSet<Guid>(this.Storage.AccountStorage.GetMock(accountId).MemberOfLists);
            }

            public HashSet<Guid> GetFollowingAccounts(Guid listId)
            {
                return new HashSet<Guid>(this.GetMock(listId).Followers);
            }

            #endregion
        }

        protected class MockMsg : IMessage
        {
            #region Implementation of IMessage

            public Guid Id { get; set; }

            public Guid PosterId { get; set; }

            public string PosterName { get; set; }

            public string PosterAvatar { get; set; }

            public DateTime Date { get; set; }

            public string Content { get; set; }

            #endregion

            public ISet<Guid> TaggedBy { get; set; }
        }

        protected class MockMsgStorage : IMsgStorage
        {
            public MockMsgStorage(MockStorage storage)
            {
                this.Storage = storage;
                this.MsgFromId = new Dictionary<Guid, MockMsg>();
            }

            public IDictionary<Guid, MockMsg> MsgFromId { get; set; }

            protected MockStorage Storage { get; set; }

            public MockMsg GetMock(Guid msgId)
            {
                MockMsg msg;

                if (!this.MsgFromId.TryGetValue(msgId, out msg))
                {
                    throw new MessageNotFound();
                }

                return msg;
            }

            #region Implementation of IMsgStorage

            public List<IMessage> GetListsMsgFrom(HashSet<Guid> listsId, DateTime firstMsgDate, int msgNumber)
            {
                var messages = new List<IMessage>();
                foreach (var listId in listsId)
                {
                    messages.AddRange(
                        this.Storage.ListStorage.GetMock(listId).Messages.Select(this.GetMock).Where(
                            msg => msg.Date > firstMsgDate));
                }

                messages.Sort((msg1, msg2) => DateTime.Compare(msg1.Date, msg2.Date));

                return messages.Distinct().Take(msgNumber).ToList();
            }

            public List<IMessage> GetListsMsgTo(HashSet<Guid> listsId, DateTime lastMsgDate, int msgNumber)
            {
                var messages = new List<IMessage>();
                foreach (var listId in listsId)
                {
                    messages.AddRange(
                        this.Storage.ListStorage.GetMock(listId).Messages.Select(this.GetMock).Where(
                            msg => msg.Date < lastMsgDate));
                }

                // Note the reversing of the parameters because we supposedly want the *lasts* messages first
                messages.Sort((msg1, msg2) => DateTime.Compare(msg2.Date, msg1.Date));

                return messages.Distinct().Take(msgNumber).ToList();
            }

            public void Tag(Guid accountId, Guid msgId)
            {
                var account = this.Storage.AccountStorage.GetMock(accountId);
                var msg = this.GetMock(msgId);

                msg.TaggedBy.Add(accountId);
                account.TaggedMessages.Add(msgId);
            }

            public void Untag(Guid accoundId, Guid msgId)
            {
                try
                {
                    var account = this.Storage.AccountStorage.GetMock(accoundId);
                    var msg = this.GetMock(msgId);

                    msg.TaggedBy.Remove(accoundId);
                    account.TaggedMessages.Remove(msgId);
                }
                catch (AccountNotFound)
                {
                }
                catch (MessageNotFound)
                {
                }
            }

            public List<IMessage> GetTaggedFrom(Guid accoundId, DateTime firstMsgDate, int msgNumber)
            {
                var listsId = this.Storage.AccountStorage.GetMock(accoundId).TaggedMessages;
                var messages = new List<IMessage>();
                foreach (var listId in listsId)
                {
                    messages.AddRange(
                        this.Storage.ListStorage.GetMock(listId).Messages.Select(this.GetMock).Where(
                            msg => msg.Date > firstMsgDate));
                }

                messages.Sort((msg1, msg2) => DateTime.Compare(msg1.Date, msg2.Date));

                return messages.Take(msgNumber).ToList();
            }

            public List<IMessage> GetTaggedTo(Guid accountId, DateTime lastMsgDate, int msgNumber)
            {
                var listsId = this.Storage.AccountStorage.GetMock(accountId).TaggedMessages;
                var messages = new List<IMessage>();
                foreach (var listId in listsId)
                {
                    messages.AddRange(
                        this.Storage.ListStorage.GetMock(listId).Messages.Select(this.GetMock).Where(
                            msg => msg.Date < lastMsgDate));
                }

                // Note the reversing of the parameters because we supposedly want the *lasts* messages first
                messages.Sort((msg1, msg2) => DateTime.Compare(msg2.Date, msg1.Date));

                return messages.Take(msgNumber).ToList();
            }

            public Guid Post(Guid accountId, string content)
            {
                var account = this.Storage.AccountStorage.GetMock(accountId);
                var id = Guid.NewGuid();
                var message = new MockMsg
                    {
                        Content = content,
                        Date = DateTime.Now,
                        Id = id,
                        PosterAvatar = string.Empty,
                        PosterId = accountId,
                        PosterName = account.Name,
                        TaggedBy = new HashSet<Guid>()
                    };

                this.MsgFromId.Add(id, message);
                account.Messages.Add(id);
                foreach (var list in account.MemberOfLists.Select(this.Storage.ListStorage.GetMock))
                {
                    list.Messages.Add(id);
                    list.Messages =
                        new List<Guid>(list.Messages.OrderBy(msgId => this.Storage.MsgStorage.GetMock(msgId).Date));
                }

                return id;
            }

            public Guid Copy(Guid accountId, Guid msgId)
            {
                // TODO: must *not* be implemented until we know what this really means
                throw new NotImplementedException();
            }

            public void Remove(Guid id)
            {
                var msg = this.GetMock(id);
                var poster = this.Storage.AccountStorage.GetMock(msg.PosterId);

                poster.Messages.Remove(id);
                foreach (var list in poster.MemberOfLists.Select(this.Storage.ListStorage.GetMock))
                {
                    list.Messages.Remove(id);
                }

                foreach (var tagger in msg.TaggedBy.Select(this.Storage.AccountStorage.GetMock))
                {
                    tagger.TaggedMessages.Remove(id);
                }
            }

            #endregion
        }
    }
}
