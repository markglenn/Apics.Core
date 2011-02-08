using System;
using System.Data;
using System.Linq;
using Apics.Data.AptifyAdapter.Mapping.Metadata;
using Castle.ActiveRecord.Framework.Internal;

namespace Apics.Data.AptifyAdapter.Mapping.Visitors
{
    internal class PropertyVisitor : AbstractDepthFirstVisitor
    {
        private readonly TableMappings mappings;

        public PropertyVisitor( TableMappings mappings )
        {
            this.mappings = mappings;
        }

        /// <summary>
        /// Updates validations for the property
        /// </summary>
        /// <param name="model">Property model</param>
        public override void VisitProperty( PropertyModel model )
        {
            AptifyTableMetadata table = this.mappings.GetTableMetadata( model.Property.ReflectedType );
            string columnName = model.PropertyAtt.Column ?? model.Property.Name;

            AptifyColumnMetadata column;
            if ( table.Columns.TryGetValue( columnName, out column ) )
            {
                if ( model.PropertyAtt.Column != null )
                {
                    table.Columns.Remove( column.Name );
                    table.Columns[ model.Property.Name ] = column;
                }

                model.PropertyAtt.NotNull = !column.Nullable;
            }
            else if ( String.IsNullOrEmpty( model.PropertyAtt.Table ) )
            {
                // If the table property is set, this is a joined table column
                
                string error = String.Format( "Could not map {0} to entity {1}.  Column not defined in Aptify entity.",
                    columnName, model.Property.ReflectedType );
                throw new InvalidConstraintException( error );
            }

            base.VisitProperty( model );
        }
    }
}