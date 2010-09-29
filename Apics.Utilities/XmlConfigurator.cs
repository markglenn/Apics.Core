using System;
using System.Linq;
using System.Configuration;
using System.Xml;
using System.Xml.Serialization;

namespace Apics.Utilities
{
    public class XmlConfigurator<T> : IConfigurationSectionHandler
    {
        #region IConfigurationSectionHandler Members

        public object Create( object parent, object configContext, XmlNode section )
        {
            if( section == null )
                return null;

            var serializer = new XmlSerializer( typeof( T ) );

            using( var reader = new XmlNodeReader( section ) )
                return serializer.Deserialize( reader );
        }

        #endregion
    }
}
