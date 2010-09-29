using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Apics.Utilities.Messaging;
using Moq;
using NUnit.Framework;
using System.Messaging;

namespace Apics.Utilities.Tests.Messaging
{
    [DataContract]
    public class MockMessage : IMessage
    {
        #region Implementation of IMessage

        [DataMember]
        public string MessageId
        {
            get { return this.GetType( ).AssemblyQualifiedName; }
            set { }
        }

        #endregion
    }

    [TestFixture]
    public class JsonMessageFormatterTests
    {
        private readonly string jsonValue = string.Format( @"{{""MessageId"":""{0}""}}", typeof( MockMessage ).AssemblyQualifiedName );
        private readonly JsonMessageFormatter formatter = new JsonMessageFormatter( );

        [Test]
        public void CanRead_TrueIfTheMessageHasBody( )
        {
            Assert.IsTrue(
                formatter.CanRead( new Message {
                    BodyStream = new MemoryStream( Encoding.UTF8.GetBytes( "test" ) )
                } ) );
        }

        [Test]
        public void Clone_CreatesNewFormatter( )
        {
            var clone = formatter.Clone( );
            
            Assert.IsInstanceOf( typeof( JsonMessageFormatter ), clone );
        }

        [Test]
        public void Read_ReturnsMessageOfProperType( )
        {
            var message =
                formatter.Read( new Message {
                    BodyStream = new MemoryStream( Encoding.UTF8.GetBytes( jsonValue ) ),
                    Label = typeof( MockMessage ).AssemblyQualifiedName ?? String.Empty
                } );

            Assert.IsInstanceOf<MockMessage>( message );
        }

        [Test]
        public void Read_ThrowsExceptionOnBadData( )
        {
            var message = new Message {
                BodyStream = new MemoryStream( Encoding.UTF8.GetBytes( "{" ) )
            };

            Assert.Throws<InvalidDataException>( ( ) => formatter.Read( message ) );

            message.BodyStream = new MemoryStream( Encoding.UTF8.GetBytes( "{\"MessageId\":\"invalid.type\"}" ) );

            Assert.Throws<InvalidDataException>( ( ) => formatter.Read( message ) );
        }

        [Test]
        public void Write_SerializesMessage( )
        {
            var message = new Message( );

            formatter.Write( message, new MockMessage( ) );
            
            using ( var reader = new StreamReader( message.BodyStream ) )
                Assert.AreEqual( jsonValue, reader.ReadToEnd( ) );
        }
    }
}
