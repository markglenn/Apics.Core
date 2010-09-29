using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;

namespace Apics.Model.Fulfillment
{
    [ActiveRecord]
    public class ShipType
    {
        [PrimaryKey]
        public virtual int Id { get; set; }

        [Property]
        public virtual string Name { get; set; }

        [Property( "APICSShipmentCode" )]
        public virtual string ApicsShipmentCode { get; set; }

        [Property]
        public virtual bool IsActive { get; set; }
    }
}
