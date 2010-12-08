using System;
using System.Linq;
using Castle.ActiveRecord;

namespace Apics.Data.Database
{
	public sealed class Transaction : ITransaction
	{
		#region [ Private Members ]

		private readonly TransactionScope scope = new TransactionScope( );
		private bool isCommitted;

		#endregion [ Private Members ]

		#region [ ITransaction Members ]

		public void Commit( )
		{
			this.scope.VoteCommit( );
			this.isCommitted = true;
		}

		public void Rollback( )
		{
			this.scope.VoteRollBack( );
			this.isCommitted = true;
		}

        public void Flush( )
        {
            this.scope.Flush( );
        }

		#endregion [ ITransaction Members ]

		#region [ IDisposable Members ]

		~Transaction( )
		{
			Dispose( false );
		}

		public void Dispose( )
		{
			Dispose( true );
			GC.SuppressFinalize( this );
		}

	    private void Dispose( bool disposing )
		{
			if ( !isCommitted )
				Rollback( );

			if ( disposing )
				this.scope.Dispose( );
		}

		#endregion [ IDisposable Members ]
	}
}