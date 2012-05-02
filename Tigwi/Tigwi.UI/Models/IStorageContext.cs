// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStorageContext.cs" company="ENS Paris">
//   BSD
// </copyright>
// <summary>
//   The interface specification for a UnitOfWork context.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Tigwi.UI.Models
{
    /// <summary>
    /// The interface specification for a UnitOfWork context.
    /// </summary>
    public interface IStorageContext
    {
        #region Public Properties

        /// <summary>
        /// Gets the accounts repository.
        /// </summary>
        IAccountRepository Accounts { get; }

        /// <summary>
        /// Gets the lists repository.
        /// </summary>
        IListRepository Lists { get; }

        /// <summary>
        /// Gets the posts repository.
        /// </summary>
        IPostRepository Posts { get; }

        /// <summary>
        /// Gets the users repository.
        /// </summary>
        IUserRepository Users { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Commit the changes to the underlying data persistence.
        /// </summary>
        void SaveChanges();

        // TODO (later): Rollback()

        #endregion
    }
}