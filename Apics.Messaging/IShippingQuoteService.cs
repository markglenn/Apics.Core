using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Apics.Messaging.Messages.Shipping;

namespace Apics.Messaging
{
    [ServiceContract( Namespace = "http://www.apics.org/services/" )]
    public interface IShippingQuoteService
    {
        [OperationContract( 
            Action = "http://wwww.apics.org/services/shipping/quoterequest",
            ReplyAction = "http://wwww.apics.org/services/shipping/quotes" )]
        ShipmentQuote[ ] GetShipmentQuotes( ShippingQuoteRequest request );
    }
}
