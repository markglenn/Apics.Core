using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Apics.Utilities.Validation
{
    [AttributeUsage( AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true )]
    public sealed class RequiredValueAttribute : ValidationAttribute
    {
        #region [ Private Members ]

        private readonly object value;
        
        #endregion [ Private Members ]

        public RequiredValueAttribute( object value )
        {
            this.value = value;
        }

        public override bool IsValid( object value )
        {
            return value.Equals( this.value );
        }
    }
}
