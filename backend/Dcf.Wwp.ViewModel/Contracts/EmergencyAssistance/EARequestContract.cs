using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Api.Library.Contracts.EmergencyAssistance
{
    public class EARequestContract
    {
        public int                           Id                    { get; set; }
        public decimal                       RequestNumber         { get; set; }
        public decimal?                      CaresCaseNumber       { get; set; }
        public int?                          StatusId              { get; set; }
        public string                        StatusCode            { get; set; }
        public string                        StatusName            { get; set; }
        public List<int?>                    StatusReasonIds       { get; set; }
        public List<string>                  StatusReasonCodes     { get; set; }
        public List<string>                  StatusReasonNames     { get; set; }
        public DateTime?                     StatusLastUpdated     { get; set; }
        public DateTime?                     StatusDeadLine        { get; set; }
        public int?                          OrganizationId        { get; set; }
        public string                        OrganizationCode      { get; set; }
        public string                        OrganizationName      { get; set; }
        public string                        ApprovedPaymentAmount { get; set; }
        public EADemographicsContract        EaDemographics        { get; set; }
        public EAEmergencyTypeContract       EaEmergencyType       { get; set; }
        public EAGroupMembersContract        EaGroupMembers        { get; set; }
        public EAHouseHoldFinancialsContract EaHouseHoldFinancials { get; set; }
        public EAAgencySummaryContract       EaAgencySummary       { get; set; }
        public List<CommentContract>         EaComments            { get; set; }
        public List<EAPaymentContract>       EaPayments            { get; set; }
    }

    public class BaseEAContract
    {
        public int      RequestId                { get; set; }
        public string   ModifiedBy               { get; set; }
        public DateTime ModifiedDate             { get; set; }
        public bool     IsSubmittedViaDriverFlow { get; set; }
    }
}
