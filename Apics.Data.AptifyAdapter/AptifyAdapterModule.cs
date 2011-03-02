using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using Apics.Data.AptifyAdapter.Configuration;
using Apics.Data.AptifyAdapter.Event;
using Apics.Data.Dialect;
using log4net;
using Ninject;
using Ninject.Modules;
using Aptify.Framework.Application;
using System.Security.Permissions;
using Apics.Data.AptifyAdapter.ADO;
using Aptify.Framework.DataServices;

namespace Apics.Data.AptifyAdapter
{
    /// <summary>
    /// Dependency injection module to initialize the aptify adapter properly
    /// </summary>
    public class AptifyAdapterModule : NinjectModule
    {
        private static readonly ILog Log = LogManager.GetLogger( typeof( AptifyAdapterModule ) );
        private readonly string connectionString;

        public AptifyAdapterModule( string connectionString )
        {
            this.connectionString = connectionString;
        }

        #region [ NinjectModule Overrides ]

        public override void Load( )
        {
            // Make sure to load DLLs from the DB if required
            AppDomain.CurrentDomain.AssemblyResolve += OnAssembyResolve;

            // Setup the connection string builder for future requests
            Bind<AptifyConnectionStringBuilder>( ).ToSelf( ).InSingletonScope( )
                .WithConstructorArgument( "connectionString", this.connectionString );
            Bind<UserCredentials>( ).ToMethod( c => c.Kernel.Get<AptifyConnectionStringBuilder>( ).Credentials );

            Bind<IDialect>( ).To<AptifyDialect>( ).InSingletonScope( );
            Bind<AptifyServer>( ).ToSelf( ).InSingletonScope( );

            Bind<AptifyApplication>( ).ToMethod( c =>
            {
                var parameter = c.Kernel.Get<AptifyConnectionStringBuilder>( ).Credentials;
                return new AptifyApplication( parameter, false );
            } ).InRequestScope( );

            // Composite event handler
            Bind<IEventHandler>( ).ToMethod( c =>
                new EventHandlerComposite(
                    c.Kernel.Get<NHibernateSaveEvent>( ),
                    c.Kernel.Get<NHibernateUpdateEvent>( ),
                    c.Kernel.Get<NHibernateDeleteEvent>( ) ) );

            Bind<IDataStore>( ).To<AptifyDataStore>( )
                .WithConstructorArgument( "provider", typeof( AptifyConnectionProvider ) );

            Log.Info( "Loaded AptifyAdapterModule" );
        }

        #endregion [ NinjectModule Overrides ]

        #region [ Aptify Assembly Loading ]

        private Assembly OnAssembyResolve( object sender, ResolveEventArgs args )
        {
            // Invalid arguments, we can't use them
            if ( args == null || String.IsNullOrEmpty( args.Name ) )
                return null;

            var filename = new AssemblyName( args.Name ).Name;

            if ( !filename.EndsWith( ".dll" ) )
                filename += ".dll";

            try
            {
                return Assembly.LoadFrom(
                    Path.Combine( @"C:\Program Files\Aptify 5.0\", filename ) );
            }
            catch ( FileNotFoundException )
            {
                return null;
            }

        }

        #endregion [ Aptify Assembly Loading ]
    }
}