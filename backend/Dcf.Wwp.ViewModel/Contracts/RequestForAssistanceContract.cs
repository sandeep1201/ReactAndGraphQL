using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class RequestForAssistanceContract : BaseContract
    {
        public decimal?            RfaNumber                            { get; set; }
        public int?                ProgramId                            { get; set; }
        public string              ProgramCode                          { get; set; }
        public string              ProgramName                          { get; set; }
        public int                 StatusId                             { get; set; }
        public string              StatusName                           { get; set; }
        public DateTime?           StatusDate                           { get; set; }
        public int?                CountyOfResidenceId                  { get; set; }
        public string              CountyOfResidenceName                { get; set; }
        public string              AgencyCountyName                     { get; set; }
        public string              AgencyName                           { get; set; }
        public DateTime?           ApplicationDate                      { get; set; }
        public DateTime?           ApplicationDueDate                   { get; set; }
        public DateTime?           EnrollmentDate                       { get; set; }
        public DateTime?           DisenrollmentDate                    { get; set; }
        public int?                CourtOrderCountyTribeId              { get; set; }
        public string              CourtOrderCountyTribeName            { get; set; }
        public int?                CourtOrderCountyTribeNumber          { get; set; }
        public int?                WorkProgramOfficeId                  { get; set; }
        public int?                WorkProgramOfficeNumber              { get; set; }
        public string              WorkProgramOfficeName                { get; set; }
        public DateTime?           CourtOrderEffectiveDate              { get; set; }
        public int?                ContractorId                         { get; set; }
        public string              ContractorName                       { get; set; }
        public int?                CompletionReasonId                   { get; set; }
        public string              CompletionReasonDetails              { get; set; }
        public decimal?            AnnualHouseholdIncome                { get; set; }
        public int?                HouseholdSize                        { get; set; }
        public DateTime?           LastDateOfEmployment                 { get; set; }
        public bool?               HasWorked16HoursLess                 { get; set; }
        public bool?               TjTmjIsEligibleForUnemployment       { get; set; }
        public bool?               TjTmjIsReceivingW2Benefits           { get; set; }
        public bool?               IsUSCitizen                          { get; set; }
        public bool?               TjTmjHasNeverEmployed                { get; set; }
        public bool?               TjTmjHasWorked1040Hours              { get; set; }
        public bool?               TjTmjIsAppCompleteAndSigned          { get; set; }
        public bool?               TjTmjHasEligibilityBeenVerified      { get; set; }
        public bool?               HasChildren                          { get; set; }
        public List<ChildContract> Children                             { get; set; }
        public bool?               TjTmjIsBenefitFromSubsidizedJob      { get; set; }
        public string              TjTmjBenefitFromSubsidizedJobDetails { get; set; }
        public int[]               PopulationTypesIds                   { get; set; }
        public string[]            PopulationTypesNames                 { get; set; }
        public string              PopulationTypeDetails                { get; set; }
        public bool?               IsEligible                           { get; set; }
        public bool                IsVoluntary                          { get; set; }
        public decimal?            KIDSPin                              { get; set; }
        public string              ReferralSource                       { get; set; }
        public List<string>        EligibilityCodes                     { get; set; }
        public string              ModifiedBy                           { get; set; }
        public DateTime?           ModifiedDate                         { get; set; }
        public byte[]              RowVersion                           { get; set; }

        [JsonIgnore]
        public List<string> Alerts { get; set; }

        public RequestForAssistanceContract()
        {
            Children         = new List<ChildContract>();
            Alerts           = new List<string>();
            EligibilityCodes = new List<string>();
        }

        public RequestForAssistanceContract(decimal? rfaNumber, int programId, string programCode, string programName, int statusId, string statusName, DateTime? statusDate, int? countyOfResidenceId,
                                            string countyOfResidenceName, string agencyCountyName, string agencyName, DateTime? applicationDate, DateTime? applicationDueDate, DateTime? enrollmentDate,
                                            DateTime? disenrollmentDate, int? courtOrderCountyTribeId, string courtOrderCountyTribeName, int? courtOrderCountyTribeNumber, int? workProgramOfficeId,
                                            int? workProgramOfficeNumber, string workProgramOfficeName, DateTime? courtOrderEffectiveDate, int? contractorId, string contractorName,
                                            int? completionReasonId, string completionReasonDetails, decimal? annualHouseholdIncome, int? householdSize, DateTime? lastDateOfEmployment,
                                            bool? hasWorked16HoursLess, bool? tjTmjIsEligibleForUnemployment, bool? tjTmjIsReceivingW2Benefits, bool? isUSCitizen, bool? tjTmjHasNeverEmployed,
                                            bool? tjTmjHasWorked1040Hours, bool? tjTmjIsAppCompleteAndSigned, bool? tjTmjHasEligibilityBeenVerified, bool? hasChildren, List<ChildContract> children,
                                            bool? tjTmjIsBenefitFromSubsidizedJob, string tjTmjBenefitFromSubsidizedJobDetails, int[] populationTypesIds, string[] populationTypesNames,
                                            string populationTypeDetails, bool? isEligible, List<string> eligibilityCodes, string modifiedBy, DateTime? modifiedDate, byte[] rowVersion)
        {
            RfaNumber                            = rfaNumber;
            ProgramId                            = programId;
            ProgramCode                          = programCode;
            ProgramName                          = programName;
            StatusId                             = statusId;
            StatusName                           = statusName;
            StatusDate                           = statusDate;
            CountyOfResidenceId                  = countyOfResidenceId;
            CountyOfResidenceName                = countyOfResidenceName;
            AgencyCountyName                     = agencyCountyName;
            AgencyName                           = agencyName;
            ApplicationDate                      = applicationDate;
            ApplicationDueDate                   = applicationDueDate;
            EnrollmentDate                       = enrollmentDate;
            DisenrollmentDate                    = disenrollmentDate;
            CourtOrderCountyTribeId              = courtOrderCountyTribeId;
            CourtOrderCountyTribeName            = courtOrderCountyTribeName;
            CourtOrderCountyTribeNumber          = courtOrderCountyTribeNumber;
            WorkProgramOfficeId                  = workProgramOfficeId;
            WorkProgramOfficeNumber              = workProgramOfficeNumber;
            WorkProgramOfficeName                = workProgramOfficeName;
            CourtOrderEffectiveDate              = courtOrderEffectiveDate;
            ContractorId                         = contractorId;
            ContractorName                       = contractorName;
            CompletionReasonId                   = completionReasonId;
            completionReasonDetails              = completionReasonDetails;
            AnnualHouseholdIncome                = annualHouseholdIncome;
            HouseholdSize                        = householdSize;
            LastDateOfEmployment                 = lastDateOfEmployment;
            HasWorked16HoursLess                 = hasWorked16HoursLess;
            TjTmjIsEligibleForUnemployment       = tjTmjIsEligibleForUnemployment;
            TjTmjIsReceivingW2Benefits           = tjTmjIsReceivingW2Benefits;
            IsUSCitizen                          = isUSCitizen;
            TjTmjHasNeverEmployed                = tjTmjHasNeverEmployed;
            TjTmjHasWorked1040Hours              = tjTmjHasWorked1040Hours;
            TjTmjIsAppCompleteAndSigned          = tjTmjIsAppCompleteAndSigned;
            TjTmjHasEligibilityBeenVerified      = tjTmjHasEligibilityBeenVerified;
            HasChildren                          = hasChildren;
            Children                             = children;
            TjTmjIsBenefitFromSubsidizedJob      = tjTmjIsBenefitFromSubsidizedJob;
            TjTmjBenefitFromSubsidizedJobDetails = tjTmjBenefitFromSubsidizedJobDetails;
            PopulationTypesIds                   = populationTypesIds;
            PopulationTypesNames                 = populationTypesNames;
            PopulationTypeDetails                = populationTypeDetails;
            IsEligible                           = isEligible;
            EligibilityCodes                     = eligibilityCodes;
            ModifiedBy                           = modifiedBy;
            ModifiedDate                         = modifiedDate;
            RowVersion                           = rowVersion;
        }

        /// <remarks>
        /// This is for use by the NRules engine 
        /// </remarks>
        /// <param name="message">Failure message</param>
        public void AddAlert(string message)
        {
            Alerts.Add(message);
        }

        // We if we have the Program ID set, we can detect the program of the RFA.
        [JsonIgnore]
        public bool IsCF => ProgramId == Wwp.Model.Interface.Constants.EnrolledProgram.ChildrenFirstId;

        [JsonIgnore]
        public bool IsTJ => ProgramId == Wwp.Model.Interface.Constants.EnrolledProgram.TransitionalJobsId;

        [JsonIgnore]
        public bool IsTMJ => ProgramId == Wwp.Model.Interface.Constants.EnrolledProgram.TransformMilwaukeeJobsId;

        [JsonIgnore]
        public bool IsFCDP => ProgramId == Wwp.Model.Interface.Constants.EnrolledProgram.FCDPId;
    }
}
