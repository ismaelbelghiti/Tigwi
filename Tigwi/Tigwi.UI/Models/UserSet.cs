// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserSet.cs" company="">
//   
// </copyright>
// <summary>
//   The i user set.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Tigwi.UI.Models
{
    using System;

    /// <summary>
    /// The i user set.
    /// </summary>
    public interface IUserSet
    {
        #region Public Methods and Operators

        /// <summary>
        /// The find.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <returns>
        /// </returns>
        UserModel Find(Guid user);

        #endregion
    }

    /// <summary>
    /// The user set.
    /// </summary>
    public class UserSet : IUserSet
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UserSet"/> class. 
        /// Initializes a new instance of the <see cref="UserSet"/> class. 
        /// Initializes a new instance of the <see cref="UserSet"/> class.
        /// </summary>
        /// <param name="o">
        /// The o.
        /// </param>
        public UserSet(object o)
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The find.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public UserModel Find(Guid user)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}