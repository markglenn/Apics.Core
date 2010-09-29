using System;
using System.Linq;
using Apics.Data.Dialect;
using NHibernate.Dialect;

namespace Apics.Data.AptifyAdapter
{
    internal class AptifyDialect : IDialect
    {
        private readonly string dialectClass = typeof( MsSql2008Dialect ).AssemblyQualifiedName;
        private readonly string driverClass = typeof( AptifyDriver ).AssemblyQualifiedName;

        #region IDialect Members

        public string Name
        {
            get { return "Aptify Dialect"; }
        }

        public string Dialect
        {
            get { return this.dialectClass; }
        }

        public string DriverClass
        {
            get { return this.driverClass; }
        }

        #endregion
    }
}