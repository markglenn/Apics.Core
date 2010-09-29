using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using Aptify.Framework.DataServices;

namespace Apics.Data.AptifyAdapter.ADO
{
    /// <summary>
    /// Wrapper around Aptify's definition of a named scope transaction
    /// </summary>
    public class AptifyTransaction : DbTransaction, IAptifyTransaction
    {
        private readonly DbConnection connection;
        private readonly DataAction dataAction;
        private readonly IsolationLevel isolationLevel;
        private readonly string transactionName;

        /// <summary>
        /// Creates a transaction for Aptify
        /// </summary>
        /// <param name="connection">Connection to the database</param>
        /// <param name="dataAction">Database action connection</param>
        /// <param name="isolationLevel">Isolation level to use during this transaction</param>
        public AptifyTransaction( DbConnection connection, DataAction dataAction, IsolationLevel isolationLevel )
        {
            this.connection = connection;
            this.dataAction = dataAction;
            this.isolationLevel = isolationLevel;

            this.transactionName = dataAction.BeginTransaction( isolationLevel, true );
        }

        /// <summary>
        /// Backend connection
        /// </summary>
        protected override DbConnection DbConnection
        {
            get { return this.connection; }
        }

        /// <summary>
        /// Isolation level
        /// </summary>
        public override IsolationLevel IsolationLevel
        {
            get { return this.isolationLevel; }
        }

        #region IAptifyTransaction Members

        /// <summary>
        /// Name of the transaction used in the reads/writes
        /// </summary>
        public string TransactionName
        {
            get { return this.transactionName; }
        }

        #endregion

        /// <summary>
        /// Commits the transaction
        /// </summary>
        public override void Commit( )
        {
            this.dataAction.CommitTransaction( this.transactionName );
        }

        /// <summary>
        /// Rolls back the transaction
        /// </summary>
        public override void Rollback( )
        {
            this.dataAction.RollbackTransaction( this.transactionName );
        }
    }
}