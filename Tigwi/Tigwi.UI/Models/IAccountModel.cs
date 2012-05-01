namespace Tigwi.UI.Models
{
    using System;
    using System.Collections.Generic;

    using Tigwi.UI.Models.Storage;

    public interface IAccountModel
    {
        IUserModel Admin { get; set; }

        ICollection<IListModel> AllFollowedLists { get; }

        ICollection<IListModel> AllOwnedLists { get; }

        string Description { get; set; }

        ICollection<IListModel> MemberOfLists { get; }

        string Name { get; }

        IListModel PersonalList { get; }

        ICollection<IListModel> PublicFollowedLists { get; }

        ICollection<IListModel> PublicOwnedLists { get; }

        ICollection<IUserModel> Users { get; }

        Guid Id { get; }
    }
}