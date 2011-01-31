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

        [BelongsTo( "ShipperID", Lazy = FetchWhen.OnInvoke )]
        public virtual Company Shipper { get; set; }

        [Property]
        public virtual bool IsActive { get; set; }

        [Property( "APICSShipmentBy", Length = 50 )]
        public virtual string ShipmentBy { get; set; }

        [Property( "APICSUPSServiceCode", Length = 50 )]
        public virtual string UPSServiceCode { get; set; }

        [Property( "APICSUPSPackagingType", Length = 50 )]
        public virtual string UPSPackagingType { get; set; }

        [Property( "APICSFedExServiceType", Length = 50 )]
        public virtual string FedExServiceType { get; set; }

        [Property( "APICSFedExPackagingType", Length = 50 )]
        public virtual string FedExPackagingType { get; set; }

        [Property( "APICSDiscount" )]
        public virtual decimal? Discount { get; set; }

        [Property( "APICSHandlingCharge" )]
        public virtual decimal? HandlingCharge { get; set; }

    }
}
