using System;
using System.Linq;
using Apics.Data.AptifyAdapter.Mapping.Metadata;

namespace Apics.Data.AptifyAdapter.Mapping
{
    /// <summary>
    /// Stores the mapping to an entity child
    /// </summary>
    internal class AptifyChildEntity
    {
        private readonly string aptifyName;
        private readonly AptifyEntityMetadata entity;
        private readonly string propertyName;

        #region [ Public Properties ]

        /// <summary>
        /// Child entity
        /// </summary>
        internal AptifyEntityMetadata Entity
        {
            get { return this.entity; }
        }

        /// <summary>
        /// Column name used in Aptify entity
        /// </summary>
        internal string AptifyName
        {
            get { return this.aptifyName; }
        }

        /// <summary>
        /// Property name used in the ActiveRecord model
        /// </summary>
        internal string PropertyName
        {
            get { return this.propertyName; }
        }

        #endregion [ Public Properties ]

        /// <summary>
        /// Creates an entity child mapping
        /// </summary>
        /// <param name="entity">Child entity</param>
        /// <param name="aptifyName">Column name used in the Aptify entity</param>
        /// <param name="propertyName">Property name used in the ActiveRecord model</param>
        internal AptifyChildEntity( AptifyEntityMetadata entity, string aptifyName, string propertyName )
        {
            if( entity == null )
                throw new ArgumentNullException( "entity" );

            if( String.IsNullOrEmpty( aptifyName ) )
                throw new ArgumentException( "aptifyName cannot be null or empty string" );

            if( String.IsNullOrEmpty( propertyName ) )
                throw new ArgumentException( "propertyName cannot be null or empty string" );

            this.entity = entity;
            this.aptifyName = aptifyName;
            this.propertyName = propertyName;
        }
    }
}