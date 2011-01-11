using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;
using System.Diagnostics;

namespace Apics.Model.User
{
    [ActiveRecord( Lazy = true )]
    [DebuggerDisplay( "MemberType: {Id}" )]
    public class MemberType
    {
        [PrimaryKey]
        public virtual int Id { get; set; }

        [Property( NotNull = true, Length = 100 )]
        public virtual string Name { get; set; }

        [BelongsTo( "ParentID", Lazy = FetchWhen.OnInvoke )]
        public virtual MemberType Parent { get; set; }

        [Property( Length = 255 )]
        public virtual string Description { get; set; }

        [Property]
        public virtual bool IsMember { get; set; }

        [Property]
        public virtual bool IsActive { get; set; }

        [Property( NotNull = true, Length = 100 )]
        public virtual string DefaultType { get; set; }

    }

}
