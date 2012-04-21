namespace Tigwi.UI.Models.Storage
{
    using System;

    public interface IAccountRepository
    {
        #region Public Methods and Operators

        StorageAccountModel Create(StorageUserModel user, string name, string description);

        void Delete(StorageAccountModel account);

        StorageAccountModel Find(Guid account);

        StorageAccountModel Find(string name);

        #endregion
    }
}