using System;
using System.Linq;
using Dcf.Wwp.Model.Interface;
using DCF.Timelimits.Rules.Domain;

namespace Dcf.Wwp.Api.Library.Contracts.Timelimits
{
    public class TimelineSummaryContract
    {
        public Int32? ParticipantId { get; set; }
        public Int32? FederalUsed { get; set; }
        public Int32? FederalMax { get; set; }
        public Int32? StateUsed { get; set; }
        public Int32? StateMax { get; set; }
        public Int32? CSJUsed { get; set; }
        public Int32? CSJMax { get; set; }
        public Int32? W2TUsed { get; set; }
        public Int32? W2TMax { get; set; }
        public Int32? TMPUsed { get; set; }
        public Int32? TNPUsed { get; set; }
        public Int32? TempUsed { get; set; }
        public Int32? TempMax { get; set; }
        public Int32? CMCUsed { get; set; }
        public Int32? CMCMax { get; set; }
        public Int32? OPCUsed { get; set; }
        public Int32? OPCMax { get; set; }
        public Int32? OtherUsed { get; set; }
        public Int32? OtherMax { get; set; }
        public Int32? OTF { get; set; }
        public Int32? Tribal { get; set; }
        public Int32? TJB { get; set; }
        public Int32? JOBS { get; set; }
        public Int32? NO24 { get; set; }
        public string FactDetails { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Boolean? CSJExtensionDue { get; set; }
        public Boolean? W2TExtensionDue { get; set; }
        public Boolean? TempExtensionDue { get; set; }
        public Boolean? StateExtensionDue { get; set; }


        public static TimelineSummaryContract Create(ITimeLimitSummary timelineSummary)
        {
            var newContract = new TimelineSummaryContract();
            newContract.ParticipantId = timelineSummary.ParticipantId;
            newContract.FederalUsed = timelineSummary.FederalUsed;
            newContract.FederalMax = timelineSummary.FederalMax;
            newContract.StateUsed = timelineSummary.StateUsed;
            newContract.StateMax = timelineSummary.StateMax;
            newContract.CSJUsed = timelineSummary.CSJUsed;
            newContract.CSJMax = timelineSummary.CSJMax;
            newContract.W2TUsed = timelineSummary.W2TUsed;
            newContract.W2TMax = timelineSummary.W2TMax;
            newContract.TMPUsed = timelineSummary.TMPUsed;
            newContract.TNPUsed = timelineSummary.TNPUsed;
            newContract.TempUsed = timelineSummary.TempUsed;
            newContract.TempMax = timelineSummary.TempMax;
            newContract.CMCUsed = timelineSummary.CMCUsed;
            newContract.CMCMax = timelineSummary.CMCMax;
            newContract.OPCUsed = timelineSummary.OPCUsed;
            newContract.OPCMax = timelineSummary.OPCMax;
            newContract.OtherUsed = timelineSummary.OtherUsed;
            newContract.OtherMax = timelineSummary.OtherMax;
            newContract.OTF = timelineSummary.OTF;
            newContract.Tribal = timelineSummary.Tribal;
            newContract.TJB = timelineSummary.TJB;
            newContract.JOBS = timelineSummary.JOBS;
            newContract.NO24 = timelineSummary.NO24;
            newContract.FactDetails = timelineSummary.FactDetails;
            newContract.IsDeleted = timelineSummary.IsDeleted;
            newContract.CreatedDate = timelineSummary.CreatedDate;
            newContract.ModifiedBy = timelineSummary.ModifiedBy;
            newContract.ModifiedDate = timelineSummary.ModifiedDate;
            newContract.CSJExtensionDue = timelineSummary.CSJExtensionDue;
            newContract.W2TExtensionDue = timelineSummary.W2TExtensionDue;
            newContract.TempExtensionDue = timelineSummary.TempExtensionDue;
            newContract.StateExtensionDue = timelineSummary.StateExtensionDue;
            return newContract;
        }
}
}