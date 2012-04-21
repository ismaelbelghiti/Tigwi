namespace Tigwi.UI.Models.Storage
{
    using StorageLibrary;

    public interface IStorageContext
    {
        #region Public Properties

        IAccountRepository Accounts { get; }

        IListSet Lists { get; }

        IPostSet Posts { get; }

        IUserRepository Users { get; }

        #endregion
    }

    public class StorageContext : IStorageContext
    {
        #region Constants and Fields

        private readonly IStorage storageObj;

        private IAccountRepository accounts;

        private IListSet lists;

        private IPostSet posts;

        private IUserRepository users;

        #endregion

        #region Constructors and Destructors

        public StorageContext(IStorage storageObj)
        {
            this.storageObj = storageObj;
        }

        #endregion

        #region Public Properties

        public IAccountRepository Accounts
        {
            get
            {
                return this.accounts ?? (this.accounts = new AccountRepository(this.StorageObj, this));
            }
        }

        public IListSet Lists
        {
            get
            {
                return this.lists ?? (this.lists = new ListSet(this.StorageObj));
            }
        }

        public IPostSet Posts
        {
            get
            {
                return this.posts ?? (this.posts = new PostSet(this.StorageObj));
            }
        }

        public IUserRepository Users
        {
            get
            {
                return this.users ?? (this.users = new UserRepository(this.StorageObj, this));
            }
        }

        #endregion

        #region Properties

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