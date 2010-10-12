using System.ServiceModel;
using Apics.Messaging.Messages.Shipping;

namespace Apics.Messaging
{
    [ServiceContract]
    public interface IShippingQuoteService
    {
        [OperationContract]
        ShipmentQuote[ ] GetShipmentQuotes( ShippingQuoteRequest request );
    }
}
