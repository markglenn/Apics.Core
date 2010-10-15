using System;
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

    [ServiceContract( Name="IShippingQuoteService" )]
    public interface IAsyncShippingQuoteService
    {
        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginGetShipmentQuotes( ShippingQuoteRequest request, AsyncCallback callback, object state );

        ShipmentQuote[ ] EndGetShipmentQuotes( IAsyncResult result );
    }
}
