using System;
using System.Data;
using System.Linq;
using Apics.Data.Dialect;
using NHibernate.Connection;

namespace Apics.Data.Database
{
	public class DatabaseDataStore : AbstractDataStore
	{
		public DatabaseDataStore( IDialect dialect, string connectionString )
			: base( null, dialect, typeof( DriverConnectionProvider ), connectionString )
		{
		}

	    #region Overrides of AbstractDataStore

	    public override IDbConnection CreateConnection( )
	    {
            throw new NotSupportedException( );
	    }

	    #endregion
	}
}
