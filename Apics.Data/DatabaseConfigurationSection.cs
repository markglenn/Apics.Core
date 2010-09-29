using System;
using System.Linq;
using System.Configuration;

namespace Apics.Data
{
    public class DatabaseConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty( "adapter" )]
        public string AdapterModule
        {
            get { return ( string )this[ "adapter" ]; }
            set { this[ "adapter" ] = value; }
        }

        [ConfigurationProperty( "connection" )]
        public string ConnectionString
        {
            get { return ( string )this[ "connection" ]; }
            set { this[ "connection" ] = value; }
        }

        [ConfigurationProperty( "modelAssembly" )]
        public string ModelAssembly
        {
            get { return ( string )this[ "modelAssembly" ]; }
            set { this[ "modelAssembly" ] = value; }
        }
    }
}
