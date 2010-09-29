using System;
using System.Runtime.Serialization;

namespace Apics.Utilities.Messaging
{
    public interface IMessage
    {
        /// <summary>
        /// Message ID
        /// </summary>
        [DataMember]
        string MessageId { get; set; }
    }
}