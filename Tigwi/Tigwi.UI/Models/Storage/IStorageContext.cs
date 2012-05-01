namespace Tigwi.UI.Models.Storage
{
    public interface IStorageContext
    {
        #region Public Properties

        IAccountRepository Accounts { get; }

        IListRepository Lists { get; }

        IPostRepository Posts { get; }

        IUserRepository Users { get; }

        #endregion

        #region Public Methods and Operators

        void SaveChanges();

        #endregion
    }
}