using System;
using System.Linq;
using Apics.Data.AptifyAdapter.Mapping.Metadata;
using Castle.ActiveRecord.Framework.Internal;

namespace Apics.Data.AptifyAdapter.Mapping.Visitors
{
    internal class BelongsToVisitor : AbstractDepthFirstVisitor
    {
        private readonly TableMappings mappings;

        public BelongsToVisitor( TableMappings mappings )
        {
            this.mappings = mappings;
        }

        /// <summary>
        /// Maps the parent/child relationship
        /// </summary>
        /// <param name="model">Model that contains the relationship</param>
        public override void VisitBelongsTo( BelongsToModel model )
        {
            if( model.BelongsToAtt.Column == null )
                throw new ArgumentException( "BelongsTo missing column parameter: " + model.Property );

            Type child = model.Property.ReflectedType;

            // Get the table and column that is mapped
            AptifyTableMetadata table = this.mappings.GetTableMetadata( child );

            AptifyColumnMetadata column;
            if( !table.Columns.TryGetValue( model.BelongsToAtt.Column, out column ) )
            {
                throw new InvalidOperationException(
                    String.Format( "Could not find column {0} in {1}", model.BelongsToAtt.Column,
                        model.Property.DeclaringType.FullName ) );
            }

            // Remove the old name
            table.Columns.Remove( column.Name );

            // Reinsert with the updated property name
            table.Columns[ model.Property.Name ] = column;

            base.VisitBelongsTo( model );
        }
    }
}