using System;
using System.Linq;
using System.Reflection;
using Castle.ActiveRecord.Framework.Internal;

namespace Apics.Data.AptifyAdapter.Mapping
{
    [Serializable]
    public class HasManyMappingException : MappingException
    {
        #region [ Public Properties ]

        public HasManyModel Model { get; private set; }

        public Type ParentType { get; private set; }

        public PropertyInfo Property { get; private set; }

        #endregion [ Public Properties ]

        public HasManyMappingException( string message, HasManyModel model )
            : base( message )
        {
            Model = model;
            ParentType = model.ContainingTypeModel.Type;
            Property = model.Property;
        }

        public HasManyMappingException( HasManyModel model )
            : this( "Could not map HasMany property", model )
        {
        }
    }
}