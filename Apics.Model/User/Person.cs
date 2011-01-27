using System;
using System.Collections.Generic;
using System.Linq;
using Castle.ActiveRecord;
using Apics.Model.Location;
using System.ComponentModel;
using Apics.Utilities.Extension;
using Apics.Model.Certification;

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
        private readonly IEnumerable<Enum> values;
        private readonly IDictionary<Enum, string> descriptions;

        public DescribedEnumStringType( )
        {
            this.values = Enum.GetValues( typeof( T ) ).Cast<Enum>( );

            this.descriptions = this.values.ToDictionary( v => v, v => v.GetDescription( ) );
        }

        public override object GetValue( object code )
        {
            if ( code == null )
                return String.Empty;

            var type = typeof( T );
            var name = Enum.GetName( type, code );
            var enumeration = ( Enum )Enum.Parse( type, name );

            return enumeration.GetDescription( );
        }

        public override object GetInstance( object code )
        {
            if ( code == null )
                return default( T );

            string enumString = code.ToString( ).Trim( );

            return this.descriptions.Where( v => v.Value == enumString ).Select( v => v.Key )
                .SingleOrDefault( ) ?? this.values.First( );
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

        [BelongsTo( "MemberTypeID", Lazy = FetchWhen.OnInvoke )]
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

        [HasMany]
        public virtual IList<PersonSubmission> Submissions { get; set; }

    }

    public class PersonWithPreferredAddress
    {
        public Person Person;
        public int AddressId;
    }

    public static class PersonQueries
    {
        public static IQueryable<PersonWithPreferredAddress> WithPreferredAddresses( this IEnumerable<Person> people )
        {
            return people.AsQueryable( )
                .Select( p => new PersonWithPreferredAddress
                {
                    Person = p,
                    AddressId = (
                        p.PreferredAddress == PreferredAddress.Home && p.HomeAddress != null ? p.HomeAddress.Id :
                        p.PreferredAddress == PreferredAddress.Business && p.Address != null ? p.Address.Id :
                        p.BillingAddress != null ? p.BillingAddress.Id :
                        p.HomeAddress != null ? p.HomeAddress.Id :
                        p.Address != null ? p.Address.Id : -1 )
                } );
        }
    }
}
