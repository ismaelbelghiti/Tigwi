namespace Tigwi.UI.Models
{
    #region

    using System;
    using System.Security.Principal;

    using Tigwi.UI.Models.Storage;

    #endregion

    public class CustomIdentity : IIdentity
    {
        #region Constructors and Destructors

        public CustomIdentity(StorageUserModel user, StorageAccountModel account)
        {
            this.User = user;
            this.Account = account;
            this.Name = user.Login + "/" + account.Name;
            this.IsAuthenticated = true;
        }

        #endregion

        #region Public Properties

        public StorageAccountModel Account { get; private set; }

        public string AuthenticationType
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsAuthenticated { get; private set; }

        public string Name { get; private set; }

        public StorageUserModel User { get; private set; }

        #endregion
    }
}