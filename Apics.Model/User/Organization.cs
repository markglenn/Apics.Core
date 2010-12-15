using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;
using Apics.Model.Location;
using System.Diagnostics;

namespace Apics.Model.User
{
    [ActiveRecord( Lazy = true )]
    [DebuggerDisplay( "Organization: {Id}" )]
    public class Organization
    {
        [PrimaryKey]
        public virtual int Id { get; set; }

        [Property( NotNull = true, Length = 255 )]
        public virtual string Name { get; set; }

        [BelongsTo( "ParentID", Lazy = FetchWhen.OnInvoke )]
        public virtual Organization Parent { get; set; }

        [BelongsTo( "AddressID", Lazy = FetchWhen.OnInvoke )]
        public virtual Address Address { get; set; }

        [BelongsTo( "BillingAddressID", Lazy = FetchWhen.OnInvoke )]
        public virtual Address BillingAddress { get; set; }

        [BelongsTo( "MainPhoneID", Lazy = FetchWhen.OnInvoke )]
        public virtual PhoneNumber MainPhone { get; set; }

    }

}
