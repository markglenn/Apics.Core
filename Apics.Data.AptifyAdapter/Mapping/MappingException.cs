using System;
using System.Linq;
using System.Runtime.Serialization;

namespace Apics.Data.AptifyAdapter.Mapping
{
    [Serializable]
    public abstract class MappingException : Exception
    {
        protected MappingException( )
        {
        }

        protected MappingException( string message )
            : base( message )
        {
        }

        protected MappingException( string message, Exception innerException )
            : base( message, innerException )
        {
        }

        protected MappingException( SerializationInfo info, StreamingContext context )
            : base( info, context )
        {
        }
    }
}