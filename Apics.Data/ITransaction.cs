using System;
using System.Linq;

namespace Apics.Data
{
    /// <summary>
    /// Data store transaction that defaults to rolling back
    /// </summary>
    /// <example>
    ///	using ( var tx = datastore.CreateTransaction( ) )
    ///	{
    ///		...
    ///		tx.Commit( );
    /// }
    /// </example>
    public interface ITransaction : IDisposable
    {
        /// <summary>
        /// Forcably rolls back the transaction
        /// </summary>
        void Rollback( );

        /// <summary>
        /// Commits the transaction
        /// </summary>
        void Commit( );

        /// <summary>
        /// Flushes current changes to the database
        /// </summary>
        void Flush( );
    }
}
