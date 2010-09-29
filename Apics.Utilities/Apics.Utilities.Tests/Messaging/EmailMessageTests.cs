using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;
using Apics.Utilities.Messaging;
using NUnit.Framework;
using System.Net.Mail;
using System.Messaging;

namespace Apics.Utilities.Tests.Messaging
{
    [TestFixture]
    public class EmailMessageTests
    {
        [Test]
        public void Ctor_SetsMessageIdToTypeName( )
        {
            var message = new EmailMessage( );

            Assert.AreEqual( typeof( EmailMessage ).ToString( ), message.MessageId );
        }

        [Test]
        public void MessageIsSerializable( )
        {
            var message = new EmailMessage {
                FromAddress = new EmailAddress( "fromaddress@example.com" ),
                Subject = "Example message subject",
                Template = "test template.email"
            };

            message.ToAddress.Add( new EmailAddress( "toaddress@example.com", "To Address" ) );
            message.CcAddresses.Add( new EmailAddress( "ccaddress@example.com" ) );

            var serializer = new DataContractJsonSerializer( typeof( EmailMessage ) );
            var stream = new MemoryStream( );
            
            serializer.WriteObject( stream, message );

            stream.Seek( 0, SeekOrigin.Begin );

            Assert.AreEqual( message, serializer.ReadObject( stream ) );
        }

    }

    [TestFixture]
    public class EmailAddressTests
    {
        [Test]
        public void Ctor_SetsEmailAddress( )
        {
            var address = new EmailAddress( "test@example.com" );
            Assert.AreEqual( "test@example.com", address.Address );
            Assert.AreEqual( String.Empty, address.DisplayName );
        }

        [Test]
        public void Ctor_SetsEmailAddressAndDisplayName( )
        {
            var address = new EmailAddress( "test@example.com", "Mark Glenn" );

            Assert.AreEqual( "test@example.com", address.Address );
            Assert.AreEqual( "Mark Glenn", address.DisplayName );
        }

        [Test]
        public void Ctor_NullEmailAddress_ThrowsArgumentNullException( )
        {
            Assert.Throws<ArgumentNullException>( ( ) => new EmailAddress( null ) );
            Assert.Throws<ArgumentNullException>( ( ) => new EmailAddress( null, String.Empty ) );
        }

        [Test]
        public void ImplicitConversionToMailAddress( )
        {
            var address = new EmailAddress( "test@example.com", "Test User" );

            var mailAddress = ( MailAddress )address;

            Assert.AreEqual( address.Address, mailAddress.Address );
            Assert.AreEqual( address.DisplayName, mailAddress.DisplayName );
        }

        [Test]
        public void Equals_ChecksForEmailEquality( )
        {
            var email1 = new EmailAddress( "test1@example.com" );
            var email2 = new EmailAddress( "test2@example.com" );
            var email3 = new EmailAddress( "test1@example.com");
            var email4 = new EmailAddress( "test1@example.com", "Different name" );

            Assert.AreEqual( email1, email1 );
            Assert.AreNotEqual( email1, email2 );
            Assert.AreEqual( email1, email3 );
            Assert.AreNotEqual( email1, email4 );
        }
    }

    [TestFixture]
    internal class EmailAddressCollectionTests
    {
        [Test]
        public void ImplicitConversionToMailAddressCollection( )
        {
            var addresses = new EmailAddressCollection {
                new EmailAddress( "test@example.com" ),
                new EmailAddress( "test@example.com", "Test User" )
            };

            var mailAddresses = ( MailAddressCollection )addresses;

            Assert.AreEqual( 2, addresses.Count );

            for( int i = 0; i < addresses.Count; ++i )
                Assert.AreEqual( mailAddresses[ i ], ( MailAddress )addresses[ i ] );
        }

        [Test]
        public void Equals_ChecksForEmailEquality( )
        {
            var addresses1 = new EmailAddressCollection {
                new EmailAddress( "test@example.com" )
            };

            var addresses2 = new EmailAddressCollection {
                new EmailAddress( "test@example.com" )
            };

            Assert.AreEqual( addresses1, addresses2 );

            addresses2.Add( new EmailAddress( "another@example.com" ) );
            Assert.AreNotEqual( addresses1, addresses2 );
        }
    }

}
