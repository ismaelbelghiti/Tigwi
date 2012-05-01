namespace Tigwi.UI.Models.Storage
{
    using System;
    using System.Collections.Generic;

    using StorageLibrary;

    public class UserRepository : StorageEntityRepository<StorageUserModel>, IUserRepository
    {
        public UserRepository(IStorage storage, IStorageContext storageContext)
            : base(storage, storageContext)
        {
        }

        #region Public Methods and Operators

        public StorageUserModel Create(string login, string email)
        {
            // Create a new user via medium-level storage calls and return the corresponding Model
            try
            {
                var id = this.Storage.User.Create(login, email, string.Empty);
                return this.Find(id);
            }
            catch (UserAlreadyExists userAlreadyExists)
            {
                throw new DuplicateUserException(login, userAlreadyExists);
            }
        }

        public void Delete(StorageUserModel user)
        {
            // TODO: fixme
            var id = user.Id;

            // Forget everything about him
            this.Storage.User.Delete(id);
            this.EntitiesMap.Remove(id);
        }

        public StorageUserModel Find(Guid user)
        {
            StorageUserModel userModel;

            // First check the cache.
            if (!this.EntitiesMap.TryGetValue(user, out userModel))
            {
                userModel = new StorageUserModel(this.Storage, this.StorageContext, user);
                this.EntitiesMap.Add(user, userModel);
            }

            return userModel;
        }

        public StorageUserModel Find(string login)
        {
            try
            {
                var id = this.Storage.User.GetId(login);
                return this.Find(id);
            }
            catch (UserNotFound userNotFound)
            {
                throw new UserNotFoundException(login, userNotFound);
            }
        }

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