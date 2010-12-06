using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Apics.Model.Fulfillment;
using Apics.Data.AptifyAdapter.Extension;

namespace Apics.Data.AptifyAdapter.IntegrationTests.Orders
{
    public class HandlePaymentsTests : AptifyIntegrationTestsBase
    {
        [Test]
        public void Test( )
        {
            var orders = GetRepository<Order>( );

            var order = orders
                .Where( o => o.OrderState.Status.Id == 1 )
                .Where( o => o.InitialPaymentAmount > 0.0M )
                .First( o => o.Shipments.Any( ) );

            var cost = order.Costs.GrandTotal;
            var shippingCost = Math.Round( ( decimal )new Random( ).NextDouble( ) * 10.0M, 2 );
            var shipment = order.Shipments.First( );

            shipment.OverrideShippingCalculation = true;
            shipment.ShippingCharge = shippingCost;
            
            GetRepository<Shipment>( ).Update( shipment );

            orders.Update( order );
            GetRepository<OrderCosts>( ).Refresh( order.Costs );

            Assert.AreEqual( order.Costs.SubTotal + shippingCost, order.Costs.GrandTotal );
        }
    }
}
