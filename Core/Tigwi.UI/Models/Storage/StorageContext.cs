#region copyright
// Copyright (c) 2012, TIGWI
// All rights reserved.
// Distributed under  BSD 2-Clause license
#endregion
namespace Tigwi.UI.Models.Storage
{
    using Tigwi.Storage.Library;

    /// <summary>
    /// An implementation of a storage context connected to Azure Storage.
    /// </summary>
    public class StorageContext : IStorageContext
    {
        #region Constants and Fields

        /// <summary>
        /// The internal storage object.
        /// </summary>
        private readonly IStorage storageObj;

        /// <summary>
        /// The accounts repository.
        /// </summary>
        private AccountRepository accounts;

        /// <summary>
        /// The lists repository.
        /// </summary>
        private ListRepository lists;

        /// <summary>
        /// The posts repository.
        /// </summary>
        private PostRepository posts;

        /// <summary>
        /// The users repository.
        /// </summary>
        private UserRepository users;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StorageContext"/> class. 
        /// </summary>
        /// <param name="storageObj">
        /// The storage object.
        /// </param>
        public StorageContext(IStorage storageObj)
        {
            this.storageObj = storageObj;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the accounts repository.
        /// </summary>
        public IAccountRepository Accounts
        {
            get
            {
                return this.InternalAccounts;
            }
        }

        /// <summary>
        /// Gets the lists repository.
        /// </summary>
        public IListRepository Lists
        {
            get
            {
                return this.InternalLists;
            }
        }

        /// <summary>
        /// Gets the posts repository.
        /// </summary>
        public IPostRepository Posts
        {
            get
            {
                return this.InternalPosts;
            }
        }

        /// <summary>
        /// Gets the users repository.
        /// </summary>
        public IUserRepository Users
        {
            get
            {
                return this.InternalUsers;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the accounts repository with its real type, for internal use.
        /// </summary>
        internal AccountRepository InternalAccounts
        {
            get
            {
                return this.accounts ?? (this.accounts = new AccountRepository(this));
            }
        }

        /// <summary>
        /// Gets the lists repository with its real type, for internal use.
        /// </summary>
        internal ListRepository InternalLists
        {
            get
            {
                return this.lists ?? (this.lists = new ListRepository(this));
            }
        }

        /// <summary>
        /// Gets the posts repository with its real type, for internal use.
        /// </summary>
        internal PostRepository InternalPosts
        {
            get
            {
                return this.posts ?? (this.posts = new PostRepository(this));
            }
        }

        /// <summary>
        /// Gets the users repository with its real type, for internal use.
        /// </summary>
        internal UserRepository InternalUsers
        {
            get
            {
                return this.users ?? (this.users = new UserRepository(this));
            }
        }

        /// <summary>
        /// Gets the internal storage object.
        /// </summary>
        internal IStorage StorageObj
        {
            get
            {
                return this.storageObj;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Commit the changes to the storage.
        /// </summary>
        public bool SaveChanges()
        {
            var success = true;

            if (this.users != null)
            {
                success &= this.users.SaveChanges();
            }

            if (this.posts != null)
            {
                success &= this.posts.SaveChanges();
            }

            if (this.accounts != null)
            {
                success &= this.accounts.SaveChanges();
            }

            if (this.lists != null)
            {
                success &= this.lists.SaveChanges();
            }

            return success;
        }

        #endregion
    }
}