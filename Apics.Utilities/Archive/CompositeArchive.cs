using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Apics.Utilities.Archive
{
    public class CompositeArchive : IArchive, ICollection<IArchive>
    {
        private readonly IList<IArchive> archives = new List<IArchive>( );

        public IEnumerable<string> FileNames
        {
            get
            {
                var fileNames = new List<string>( );

                foreach( IArchive archive in this.archives )
                    fileNames.AddRange( archive );

                return fileNames;
            }
        }

        #region IArchive Members

        public string ArchiveName
        {
            get { return "CompositeArchive"; }
        }

        public Stream OpenFile( string fileName )
        {
            return (
                from archive in this.archives
                where archive.Exists( fileName )
                select archive.OpenFile( fileName )
                ).FirstOrDefault( );
        }

        public bool Exists( string fileName )
        {
            return this.archives.Any( archive => archive.Exists( fileName ) );
        }

        public Stream CreateWritableStream( string fileName )
        {
            return ( from archive in this.archives
                where !archive.IsReadOnly
                select archive.CreateWritableStream( fileName ) ).FirstOrDefault( );
        }

        public bool DeleteFile( string fileName )
        {
            throw new NotSupportedException( );
        }

        bool IArchive.IsReadOnly
        {
            get { return false; }
        }

        IEnumerator IEnumerable.GetEnumerator( )
        {
            return this.archives.GetEnumerator( );
        }

        public void Dispose( )
        {
            Dispose( true );
            GC.SuppressFinalize( this );
        }

        IEnumerator<string> IEnumerable<string>.GetEnumerator( )
        {
            var fileNames = new List<string>( );

            foreach( IArchive archive in this.archives )
                fileNames.AddRange( archive );

            return fileNames.GetEnumerator( );
        }

        #endregion

        #region ICollection<IArchive> Members

        public bool IsReadOnly
        {
            get { return !this.archives.Any( archive => !archive.IsReadOnly ); }
        }

        public void Add( IArchive item )
        {
            this.archives.Add( item );
        }

        public void Clear( )
        {
            this.archives.Clear( );
        }

        public bool Contains( IArchive item )
        {
            return this.archives.Contains( item );
        }

        public void CopyTo( IArchive[ ] array, int arrayIndex )
        {
            this.archives.CopyTo( array, arrayIndex );
        }

        public int Count
        {
            get { return this.archives.Count; }
        }

        public bool Remove( IArchive item )
        {
            return this.archives.Remove( item );
        }

        public IEnumerator<IArchive> GetEnumerator( )
        {
            return this.archives.GetEnumerator( );
        }

        #endregion

        /// <summary>
        /// Destructor
        /// </summary>
        ~CompositeArchive( )
        {
            Dispose( false );
        }

        /// <summary>
        /// Returns a list of archives that have this fileName
        /// </summary>
        /// <param name="fileName">Filename to search for</param>
        /// <returns>Enumerable of archives that contain the file</returns>
        public IEnumerable<IArchive> GetArchivesWithFile( string fileName )
        {
            return this.archives.Where( a => a.Exists( fileName ) ).AsEnumerable( );
        }

        protected virtual void Dispose( bool disposing )
        {
            if( !disposing )
                return;

            foreach( IArchive archive in this.archives )
                archive.Dispose( );
        }
    }
}