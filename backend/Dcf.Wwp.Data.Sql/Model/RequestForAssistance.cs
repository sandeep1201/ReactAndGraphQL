using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class RequestForAssistance
    {
        #region Properties

        public int       ParticipantId                  { get; set; }
        public int       RequestForAssistanceStatusId   { get; set; }
        public DateTime? RequestForAssistanceStatusDate { get; set; }
        public decimal?  RfaNumber                      { get; set; }
        public int       EnrolledProgramId              { get; set; }
        public int?      CountyOfResidenceId            { get; set; }
        public int?      OfficeId                       { get; set; }
        public bool      IsDeleted                      { get; set; }
        public string    ModifiedBy                     { get; set; }
        public DateTime? ModifiedDate                   { get; set; }

        #endregion

        #region Navigation Properties

        public virtual CountyAndTribe                                        CountyOfResidence                         { get; set; }
        public virtual EnrolledProgram                                       EnrolledProgram                           { get; set; }
        public virtual Participant                                           Participant                               { get; set; }
        public virtual RequestForAssistanceStatus                            RequestForAssistanceStatus                { get; set; }
        public virtual Office                                                Office                                    { get; set; }
        public virtual ICollection<RequestForAssistanceChild>                RequestForAssistanceChilds                { get; set; } = new List<RequestForAssistanceChild>();
        public virtual ICollection<ParticipantEnrolledProgram>               ParticipantEnrolledPrograms               { get; set; } = new List<ParticipantEnrolledProgram>();
        public virtual ICollection<RequestForAssistancePopulationTypeBridge> RequestForAssistancePopulationTypeBridges { get; set; } = new List<RequestForAssistancePopulationTypeBridge>();
        public virtual ICollection<RequestForAssistanceRuleReason>           RequestForAssistanceRuleReasons           { get; set; } = new List<RequestForAssistanceRuleReason>();
        public virtual ICollection<CFRfaDetail>                              CFRfaDetails                              { get; set; } = new List<CFRfaDetail>();
        public virtual ICollection<FCDPRfaDetail>                            FCDPRfaDetails                            { get; set; } = new List<FCDPRfaDetail>();
        public virtual ICollection<TJTMJRfaDetail>                           TJTMJRfaDetails                           { get; set; } = new List<TJTMJRfaDetail>();

        #endregion
    }
}
