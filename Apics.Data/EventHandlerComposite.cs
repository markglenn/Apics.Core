using System;
using System.Linq;
using NHibernate.Event;

namespace Apics.Data
{
    public class EventHandlerComposite : IEventHandler
    {
        private readonly IEventHandler[ ] handlers;

        public EventHandlerComposite( params IEventHandler[ ] handlers )
        {
            this.handlers = handlers;
        }

        #region IEventHandler Members

        public void Register( EventListeners eventListeners )
        {
            foreach( IEventHandler handler in this.handlers )
                handler.Register( eventListeners );
        }

        #endregion
    }
}