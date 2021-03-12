using System;
using System.Collections.Generic;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class EAIPV : BaseEntity
    {
        #region Properties

        public int       ParticipantId     { get; set; }
        public int       OrganizationId    { get; set; }
        public DateTime  DeterminationDate { get; set; }
        public int       IPVNumber         { get; set; }
        public int       OccurrenceId      { get; set; }
        public int?       MailingAddressId  { get; set; }
        public int       StatusId          { get; set; }
        public DateTime  PenaltyStartDate  { get; set; }
        public DateTime? PenaltyEndDate    { get; set; }
        public string    Description       { get; set; }
        public string    Notes             { get; set; }
        public int?      CountyId          { get; set; }
        public bool      IsDeleted         { get; set; }
        public string    ModifiedBy        { get; set; }
        public DateTime  ModifiedDate      { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Participant                    Participant               { get; set; }
        public virtual Organization                   Organization              { get; set; }
        public virtual EAIPVOccurrence                Occurrence                { get; set; }
        public virtual EAIPVStatus                    Status                    { get; set; }
        public virtual EAAlternateMailingAddress      EaAlternateMailingAddress { get; set; }
        public virtual CountyAndTribe                 County                    { get; set; }
        public virtual ICollection<EAIPVReasonBridge> EaIpvReasonBridges        { get; set; } = new List<EAIPVReasonBridge>();

        #endregion
    }
}
