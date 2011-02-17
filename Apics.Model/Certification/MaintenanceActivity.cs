using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;
using System.Diagnostics;

namespace Apics.Model.Certification
{
    [ActiveRecord( "APICSCertificationMaintenanceActivity", Lazy = true )]
    [DebuggerDisplay( "Maintenance Activity: {Id}" )]
    public class MaintenanceActivity
    {
        [PrimaryKey]
        public virtual int Id { get; set; }

        [BelongsTo( "CategoryID", NotNull = true, Lazy = FetchWhen.OnInvoke )]
        public virtual MaintenanceCategory Category { get; set; }

        [Property( NotNull = true, Length = 200 )]
        public virtual string Description { get; set; }

        [Property( NotNull = true, Length = 200 )]
        public virtual string PointsDescription { get; set; }

        [Property]
        public virtual int? SortOrder { get; set; }

    }

}
