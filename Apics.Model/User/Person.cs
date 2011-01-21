using System;
using System.Collections.Generic;
using System.Linq;
using Castle.ActiveRecord;
using Apics.Model.Location;
using System.ComponentModel;
using Apics.Utilities.Extension;

namespace Apics.Model.User
{
    public enum PreferredAddress
    {
        [Description( "Home Address" )]
        Home,
        [Description( "Business Address" )]
        Business,
        [Description( "Billing Address" )]
        Billing
    }

    public class DescribedEnumStringType<T> : NHibernate.Type.EnumStringType<T> where T : struct, IConvertible
    {
        public override object GetValue( object code )
        {
            if ( code == null )
                return String.Empty;

            var type = typeof( T );
            var name = Enum.GetName( type, code );
            var enumeration = ( Enum )Enum.Parse( type, name );

            return enumeration.GetDescription( );
        }

    } 

    [ActiveRecord( Lazy = true )]
    public class Person
    {
        [PrimaryKey]
        public virtual int Id { get; set; }

        [Property]
        public virtual string Prefix { get; set; }

        [Property]
        public virtual string FirstName { get; set; }

        [Property]
        public virtual string MiddleName { get; set; }

        [Property]
        public virtual string LastName { get; set; }

        [BelongsTo( "AddressID", Lazy = FetchWhen.OnInvoke )]
        public virtual Address Address { get; set; }

        [BelongsTo( "BillingAddressID", Lazy = FetchWhen.OnInvoke )]
        public virtual Address BillingAddress { get; set; }

        [BelongsTo( "HomeAddressID", Lazy = FetchWhen.OnInvoke )]
        public virtual Address HomeAddress { get; set; }

        [BelongsTo( "CompanyID", Lazy = FetchWhen.OnInvoke )]
        public virtual Company Company { get; set; }

        [BelongsTo("MemberTypeID", Lazy = FetchWhen.OnInvoke)]
        public virtual MemberType MemberType { get; set; }

        [Property( "PreferredAddress", NotNull = true,
            ColumnType = @"Apics.Model.User.DescribedEnumStringType`1[Apics.Model.User.PreferredAddress], Apics.Model" )]
        public virtual PreferredAddress PreferredAddress { get; set; }

        [Property]
        public virtual string PreferredShippingAddress { get; set; }

        [Property]
        public virtual string PreferredBillingAddress { get; set; }

        [BelongsTo( "PhoneID", Lazy = FetchWhen.OnInvoke )]
        public virtual PhoneNumber Phone { get; set; }

        [Property]
        public virtual string Email1 { get; set; }

        [Property]
        public virtual string WhoCreated { get; set; }

        [Property]
        public virtual string WhoUpdated { get; set; }

        [Property]
        public virtual DateTime DateCreated { get; set; }

        [Property]
        public virtual DateTime DateUpdated { get; set; }

    }
}
