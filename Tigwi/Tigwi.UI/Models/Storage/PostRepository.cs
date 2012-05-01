namespace Tigwi.UI.Models.Storage
{
    #region

    using System;

    using StorageLibrary;

    #endregion

    public class PostRepository : StorageEntityRepository<StoragePostModel>, IPostRepository
    {
        #region Constructors and Destructors

        public PostRepository(IStorage storage, IStorageContext storageContext)
            : base(storage, storageContext)
        {
        }

        #endregion

        #region Public Methods and Operators

        public IPostModel Create(IAccountModel poster, string content)
        {
            // TODO: StorageUserModel.Post
            try
            {
                var id = this.Storage.Msg.Post(poster.Id, content);

                var message = new Message(id, poster.Id, poster.Name, string.Empty, DateTime.Now, content);

                return new StoragePostModel(this.StorageContext, message);
            }
            catch (AccountNotFound ex)
            {
                throw new AccountNotFoundException(poster.Name, ex);
            }
        }

        public void Delete(IPostModel post)
        {
            throw new NotImplementedException();
        }

        #endregion

        // TODO: We need a find.

        internal void SaveChanges()
        {
            
        }
    }
}