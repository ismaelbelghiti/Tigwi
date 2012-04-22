namespace Tigwi.UI.Models.Storage
{
    using StorageLibrary;

    public interface IStorageContext
    {
        #region Public Properties

        IAccountRepository Accounts { get; }

        IListRepository Lists { get; }

        IPostRepository Posts { get; }

        IUserRepository Users { get; }

        #endregion
    }

    public class StorageContext : IStorageContext
    {
        #region Constants and Fields

        private readonly IStorage storageObj;

        private IAccountRepository accounts;

        private IListRepository lists;

        private IPostRepository posts;

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

        public IListRepository Lists
        {
            get
            {
                return this.lists ?? (this.lists = new ListRepository(this.StorageObj, this));
            }
        }

        public IPostRepository Posts
        {
            get
            {
                return this.posts ?? (this.posts = new PostRepository(this.StorageObj, this));
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