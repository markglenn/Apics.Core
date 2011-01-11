using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using Aptify.Framework.DataServices;

namespace Apics.Data.AptifyAdapter.ADO
{
    public class AptifyCommand : DbCommand
    {
        private readonly AptifyParameterCollection parameters = new AptifyParameterCollection( );
        private DbConnection connection;
        private DataAction dataAction;
        private AptifyTransaction transaction;

        public AptifyCommand( DbConnection connection, DataAction dataAction )
            : this( )
        {
            this.connection = connection;
            this.dataAction = dataAction;
        }

        public AptifyCommand( )
        {
            CommandType = CommandType.Text;
        }

        #region [ DbCommand Overrides ]

        public override string CommandText { get; set; }

        public override int CommandTimeout { get; set; }

        public override sealed CommandType CommandType { get; set; }

        protected override DbConnection DbConnection
        {
            get { return this.connection; }
            set
            {
                this.connection = value;
                this.dataAction = ( ( AptifyConnection )this.connection ).GetDataAction( );
            }
        }

        protected override DbParameterCollection DbParameterCollection
        {
            get { return this.parameters; }
        }

        protected override DbTransaction DbTransaction
        {
            get { return this.transaction; }
            set { this.transaction = ( AptifyTransaction )value; }
        }

        public override bool DesignTimeVisible { get; set; }

        public override UpdateRowSource UpdatedRowSource
        {
            get { throw new NotSupportedException( ); }
            set { throw new NotSupportedException( ); }
        }

        public override void Cancel( )
        {
            throw new NotSupportedException( "Can't cancel an Aptify command" );
        }

        protected override DbParameter CreateDbParameter( )
        {
            return new SqlParameter( );
        }

        protected override DbDataReader ExecuteDbDataReader( CommandBehavior behavior )
        {
            IDataReader reader;

            if( CommandText.StartsWith( "insert", true, CultureInfo.InvariantCulture ) )
                return null;

            if( DbTransaction == null )
            {
                reader = this.dataAction.ExecuteDataReaderParametrized(
                    CommandText, CommandType, this.parameters, behavior );
            }
            else
            {
                reader = this.dataAction.ExecuteDataReaderParametrized(
                    CommandText, CommandType, this.parameters, behavior, this.transaction.TransactionName );
            }

            return new AptifyDataReader( reader );
        }

        public override int ExecuteNonQuery( )
        {
            // Straight text queries are assumed to come from NHibernate
            if ( this.CommandType == CommandType.Text )
                return 1;

            if ( DbTransaction == null )
            {
                return this.dataAction.ExecuteNonQueryParametrized(
                    CommandText, CommandType, this.parameters );
            }
            
            return this.dataAction.ExecuteNonQueryParametrized(
                this.CommandText, this.CommandType, this.parameters, this.transaction.TransactionName );
        }

        public override object ExecuteScalar( )
        {
            if( DbTransaction == null )
            {
                return this.dataAction.ExecuteScalarParametrized(
                    CommandText, CommandType, this.parameters );
            }

            return this.dataAction.ExecuteScalarParametrized(
                CommandText, CommandType, this.parameters, this.transaction.TransactionName );
        }

        public override void Prepare( )
        {
            // Do nothing
        }

        #endregion [ DbCommand Overrides ]
    }
}