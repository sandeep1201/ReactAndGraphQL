using System;
using System.Collections.Generic;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class EmploymentInformation : BaseEntity
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

        public virtual Participant                                        Participant                           { get; set; }
        public virtual JobType                                            JobType                               { get; set; }
        public virtual City                                               City                                  { get; set; }
        public virtual WageHour                                           WageHour                              { get; set; }
        public virtual ICollection<EmployabilityPlanEmploymentInfoBridge> EmploybilityPlanEmploymentInfoBridges { get; set; } = new List<EmployabilityPlanEmploymentInfoBridge>();
        public virtual ICollection<WeeklyHoursWorked>                     WeeklyHoursWorked                     { get; set; } = new List<WeeklyHoursWorked>();
        public virtual ICollection<POPClaimEmploymentBridge>              POPClaimEmploymentBridges             { get; set; } = new List<POPClaimEmploymentBridge>();
        public virtual ICollection<Absence>                               Absences                              { get; set; } = new List<Absence>();
        public virtual ICollection<EmploymentVerification>                EmploymentVerifications               { get; set; } = new List<EmploymentVerification>();

        #endregion
    }
}
