using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface IEmploymentInformation : ICommonModel, ICloneable, IHasDeleteReason
    {
        Int32                                                        ParticipantId                                      { get; set; }
        Int32?                                                       WorkHistorySectionId                               { get; set; }
        Int32?                                                       JobTypeId                                          { get; set; }
        DateTime?                                                    JobBeginDate                                       { get; set; }
        DateTime?                                                    JobEndDate                                         { get; set; }
        Boolean?                                                     IsCurrentlyEmployed                                { get; set; }
        Decimal?                                                     TotalSubsidizedHours                               { get; set; }
        String                                                       JobPosition                                        { get; set; }
        String                                                       CompanyName                                        { get; set; }
        String                                                       Fein                                               { get; set; }
        String                                                       StreetAddress                                      { get; set; }
        String                                                       ZipAddress                                         { get; set; }
        Int32?                                                       CityId                                             { get; set; }
        Int32?                                                       ContactId                                          { get; set; }
        Int32?                                                       JobDutiesId                                        { get; set; }
        Int32?                                                       LeavingReasonId                                    { get; set; }
        Int32?                                                       EmploymentProgramtypeId                            { get; set; }
        Int32?                                                       SortOrder                                          { get; set; }
        Boolean                                                      IsDeleted                                          { get; set; }
        Int32?                                                       OtherJobInformationId                              { get; set; }
        Int32?                                                       WageHoursId                                        { get; set; }
        String                                                       Notes                                              { get; set; }
        String                                                       LeavingReasonDetails                               { get; set; }
        Boolean?                                                     IsConverted                                        { get; set; }
        short?                                                       EmploymentSequenceNumber                           { get; set; }
        short?                                                       OriginalOfficeNumber                               { get; set; }
        bool?                                                        IsCurrentJobAtCreation                             { get; set; }
        ICollection<IAbsence>                                        Absences                                           { get; set; }
        ICollection<IAbsence>                                        AllAbsences                                        { get; set; }
        ICity                                                        City                                               { get; set; }
        IContact                                                     Contact                                            { get; set; }
        IJobType                                                     JobType                                            { get; set; }
        ILeavingReason                                               LeavingReason                                      { get; set; }
        IOtherJobInformation                                         OtherJobInformation                                { get; set; }
        IParticipant                                                 Participant                                        { get; set; }
        IWageHour                                                    WageHour                                           { get; set; }
        int?                                                         EmployerOfRecordTypeId                             { get; set; }
        IEmploymentProgramType                                       EmploymentProgramType                              { get; set; }
        IEmployerOfRecordType                                        EmployerOfRecordType                               { get; set; }
        IWorkHistorySection                                          WorkHistorySection                                 { get; set; }
        ICollection<IEmploymentInformationBenefitsOfferedTypeBridge> EmploymentInformationBenefitsOfferedTypeBridges    { get; set; }
        ICollection<IEmploymentInformationBenefitsOfferedTypeBridge> AllEmploymentInformationBenefitsOfferedTypeBridges { get; set; }
        ICollection<IEmploymentInformationJobDutiesDetailsBridge>    EmploymentInformationJobDutiesDetailsBridges       { get; set; }
        ICollection<IEmploymentInformationJobDutiesDetailsBridge>    AllEmploymentInformationJobDutiesDetailsBridges    { get; set; }
        ICollection<IEmployerOfRecordInformation>                    EmployerOfRecordInformations                       { get; set; }
        ICollection<IEPEIBridge>                                     EPEIBridges                                        { get; set; }
        ICollection<IWeeklyHoursWorked>                              WeeklyHoursWorkedEntries                           { get; set; }
        ICollection<IEmploymentVerification>                         EmploymentVerifications                            { get; set; }
    }
}
