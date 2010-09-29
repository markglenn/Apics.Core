using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;

namespace Apics.Model.User
{
    [ActiveRecord( Lazy = true )]
    public class PhoneNumber
    {
        [PrimaryKey]
        public virtual int Id { get; set; }

        [Property( Length = 5, NotNull = true, ColumnType = "Apics.Data.UserType.NChar, Apics.Data" )]
        public virtual string CountryCode { get; set; }

        [Property( Length = 5, NotNull = true, ColumnType = "Apics.Data.UserType.NChar, Apics.Data" )]
        public virtual string AreaCode { get; set; }

        [Property( Length = 15, NotNull = true, ColumnType = "Apics.Data.UserType.NChar, Apics.Data" )]
        public virtual string Phone { get; set; }

        [Property( "PhoneExtension", Length = 10, NotNull = true, ColumnType = "Apics.Data.UserType.NChar, Apics.Data" )]
        public virtual string Extension { get; set; }
    }
}
