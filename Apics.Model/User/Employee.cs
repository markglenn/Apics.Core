using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;
using System.Diagnostics;

namespace Apics.Model.User
{
    [ActiveRecord( Lazy = true )]
    [DebuggerDisplay( "Employee: {Id}" )]
    public class Employee
    {
        [PrimaryKey]
        public virtual int Id { get; set; }

        [Property( NotNull = true, Length = 5 )]
        public virtual string Prefix { get; set; }

        [Property( NotNull = true, Length = 20 )]
        public virtual string FirstName { get; set; }

        [Property( NotNull = true, Length = 20 )]
        public virtual string MiddleName { get; set; }

        [Property( NotNull = true, Length = 25 )]
        public virtual string LastName { get; set; }

        [Property( NotNull = true, Length = 50 )]
        public virtual string Title { get; set; }

        [BelongsTo( "OrganizationID", NotNull = true, Lazy = FetchWhen.OnInvoke )]
        public virtual Organization Organization { get; set; }

        [Property( NotNull = true, Length = 5 )]
        public virtual string Suffix { get; set; }

    }

}
