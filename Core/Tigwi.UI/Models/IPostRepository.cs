// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPostRepository.cs" company="ENS Paris">
//   BSD
// </copyright>
// <summary>
//   The interface specification for a post repository.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Tigwi.UI.Models
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The interface specification for a post repository
    /// </summary>
    public interface IPostRepository
    {
        #region Public Methods and Operators

        /// <summary>
        /// Creates a new post with the given account as author.
        /// </summary>
        /// <param name="poster">
        /// The new post's author.
        /// </param>
        /// <param name="content">
        /// The new post's content.
        /// </param>
        /// <returns>
        /// </returns>
        IPostModel Create(IAccountModel poster, string content);

        IPostModel Find(Guid id);

        IEnumerable<IPostModel> LastPosts();

        /// <summary>
        /// Deletes the given post.
        /// </summary>
        /// <param name="post">
        /// The post to delete.
        /// </param>
        void Delete(IPostModel post);

        #endregion
    }
}