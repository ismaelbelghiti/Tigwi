namespace Tigwi.UI.Models
{
    using System;

    /// <summary>
    /// The i list set.
    /// </summary>
    public interface IListSet
    {
        ListModel Find(Guid listId);
    }

    /// <summary>
    /// The list set.
    /// </summary>
    public class ListSet : IListSet
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ListSet"/> class. 
        /// Initializes a new instance of the <see cref="ListSet"/> class.
        /// </summary>
        /// <param name="o">
        /// The o.
        /// </param>
        public ListSet(object o)
        {
        }

        #endregion

        public ListModel Find(Guid listId)
        {
            throw new NotImplementedException();
        }
    }
}