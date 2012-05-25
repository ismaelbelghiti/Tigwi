// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PostRepository.cs" company="ENS Paris">
//   BSD
// </copyright>
// <summary>
//   An implementation of the <see cref="IPostRepository" /> interface for Azure Storage.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Tigwi.UI.Models.Storage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Tigwi.Storage.Library;

    /// <summary>
    /// An implementation of the <see cref="IListRepository" /> interface for Azure Storage.
    /// </summary>
    public class PostRepository : StorageEntityRepository<StoragePostModel>, IPostRepository
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PostRepository"/> class. 
        /// </summary>
        /// <param name="storageContext">
        /// The storage context.
        /// </param>
        public PostRepository(StorageContext storageContext)
            : base(storageContext)
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Create a new post, posted by the given account.
        /// </summary>
        /// <param name="poster">
        /// The poster's account.
        /// </param>
        /// <param name="content">
        /// The message's content.
        /// </param>
        /// <returns>
        /// A <see cref="IPostModel"/> representing the newly created message.
        /// </returns>
        /// <exception cref="AccountNotFoundException">
        /// When the given account does not exist.
        /// </exception>
        public IPostModel Create(IAccountModel poster, string content)
        {
            // TODO: Move to StorageUserModel.Post
            try
            {
                Guid id = this.Storage.Msg.Post(poster.Id, content);

                // TODO: clean this up. Maybe don't use an intermediate IMessage.
                var message = new Message(id, poster.Id, poster.Name, string.Empty, DateTime.Now, content)
                    {
                        Content = content, 
                        Id = id, 
                        Date = DateTime.Now, 
                        PosterAvatar = string.Empty, 
                        PosterId = poster.Id, 
                        PosterName = poster.Name
                    };

                return new StoragePostModel(this.StorageContext, message);
            }
            catch (AccountNotFound ex)
            {
                throw new AccountNotFoundException(poster.Name, ex);
            }
        }

        public IPostModel Find(Guid id)
        {
            return new StoragePostModel(this.StorageContext, this.Storage.Msg.GetMessage(id));
        }

        public IEnumerable<IPostModel> LastPosts()
        {
            return this.Storage.Msg.GetLastMessages().Select(msg => new StoragePostModel(this.StorageContext, msg));
        }

                /// <summary>
        /// Delete the given post.
        /// </summary>
        /// <param name="post">
        /// The post to delete.
        /// </param>
        /// <exception cref="NotImplementedException">
        /// Because this is not yet implemented !
        /// </exception>
        public void Delete(IPostModel post)
        {
            // TODO: mark as deleted
            this.Storage.Msg.Remove(post.Id);
        }

        #endregion

        // TODO: We need a find. Ask the Storage team
        #region Methods

        /// <summary>
        /// Commit the changes.
        /// </summary>
        internal bool SaveChanges()
        {
            // Nothing to do as posts can't change
            // Thus, it's always a success
            return true;
        }

        #endregion
    }
}