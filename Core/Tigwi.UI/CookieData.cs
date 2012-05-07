// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CookieData.cs" company="ENS Paris">
//   BSD
// </copyright>
// <summary>
//   A serializable <see cref="CookieData" /> class to store some data in a cookie (currently, the current user and
//   the current account's IDs).
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Tigwi.UI
{
    using System;

    /// <summary>
    /// The cookie data.
    /// </summary>
    [Serializable]
    public class CookieData
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the current account by its ID.
        /// </summary>
        public Guid AccountId { get; set; }

        /// <summary>
        /// Gets or sets the current user by its ID.
        /// </summary>
        public Guid UserId { get; set; }

        #endregion
    }
}