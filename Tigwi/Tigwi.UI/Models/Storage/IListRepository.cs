namespace Tigwi.UI.Models.Storage
{
    #region

    using System;

    #endregion

    public interface IListRepository
    {
        #region Public Methods and Operators

        IListModel Create(StorageAccountModel account, string name, string description, bool isPrivate);

        void Delete(StorageListModel list);

        StorageListModel Find(Guid listId);

        #endregion
    }
}