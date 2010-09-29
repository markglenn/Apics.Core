using System;
using System.Linq;

namespace Apics.Utilities.Extension
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Determines if this date falls on a weekend
        /// </summary>
        /// <param name="date">Date to check</param>
        /// <returns>True if this date </returns>
        public static bool IsWeekend( this DateTime date )
        {
            switch( date.DayOfWeek )
            {
                case DayOfWeek.Saturday:
                case DayOfWeek.Sunday:
                    return true;

                default:
                    return false;
            }
        }

        public static bool IsWeekday( this DateTime date )
        {
            return !date.IsWeekend( );
        }

        #region [ Day Finder Extensions ]

        public static DateTime NextBusinessDay( this DateTime date )
        {
            do
            {
                date = date.AddDays( 1 );
            } while( date.IsWeekend( ) );

            return date;
        }

        public static DateTime First( this DateTime date )
        {
            return date.AddDays( 1 - date.Day );
        }

        /// <summary>
        /// Gets a DateTime representing the first specified day in the current month
        /// </summary>
        /// <param name="current">The current day</param>
        /// <param name="dayOfWeek">The current day of week</param>
        /// <returns></returns>
        public static DateTime First( this DateTime current, DayOfWeek dayOfWeek )
        {
            DateTime first = current.First( );

            if( first.DayOfWeek != dayOfWeek )
                first = first.Next( dayOfWeek );

            return first;
        }

        /// <summary>
        /// Gets a DateTime representing the last day in the current month
        /// </summary>
        /// <param name="date">The current date</param>
        /// <returns>Last day of this month</returns>
        public static DateTime Last( this DateTime date )
        {
            return date.AddMonths( 1 ).AddDays( -date.Day );
        }

        /// <summary>
        /// Gets a DateTime representing the last specified day in the current month
        /// </summary>
        /// <param name="current">The current date</param>
        /// <param name="dayOfWeek">The current day of week</param>
        /// <returns></returns>
        public static DateTime Last( this DateTime current, DayOfWeek dayOfWeek )
        {
            DateTime last = current.Last( );

            last = last.AddDays( Math.Abs( dayOfWeek - last.DayOfWeek ) * -1 );
            return last;
        }

        /// <summary>
        /// Gets a DateTime representing the first date following the current date which falls on the given day of the week
        /// </summary>
        /// <param name="current">The current date</param>
        /// <param name="dayOfWeek">The day of week for the next date to get</param>
        public static DateTime Next( this DateTime current, DayOfWeek dayOfWeek )
        {
            int offsetDays = dayOfWeek - current.DayOfWeek;

            if( offsetDays <= 0 )
            {
                offsetDays += 7;
            }

            DateTime result = current.AddDays( offsetDays );

            return result;
        }

        #endregion [ Day Finder Extensions ]

        #region [ Quick Date Extensions ]

        /// <summary>
        /// Returns a DateTime representing the specified day in January
        /// in the specified year.
        /// </summary>
        public static DateTime January( this int day, int year )
        {
            return new DateTime( year, 1, day );
        }

        /// <summary>
        /// Returns a DateTime representing the specified day in February
        /// in the specified year.
        /// </summary>
        public static DateTime February( this int day, int year )
        {
            return new DateTime( year, 2, day );
        }

        /// <summary>
        /// Returns a DateTime representing the specified day in March
        /// in the specified year.
        /// </summary>
        public static DateTime March( this int day, int year )
        {
            return new DateTime( year, 3, day );
        }

        /// <summary>
        /// Returns a DateTime representing the specified day in April
        /// in the specified year.
        /// </summary>
        public static DateTime April( this int day, int year )
        {
            return new DateTime( year, 4, day );
        }

        /// <summary>
        /// Returns a DateTime representing the specified day in May
        /// in the specified year.
        /// </summary>
        public static DateTime May( this int day, int year )
        {
            return new DateTime( year, 5, day );
        }

        /// <summary>
        /// Returns a DateTime representing the specified day in June
        /// in the specified year.
        /// </summary>
        public static DateTime June( this int day, int year )
        {
            return new DateTime( year, 6, day );
        }

        /// <summary>
        /// Returns a DateTime representing the specified day in July
        /// in the specified year.
        /// </summary>
        public static DateTime July( this int day, int year )
        {
            return new DateTime( year, 7, day );
        }

        /// <summary>
        /// Returns a DateTime representing the specified day in August
        /// in the specified year.
        /// </summary>
        public static DateTime August( this int day, int year )
        {
            return new DateTime( year, 8, day );
        }

        /// <summary>
        /// Returns a DateTime representing the specified day in September
        /// in the specified year.
        /// </summary>
        public static DateTime September( this int day, int year )
        {
            return new DateTime( year, 9, day );
        }

        /// <summary>
        /// Returns a DateTime representing the specified day in October
        /// in the specified year.
        /// </summary>
        public static DateTime October( this int day, int year )
        {
            return new DateTime( year, 10, day );
        }

        /// <summary>
        /// Returns a DateTime representing the specified day in November
        /// in the specified year.
        /// </summary>
        public static DateTime November( this int day, int year )
        {
            return new DateTime( year, 11, day );
        }

        /// <summary>
        /// Returns a DateTime representing the specified day in December
        /// in the specified year.
        /// </summary>
        public static DateTime December( this int day, int year )
        {
            return new DateTime( year, 12, day );
        }

        #endregion [ Quick Date Extensions ]

        #region [ Time Extensions ]

        /// <summary>
        /// Gets a DateTime representing midnight on the current date
        /// </summary>
        /// <param name="current">The current date</param>
        public static DateTime Midnight( this DateTime current )
        {
            return new DateTime( current.Year, current.Month, current.Day );
        }

        /// <summary>
        /// Gets a DateTime representing noon on the current date
        /// </summary>
        /// <param name="current">The current date</param>
        public static DateTime Noon( this DateTime current )
        {
            return new DateTime( current.Year, current.Month, current.Day, 12, 0, 0 );
        }

        /// <summary>
        /// Sets the time of the current date with minute precision
        /// </summary>
        /// <param name="current">The current date</param>
        /// <param name="hour">The hour</param>
        /// <param name="minute">The minute</param>
        public static DateTime SetTime( this DateTime current, int hour, int minute )
        {
            return SetTime( current, hour, minute, 0, 0 );
        }

        /// <summary>
        /// Sets the time of the current date with second precision
        /// </summary>
        /// <param name="current">The current date</param>
        /// <param name="hour">The hour</param>
        /// <param name="minute">The minute</param>
        /// <param name="second">The second</param>
        /// <returns></returns>
        public static DateTime SetTime( this DateTime current, int hour, int minute, int second )
        {
            return SetTime( current, hour, minute, second, 0 );
        }

        /// <summary>
        /// Sets the time of the current date with millisecond precision
        /// </summary>
        /// <param name="current">The current date</param>
        /// <param name="hour">The hour</param>
        /// <param name="minute">The minute</param>
        /// <param name="second">The second</param>
        /// <param name="millisecond">The millisecond</param>
        /// <returns></returns>
        public static DateTime SetTime( this DateTime current, int hour, int minute, int second, int millisecond )
        {
            return new DateTime( current.Year, current.Month, current.Day, hour, minute, second, millisecond );
        }

        #endregion [ Time Extensions ]

        #region [ Relative Dates ]

        public static DateTime Ago( this TimeSpan span )
        {
            return DateTime.Now - span;
        }

        public static DateTime FromNow( this TimeSpan span )
        {
            return span.Ago( );
        }

        public static DateTime AgoSince( this TimeSpan span, DateTime reference )
        {
            return reference - span;
        }

        public static DateTime From( this TimeSpan span, DateTime reference )
        {
            return reference + span;
        }

        #endregion [ Relative Dates ]
    }
}