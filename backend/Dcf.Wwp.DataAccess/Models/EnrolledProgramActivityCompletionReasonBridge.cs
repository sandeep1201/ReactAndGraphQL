using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class EnrolledProgramActivityCompletionReasonBridge : BaseEntity
    {
        #region Properties

        public int      EnrolledProgramId          { get; set; }
        public int      ActivityCompletionReasonId { get; set; }
        public bool     IsDeleted                  { get; set; }
        public string   ModifiedBy                 { get; set; }
        public DateTime ModifiedDate               { get; set; }

        #endregion

        #region Navigation Properties

        public virtual EnrolledProgram          EnrolledProgram          { get; set; }
        public virtual ActivityCompletionReason ActivityCompletionReason { get; set; }

        #endregion
    }
}
