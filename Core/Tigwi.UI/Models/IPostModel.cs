// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPostModel.cs" company="ENS Paris">
//   BSD
// </copyright>
// <summary>
//   The interface specification for a post model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Tigwi.UI.Models
{
    using System;

    /// <summary>
    /// The interface specification for a post model.
    /// </summary>
    public interface IPostModel
    {
        #region Public Properties

        /// <summary>
        /// Gets the post's content.
        /// </summary>
        [System.Web.Mvc.AllowHtml]
        string Content { get; }

        /// <summary>
        /// Gets the post's ID.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Gets the DateTime at which the post was created.
        /// </summary>
        DateTime PostDate { get; }

        /// <summary>
        /// Gets the author of the post.
        /// </summary>
        IAccountModel Poster { get; }

        #endregion
    }
}