using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;
using System.Diagnostics;
using Apics.Model.User;

namespace Apics.Model.Certification
{
    [ActiveRecord( "PearsonVuePersonSubmission", Lazy = true )]
    [DebuggerDisplay( "PersonSubmission: {Id}" )]
    public class PersonSubmission
    {
        [PrimaryKey]
        public virtual int Id { get; set; }

        [BelongsTo( "PersonID", Lazy = FetchWhen.OnInvoke )]
        public virtual Person Person { get; set; }

        [Property]
        public virtual DateTime SubmissionDateTime { get; set; }

        [Property]
        public virtual bool IsAccepted { get; set; }

        [Property]
        public virtual DateTime? LastDateAccepted { get; set; }

        [Property( Length = 512 )]
        public virtual string Message { get; set; }

    }

}
