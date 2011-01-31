using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Apics.Model.Fulfillment;
using Apics.Model.User;
using Apics.Model.Financial;
using Ninject;
using Aptify.Framework.BusinessLogic.GenericEntity;

namespace Apics.Data.AptifyAdapter.IntegrationTests.Orders
{
    [TestFixture]
    public class HandlePaymentsTests : AptifyIntegrationTestsBase
    {
        [Test]
        public void UpdatingShipmentCostsUpdatesOrderCosts( )
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

            orders.Update( order );

            GetRepository<OrderCosts>( ).Refresh( order.Costs );
            Assert.AreEqual( order.Costs.SubTotal + shippingCost, order.Costs.GrandTotal );
        }

        [Test]
        public void TestCreditCardOrderCreation( )
        {
            try
            {


                var orders = GetRepository<Order>( );

                var order = new Order
                {
                    BillToPerson = GetRepository<Person>( ).GetProxy( 1744343 ),
                    ShipToPerson = GetRepository<Person>( ).GetProxy( 1744343 ),
                };

                var item = new OrderItem
                {
                    Product = GetRepository<Product>( ).GetProxy( 5795 ),
                    Quantity = 1,
                    Price = 1649
                };

                order.Items.Add( item );

                order.PaymentInformation.PaymentType = GetRepository<PaymentType>( ).GetProxy( 1 );
                order.PaymentInformation.CCAccountNumber = "4111111111111111";
                order.PaymentInformation.CCExpireDate = DateTime.Today.AddYears( 1 );

                order.OrderState.Status = GetRepository<OrderStatus>( ).GetProxy( 1 );
                order.OrderState.Type = GetRepository<OrderType>( ).GetProxy( 1 );
                order.ShipType = GetRepository<ShipType>( ).GetProxy( 1 );
                order.Employee = GetRepository<Employee>( ).GetProxy( 1 );

                orders.Insert( order );
            }
            catch ( Exception )
            {

                throw;
            }
        }

        [Test]
        public void TestCreditCardRetrieval( )
        {
            var orders = GetRepository<Order>( );

            var order = orders
                .Where( o => o.PaymentInformation != null )
                .Where( o => o.PaymentInformation.PaymentType != null )
                .Where( o => o.PaymentInformation.PaymentType.Id == 1 )
                .First( );

            var server = Kernel.Get<AptifyServer>( );

            var entity = server.GetEntity( order );

            Assert.IsNotEmpty( ( string )entity.GetField( "CCAccountNumber" ).Value );
        }

        [Test]
        public void TestPurchaseOrderCreation( )
        {
            var orders = GetRepository<Order>( );

            var order = new Order
            {
                BillToPerson = GetRepository<Person>( ).GetProxy( 1744343 ),
                ShipToPerson = GetRepository<Person>( ).GetProxy( 1744343 ),
            };

            var item = new OrderItem
            {
                Product = GetRepository<Product>( ).GetProxy( 5795 ),
                Quantity = 1,
                Price = 1649
            };

            order.Items.Add( item );

            order.PaymentInformation.PaymentType = GetRepository<PaymentType>( ).GetProxy( 6 );
            order.PaymentInformation.PONumber = "1234";
            order.InitialPaymentAmount = 200;

            order.OrderState.Status = GetRepository<OrderStatus>( ).GetProxy( 1 );
            order.OrderState.Type = GetRepository<OrderType>( ).GetProxy( 1 );
            order.ShipType = GetRepository<ShipType>( ).GetProxy( 1 );
            order.Employee = GetRepository<Employee>( ).GetProxy( 1 );

            orders.Insert( order );
        }
    }
}
