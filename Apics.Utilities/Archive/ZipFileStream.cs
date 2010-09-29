using System;
using System.IO;
using System.Linq;
using ICSharpCode.SharpZipLib.Zip;

namespace Apics.Utilities.Archive
{
    internal class ZipFileStream : Stream
    {
        private readonly string fileName;
        private readonly ZipOutputStream zipOutputStream;
        private MemoryStream baseStream = new MemoryStream( );

        internal ZipFileStream( ZipOutputStream zipOutputStream, string fileName )
        {
            // Check thar our parameters are properly set
            if( zipOutputStream == null )
                throw new ArgumentNullException( "zipOutputStream" );

            if( fileName == null )
                throw new ArgumentNullException( "fileName" );

            this.zipOutputStream = zipOutputStream;
            this.fileName = fileName;
        }

        #region [ Stream Overloads ]

        public override bool CanRead
        {
            get { return this.baseStream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return this.baseStream.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return this.baseStream.CanWrite; }
        }

        public override long Length
        {
            get { return this.baseStream.Length; }
        }

        public override long Position
        {
            get { return this.baseStream.Position; }
            set { this.baseStream.Position = value; }
        }

        public override void Close( )
        {
            if( this.baseStream != null )
            {
                var entry = new ZipEntry( this.fileName )
                {
                    Size = this.baseStream.Length,
                    DateTime = DateTime.Now
                };

                this.zipOutputStream.PutNextEntry( entry );

                this.zipOutputStream.Write( this.baseStream.ToArray( ), 0, Convert.ToInt32( this.baseStream.Length ) );
                this.zipOutputStream.Flush( );

                this.baseStream.Close( );
                this.baseStream = null;
            }

            base.Close( );
        }

        public override void Flush( )
        {
            this.baseStream.Flush( );
        }

        public override int Read( byte[ ] buffer, int offset, int count )
        {
            return this.baseStream.Read( buffer, offset, count );
        }

        public override long Seek( long offset, SeekOrigin origin )
        {
            return this.baseStream.Seek( offset, origin );
        }

        public override void SetLength( long value )
        {
            this.baseStream.SetLength( value );
        }

        public override void Write( byte[ ] buffer, int offset, int count )
        {
            this.baseStream.Write( buffer, offset, count );
        }

        protected override void Dispose( bool disposing )
        {
            base.Dispose( disposing );

            if( disposing && this.baseStream != null )
                this.baseStream.Dispose( );
        }

        #endregion [ Stream Overloads ]
    }
}