namespace Tigwi.UI.Models.Storage
{
    using System;

    public interface IPostRepository
    {
        #region Public Methods and Operators

        IPostModel Create(IAccountModel poster, string content);

        void Delete(IPostModel post);

        #endregion
    }
}