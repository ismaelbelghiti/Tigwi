namespace Tigwi.UI.Models.Storage
{
    using System;

    public interface IPostRepository
    {
        #region Public Methods and Operators

        StoragePostModel Create(StorageAccountModel poster, string content);

        void Delete(StoragePostModel post);

        #endregion
    }
}