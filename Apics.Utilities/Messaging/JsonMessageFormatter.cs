using System;
using System.IO;
using System.Messaging;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Apics.Utilities.Messaging
{
    public class JsonMessageFormatter : IMessageFormatter
    {
        #region [ Implementation of ICloneable ]

        public object Clone( )
        {
            return new JsonMessageFormatter( );
        }

        #endregion [ Implementation of ICloneable ]

        #region [ Implementation of IMessageFormatter ]

        public bool CanRead( Message message )
        {
            return message.BodyStream.CanRead;
        }

        public object Read( Message message )
        {
            if ( !CanRead( message ) )
                return null;

            try
            {
                var serializer = new DataContractJsonSerializer( Type.GetType( message.Label ) );
                return serializer.ReadObject( message.BodyStream );
            }
            catch ( Exception ex )
            {
                throw new InvalidDataException( "Message is in an invalid format", ex );
            }
        }

        public void Write( Message message, object obj )
        {
            if ( message == null )
                throw new ArgumentNullException( "message" );

            var objType = obj.GetType( );
            
            if( objType.AssemblyQualifiedName == null )
                throw new InvalidOperationException( "Object has no type" );
           
            var serializer = new DataContractJsonSerializer( objType );

            message.Label = objType.AssemblyQualifiedName;
            message.BodyStream = new MemoryStream( );

            serializer.WriteObject( message.BodyStream, obj );
            message.BodyStream.Seek( 0, SeekOrigin.Begin );
        }

        #endregion [ Implementation of IMessageFormatter ]
    }
}
