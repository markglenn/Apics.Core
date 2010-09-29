using System;
using System.Configuration;
using System.Linq;

namespace Apics.Utilities.Network
{
    public class FtpSettingsSection : ConfigurationSection
    {
        [ConfigurationProperty( "uri", IsRequired = true )]
        public string Uri
        {
            get
            {
                var uri = ( string )this[ "uri" ];

                if ( uri.EndsWith( "/", StringComparison.Ordinal ) )
                    return uri;

                return uri + "/";
            }
            set { this[ "uri" ] = value; }
        }

        [ConfigurationProperty( "username", IsRequired = true )]
        public string UserName
        {
            get { return ( string )this[ "username" ]; }
            set { this[ "username" ] = value; }
        }

        [ConfigurationProperty( "password", IsRequired = true )]
        public string Password
        {
            get { return ( string )this[ "password" ]; }
            set { this[ "password" ] = value; }
        }
    }
}
