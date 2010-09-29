using System;
using System.Linq;
using Castle.ActiveRecord;

namespace Apics.Model.Location
{
    [ActiveRecord( Lazy = true )]
    public class Country
    {
        [PrimaryKey]
        public virtual int Id { get; set; }

        [Property( Column = "Country", Length = 100, NotNull = true )]
        public virtual string Name { get; set; }

        [Property( Column = "ISOCode" )]
        public virtual string Iso2Code { get; set; }
    }
}
