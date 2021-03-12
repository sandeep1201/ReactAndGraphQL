using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class ParticipationPeriodSummary : BaseEntity
    {
        #region Properties

        public int       ParticipantId                { get; set; }
        public DateTime  ParticipationPeriodBeginDate { get; set; }
        public DateTime  ParticipationPeriodEndDate   { get; set; }
        public decimal   CaseNumber                   { get; set; }
        public decimal?  AppliedHours                 { get; set; }
        public decimal?  UnAppliedHours               { get; set; }
        public decimal?  SanctionableHours            { get; set; }
        public bool      IsDeleted                    { get; set; }
        public string    ModifiedBy                   { get; set; }
        public DateTime? ModifiedDate                 { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Participant Participant { get; set; }

        #endregion

        #region Clone

        #endregion
    }
}
