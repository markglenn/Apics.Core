using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;
using System.Diagnostics;

namespace Apics.Model.Certification
{
    [ActiveRecord( "APICSCertificationMaintenanceCategory", Lazy = true )]
    [DebuggerDisplay( "Maintenance Category: {Id}" )]
    public class MaintenanceCategory
    {
        [PrimaryKey]
        public virtual int Id { get; set; }

        [Property( NotNull = true, Length = 200 )]
        public virtual string Name { get; set; }

        [Property]
        public virtual int? MaxCPIMPoints { get; set; }

        [Property]
        public virtual int? MaxCFPIMPoints { get; set; }

        [HasMany( ColumnKey = "CategoryID", Lazy = false, OrderBy = "SortOrder" )]
        public virtual IList<MaintenanceActivity> Activities { get; set; }
    }

}
