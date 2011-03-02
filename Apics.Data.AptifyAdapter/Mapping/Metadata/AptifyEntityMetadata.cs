using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Apics.Data.AptifyAdapter.Mapping.Metadata
{
    /// <summary>
    /// Contains the simple metadata of an Aptify entity
    /// </summary>
    [DebuggerDisplay( "{Name}" )]
    public class AptifyEntityMetadata
    {
        #region [ Private Members ]

        private readonly IList<AptifyChildEntity> children =
            new List<AptifyChildEntity>( );

        private readonly int id;
        private readonly string name;

        private readonly IDictionary<string, AptifyTableMetadata> tables =
            new Dictionary<string, AptifyTableMetadata>( );

        #endregion [ Private Members ]

        #region [ Public Properties ]

        /// <summary>
        /// The ID of the aptify entity
        /// </summary>
        internal int Id
        {
            get { return this.id; }
        }

        /// <summary>
        /// The name of the entity
        /// </summary>
        internal string Name
        {
            get { return this.name; }
        }

        /// <summary>
        /// An enumerable of all the tables defined by this entity
        /// </summary>
        internal IEnumerable<AptifyTableMetadata> Tables
        {
            get { return this.tables.Values; }
        }

        /// <summary>
        /// Parent entity
        /// </summary>
        internal AptifyEntityMetadata Parent { get; set; }

        /// <summary>
        /// All the child entities of this parent
        /// </summary>
        internal IEnumerable<AptifyChildEntity> Children
        {
            get { return this.children; }
        }

        #endregion [ Public Properties ]

        internal AptifyEntityMetadata( int id, string name )
        {
            this.id = id;
            this.name = name;

            this.children = new List<AptifyChildEntity>( );
        }

        internal void AddTable( AptifyTableMetadata table )
        {
            this.tables.Add( table.Name, table );
        }

        internal AptifyTableMetadata GetTable( string tableName )
        {
            return this.tables[ tableName ];
        }

        internal void AddChild( AptifyEntityMetadata child, string propertyName )
        {
            this.children.Add( new AptifyChildEntity( child, child.Name, propertyName ) );
        }

        #region [ Object Overrides ]

        public override string ToString( )
        {
            return Name;
        }

        #endregion [ Object Overrides ]
    }
}