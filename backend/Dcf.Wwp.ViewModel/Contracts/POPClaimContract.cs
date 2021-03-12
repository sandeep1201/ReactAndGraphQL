using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class POPClaimContract
    {
        public int       Id                         { get; set; }
        public int       ParticipantId              { get; set; }
        public int       OrganizationId             { get; set; }
        public string    AgencyCode                 { get; set; }
        public string    AgencyName                 { get; set; }
        public decimal   PinNumber                  { get; set; }
        public string    ParticipantName            { get; set; }
        public string    FirstName                  { get; set; }
        public string    MiddleInitial              { get; set; }
        public string    LastName                   { get; set; }
        public string    SuffixName                 { get; set; }
        public DateTime? ClaimPeriodBeginDate       { get; set; }
        public DateTime? ClaimEffectiveDate         { get; set; }
        public int?      ClaimStatusTypeId          { get; set; }
        public int       POPClaimTypeId             { get; set; }
        public string    POPClaimTypeCode           { get; set; }
        public string    POPClaimTypeName           { get; set; }
        public string    ClaimStatusTypeCode        { get; set; }
        public string    ClaimStatusTypeName        { get; set; }
        public string    ClaimStatusTypeDisplayName { get; set; }
        public DateTime? ClaimStatusDate            { get; set; }
        public string    Details                    { get; set; }
        public bool      IsSubmit                   { get; set; }
        public bool      IsDeleted                  { get; set; }
        public bool      IsWithdraw                 { get; set; }
        public int?      ActivityId                 { get; set; }

        public string                           ActivityCode        { get;  set; }
        public DateTime?                        ActivityBeginDate   {  get; set; }
        public DateTime?                        ActivityEndDate     {  get; set; }
        public string                           ModifiedBy          { get;  set; }
        public DateTime                         ModifiedDate        { get;  set; }
        public List<POPClaimStatusContract>     POPClaimStatuses    { get;  set; }
        public List<POPClaimEmploymentContract> POPClaimEmployments { get;  set; }
    }
}
