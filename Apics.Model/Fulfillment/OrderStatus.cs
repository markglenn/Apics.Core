using System;
using System.Diagnostics;
using System.Linq;
using Apics.Data;
using Castle.ActiveRecord;

namespace Apics.Model.Fulfillment
{
    [ActiveRecord( "OrderStatusType" )]
    [DebuggerDisplay( "Status: {Name.Trim()}" )]
    public class OrderStatus
    {
        [PrimaryKey]
        public virtual int Id { get; set; }

        /// <summary>
        /// Name of the order status
        /// </summary>
        [Property( Length = 50, NotNull = true )]
        public virtual string Name { get; set; }

        /// <summary>
        /// Description of the status
        /// </summary>
        [Property( Length = 255 )]
        public virtual string Description { get; set; }
    }

    /// <summary>
    /// Order status queries
    /// </summary>
    public static class OrderStatusQueries
    {
        /// <summary>
        /// Gets a proxy for the order status by name
        /// </summary>
        /// <param name="statuses">Status repository</param>
        /// <param name="name">Name to find</param>
        /// <returns>Status that matches the given name</returns>
        public static OrderStatus GetByName( this IRepository<OrderStatus> statuses, string name )
        {
            return statuses.GetProxy( 
                statuses.Where( s => s.Name == name ).Select( s => s.Id ).First( ) );
        }
    }
}