using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Castle.ActiveRecord;

namespace Apics.Model.Fulfillment
{
    [ActiveRecord( Lazy = true )]
    [DebuggerDisplay( "Product: {Name}" )]
    public class Product
    {
        [PrimaryKey]
        public virtual int Id { get; set; }

        [Property( Length = 50, NotNull = true )]
        public virtual string Name { get; set; }

        [Property]
        public virtual string Description { get; set; }

        /// <summary>
        /// Parent product
        /// </summary>
        [BelongsTo( "ParentID", Lazy = FetchWhen.OnInvoke )]
        public virtual Product Parent { get; set; }

        /// <summary>
        /// Child products
        /// </summary>
        [HasMany( Lazy = true )]
        public virtual IList<Product> ChildProducts { get; set; }

        [BelongsTo( "CategoryID", NotNull = true, Lazy = FetchWhen.OnInvoke )]
        public virtual ProductCategory Category { get; set; }

        [BelongsTo( "DistributionTypeID", Lazy = FetchWhen.OnInvoke )]
        public virtual DistributionType DistributionType { get; set; }

        [Property( Length = 50 )]
        public virtual string Code { get; set; }

        [BelongsTo( "ProductTypeID", Lazy = FetchWhen.OnInvoke )]
        public virtual ProductType ProductType { get; set; }

        [Property]
        public virtual DateTime DateAvailable { get; set; }

        [Property]
        public virtual bool IsSold { get; set; }

        [HasMany( Lazy = true )]
        public IList<ProductPrice> ProductPrices { get; set; }

    }
}