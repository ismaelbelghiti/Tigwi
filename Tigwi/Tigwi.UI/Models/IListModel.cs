// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IListModel.cs" company="ENS Paris">
//   BSD
// </copyright>
// <summary>
//   The interface specification for a List.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Tigwi.UI.Models
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The interface specification for a list.
    /// </summary>
    public interface IListModel
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the list's description.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Gets the list's followers.
        /// </summary>
        ICollection<IAccountModel> Followers { get; }

        /// <summary>
        /// Gets the list's ID.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Gets a value indicating whether the list is a personal list or not.
        /// </summary>
        bool IsPersonal { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the list is private or not.
        /// </summary>
        bool IsPrivate { get; set; }

        /// <summary>
        /// Gets the list's members (accounts whose posts appear on the list).
        /// </summary>
        ICollection<IAccountModel> Members { get; }

        /// <summary>
        /// Gets or sets the list's name.
        /// </summary>
        string Name { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets a (truncated) collection of the posts in the list posted after a given DateTime (excluded).
        /// </summary>
        /// <param name="date">
        /// The lower bound DateTime (excluded).
        /// </param>
        /// <param name="maximum">
        /// The maximum number of posts to retrieve.
        /// </param>
        /// <returns>
        /// </returns>
        ICollection<IPostModel> PostsAfter(DateTime date, int maximum = 100);

        /// <summary>
        /// Gets a (truncated) collection of the posts posted before a given DateTime (excluded).
        /// </summary>
        /// <param name="date">
        /// The higher bound DateTime (excluded).
        /// </param>
        /// <param name="maximum">
        /// The maximum number of posts to retrieve.
        /// </param>
        /// <returns>
        /// </returns>
        ICollection<IPostModel> PostsBefore(DateTime date, int maximum = 100);

        #endregion
    }
}