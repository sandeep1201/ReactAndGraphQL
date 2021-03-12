using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class TimeLimit
    {
        #region Properties

        public int?      ParticipantID        { get; set; }
        public decimal?  PIN_NUM               { get; set; }
        public DateTime? EffectiveMonth       { get; set; }
        public int?      TimeLimitTypeId      { get; set; }
        public bool?     TwentyFourMonthLimit { get; set; }
        public bool?     StateTimelimit       { get; set; }
        public bool?     FederalTimeLimit     { get; set; }
        public int?      StateId              { get; set; }
        public int?      ChangeReasonId       { get; set; }
        public string    ChangeReasonDetails  { get; set; }
        public string    Notes                { get; set; }
        public DateTime? CreatedDate          { get; set; }
        public bool      IsDeleted            { get; set; }
        public string    ModifiedBy           { get; set; }
        public DateTime? ModifiedDate         { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Participant    Participant    { get; set; }
        public virtual TimeLimitType  TimeLimitType  { get; set; }
        public virtual TimeLimitState TimeLimitState { get; set; }
        public virtual ChangeReason   ChangeReason   { get; set; }

        #endregion

        #region Convenience Properties

        [NotMapped]
        public decimal? PinNumber
        {
            get => PIN_NUM;
            set => PIN_NUM = value;
        }

        #endregion
    }
}
