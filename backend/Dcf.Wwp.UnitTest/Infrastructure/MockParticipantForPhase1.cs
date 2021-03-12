using System;
using System.Collections.Generic;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.UnitTest.Infrastructure
{
    public class MockParticipantForPhase1 : IParticipant
    {
        public int                                                   Id                                       { get; set; }
        public string                                                ModifiedBy                               { get; set; }
        public DateTime?                                             ModifiedDate                             { get; set; }
        public byte[]                                                RowVersion                               { get; set; }
        public bool                                                  IsDeleted                                { get; set; }
        public decimal?                                              PinNumber                                { get; set; }
        public string                                                FirstName                                { get; set; }
        public string                                                MiddleInitialName                        { get; set; }
        public string                                                LastName                                 { get; set; }
        public string                                                SuffixName                               { get; set; }
        public DateTime?                                             DateOfBirth                              { get; set; }
        public bool                                                  HasPrograms                              { get; }
        public DateTime?                                             DateOfDeath                              { get; set; }
        public string                                                GenderIndicator                          { get; set; }
        public decimal?                                              MCI_ID                                   { get; set; }
        public bool?                                                 HasBeenThroughClientReg                  { get; set; }
        public DateTime?                                             TotalLifetimeHoursDate                   { get; set; }
        public bool?                                                 IsAmericanIndian                         { get; set; }
        public bool?                                                 IsAsian                                  { get; set; }
        public bool?                                                 IsBlack                                  { get; set; }
        public bool?                                                 IsHispanic                               { get; set; }
        public bool?                                                 IsPacificIslander                        { get; set; }
        public bool?                                                 IsWhite                                  { get; set; }
        public bool?                                                 HasAlias                                 { get; set; }
        public ICollection<IParticipantEnrolledProgram>              EnrolledParticipantEnrolledPrograms      { get; }
        public ICollection<IInformalAssessment>                      InformalAssessments                      { get; set; }
        public ICollection<IContact>                                 Contacts                                 { get; set; }
        public ICollection<IAKA>                                     AKAs                                     { get; set; }
        public ICollection<IAKA>                                     AllAKAs                                  { get; set; }
        public ICollection<IEducationExam>                           EducationExams                           { get; set; }
        public ICollection<IParticipantChildRelationshipBridge>      ParticipantChildRelationshipBridges      { get; set; }
        public ICollection<IEmploymentInformation>                   EmploymentInformations                   { get; set; }
        public ICollection<IEmploymentInformation>                   AllEmploymentInformations                { get; set; }
        public ICollection<IParticipantEnrolledProgram>              ParticipantEnrolledPrograms              { get; set; }
        public ICollection<IRequestForAssistance>                    RequestsForAssistance                    { get; set; }
        public ICollection<IParticipantContactInfo>                  ParticipantContactInfoes                 { get; set; }
        public ICollection<IOtherDemographic>                        OtherDemographics                        { get; set; }
        public ICollection<IConfidentialPinInformation>              ConfidentialPinInformations              { get; set; }
        public IInformalAssessment                                   InitialInformalAssessment                { get; }
        public IInformalAssessment                                   InProgressInformalAssessment             { get; }
        public ILanguageSection                                      LanguageSection                          { get; }
        public IWorkHistorySection                                   WorkHistorySection                       { get; }
        public IWorkProgramSection                                   WorkProgramSection                       { get; }
        public IEducationSection                                     EducationSection                         { get; }
        public IPostSecondaryEducationSection                        PostSecondaryEducationSection            { get; }
        public IMilitaryTrainingSection                              MilitaryTrainingSection                  { get; }
        public IHousingSection                                       HousingSection                           { get; }
        public ITransportationSection                                TransportationSection                    { get; }
        public ILegalIssuesSection                                   LegalIssuesSection                       { get; }
        public IBarrierSection                                       BarrierSection                           { get; }
        public IChildYouthSection                                    ChildYouthSection                        { get; }
        public IFamilyBarriersSection                                FamilyBarriersSection                    { get; }
        public INonCustodialParentsSection                           NonCustodialParentsSection               { get; }
        public INonCustodialParentsReferralSection                   NonCustodialParentsReferralSection       { get; }
        public ICollection<IBarrierDetail>                           BarrierDetails                           { get; set; }
        public ICollection<IBarrierDetail>                           AllBarrierDetails                        { get; set; }
        public ICollection<IEmployabilityPlan>                       EmployabilityPlans                       { get; set; }
        public ICollection<IParticipationStatu>                      ParticipationStatus                      { get; set; }
        public ICollection<IEARequestParticipantBridge>              EaRequestParticipantBridges              { get; set; }
        public ICollection<IParticipantEnrolledProgramCutOverBridge> ParticipantEnrolledProgramCutOverBridges { get; set; }
        public ICollection<ITransaction>                             Transactions                             { get; set; }
        public ICollection<IWorkerTaskList>                          WorkerTaskLists                          { get; set; }
        public int?                                                  AgeInYears                               { get; }
        public bool?                                                 Is60DaysVerified                         { get; set; }
    }
}
