using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using Aptify.Framework.DataServices;

namespace Apics.Data.AptifyAdapter.ADO
{
    internal class AptifyConnection : DbConnection
    {
        private readonly DataAction dataAction;
        private ConnectionState connectionState = ConnectionState.Closed;
        private AptifyConnectionStringBuilder connectionString;

        public AptifyConnection( string connectionString )
            : this( new AptifyConnectionStringBuilder( connectionString ) )
        {
        }

        public AptifyConnection( AptifyConnectionStringBuilder connectionString )
        {
            this.connectionString = connectionString;
            this.dataAction = new DataAction( this.connectionString.Credentials );
        }

        #region [ DbConnection Overrides ]

        public override string ConnectionString
        {
            get { return this.connectionString.ToString( ); }
            set { this.connectionString = new AptifyConnectionStringBuilder( value ); }
        }

        public override string DataSource
        {
            get { return this.connectionString.DataSource; }
        }

        public override string Database
        {
            get { return this.connectionString.InitialCatalog; }
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

        internal DataAction GetDataAction( )
        {
            return this.dataAction;
        }
    }
}