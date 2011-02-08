using System;
using System.Linq;
using Castle.ActiveRecord;
using System.Collections.Generic;

namespace Apics.Model.Location
{
    [ActiveRecord( Lazy = true )]
    [JoinedTable( "APICSCountry", Column = "CountryID" )]
    public class Country
    {
        [PrimaryKey]
        public virtual int Id { get; set; }

        [Property( Column = "Country", Length = 100, NotNull = true )]
        public virtual string Name { get; set; }

        [Property( NotNull = true, Length = 5 )]
        public virtual string TelephoneCode { get; set; }

        [Property( Column = "ISOCode" )]
        public virtual string Iso2Code { get; set; }

        [Property( "ISO3Code", Table = "APICSCountry" )]
        public virtual string Iso3Code { get; set; }
    }
}
