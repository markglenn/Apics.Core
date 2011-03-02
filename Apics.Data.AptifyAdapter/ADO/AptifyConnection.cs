using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using Aptify.Framework.DataServices;
using Aptify.Framework.Application;

namespace Apics.Data.AptifyAdapter.ADO
{
    internal class AptifyConnection : DbConnection
    {
        #region [ Private Members ]

        private readonly DataAction dataAction;
        private ConnectionState connectionState;

        #endregion [ Private Members ]

        public AptifyConnection( DataAction dataAction )
        {
            this.dataAction = dataAction;
        }

        internal DataAction GetDataAction( )
        {
            return this.dataAction;
        }

        #region [ DbConnection Overrides ]

        public override string ConnectionString
        {
            get { return this.dataAction.UserCredentials.ConnectionString; }
            set { throw new NotSupportedException( ); }
        }

        public override string DataSource
        {
            get { return this.dataAction.UserCredentials.Server; }
        }

        public override string Database
        {
            get { return this.dataAction.UserCredentials.EntitiesDatabase; }
        }

        public override string ServerVersion
        {
            get { return "SQL Server 2008 w/ Aptify Adapter"; }
        }

        public override ConnectionState State
        {
            get { return this.connectionState; }
        }

        protected override DbTransaction BeginDbTransaction( IsolationLevel isolationLevel )
        {
            return new AptifyTransaction( this, this.dataAction, isolationLevel );
        }

        public override void ChangeDatabase( string databaseName )
        {
            throw new NotSupportedException( );
        }

        public override void Close( )
        {
            this.connectionState = ConnectionState.Closed;
        }

        protected override DbCommand CreateDbCommand( )
        {
            return new AptifyCommand( this, this.dataAction );
        }

        public override void Open( )
        {
            this.connectionState = ConnectionState.Open;
        }

        #endregion [ DbConnection Overrides ]

    }
}