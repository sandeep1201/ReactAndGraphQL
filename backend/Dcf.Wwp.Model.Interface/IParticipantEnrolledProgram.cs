using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface IParticipantEnrolledProgram : ICommonModel
    {
        int                               ParticipantId               { get; set; }
        int?                              EnrolledProgramId           { get; set; }
        int?                              EnrolledProgramStatusCodeId { get; set; }
        DateTime?                         EnrollmentDate              { get; set; }
        DateTime?                         DisenrollmentDate           { get; set; }
        int?                              CompletionReasonId          { get; set; }
        decimal?                          CASENumber                  { get; set; }
        DateTime?                         ReferralDate                { get; set; }
        int?                              WorkerId                    { get; set; }
        IEnrolledProgram                  EnrolledProgram             { get; set; }
        IOffice                           Office                      { get; set; }
        IParticipant                      Participant                 { get; set; }
        IWorker                           Worker                      { get; set; }
        IWorker                           LFFEP                       { get; set; }
        IEnrolledProgramStatusCode        StatusCode                  { get; set; }
        ICollection<IOfficeTransfer>      OfficeTransfers             { get; set; }
        ICollection<IPEPOtherInformation> PEPOtherInformations        { get; set; }
        IRequestForAssistance             RequestForAssistance        { get; set; }
        ICollection<IEmployabilityPlan>   EmployabilityPlans          { get; set; }
        int?                              LFFEPId                     { get; set; }
        bool                              IsW2                        { get; }
        bool                              IsTmj                       { get; }
        bool                              IsTJ                        { get; }
        bool                              IsCF                        { get; }
        bool                              IsFCDP                      { get; }
        bool                              IsLF                        { get; }
        bool                              IsDisenrolled               { get; }
        bool                              IsEnrolled                  { get; }
        bool                              IsReferred                  { get; }
        bool                              CanTransferContractAreas(string originContractArea, string destinationContractArea);
        bool                              IsInBalanceOfState       { get; }
        bool                              IsInMilwaukee            { get; }
        string                            CurrentRegCode           { get; set; }
        string                            ReferralRegistrationCode { get; set; }
    }
}
