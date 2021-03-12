using System;
using System.Collections.Generic;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class CareerAssessment : BaseEntity
    {
        #region Properties

        public int      Id                          { get; set; }
        public int      ParticipantId               { get; set; }
        public DateTime CompletionDate              { get; set; }
        public string   AssessmentProvider          { get; set; }
        public string   AssessmentToolUsed          { get; set; }
        public string   AssessmentResults           { get; set; }
        public int?     CareerAssessmentContactId   { get; set; }
        public string   RelatedOccupation           { get; set; }
        public string   AssessmentResultAppliedToEP { get; set; }
        public bool     IsDeleted                   { get; set; }
        public DateTime CreatedDate                 { get; set; }
        public string   ModifiedBy                  { get; set; }
        public DateTime ModifiedDate                { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Contact                                    Contact                        { get; set; }
        public virtual Participant                                Participant                    { get; set; }
        public virtual ICollection<CareerAssessmentElementBridge> CareerAssessmentElementBridges { get; set; } = new List<CareerAssessmentElementBridge>();

        #endregion

        #region Clone

        #endregion
    }
}
