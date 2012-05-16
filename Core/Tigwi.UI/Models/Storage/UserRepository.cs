// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserRepository.cs" company="ENS Paris">
//   BSD
// </copyright>
// <summary>
//   An implementation of the <see cref="IUSerRepository" /> interface for Azure Storage.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Tigwi.UI.Models.Storage
{
    using System;
    using System.Data;

    using Tigwi.Storage.Library;

    /// <summary>
    /// An implementation of the <see cref="Tigwi.UI.Models.IUserRepository" /> interface for Azure Storage.
    /// </summary>
    public class UserRepository : StorageEntityRepository<StorageUserModel>, IUserRepository
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class. 
        /// </summary>
        /// <param name="storageContext">
        /// The storage context.
        /// </param>
        internal UserRepository(StorageContext storageContext)
            : base(storageContext)
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="login">
        /// The new user's login.
        /// </param>
        /// <param name="email">
        /// The new user's email.
        /// </param>
        /// <returns>
        /// A <see cref="IUserModel" /> representing the new user.
        /// </returns>
        /// <exception cref="DuplicateUserException">
        /// When there is already a user with the given login. 
        /// </exception>
        public IUserModel Create(string login, string email)
        {
            try
            {
                // Create a new user via medium-level storage calls, then find it by its ID.
                // TODO: prepopulate ?
                var id = this.Storage.User.Create(login, email, new byte[1]);
                return this.Find(id);
            }
            catch (UserAlreadyExists ex)
            {
                throw new DuplicateUserException(login, ex);
            }
        }

        /// <summary>
        /// Delete an existing user.
        /// </summary>
        /// <param name="user">
        /// The user to delete.
        /// </param>
        public void Delete(IUserModel user)
        {
            // TODO: fixme
            Guid id = user.Id;

            // Forget everything about him
            this.Storage.User.Delete(id);
            this.EntitiesMap.Remove(id);
        }

        /// <summary>
        /// Find a user given its ID.
        /// </summary>
        /// <param name="userId">
        /// The user ID to find.
        /// </param>
        /// <returns>
        /// A <see cref="IUserModel" /> representing the found user.
        /// </returns>
        public IUserModel Find(Guid userId)
        {
            return this.InternalFind(userId);
        }

        /// <summary>
        /// Find a user given its login.
        /// </summary>
        /// <param name="login">
        /// The user login to find.
        /// </param>
        /// <returns>
        /// A <see cref="IUserModel" /> representing the found user.
        /// </returns>
        /// <exception cref="UserNotFoundException">
        /// Where there is no user with the given login.
        /// </exception>
        public IUserModel Find(string login)
        {
            IUserModel model;

            if (!this.TryFind(login, out model))
            {
                throw new UserNotFoundException(login, null);
            }

            return model;
        }

        public bool TryFind(string login, out IUserModel user)
        {
            try
            {
                // Delegate the call to the API.
                var id = this.Storage.User.GetId(login);
                user = this.Find(id);
                return true;
            }
            catch (UserNotFound)
            {
                user = null;
                return false;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Find a user by its ID and returns it with its true type, <see cref="StorageUserModel" />.
        /// </summary>
        /// <param name="userId">
        /// The user.
        /// </param>
        /// <returns>
        /// </returns>
        internal StorageUserModel InternalFind(Guid userId)
        {
            StorageUserModel userModel;

            // First check the cache.
            if (!this.EntitiesMap.TryGetValue(userId, out userModel))
            {
                userModel = new StorageUserModel(this.Storage, this.StorageContext, userId);
                this.EntitiesMap.Add(userId, userModel);
            }

            return userModel;
        }

        /// <summary>
        /// Commit the changes.
        /// </summary>
        internal void SaveChanges()
        {
            foreach (var user in this.EntitiesMap)
            {
                user.Value.Save();
            }
        }

        #endregion
    }
}