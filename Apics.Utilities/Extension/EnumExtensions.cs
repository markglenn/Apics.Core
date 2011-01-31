using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Apics.Utilities.Extension
{
    public static class EnumExtensions
    {
        public static string GetDescription( this Enum enumeration )
        {
            return enumeration.GetType( )
               .GetField( ( enumeration ).ToString( ) )
               .GetCustomAttributes( typeof( DescriptionAttribute ), false )
               .Select( a => ( ( DescriptionAttribute )a ).Description )
               .FirstOrDefault( );
        }

        public static T AsEnum<T>( this string value )
        {
            return ( T )Enum.Parse( typeof( T ), value, true );
        }
    }
}
