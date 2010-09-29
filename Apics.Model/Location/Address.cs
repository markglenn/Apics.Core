using System;
using System.Linq;
using Castle.ActiveRecord;

namespace Apics.Model.Location
{
    [ActiveRecord( Lazy = true )]
    public class Address
    {
        [PrimaryKey]
        public virtual int Id { get; set; }

        [BelongsTo( "AddressTypeID" )]
        public virtual AddressType AddressType { get; set; }

        [Property( NotNull = true )]
        public virtual string Line1 { get; set; }

        [Property]
        public virtual string Line2 { get; set; }

        [Property]
        public virtual string Line3 { get; set; }

        [Property]
        public virtual string Line4 { get; set; }

        [Property( Length = 50 )]
        public virtual string City { get; set; }

        [Property( Length = 100 )]
        public virtual string County { get; set; }

        [Property( Length = 30 )]
        public virtual string StateProvince { get; set; }

        [Property( Length = 25 )]
        public virtual string PostalCode { get; set; }

        [BelongsTo( "CountryCodeID" )]
        public virtual Country Country { get; set; }
    }
}
