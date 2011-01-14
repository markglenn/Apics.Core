using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;
using System.Diagnostics;
using Apics.Model.User;
using NHibernate.Type;

namespace Apics.Model.Certification
{
    public enum ApplicationStatus
    {
        Approved,
        Denied,
        Applied
    }

    public class ApplicationStatusType : EnumStringType<ApplicationStatus> { }

    [ActiveRecord( "APICSCertificationApplication", Lazy = true )]
    [DebuggerDisplay( "APICSCertificationApplication: {Id}" )]
    public class Application
    {
        public Application( )
        {
            this.ApplicationDate = DateTime.Now;
            this.University = String.Empty;
        }

        [PrimaryKey]
        public virtual int Id { get; set; }

        [BelongsTo( "PersonID", NotNull = true, Lazy = FetchWhen.OnInvoke )]
        public virtual Person Person { get; set; }

        [BelongsTo( "APICSExamID", NotNull = true, Lazy = FetchWhen.OnInvoke )]
        public virtual Exam Exam { get; set; }

        [Property]
        public virtual DateTime ApplicationDate { get; set; }

        [Property]
        public virtual DateTime? ExamDate { get; set; }

        [Property]
        public virtual bool HasBachelorsDegree { get; set; }

        [Property( NotNull = true, Length = 200 )]
        public virtual string University { get; set; }

        [Property]
        public virtual bool HasCFPIM { get; set; }

        [Property]
        public virtual bool HasCPIM { get; set; }

        [Property]
        public virtual bool HasCIRM { get; set; }

        [Property]
        public virtual bool HasCPM { get; set; }

        [Property( Length = 100 )]
        public virtual string OtherCertifications { get; set; }

        [Property( "ApplicationStatus", NotNull = true, Length = 25,
            ColumnType = "Apics.Model.Certification.ApplicationStatusType, Apics.Model" )]
        public virtual ApplicationStatus Status { get; set; }

        [Property]
        public virtual int EmploymentYears { get; set; }

        [Property]
        public virtual bool WasAudited { get; set; }

        [Property( Length = 4000 )]
        public virtual string Notes { get; set; }

    }

}
