using Castle.ActiveRecord;

namespace Apics.Model.Fulfillment
{
    /// <summary>
    /// The state of the order
    /// </summary>
    [ActiveRecord( Lazy = true )]
    public class OrderState
    {
        /// <summary>
        /// The order ID
        /// </summary>
        [PrimaryKey]
        public virtual int Id { get; set; }

        /// <summary>
        /// The attached order
        /// </summary>
        [OneToOne]
        public virtual Order Order { get; set; }

        /// <summary>
        /// Is the order being drop shipped?
        /// </summary>
        [Property( NotNull = true )]
        public virtual bool DropShip { get; set; }

        /// <summary>
        /// Order status
        /// </summary>
        [BelongsTo( "OrderStatusID", NotNull = true )]
        public virtual OrderStatus Status { get; set; }

        [BelongsTo( "OrderTypeID", NotNull = true )]
        public virtual OrderType Type { get; set; }
    }
}