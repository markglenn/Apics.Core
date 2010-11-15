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
        /// <returns>A string containng the hmac</returns>
        public string GenerateLink( string url )
        {
            return EncodeHash( url, DateTime.Now.ToFileTimeUtc( ) );
        }
        
        /// <summary>
        /// Validates the URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>True if the URL is considered valid</returns>
        public bool ValidateUrl( string url )
        {
            if ( url == null )
                throw new ArgumentNullException( "url" );

            var match = UriRegex.Match( url );

            if ( !match.Success )
                return false;

            var hashcode = HashEncode( match.Groups[ 1 ].Value );
            var urlCode = match.Groups[ 2 ].Value;

            return String.Equals( hashcode, urlCode, StringComparison.OrdinalIgnoreCase );
        }

        #region [ Private Members ]
        
        private string EncodeHash( string url, long timestamp )
        {
            StringBuilder sb = new StringBuilder( url )
                .Append( url.Contains( "?" ) ? "&" : "?" )
                .AppendFormat( "timestamp={0}", timestamp );

            return sb.AppendFormat( "&hmac={0}", HashEncode( sb.ToString( ) ) ).ToString( );
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
