using System;
using System.Data;
using System.Linq;
using Apics.Data.AptifyAdapter.ADO;
using Apics.Data.AptifyAdapter.Mapping;
using Apics.Data.AptifyAdapter.Mapping.Metadata;
using Apics.Data.AptifyAdapter.Mapping.Visitors;
using Apics.Data.Dialect;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;
using Castle.ActiveRecord.Framework.Internal;
using log4net;
using ConfigEnvironment = NHibernate.Cfg.Environment;

namespace Apics.Data.AptifyAdapter
{
    /// <summary>
    /// Connection store class to an aptify backend
    /// </summary>
    public class AptifyDataStore : DataStoreBase
    {
        #region [ Private Members ]

        private static readonly ILog Log = LogManager.GetLogger( typeof( AptifyDataStore ) );
        private readonly AptifyServer server;

        #endregion [ Private Members ]

        /// <summary>
        /// Creates a new aptify store
        /// </summary>
        /// <param name="server">Server that this store is connecting</param>
        /// <param name="dialect">Dialect used to talk to the database</param>
        /// <param name="provider">Type of provider</param>
        /// <param name="handler">Event handler</param>
        public AptifyDataStore( AptifyServer server, IDialect dialect, Type provider, IEventHandler handler )
            : base( handler, dialect, provider, server.Connection.ConnectionString )
        {
            if( server == null )
                throw new ArgumentNullException( "server" );

            this.server = server;

            // Setup our transaction factory
            Properties[ ConfigEnvironment.TransactionStrategy ] =
                typeof( AptifyTransactionFactory ).AssemblyQualifiedName;

            // Handle the event to allow mapping
            ActiveRecordStarter.ModelsCreated += OnModelsCreated;
        }

        /// <summary>
        /// Handles the models after ActiveRecord has loaded them into memory
        /// </summary>
        /// <param name="models">Collection of all the models</param>
        /// <param name="source">The configuration</param>
        internal void OnModelsCreated( ActiveRecordModelCollection models, IConfigurationSource source )
        {
            Log.Debug( "Mapping Aptify entities to ActiveRecord models" );

            // Create a temporary IDbConnection that goes through Aptify
            using( IDbConnection connection = this.server.CreateConnection( ) )
            {
                // Load the entities from Aptify
                EntityMetadataCollection entitydata = new AptifyEntityLoader( connection ).LoadEntityMetadata( );
                var mapper = new AptifyModelMapper( );

                this.server.Tables = mapper.MapTables( entitydata, models );
            }

            Log.Info( "Successfully mapped ActiveRecord models" );
        }

        #region [ Overrides of DataStoreBase ]

        public override IDbConnection CreateConnection( )
        {
            return this.server.CreateConnection( );
        }

        #endregion [ Overrides of DataStoreBase ]
    }
}