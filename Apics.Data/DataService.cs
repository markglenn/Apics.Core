using System;
using System.Linq;
using Ninject;
using Ninject.Modules;
using System.Configuration;
using System.Reflection;
using log4net;
using Ninject.Parameters;
using Apics.Data.Database;

namespace Apics.Data
{
    /// <summary>
    /// Database access service
    /// </summary>
    public class DataService : IDataService
    {
        #region [ Private Members ]
        
        private static readonly ILog Log = LogManager.GetLogger( typeof( DataService ) );

        private readonly IDataStore dataStore;

        #endregion [ Private Members ]
        
        #region [ Public Properties ]

        public IDataStore DataStore
        {
            get { return this.dataStore; }
        }

        #endregion [ Public Properties ]

        #region [ Constructors ]

        /// <summary>
        /// Creates a data service using a config file
        /// </summary>
        /// <param name="kernel">Dependency injection kernel</param>
        /// <param name="dataSection">Configuration section name</param>
        public DataService( IKernel kernel, string dataSection )
            : this( kernel, 
                ( DatabaseConfigurationSection )ConfigurationManager.GetSection( dataSection ) )
        {
        }

        /// <summary>
        /// Creates a data service using a config file
        /// </summary>
        /// <param name="kernel">Dependency injection kernel</param>
        /// <param name="config">Configuration section name</param>
        public DataService( IKernel kernel, DatabaseConfigurationSection config )
            : this( kernel, config.ConnectionString, config.ModelAssembly, config.AdapterModule )
        {
        }

        public DataService( IKernel kernel, string connectionString, string modelAssembly ) 
            : this( kernel, connectionString, modelAssembly, null )
        {
        }

        public DataService( IKernel kernel, string connectionString, string modelAssembly, 
            string adapterModule )
        {
            // Load any configured adapter module);
            LoadAdapterModule( kernel, adapterModule, connectionString );

            // Make sure the model assembly is loaded
            if ( !String.IsNullOrEmpty( modelAssembly ) )
            {
                Log.InfoFormat( "Loading model assembly: {0}", modelAssembly );
                Assembly.Load( modelAssembly );
            }

            // Create the datastore object using DI
            this.dataStore = kernel.Get<IDataStore>(
                new ConstructorArgument( "connectionString", connectionString ) );

            // Make sure to initialize the datastore correctly
            this.dataStore.Initialize( );

            // Load up the repository module so we can do DI on the repositories
            kernel.Load( new RepositoryModule( this ) );
        }

        #endregion [ Constructors ]

        /// <summary>
        /// Loads an adapter module into the DI kernel
        /// </summary>
        /// <param name="kernel">Dependency injection kernel</param>
        /// <param name="adapterModule">Adapter module type</param>
        /// <param name="connectionString">Connection string</param>
        private static void LoadAdapterModule( IKernel kernel, string adapterModule, 
            string connectionString )
        {
            Type moduleType = Type.GetType( adapterModule, false );

            // Couldn't find the module
            if( moduleType == null )
            {
                // A module was requested, but it's missing
                if( !String.IsNullOrEmpty( adapterModule ) )
                    throw new InvalidOperationException( "Could not find adapter module: " + adapterModule );

                // Default to the database adapter module
                moduleType = typeof( DatabaseModule );
            }

            Log.InfoFormat( "Using database adapter: {0}", moduleType.Name );

            kernel.Bind( moduleType ).ToSelf( );

            var module = ( INinjectModule )kernel.Get( moduleType,
                new ConstructorArgument( "connectionString", connectionString ) );

            kernel.Load( module );
        }

        #region [ IDisposable Members ]

        public void Dispose( )
        {
            Dispose( true );
            GC.SuppressFinalize( this );
        }

        ~DataService( )
        {
            Dispose( false );
        }

        protected virtual void Dispose( bool disposing )
        {
            if ( !disposing )
                return;

            if ( this.dataStore != null )
                this.dataStore.Dispose( );
        }

        #endregion [ IDisposable Members ]
    }
}
