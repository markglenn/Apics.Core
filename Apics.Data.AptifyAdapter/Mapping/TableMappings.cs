using System;
using System.Collections.Generic;
using System.Linq;
using Apics.Data.AptifyAdapter.Mapping.Metadata;
using NHibernate.Proxy;

namespace Apics.Data.AptifyAdapter.Mapping
{
    public class TableMappings
    {
        private readonly IDictionary<Type, AptifyTableMetadata> tables =
            new Dictionary<Type, AptifyTableMetadata>( );

        internal void Add( Type type, AptifyTableMetadata table )
        {
            this.tables.Add( type, table );
        }

        internal AptifyTableMetadata GetTableMetadata( Object entity )
        {
            Type type = NHibernateProxyHelper.GetClassWithoutInitializingProxy( entity );
            return this.tables[ type ];
        }

        internal AptifyTableMetadata GetTableMetadata( Type type )
        {
            return this.tables[ type ];
        }

        internal AptifyEntityMetadata GetEntityMetadata( Object entity )
        {
            Type type = NHibernateProxyHelper.GetClassWithoutInitializingProxy( entity );
            return this.tables[ type ].Entity;
        }
    }
}