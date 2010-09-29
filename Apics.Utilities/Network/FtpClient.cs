using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Apics.Utilities.Extension;
using log4net;

namespace Apics.Utilities.Network
{
    public class FtpClient
    {
        #region [ Private Members ]

        private static readonly ILog Log = LogManager.GetLogger( typeof( FtpClient ) );

        private readonly string username;
        private readonly string password;
        private readonly Uri uri;

		#endregion [ Private Members ]

		#region [ Constructors ]

		public FtpClient ( Uri uri, string userName, string password )
		{
		    this.uri = uri;
		    this.username = userName;
		    this.password = password;
		}

		#endregion [ Constructors ]

        #region [ Public Methods ]

        /// <summary>
        /// Uploads a stream to the desired path
        /// </summary>
        /// <param name="path"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
		public bool Upload ( string path, Stream stream )
		{
            if ( stream.CanSeek )
			    stream.Position = 0;

			var request = CreateRequest( path, WebRequestMethods.Ftp.UploadFile );
			request.ContentLength = stream.Length;

			try
			{
                using ( Stream output = request.GetRequestStream( ) )
                    stream.WriteTo( output );

			    return true;
			}
			catch ( Exception ex )
			{
                Log.ErrorFormat( "Could not upload {0}: {1}", path, ex );
		        return false;
			}
		}

        /// <summary>
        /// Downloads a file to a stream
        /// </summary>
        /// <param name="fileName">Path to the file</param>
        /// <returns>Stream of the file</returns>
        public Stream Download ( string fileName )
		{
		    var request = CreateRequest( fileName, WebRequestMethods.Ftp.DownloadFile );
		    try
		    {
		        var response = request.GetResponse( );

                if ( response == null )
                    throw new WebException( "Null response" );

		        return response.GetResponseStream( );
		    }
		    catch( WebException ex )
		    {
		        Log.ErrorFormat( "Could not download {0}: {1}",
		            fileName, ex );

		        return null;
		    }
		}
        
        /// <summary>
        /// Gets a file listing of the ftp site
        /// </summary>
        /// <param name="path">Path to get listing of</param>
        /// <returns>Enumerable of files and directories in the path</returns>
		public IEnumerable<FtpFileInformation> GetDetailedFileList ( string path )
		{
			var request = CreateRequest ( path, WebRequestMethods.Ftp.ListDirectoryDetails );

			var information = new List<FtpFileInformation> ( );

			try
			{
                using ( WebResponse response = request.GetResponse( ) )
                {
                    if( response == null )
                        throw new WebException( "Could not get path listing" );

                    using( var stream = response.GetResponseStream( ) )
                    {

                        if( stream == null )
                            throw new WebException( "Could not get path listing" );

                        var reader = new StreamReader( stream );

                        for ( var line = reader.ReadLine( ); line != null; 
                            line = reader.ReadLine( ) )
                        {
                            try
                            {
                                information.Add( new FtpFileInformation( line, path ) );
                            }
                            catch( ApplicationException )
                            {
                                Log.WarnFormat( "File format exception: {0}", line );
                            }
                        }

                    }
                }
			    return information;
			}
			catch ( WebException ex )
			{
			    Log.ErrorFormat( "Unknown error when receiving path listing: {0}",
			        ex );
				return null;
			}
		}

        /// <summary>
        /// Deletes a file from the FTP server
        /// </summary>
        /// <param name="fileName">Path to the file that needs deleting</param>
        /// <returns>True if successful</returns>
		public bool Delete ( string fileName )
		{
			FtpWebRequest request = CreateRequest ( fileName, WebRequestMethods.Ftp.DeleteFile );

			try
			{
			    return request.GetResponse( ) != null;
			}
			catch ( WebException ex )
			{
				Log.ErrorFormat( "Could not delete {0}: {1}", fileName, ex );
				return false;
			}
		}

        #endregion [ Public Methods ]

		#region [ Private Methods ]

		private FtpWebRequest CreateRequest ( string fileName, string method )
		{
		    var requestedUri = new Uri( this.uri, fileName );

            if ( !requestedUri.IsWellFormedOriginalString(  ) )
                throw new InvalidOperationException( "Invalid URI: " + requestedUri );

            var request = ( FtpWebRequest )WebRequest.Create( requestedUri );

			request.Credentials = new NetworkCredential ( username, password );
			request.KeepAlive = false;
			request.UseBinary = true;
			request.UsePassive = true;
			request.Method = method;

			return request;
		}

        #endregion [ Private Methods ]

    }
}
