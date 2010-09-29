using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Castle.ActiveRecord;

namespace Apics.Model.Fulfillment
{
    [ActiveRecord( Lazy = true )]
    [DebuggerDisplay( "Distribution Type: {Name.Trim( )}" )]
    public class DistributionType
    {
        [PrimaryKey]
        public virtual int Id { get; set; }

        [Property( Length = 50, NotNull = true )]
        public virtual string Name { get; set; }

        [Property]
        public virtual string Description { get; set; }

        [HasMany]
        public virtual IList<Product> Products { get; set; }
    }
}