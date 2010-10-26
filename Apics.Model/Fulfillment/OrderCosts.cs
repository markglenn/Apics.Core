using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;

namespace Apics.Model.Fulfillment
{
    [ActiveRecord( "OrderTotal" )]
    public class OrderCosts
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

        [Property( "CALC_SubTotal" )]
        public virtual decimal SubTotal { get; set; }

        [Property( "CALC_SalesTax" )]
        public virtual decimal SalesTax { get; set; }

        [Property( "CALC_GrandTotal" )]
        public virtual decimal GrandTotal { get; set; }
    }
}
