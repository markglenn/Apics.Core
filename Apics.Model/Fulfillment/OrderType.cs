using System;
using System.Linq;
using Castle.ActiveRecord;

namespace Apics.Model.Fulfillment
{
    [ActiveRecord( Lazy = true )]
    public class OrderType
    {
        [PrimaryKey]
        public virtual int Id { get; set; }

        [Property]
        public virtual string Name { get; set; }
    }
}