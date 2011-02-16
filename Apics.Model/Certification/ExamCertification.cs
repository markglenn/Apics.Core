using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;
using System.Diagnostics;
using Apics.Model.User;
using System.ComponentModel;

namespace Apics.Model.Certification
{

    public enum CertificationStatus
    {
        [Description( "Active" )]
        Active,

        [Description( "Approved" )]
        Approved,

        [Description( "Exams Required" )]
        ExamsRequired,

        [Description( "HasCFPIM" )]
        HasCFPIM,

        [Description( "Reapply" )]
        Reapply,

        [Description( "Recertified" )]
        Recertified,

        [Description( "Suspended" )]
        Suspended
    }

    [ActiveRecord( "APICSExamCertification", Lazy = true )]
    [DebuggerDisplay( "ExamCertification: {Id}" )]
    public class ExamCertification
    {
        [PrimaryKey]
        public virtual int Id { get; set; }

        [BelongsTo( "PersonID", NotNull = true, Lazy = FetchWhen.OnInvoke )]
        public virtual Person Person { get; set; }

        [Property( NotNull = true, Length = 5 )]
        public virtual string Name { get; set; }

        [Property]
        public virtual DateTime DateCertified { get; set; }

        [Property]
        public virtual DateTime? StopDate { get; set; }

        [Property]
        public virtual bool CertifiedForLife { get; set; }

        [Property( "Status", NotNull = true,
            ColumnType = @"Apics.Model.DescribedEnumStringType`1[Apics.Model.Certification.CertificationStatus], Apics.Model" )]
        public virtual CertificationStatus Status { get; set; }

        [Property]
        public virtual DateTime? StatusDate { get; set; }

        [Property( "OriginalCertDate" )]
        public virtual DateTime OriginalCertificationDate { get; set; }

        [Property]
        public virtual int? PointsNeeded { get; set; }

        [Property]
        public virtual DateTime? DateToAddPenalty { get; set; }

    }

}
