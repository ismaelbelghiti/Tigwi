using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tigwi.UI.Models
{
    using System.Collections;

    public interface IListModelEnumerable : IEnumerable<IListModel>
    {
        IEnumerable<Guid> Ids { get; }

        IEnumerable<IPostModel> PostsAfter(DateTime date, int maximum = 100);

        IEnumerable<IPostModel> PostsBefore(DateTime date, int maximum = 100);
    }
}