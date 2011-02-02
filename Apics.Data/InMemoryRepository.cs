using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Apics.Data
{
    public class InMemoryRepository<T> : IRepository<T> where T : class
    {
        private List<T> storage = new List<T>( );
        
        public InMemoryRepository( )
        {

        }

        public InMemoryRepository( IEnumerable<T> items )
        {
            this.storage.AddRange( items );
        }

        #region IRepository<T> Members

        public void Insert( T entity )
        {
            this.storage.Add( entity );
        }

        public void Update( T entity )
        {
        }

        public void Delete( T entity )
        {
            this.storage.Remove( entity );
        }

        public void Evict( T entity )
        {
            
        }

        public void Refresh( T entity )
        {
            
        }

        public void DeleteAll( )
        {
            this.storage.Clear( );
        }

        public T GetProxy( object id )
        {
            throw new NotSupportedException( );
        }

        public void InsertOrUpdate( T entity )
        {
            if ( !this.storage.Contains( entity ) )
                this.storage.Add( entity );
        }

        #endregion

        #region IRepository Members

        public void Insert( object entity )
        {
            this.Insert( ( T )entity );
        }

        public void Update( object entity )
        {
            this.Update( ( T )entity );   
        }

        public void Delete( object entity )
        {
            this.Delete( ( T )entity );   
        }

        public void Evict( object entity )
        {
            this.Evict( ( T )entity );
        }

        public object GetById( object id )
        {
            throw new NotSupportedException( );
        }

        #endregion

        #region IQueryable Members

        public Type ElementType
        {
            get { return typeof( T ); }
        }

        public Expression Expression
        {
            get { return this.storage.AsQueryable( ).Expression; }
        }

        public IQueryProvider Provider
        {
            get { return this.storage.AsQueryable( ).Provider; }
        }

        #endregion

        #region IEnumerable Members

        public System.Collections.IEnumerator GetEnumerator( )
        {
            return this.storage.GetEnumerator( );
        }

        #endregion

        #region IEnumerable<T> Members

        IEnumerator<T> IEnumerable<T>.GetEnumerator( )
        {
            return this.storage.GetEnumerator( );
        }

        #endregion

        #region IRepository<T> Members


        #endregion
    }
}
