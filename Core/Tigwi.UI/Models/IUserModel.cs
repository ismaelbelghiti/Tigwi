#region copyright
// Copyright (c) 2012, TIGWI
// All rights reserved.
// Distributed under  BSD 2-Clause license
#endregion
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

        /// <summary>
        /// Gets the list of active API keys for this user
        /// </summary>
        IDictionary<Guid, string> ApiKeys { get; }

        #endregion

        /// <summary>
        /// Generate and return a new api key
        /// </summary>
        /// <param name="applicationName">Name of the application that will use the key</param>
        /// <returns></returns>
        Guid GenerateApiKey(string applicationName);

        /// <summary>
        /// Deactivate an api key belonging to the user
        /// </summary>
        /// <param name="apiKey"></param>
        void DeactivateApiKey(Guid apiKey);
    }
}