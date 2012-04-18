// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AccountModel.cs" company="">
//   
// </copyright>
// <summary>
//   The account model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Tigwi.UI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

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
        public virtual UserModel Admin { get; set; }

        /// <summary>
        /// Gets or sets Description.
        /// </summary>
        [DataType(DataType.MultilineText)]
        public virtual string Description { get; set; }

        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        [Key]
        [Editable(false)]
        public virtual Guid Id { get; protected set; }

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        [Key]
        [Required]
        [Editable(false)]
        public virtual string Name { get; protected set; }

        /// <summary>
        /// Gets or sets Users.
        /// </summary>
        public virtual ICollection<UserModel> Users { get; protected set; }

        public virtual ICollection<ListModel> PublicOwnedLists { get; protected set; }

        public virtual ICollection<ListModel> AllOwnedLists { get; protected set; }

        public virtual ICollection<ListModel> PublicFollowedLists { get; protected set; }

        public virtual ICollection<ListModel> AllFollowedLists { get; protected set; }

        public virtual ICollection<ListModel> MemberLists { get; protected set; } 

        public virtual ListModel PersonalList { get; protected set; }

        #endregion

        public abstract void Save();
    }
}