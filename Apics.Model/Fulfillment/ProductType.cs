using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Castle.ActiveRecord;

namespace Apics.Model.Fulfillment
{
    [ActiveRecord( Lazy = true )]
    [DebuggerDisplay( "Product Category: {Name}" )]
    public class ProductType
    {
        [PrimaryKey]
        public virtual int Id { get; set; }

        [Property( Length = 100, NotNull = true )]
        public virtual string Name { get; set; }

        [HasMany]
        public virtual IList<Product> Products { get; set; }

        [Property]
        public virtual string Description { get; set; }
    }
}