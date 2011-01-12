using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Ninject;

namespace Apics.Data.AptifyAdapter.IntegrationTests
{
    public abstract class AptifyIntegrationTestsBase
    {
        protected IKernel Kernel { get; set; }
        protected IDataService Service { get; set; }
        protected ITransaction Transaction { get; set; }

        protected IRepository<T> GetRepository<T>( ) where T : class
        {
            return Service.DataStore.Repository<T>( );
        }

        [TestFixtureSetUp]
        public void FixtureSetup( )
        {
            Kernel = new StandardKernel( );
            Service = new DataService( Kernel,
                @"Data Source=192.168.9.204\APTIFYDEV;Initial Catalog=Aptify;User ID=sa;Password=sa@SQL082K8;",
                typeof( Apics.Model.Fulfillment.Order ).Assembly.FullName,
                typeof( Apics.Data.AptifyAdapter.AptifyAdapterModule ).AssemblyQualifiedName );

            AppDomain.CurrentDomain.Load( "AptifyOrdersEntity.dll" );
            AppDomain.CurrentDomain.Load( "AptifySecurityKey.dll" );
        }

        [TestFixtureTearDown]
        public void FixtureTeardown( )
        {
            Service.Dispose( );
            Kernel.Dispose( );
        }
        
        [SetUp]
        public void AptifyIntegrationTestsBase_Setup( )
        {
            Transaction = Service.DataStore.CreateTransaction( );
        }

        [TearDown]
        public void AptifyIntegrationTestsBase_TearDown( )
        {
            Transaction.Dispose( );
        }
    }
}
