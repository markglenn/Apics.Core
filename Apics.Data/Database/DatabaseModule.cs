using System;
using System.Linq;
using Ninject.Modules;
using Apics.Data.Dialect;

namespace Apics.Data.Database
{
    public class DatabaseModule : NinjectModule
    {
        private readonly string connectionString;

        public DatabaseModule( string connectionString )
        {
            this.connectionString = connectionString;
        }

        public override void Load( )
        {
            Bind<IDialect>( ).To<SqlServer2000Dialect>( ).InSingletonScope( );
            Bind<IDataStore>( ).To<DatabaseDataStore>( ).InSingletonScope( )
                .WithConstructorArgument( "connectionString", this.connectionString );
        }
    }
}
