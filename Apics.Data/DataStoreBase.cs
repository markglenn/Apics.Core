using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Apics.Data.Database;
using Apics.Data.Dialect;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;
using Castle.ActiveRecord.Framework.Config;
using NHibernate.ByteCode.Castle;
using NHibernate.Cfg;
using ConfigEnvironment = NHibernate.Cfg.Environment;

namespace Apics.Data
{
	public abstract class DataStoreBase : IDataStore
	{
		#region [ Private Members ]

		protected readonly IDictionary<string, string> Properties;

		#endregion [ Private Members ]

	    protected DataStoreBase( IEventHandler handler, IDialect dialect, 
			Type connectionProvider, string connectionString )
		{
			if ( dialect == null )
				throw new ArgumentNullException( "dialect" );

			if ( connectionProvider == null )
				throw new ArgumentNullException( "connectionProvider" );

			if ( String.IsNullOrEmpty( connectionString ) )
				throw new ArgumentException( "connectionString" );

			// Set the defaults of the properties
			this.Properties = new Dictionary<string, string> {
				{ ConfigEnvironment.ConnectionDriver, dialect.DriverClass },
				{ ConfigEnvironment.Dialect, dialect.Dialect },
				{ ConfigEnvironment.ConnectionString, connectionString },
				{ ConfigEnvironment.ProxyFactoryFactoryClass, typeof( ProxyFactoryFactory ).AssemblyQualifiedName },
				{ ConfigEnvironment.ConnectionProvider, connectionProvider.AssemblyQualifiedName }
			};

			// Setup the advanced properties
			SetAdvancedConfiguration( handler );
		}

		public void Initialize( )
		{
			var source = new InPlaceConfigurationSource( );
			source.Add( typeof( ActiveRecordBase ), this.Properties );

			ActiveRecordStarter.Initialize( GetDataAccessAssemblies( ), source );
		}

	    #region [ Private Methods ]

		/// <summary>
		/// Set up advanced configurations in NHibernate
		/// </summary>
		private void SetAdvancedConfiguration( IEventHandler handler )
		{
			ActiveRecordStarter.SessionFactoryHolderCreated += holder =>
			{
				holder.OnRootTypeRegistered += ( sender, rootType ) =>
				{
					var nhConfig = ( ( ISessionFactoryHolder )sender ).GetConfiguration( rootType );

					if ( handler != null )
						handler.Register( nhConfig.EventListeners );

					// Can set the event handlers here
					OnSetAdvancedConfiguration( nhConfig );
				};
			};
		}

		#endregion [ Private Methods ]

		#region [ Overridable Advanced Methods ]

		protected virtual void OnSetAdvancedConfiguration( Configuration nhibernateConfig )
		{
            nhibernateConfig.SetProperty( ConfigEnvironment.ShowSql, "true" );
			// Do nothing
		}

		/// <summary>
		/// Find all the data model assemblies based on what assemblies are being referenced
		/// </summary>
		/// <returns>An array of all loaded assemblies that reference Castle.ActiveRecord</returns>
		protected virtual Assembly[ ] GetDataAccessAssemblies( )
		{
			IEnumerable<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies( );

			assemblies = assemblies.Where( a => a.GetReferencedAssemblies( ).Any( 
				ra => ra.FullName.ToLowerInvariant( ).Contains( "castle.activerecord" ) ) );

			return assemblies.ToArray( );
		}

		#endregion [ Overridable Advanced Methods ]

		#region [ IDataStore Members ]

	    /// <summary>
	    /// Creates a repository object that points to a data store
	    /// </summary>
	    /// <typeparam name="T">Type of the repository</typeparam>
	    /// <returns>Repository that points to the data store</returns>
	    public virtual IRepository<T> Repository<T>( ) where T : class
		{
			return new DatabaseRepository<T>( );
		}

	    /// <summary>
	    /// Creates a transaction object that is automatically rolled back if not committed
	    /// </summary>
	    /// <returns>Transaction object</returns>
	    public virtual ITransaction CreateTransaction( )
		{
			return new Transaction( );
		}

	    /// <summary>
	    /// Creates a general connection to the database
	    /// </summary>
	    /// <returns>A new connection to the database</returns>
	    public abstract IDbConnection CreateConnection( );

        public virtual ISession CreateSession( )
        {
            return new DatabaseSession( new SessionScope( FlushAction.Never ) );
        }

		#endregion [ IDataStore Members ]

		#region [ IDisposable Members ]

        ~DataStoreBase( )
		{
			Dispose( false );
		}

		public void Dispose( )
		{
			Dispose( true );
			GC.SuppressFinalize( this );
		}

		protected virtual void Dispose( bool disposing )
		{
			
		}

		#endregion [ IDisposable Members ]

    }
}