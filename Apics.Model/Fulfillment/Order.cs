using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Apics.Model.Location;
using Apics.Model.User;
using Castle.ActiveRecord;

namespace Apics.Model.Fulfillment
{
    /// <summary>
    /// An order in the system
    /// </summary>
    [ActiveRecord( "OrderMaster", Lazy = true )]
    [DebuggerDisplay( "Order: {Id}" )]
    public class Order
    {
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

        /// <summary>
        /// Person receiving this shipment
        /// </summary>
        [BelongsTo( "ShipToID" )]
        public virtual Person ShipToPerson { get; set; }

        /// <summary>
        /// Person that was billed
        /// </summary>
        [BelongsTo( "BillToID" )]
        public virtual Person BillToPerson { get; set; }

        /// <summary>
        /// Billing company
        /// </summary>
        [BelongsTo( "BillToCompanyID" )]
        public virtual Company BillToCompany { get; set; }

        /// <summary>
        /// Shipping company
        /// </summary>
        [BelongsTo( "ShipToCompanyID" )]
        public virtual Company ShipToCompany { get; set; }

        /// <summary>
        /// Billing phone number
        /// </summary>
        [BelongsTo( "BillToPhoneID" )]
        public virtual PhoneNumber BillToPhone { get; set; }

        /// <summary>
        /// The address to which the order is being shipped
        /// </summary>
        [BelongsTo( "ShipToAddressID" )]
        public virtual Address ShipToAddress { get; set; }

        /// <summary>
        /// Shipping phone number
        /// </summary>
        [BelongsTo( "ShipToPhoneID" )]
        public virtual PhoneNumber ShipToPhone { get; set; }

        /// <summary>
        /// The address to which the order is being billed
        /// </summary>
        [BelongsTo( "BillToAddressID" )]
        public virtual Address BillToAddress { get; set; }

        [BelongsTo( "ShipTypeID" )]
        public virtual ShipType ShipType { get; set; }
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
    }
}