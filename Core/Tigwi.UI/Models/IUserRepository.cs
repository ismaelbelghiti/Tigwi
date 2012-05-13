// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IUserRepository.cs" company="ENS Paris">
//   BSD
// </copyright>
// <summary>
//   The interface specification for a user repository.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Tigwi.UI.Models
{
    using System;

    using Tigwi.UI.Models.Storage;

    /// <summary>
    /// The interface specification for a user repository.
    /// </summary>
    public interface IUserRepository
    {
        #region Public Methods and Operators

        /// <summary>
        /// Creates a new user with the given credentials.
        /// </summary>
        /// <param name="login">
        /// The desired login.
        /// </param>
        /// <param name="email">
        /// The desired email address.
        /// </param>
        /// <returns>
        /// A <see cref="IUserModel"/> representing the newly created user.
        /// </returns>
        /// <exception cref="DuplicateUserException">
        /// When there is already a user with the given credentials.
        /// </exception>
        IUserModel Create(string login, string email);

        /// <summary>
        /// Deletes the given user and replaces it with a shallow "Deleted" object.
        /// </summary>
        /// <param name="user">
        /// The user to delete.
        /// </param>
        void Delete(IUserModel user);

        /// <summary>
        /// Finds a user with the given Id.
        /// </summary>
        /// <param name="userId">
        /// The Id of the user to retrieve.
        /// </param>
        /// <returns>
        /// A <see cref="IUserModel"/> representing the user with the given Id.
        /// </returns>
        IUserModel Find(Guid userId);

        /// <summary>
        /// Finds a user with the given login.
        /// </summary>
        /// <param name="login">
        /// The login identifying the user to retrieve.
        /// </param>
        /// <returns>
        /// A <see cref="IUserModel"/> representing the user with the given login.
        /// </returns>
        /// <exception cref="UserNotFoundException">
        /// When there is no user with the given login.
        /// </exception>
        IUserModel Find(string login);

        bool TryFind(string login, out IUserModel user);

        #endregion
    }
}