using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class AuxiliaryContract
    {
        public int                           Id                             { get; set; }
        public int                           ParticipantId                  { get; set; }
        public decimal?                      PinNumber                      { get; set; }
        public string                        FirstName                      { get; set; }
        public string                        MiddleInitial                  { get; set; }
        public string                        LastName                       { get; set; }
        public string                        SuffixName                     { get; set; }
        public decimal?                      CaseNumber                     { get; set; }
        public short?                        CountyNumber                   { get; set; }
        public string                        CountyName                     { get; set; }
        public short?                        OfficeNumber                   { get; set; }
        public string                        OfficeName                     { get; set; }
        public string                        AgencyCode                     { get; set; }
        public string                        ProgramCd                      { get; set; }
        public string                        SubProgramCd                   { get; set; }
        public short?                        AGSequenceNumber               { get; set; }
        public int                           ParticipationPeriodId          { get; set; }
        public string                        ParticipationPeriodName        { get; set; }
        public short                         ParticipationPeriodYear        { get; set; }
        public decimal?                      OriginalPayment                { get; set; }
        public decimal?                      RecoupmentAmount               { get; set; }
        public decimal                       RequestedAmount                { get; set; }
        public int                           AuxiliaryReasonId              { get; set; }
        public string                        AuxiliaryReasonName            { get; set; }
        public int?                          AuxiliaryStatusTypeId          { get; set; }
        public string                        AuxiliaryStatusTypeCode        { get; set; }
        public string                        AuxiliaryStatusTypeName        { get; set; }
        public string                        AuxiliaryStatusTypeDisplayName { get; set; }
        public DateTime?                     AuxiliaryStatusDate            { get; set; }
        public string                        Details                        { get; set; }
        public bool                          IsSubmit                       { get; set; }
        public bool                          IsWithdraw                     { get; set; }
        public bool?                         IsAllowed                      { get; set; }
        public decimal?                      OverPayAmount                  { get; set; }
        public string                        ModifiedBy                     { get; set; }
        public DateTime                      ModifiedDate                   { get; set; }
        public string                        WIUID                          { get; set; }
        public string                        RequestedUserForApprovalAndDB2 { get; set; }
        public List<AuxiliaryStatusContract> AuxiliaryStatuses              { get; set; }
    }

    public class AuxiliaryStatusContract
    {
        public int      Id                         { get; set; }
        public int      AuxiliaryStatusTypeId      { get; set; }
        public string   AuxiliaryStatusName        { get; set; }
        public string   AuxiliaryStatusDisplayName { get; set; }
        public DateTime AuxiliaryStatusDate        { get; set; }
        public string   Details                    { get; set; }
        public string   ModifiedBy                 { get; set; }
        public DateTime ModifiedDate               { get; set; }
    }
}
