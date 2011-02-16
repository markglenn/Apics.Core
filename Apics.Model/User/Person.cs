using System;
using System.Collections.Generic;
using System.Linq;
using Castle.ActiveRecord;
using Apics.Model.Location;
using System.ComponentModel;
using Apics.Utilities.Extension;
using Apics.Model.Certification;
using Apics.Model.Fulfillment;

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
            ColumnType = @"Apics.Model.DescribedEnumStringType`1[Apics.Model.User.PreferredAddress], Apics.Model" )]
        public virtual PreferredAddress PreferredAddress { get; set; }

        [HasMany( ColumnKey = "BillToID", Lazy = true )]
        public virtual IList<Order> BillToOrders { get; set; }

        [HasMany( ColumnKey = "ShipToID", Lazy = true )]
        public virtual IList<Order> ShipToOrders { get; set; }

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

        [HasMany( Lazy = true )]
        public virtual IList<PersonSubmission> Submissions { get; set; }

        [HasMany( Lazy = true )]
        public virtual IList<ExamCertification> ExamCertifications { get; set; }
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

        public static IQueryable<Person> FromCountries( this IEnumerable<Person> people, IEnumerable<Address> addresses, 
            IEnumerable<Country> countries )
        {
            return from p in people.WithPreferredAddresses( )
                   join a1 in addresses on p.AddressId equals a1.Id
                   where countries.Contains( a1.Country )
                   select p.Person;
        }
    }
}
