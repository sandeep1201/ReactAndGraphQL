using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface IRequestForAssistance : ICommonDelModel
    {
        int       ParticipantId                  { get; set; }
        int       RequestForAssistanceStatusId   { get; set; }
        DateTime? RequestForAssistanceStatusDate { get; set; }
        decimal?  RfaNumber                      { get; set; }
        int       EnrolledProgramId              { get; set; }
        int?      CountyOfResidenceId            { get; set; }
        int?      OfficeId                       { get; set; }
        bool      IsTMJ                          { get; }
        bool      IsTJ                           { get; }
        bool      IsCF                           { get; }
        bool      IsFCDP                         { get; }
        bool      IsEnrolled                     { get; }
        bool      IsReferred                     { get; }
        bool      IsInProgress                   { get; }

        ICountyAndTribe                                        CountyOfResidence                         { get; set; }
        IEnrolledProgram                                       EnrolledProgram                           { get; set; }
        IRequestForAssistanceStatus                            RequestForAssistanceStatus                { get; set; }
        IParticipant                                           Participant                               { get; set; }
        IOffice                                                Office                                    { get; set; }
        ICollection<IRequestForAssistanceChild>                RequestForAssistanceChilds                { get; set; }
        ICollection<IParticipantEnrolledProgram>               ParticipantEnrolledPrograms               { get; set; }
        ICollection<IRequestForAssistanceChild>                AllRequestForAssistanceChilds             { get; set; }
        ICollection<IRequestForAssistancePopulationTypeBridge> RequestForAssistancePopulationTypeBridges { get; set; }
        ICollection<IRequestForAssistanceRuleReason>           RequestForAssistanceRuleReasons           { get; set; }
        ICollection<ICFRfaDetail>                              CFRfaDetails                              { get; set; }
        ICollection<ITJTMJRfaDetail>                           TJTMJRfaDetails                           { get; set; }
        ICollection<IFCDPRfaDetail>                            FCDPRfaDetails                            { get; set; }
    }
}
