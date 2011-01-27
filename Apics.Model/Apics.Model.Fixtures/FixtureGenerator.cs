using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture;
using Apics.Model.User;
using Apics.Model.Location;

namespace Apics.Model.Fixtures
{
    public static class FixtureGenerator
    {
        public static Fixture Create( )
        {
            var fixture = new Fixture( );

            var homeAddressType = fixture.Build<AddressType>( )
                .With( t => t.Name, "Home Address" )
                .CreateAnonymous( );

            var businessAddressType = fixture.Build<AddressType>( )
                .With( t => t.Name, "Business Address" )
                .CreateAnonymous( );

            var billingAddressType = fixture.Build<AddressType>( )
                .With( t => t.Name, "Billing Address" )
                .CreateAnonymous( );

            fixture.Customize<MemberType>(
                mt => mt.Without( x => x.Parent )
            );

            fixture.Customize<Company>( c => c
                .Without( x => x.ParentCompany )
                .Without( x => x.RootCompany )
            );

            fixture.Customize<Person>( p => p
                .With( x => x.HomeAddress, fixture.Build<Address>( )
                    .With( a => a.AddressType, homeAddressType )
                    .CreateAnonymous( ) )
                .With( x => x.Address, fixture.Build<Address>( )
                    .With( a => a.AddressType, businessAddressType )
                    .CreateAnonymous( ) )
                .With( x => x.BillingAddress, fixture.Build<Address>( )
                    .With( a => a.AddressType, billingAddressType )
                    .CreateAnonymous( ) ) );

            return fixture;
        }
    }
}
