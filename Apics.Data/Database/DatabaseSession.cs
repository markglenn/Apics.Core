using System;
using System.Linq;
using Castle.ActiveRecord;

namespace Apics.Data.Database
{
    public sealed class DatabaseSession : ISession
    {
        private readonly ISessionScope session;

        public DatabaseSession( ISessionScope session )
        {
            if( session == null )
                throw new ArgumentNullException( "session" );

            this.session = session;
        }

        #region ISession Members

        public void Dispose( )
        {
            this.session.Dispose( );
        }

        #endregion
    }
}