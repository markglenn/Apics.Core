using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Apics.Utilities.Messaging
{
    [ServiceContract]
    public interface ISendMessageContract
    {
        [OperationContract( IsOneWay = true )]
        void SubmitMessage( IMessage message );
    }
}
