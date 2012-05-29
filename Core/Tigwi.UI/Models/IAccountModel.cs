#region copyright
// Copyright (c) 2012, TIGWI
// All rights reserved.
// Distributed under  BSD 2-Clause license
#endregion
namespace Tigwi.UI.Models
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The interface specification for an account
    /// </summary>
    public interface IAccountModel
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the account's owner.
        /// </summary>
        IUserModel Admin { get; set; }

        /// <summary>
        /// Gets a collection of all the lists followed by the account.
        /// </summary>
        IListModelCollection AllFollowedLists { get; }

        /// <summary>
        /// Gets a collection of all the lists owned by the account.
        /// </summary>
        IListModelEnumerable AllOwnedLists { get; }

        /// <summary>
        /// Gets or sets the account's description.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Gets the account ID.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Gets the lists of whose the account is a member.
        /// </summary>
        IListModelCollection MemberOfLists { get; }

        /// <summary>
        /// Gets the account's name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the account's personal list.
        /// </summary>
        IListModel PersonalList { get; }

        /// <summary>
        /// Gets the public lists followed by the account.
        /// </summary>
        IListModelEnumerable PublicFollowedLists { get; }

        /// <summary>
        /// Gets the public lists owned by the account.
        /// </summary>
        IListModelEnumerable PublicOwnedLists { get; }

        /// <summary>
        /// Gets all the users who can post to the account's timeline.
        /// </summary>
        ICollection<IUserModel> Users { get; }

        #endregion
    }
}