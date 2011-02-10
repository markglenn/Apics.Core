using System;
using System.Collections.Generic;
using System.Linq;
using Apics.Data.AptifyAdapter.ADO;
using NHibernate;
using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Event;
using NHibernate.Persister.Entity;
using NHibernateStatus = NHibernate.Engine.Status;
using NHibernate.Metadata;

namespace Apics.Data.AptifyAdapter.Store
{
    internal enum EntityStatus
    {
        /// <summary>
        /// This is a new entity to be saved
        /// </summary>
        New,

        /// <summary>
        /// This entity has changed one or more properties
        /// </summary>
        Dirty,

        /// <summary>
        /// This is a clean entity
        /// </summary>
        Clean
    }

    /// <summary>
    /// Temporary storage of the NHibernate entity before saving/updating 
    /// </summary>
    internal class EntityStore
    {
        #region [ Private Members ]
        
        private readonly object entityObject;
        private readonly IClassMetadata metadata;
        private readonly IEventSource session;
        private readonly AptifyNHibernateTransaction transaction;
        private readonly IEntityPersister persister;

        private object[ ] currentState;

        private EntityStatus status;

        #endregion [ Private Members ]

        #region [ Public Properties ]

        /// <summary>
        /// Gets the value of one of the properties
        /// </summary>
        /// <param name="index">Index of the property</param>
        /// <returns>Value of the property</returns>
        internal object this[ int index ]
        {
            get { return this.currentState[ index ]; }
        }

        /// <summary>
        /// Status of this entity
        /// </summary>
        internal EntityStatus Status
        {
            get
            {
                var o = this.entityObject as ModelBase;

                if ( o != null && o.ForceSave && this.status == EntityStatus.Clean )
                    return EntityStatus.Dirty;

                return this.status;
            }
            set
            {
                this.status = value;
            }
        }

        /// <summary>
        /// The dirty indices
        /// </summary>
        internal IEnumerable<int> DirtyIndices { get; private set; }

        /// <summary>
        /// NHibernate entity to store
        /// </summary>
        internal object EntityObject
        {
            get { return this.entityObject; }
        }

        /// <summary>
        /// Identifier of the entity
        /// </summary>
        public int Id
        {
            get { return ( int )this.metadata.GetIdentifier( this.entityObject, EntityMode.Poco ); }
            set { this.metadata.SetIdentifier( this.entityObject, value, EntityMode.Poco ); }
        }

        /// <summary>
        /// The transaction 
        /// </summary>
        public AptifyNHibernateTransaction Transaction
        {
            get { return this.transaction; }
        }

        /// <summary>
        /// The session that this store is attached
        /// </summary>
        public IEventSource Session
        {
            get { return this.session; }
        }

        public IEntityPersister Persister
        {
            get { return this.persister; }
        }

        #endregion [ Public Properties ]

        internal EntityStore( IEventSource session, Object entityObject )
        {
            if ( session.TransactionInProgress )
                this.transaction = ( AptifyNHibernateTransaction )session.Transaction;

            this.entityObject = entityObject;
            this.session = session;
            this.metadata = session.SessionFactory.GetClassMetadata( entityObject.GetType( ) );

            persister = this.session.GetEntityPersister( this.metadata.EntityName, entityObject );
            LoadEntityState( );
        }

        /// <summary>
        /// Sets the status of the entity as clean
        /// </summary>
        /// <param name="id">New ID to set for the object</param>
        internal void MarkAsPersisted( int id )
        {
            this.status = EntityStatus.Clean;
            this.DirtyIndices = null;

            this.Id = id;

            // Reload the entity because it may have changed
            this.session.Refresh( this.entityObject, LockMode.None );
        }

        internal EntityStore CreateChild( object item )
        {
            return new EntityStore( this.session, item );
        }

        #region [ Private Methods ]

        /// <summary>
        /// Loads the current state and sets the entity status
        /// </summary>
        private void LoadEntityState( )
        {
            // Pull in the values of all the 
            this.currentState = this.metadata.GetPropertyValues( this.entityObject, EntityMode.Poco );

            var entry = this.session.PersistenceContext.GetEntry( this.entityObject );

            // Get the dirty indices
            this.DirtyIndices = GetDirtyIndices( entry, this.currentState );

            // Check the status based on the dirty indices
            if( entry == null || !entry.ExistsInDatabase )
                this.status = EntityStatus.New;
            else if( this.DirtyIndices.Any( ) )
                this.status = EntityStatus.Dirty;
            else
                this.status = EntityStatus.Clean;
        }

        /// <summary>
        /// Gets the indices of the object that are currently dirty
        /// </summary>
        /// <param name="entry">Entity to check</param>
        /// <param name="currentState">Current state of the entity</param>
        /// <returns>A list of dirty indices</returns>
        private static IEnumerable<int> GetDirtyIndices( EntityEntry entry, IList<object> currentState )
        {
            // Currently not stored in the database, all fields are considered dirty
            if( IsTransient( entry ) )
                return Enumerable.Range( 0, currentState.Count );

            var dirtyProperties = new List<int>( );

            for( int i = 0; i < entry.LoadedState.Length; ++i )
            {
                // Check if the state has changed and that the property is updateable
                if( entry.Persister.PropertyUpdateability[ i ] && !Equals( entry.LoadedState[ i ], currentState[ i ] ) )
                    dirtyProperties.Add( i );

                else if( entry.Persister.PropertyTypes[ i ].IsCollectionType )
                {
                    // Collections are handled differently
                    var collection = ( IPersistentCollection )currentState[ i ];

                    // Add the dirty index if the collection has changed
                    if( collection.IsDirty )
                        dirtyProperties.Add( i );
                }
            }

            return dirtyProperties;
        }

        /// <summary>
        /// Is this entity not attached to the session
        /// </summary>
        /// <param name="entry">Entry to check transience</param>
        /// <returns>True if the entity is not housed within the session</returns>
        private static bool IsTransient( EntityEntry entry )
        {
            if ( entry == null || ( ( int )entry.Id ) == 0 )
                return true;

            // Proxy entities are considered in the session
            if( entry.Status != NHibernateStatus.Loaded )
                return false;

            // Is the entity in the session
            return !entry.ExistsInDatabase;
        }

        #endregion [ Private Methods ]
    }
}