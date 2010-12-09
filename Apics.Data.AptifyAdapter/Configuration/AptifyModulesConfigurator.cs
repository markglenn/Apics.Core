using System;
using System.Linq;
using System.Xml.Serialization;
using System.Configuration;
using System.Xml;

namespace Apics.Data.AptifyAdapter.Configuration
{
    public class AptifyModulesConfigurator :  IConfigurationSectionHandler
    {
        #region IConfigurationSectionHandler Members

        public object Create( object parent, object configContext, XmlNode section )
        {
            if ( section == null )
                return null;

            var serializer = new XmlSerializer( typeof( AptifyModuleSettingConfiguration ) );

            using ( var reader = new XmlNodeReader( section ) )
                return serializer.Deserialize( reader );
        }

        #endregion
    }

    [Serializable, XmlRoot( "aptify.modules" )]
    public class AptifyModuleSettingConfiguration
    {
        [XmlArray( "modules" ), XmlArrayItem( "module", typeof( AptifyModule ) )]
        public AptifyModule[ ] Modules { get; set; }
    }

    [Serializable]
    public class AptifyModule
    {
        [XmlAttribute( "type" )]
        public string Type { get; set; }
    }
}