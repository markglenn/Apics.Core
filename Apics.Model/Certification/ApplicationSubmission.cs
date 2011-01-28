using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;
using System.Diagnostics;
using Apics.Model.User;

namespace Apics.Model.Certification
{
    [ActiveRecord( "PearsonVueCertApplicationSubmission", Lazy = true )]
    [DebuggerDisplay( "ApplicationSubmission: {Id}" )]
    public class ApplicationSubmission
    {
        [PrimaryKey]
        public virtual int Id { get; set; }

        [BelongsTo( "APICSCertificationApplicationID", Lazy = FetchWhen.OnInvoke )]
        public virtual Application Application { get; set; }

        [Property]
        public virtual DateTime SubmissionDateTime { get; set; }

        [Property]
        public virtual bool IsAccepted { get; set; }

        [Property( Length = 512 )]
        public virtual string Message { get; set; }

        [Property]
        public virtual DateTime? LastDateAccepted { get; set; }
    }

}
