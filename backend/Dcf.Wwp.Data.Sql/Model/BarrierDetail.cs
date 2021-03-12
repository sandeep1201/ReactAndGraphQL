using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class BarrierDetail
    {
        #region Properties

        public int?      ParticipantId            { get; set; }
        public int?      BarrierTypeId            { get; set; }
        public int?      BarrierSectionId         { get; set; }
        public DateTime? OnsetDate                { get; set; }
        public DateTime? EndDate                  { get; set; }
        public bool?     IsAccommodationNeeded    { get; set; }
        public string    Details                  { get; set; }
        public bool      WasClosedAtDisenrollment { get; set; }
        public bool      IsDeleted                { get; set; }
        public string    ModifiedBy               { get; set; }
        public DateTime? ModifiedDate             { get; set; }
        public bool?     IsConverted              { get; set; }

        #endregion

        #region Navigation Properties

        public virtual BarrierSection                               BarrierSection                   { get; set; }
        public virtual BarrierType                                  BarrierType                      { get; set; }
        public virtual Participant                                  Participant                      { get; set; }
        public virtual ICollection<BarrierAccommodation>            BarrierAccommodations            { get; set; } = new List<BarrierAccommodation>();
        public virtual ICollection<BarrierDetailContactBridge>      BarrierDetailContactBridges      { get; set; } = new List<BarrierDetailContactBridge>();
        public virtual ICollection<BarrierTypeBarrierSubTypeBridge> BarrierTypeBarrierSubTypeBridges { get; set; } = new List<BarrierTypeBarrierSubTypeBridge>();
        public virtual ICollection<FormalAssessment>                FormalAssessments                { get; set; } = new List<FormalAssessment>();

        #endregion
    }
}
