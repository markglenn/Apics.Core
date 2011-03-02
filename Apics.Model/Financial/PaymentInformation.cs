using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;
using Apics.Model.Fulfillment;

namespace Apics.Model.Financial
{
    [ActiveRecord( Lazy = true )]
    public class PaymentInformation
    {
        [PrimaryKey]
        public virtual int Id { get; set; }

        [BelongsTo( "PaymentTypeID", NotNull = true, Lazy = FetchWhen.OnInvoke, Cascade = CascadeEnum.None )]
        public virtual PaymentType PaymentType { get; set; }

        [Property( Length = 4000 )]
        public virtual string CCAccountNumber { get; set; }

        [Property]
        public virtual DateTime? CCExpireDate { get; set; }

        [Property( Length = 100 )]
        public virtual string CCAuthCode { get; set; }

        [Property( Length = 50 )]
        public virtual string CCAuthType { get; set; }

        [Property( Length = 20 )]
        public virtual string CCPartial { get; set; }

        [Property( Length = 20 )]
        public virtual string CheckNumber { get; set; }

        [Property( Length = 50 )]
        public virtual string AccountName { get; set; }

        [Property( Length = 50 )]
        public virtual string AccountNumber { get; set; }

        [Property( Length = 35 )]
        public virtual string Bank { get; set; }

        [Property( Length = 50 )]
        public virtual string BranchName { get; set; }

        [Property( Length = 30 )]
        public virtual string ABA { get; set; }

        [BelongsTo( "CreditOrderID", Lazy = FetchWhen.OnInvoke )]
        public virtual Order CreditOrder { get; set; }

        [Property( Length = 25 )]
        public virtual string PONumber { get; set; }

        [Property]
        public virtual DateTime? DueDate { get; set; }

        [Property( Length = 50 )]
        public virtual string Terms { get; set; }

    }
}
