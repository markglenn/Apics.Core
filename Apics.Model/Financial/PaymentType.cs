using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;
using System.ComponentModel;

namespace Apics.Model.Financial
{
    public enum PaymentMethodType
    {
        [Description( "Check" )]
        Check,

        [Description( "Credit Card" )]
        CreditCard,

        [Description( "Credit Memo" )]
        CreditMemo,

        [Description( "Purchase Order" )]
        PurchaseOrder,

        [Description( "Wire Transfer" )]
        WireTransfer
    }

    [ActiveRecord( Lazy = true )]
    public class PaymentType
    {
        [PrimaryKey]
        public virtual int Id { get; set; }

        [Property]
        public virtual string Name { get; set; }

        /// <summary>
        /// Payment type (credit card, 
        /// </summary>
        [Property( "Type", NotNull = true, 
            ColumnType = @"Apics.Model.DescribedEnumStringType`1[Apics.Model.Financial.PaymentMethodType], Apics.Model" )]
        public virtual PaymentMethodType Type { get; set; }

        [Property( "Active", NotNull = true )]
        public virtual bool IsActive { get; set; }
    }
}
