using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace Apics.Utilities.Extension
{
    public static class StreamExtensions
    {
        /// <summary>
        /// Writes a stream to another stream
        /// </summary>
        /// <param name="inputStream">Stream to read</param>
        /// <param name="outputStream">Output stream</param>
        /// <exception cref="ArgumentNullException">Thrown when an argument is null</exception>
        /// <exception cref="IOException">Thrown when the in stream cannot be read or the outstream written</exception>
        public static void WriteTo( this Stream inputStream, Stream outputStream )
        {
            if ( inputStream == null )
                throw new ArgumentNullException( "inputStream" );

            if ( outputStream == null )
                throw new ArgumentNullException( "outputStream" );

            // 32K aught to be enough for anyone
            var buffer = new byte[ Int16.MaxValue ];

            int readBytes;

            while ( ( readBytes = inputStream.Read( buffer, 0, buffer.Length ) ) != 0 )
                outputStream.Write( buffer, 0, readBytes );

            outputStream.Flush( );
        }

        /// <summary>
        /// Copies a stream to another stream
        /// </summary>
        /// <param name="inputStream">Seekable stream to duplicate</param>
        /// <param name="outputStream">Output stream</param>
        /// <exception cref="ArgumentException">Thrown when an argument is not valid for copying</exception>
        /// <exception cref="ArgumentNullException">Thrown when an argument is null</exception>
        /// <exception cref="IOException">Thrown when the in stream cannot be read or the outstream written</exception>
        public static void CopyTo( this Stream inputStream, Stream outputStream )
        {
            if ( inputStream == null )
                throw new ArgumentNullException( "inputStream" );

            if ( outputStream == null )
                throw new ArgumentNullException( "outputStream" );

            if ( !inputStream.CanSeek )
                throw new ArgumentException( "instream must be seekable" );

            inputStream.Seek( 0, SeekOrigin.Begin );
            inputStream.WriteTo( outputStream );
            inputStream.Seek( 0, SeekOrigin.Begin );
        }

        public static GZipStream Compress( this Stream stream, bool leaveOpen = false )
        {
            return new GZipStream( stream, CompressionMode.Compress, leaveOpen );
        }

        public static GZipStream Decompress( this Stream stream, bool leaveOpen = false )
        {
            return new GZipStream( stream, CompressionMode.Decompress, leaveOpen );
        }

        public static string ToHexString( this byte[ ] buffer )
        {
            return String.Join( String.Empty, buffer.Select( b => b.ToString( "x2" ) ).ToArray( ) );
        }
    }
}
