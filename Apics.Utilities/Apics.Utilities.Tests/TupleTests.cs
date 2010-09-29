using System;
using System.Linq;
using NUnit.Framework;

namespace Apics.Utilities.Tests
{
    [TestFixture]
    public class TupleTests
    {
        [Test]
        public void Create_CreatesTuple( )
        {
            const string t1 = "Hello";
            const string t2 = "World";
            const string t3 = "Again";
            const string t4 = "!";

            Tuple<string> tuple1 = Tuple.Create( t1 );
            Tuple<string, string> tuple2 = Tuple.Create( t1, t2 );
            Tuple<string, string, string> tuple3 = Tuple.Create( t1, t2, t3 );
            Tuple<string, string, string, string> tuple4 = Tuple.Create( t1, t2, t3, t4 );

            Assert.AreEqual( t1, tuple1.First );

            Assert.AreEqual( t1, tuple2.First );
            Assert.AreEqual( t2, tuple2.Second );

            Assert.AreEqual( t1, tuple3.First );
            Assert.AreEqual( t2, tuple3.Second );
            Assert.AreEqual( t3, tuple3.Third );

            Assert.AreEqual( t1, tuple4.First );
            Assert.AreEqual( t2, tuple4.Second );
            Assert.AreEqual( t3, tuple4.Third );
            Assert.AreEqual( t4, tuple4.Fourth );
        }


        [Test]
        public void Create_InfersType( )
        {
            var t1 = new Object( );
            const int t2 = 5;
            const string t3 = "Hello";
            Tuple<int, string, Tuple<int, int>> t4 = Tuple.Create( 4, "test", Tuple.Create( 5, 1 ) );

            Tuple<object, int, string, Tuple<int, string, Tuple<int, int>>> tuple4 = Tuple.Create( t1, t2, t3, t4 );

            Assert.IsAssignableFrom( t1.GetType( ), tuple4.First );
            Assert.IsAssignableFrom( t2.GetType( ), tuple4.Second );
            Assert.IsAssignableFrom( t3.GetType( ), tuple4.Third );
            Assert.IsAssignableFrom( t4.GetType( ), tuple4.Fourth );
        }

        [Test]
        public void ToString_CreatesStringRepresentation( )
        {
            const string t1 = "Hello";
            const string t2 = "World";
            const string t3 = "Again";
            const string t4 = "!";

            Tuple<string> tuple1 = Tuple.Create( t1 );
            Tuple<string, string> tuple2 = Tuple.Create( t1, t2 );
            Tuple<string, string, string> tuple3 = Tuple.Create( t1, t2, t3 );
            Tuple<string, string, string, string> tuple4 = Tuple.Create( t1, t2, t3, t4 );

            Assert.AreEqual( "[Hello]", tuple1.ToString( ) );
            Assert.AreEqual( "[Hello, World]", tuple2.ToString( ) );
            Assert.AreEqual( "[Hello, World, Again]", tuple3.ToString( ) );
            Assert.AreEqual( "[Hello, World, Again, !]", tuple4.ToString( ) );
        }
    }
}