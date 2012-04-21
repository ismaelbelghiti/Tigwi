// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AccountSet.cs" company="">
//   
// </copyright>
// <summary>
//   The i account set.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Tigwi.UI.Models.Storage
{
    using System;

    using StorageLibrary;

    /// <summary>
    /// The account set.
    /// </summary>
    public class AccountRepository : IAccountRepository
    {
        #region Constants and Fields

        /// <summary>
        /// The storage obj.
        /// </summary>
        private readonly IStorage storageObj;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountRepository"/> class. 
        /// Initializes a new instance of the <see cref="AccountRepository"/> class. 
        /// Initializes a new instance of the <see cref="AccountRepository"/> class.
        /// </summary>
        /// <param name="storageObj">
        /// The storage obj.
        /// </param>
        public AccountRepository(IStorage storageObj)
        {
            this.storageObj = storageObj;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets StorageObj.
        /// </summary>
        protected IStorage StorageObj
        {
            get
            {
                return this.storageObj;
            }
        }

        #endregion

        #region Public Methods and Operators

        public StorageAccountModel Create(StorageUserModel user, string name, string description)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="account">
        /// The account.
        /// </param>
        public void Delete(StorageAccountModel account)
        {
            this.StorageObj.Account.Delete(account.Id);
        }

        /// <summary>
        /// The find.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// </returns>
        public StorageAccountModel Find(Guid id)
        {
            return new StorageAccountModel(this.StorageObj, id);
        }

        /// <summary>
        /// The find.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// </returns>
        public StorageAccountModel Find(string name)
        {
            return new StorageAccountModel(this.StorageObj, name);
        }

        #endregion
    }
}