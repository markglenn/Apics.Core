using System;
using System.Linq;
using Castle.ActiveRecord;

namespace Apics.Model.Location
{
    [ActiveRecord( Lazy = true )]
    public class AddressType
    {
        [PrimaryKey]
        public virtual int Id { get; set; }

        [Property( Length = 50, NotNull = true )]
        public virtual string Name { get; set; }

        [Property( Length = 2000 )]
        public virtual string Description { get; set; }
    }
}