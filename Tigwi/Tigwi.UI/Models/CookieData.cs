namespace Tigwi.UI.Models
{
    #region

    using System;

    #endregion

    [Serializable]
    public class CookieData
    {
        #region Public Properties

        public Guid AccountId { get; set; }

        public Guid UserId { get; set; }

        #endregion
    }
}