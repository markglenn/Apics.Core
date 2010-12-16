using System;
using System.Collections;
using System.Linq;
using NHibernate.Engine;
using NHibernate.Engine.Transaction;
using NHibernate.Transaction;

namespace Apics.Data.AptifyAdapter.ADO
{
    /// <summary>
    /// Factory used to build the transactions
    /// </summary>
    public class AptifyTransactionFactory : ITransactionFactory
    {
        #region ITransactionFactory Members

        /// <summary>
        /// Configures the transaction factory
        /// </summary>
        /// <param name="props">Configuration properties</param>
        public void Configure( IDictionary props )
        {
        }

        /// <summary>
        /// Creates a transaction
        /// </summary>
        /// <param name="session">Session used for this transaction</param>
        /// <returns>New aptify transaction</returns>
        public NHibernate.ITransaction CreateTransaction( ISessionImplementor session )
        {
            return new AptifyNHibernateTransaction( session );
        }

        public void EnlistInDistributedTransactionIfNeeded( ISessionImplementor session )
        {
            // Do nothing since we do not support distributed transactions
        }

        public void ExecuteWorkInIsolation( ISessionImplementor session, IIsolatedWork work, bool transacted )
        {
            throw new NotSupportedException( );
        }

        public bool IsInDistributedActiveTransaction( ISessionImplementor session )
        {
            return true;
        }

        #endregion
    }
}