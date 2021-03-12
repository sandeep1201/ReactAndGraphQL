using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class NonParticipationGoodCauseSummaryContract
    {
        public string TotalNonParticipationHours { get; set; }

        public string TotalGoodCauseHours { get; set; }

        public string RemainingNonSanctionableHours { get; set; }

        public string TotalSanctionableHours { get; set; }

        public List<NonParticipationGoodCauseContract> ParticipationTracking { get; set; }
    }

    public class NonParticipationGoodCauseContract : ParticipationTrackingContract
    {
        public bool? IsActivitySanctionable { get; set; }

        public bool? IsPlacementTypeSanctionable { get; set; }

        public bool? IsPendingOrOngoingAssessment { get; set; }

        public bool? IsNonParticipationSanctionable
        {
            get
            {
                return (IsActivitySanctionable == true && IsPlacementTypeSanctionable == true) ? true:false;
            }
        }

        public string SanctionableHours
        {
            get
            {
                if (IsNonParticipationSanctionable == false)
                {
                    return "0.0";
                }
                else
                {
                    // A temporary variable to store the result of your decimal
                    decimal td;

                    // Fixup your decimal as set it either to null or the appropriate decimal value
                    decimal? nonParticipatedHours = Decimal.TryParse(NonParticipatedHours, out td) ? td : (decimal?)null;
                    decimal? goodCausedHours = Decimal.TryParse(GoodCausedHours, out td) ? td : (decimal?)null;

                    return (nonParticipatedHours - goodCausedHours).ToString();
                }
            }
        }
    }


}
