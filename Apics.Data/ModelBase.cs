using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Apics.Data
{
    public class SavingEventArgs : EventArgs
    {
        public Object Backing { get; set; }
    }

    public abstract class ModelBase
    {
        public virtual event EventHandler<SavingEventArgs> Saving;

        public virtual bool ForceSave
        {
            get { return this.Saving != null; }
        }

        public virtual void HandleSave( Object backing )
        {
            lock ( Saving )
            {
                var handlers = Saving;

                if ( handlers != null )
                    handlers( this, new SavingEventArgs { Backing = backing } );

                Saving = null;
            }
        }
    }
}
