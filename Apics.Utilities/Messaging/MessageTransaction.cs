using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;

namespace Apics.Utilities.Messaging
{
    public class MessageTransaction : IDisposable
    {
        #region [ Internal Properties ]

        internal MessageQueueTransaction Transaction { get; private set; }

        #endregion [ Internal Properties ]

        public bool IsCommited
        {
            get { return this.Transaction.Status != MessageQueueTransactionStatus.Pending; }
        }

        public MessageTransaction( )
        {
            this.Transaction = new MessageQueueTransaction( );
            this.Transaction.Begin( );
        }

        public void Commit( )
        {
            this.Transaction.Commit( );
        }

        public void Rollback( )
        {
            this.Transaction.Abort( );
        }

        #region [ IDisposable Members ]

        ~MessageTransaction( )
        {
            Dispose( false );
        }

        public void Dispose( )
        {
            Dispose( true );
            GC.SuppressFinalize( this );
        }

        protected void Dispose( bool disposing )
        {
            if( !disposing ) 
                return;

            if ( this.Transaction.Status == MessageQueueTransactionStatus.Pending )
                Rollback( );

            this.Transaction.Dispose( );
        }

        #endregion [ IDisposable Members ]
    }
}
