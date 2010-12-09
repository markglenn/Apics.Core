using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Apics.Utilities.Security;

namespace Apics.Utilities.Tests.Security
{
    [TestFixture]
    public class HmacUrlTests
    {
        [Test]
        public void Ctor_NullOrEmptyKeyThrowsArgumentException( )
        {
            Assert.Throws<ArgumentException>( ( ) => new HmacUrlAuthorization( null ) );
            Assert.Throws<ArgumentException>( ( ) => new HmacUrlAuthorization( String.Empty ) );
        }

        [Test]
        public void GenerateLink_WithNoUrlParameters_AddsTimestampAndHmac( )
        {
            var hmac = new HmacUrlAuthorization( "ABCD" );
            var url = new Uri( "http://www.example.com/testurl" );

            var result = hmac.GenerateLink( url ).ToString( );

            Assert.IsTrue( result.StartsWith( url.ToString( ) ) );
            Assert.IsTrue( result.Contains( "?timestamp=" ) );
            Assert.IsTrue( result.Contains( "&hmac=" ) );
        }

        [Test]
        public void GenerateLink_WithParameters_AddsTimestampAndHmac( )
        {
            var hmac = new HmacUrlAuthorization( "ABCD" );
            var url = new Uri( "http://www.example.com/testurl?test=1" );

            var result = hmac.GenerateLink( url ).ToString( );

            Assert.IsTrue( result.StartsWith( url.ToString( ) ) );
            Assert.IsTrue( result.Contains( "&timestamp=" ) );
            Assert.IsTrue( result.Contains( "&hmac=" ) );
        }

        [Test]
        public void ValidateUrl_ValidatesHmacEncodedUrl( )
        {
            var valid = new Uri( "http://www.example.com/testurl?test=1&timestamp=129331882172255018&hmac=ad2c7db6708ed1856412d9824789181bc117f2fb" );
            var invalid = new Uri( "http://www.example.com/testurl?test=1&timestamp=129331882172255018&hmac=ad2c7db6708ed1856412d9824789181bc117f2f" );
            var invalid2 = new Uri( "http://www.example.com" );

            Assert.IsTrue( new HmacUrlAuthorization( "ABCD" ).ValidateUrl( valid ) );
            Assert.IsFalse( new HmacUrlAuthorization( "ABCD" ).ValidateUrl( invalid ) );
            Assert.IsFalse( new HmacUrlAuthorization( "ABCD" ).ValidateUrl( invalid2 ) );
        }

        [Test]
        public void ValidateUrl_NullArgumentThrowsArgumentNullException( )
        {
            Assert.Throws<ArgumentNullException>( ( ) => new HmacUrlAuthorization( "ABCD" ).ValidateUrl( null ) );
        }
    }
}
