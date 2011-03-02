using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Moq;
using Aptify.Framework.Application;

namespace Apics.Data.AptifyAdapter.Tests
{
    [TestFixture]
    public class AptifyServerTests
    {
        private Mock<AptifyApplication> application;

        [SetUp]
        public void Setup( )
        {
            this.application = new Mock<AptifyApplication>( );
        }

        [Test]
        public void Ctor_NullApplicationThrowsArgumentNullException( )
        {
            Assert.Throws<ArgumentNullException>( ( ) => new AptifyServer( null ) );
        }
    }
}
