using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;
using System.Diagnostics;

namespace Apics.Model.Certification
{
    [ActiveRecord( "APICSCertificationMaintenanceActivityPoint", Lazy = true )]
    [DebuggerDisplay( "Maintenance Activity Point: {Id}" )]
    public class MaintenanceActivityPoint
    {
        [PrimaryKey]
        public virtual int Id { get; set; }

        [BelongsTo( "ActivityID", NotNull = true, Lazy = FetchWhen.OnInvoke )]
        public virtual MaintenanceActivity Activity { get; set; }

        [Property]
        public virtual int Year { get; set; }

        [Property]
        public virtual decimal Points { get; set; }

    }

}
