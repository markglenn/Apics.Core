using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Castle.ActiveRecord;

namespace Apics.Model.Fulfillment
{
    [ActiveRecord( "OrderDetail", Lazy = true )]
    public class OrderItem
    {
        [PrimaryKey]
        public virtual int Id { get; set; }

        [BelongsTo( "OrderID" )]
        public virtual Order Order { get; set; }

        [Property]
        public virtual string Comments { get; set; }

        [BelongsTo( "ProductID", Lazy = FetchWhen.OnInvoke )]
        public virtual Product Product { get; set; }

        [Property( NotNull = true )]
        public virtual decimal Price { get; set; }

        [Property( NotNull = true )]
        public virtual int Quantity { get; set; }

        [Property( NotNull = true )]
        public virtual int QuantityShipped { get; set; }

        [Property( NotNull = true )]
        public virtual int Sequence { get; set; }

        [Property( NotNull = true )]
        public virtual int ParentSequence { get; set; }
    }

    public static class OrderItemQueries
    {
        public static IQueryable<OrderItem> Shippable( this IEnumerable<OrderItem> items )
        {
            return items.AsQueryable( )
                .Where( i => i.Product.DistributionType.Name == "Hard Shipment" )
                .Where( i => i.Product.Parent == null )
                .Where( i => i.Product.Code != String.Empty );
        }
    }
}