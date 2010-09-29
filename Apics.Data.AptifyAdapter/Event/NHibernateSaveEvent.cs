using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Event;

namespace Apics.Data.AptifyAdapter.Event
{
    internal class NHibernateSaveEvent : NHibernateBaseEvent, ISaveOrUpdateEventListener
    {
        public NHibernateSaveEvent( AptifyServer server ) :
            base( server )
        {
        }

        #region ISaveOrUpdateEventListener Members

        public void OnSaveOrUpdate( SaveOrUpdateEvent @event )
        {
            AptifyEntity entity = LoadEntity( @event.Session, @event.Entity );

            entity.SaveOrUpdate( );
        }

        #endregion

        public override void Register( EventListeners eventListeners )
        {
            IEnumerable<ISaveOrUpdateEventListener> listeners =
                eventListeners.SaveOrUpdateEventListeners.Union( new[ ] { this } );

            eventListeners.SaveOrUpdateEventListeners = listeners.ToArray( );
        }
    }
}