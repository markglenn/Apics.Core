using System;
using System.Linq;
using NHibernate.Event;

namespace Apics.Data
{
    public interface IEventHandler
    {
        void Register( EventListeners eventListeners );
    }
}
