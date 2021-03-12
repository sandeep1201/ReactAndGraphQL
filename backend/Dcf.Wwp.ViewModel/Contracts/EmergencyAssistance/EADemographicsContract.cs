using System;

namespace Dcf.Wwp.Api.Library.Contracts.EmergencyAssistance
{
    public class EADemographicsContract : BaseEAContract
    {
        public DateTime                      ApplicationDate                 { get; set; }
        public decimal?                      CaresCaseNumber                 { get; set; }
        public EADemographicsContactContract EaDemographicsContact           { get; set; }
        public bool?                         DidApplicantTakeCareOfAnyChild  { get; set; }
        public bool?                         WillTheChildStayInApplicantCare { get; set; }
        public int?                          ApplicationInitiatedMethodId    { get; set; }
        public string                        ApplicationInitiatedMethodCode  { get; set; }
        public string                        ApplicationInitiatedMethodName  { get; set; }
        public decimal?                      AccessTrackingNumber            { get; set; }
    }

    public class EADemographicsContactContract
    {
        public bool?                   IsHomeless                      { get; set; }
        public bool?                   IsMailingSameAsHouseholdAddress { get; set; }
        public int?                    CountyOfResidenceId             { get; set; }
        public string                  CountyOfResidenceName           { get; set; }
        public FinalistAddressContract HouseholdAddress                { get; set; }
        public FinalistAddressContract MailingAddress                  { get; set; }
        public string                  PhoneNumber                     { get; set; }
        public bool?                   CanText                         { get; set; }
        public string                  AlternatePhoneNumber            { get; set; }
        public bool?                   CanTextAlternate                { get; set; }
        public string                  EmailAddress                    { get; set; }
        public string                  BestWayToReach                  { get; set; }
    }
}
