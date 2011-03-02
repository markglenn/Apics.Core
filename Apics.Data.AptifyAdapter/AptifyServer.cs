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
using Aptify.Framework.DataServices;

namespace Apics.Data.AptifyAdapter
{
    /// <summary>
    /// Connection to the Aptify server ಠ_ಠ
    /// </summary>
    public class AptifyServer
    {
        #region [ Private Members ]

        private static readonly ILog Log = LogManager.GetLogger( typeof( AptifyServer ) );

        private readonly AptifyApplication application;

        #endregion [ Private Members ]

        #region [ Public Properties ]

        /// <summary>
        /// Mappings of all the tables to aptify entities
        /// </summary>
        public TableMappings Tables { get; set; }

        /// <summary>
        /// Connection string
        /// </summary>
        public string ConnectionString
        {
            get { return this.application.UserCredentials.ConnectionString; }
        }

        #endregion [ Public Properties ]

        /// <summary>
        /// Creates a connection to the aptify application server
        /// </summary>
        /// <param name="connection">Connection string</param>
        public AptifyServer( AptifyApplication application )
        {
            if ( application == null )
                throw new ArgumentNullException( "application" );

            this.application = application;
        }

        #region [ GetEntity Methods ]

        /// <summary>
        /// Gets the generic entity from the server
        /// </summary>
        /// <param name="entityMetadata">Entity metadata for loading</param>
        /// <param name="store">The NHibernate storage about the updated object</param>
        /// <returns>A new generic entity that maps to this table and store</returns>
        public AptifyGenericEntityBase GetEntity( AptifyEntityMetadata entityMetadata, EntityStore store )
        {
            Log.DebugFormat( "Creating aptify entity for {0}", entityMetadata.Name );

            this.application.UserCredentials.DefaultTransactionID = String.Empty;
            var entityInfo = this.application.get_Entity( entityMetadata.Id );

            return entityInfo.GetEntityObject( store.Id );
        }

        public AptifyGenericEntityBase GetEntity( string name, int id, IAptifyTransaction transaction )
        {
            this.application.UserCredentials.DefaultTransactionID = transaction == null ? String.Empty :
                transaction.TransactionName;

            return this.application.GetEntityObject( name, ( long )id );
        }

        public AptifyGenericEntityBase GetEntity( object entity, IAptifyTransaction transaction )
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

            // TODO: Fix this hack
            this.application.UserCredentials.DefaultTransactionID = 
                transaction != null ? transaction.TransactionName : String.Empty;

            return this.application.get_Entity( aptifyEntity.Id )
                .GetEntityObject( id );
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

            // This is not a child entity within Aptify
            if (subType == null)
                return GetEntity(entityMetadata, store);

            switch( store.Status )
            {
                case EntityStatus.Clean:
                case EntityStatus.Dirty:
                    Log.DebugFormat( "Loading subentity for {0}", entityMetadata.Name );
                    return subType.Find( "id", store.Id );

                case EntityStatus.New:
                    Log.DebugFormat( "Loading new subentity for {0}", entityMetadata.Name );
                    return subType.Add( );

                default:
                    throw new InvalidOperationException( "Unknown entity status " + store.Status );
            }
        }

        /// <summary>
        /// Gets an entity by id and entity name
        /// </summary>
        /// <param name="name">Entity name</param>
        /// <param name="id">Object id</param>
        /// <returns>A generic entity based on the name and id</returns>
        internal AptifyGenericEntityBase GetEntity( string name, int id )
        {
            return GetEntity( name, id, null );
        }

        /// <summary>
        /// Load the entity by the object out of transaction
        /// </summary>
        /// <param name="entity">Object of the entity</param>
        /// <returns>The generic entity based on the entity object type</returns>
        public AptifyGenericEntityBase GetEntity( object entity )
        {
            return GetEntity( entity, null );
        }

        #endregion [ GetEntity Methods ]

        /// <summary>
        /// Helper method to quickly create a database connection that runs through the aptify server
        /// </summary>
        /// <returns>A new IDbConnection that connects to Aptify</returns>
        public IDbConnection CreateConnection( )
        {
            return new AptifyConnection( new DataAction( this.application.UserCredentials ) );
        }

        /// <summary>
        /// Save the given entity to the database
        /// </summary>
        /// <param name="genericEntity">Generic entity to save</param>
        /// <param name="transaction">Transaction to save within</param>
        /// <returns>The new ID of the entity</returns>
        public int SaveEntity( AptifyGenericEntityBase genericEntity, IAptifyTransaction transaction )
        {
            string message = String.Empty;
            string transactionName = ( transaction == null ) ? null : transaction.TransactionName;

            // Validate the entity for errors
            if ( !genericEntity.Validate( ref message ) )
                throw new DataException( message );

            // Try saving the entity
            if ( !genericEntity.Save( false, ref message, transactionName ) )
            {
                // It's possible the error message is not returned
                if ( String.IsNullOrEmpty( message ) )
                    message = genericEntity.LastError;

                throw new DataException( message );
            }

            return ( int )genericEntity.RecordID;
        }
    }
}