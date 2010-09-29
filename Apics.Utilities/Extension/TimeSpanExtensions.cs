using System;
using System.Linq;

namespace Apics.Utilities.Extension
{
    public static class TimeSpanExtensions
    {
        private const int DaysInWeek = 7;

        /// <summary>
        /// Returns a TimeSpan representing the specified number of ticks.
        /// </summary>
        public static TimeSpan Ticks( this int ticks )
        {
            return TimeSpan.FromTicks( ticks );
        }

        /// <summary>
        /// Returns a TimeSpan representing the specified number of milliseconds.
        /// </summary>
        public static TimeSpan Milliseconds( this int milliseconds )
        {
            return TimeSpan.FromMilliseconds( milliseconds );
        }

        /// <summary>
        /// Returns a TimeSpan representing the specified number of seconds.
        /// </summary>
        public static TimeSpan Seconds( this int seconds )
        {
            return TimeSpan.FromSeconds( seconds );
        }

        /// <summary>
        /// Returns a TimeSpan representing the specified number of minutes.
        /// </summary>
        public static TimeSpan Minutes( this int minutes )
        {
            return TimeSpan.FromMinutes( minutes );
        }

        /// <summary>
        /// Returns a TimeSpan representing the specified number of hours.
        /// </summary>
        public static TimeSpan Hours( this int hours )
        {
            return TimeSpan.FromHours( hours );
        }

        /// <summary>
        /// Returns a TimeSpan representing the specified number of days.
        /// </summary>
        public static TimeSpan Days( this int days )
        {
            return TimeSpan.FromDays( days );
        }

        /// <summary>
        /// Returns a timespan representing the number of weeks
        /// </summary>
        /// <param name="weeks">Number of weeks</param>
        /// <returns>Timespan of the number of weeks</returns>
        public static TimeSpan Weeks( this int weeks )
        {
            return TimeSpan.FromDays( DaysInWeek * weeks );
        }

        /// <summary>
        /// Returns a timespan representing the number of years
        /// </summary>
        /// <param name="years">Number of years</param>
        /// <returns>Timespan of the number of years</returns>
        public static TimeSpan Years( this int years )
        {
            return DateTime.Now.AddYears( years ) - DateTime.Now;
        }
    }
}