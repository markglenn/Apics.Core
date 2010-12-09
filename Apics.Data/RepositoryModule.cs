using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject.Modules;
using Apics.Data;
using System.Reflection;

namespace Apics.Data
{
    public class RepositoryModule : NinjectModule
    {
        private readonly IDataService service;
        private readonly MethodInfo createRepositoryMethod;

        public RepositoryModule( IDataService service )
        {
            if ( service == null )
                throw new ArgumentNullException( "service" );

            this.service = service;
            this.createRepositoryMethod = service.DataStore.GetType( ).GetMethod( "Repository" );
        }

        public override void Load( )
        {
            Bind( typeof( IRepository<> ) ).ToMethod( m =>
            {
                var method = this.createRepositoryMethod.MakeGenericMethod( m.GenericArguments );
                return method.Invoke( this.service.DataStore, new Object[ ] { } );
            } );
        }
    }
}
