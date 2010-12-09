using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using Apics.Utilities.Extension;
using System.Text.RegularExpressions;

namespace Apics.Utilities.Security
{
    public class HmacUrlAuthorization : IUrlAuthorization
    {
        #region [ Private Members ]

        private readonly HMAC hmac;
        private static Regex UriRegex = new Regex( @"(^.+)[\&\?]hmac=([0-9a-fA-F]+)$" );

        #endregion [ Private Members ]

        /// <summary>
        /// Initializes a new instance of the <see cref="HmacUrlAuthorization"/> class.
        /// </summary>
        /// <param name="key">The key to use for encoding</param>
        public HmacUrlAuthorization( string key )
        {
            if ( String.IsNullOrEmpty( key ) )
                throw new ArgumentException( "HMAC key cannot be null or empty", "key" );

            this.hmac = new HMACSHA1( Encoding.UTF8.GetBytes( key ) );
        }

        /// <summary>
        /// Generates the link.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>A string containing the HMAC</returns>
        public Uri GenerateLink( Uri uri )
        {
            return EncodeHash( uri, DateTime.Now.ToFileTimeUtc( ) );
        }
        
        /// <summary>
        /// Validates the URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>True if the URL is considered valid</returns>
        public bool ValidateUrl( Uri uri )
        {
            if ( uri == null )
                throw new ArgumentNullException( "uri" );

            var builder = new UriBuilder( uri );

            var hashcode = builder.GetQueryParam( "hmac" );

            if ( hashcode == null )
                return false;

            builder.RemoveQueryParam( "hmac" );

            return String.Equals( hashcode, HashEncode( builder.Uri ), StringComparison.OrdinalIgnoreCase );
        }

        #region [ Private Members ]
        
        private Uri EncodeHash( Uri uri, long timestamp )
        {
            UriBuilder builder = new UriBuilder( uri );

            builder.SetQueryParam( "timestamp", timestamp.ToString( ) );
            builder.SetQueryParam( "hmac", HashEncode( builder.Uri.ToString( ) ) );

            return builder.Uri;
        }

        private string HashEncode( Uri uri )
        {
            return HashEncode( uri.ToString( ) );
        }

        private string HashEncode( string text )
        {
            var hash = this.hmac.ComputeHash( 
                Encoding.UTF8.GetBytes( text.ToLowerInvariant( ) ) );

            return hash.ToHexString( );
        }

        #endregion [ Private Members ]

    }
}
