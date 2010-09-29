using System;
using System.Collections.Generic;
using System.Linq;
using Apics.Data.AptifyAdapter.Mapping.Metadata;
using Castle.ActiveRecord.Framework.Internal;

namespace Apics.Data.AptifyAdapter.Mapping.Visitors
{
    internal class HasManyVisitor : AbstractDepthFirstVisitor
    {
        private readonly IEnumerable<AptifyEntityMetadata> entities;
        private readonly TableMappings mappings;

        internal HasManyVisitor( IEnumerable<AptifyEntityMetadata> entities, TableMappings mappings )
        {
            this.mappings = mappings;
            this.entities = entities;
        }

        public override void VisitHasMany( HasManyModel model )
        {
            Type childType = model.Property.PropertyType.GetGenericArguments( ).SingleOrDefault( );
            string tableName = model.ContainingTypeModel.ActiveRecordAtt.Table;

            if( childType == null )
                throw new HasManyMappingException( "Property must have only one generic argument", model );

            AptifyEntityMetadata parentEntity =
                this.entities.FirstOrDefault( e => e.Tables.Any( t => t.Name == tableName ) );
            AptifyEntityMetadata childEntity = this.mappings.GetTableMetadata( childType ).Entity;

            // Can't map the table yet
            if( childEntity == null )
                return;

            if( childEntity.Parent == parentEntity )
                parentEntity.AddChild( childEntity, model.Property.Name );

            base.VisitHasMany( model );
        }
    }
}