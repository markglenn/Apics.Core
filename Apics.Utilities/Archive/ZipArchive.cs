using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ICSharpCode.SharpZipLib.Zip;

namespace Apics.Utilities.Archive
{
    public class ZipArchive : IArchive
    {
        private readonly ZipFile file;
        private readonly string path;
        private ZipOutputStream output;

        /// <summary>
        /// Opens a standard zip archive
        /// </summary>
        /// <param name="path">Path to zip file</param>
        /// <exception cref="ArgumentNullException">Thrown when the path is null</exception>
        public ZipArchive( string path )
        {
            if( path == null )
                throw new ArgumentNullException( "path" );

            this.path = path;
            this.file = new ZipFile( path );
        }

        /// <summary>
        /// Opens a zip archive stored inside a stream
        /// </summary>
        /// <param name="stream">Stream containing the zip file</param>
        public ZipArchive( Stream stream )
        {
            this.file = new ZipFile( stream );
        }

        /// <summary>
        /// Used internally for creating a writable stream
        /// </summary>
        private ZipArchive( )
        {
        }

        /// <summary>
        /// List of all the files within the zip file
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when a write only zip file is being used</exception>
        public IEnumerable<string> FileNames
        {
            get
            {
                if( !IsReadOnly )
                    throw new InvalidOperationException( "Cannot get a list of files from a write only zip file" );

                return this.file.Cast<ZipEntry>( ).ToList( ).Select( e => e.Name );
            }
        }

        #region IArchive Members

        /// <summary>
        /// Gets the name of the archive
        /// </summary>
        public string ArchiveName
        {
            get
            {
                if( !IsReadOnly )
                    throw new InvalidOperationException( "Cannot get the name of the archive from a write only zip file" );

                return this.path;
            }
        }

        /// <summary>
        /// Opens a file within the zip file
        /// </summary>
        /// <param name="fileName">File to open</param>
        /// <returns>Stream to that file</returns>
        /// <exception cref="NotSupportedException">Thrown when a write only zip file is being used</exception>
        /// <exception cref="InvalidOperationException">Thrown when the file does not exist in the zip file</exception>
        public Stream OpenFile( string fileName )
        {
            if( this.file == null )
                throw new InvalidOperationException( "Cannot open file within write only zip files" );

            int index = this.file.FindEntry( fileName.Replace( '\\', '/' ), true );

            // File was not found
            if( index == -1 )
                throw new FileNotFoundException( );

            return this.file.GetInputStream( index );
        }

        /// <summary>
        /// Checks if a file exists within the open zip file
        /// </summary>
        /// <param name="fileName">File to check</param>
        /// <returns>True if the file exists</returns>
        /// <exception cref="InvalidOperationException">Thrown when a write only zip file is being used</exception>
        public bool Exists( string fileName )
        {
            if( this.file == null )
                throw new InvalidOperationException( "Cannot open file within write only zip files" );

            return ( this.file.FindEntry( fileName.Replace( '\\', '/' ), true ) != -1 );
        }

        /// <summary>
        /// States whether the zip file is in write only mode
        /// </summary>
        public bool IsReadOnly
        {
            get { return this.output == null; }
        }

        /// <summary>
        /// Creates a writable stream within the zip file
        /// </summary>
        /// <param name="fileName">File to write within the zip file</param>
        /// <returns>A writable stream to write within the zip file</returns>
        /// <exception cref="InvalidOperationException">Thrown when a read only zip file is being used</exception>
        public Stream CreateWritableStream( string fileName )
        {
            if( IsReadOnly )
                throw new InvalidOperationException( "Can't write to read only Zip file" );

            return new ZipFileStream( this.output, fileName );
        }

        /// <summary>
        /// Not supported
        /// </summary>
        /// <param name="fileName">File to delete</param>
        /// <returns>Never returns</returns>
        /// <exception cref="NotSupportedException">Always thrown</exception>
        public bool DeleteFile( string fileName )
        {
            throw new NotSupportedException( );
        }

        /// <summary>
        /// Disposes the archive
        /// </summary>
        public void Dispose( )
        {
            Dispose( true );
            GC.SuppressFinalize( this );
        }

        /// <summary>
        /// Gets an enumerator of all the files
        /// </summary>
        /// <returns>Enumerator for all the file names</returns>
        /// <exception cref="InvalidOperationException">Thrown when a write only zip file is being used</exception>
        public IEnumerator<string> GetEnumerator( )
        {
            if( !IsReadOnly )
                throw new InvalidOperationException( "Cannot get a list of files from a write only zip file" );

            return this.file.Cast<ZipEntry>( ).Select( e => e.Name ).GetEnumerator( );
        }

        /// <summary>
        /// Gets an enumerator of all the files
        /// </summary>
        /// <returns>Enumerator for all the file names</returns>
        /// <exception cref="InvalidOperationException">Thrown when a write only zip file is being used</exception>
        IEnumerator IEnumerable.GetEnumerator( )
        {
            return GetEnumerator( );
        }

        #endregion

        /// <summary>
        /// Creates a writable version of the zip archive
        /// </summary>
        /// <param name="baseStream">Stream where the zip file will be written</param>
        /// <returns>A new zip archive</returns>
        /// <exception cref="ArgumentNullException">Thrown when the base stream is null</exception>
        public static ZipArchive CreateWritable( Stream baseStream )
        {
            if( baseStream == null )
                throw new ArgumentNullException( "baseStream" );

            var archive = new ZipArchive
            {
                output = new ZipOutputStream( baseStream )
            };

            return archive;
        }

        /// <summary>
        /// Deconstructor
        /// </summary>
        ~ZipArchive( )
        {
            Dispose( false );
        }

        /// <summary>
        /// Disposes the archive
        /// </summary>
        /// <param name="disposing">True if Dispose was called, false if the destructor calls</param>
        protected virtual void Dispose( bool disposing )
        {
            if( !disposing )
                return;

            if( IsReadOnly )
            {
                this.file.Close( );
            }
            else
            {
                this.output.Finish( );
                this.output.Close( );
                this.output.Dispose( );
            }
        }
    }
}