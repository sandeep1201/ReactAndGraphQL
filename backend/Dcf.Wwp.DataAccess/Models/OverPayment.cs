using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class OverPayment : BaseEntity
    {
        #region Properties

        public int      ParticipantId                { get; set; }
        public DateTime ParticipationPeriodBeginDate { get; set; }
        public DateTime ParticipationPeriodEndDate   { get; set; }
        public decimal  CaseNumber                   { get; set; }
        public decimal  Amount                       { get; set; }
        public int      OfficeId             { get; set; }
        public string   Reason                       { get; set; }
        public DateTime CreatedDate                  { get; set; }
        public bool     IsDeleted                    { get; set; }
        public string   ModifiedBy                   { get; set; }
        public DateTime ModifiedDate                 { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Participant Participant    { get; set; }
        public virtual Office      Office { get; set; }

        #endregion

        #region Clone

        #endregion
    }
}
