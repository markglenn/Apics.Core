using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apics.Utilities.Extension;

namespace Apics.Model
{
    public class DescribedEnumStringType<T> : NHibernate.Type.EnumStringType<T> where T : struct
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
}
