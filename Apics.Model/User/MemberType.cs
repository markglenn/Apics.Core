using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;

namespace Apics.Model.User
{
    [ActiveRecord(Lazy = true)]
    public class MemberType
    {
        [PrimaryKey]
        public virtual int Id { get; set; }

        [Property]
        public virtual string Name { get; set; }

        //[HasMany( Lazy = true, Table = "Persons", ColumnKey = "MemberTypeID" )]
        //public virtual IList<Person> Members { get; set; }
    }
}
