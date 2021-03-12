using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class EnrolledProgramPinCommentTypeBridge : BaseEntity
    {
        #region Properties

        public int       EnrolledProgramId { get; set; }
        public int       PinCommentTypeId  { get; set; }
        public bool      SystemUseOnly     { get; set; }
        public DateTime  EffectiveDate     { get; set; }
        public DateTime? EndDate           { get; set; }
        public bool      IsDeleted         { get; set; }
        public string    ModifiedBy        { get; set; }
        public DateTime  ModifiedDate      { get; set; }

        #endregion

        #region Navigation Properties

        public virtual EnrolledProgram EnrolledProgram { get; set; }
        public virtual PinCommentType  PinCommentType  { get; set; }

        #endregion
    }
}
