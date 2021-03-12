using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class TJTMJRfaDetail
    {
        #region Properties

        public int?      RequestForAssistanceId          { get; set; }
        public int?      ContractorId                    { get; set; }
        public DateTime? ApplicationDate                 { get; set; }
        public DateTime? ApplicationDueDate              { get; set; }
        public bool?     IsUnder18                       { get; set; }
        public int?      HouseholdSizeId                 { get; set; }
        public decimal?  HouseholdIncome                 { get; set; }
        public DateTime? LastEmploymentDate              { get; set; }
        public bool?     HasWorkedLessThan16Hours        { get; set; }
        public bool?     IsEligibleForUnemployment       { get; set; }
        public bool?     IsReceivingW2Benefits           { get; set; }
        public bool?     IsCitizen                       { get; set; }
        public bool?     HasWorked1040Hours              { get; set; }
        public bool?     IsAppCompleteAndSigned          { get; set; }
        public bool?     HasEligibilityBeenVerified      { get; set; }
        public bool?     IsBenefitFromSubsidizedJob      { get; set; }
        public string    BenefitFromSubsidizedJobDetails { get; set; }
        public bool?     IsEligible                      { get; set; }
        public string    PopulationTypeDetails           { get; set; }
        public bool?     HasNeverEmployed                { get; set; }
        public bool      IsDeleted                       { get; set; }
        public string    ModifiedBy                      { get; set; }
        public DateTime? ModifiedDate                    { get; set; }

        #endregion

        #region Navigation Properties

        public virtual RequestForAssistance RequestForAssistance { get; set; }
        public virtual Organization         Organization         { get; set; }

        #endregion
    }
}
