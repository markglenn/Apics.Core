using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;

namespace Apics.Model.Financial
{
    [ActiveRecord( Lazy = true )]
    public class PaymentType
    {
        [PrimaryKey]
        public virtual int Id { get; set; }

        [Property]
        public virtual string Name { get; set; }

    }
}
