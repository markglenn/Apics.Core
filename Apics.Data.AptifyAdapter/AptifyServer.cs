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
using Ninject;

namespace Apics.Data.AptifyAdapter
{
    /// <summary>
    /// Connection to the Aptify server
    /// </summary>
    public class AptifyServer
    {
        private static readonly ILog Log = LogManager.GetLogger( typeof( AptifyServer ) );

        private readonly AptifyConnectionStringBuilder connection;
        private readonly IKernel kernel;

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
        public AptifyServer( IKernel kernel, AptifyConnectionStringBuilder connection )
        {
            this.kernel = kernel;
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

            var application = this.kernel.Get<AptifyApplication>( );

            application.UserCredentials.DefaultTransactionID = String.Empty;
            
            EntityInfo info = application.get_Entity( entityMetadata.Id );
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

        internal AptifyGenericEntityBase GetEntity( string name, int id, IAptifyTransaction transaction = null )
        {
            var application = this.kernel.Get<AptifyApplication>( );

            application.UserCredentials.DefaultTransactionID = transaction == null ? String.Empty :
                transaction.TransactionName;

            return application.GetEntityObject( name, ( long )id );
        }

        public AptifyGenericEntityBase GetEntity( object entity, IAptifyTransaction transaction = null )
        {
            var aptifyEntity = this.Tables.GetEntityMetadata( entity );

            // Reflection magic to get the ID.  Let's hope nobody checks this.
            var prop = (
                from p in entity.GetType( ).GetProperties( )
                where p.Name.Equals( "id", StringComparison.InvariantCultureIgnoreCase )
                select p ).FirstOrDefault( );

            if ( prop == null )
                throw new InvalidOperationException( "Entity does not have an ID property" );

            if ( !prop.PropertyType.IsValueType )
                throw new InvalidOperationException( "Entity's ID is not a valid type.  Please use an int or long" );

            long id = Convert.ToInt64( prop.GetValue( entity, null ) );

            var application = this.kernel.Get<AptifyApplication>( );
            
            // TODO: Fix this hack
            application.UserCredentials.DefaultTransactionID = transaction != null ? transaction.TransactionName : String.Empty;

            EntityInfo info = application.get_Entity( aptifyEntity.Id );

            return info.GetEntityObject( id );
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

            // This is not a child entity within Aptify
            if (subType == null)
                return GetEntity(entityMetadata, store);

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