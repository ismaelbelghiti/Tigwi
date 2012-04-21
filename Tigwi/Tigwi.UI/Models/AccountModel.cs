namespace Tigwi.UI.Models
{
    #region

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    #endregion

    /// <summary>
    /// The account model.
    /// </summary>
    public abstract class AccountModel
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets Admin.
        /// </summary>
        [Display(Name = "Owner", 
            Description =
                "The owner of an account can add or remove people with access to this account, and can defintely delete the account."
            )]
        public abstract UserModel Admin { get; set; }

        public abstract ICollection<ListModel> AllFollowedLists { get; }

        public abstract ICollection<ListModel> AllOwnedLists { get; }

        /// <summary>
        /// Gets or sets Description.
        /// </summary>
        [DataType(DataType.MultilineText)]
        public abstract string Description { get; set; }

        /// <summary>
        /// Gets Id.
        /// </summary>
        [Key]
        [Editable(false)]
        public abstract Guid Id { get; }

        public abstract ICollection<ListModel> MemberLists { get; }

        /// <summary>
        /// Gets Name.
        /// </summary>
        [Key]
        [Required]
        [Editable(false)]
        public abstract string Name { get; }

        public abstract ListModel PersonalList { get; }

        public abstract ICollection<ListModel> PublicFollowedLists { get; }

        public abstract ICollection<ListModel> PublicOwnedLists { get; }

        public abstract ICollection<UserModel> Users { get; }

        #endregion

        #region Public Methods and Operators

        public abstract void Save();

        #endregion
    }
}