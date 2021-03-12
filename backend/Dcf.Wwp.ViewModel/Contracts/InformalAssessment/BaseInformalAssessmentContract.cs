using System;

namespace Dcf.Wwp.Api.Library.Contracts.InformalAssessment
{
    public abstract class BaseInformalAssessmentContract
    {
        public bool IsSubmittedViaDriverFlow { get; set; }
        public byte[] RowVersion { get; set; }
        public byte[] AssessmentRowVersion { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedByName { get; set; }
    }
}
