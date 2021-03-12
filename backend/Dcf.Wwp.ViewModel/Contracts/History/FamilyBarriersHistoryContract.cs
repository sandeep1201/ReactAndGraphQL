using System.Collections.Generic;


namespace Dcf.Wwp.Api.Library.Contracts.History
{
    class FamilyBarriersHistoryContract
    {
        public List<HistoryValueContract> HasEverAppliedSsi { get; set; }
        public List<HistoryValueContract> IsCurrentlyApplyingSsi { get; set; }
        public List<HistoryValueContract> SsiApplicationStatusId { get; set; }
        public List<HistoryValueContract> SsiApplicationStatusName { get; set; }
        public List<HistoryValueContract> SsiApplicationStatusDetails { get; set; }
        public List<HistoryValueContract> SsiApplicationDate { get; set; }
        public List<HistoryValueContract> SsiApplicationIsAnyoneHelping { get; set; }
        public List<HistoryValueContract> SsiApplicationDetails { get; set; }
        public List<HistoryValueContract> SsiApplicatioContactId { get; set; }
        public List<HistoryValueContract> HasReceivedPastSsi { get; set; }
        public List<HistoryValueContract> PastSsiDetails { get; set; }
        public List<HistoryValueContract> HasDeniedSsi { get; set; }
        public List<HistoryValueContract> DeniedSsiDate { get; set; }
        public List<HistoryValueContract> DeniedSsiDetails { get; set; }
        public List<HistoryValueContract> IsInterestedInLearningMoreSsi { get; set; }
        public List<HistoryValueContract> InterestedInLearningMoreSsiDetails { get; set; }
        public List<HistoryValueContract> HasAnyoneAppliedForSsi { get; set; }
        public List<HistoryValueContract> IsAnyoneReceivingSsi { get; set; }
        public List<HistoryValueContract> AnyoneReceivingSsiDetails { get; set; }
        public List<HistoryValueContract> IsAnyoneApplyingForSsi { get; set; }

    }
}
