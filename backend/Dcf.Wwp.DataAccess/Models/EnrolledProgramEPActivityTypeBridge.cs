using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class EnrolledProgramEPActivityTypeBridge : BaseEntity
    {
        #region Properties

        public int?     EnrolledProgramId   { get; set; }
        public int?     ActivityTypeId      { get; set; }
        public bool?    IsSelfDirected      { get; set; }
        public bool     IsDeleted           { get; set; }
        public string   ModifiedBy          { get; set; }
        public DateTime ModifiedDate        { get; set; }
        public bool?    IsUpfrontActivity   { get; set; }
        public bool?    IsSanctionable      { get; set; }
        public bool?    IsAssessmentRelated { get; set; }

        #endregion

        #region Navigation Properties

        public virtual EnrolledProgram EnrolledProgram { get; set; }
        public virtual ActivityType    ActivityType    { get; set; }

        #endregion
    }
}
