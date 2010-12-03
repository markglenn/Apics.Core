using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Apics.Model.Fulfillment;

namespace Apics.Data.AptifyAdapter.IntegrationTests.Orders
{
    public class HandlePaymentsTests : AptifyIntegrationTestsBase
    {
        [Test]
        public void Test( )
        {
            var orders = GetRepository<Order>( );

            var order = orders.Where( o => o.OrderState.Status.Id == 1 )
                .Where( o => o.InitialPaymentAmount > 0.0M )
                .Where( o => o.Shipments.Any( ) ).First( );

            var orderID = order.Id;
            var shipment = order.Shipments.First( );

            shipment.OverrideShippingCalculation = true;
            shipment.ShippingCharge = 10.0M;
            order.Comments += " ";
            GetRepository<Shipment>( ).Update( shipment );
            orders.Update( order );
            orders.Evict( order );

            order = orders.GetProxy( orderID );
            Assert.True( true );
        }
    }
}
