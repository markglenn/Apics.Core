using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Apics.Data.AptifyAdapter.ADO;
using NHibernate.Connection;
using ConfigEnvironment = NHibernate.Cfg.Environment;
using Aptify.Framework.DataServices;

namespace Apics.Data.AptifyAdapter
{
    public class AptifyConnectionProvider : ConnectionProvider
    {
        private AptifyConnectionStringBuilder connectionString;

        #region [ ConnectionProvider Overrides ]

        protected override string ConnectionString
        {
            get { return this.connectionString.ConnectionString; }
        }

        public override IDbConnection GetConnection( )
        {
            return new AptifyConnection( new DataAction( this.connectionString.Credentials ) );
        }

        public override void Configure( IDictionary<string, string> settings )
        {
            this.connectionString = new AptifyConnectionStringBuilder( settings[ ConfigEnvironment.ConnectionString ] );
            base.Configure( settings );
        }

        #endregion [ ConnectionProvider Overrides ]
    }
}