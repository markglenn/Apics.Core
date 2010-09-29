using System;
using System.Linq;
using Apics.Utilities.Extension;
using NUnit.Framework;

namespace Apics.Utilities.Tests.Extension
{
    [TestFixture]
    public class DateTimeExtensionsTests
    {
        [Test]
        public void First_ReturnsFirstOfMonth( )
        {
            Assert.AreEqual( 1.July( 2010 ), 17.July( 2010 ).First( ) );
        }

        [Test]
        public void IsWeekend_ReturnsTrueIfWeekend( )
        {
            Assert.IsTrue( new DateTime( 2010, 7, 18 ).IsWeekend( ) );
            Assert.IsFalse( new DateTime( 2010, 7, 19 ).IsWeekend( ) );
        }

        [Test]
        public void Last_ReturnsLastDayOfMonth( )
        {
            Assert.AreEqual( 31.July( 2010 ), 17.July( 2010 ).Last( ) );
        }

        [Test]
        public void NextBusinessDay_ReturnsNextBusinessDay( )
        {
            Assert.AreEqual( 19.July( 2010 ), 17.July( 2010 ).NextBusinessDay( ) );
            Assert.AreEqual( 19.July( 2010 ), 18.July( 2010 ).NextBusinessDay( ) );
            Assert.AreEqual( 20.July( 2010 ), 19.July( 2010 ).NextBusinessDay( ) );
        }
    }
}