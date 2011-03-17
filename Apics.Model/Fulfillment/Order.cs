using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Apics.Model.Location;
using Apics.Model.User;
using Castle.ActiveRecord;
using Apics.Model.Financial;
using Apics.Data;

namespace Apics.Model.Fulfillment
{
    /// <summary>
    /// An order in the system
    /// </summary>
    [ActiveRecord( "OrderMaster", Lazy = true )]
    [DebuggerDisplay( "Order: {Id}" )]
    public class Order
    {
        public Order( )
        {
            this.Items = new List<OrderItem>( );
            this.OrderState = new OrderState( );
            this.Shipments = new List<Shipment>( );
            this.PaymentInformation = new PaymentInformation( );
            this.Costs = new OrderCosts( );
        }

        /// <summary>
        /// Order ID
        /// </summary>
        [PrimaryKey]
        public virtual int Id { get; set; }

        /// <summary>
        /// Date this order was placed
        /// </summary>
        [Property]
        public virtual DateTime OrderDate { get; set; }

        [Property]
        public virtual bool BillToSameAsShipTo { get; set; }

        [Property( "ReturnShippingCharge" )]
        public virtual bool ShouldCalculateShipping { get; set; }

        /// <summary>
        /// Any comments associated with the order
        /// </summary>
        [Property]
        public virtual string Comments { get; set; }

        /// <summary>
        /// Order items
        /// </summary>
        [HasMany( Cascade = ManyRelationCascadeEnum.All, Lazy = true )]
        public virtual IList<OrderItem> Items { get; set; }

        /// <summary>
        /// State of the order
        /// </summary>
        [OneToOne]
        public virtual OrderState OrderState { get; set; }

        /// <summary>
        /// State of the order
        /// </summary>
        [OneToOne]
        public virtual OrderCosts Costs { get; set; }

        [Property]
        public virtual decimal? InitialPaymentAmount { get; set; }

        /// <summary>
        /// Person receiving this shipment
        /// </summary>
        [BelongsTo( "ShipToID", Lazy = FetchWhen.OnInvoke )]
        public virtual Person ShipToPerson { get; set; }

        /// <summary>
        /// Person that was billed
        /// </summary>
        [BelongsTo( "BillToID", Lazy = FetchWhen.OnInvoke )]
        public virtual Person BillToPerson { get; set; }

        /// <summary>
        /// Billing company
        /// </summary>
        [BelongsTo( "BillToCompanyID", Lazy = FetchWhen.OnInvoke )]
        public virtual Company BillToCompany { get; set; }

        /// <summary>
        /// Shipping company
        /// </summary>
        [BelongsTo( "ShipToCompanyID", Lazy = FetchWhen.OnInvoke )]
        public virtual Company ShipToCompany { get; set; }

        /// <summary>
        /// Billing phone number
        /// </summary>
        [BelongsTo( "BillToPhoneID", Lazy = FetchWhen.OnInvoke )]
        public virtual PhoneNumber BillToPhone { get; set; }

        /// <summary>
        /// The address to which the order is being shipped
        /// </summary>
        [BelongsTo( "ShipToAddressID", Lazy = FetchWhen.OnInvoke, Cascade = CascadeEnum.All )]
        public virtual Address ShipToAddress { get; set; }

        /// <summary>
        /// Shipping phone number
        /// </summary>
        [BelongsTo( "ShipToPhoneID", Lazy = FetchWhen.OnInvoke )]
        public virtual PhoneNumber ShipToPhone { get; set; }

        /// <summary>
        /// The address to which the order is being billed
        /// </summary>
        [BelongsTo( "BillToAddressID", Lazy = FetchWhen.OnInvoke, Cascade = CascadeEnum.All )]
        public virtual Address BillToAddress { get; set; }

        [BelongsTo( "ShipTypeID", Lazy = FetchWhen.OnInvoke )]
        public virtual ShipType ShipType { get; set; }

        [BelongsTo( "PaymentInformationID", Cascade = CascadeEnum.All, Lazy = FetchWhen.OnInvoke )]
        public virtual PaymentInformation PaymentInformation { get; set; }

        [HasMany( Lazy = true, Cascade = ManyRelationCascadeEnum.All )]
        public virtual IList<Shipment> Shipments { get; set; }

        [BelongsTo( "EmployeeID", NotNull = true, Lazy = FetchWhen.OnInvoke )]
        public virtual Employee Employee { get; set; }

    }

    public static class OrderQueries
    {
        public static IQueryable<Order> Shippable( this IEnumerable<Order> orders )
        {
            return orders.AsQueryable( )
                .Where( o => o.OrderState.Status.Name == "Taken" )
                .Where( o => o.OrderState.Type.Name == "Regular" )
                .Where( o => o.Items.Any( i =>
                    i.Product.DistributionType.Name == "Hard Shipment" &&
                        i.Product.Parent == null &&
                            i.Product.Code != String.Empty ) )
                .Where( o => !o.Items.Any( i => i.Product.DateAvailable > DateTime.Now ) );
        }

        public static IQueryable<Order> Shipped( this IEnumerable<Order> orders )
        {
            return orders.AsQueryable( )
                .Where( o => o.OrderState.Status.Name == "Shipped" );
        }

        public static IQueryable<Order> WithProduct( this IEnumerable<Order> orders, string productCode )
        {
            return orders.AsQueryable( ).Where( o => 
                o.Items.Any( i =>
                    i.Product.Code == productCode ) );
        }
    }
}