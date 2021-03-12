using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class ParticipationMakeUpEntry : BaseEntity
    {
        #region Properties

        public int      ParticipationEntryId { get; set; }
        public DateTime MakeupDate           { get; set; }
        public decimal  MakeupHours          { get; set; }
        public bool     IsDeleted            { get; set; }
        public string   ModifiedBy           { get; set; }
        public DateTime ModifiedDate         { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ParticipationEntry ParticipationEntry { get; set; }

        #endregion

        #region Clone

        #endregion
    }
}
