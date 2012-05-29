#region copyright
// Copyright (c) 2012, TIGWI
// All rights reserved.
// Distributed under  BSD 2-Clause license
#endregion
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
        /// <returns>True when every changes were saved, false when at least one error occured.</returns>
        bool SaveChanges();

        // TODO (later): Rollback()

        #endregion
    }
}