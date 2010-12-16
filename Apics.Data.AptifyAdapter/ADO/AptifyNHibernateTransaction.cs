using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NHibernate;
using NHibernate.Engine;
using NHibernate.Impl;
using NHibernate.Transaction;
using INHTransaction = NHibernate.ITransaction;

namespace Apics.Data.AptifyAdapter.ADO
{
    // ReSharper disable EmptyGeneralCatchClause

    public class AptifyNHibernateTransaction : INHTransaction, IAptifyTransaction
    {
        private readonly Guid sessionId;
        private bool begun;
        private bool commitFailed;

        /// <summary>
        /// A flag to indicate if <c>Dispose()</c> has been called.
        /// </summary>
        private bool isAlreadyDisposed;

        private ISessionImplementor session;

        private IList<ISynchronization> synchronizations;
        private AptifyTransaction transaction;

        #region [ Public Properties ]

        public string TransactionName
        {
            get { return this.transaction.TransactionName; }
        }

        #endregion [ Public Properties ]

        /// <summary>
        /// Initializes a new instance of the <see cref="AdoTransaction"/> class.
        /// </summary>
        /// <param name="session">The <see cref="ISessionImplementor"/> the Transaction is for.</param>
        public AptifyNHibernateTransaction( ISessionImplementor session )
        {
            this.session = session;
            this.sessionId = this.session.SessionId;
        }

        public IsolationLevel IsolationLevel
        {
            get { return this.transaction.IsolationLevel; }
        }

        #region ITransaction Members

        /// <summary>
        /// Enlist the <see cref="IDbCommand"/> in the current <see cref="ITransaction"/>.
        /// </summary>
        /// <param name="command">The <see cref="IDbCommand"/> to enlist in this Transaction.</param>
        /// <remarks>
        /// <para>
        /// This takes care of making sure the <see cref="IDbCommand"/>'s Transaction property 
        /// contains the correct <see cref="IDbTransaction"/> or <see langword="null" /> if there is no
        /// Transaction for the ISession - ie <c>BeginTransaction()</c> not called.
        /// </para>
        /// <para>
        /// This method may be called even when the transaction is disposed.
        /// </para>
        /// </remarks>
        public void Enlist( IDbCommand command )
        {
            command.Transaction = this.transaction;
        }

        public void RegisterSynchronization( ISynchronization sync )
        {
            if( sync == null ) throw new ArgumentNullException( "sync" );
            if( this.synchronizations == null )
            {
                this.synchronizations = new List<ISynchronization>( );
            }
            this.synchronizations.Add( sync );
        }

        public void Begin( )
        {
            Begin( IsolationLevel.Unspecified );
        }

        /// <summary>
        /// Begins the <see cref="IDbTransaction"/> on the <see cref="IDbConnection"/>
        /// used by the <see cref="ISession"/>.
        /// </summary>
        /// <exception cref="TransactionException">
        /// Thrown if there is any problems encountered while trying to create
        /// the <see cref="IDbTransaction"/>.
        /// </exception>
        public void Begin( IsolationLevel isolationLevel )
        {
            using( new SessionIdLoggingContext( this.sessionId ) )
            {
                if( this.begun )
                    return;

                if( this.commitFailed )
                    throw new TransactionException( "Cannot restart transaction after failed commit" );

                if( isolationLevel == IsolationLevel.Unspecified )
                    isolationLevel = this.session.Factory.Settings.IsolationLevel;

                if( isolationLevel == IsolationLevel.Unspecified )
                    this.transaction = ( AptifyTransaction )this.session.Connection.BeginTransaction( );
                else
                    this.transaction = ( AptifyTransaction )this.session.Connection.BeginTransaction( isolationLevel );

                this.begun = true;
                WasCommitted = false;
                WasRolledBack = false;

                this.session.AfterTransactionBegin( this );
            }
        }

        /// <summary>
        /// Commits the <see cref="ITransaction"/> by flushing the <see cref="ISession"/>
        /// and committing the <see cref="IDbTransaction"/>.
        /// </summary>
        /// <exception cref="TransactionException">
        /// Thrown if there is any exception while trying to call <c>Commit()</c> on 
        /// the underlying <see cref="IDbTransaction"/>.
        /// </exception>
        public void Commit( )
        {
            using( new SessionIdLoggingContext( this.sessionId ) )
            {
                CheckNotDisposed( );
                CheckBegun( );
                CheckNotZombied( );

                if( this.session.FlushMode != FlushMode.Never )
                {
                    this.session.Flush( );
                }

                NotifyLocalSynchsBeforeTransactionCompletion( );
                this.session.BeforeTransactionCompletion( this );

                try
                {
                    this.transaction.Commit( );

                    WasCommitted = true;
                    AfterTransactionCompletion( true );
                    Dispose( );
                }
                catch( HibernateException )
                {
                    AfterTransactionCompletion( false );
                    this.commitFailed = true;

                    // Don't wrap HibernateExceptions
                    throw;
                }
                catch( Exception e )
                {
                    AfterTransactionCompletion( false );
                    this.commitFailed = true;

                    throw new TransactionException( "Commit failed with SQL exception", e );
                }
                finally
                {
                    CloseIfRequired( );
                }
            }
        }

        /// <summary>
        /// Rolls back the <see cref="ITransaction"/> by calling the method <c>Rollback</c> 
        /// on the underlying <see cref="IDbTransaction"/>.
        /// </summary>
        /// <exception cref="TransactionException">
        /// Thrown if there is any exception while trying to call <c>Rollback()</c> on 
        /// the underlying <see cref="IDbTransaction"/>.
        /// </exception>
        public void Rollback( )
        {
            using( new SessionIdLoggingContext( this.sessionId ) )
            {
                CheckNotDisposed( );
                CheckBegun( );
                CheckNotZombied( );

                if( this.commitFailed )
                    return;
                try
                {
                    this.transaction.Rollback( );
                    WasRolledBack = true;
                    Dispose( );
                }
                catch( HibernateException )
                {
                    // Don't wrap HibernateExceptions
                    throw;
                }
                catch( Exception e )
                {
                    throw new TransactionException( "Rollback failed with SQL Exception", e );
                }
                finally
                {
                    AfterTransactionCompletion( false );
                    CloseIfRequired( );
                }
            }
        }

        /// <summary>
        /// Gets a <see cref="Boolean"/> indicating if the transaction was rolled back.
        /// </summary>
        /// <value>
        /// <see langword="true" /> if the <see cref="IDbTransaction"/> had <c>Rollback</c> called
        /// without any exceptions.
        /// </value>
        public bool WasRolledBack { get; private set; }

        /// <summary>
        /// Gets a <see cref="Boolean"/> indicating if the transaction was committed.
        /// </summary>
        /// <value>
        /// <see langword="true" /> if the <see cref="IDbTransaction"/> had <c>Commit</c> called
        /// without any exceptions.
        /// </value>
        public bool WasCommitted { get; private set; }

        public bool IsActive
        {
            get { return this.begun && !WasRolledBack && !WasCommitted; }
        }

        /// <summary>
        /// Takes care of freeing the managed and unmanaged resources that 
        /// this class is responsible for.
        /// </summary>
        public void Dispose( )
        {
            Dispose( true );
        }

        #endregion

        private void AfterTransactionCompletion( bool successful )
        {
            using( new SessionIdLoggingContext( this.sessionId ) )
            {
                this.session.AfterTransactionCompletion( successful, this );
                NotifyLocalSynchsAfterTransactionCompletion( successful );
                this.session = null;
                this.begun = false;
            }
        }

        private static void CloseIfRequired( )
        {
            //bool close = session.ShouldAutoClose() && !transactionContext.isClosed();
            //if (close)
            //{
            //    transactionContext.managedClose();
            //}
        }

        /// <summary>
        /// Finalizer that ensures the object is correctly disposed of.
        /// </summary>
        ~AptifyNHibernateTransaction( )
        {
            Dispose( false );
        }

        /// <summary>
        /// Takes care of freeing the managed and unmanaged resources that 
        /// this class is responsible for.
        /// </summary>
        /// <param name="isDisposing">Indicates if this AdoTransaction is being Disposed of or Finalized.</param>
        /// <remarks>
        /// If this AdoTransaction is being Finalized (<c>isDisposing==false</c>) then make sure not
        /// to call any methods that could potentially bring this AdoTransaction back to life.
        /// </remarks>
        protected virtual void Dispose( bool isDisposing )
        {
            using( new SessionIdLoggingContext( this.sessionId ) )
            {
                if( this.isAlreadyDisposed )
                {
                    // don't dispose of multiple times.
                    return;
                }

                // free managed resources that are being managed by the AdoTransaction if we
                // know this call came through Dispose()
                if( isDisposing )
                {
                    if( this.transaction != null )
                        this.transaction.Dispose( );

                    if( IsActive && this.session != null )
                    {
                        // Assume we are rolled back
                        AfterTransactionCompletion( false );
                    }
                }

                // free unmanaged resources here

                this.isAlreadyDisposed = true;
                // nothing for Finalizer to do - so tell the GC to ignore it
                GC.SuppressFinalize( this );
            }
        }

        private void CheckNotDisposed( )
        {
            if( this.isAlreadyDisposed )
                throw new ObjectDisposedException( "AdoTransaction" );
        }

        private void CheckBegun( )
        {
            if( !this.begun )
                throw new TransactionException( "Transaction not successfully started" );
        }

        private void CheckNotZombied( )
        {
            if( this.transaction != null && this.transaction.Connection == null )
                throw new TransactionException( "Transaction not connected, or was disconnected" );
        }

        private void NotifyLocalSynchsBeforeTransactionCompletion( )
        {
            if( this.synchronizations == null )
                return;

            foreach( ISynchronization t in this.synchronizations )
            {
                try
                {
                    t.BeforeCompletion( );
                }
                catch
                {
                }
            }
        }

        private void NotifyLocalSynchsAfterTransactionCompletion( bool success )
        {
            this.begun = false;
            if( this.synchronizations == null )
                return;

            foreach( ISynchronization t in this.synchronizations )
            {
                try
                {
                    t.AfterCompletion( success );
                }

                catch
                {
                }
            }
        }
    }

    // ReSharper restore EmptyGeneralCatchClause
}