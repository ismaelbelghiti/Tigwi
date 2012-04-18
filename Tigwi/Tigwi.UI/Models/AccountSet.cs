// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AccountSet.cs" company="">
//   
// </copyright>
// <summary>
//   The i account set.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Tigwi.UI.Models
{
    using System;

    using StorageLibrary;

    /// <summary>
    /// The i account set.
    /// </summary>
    public interface IAccountSet
    {
        #region Public Methods and Operators

        /// <summary>
        /// The create.
        /// </summary>
        /// <returns>
        /// </returns>
        AccountModel Create();

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="account">
        /// The account.
        /// </param>
        void Delete(AccountModel account);

        /// <summary>
        /// The find.
        /// </summary>
        /// <param name="account">
        /// The account.
        /// </param>
        /// <returns>
        /// </returns>
        AccountModel Find(Guid account);

        /// <summary>
        /// The find.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// </returns>
        AccountModel Find(string name);

        #endregion
    }

    /// <summary>
    /// The account set.
    /// </summary>
    public class AccountSet : IAccountSet
    {
        #region Constants and Fields

        /// <summary>
        /// The storage obj.
        /// </summary>
        private readonly IStorage storageObj;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountSet"/> class. 
        /// Initializes a new instance of the <see cref="AccountSet"/> class. 
        /// Initializes a new instance of the <see cref="AccountSet"/> class.
        /// </summary>
        /// <param name="storageObj">
        /// The storage obj.
        /// </param>
        public AccountSet(IStorage storageObj)
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

        /// <summary>
        /// The create.
        /// </summary>
        /// <exception cref="NotImplementedException">
        /// Not implemented folks :-)
        /// </exception>
        public AccountModel Create()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="account">
        /// The account.
        /// </param>
        public void Delete(AccountModel account)
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
        public AccountModel Find(Guid id)
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
        public AccountModel Find(string name)
        {
            return new StorageAccountModel(this.StorageObj, name);
        }

        #endregion
    }
}