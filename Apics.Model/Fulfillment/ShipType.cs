using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;
using Apics.Model.User;
using System.Diagnostics;

namespace Apics.Model.Fulfillment
{
    [ActiveRecord( Lazy = true )]
    [DebuggerDisplay( "ShipType: {Id}" )]
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

        [BelongsTo( "ShipperID", Lazy = FetchWhen.OnInvoke )]
        public virtual Company Shipper { get; set; }
    }
}
