namespace Tigwi.UI.Models
{
    #region

    using System;

    using Tigwi.UI.Models.Storage;

    #endregion

    public interface IListRepository
    {
        #region Public Methods and Operators

        IListModel Create(IAccountModel account, string name, string description, bool isPrivate);

        void Delete(IListModel list);

        IListModel Find(Guid listId);

        #endregion
    }
}