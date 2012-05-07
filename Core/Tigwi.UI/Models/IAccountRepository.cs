// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAccountRepository.cs" company="ENS Paris">
//   BSD
// </copyright>
// <summary>
//   The interface specification for an Account repository.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Tigwi.UI.Models
{
    using System;

    using Tigwi.UI.Models.Storage;

    /// <summary>
    /// The interface specification for an Account repository.
    /// </summary>
    public interface IAccountRepository
    {
        #region Public Methods and Operators

        /// <summary>
        /// Creates a new account.
        /// </summary>
        /// <param name="user">
        /// The new account's administrator.
        /// </param>
        /// <param name="name">
        /// The new account's name.
        /// </param>
        /// <param name="description">
        /// The new account's description.
        /// </param>
        /// <returns>
        /// A <see cref="IAccountModel"/> representing the newly created account.
        /// </returns>
        /// <exception cref="DuplicateAccountException">
        /// When there is already an existing account with the same name.
        /// </exception>
        IAccountModel Create(IUserModel user, string name, string description);

        /// <summary>
        /// Deletes an existing account and replace it by a shallow "Deleted" object.
        /// </summary>
        /// <param name="account">
        /// The account to delete.
        /// </param>
        void Delete(IAccountModel account);

        /// <summary>
        /// Find an account corresponding to the given <see cref="Guid"/>.
        /// </summary>
        /// <param name="account">
        /// The Guid of the account to retrieve.
        /// </param>
        /// <returns>
        /// The <see cref="StorageAccountModel"/> representing the account with the given Id in the context.
        /// </returns>
        /// <exception cref="AccountNotFoundException">
        /// When there is no account with the given Id in the context.
        /// </exception>
        IAccountModel Find(Guid account);

        /// <summary>
        /// Find an account corresponding to the given name.
        /// </summary>
        /// <param name="name">
        /// The name of the account to retrieve
        /// </param>
        /// <returns>
        /// The <see cref="StorageAccountModel"/> representing the account with the given name in the context.
        /// </returns>
        /// <exception cref="AccountNotFoundException">
        /// When there is no account with the given name in the context.
        /// </exception>
        IAccountModel Find(string name);

        #endregion
    }
}