using System;
using System.Linq;
using Apics.Data.AptifyAdapter.Store;
using NHibernate.Event;

namespace Apics.Data.AptifyAdapter.Event
{

    internal abstract class NHibernateBaseEvent : IEventHandler
    {
        protected NHibernateBaseEvent( AptifyServer server )
        {
            Server = server;
        }

        protected AptifyServer Server { get; private set; }

        #region IEventHandler Members

        public abstract void Register( EventListeners eventListeners );

        #endregion

        protected AptifyEntity LoadEntity( IEventSource session, Object entity )
        {
            if( entity == null )
                throw new ArgumentNullException( "entity" );

            var entityStore = new EntityStore( session, entity );

            return new AptifyEntity( Server, entityStore );
        }
    }
}
