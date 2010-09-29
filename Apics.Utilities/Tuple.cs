using System;
using System.Linq;

namespace Apics.Utilities
{
    public static class Tuple
    {
        public static Tuple<TFirst> Create<TFirst>( TFirst first )
        {
            return new Tuple<TFirst>( first );
        }

        public static Tuple<TFirst, TSecond> Create<TFirst, TSecond>( TFirst first, TSecond second )
        {
            return new Tuple<TFirst, TSecond>( first, second );
        }

        public static Tuple<TFirst, TSecond, TThird> Create<TFirst, TSecond, TThird>( TFirst first, TSecond second,
            TThird third )
        {
            return new Tuple<TFirst, TSecond, TThird>( first, second, third );
        }

        public static Tuple<TFirst, TSecond, TThird, TFourth> Create<TFirst, TSecond, TThird, TFourth>(
            TFirst first, TSecond second, TThird third, TFourth fourth )
        {
            return new Tuple<TFirst, TSecond, TThird, TFourth>( first, second, third, fourth );
        }
    }

    public class Tuple<TFirst>
    {
        private readonly TFirst first;

        #region [ Public Properties ]

        public TFirst First
        {
            get { return this.first; }
        }

        #endregion [ Public Properties ]

        internal Tuple( TFirst first )
        {
            this.first = first;
        }

        public override string ToString( )
        {
            return String.Format( "[{0}]", First );
        }
    }

    public class Tuple<TFirst, TSecond> : Tuple<TFirst>
    {
        private readonly TSecond second;

        #region [ Public Properties ]

        public TSecond Second
        {
            get { return this.second; }
        }

        #endregion [ Public Properties ]

        internal Tuple( TFirst first, TSecond second )
            : base( first )
        {
            this.second = second;
        }

        public override string ToString( )
        {
            return String.Format( "[{0}, {1}]", First, Second );
        }
    }

    public class Tuple<TFirst, TSecond, TThird> : Tuple<TFirst, TSecond>
    {
        private readonly TThird third;

        #region [ Public Properties ]

        public TThird Third
        {
            get { return this.third; }
        }

        #endregion [ Public Properties ]

        internal Tuple( TFirst first, TSecond second, TThird third )
            : base( first, second )
        {
            this.third = third;
        }

        public override string ToString( )
        {
            return String.Format( "[{0}, {1}, {2}]", First, Second, Third );
        }
    }

    public class Tuple<TFirst, TSecond, TThird, TFourth> : Tuple<TFirst, TSecond, TThird>
    {
        #region [ MyRegion ]

        private readonly TFourth fourth;

        #endregion [ MyRegion ]

        #region [ Public Properties ]

        public TFourth Fourth
        {
            get { return this.fourth; }
        }

        #endregion [ Public Properties ]

        internal Tuple( TFirst first, TSecond second, TThird third, TFourth fourth )
            : base( first, second, third )
        {
            this.fourth = fourth;
        }

        public override string ToString( )
        {
            return String.Format( "[{0}, {1}, {2}, {3}]", First, Second, Third, Fourth );
        }
    }
}