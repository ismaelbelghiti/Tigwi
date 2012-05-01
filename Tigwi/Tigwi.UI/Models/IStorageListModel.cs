namespace Tigwi.UI.Models
{
    using System;
    using System.Collections.Generic;

    using Tigwi.UI.Models.Storage;

    public interface IListModel
    {
        string Description { get; set; }

        ICollection<IAccountModel> Followers { get; }

        bool IsPersonal { get; }

        bool IsPrivate { get; set; }

        ICollection<IAccountModel> Members { get; }

        string Name { get; set; }

        Guid Id { get; }

        ICollection<IPostModel> PostsAfter(DateTime date, int maximum = 100);

        ICollection<IPostModel> PostsBefore(DateTime date, int maximum = 100);
    }
}