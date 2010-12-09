using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;
using System.Diagnostics;
using Apics.Model.User;
using Apics.Model.Location;

namespace Apics.Model.Fulfillment
{
    [ActiveRecord( "OrderShipment", Lazy = true )]
    [DebuggerDisplay( "Shipment: {Id}" )]
    public class Shipment
    {
        [PrimaryKey]
        public virtual int Id { get; set; }

        [BelongsTo( "OrderID", NotNull = true, Lazy = FetchWhen.OnInvoke )]
        public virtual Order Order { get; set; }

        [Property]
        public virtual int Sequence { get; set; }

        //[BelongsTo( "ShipTypeID", NotNull = true, Lazy = FetchWhen.OnInvoke )]
        //public virtual ShipmentType ShipType { get; set; }

        [BelongsTo( "ShipToID", NotNull = true, Lazy = FetchWhen.OnInvoke )]
        public virtual Person ShipToPerson { get; set; }

        [BelongsTo( "ShipToAddressID", NotNull = true, Lazy = FetchWhen.OnInvoke )]
        public virtual Address ShipToAddress { get; set; }

        [Property( "ShipTrackingNum", Length = 100 )]
        public virtual string TrackingNumber { get; set; }

        [Property( "OverrideShippingCalc" )]
        public virtual bool OverrideShippingCalculation { get; set; }

        [Property( "OverrideHandlingCalc" )]
        public virtual bool OverrideHandlingCalculation { get; set; }

        [Property]
        public virtual decimal ShippingCharge { get; set; }

        [Property]
        public virtual decimal ShippingCost { get; set; }

        [Property]
        public virtual decimal HandlingCharge { get; set; }

        //[BelongsTo( "TaxJurisdictionID", Lazy = FetchWhen.OnInvoke )]
        //public virtual TaxJurisdiction TaxJurisdiction { get; set; }

        [Property]
        public virtual bool TaxIncludedInCharges { get; set; }

        //[BelongsTo( "ShipmentTypeGroupingID", Lazy = FetchWhen.OnInvoke )]
        //public virtual ShipmentTypeGrouping ShipmentTypeGrouping { get; set; }

        [Property]
        public virtual decimal TotalQuantity { get; set; }

        [Property]
        public virtual decimal TotalWeight { get; set; }

        [Property( NotNull = true, Length = 10 )]
        public virtual string WeightUnits { get; set; }

        //[BelongsTo( "ShipmentTypeMatrixID", Lazy = FetchWhen.OnInvoke )]
        //public virtual ShipmentTypeMatrix ShipmentTypeMatrix { get; set; }

    }


}
