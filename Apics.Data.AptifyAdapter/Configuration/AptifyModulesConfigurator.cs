using System;
using System.Linq;
using System.Xml.Serialization;
using Apics.Utilities;

namespace Apics.Data.AptifyAdapter.Configuration
{
    public class AptifyModulesConfigurator : XmlConfigurator<AptifyModuleSettingConfiguration>
    {
    }

    [Serializable, XmlRoot( "aptify.modules" )]
    public class AptifyModuleSettingConfiguration
    {
        [XmlArray( "modules" ), XmlArrayItem( "module" )]
        public AptifyModule[ ] Modules { get; set; }
    }

    public class AptifyModule
    {
        [XmlAttribute( "type" )]
        public string Type { get; set; }
    }
}