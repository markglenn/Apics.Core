using System;
using System.Collections.Generic;
using System.Linq;
using Apics.Data.AptifyAdapter.Mapping.Metadata;
using Castle.ActiveRecord.Framework.Internal;

namespace Apics.Data.AptifyAdapter.Mapping.Visitors
{
    /// <summary>
    /// Model visitor to go through the entire model listing and map them
    /// to Aptify.
    /// </summary>
    public class AptifyModelMapper
    {
        internal TableMappings MapTables( IEnumerable<AptifyEntityMetadata> entities, ActiveRecordModelCollection models )
        {
            var mappings = new TableMappings( );

            var visitors = new List<IVisitor>
            {
                new ModelVisitor( entities, mappings ),
                new HasManyVisitor( entities, mappings ),
                new BelongsToVisitor( mappings ),
                new PropertyVisitor( mappings ),
            };

            foreach( IVisitor visitor in visitors )
                foreach( ActiveRecordModel model in models )
                    visitor.VisitModel( model );

            return mappings;
        }
    }
}