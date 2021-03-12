using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class AfterPullDownWeeklyBatchDetails : BaseEntity
    {
        #region Properties

        public int      ParticipantId          { get; set; }
        public decimal  CaseNumber             { get; set; }
        public DateTime ParticipationBeginDate { get; set; }
        public DateTime ParticipationEndDate   { get; set; }
        public DateTime WeeklyBatchDate        { get; set; }
        public decimal  PreviousNPHours        { get; set; }
        public decimal  PreviousGCHours        { get; set; }
        public decimal  CurrentNPHours         { get; set; }
        public decimal  CurrentGCHours         { get; set; }
        public decimal  PreviousUnAppliedHours { get; set; }
        public decimal  CurrentUnAppliedHours  { get; set; }
        public decimal  Calculation            { get; set; }
        public string   OverPaymentOrAux       { get; set; }
        public bool     IsDeleted              { get; set; }
        public string   ModifiedBy             { get; set; }
        public DateTime ModifiedDate           { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Participant Participant { get; set; }

        #endregion

        #region Clone

        #endregion
    }
}
