using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;
using Apics.Model.Location;
using System.Diagnostics;

namespace Apics.Model.Financial
{
    [ActiveRecord( Lazy = true )]
    [DebuggerDisplay( "CurrencyType: {Id}" )]
    public class CurrencyType
    {
        [PrimaryKey]
        public virtual int Id { get; set; }

        [Property( NotNull = true, Length = 50 )]
        public virtual string Name { get; set; }

        [BelongsTo( "CountryID", NotNull = true, Lazy = FetchWhen.OnInvoke )]
        public virtual Country Country { get; set; }

        [Property( NotNull = true, Length = 5 )]
        public virtual string CurrencySymbol { get; set; }

        [Property( NotNull = true, Length = 5 )]
        public virtual string TradingSymbol { get; set; }

        [Property]
        public virtual int NumDigitsAfterDecimal { get; set; }

        [Property( Length = 50 )]
        public virtual string FormatString { get; set; }

        [Property( Length = 100 )]
        public virtual string GeneralLedgerSegment { get; set; }

        [Property( Length = 10 )]
        public virtual string GeneralLedgerDelimiter { get; set; }

    }

}
