using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Api.Library.Contracts.EmergencyAssistance
{
    public class EAEmergencyTypeContract : BaseEAContract
    {
        public List<int>                       EmergencyTypeIds        { get; set; }
        public List<string>                    EmergencyTypeCodes      { get; set; }
        public List<string>                    EmergencyTypeNames      { get; set; }
        public string                          EmergencyDetails        { get; set; }
        public EAImpendingHomelessnessContract EaImpendingHomelessness { get; set; }
        public EAHomelessnessContract          EaHomelessness          { get; set; }
        public EAEnergyCrisisContract          EaEnergyCrisis          { get; set; }
    }

    public class EAImpendingHomelessnessContract
    {
        public bool?                   HaveEvictionNotice                       { get; set; }
        public string                  DateOfEvictionNotice                     { get; set; }
        public string                  DifficultToPayDetails                    { get; set; }
        public bool?                   IsCurrentLandLordUnknown                 { get; set; }
        public string                  LandLordName                             { get; set; }
        public string                  ContactPerson                            { get; set; }
        public string                  LandLordPhone                            { get; set; }
        public FinalistAddressContract LandLordAddress                          { get; set; }
        public bool?                   NeedingDifferentHomeForAbuse             { get; set; }
        public bool?                   NeedingDifferentHomeForRentalForeclosure { get; set; }
        public string                  DateOfFamilyDeparture                    { get; set; }
        public bool?                   IsYourBuildingDecidedUnSafe              { get; set; }
        public string                  DateBuildingWasDecidedUnSafe             { get; set; }
        public bool?                   IsInspectionReportAvailable              { get; set; }
        public int?                    EmergencyTypeReasonId                    { get; set; }
        public string                  EmergencyTypeReasonName                  { get; set; }
    }

    public class EAHomelessnessContract
    {
        public bool?  InLackOfPlace                { get; set; }
        public string DateOfStart                  { get; set; }
        public bool?  PlanOnPermanentPlace         { get; set; }
        public bool?  InShelterForDomesticAbuse    { get; set; }
        public bool?  IsYourBuildingDecidedUnSafe  { get; set; }
        public string DateBuildingWasDecidedUnSafe { get; set; }
        public bool?  IsInspectionReportAvailable  { get; set; }
        public int?   EmergencyTypeReasonId        { get; set; }
        public string EmergencyTypeReasonName      { get; set; }
    }

    public class EAEnergyCrisisContract
    {
        public bool?  InNeedForUtilities       { get; set; }
        public string DifficultyForUtilityBill { get; set; }
        public string ExistingAppliedHelp      { get; set; }
        public bool?  HaveThreat               { get; set; }
    }
}
