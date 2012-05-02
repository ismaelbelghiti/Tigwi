// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomIdentity.cs" company="ENS Paris">
//   BSD
// </copyright>
// <summary>
//   A custom identity class allowing us to store some additional informations in the FormsAuthentication framework.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Tigwi.UI
{
    using System;
    using System.Runtime.Serialization;
    using System.Security.Principal;

    /// <summary>
    /// A custom identity class allowing us to store some additional informations in the FormsAuthentication framework..
    /// </summary>
    [Serializable]
    public class CustomIdentity : GenericIdentity, ISerializable
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomIdentity"/> class. 
        /// </summary>
        /// <param name="userId">
        /// The current user's id.
        /// </param>
        /// <param name="accountId">
        /// The current account's id.
        /// </param>
        /// <param name="name">
        /// The identity name (for GenericIdentity).
        /// </param>
        /// <param name="type">
        /// The connection type (for GenericIdentity).
        /// </param>
        public CustomIdentity(Guid userId, Guid accountId, string name, string type)
            : base(name, type)
        {
            this.UserId = userId;
            this.AccountId = accountId;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the current account by its ID.
        /// </summary>
        public Guid AccountId { get; set; }

        /// <summary>
        /// Gets or sets the current user by its ID.
        /// </summary>
        public Guid UserId { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// De-serialize to a GenericIdentity for serialization outside of Tigwi's boundaries.
        /// This is required at least by the development server and should be kept as is.
        /// </summary>
        /// <param name="info">
        /// The serialization information.
        /// </param>
        /// <param name="context">
        /// The streaming context to serialize into.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// When serialization is not supported.
        /// </exception>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (context.State != StreamingContextStates.CrossAppDomain)
            {
                throw new InvalidOperationException("Serialization not supported");
            }

            var genericIdentity = new GenericIdentity(this.Name, this.AuthenticationType);
            info.SetType(genericIdentity.GetType());

            var serializableMembers = FormatterServices.GetSerializableMembers(genericIdentity.GetType());
            var serializableValues = FormatterServices.GetObjectData(genericIdentity, serializableMembers);

            for (var i = 0; i < serializableMembers.Length; i++)
            {
                info.AddValue(serializableMembers[i].Name, serializableValues[i]);
            }
        }

        #endregion
    }
}