using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;

namespace Apics.Model.Financial
{
    [ActiveRecord( Lazy = true )]
    public class PaymentInformation
    {
        [PrimaryKey]
        public virtual int Id { get; set; }

        [BelongsTo( "PaymentTypeID" )]
        public virtual PaymentType PaymentType { get; set; }
        
    }
}
