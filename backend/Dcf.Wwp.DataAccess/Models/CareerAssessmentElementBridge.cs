using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class CareerAssessmentElementBridge : BaseEntity
    {
        #region Properties

        public int      CareerAssessmentId { get; set; }
        public int      ElementId          { get; set; }
        public bool      IsDeleted          { get; set; }
        public string    ModifiedBy         { get; set; }
        public DateTime? ModifiedDate       { get; set; }

        #endregion

        #region Nav Properties

        public virtual CareerAssessment CareerAssessment { get; set; }
        public virtual Element          Element          { get; set; }

        #endregion

    }
}
