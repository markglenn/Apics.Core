using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Linq;
using Castle.ActiveRecord.Framework;

namespace Apics.Data.Database
{
	public class DatabaseRepository<T> : IRepository<T> where T : class
	{
		#region [ Private Members ]

		private readonly IQueryable<T> queryable = ActiveRecordLinq.AsQueryable<T>( );

		#endregion [ Private Members ]

		#region [ IRepository<T> Members ]

		public void Insert( T entity )
		{
			if ( entity == null )
				throw new ArgumentNullException( "entity" );
			
			ActiveRecordMediator<T>.Save( entity );
		}

		public void Update( T entity )
		{
			if ( entity == null )
				throw new ArgumentNullException( "entity" );

			ActiveRecordMediator<T>.Update( entity );
		}

        public void InsertOrUpdate( T entity )
        {
            if ( entity == null )
                throw new ArgumentNullException( "entity" );

            ActiveRecordMediator<T>.Save( entity );
        }

		public void Delete( T entity )
		{
			if ( entity == null )
				throw new ArgumentNullException( "entity" );

			ActiveRecordMediator<T>.Delete( entity );
		}

		public void Evict( T entity )
		{
			if ( entity == null )
				throw new ArgumentNullException( "entity" );

			ActiveRecordMediator.Evict( entity );
		}

        public void Refresh( T entity )
        {
            ActiveRecordMediator<T>.Refresh( entity );
        }

		#endregion [ IRepository<T> Members ]

		#region [ IEnumerable<T> Members ]

		public IEnumerator<T> GetEnumerator( )
		{
			return this.queryable.GetEnumerator( );
		}

		#endregion [ IEnumerable<T> Members ]

		#region [ IEnumerable Members ]

		IEnumerator IEnumerable.GetEnumerator( )
		{
			return this.GetEnumerator( );
		}

		#endregion [ IEnumerable Members ]

		#region [ IQueryable Members ]

	    public Type ElementType
		{
			get { return this.queryable.ElementType; }
		}

		public Expression Expression
		{
			get { return this.queryable.Expression; }
		}

		public IQueryProvider Provider
		{
			get { return this.queryable.Provider; }
		}

		#endregion [ IQueryable Members ]

		#region [ IRepository Members ]

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

		public void DeleteAll( )
		{
			ActiveRecordMediator<T>.DeleteAll( );
		}

	    public T GetProxy( object id )
	    {
	        return ActiveRecordMediator<T>.FindByPrimaryKey( id );
	    }

        public object GetById( object id )
        {
            return ActiveRecordMediator<T>.FindByPrimaryKey( id );
        }

        #endregion [ IRepository Members ]
    }

}

