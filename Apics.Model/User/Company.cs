using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;

namespace Apics.Model.User
{
    [ActiveRecord( Lazy = true )]
    public class Company
    {
        [PrimaryKey]
        public virtual int Id { get; set; }

        [Property]
        public virtual string Name { get; set; }

        [BelongsTo( "ParentID" )]
        public virtual Company ParentCompany { get; set; }

        [BelongsTo( "RootCompanyID" )]
        public virtual Company RootCompany { get; set; }
    }
}
