using System;
using System.Linq;
using NHibernate.Driver;

namespace Apics.Data.Dialect
{
	public class SQLiteDialect : IDialect
	{
	    #region [ IDialect Members ]

	    public string Name
	    {
            get { return "SQLite Dialect"; }
	    }

	    public string Dialect
        {
            get { return typeof( NHibernate.Dialect.SQLiteDialect ).AssemblyQualifiedName; }
        }

        public string DriverClass
        {
            get { return typeof( SQLite20Driver ).AssemblyQualifiedName; }
        }

        #endregion
    }
}
