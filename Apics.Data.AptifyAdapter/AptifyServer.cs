using System;
using System.Data;
using System.Linq;
using Apics.Data.AptifyAdapter.ADO;
using Apics.Data.AptifyAdapter.Mapping;
using Apics.Data.AptifyAdapter.Mapping.Metadata;
using Apics.Data.AptifyAdapter.Store;
using Aptify.Framework.Application;
using Aptify.Framework.BusinessLogic.GenericEntity;
using log4net;

namespace Apics.Data.AptifyAdapter
{
    /// <summary>
    /// Connection to the Aptify server
    /// </summary>
    public class AptifyServer
    {
        private static readonly ILog Log = LogManager.GetLogger( typeof( AptifyServer ) );

        private readonly AptifyApplication application;
        private readonly AptifyConnectionStringBuilder connection;

        #region [ Public Properties ]

        /// <summary>
        /// Mappings of all the tables to aptify entities
        /// </summary>
        public TableMappings Tables { get; set; }

        /// <summary>
        /// Connection string to Aptify
        /// </summary>
        public AptifyConnectionStringBuilder Connection
        {
            get { return this.connection; }
        }

        #endregion [ Public Properties ]

        /// <summary>
        /// Creates a connection to the aptify application server
        /// </summary>
        /// <param name="connection">Connection string</param>
        public AptifyServer( AptifyConnectionStringBuilder connection )
        {
            this.application = new AptifyApplication( connection.Credentials );

            this.connection = connection;
        }

        /// <summary>
        /// Gets the generic entity from the server
        /// </summary>
        /// <param name="entityMetadata">Entity metadata for loading</param>
        /// <param name="store">The NHibernate storage about the updated object</param>
        /// <returns>A new generic entity that maps to this table and store</returns>
        internal AptifyGenericEntityBase GetEntity( AptifyEntityMetadata entityMetadata, EntityStore store )
        {
            Log.DebugFormat( "Creating aptify entity for {0}", entityMetadata.Name );

            EntityInfo info = this.application.get_Entity( entityMetadata.Id );
            AptifyGenericEntityBase entity;

            // Aptify requires a '-1' if the entity is to be created
            switch( store.Status )
            {
                case EntityStatus.Clean:
                case EntityStatus.Dirty:
                    entity = info.GetEntityObject( store.Id );
                    break;

                case EntityStatus.New:
                    entity = info.GetEntityObject( -1 );
                    break;

                default:
                    throw new InvalidOperationException( "Unknown entity status " + store.Status );
            }

            Log.DebugFormat( "Successfully created aptify entity for {0}", entityMetadata.Name );

            return entity;
        }

        /// <summary>
        /// Gets a child entity from the server
        /// </summary>
        /// <param name="parent">Parent entity</param>
        /// <param name="entityMetadata">Metadata for the entity</param>
        /// <param name="store">The NHibernate storage about the updated object</param>
        /// <returns>A new generic entity that maps to this table and store</returns>
        internal AptifyGenericEntityBase GetEntity( AptifyEntity parent, AptifyEntityMetadata entityMetadata,
            EntityStore store )
        {
            Log.DebugFormat( "Creating child aptify entity for {0}", entityMetadata.Name );

            AptifySubTypeBase subType = parent.GenericEntity.SubTypes[ entityMetadata.Name ];
            AptifyGenericEntityBase entity;

            switch( store.Status )
            {
                case EntityStatus.Clean:
                case EntityStatus.Dirty:
                    entity = subType.Find( "id", store.Id );
                    break;

                case EntityStatus.New:
                    entity = subType.Add( );
                    break;

                default:
                    throw new InvalidOperationException( "Unknown entity status " + store.Status );
            }

            Log.DebugFormat( "Successfully loaded aptify entity for {0}", entityMetadata.Name );

            return entity;
        }

        /// <summary>
        /// Helper method to quickly create a database connection that runs through the aptify server
        /// </summary>
        /// <returns>A new IDbConnection that connects to Aptify</returns>
        internal IDbConnection CreateConnection( )
        {
            return new AptifyConnection( this.connection );
        }

        internal int SaveEntity( AptifyGenericEntityBase genericEntity, IAptifyTransaction transaction )
        {
            string errorMessage = String.Empty;

            // Is this in a transaction
            if( transaction == null )
            {
                // Save without transaction
                if( !genericEntity.Save( false, ref errorMessage ) )
                    throw new DataException( errorMessage );
            }
            else
            {
                // Save using the transaction
                if( !genericEntity.Save( false, ref errorMessage, transaction.TransactionName ) )
                    throw new DataException( errorMessage );
            }

            return ( int )genericEntity.RecordID;
        }
    }
}