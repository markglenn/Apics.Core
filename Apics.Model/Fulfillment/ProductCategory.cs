using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Castle.ActiveRecord;

namespace Apics.Model.Fulfillment
{
    [ActiveRecord( Lazy = true )]
    [DebuggerDisplay( "Category: {Name}" )]
    public class ProductCategory
    {
        [PrimaryKey]
        public virtual int Id { get; set; }

        [Property]
        public virtual string Name { get; set; }

        [HasMany]
        public virtual IList<Product> Products { get; set; }

        [Property]
        public virtual string Description { get; set; }

        [BelongsTo( "ParentID" )]
        public virtual ProductCategory Parent { get; set; }

        [HasMany]
        public virtual IList<ProductCategory> Children { get; set; }
    }
}