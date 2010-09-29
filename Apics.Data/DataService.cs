using System;
using System.Linq;
using Ninject;
using Ninject.Modules;
using System.Configuration;
using System.Reflection;
using log4net;
using Ninject.Parameters;
using Apics.Data.Database;
using Apics.Utilities.Module;

namespace Apics.Data
{
    /// <summary>
    /// Database access service
    /// </summary>
    public class DataService : IDisposable
    {
        private static readonly ILog Log = LogManager.GetLogger( typeof( DataService ) );

        private readonly IDataStore dataStore;
        private readonly IKernel kernel;
        
        #region [ Public Properties ]

        public IDataStore DataStore
        {
            get { return this.dataStore; }
        }

        #endregion [ Public Properties ]

        #region [ Constructors ]

        public DataService( )
            : this( "apics.dependency", "apics.data" )
        {
        }

        public DataService( string dependencySection, string dataSection )
            : this( NinjectFactory.Create( dependencySection ),
                ( DatabaseConfigurationSection )ConfigurationManager.GetSection( dataSection ) )
        {
        }

        public DataService( IKernel kernel, DatabaseConfigurationSection config )
        {
            // Create the DI kernel
            this.kernel = kernel;

            // Load any configured adapter module
            LoadAdapterModule( config.AdapterModule, config.ConnectionString );

            // Make sure the model assembly is loaded
            if( !String.IsNullOrEmpty( config.ModelAssembly ) )
            {
                Log.InfoFormat( "Loading model assembly: {0}", config.ModelAssembly );
                Assembly.Load( config.ModelAssembly );
            }

            // Create the datastore object using DI
            this.dataStore = this.kernel.Get<IDataStore>(
                new ConstructorArgument( "connectionString", config.ConnectionString ) );

            // Make sure to initialize the datastore correctly
            this.dataStore.Initialize( );
        }

        #endregion [ Constructors ]

      
        /// <summary>
        /// Loads an adapter module into the DI kernel
        /// </summary>
        /// <param name="adapterModule">Adapter module type</param>
        /// <param name="connectionString">Connection string</param>
        private void LoadAdapterModule( string adapterModule, string connectionString )
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

            this.kernel.Bind( moduleType ).ToSelf( );

            var module = ( INinjectModule )this.kernel.Get( moduleType,
                new ConstructorArgument( "connectionString", connectionString ) );

            this.kernel.Load( module );
        }

        ~DataService( )
        {
            Dispose( false );
        }

        protected virtual void Dispose( bool disposing )
        {
            if( !disposing ) 
                return;

            this.kernel.Dispose( );

            if( this.dataStore != null )
                this.dataStore.Dispose( );
        }

        #region [ IDisposable Members ]

        public void Dispose( )
        {
            Dispose( true );
            GC.SuppressFinalize( this );
        }

        #endregion [ IDisposable Members ]

    }
}
