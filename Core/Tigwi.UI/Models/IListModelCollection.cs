using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tigwi.UI.Models
{
    public interface IListModelCollection : ICollection<IListModel>
    {
        ICollection<Guid> Ids { get; }

        ICollection<IPostModel> PostsAfter(DateTime date, int maximum = 100);

        ICollection<IPostModel> PostsBefore(DateTime date, int maximum = 100);
    }
}