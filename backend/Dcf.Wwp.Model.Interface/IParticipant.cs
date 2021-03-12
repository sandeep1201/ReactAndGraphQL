using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface IParticipantInfo : IHasId
    {
        decimal?  PinNumber         { get; set; }
        string    FirstName         { get; set; }
        string    MiddleInitialName { get; set; }
        string    LastName          { get; set; }
        string    SuffixName        { get; set; }
        DateTime? DateOfBirth       { get; set; }
    }

    public interface IParticipant : ICommonDelModel, IParticipantInfo
    {
        // TODO: Currently this does not match the ParticipantEnrolledProgram table.
        bool      HasPrograms             { get; }
        DateTime? DateOfDeath             { get; set; }
        string    GenderIndicator         { get; set; }
        decimal?  MCI_ID                  { get; set; }
        bool?     HasBeenThroughClientReg { get; set; }
        DateTime? TotalLifetimeHoursDate  { get; set; }
        bool?     Is60DaysVerified        { get; set; }

        // These race and ethnicity properties get written to the DB as Y and N with indicator suffixes.
        bool?                                    IsAmericanIndian                    { get; set; }
        bool?                                    IsAsian                             { get; set; }
        bool?                                    IsBlack                             { get; set; }
        bool?                                    IsHispanic                          { get; set; }
        bool?                                    IsPacificIslander                   { get; set; }
        bool?                                    IsWhite                             { get; set; }
        bool?                                    HasAlias                            { get; set; }
        ICollection<IParticipantEnrolledProgram> EnrolledParticipantEnrolledPrograms { get; }
        ICollection<IInformalAssessment>         InformalAssessments                 { get; set; }
        ICollection<IContact>                    Contacts                            { get; set; }
        ICollection<IAKA>                        AKAs                                { get; set; }
        ICollection<IAKA>                        AllAKAs                             { get; set; }

        // There should only be one ChildYouthSection, but we modeled
        //ICollection<IChildYouthSection> ChildYouthSections { get; set; }
        // There should only be one FamilybarrierSection, but we modeled
        //ICollection<IFamilyBarriersSection> FamilyBarriersSections { get; set; }
        ICollection<IEducationExam>                      EducationExams                      { get; set; }
        ICollection<IParticipantChildRelationshipBridge> ParticipantChildRelationshipBridges { get; set; }
        ICollection<IEmploymentInformation>              EmploymentInformations              { get; set; }
        ICollection<IEmploymentInformation>              AllEmploymentInformations           { get; set; }
        ICollection<IParticipantEnrolledProgram>         ParticipantEnrolledPrograms         { get; set; }
        ICollection<IRequestForAssistance>               RequestsForAssistance               { get; set; }

        ICollection<IParticipantContactInfo> ParticipantContactInfoes { get; set; }
        ICollection<IOtherDemographic>       OtherDemographics        { get; set; }

        ICollection<IConfidentialPinInformation> ConfidentialPinInformations { get; set; }

        // Properties not stored in the database.
        IInformalAssessment InitialInformalAssessment    { get; }
        IInformalAssessment InProgressInformalAssessment { get; }

        // These properties are convenience properties which just get the one and
        // only Section record, even though the database could theoritically have more.
        ILanguageSection                                      LanguageSection                          { get; }
        IWorkHistorySection                                   WorkHistorySection                       { get; }
        IWorkProgramSection                                   WorkProgramSection                       { get; }
        IEducationSection                                     EducationSection                         { get; }
        IPostSecondaryEducationSection                        PostSecondaryEducationSection            { get; }
        IMilitaryTrainingSection                              MilitaryTrainingSection                  { get; }
        IHousingSection                                       HousingSection                           { get; }
        ITransportationSection                                TransportationSection                    { get; }
        ILegalIssuesSection                                   LegalIssuesSection                       { get; }
        IBarrierSection                                       BarrierSection                           { get; }
        IChildYouthSection                                    ChildYouthSection                        { get; }
        IFamilyBarriersSection                                FamilyBarriersSection                    { get; }
        INonCustodialParentsSection                           NonCustodialParentsSection               { get; }
        INonCustodialParentsReferralSection                   NonCustodialParentsReferralSection       { get; }
        ICollection<IBarrierDetail>                           BarrierDetails                           { get; set; }
        ICollection<IBarrierDetail>                           AllBarrierDetails                        { get; set; }
        ICollection<IEmployabilityPlan>                       EmployabilityPlans                       { get; set; }
        ICollection<IParticipationStatu>                      ParticipationStatus                      { get; set; }
        ICollection<IEARequestParticipantBridge>              EaRequestParticipantBridges              { get; set; }
        ICollection<IParticipantEnrolledProgramCutOverBridge> ParticipantEnrolledProgramCutOverBridges { get; set; }
        ICollection<ITransaction>                             Transactions                             { get; set; }
        ICollection<IWorkerTaskList>                          WorkerTaskLists                          { get; set; }

        // Other properties determined on demand
        int? AgeInYears { get; }

        // Properties set by SP_calls.
    }
}
