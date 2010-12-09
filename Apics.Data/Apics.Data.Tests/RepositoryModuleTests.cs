using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
using NUnit.Framework;
using Apics.Data;
using Moq;

namespace Apics.Data.Tests
{
    [TestFixture]
    public class RepositoryModuleTests
    {
        private Mock<IDataService> service = new Mock<IDataService>( );
        private Mock<IRepository<Object>> repository = new Mock<IRepository<Object>>( );

        [SetUp]
        public void Setup( )
        {
            service.Setup( s => s.DataStore.Repository<Object>( ) )
                .Returns( repository.Object );
        }

        [Test]
        public void Binds_ToDataService( )
        {
            var kernel = new StandardKernel( );

            var module = new RepositoryModule( service.Object );
            kernel.Load( module );

            Assert.AreSame( repository.Object, kernel.Get<IRepository<Object>>( ) );
    
        }
    }
}
