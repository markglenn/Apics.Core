using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;
using System.Diagnostics;
using Apics.Model.User;
using Apics.Model.Financial;

namespace Apics.Model.Fulfillment
{
    [ActiveRecord( Lazy = true )]
    [DebuggerDisplay( "ProductPrice: {Id}" )]
    public class ProductPrice
    {
        [PrimaryKey]
        public virtual int Id { get; set; }

        [BelongsTo( "ProductID", NotNull = true, Lazy = FetchWhen.OnInvoke )]
        public virtual Product Product { get; set; }

        [Property( NotNull = true, Length = 255 )]
        public virtual string Name { get; set; }

        [Property]
        public virtual decimal Price { get; set; }

        [Property]
        public virtual bool PriceIncludesTax { get; set; }

        [Property]
        public virtual bool IsDefault { get; set; }

        [Property]
        public virtual string Description { get; set; }

        [BelongsTo( "MemberTypeID", NotNull = true, Lazy = FetchWhen.OnInvoke )]
        public virtual MemberType MemberType { get; set; }

        [Property]
        public virtual bool IncludeSubMemberTypes { get; set; }

        [Property]
        public virtual DateTime StartDate { get; set; }

        [Property]
        public virtual DateTime EndDate { get; set; }

        [Property( NotNull = true, Length = 15 )]
        public virtual string Type { get; set; }

        [Property]
        public virtual decimal PercentOfBase { get; set; }

        [Property]
        public virtual decimal MinQuantity { get; set; }

        [Property]
        public virtual decimal MaxQuantity { get; set; }

        [BelongsTo( "CurrencyTypeID", NotNull = true, Lazy = FetchWhen.OnInvoke )]
        public virtual CurrencyType CurrencyType { get; set; }

        [BelongsTo( "DerivedFromCurrencyTypeID", Lazy = FetchWhen.OnInvoke )]
        public virtual CurrencyType DerivedFromCurrencyType { get; set; }

        [Property( NotNull = true, Length = 10 )]
        public virtual string RoundingType { get; set; }

        [Property]
        public virtual decimal? RoundingAmount { get; set; }

        [Property]
        public virtual string Comments { get; set; }

    }

}
