using System;
using System.Linq;
using System.Xml.Serialization;

namespace Apics.Utilities.Module
{
    public class ModuleConfigurator : XmlConfigurator<ModuleSettingConfiguration>
    {
    }

    [Serializable, XmlRoot( "apics.dependency" )]
    public class ModuleSettingConfiguration
    {
        [XmlArray( "modules" ), XmlArrayItem( "module" )]
        public ModuleSetting[ ] Modules { get; set; }
    }

    public class ModuleSetting
    {
        [XmlAttribute( "type" )]
        public string Type { get; set; }
    }
}