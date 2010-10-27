using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Apics.Data.Validation
{
    public static class ValidationHelper
    {
        /// <summary>
        /// Gets the violations of data rules of an instance
        /// </summary>
        /// <param name="instance">Instance of an object</param>
        /// <returns>An enumerable of violations</returns>
        public static IEnumerable<Violation> GetViolations( object instance )
        {
            return 
                from property in TypeDescriptor.GetProperties( instance ).Cast<PropertyDescriptor>( )
                from attribute in property.Attributes.OfType<ValidationAttribute>( )
                where !attribute.IsValid( property.GetValue( instance ) )
                select new Violation( property.Name, attribute.FormatErrorMessage( property.Name ), instance );
        }

        /// <summary>
        /// Returns true if the object is valid
        /// </summary>
        /// <param name="instance">Instance to check for validity</param>
        /// <returns>True if the instance is valid</returns>
        public static bool IsValid( object instance )
        {
            return !GetViolations( instance ).Any( );
        }
    }
}
