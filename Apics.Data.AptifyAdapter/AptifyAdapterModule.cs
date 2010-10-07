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

namespace Apics.Data.AptifyAdapter
{
    /// <summary>
    /// Dependency injection module to intialize the aptify adapter properly
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
            // Make sure to load dlls from the DB if required
            AppDomain.CurrentDomain.AssemblyResolve += OnAssembyResolve;

            Bind<IDialect>( ).To<AptifyDialect>( ).InSingletonScope( );
            Bind<AptifyServer>( ).ToSelf( ).InSingletonScope( );

            Bind<AptifyConnectionStringBuilder>( ).ToSelf( ).InSingletonScope( )
                .WithConstructorArgument( "connectionString", this.connectionString );

            // Composite event handler
            Bind<IEventHandler>( ).ToMethod( c =>
                new EventHandlerComposite(
                    c.Kernel.Get<NHibernateSaveEvent>( ),
                    c.Kernel.Get<NHibernateUpdateEvent>( ),
                    c.Kernel.Get<NHibernateDeleteEvent>( ) ) );

            Bind<IDataStore>( ).To<AptifyDataStore>( )
                .WithConstructorArgument( "provider", typeof( AptifyConnectionProvider ) );

            Log.Info( "Loaded AptifyAdapterModule" );

            // Try loading the configuration for which Aptify modules to load
            var configuration = ( AptifyModuleSettingConfiguration )ConfigurationManager.GetSection( "aptify.modules" );

            // Did not find the configuration
            if( configuration == null )
                return;

            // Load all the forced load assemblies
            foreach ( AptifyModule module in configuration.Modules )
            {
                if ( File.Exists( Path.GetFullPath( module.Type ) ) )
                    Assembly.LoadFile( Path.GetFullPath( module.Type ) );
            }
        }

        #endregion [ NinjectModule Overrides ]

        #region [ Aptify Assembly Loading ]

        private Assembly OnAssembyResolve( object sender, ResolveEventArgs args )
        {
            // Invalid arguments, we can't use them
            if( args == null || String.IsNullOrEmpty( args.Name ) )
                return null;

            using( var connection = new SqlConnection( this.connectionString ) )
            {
                try
                {
                    connection.Open( );

                    string filename = GetFilenameFromReference( args.Name );
                    byte[ ] rawFile = LoadFileFromDatabase( connection, filename );

                    if( rawFile != null )
                    {
                        try
                        {
                            // Cache the DLL into our working folder
                            File.WriteAllBytes( filename, rawFile );

                            Log.InfoFormat( "Successfully downloaded {0} from Aptify database.", args.Name );
                        }
                        catch( Exception ex )
                        {
                            Log.WarnFormat( "Could not cache {0}: {1}", filename, ex );
                        }

                        // Return the true assembly
                        return Assembly.Load( rawFile );
                    }
                }
                catch( DbException ex )
                {
                    // Problem with loading the DLL from the database
                    Log.ErrorFormat( "Exception while loading {0} from Aptify database: {1}", args.Name, ex );
                }
            }

            return null;
        }

        /// <summary>
        /// Loads a stored file from the database into memory
        /// </summary>
        /// <param name="connection">Database connection</param>
        /// <param name="filename">Filename within the database</param>
        /// <returns>Byte array of the binary data</returns>
        private static byte[ ] LoadFileFromDatabase( IDbConnection connection, string filename )
        {
            // Setup the command and parameters
            using( IDbCommand command = connection.CreateCommand( ) )
            {
                command.CommandText = Queries.GetAptifyDLLByName;
                command.CommandType = CommandType.Text;

                // Create the name parameter
                IDbDataParameter nameParameter = command.CreateParameter( );
                nameParameter.ParameterName = "@name";
                nameParameter.DbType = DbType.String;
                nameParameter.Value = filename;

                command.Parameters.Add( nameParameter );

                return ( byte[ ] )command.ExecuteScalar( );
            }
        }

        /// <summary>
        /// Parses a reference name into a filename
        /// </summary>
        /// <param name="referenceName">Original reference name</param>
        /// <returns>Filename that matches this reference</returns>
        private static string GetFilenameFromReference( string referenceName )
        {
            if( referenceName == null )
                throw new ArgumentNullException( referenceName );

            string dllName = referenceName.Split( ',' )[ 0 ];

            if( !dllName.EndsWith( ".dll" ) )
                dllName += ".dll";

            return dllName;
        }

        #endregion [ Aptify Assembly Loading ]
    }
}