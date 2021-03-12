using System;
using System.Collections.Generic;
using Dcf.Wwp.Api.Library.Contracts.InformalAssessment;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class EmploymentInfoContract : BaseInformalAssessmentContract
    {
        public     int                   Id                   { get; set; }
        public     int?                  JobTypeId            { get; set; }
        public     String                JobTypeName          { get; set; }
        public     string                JobBeginDate         { get; set; }
        public     string                JobEndDate           { get; set; }
        public     bool?                 IsCurrentlyEmployed  { get; set; }
        public     decimal?              TotalSubsidizedHours { get; set; }
        public     string                JobPosition          { get; set; }
        public     string                CompanyName          { get; set; }
        public     string                Fein                 { get; set; }
        public     LocationContract      Location             { get; set; }
        public     int?                  ContactId            { get; set; }
        public     List<JobDutyContract> JobDuties            { get; set; }
        public new byte[]                RowVersion           { get; set; }
        public new DateTime?             ModifiedDate         { get; set; }
        public     bool                  IsConverted          { get; set; }
        public     int?                  DeleteReasonId       { get; set; }
        public     string                DeleteReasonName     { get; set; }
        public     string                StreetAddress        { get; set; }
        public     string                ZipAddress           { get; set; }

        // Other job information
        public int?                           JobSectorId               { get; set; }
        public string                         JobSectorName             { get; set; }
        public int?                           EmployerOfRecordId        { get; set; }
        public string                         WorkerId                  { get; set; }
        public string                         ExpectedScheduleDetails   { get; set; }
        public JobActionTypeContract          JobAction                 { get; set; }
        public int?                           JobFoundMethodId          { get; set; }
        public string                         JobFoundMethodName        { get; set; }
        public string                         JobFoundMethodDetails     { get; set; }
        public int?                           WorkProgramId             { get; set; }
        public int?                           LeavingReasonId           { get; set; }
        public String                         LeavingReasonName         { get; set; }
        public string                         LeavingReasonDetails      { get; set; }
        public int?                           EmploymentProgramTypeId   { get; set; }
        public string                         EmploymentProgramTypeName { get; set; }
        public WageHourContract               WageHour                  { get; set; }
        public List<AbsenceContract>          Absences                  { get; set; }
        public EmployerOfRecordDetailContract EmployerOfRecordDetails   { get; set; }
        public string                         Notes                     { get; set; }
        public bool?                          IsCurrentJobAtCreation    { get; set; }
        public bool?                          HasEmploymentOnEp         { get; set; }
        public bool                           IsVerified                { get; set; }
    }
}
