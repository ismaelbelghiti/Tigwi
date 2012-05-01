namespace Tigwi.UI.Models
{
    public interface IPostRepository
    {
        #region Public Methods and Operators

        IPostModel Create(IAccountModel poster, string content);

        void Delete(IPostModel post);

        #endregion
    }
}