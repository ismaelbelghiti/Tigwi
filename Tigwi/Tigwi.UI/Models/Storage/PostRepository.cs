namespace Tigwi.UI.Models.Storage
{
    using System;

    using StorageLibrary;

    public class PostRepository : StorageEntityRepository<StoragePostModel>, IPostRepository
    {
        public PostRepository(IStorage storage, IStorageContext storageContext)
            : base(storage, storageContext)
        {
        }

        #region Implementation of IPostRepository

        public StoragePostModel Create(StorageAccountModel poster, string content)
        {
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

        public void Delete(StoragePostModel post)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}