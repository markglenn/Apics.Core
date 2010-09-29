using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using Ninject.Activation;
using Ninject.Modules;

namespace Apics.Utilities.Module
{
    public class Log4netModule : NinjectModule
    {
        private readonly Dictionary<Type, ILog> logs = new Dictionary<Type, ILog>( );

        public override void Load( )
        {
            Bind<ILog>( ).ToMethod( CreateLogger );
        }

        private ILog CreateLogger( IContext context )
        {
            Type type = context.Request.Target.Member.DeclaringType;

            lock( this.logs )
            {
                if( this.logs.ContainsKey( type ) )
                    return this.logs[ type ];

                ILog log = LogManager.GetLogger( type );
                this.logs.Add( type, log );

                return log;
            }
        }
    }
}