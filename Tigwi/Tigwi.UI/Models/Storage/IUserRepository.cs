namespace Tigwi.UI.Models.Storage
{
    using System;

    public interface IUserRepository
    {
        #region Public Methods and Operators

        StorageUserModel Create(string login, string email);

        void Delete(StorageUserModel user);

        StorageUserModel Find(Guid user);

        StorageUserModel Find(string login);

        #endregion
    }
}