using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;
using System.Diagnostics;
using Apics.Model.Fulfillment;

namespace Apics.Model.Certification
{
    [ActiveRecord( "APICSCertificationMaintenanceApplication", Lazy = true )]
    [DebuggerDisplay( "Maintenance Application: {Id}" )]
    public class MaintenanceApplication
    {
        [PrimaryKey]
        public virtual int Id { get; set; }

        [BelongsTo( "OrderID", NotNull = true, Lazy = FetchWhen.OnInvoke )]
        public virtual Order Order { get; set; }

        [Property( Length = 50 )]
        public virtual string CertificationType { get; set; }

        [Property]
        public virtual DateTime? DeadlineDate { get; set; }

        [Property]
        public virtual bool IsLocked { get; set; }

        [Property]
        public virtual DateTime ApplicationDate { get; set; }

        [Property]
        public virtual DateTime? DateUpdated { get; set; }

    }

}
