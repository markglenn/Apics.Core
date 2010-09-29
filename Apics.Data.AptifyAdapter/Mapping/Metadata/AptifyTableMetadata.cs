using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Apics.Data.AptifyAdapter.Mapping.Metadata
{
    /// <summary>
    /// Metadata for the database table used to map to an Aptify entity
    /// </summary>
    [DebuggerDisplay( "{Name} of {entity.Name}" )]
    public class AptifyTableMetadata
    {
        private readonly IDictionary<string, AptifyColumnMetadata> columns =
            new Dictionary<string, AptifyColumnMetadata>( );

        private readonly AptifyEntityMetadata entity;
        private readonly string name;

        #region [ Public Properties ]

        internal string Name
        {
            get { return this.name; }
        }

        internal IDictionary<string, AptifyColumnMetadata> Columns
        {
            get { return this.columns; }
        }

        internal AptifyEntityMetadata Entity
        {
            get { return this.entity; }
        }

        #endregion [ Public Properties ]

        internal AptifyTableMetadata( AptifyEntityMetadata entity, string name )
        {
            if( entity == null )
                throw new ArgumentNullException( "entity" );

            if( String.IsNullOrEmpty( name ) )
                throw new ArgumentException( "Table name cannot be null or empty" );

            this.name = name;
            this.entity = entity;
        }

        internal void AddColumn( AptifyColumnMetadata column )
        {
            this.columns.Add( column.Name, column );
        }

        #region [ Object Overrides ]

        public override string ToString( )
        {
            return Name;
        }

        #endregion [ Object Overrides ]
    }
}