using System.Collections.Generic;

namespace Dcf.Wwp.Api.Library.Contracts.EmergencyAssistance
{
    public class EAAgencySummaryContract : BaseEAContract
    {
        public int                           GroupSize                     { get; set; }
        public string                        TotalIncome                   { get; set; }
        public string                        TotalFinancialAssets          { get; set; }
        public string                        TotalVehicleAssets            { get; set; }
        public string                        TotalAssets                   { get; set; }
        public bool                          ApprovedPaymentsPast12Months  { get; set; }
        public bool                          HasActiveIPV                  { get; set; }
        public bool                          HasPendingIPV                 { get; set; }
        public EAEmergencyTypeContract       EaEmergencyType               { get; set; }
        public int?                          StatusId                      { get; set; }
        public string                        StatusName                    { get; set; }
        public List<int?>                    StatusReasonIds               { get; set; }
        public List<string>                  StatusReasonCodes             { get; set; }
        public List<string>                  StatusReasonNames             { get; set; }
        public string                        ApprovedPaymentAmount         { get; set; }
        public decimal?                      MaxPaymentAmount              { get; set; }
        public bool                          HasFinancialEligibilityPassed { get; set; }
        public bool                          HasComment                    { get; set; }
        public string                        Notes                         { get; set; }
        public bool?                         IsSubmit                      { get; set; }
        public List<EAFinancialNeedContract> EaFinancialNeeds              { get; set; }
    }

    public class EAFinancialNeedContract
    {
        public int?   Id                    { get; set; }
        public string Amount                { get; set; }
        public int?   FinancialNeedTypeId   { get; set; }
        public string FinancialNeedTypeName { get; set; }
    }
}
