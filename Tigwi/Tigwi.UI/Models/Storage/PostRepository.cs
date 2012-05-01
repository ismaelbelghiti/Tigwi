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
                return new StoragePostModel(this.StorageContext, null);

                /*new IMessage
                        {
                            Content = content,
                            Date = DateTime.Now,
                            Id = id,
                            PosterAvatar = string.Empty,
                            PosterId = poster.Id,
                            PosterName = poster.Name
                        });*/
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