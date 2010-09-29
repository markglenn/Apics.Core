using System;
using System.Linq;
using NHibernate.Dialect;
using NHibernate.Driver;

namespace Apics.Data.Dialect
{
    public class SqlServer2008Dialect : IDialect
    {
        #region IDialect Members

        public string Name
        {
            get { return "SQL Server 2008 Dialect"; }
        }

        public string Dialect
        {
            get { return typeof( MsSql2008Dialect ).AssemblyQualifiedName; }
        }

        public string DriverClass
        {
            get { return typeof( SqlClientDriver ).AssemblyQualifiedName; }
        }

        #endregion
    }
}
