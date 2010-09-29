using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Apics.Utilities.Archive
{
    public class FolderArchive : IArchive
    {
        private readonly string path;

        /// <summary>
        /// Creates the folder archive loader from the path
        /// </summary>
        /// <param name="path">Path to the folder</param>
        public FolderArchive( string path )
        {
            this.path = path;
        }

        public IEnumerable<string> FileNames
        {
            get { return Directory.GetFiles( this.path ); }
        }

        #region IArchive Members

        public string ArchiveName
        {
            get { return this.path; }
        }

        public Stream OpenFile( string fileName )
        {
            return new FileStream( Path.Combine( this.path, fileName ), FileMode.Open, FileAccess.Read );
        }

        public bool Exists( string fileName )
        {
            return File.Exists( Path.Combine( this.path, fileName ) );
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public Stream CreateWritableStream( string fileName )
        {
            return new FileStream( Path.Combine( this.path, fileName ), FileMode.CreateNew, FileAccess.Write );
        }

        public bool DeleteFile( string fileName )
        {
            try
            {
                Directory.Delete( fileName );
            }
            catch( Exception )
            {
                return false;
            }

            return true;
        }

        public void Dispose( )
        {
            Dispose( true );
            GC.SuppressFinalize( this );
        }

        public IEnumerator<string> GetEnumerator( )
        {
            return Directory.GetFiles( this.path ).ToList( ).GetEnumerator( );
        }

        IEnumerator IEnumerable.GetEnumerator( )
        {
            return GetEnumerator( );
        }

        #endregion

        ~FolderArchive( )
        {
            Dispose( false );
        }

        protected virtual void Dispose( bool disposing )
        {
        }
    }
}