using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Castle.ActiveRecord;

namespace Apics.Model.Certification
{
    [ActiveRecord( "APICSExam", Lazy = true )]
    [DebuggerDisplay( "APICSExam: {Id}" )]
    public class Exam
    {
        [PrimaryKey]
        public virtual int Id { get; set; }

        [Property( NotNull = true, Length = 50 )]
        public virtual string Name { get; set; }

        [Property( NotNull = true, Length = 50 )]
        public virtual string Description { get; set; }

        [Property]
        public virtual int TransferCode { get; set; }

        [Property]
        public virtual DateTime StartTestingDate { get; set; }

        [Property]
        public virtual DateTime StopTestingDate { get; set; }

        [Property( NotNull = true, Length = 3 )]
        public virtual string Program { get; set; }

        [Property( Length = 25 )]
        public virtual string PearsonVUECode { get; set; }

        [Property]
        public virtual DateTime? AuthorizationEndDate { get; set; }

        [Property]
        public virtual bool RequiresPayment { get; set; }

        [Property]
        public virtual int? AuthorizationCount { get; set; }

    }

}
