using System;
using System.Configuration;
using System.Linq;
using log4net;
using Ninject;
using Ninject.Modules;

namespace Apics.Utilities.Module
{
    public static class NinjectFactory
    {
        private static readonly ILog Log = LogManager.GetLogger( typeof( NinjectFactory ) );

        public static IKernel Create( ModuleSettingConfiguration settings )
        {
            if ( settings == null )
                throw new ArgumentException( "Invalid dependency settings or path" );

            IKernel kernel = Create( );

            LoadModules( kernel, settings );

            return kernel;
        }

        public static IKernel Create( string path )
        {
            return Create(
                ( ModuleSettingConfiguration )ConfigurationManager.GetSection( path ) );
        }

        public static IKernel Create( )
        {
            return new StandardKernel( );
        }

        private static void LoadModules( IKernel kernel, ModuleSettingConfiguration settings )
        {
            foreach( ModuleSetting module in settings.Modules )
            {
                Type type = Type.GetType( module.Type, false );

                if( type == null )
                    throw new ConfigurationErrorsException( "Invalid module in configuration: " + module.Type );

                kernel.Bind( type ).ToSelf( );
                kernel.Load( ( NinjectModule )kernel.Get( type ) );

                Log.InfoFormat( "Loaded module {0}", module.Type );
            }
        }
    }
}