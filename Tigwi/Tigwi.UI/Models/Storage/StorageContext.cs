namespace Tigwi.UI.Models.Storage
{
    using StorageLibrary;

    public class StorageContext : IStorageContext
    {
        #region Constants and Fields

        private readonly IStorage storageObj;

        private AccountRepository accounts;

        private ListRepository lists;

        private PostRepository posts;

        private UserRepository users;

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

        public void SaveChanges()
        {
            if (this.users != null)
            {
                this.users.SaveChanges();
            }

            if (this.posts != null)
            {
                this.posts.SaveChanges();
            }

            if (this.accounts != null)
            {
                this.accounts.SaveChanges();
            }

            if (this.lists != null)
            {
                this.lists.SaveChanges();
            }
        }

        #endregion

        #region Properties

        internal IStorage StorageObj
        {
            get
            {
                return this.storageObj;
            }
        }

        internal AccountRepository InternalAccounts
        {
            get
            {
                return this.accounts ?? (this.accounts = new AccountRepository(this.StorageObj, this));
            }
        }

        internal ListRepository InternalLists
        {
            get
            {
                return this.lists ?? (this.lists = new ListRepository(this.StorageObj, this));
            }
        }

        internal PostRepository InternalPosts
        {
            get
            {
                return this.posts ?? (this.posts = new PostRepository(this.StorageObj, this));
            }
        }

        internal UserRepository InternalUsers
        {
            get
            {
                return this.users ?? (this.users = new UserRepository(this.StorageObj, this));
            }
        }

        #endregion
    }
}