using System;
using System.Data;
using System.Linq;
using Apics.Data.Dialect;
using NHibernate.Connection;
using System.Data.SqlClient;

namespace Apics.Data.Database
{
	public class DatabaseDataStore : DataStoreBase
	{
		public DatabaseDataStore( IDialect dialect, string connectionString )
			: base( null, dialect, typeof( DriverConnectionProvider ), connectionString )
		{
		}

        #region [ Overrides of DataStoreBase ]

        public override IDbConnection CreateConnection( )
	    {
            throw new NotSupportedException( );
        }

        #endregion [ Overrides of DataStoreBase ]
    }
}
