using System;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class EmployabilityPlanContract
    {
        #region Properties

        public int       Id                              { get; set; }
        public int       EnrolledProgramId               { get; set; }
        public string    EnrolledProgramName             { get; set; }
        public int       PEPId                           { get; set; }
        public string    EnrolledProgramCd               { get; set; }
        public DateTime  BeginDate                       { get; set; }
        public DateTime  EndDate                         { get; set; }
        public int?      EmployabilityPlanStatusTypeId   { get; set; }
        public string    EmployabilityPlanStatusTypeName { get; set; }
        public string    ModifiedBy                      { get; set; }
        public DateTime  ModifiedDate                    { get; set; }
        public bool?     IsWorkerDeleted                 { get; set; }
        public bool?     IsDeleted                       { get; set; }
        public bool?     CanHaveActivity                 { get; set; }
        public string    CanHaveActivityDetails          { get; set; }
        public string    Notes                           { get; set; }
        public DateTime? CreatedDate                     { get; set; }
        public bool?     CanSaveWithoutActivity          { get; set; }
        public string    CanSaveWithoutActivityDetails   { get; set; }
        public int?      OrganizationId                  { get; set; }
        public DateTime? SubmitDate                      { get; set; }
        public string    ErrorMessage                    { get; set; }

        #endregion

        #region Methods

        public EmployabilityPlanContract ()
        {
        }

        public EmployabilityPlanContract(int       id,                            int    enrolledProgramId,               string enrolledProgramName,           string enrolledProgramCd, DateTime beginDate, DateTime endDate,
                                         int?      employabilityPlanStatusTypeId, string employabilityPlanStatusTypeName, string notes,                         string modifiedBy,        DateTime modifiedDate,
                                         DateTime? createdDate,                   bool?  canSaveWithoutActivity,          string canSaveWithoutActivityDetails, int?   organizationId,    int      pepId, DateTime? submitDate)

        {
            Id                              = id;
            EnrolledProgramId               = enrolledProgramId;
            EnrolledProgramName             = enrolledProgramName;
            EnrolledProgramCd               = enrolledProgramCd;
            BeginDate                       = beginDate;
            EndDate                         = endDate;
            CanHaveActivity                 = canSaveWithoutActivity;
            CanHaveActivityDetails          = canSaveWithoutActivityDetails;
            IsWorkerDeleted                 = IsWorkerDeleted;
            EmployabilityPlanStatusTypeId   = employabilityPlanStatusTypeId;
            EmployabilityPlanStatusTypeName = employabilityPlanStatusTypeName;
            Notes                           = notes;
            IsDeleted                       = IsDeleted;
            ModifiedBy                      = modifiedBy;
            ModifiedDate                    = modifiedDate;
            CreatedDate                     = createdDate;
            CanSaveWithoutActivity          = canSaveWithoutActivity;
            CanSaveWithoutActivityDetails   = canSaveWithoutActivityDetails;
            OrganizationId                  = organizationId;
            PEPId                           = pepId;
            SubmitDate                      = submitDate;
        }

        #endregion
    }
}
