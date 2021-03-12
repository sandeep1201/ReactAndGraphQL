using System.Collections.Generic;

namespace Dcf.Wwp.Api.Library.Contracts.EmergencyAssistance
{
    public class EAHouseHoldFinancialsContract : BaseEAContract
    {
        public bool?                           HasNoIncome        { get; set; }
        public bool?                           HasNoAssets        { get; set; }
        public bool?                           HasNoVehicles      { get; set; }
        public List<EAHouseHoldIncomeContract> EaHouseHoldIncomes { get; set; }
        public List<EAAssetsContract>          EaAssets           { get; set; }
        public List<EAVehiclesContract>        EaVehicles         { get; set; }
    }

    public class EAHouseHoldIncomeContract
    {
        public int?   Id                   { get; set; }
        public string IncomeType           { get; set; }
        public string MonthlyIncome        { get; set; }
        public int?   VerificationTypeId   { get; set; }
        public string VerificationTypeName { get; set; }
        public int?   GroupMember          { get; set; }
    }

    public class EAAssetsContract
    {
        public int?   Id                   { get; set; }
        public string AssetType            { get; set; }
        public string CurrentValue         { get; set; }
        public int?   VerificationTypeId   { get; set; }
        public string VerificationTypeName { get; set; }
        public int?   AssetOwner           { get; set; }
    }

    public class EAVehiclesContract
    {
        public int?   Id                               { get; set; }
        public string VehicleType                      { get; set; }
        public string VehicleValue                     { get; set; }
        public string AmountOwed                       { get; set; }
        public string VehicleEquity                    { get; set; }
        public int?   OwnerVerificationTypeId          { get; set; }
        public string OwnerVerificationTypeName        { get; set; }
        public int?   VehicleValueVerificationTypeId   { get; set; }
        public string VehicleValueVerificationTypeName { get; set; }
        public int?   OwedVerificationTypeId           { get; set; }
        public string OwedVerificationTypeName         { get; set; }
        public int?   VehicleOwner                     { get; set; }
    }
}
