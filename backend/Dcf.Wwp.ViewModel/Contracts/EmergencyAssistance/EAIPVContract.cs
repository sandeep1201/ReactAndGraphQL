using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Api.Library.Contracts.EmergencyAssistance
{
    public class EAIPVContract
    {
        public int                     Id                { get; set; }
        public int                     ParticipantId     { get; set; }
        public string                  OrganizationCode  { get; set; }
        public DateTime                DeterminationDate { get; set; }
        public int                     IPVNumber         { get; set; }
        public List<int>               ReasonIds         { get; set; }
        public List<string>            ReasonCodes       { get; set; }
        public List<string>            ReasonNames       { get; set; }
        public int                     OccurrenceId      { get; set; }
        public string                  OccurrenceCode    { get; set; }
        public string                  OccurrenceName    { get; set; }
        public FinalistAddressContract MailingAddress    { get; set; }
        public int                     StatusId          { get; set; }
        public string                  StatusCode        { get; set; }
        public string                  StatusName        { get; set; }
        public DateTime                PenaltyStartDate  { get; set; }
        public DateTime?               PenaltyEndDate    { get; set; }
        public string                  Description       { get; set; }
        public string                  Notes             { get; set; }
        public int?                    CountyId          { get; set; }
        public string                  CountyName        { get; set; }
        public bool                    IsDeleted         { get; set; }
        public string                  ModifiedBy        { get; set; }
        public DateTime                ModifiedDate      { get; set; }
        public DateTime?               OverTurnedDate    { get; set; }
    }
}
