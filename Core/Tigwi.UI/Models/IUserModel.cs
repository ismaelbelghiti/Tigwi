// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IUserModel.cs" company="ENS Paris">
//   BSD
// </copyright>
// <summary>
//   The interface specification for a user model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Tigwi.UI.Models
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The interface specification for a user model.
    /// </summary>
    public interface IUserModel
    {
        #region Public Properties

        /// <summary>
        /// Gets the accounts to whose the user can post.
        /// </summary>
        ICollection<IAccountModel> Accounts { get; }

        /// <summary>
        /// Gets or sets the user's avatar.
        /// </summary>
        string Avatar { get; set; }

        /// <summary>
        /// Gets or sets the user's email.
        /// </summary>
        string Email { get; set; }

        /// <summary>
        /// Gets or sets the user's main account Id.
        /// </summary>
        IAccountModel MainAccount { get; set; }

        /// <summary>
        /// Gets the user's ID.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Gets the user's login.
        /// </summary>
        string Login { get; }

        /// <summary>
        /// Sets the user's password
        /// </summary>
        string Password { set; }

        #endregion
    }
}