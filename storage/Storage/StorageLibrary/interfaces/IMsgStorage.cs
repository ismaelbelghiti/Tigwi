using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageLibrary
{
    // Be carefull : their is no warranty an all about time synchronisation between differents VM in azure
    // To be sure messages come in the right order, we wait a few moments between the time they are posted and the time we show them

    public interface IMsgStorage
    {
        /// <summary>
        /// Return the firsts msgNumber messages posted after firstMsgId contained into lists listsId.
        /// Does not include firstMasgDate
        /// </summary>
        /// <exception cref="ListNotFound">A list in listID doesn't exists</exception>
        List<IMessage> GetListsMsgFrom(HashSet<Guid> listsId, DateTime firstMsgDate, int msgNumber);

        /// <summary>
        /// Return the last msgNumber messages posted before lastMsgDate contained into listId.
        /// Does not include lastMsgDate
        /// </summary>
        /// <exception cref="ListNotFound">A list in listID doesn't exists</exception>
        List<IMessage> GetListsMsgTo(HashSet<Guid> listsId, DateTime lastMsgDate, int msgNumber);
        
        /// <summary>
        /// Add a message as favorit
        /// Doesn't do anything is the message is already in favorits
        /// </summary>
        /// <exception cref="AccountNotFound">no account has this ID</exception>
        /// <exception cref="MessageNotFound">no message has this ID</exception>
        void Tag(Guid accountId, Guid msgId);

        /// <summary>
        /// Remove a message from favorits
        /// Doesn't do anything if the account or the msg doesn't exists
        /// </summary>
        void Untag(Guid accoundId, Guid msgId);

        /// <summary>
        /// Return the firsts msgNumber messages posted after firstMsgId tagged by the account.
        /// Does not include firstMasgDate
        /// </summary>
        /// <exception cref="AccountNotFound">No account has this id</exception>
        List<IMessage> GetTaggedFrom(Guid accoundId, DateTime firstMsgDate, int msgNumber);

        /// <summary>
        /// Return the last msgNumber messages posted before firstMsgId tagged by the account.
        /// Does not include firstMasgDate
        /// </summary>
        /// <exception cref="AccountNotFound">No account has this ID</exception>
        List<IMessage> GetTaggedTo(Guid accountId, DateTime lastMsgDate, int msgNumber);

        /// <summary>
        /// Post a message and return its Id
        /// </summary>
        /// <exception cref="AccountNotFound">no account has this ID</exception>
        Guid Post(Guid accountId, string content);

        /// <summary>
        /// Copy a message to accountId and return the new ID
        /// </summary>
        /// <exception cref="AccountNotFound">no account has this ID</exception>
        /// <exception cref="MessageNotFound">no message has this ID</exception>
        Guid Copy(Guid accountId, Guid msgId);

        /// <summary>
        /// Delete a message
        /// Don't do anything if the message doesn't exists
        /// </summary>
        void Remove(Guid id);
    }
}
