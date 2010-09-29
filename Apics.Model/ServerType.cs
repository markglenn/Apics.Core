using System;
using System.Collections.Generic;
using System.Linq;
using Castle.ActiveRecord;

namespace Apics.Model
{
    [ActiveRecord( Lazy = true )]
    public class ServerType
    {
        [PrimaryKey]
        public virtual int Id { get; set; }

        [Property]
        public virtual string Name { get; set; }

        [HasMany]
        public virtual IList<Server> Servers { get; set; }
    }
}