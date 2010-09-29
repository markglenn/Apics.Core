using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Apics.Data.AptifyAdapter.Mapping.Metadata
{
    internal class EntityMetadataCollection : ICollection<AptifyEntityMetadata>
    {
        private readonly IList<AptifyEntityMetadata> entities = new List<AptifyEntityMetadata>( );

        #region ICollection<AptifyEntityMetadata> Members

        public void Add( AptifyEntityMetadata item )
        {
            this.entities.Add( item );
        }

        public void Clear( )
        {
            this.entities.Clear( );
        }

        public bool Contains( AptifyEntityMetadata item )
        {
            return this.entities.Contains( item );
        }

        public void CopyTo( AptifyEntityMetadata[ ] array, int arrayIndex )
        {
            this.entities.CopyTo( array, arrayIndex );
        }

        public int Count
        {
            get { return this.entities.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove( AptifyEntityMetadata item )
        {
            return this.entities.Remove( item );
        }

        public IEnumerator<AptifyEntityMetadata> GetEnumerator( )
        {
            return this.entities.GetEnumerator( );
        }

        IEnumerator IEnumerable.GetEnumerator( )
        {
            return GetEnumerator( );
        }

        #endregion

        internal AptifyEntityMetadata GetById( int entityId )
        {
            return this.entities.FirstOrDefault( e => e.Id == entityId );
        }

        internal AptifyEntityMetadata GetByName( string name )
        {
            if( name != null )
                name = name.Trim( );

            return this.entities.FirstOrDefault( e => e.Name == name );
        }
    }
}