using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public partial class EducationExam : BaseEntity
    {
        #region Properties

        public int?      ParticipantId { get; set; }
        public int?      ExamTypeId    { get; set; }
        public DateTime? DateTaken     { get; set; }
        public string    Details       { get; set; }
        public bool      IsDeleted     { get; set; }
        public string    ModifiedBy    { get; set; }
        public DateTime? ModifiedDate  { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Participant Participant { get; set; }

        #endregion
    }
}
