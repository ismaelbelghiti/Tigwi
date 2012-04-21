namespace Tigwi.UI.Models.Storage
{
    using System;

    /// <summary>
    /// The i post set.
    /// </summary>
    public interface IPostSet
    {
        #region Public Methods and Operators

        /// <summary>
        /// The find.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// </returns>
        PostModel Find(Guid id);

        /// <summary>
        /// The find.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// </returns>
        PostModel Find(string name);

        #endregion
    }

    /// <summary>
    /// The post set.
    /// </summary>
    public class PostSet : IPostSet
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PostSet"/> class. 
        /// Initializes a new instance of the <see cref="PostSet"/> class.
        /// </summary>
        /// <param name="o">
        /// The o.
        /// </param>
        public PostSet(object o)
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The find.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public PostModel Find(Guid id)
        {
            // TODO
            throw new NotImplementedException();
        }

        /// <summary>
        /// The find.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public PostModel Find(string name)
        {
            // TODO
            throw new NotImplementedException();
        }

        #endregion
    }
}