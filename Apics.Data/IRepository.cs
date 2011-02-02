using System;
using System.Linq;

namespace Apics.Data
{
    public interface IRepository : IQueryable
    {
        /// <summary>
        /// Inserts a new entity into the repository
        /// </summary>
        /// <param name="entity">Entity to insert into the repository</param>
        void Insert( object entity );

        /// <summary>
        /// Updates an entity in the repository
        /// </summary>
        /// <param name="entity">Entity to update in the repository</param>
        void Update( object entity );

        /// <summary>
        /// Deletes an entity from the repository
        /// </summary>
        /// <param name="entity">Entity to delete from the repository</param>
        void Delete( object entity );

        /// <summary>
        /// Evicts any changes made to the entity from the session
        /// </summary>
        /// <param name="entity">Entity to evict</param>
        void Evict( object entity );

        Object GetById( object id );
    }

    public interface IRepository<T> : IRepository, IQueryable<T> where T : class
    {
        /// <summary>
        /// Inserts a new entity into the repository
        /// </summary>
        /// <param name="entity">Entity to insert into the repository</param>
        void Insert( T entity );

        /// <summary>
        /// Updates an entity in the repository
        /// </summary>
        /// <param name="entity">Entity to update in the repository</param>
        void Update( T entity );

        /// <summary>
        /// Inserts or updates an entity into the repository
        /// </summary>
        /// <param name="entity">Entity to save or update</param>
        void InsertOrUpdate( T entity );

        /// <summary>
        /// Deletes an entity from the repository
        /// </summary>
        /// <param name="entity">Entity to delete from the repository</param>
        void Delete( T entity );

        /// <summary>
        /// Evicts any changes made to the entity from the session
        /// </summary>
        /// <param name="entity">Entity to evict</param>
        void Evict( T entity );

        /// <summary>
        /// Deletes all items in the repository
        /// </summary>
        void DeleteAll( );

        /// <summary>
        /// Gets a proxy object of the item by its ID
        /// </summary>
        /// <param name="id">ID of the object</param>
        /// <returns></returns>
        T GetProxy( Object id );

        /// <summary>
        /// Refreshes an object from the database, including children
        /// </summary>
        /// <param name="entity">Entity to refresh</param>
        void Refresh( T entity );

    }
}
