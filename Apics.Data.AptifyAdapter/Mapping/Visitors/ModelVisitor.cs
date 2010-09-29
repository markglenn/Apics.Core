using System;
using System.Collections.Generic;
using System.Linq;
using Apics.Data.AptifyAdapter.Mapping.Metadata;
using Castle.ActiveRecord.Framework.Internal;

namespace Apics.Data.AptifyAdapter.Mapping.Visitors
{
    internal class ModelVisitor : AbstractDepthFirstVisitor
    {
        private readonly IEnumerable<AptifyEntityMetadata> entities;
        private readonly TableMappings mappings;

        internal ModelVisitor( IEnumerable<AptifyEntityMetadata> entities, TableMappings mappings )
        {
            if( entities == null )
                throw new ArgumentNullException( "entities" );

            if( mappings == null )
                throw new ArgumentNullException( "mappings" );

            this.entities = entities;
            this.mappings = mappings;
        }

        /// <summary>
        /// Maps the ActiveRecord class to the table information from Aptify
        /// </summary>
        /// <param name="model">ActiveRecord class model</param>
        public override void VisitModel( ActiveRecordModel model )
        {
            if( model.ActiveRecordAtt == null )
                return;

            // Find the Aptify entity that maps to this ActiveRecord model
            AptifyEntityMetadata entity = this.entities.FirstOrDefault(
                e => e.Tables.Any( t => t.Name == model.ActiveRecordAtt.Table ) );

            if( entity == null )
            {
                // The mapping of the table name is incorrect
                throw new InvalidOperationException(
                    string.Format( "'{0}' is mapped to table '{1}', which is not in an entity in Aptify",
                        model.Type.FullName, model.ActiveRecordAtt.Table ) );
            }

            // Find the table within the entity
            AptifyTableMetadata table = entity.Tables.First( t => t.Name == model.ActiveRecordAtt.Table );

            // Insert this mapping into the mappings
            this.mappings.Add( model.Type, table );

            base.VisitModel( model );
        }
    }
}