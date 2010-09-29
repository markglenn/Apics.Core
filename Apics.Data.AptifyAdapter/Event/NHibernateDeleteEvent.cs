using System;
using System.Collections.Generic;
using System.Linq;
using Iesi.Collections;
using NHibernate.Event;

namespace Apics.Data.AptifyAdapter.Event
{
    internal class NHibernateDeleteEvent : NHibernateBaseEvent, IDeleteEventListener
    {
        public NHibernateDeleteEvent( AptifyServer server ) :
            base( server )
        {
        }

        #region IDeleteEventListener Members

        public void OnDelete( DeleteEvent @event, ISet transientEntities )
        {
            throw new NotImplementedException( "Deleting transient entities is not supported" );
        }

        public void OnDelete( DeleteEvent @event )
        {
            AptifyEntity entity = LoadEntity( @event.Session, @event.Entity );

            entity.Delete( );
        }

        #endregion

        public override void Register( EventListeners eventListeners )
        {
            IEnumerable<IDeleteEventListener> listeners = eventListeners.DeleteEventListeners.Union( new[ ] { this } );

            eventListeners.DeleteEventListeners = listeners.ToArray( );
        }
    }
}