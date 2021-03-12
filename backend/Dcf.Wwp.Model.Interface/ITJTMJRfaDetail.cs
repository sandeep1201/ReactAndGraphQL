using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface ITJTMJRfaDetail : ICommonDelModel
    {
        int?      RequestForAssistanceId          { get; set; }
        int?      ContractorId                    { get; set; }
        DateTime? ApplicationDate                 { get; set; }
        DateTime? ApplicationDueDate              { get; set; }
        bool?     IsUnder18                       { get; set; }
        int?      HouseholdSizeId                 { get; set; }
        decimal?  HouseholdIncome                 { get; set; }
        DateTime? LastEmploymentDate              { get; set; }
        bool?     HasWorkedLessThan16Hours        { get; set; }
        bool?     IsEligibleForUnemployment       { get; set; }
        bool?     IsReceivingW2Benefits           { get; set; }
        bool?     IsCitizen                       { get; set; }
        bool?     HasWorked1040Hours              { get; set; }
        bool?     IsAppCompleteAndSigned          { get; set; }
        bool?     HasEligibilityBeenVerified      { get; set; }
        bool?     IsBenefitFromSubsidizedJob      { get; set; }
        string    BenefitFromSubsidizedJobDetails { get; set; }
        bool?     IsEligible                      { get; set; }
        string    PopulationTypeDetails           { get; set; }
        bool?     HasNeverEmployed                { get; set; }

        IOrganization         Organization         { get; set; }
        IRequestForAssistance RequestForAssistance { get; set; }
    }
}
