namespace Tigwi.UI.Models.Storage
{
    using System;
    using System.Collections.Generic;

    using StorageLibrary;

    public class UserRepository : IUserRepository
    {
        public UserRepository(IStorage storage, IStorageContext storageContext)
        {
            this.Storage = storage;
            this.StorageContext = storageContext;
            this.Users = new Dictionary<Guid, StorageUserModel>();
        }

        protected IStorage Storage { get; private set; }

        protected IStorageContext StorageContext { get; private set; }

        protected Dictionary<Guid, StorageUserModel> Users { get; set; } 

        #region Public Methods and Operators

        public StorageUserModel Create(string login, string email)
        {
            try
            {
                var id = this.Storage.User.Create(login, email, string.Empty);
                return this.Find(id);
            }
            catch (UserAlreadyExists userAlreadyExists)
            {
                // throw new DuplicateUserException(login, userAlreadyExists);
                throw;
            }
        }

        public void Delete(StorageUserModel user)
        {
            this.Storage.User.Delete(user.Id);
            this.Users.Remove(user.Id);
        }

        public StorageUserModel Find(Guid user)
        {
            StorageUserModel userModel;

            if (this.Users.TryGetValue(user, out userModel))
            {
                return userModel;
            }

            userModel = new StorageUserModel(this.Storage, this.StorageContext, user);
            this.Users.Add(user, userModel);
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
                // throw new UserNotFoundException(login, userNotFound);
                throw;
            }
        }

        #endregion
    }
}