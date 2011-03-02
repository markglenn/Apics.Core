using System;
using System.Diagnostics;
using System.Linq;

namespace Apics.Data.AptifyAdapter.Mapping.Metadata
{
    /// <summary>
    /// General column metadata
    /// </summary>
    [DebuggerDisplay( "{columnName}" )]
    internal class AptifyColumnMetadata
    {
        private readonly string columnName;
        private readonly AptifyEntityMetadata joinEntity;
        private readonly bool nullable;
        private readonly bool isEmbedded;

        #region [ Public Properties ]

        /// <summary>
        /// Original name of the aptify column
        /// </summary>
        internal string Name
        {
            get { return this.columnName; }
        }

        /// <summary>
        /// Is this column nullable
        /// </summary>
        internal bool Nullable
        {
            get { return this.nullable; }
        }

        /// <summary>
        /// Name of the column if it is a foreign key
        /// </summary>
        internal AptifyEntityMetadata JoinEntity
        {
            get { return this.joinEntity; }
        }

        /// <summary>
        /// Is this column a foreign key
        /// </summary>
        internal bool IsForeignKeyColumn
        {
            get { return this.joinEntity != null; }
        }

        /// <summary>
        /// Is this column an embedded entity?
        /// </summary>
        internal bool IsEmbedded
        {
            get { return this.isEmbedded; }
        }

        #endregion [ Public Properties ]

        internal AptifyColumnMetadata( string column, bool nullable, AptifyEntityMetadata joinEntity, bool isEmbedded )
        {
            if( String.IsNullOrEmpty( column ) )
                throw new ArgumentException( "Table column cannot be set to a null or empty string" );

            this.columnName = column;
            this.nullable = nullable;

            // Used for foreign key columns
            this.joinEntity = joinEntity;
            this.isEmbedded = isEmbedded;
        }

        internal AptifyColumnMetadata( string column, bool nullable )
            : this( column, nullable, null, false )
        {
        }
    }
}