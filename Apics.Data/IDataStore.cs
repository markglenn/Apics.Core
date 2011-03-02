using System;
using System.Data;
using System.Linq;

namespace Apics.Data
{
    /// <summary>
    /// Access to the data storage unit and factory for access classes
    /// </summary>
    public interface IDataStore : IDisposable
    {
        /// <summary>
        /// Creates a repository object that points to a data store
        /// </summary>
        /// <typeparam name="T">Type of the repository</typeparam>
        /// <returns>Repository that points to the data store</returns>
        IRepository<T> Repository<T>( ) where T : class;

        /// <summary>
        /// Creates a transaction object that is automatically rolled back if not committed
        /// </summary>
        /// <returns>Transaction object</returns>
        ITransaction CreateTransaction( );

        /// <summary>
        /// Creates a session to encase a series of queries
        /// </summary>
        /// <returns>Session to the database</returns>
        ISession CreateSession( );

		/// <summary>
		/// Initializes the data store so it can be officially used
		/// </summary>
		void Initialize( );

        /// <summary>
        /// Creates a general connection to the database
        /// </summary>
        /// <returns>A new connection to the database</returns>
	    IDbConnection CreateConnection( );

        /// <summary>
        /// Creates a transaction object that is automatically rolled back if not committed
        /// </summary>
        /// <param name="isolationLevel">Isolation level</param>
        /// <returns>Transaction object</returns>
        ITransaction CreateTransaction( IsolationLevel isolationLevel );
    }
}
