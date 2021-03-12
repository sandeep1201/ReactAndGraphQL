using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class WorkProgramSection
    {
        #region Properties

        public int       ParticipantId     { get; set; }
        public bool?     IsInOtherPrograms { get; set; }
        public string    Notes             { get; set; }
        public bool      IsDeleted         { get; set; }
        public string    ModifiedBy        { get; set; }
        public DateTime? ModifiedDate      { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Participant                      Participant          { get; set; }
        public virtual ICollection<InvolvedWorkProgram> InvolvedWorkPrograms { get; set; } = new List<InvolvedWorkProgram>();

        #endregion
    }
}
