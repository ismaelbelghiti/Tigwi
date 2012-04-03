using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageLibrary
{
    public interface IMsgStorage
    {
        /// <summary>
        /// Return the firsts msgNumber messages posted after firstMsgId contained into lists listsId.
        /// Does not include firstMasgDate
        /// Can throw : ListNotFound
        /// </summary>
        List<IMessage> GetListsMsgFrom(HashSet<Guid> listsId, DateTime firstMsgDate, int msgNumber);
        /// <summary>
        /// Return the last msgNumber messages posted before lastMsgDate contained into listId.
        /// Does not include lastMsgDate
        /// Can throw : ListNotFound
        /// </summary>
        List<IMessage> GetListsMsgTo(HashSet<Guid> listsId, DateTime lastMsgDate, int msgNumber);
        
        /// <summary>
        /// Add a message as favorit
        /// Can throw : AccountNotFound, MessageNotFound
        /// </summary>
        void Tag(Guid accountId, Guid msgId);
        /// <summary>
        /// Remove a message from favorits
        /// Can throw : AccountNotFound, MessageNotFound
        /// </summary>
        void Untag(Guid accoundId, Guid msgId);

        /// <summary>
        /// Return the firsts msgNumber messages posted after firstMsgId tagged by the account.
        /// Does not include firstMasgDate
        /// Can throw : UserNotFound
        /// </summary>
        List<IMessage> GetTaggedFrom(Guid accoundId, DateTime firstMsgDate, int msgNumber);
        /// <summary>
        /// Return the last msgNumber messages posted before firstMsgId tagged by the account.
        /// Does not include firstMasgDate
        /// Can throw : UserNotFound
        /// </summary>
        List<IMessage> GetTaggedTo(Guid accountId, DateTime lastMsgDate, int msgNumber);

        /// <summary>
        /// Post a message and return its Id
        /// Can throw : AccountNotFound
        /// </summary>
        Guid Post(Guid accountId, string content);
        /// <summary>
        /// Copy a message to accountId and return the new ID
        /// Can throw : AccountNotFound, MessageNotFound
        /// </summary>
        Guid Copy(Guid accountId, Guid msgId);
        /// <summary>
        /// Delete a message
        /// </summary>
        void Remove(Guid id);
    }
}
