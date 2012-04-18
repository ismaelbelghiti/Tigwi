// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StorageContext.cs" company="">
//   
// </copyright>
// <summary>
//   The storage context.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Tigwi.UI.Models
{
    using System;

    using StorageLibrary;

    /// <summary>
    /// The i storage context.
    /// </summary>
    public interface IStorageContext
    {
        #region Public Properties

        /// <summary>
        /// Gets Accounts.
        /// </summary>
        IAccountSet Accounts { get; }

        /// <summary>
        /// Gets Lists.
        /// </summary>
        IListSet Lists { get; }

        /// <summary>
        /// Gets Posts.
        /// </summary>
        IPostSet Posts { get; }

        /// <summary>
        /// Gets Users.
        /// </summary>
        IUserSet Users { get; }

        #endregion
    }

    /// <summary>
    /// The storage context.
    /// </summary>
    public class StorageContext : IStorageContext
    {
        #region Constants and Fields

        /// <summary>
        /// The storage obj.
        /// </summary>
        private readonly IStorage storageObj;

        /// <summary>
        /// The accounts.
        /// </summary>
        private IAccountSet accounts;

        /// <summary>
        /// The lists.
        /// </summary>
        private IListSet lists;

        /// <summary>
        /// The posts.
        /// </summary>
        private IPostSet posts;

        /// <summary>
        /// The users.
        /// </summary>
        private IUserSet users;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StorageContext"/> class. 
        /// Initializes a new instance of the <see cref="StorageContext"/> class.
        /// </summary>
        /// <param name="storageObj">
        /// The storage obj.
        /// </param>
        public StorageContext(IStorage storageObj)
        {
            this.storageObj = storageObj;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets Accounts.
        /// </summary>
        public IAccountSet Accounts
        {
            get
            {
                return this.accounts ?? (this.accounts = new AccountSet(this.StorageObj));
            }
        }

        /// <summary>
        /// Gets Lists.
        /// </summary>
        public IListSet Lists
        {
            get
            {
                return this.lists ?? (this.lists = new ListSet(this.StorageObj));
            }
        }

        /// <summary>
        /// Gets Posts.
        /// </summary>
        public IPostSet Posts
        {
            get
            {
                return this.posts ?? (this.posts = new PostSet(this.StorageObj));
            }
        }

        /// <summary>
        /// Gets Users.
        /// </summary>
        public IUserSet Users
        {
            get
            {
                return this.users ?? (this.users = new UserSet(this.StorageObj));
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets StorageObj.
        /// </summary>
        private IStorage StorageObj
        {
            get
            {
                return this.storageObj;
            }
        }

        #endregion
    }
}