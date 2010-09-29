using System;
using System.Linq;
using NHibernate.Dialect;
using NHibernate.Driver;

namespace Apics.Data.Dialect
{
    public class SqlServer2000Dialect : IDialect
    {
        #region IDialect Members

        public string Name
        {
            get { return "SQL Server 2000 Dialect"; }
        }

        public string Dialect
        {
            get { return typeof( MsSql2000Dialect ).AssemblyQualifiedName; }
        }

        public string DriverClass
        {
            get { return typeof( SqlClientDriver ).AssemblyQualifiedName; }
        }

        #endregion
    }
}
