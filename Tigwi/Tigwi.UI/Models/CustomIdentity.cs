namespace Tigwi.UI.Models
{
    #region

    using System;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Security.Principal;

    using Tigwi.UI.Models.Storage;

    #endregion

    [Serializable]
    public class CustomIdentity : GenericIdentity, ISerializable
    {
        #region Constructors and Destructors

        public CustomIdentity(Guid userId, Guid accountId, string name, string type)
            : base(name, type)
        {
            this.UserId = userId;
            this.AccountId = accountId;
        }

        #endregion

        #region Public Properties

        public Guid AccountId { get; set; }

        public Guid UserId { get; set; }

        #endregion

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

            for (int i = 0; i < serializableMembers.Length; i++)
            {
                info.AddValue(serializableMembers[i].Name, serializableValues[i]);
            }
        }
    }
}