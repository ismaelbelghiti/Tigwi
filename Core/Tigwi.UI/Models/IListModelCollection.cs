using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tigwi.UI.Models
{
    public interface IListModelCollection : ICollection<IListModel>, IListModelEnumerable
    {
    }
}