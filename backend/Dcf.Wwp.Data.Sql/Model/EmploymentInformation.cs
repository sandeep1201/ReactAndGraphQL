using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class EmploymentInformation
    {
        #region Properties

        public int       ParticipantId            { get; set; }
        public int?      WorkHistorySectionId     { get; set; }
        public int?      JobTypeId                { get; set; }
        public DateTime? JobBeginDate             { get; set; }
        public DateTime? JobEndDate               { get; set; }
        public bool?     IsCurrentlyEmployed      { get; set; }
        public decimal?  TotalSubsidizedHours     { get; set; }
        public string    JobPosition              { get; set; }
        public string    CompanyName              { get; set; }
        public string    Fein                     { get; set; }
        public string    StreetAddress            { get; set; }
        public string    ZipAddress               { get; set; }
        public int?      CityId                   { get; set; }
        public int?      ContactId                { get; set; }
        public int?      JobDutiesId              { get; set; }
        public int?      LeavingReasonId          { get; set; }
        public int?      OtherJobInformationId    { get; set; }
        public int?      WageHoursId              { get; set; }
        public string    Notes                    { get; set; }
        public int?      EmploymentProgramtypeId  { get; set; }
        public string    LeavingReasonDetails     { get; set; }
        public int?      EmployerOfRecordTypeId   { get; set; }
        public short?    EmploymentSequenceNumber { get; set; }
        public short?    OriginalOfficeNumber     { get; set; }
        public bool?     IsConverted              { get; set; }
        public bool?     IsCurrentJobAtCreation   { get; set; }
        public int?      DeleteReasonId           { get; set; }
        public int?      SortOrder                { get; set; }
        public string    ModifiedBy               { get; set; }
        public DateTime? ModifiedDate             { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Participant                                                 Participant                                     { get; set; }
        public virtual WorkHistorySection                                          WorkHistorySection                              { get; set; }
        public virtual JobType                                                     JobType                                         { get; set; }
        public virtual City                                                        City                                            { get; set; }
        public virtual Contact                                                     Contact                                         { get; set; }
        public virtual LeavingReason                                               LeavingReason                                   { get; set; }
        public virtual DeleteReason                                                DeleteReason                                    { get; set; }
        public virtual OtherJobInformation                                         OtherJobInformation                             { get; set; }
        public virtual WageHour                                                    WageHour                                        { get; set; }
        public virtual EmploymentProgramType                                       EmploymentProgramType                           { get; set; }
        public virtual EmployerOfRecordType                                        EmployerOfRecordType                            { get; set; }
        public virtual ICollection<Absence>                                        Absences                                        { get; set; } = new List<Absence>();
        public virtual ICollection<EmployerOfRecordInformation>                    EmployerOfRecordInformations                    { get; set; } = new List<EmployerOfRecordInformation>();
        public virtual ICollection<EmploymentInformationBenefitsOfferedTypeBridge> EmploymentInformationBenefitsOfferedTypeBridges { get; set; } = new List<EmploymentInformationBenefitsOfferedTypeBridge>();
        public virtual ICollection<EmploymentInformationJobDutiesDetailsBridge>    EmploymentInformationJobDutiesDetailsBridges    { get; set; } = new List<EmploymentInformationJobDutiesDetailsBridge>();
        public virtual ICollection<EPEIBridge>                                     EPEIBridges                                     { get; set; } = new List<EPEIBridge>();
        public virtual ICollection<WeeklyHoursWorked>                              WeeklyHoursWorkedEntries                        { get; set; } = new List<WeeklyHoursWorked>();
        public virtual ICollection<EmploymentVerification>                         EmploymentVerifications                         { get; set; } = new List<EmploymentVerification>();

        #endregion

        [NotMapped]
        public bool IsDeleted
        {
            get => DeleteReasonId.HasValue;
            set {  }
        }
    }
}
