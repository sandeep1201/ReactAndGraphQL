using System.Collections.Generic;


namespace Dcf.Wwp.Api.Library.Contracts.History
{
    class LegalIssuesHistoryContract
    {
        public List<HistoryValueContract> IsConvictedOfCrime { get; set; }
        public List<CriminalChargeHistoryContract> Convictions { get; set; }
        public List<HistoryValueContract> IsUnderCommunitySupervision { get; set; }
        public List<HistoryValueContract> CommunitySupervisionDetails { get; set; }
        public List<HistoryValueContract> IsPending { get; set; }
        public List<PendingChargeHistoryContract> Pendings { get; set; }
        public List<HistoryValueContract> HasFamilyLegalIssues { get; set; }
        public List<HistoryValueContract> FamilyLegalIssueNotes { get; set; } 
        public List<HistoryValueContract> HasUpcomingCourtDates { get; set; }
        public List<CourtDateHistoryContract> CourtDates { get; set; }
        public List<HistoryValueContract> Notes { get; set; }
        public List<HistoryValueContract> CommunitySupervisionContactId { get; set; }

        public LegalIssuesHistoryContract()
        {
            IsConvictedOfCrime = new List<HistoryValueContract>();
            Convictions = new List<CriminalChargeHistoryContract>();
            IsUnderCommunitySupervision = new List<HistoryValueContract>();
            CommunitySupervisionDetails = new List<HistoryValueContract>();
            IsPending = new List<HistoryValueContract>();
            Pendings = new List<PendingChargeHistoryContract>();
            HasFamilyLegalIssues = new List<HistoryValueContract>();
            FamilyLegalIssueNotes = new List<HistoryValueContract>();
            HasUpcomingCourtDates = new List<HistoryValueContract>();
            CourtDates = new List<CourtDateHistoryContract>();
            Notes = new List<HistoryValueContract>();
            CommunitySupervisionContactId = new List<HistoryValueContract>();
        }
    }

    public class CriminalChargeHistoryContract
    {
        public List<HistoryValueContract> TypeId { get; set; }
        public List<HistoryValueContract> Date { get; set; }
        public List<HistoryValueContract> IsDateUnknown { get; set; }
        public List<HistoryValueContract> Details { get; set; }

        public CriminalChargeHistoryContract()
        {
            TypeId = new List<HistoryValueContract>();
            Date = new List<HistoryValueContract>();
            IsDateUnknown = new List<HistoryValueContract>();
            Details = new List<HistoryValueContract>();
        }
    }

    public class CourtDateHistoryContract
    {
        public List<HistoryValueContract> Location { get; set; }
        public List<HistoryValueContract> Date { get; set; }
        public List<HistoryValueContract> IsDateUnknown { get; set; }
        public List<HistoryValueContract> Details { get; set; }

        public CourtDateHistoryContract()
        {
            Location = new List<HistoryValueContract>();
            Date = new List<HistoryValueContract>();
            IsDateUnknown = new List<HistoryValueContract>();
            Details = new List<HistoryValueContract>();
        }
    }

    public class PendingChargeHistoryContract
    {
        public List<HistoryValueContract> TypeId { get; set; }
        public List<HistoryValueContract> Date { get; set; }
        public List<HistoryValueContract> IsDateUnknown { get; set; }
        public List<HistoryValueContract> Details { get; set; }

        public PendingChargeHistoryContract()
        {
            TypeId = new List<HistoryValueContract>();
            Date = new List<HistoryValueContract>();
            IsDateUnknown = new List<HistoryValueContract>();
            Details = new List<HistoryValueContract>();
        }
    }
}
