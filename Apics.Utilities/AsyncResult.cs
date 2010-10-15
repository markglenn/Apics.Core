using System;
using System.Linq;
using System.Threading;

namespace Apics.Utilities
{
    public class AsyncResult<T> : IAsyncResult, IDisposable
    {
        #region [ Private Members ]

        private readonly AsyncCallback callback;
        private bool completed;
        private bool completedSynchronously;
        private readonly object asyncState;
        private readonly ManualResetEvent waitHandle;
        private Exception exception;
        private readonly object syncRoot;
        private T result;

        #endregion [ Private Members ]

        public AsyncResult( AsyncCallback cb, object state )
            : this( cb, state, false )
        {
        }

        public AsyncResult( AsyncCallback cb, object state, bool completed )
        {
            this.callback = cb;
            this.asyncState = state;
            this.completed = completed;
            this.completedSynchronously = completed;

            this.waitHandle = new ManualResetEvent( false );
            this.syncRoot = new object( );
        }

        #region [ IAsyncResult Members ]

        public object AsyncState
        {
            get { return this.asyncState; }
        }

        public WaitHandle AsyncWaitHandle
        {
            get { return this.waitHandle; }
        }

        public bool CompletedSynchronously
        {
            get
            {
                lock ( this.syncRoot )
                    return this.completedSynchronously;
            }
        }

        public bool IsCompleted
        {
            get
            {
                lock ( this.syncRoot )
                    return this.completed;
            }
        }

        public T Result
        {
            get
            {
                lock( this.syncRoot )
                    return this.result;
            }
        }

        #endregion [ IAsyncResult Members ]

        #region [ IDisposable Members ]

        public void Dispose( )
        {
            this.Dispose( true );
            GC.SuppressFinalize( this );
        }

        protected virtual void Dispose( bool disposing )
        {
            if ( disposing )
            {
                lock ( this.syncRoot )
                {
                    if ( this.waitHandle != null )
                    {
                        ( ( IDisposable )this.waitHandle ).Dispose( );
                    }
                }
            }
        }
        
        #endregion [ IDisposable Members ]

        public Exception Exception
        {
            get
            {
                lock ( this.syncRoot )
                    return this.exception;
            }
        }

        public void Complete( T completedResult, bool completedSynch )
        {
            lock ( this.syncRoot )
            {
                this.completed = true;
                this.completedSynchronously = completedSynch;
                this.result = completedResult;
            }

            this.SignalCompletion( );
        }

        public void HandleException( Exception e, bool completedSynch )
        {
            lock ( this.syncRoot )
            {
                this.completed = true;
                this.completedSynchronously = completedSynch;
                this.exception = e;
            }

            this.SignalCompletion( );
        }

        private void SignalCompletion( )
        {
            this.waitHandle.Set( );
            
            if ( this.callback != null )
                ThreadPool.QueueUserWorkItem( _ => this.callback( this ) );
        }

    }
}
