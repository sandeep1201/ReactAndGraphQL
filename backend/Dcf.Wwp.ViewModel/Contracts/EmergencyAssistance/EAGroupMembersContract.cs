using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Api.Library.Contracts.EmergencyAssistance
{
    public class EAGroupMembersContract : BaseEAContract
    {
        public List<EARequestParticipantContract> EaGroupMembers          { get; set; }
        public bool?                              IsPreviousMemberClicked { get; set; }
    }

    public class EARequestParticipantContract
    {
        public int       Id                   { get; set; }
        public decimal?  PinNumber            { get; set; }
        public int?      ParticipantId        { get; set; }
        public int       EARequestId          { get; set; }
        public string    FirstName            { get; set; }
        public string    MiddleInitial        { get; set; }
        public string    LastName             { get; set; }
        public string    SuffixName           { get; set; }
        public DateTime? ParticipantDOB       { get; set; }
        public int?      EAIndividualTypeId   { get; set; }
        public string    EAIndividualTypeCode { get; set; }
        public string    EAIndividualTypeName { get; set; }
        public int?      EARelationTypeId     { get; set; }
        public string    EARelationTypeName   { get; set; }
        public bool?     IsIncluded           { get; set; }
        public decimal?  SSN                  { get; set; }
        public DateTime? SSNAppliedDate       { get; set; }
        public int?      SSNExemptTypeId      { get; set; }
        public string    SSNExemptTypeName    { get; set; }
        public string    ModifiedBy           { get; set; }
        public DateTime  ModifiedDate         { get; set; }
    }

    public class EAAGMembers
    {
        public decimal?  CaseNumber           { get; set; }
        public decimal?  PinNumber            { get; set; }
        public decimal?  OtherPersonPinNumber { get; set; }
        public string    OtherPersonFirstName { get; set; }
        public string    OtherPersonLastName  { get; set; }
        public DateTime? OtherPersonDOB       { get; set; }
        public string    Relationship         { get; set; }
        public int?      OtherPersonAge       { get; set; }
        public string    OtherPersonGender    { get; set; }
        public decimal?  SSN                  { get; set; }
        public int?      Ids                  { get; set; }
    }

    public class T0011
    {
        public decimal?  PinNumber            { get; set; }
        public decimal?  SSN                  { get; set; }
        public string    OtherPersonFirstName { get; set; }
        public string    OtherPersonLastName  { get; set; }
        public DateTime? OtherPersonDOB       { get; set; }
        public int?      OtherPersonAge       { get; set; }
        public string    OtherPersonGender    { get; set; }
    }
}
