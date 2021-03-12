using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class TimeLimit : BaseEntity
    {
        #region Properties

        public int?      ParticipantId        { get; set; }
        public DateTime? EffectiveMonth       { get; set; }
        public int?      TimeLimitTypeId      { get; set; }
        public bool?     TwentyFourMonthLimit { get; set; }
        public bool?     StateTimelimit       { get; set; }
        public bool?     FederalTimeLimit     { get; set; }
        public int?      StateId              { get; set; }
        public int?      ChangeReasonId       { get; set; }
        public string    ChangeReasonDetails  { get; set; }
        public string    Notes                { get; set; }
        public bool      IsDeleted            { get; set; }
        public DateTime? CreatedDate          { get; set; }
        public string    ModifiedBy           { get; set; }
        public DateTime? ModifiedDate         { get; set; }
        public decimal?  PIN_NUM              { get; set; }

        #endregion

        #region  Navigation Properties

        public virtual Participant Participant { get; set; }

        #endregion
    }
}
