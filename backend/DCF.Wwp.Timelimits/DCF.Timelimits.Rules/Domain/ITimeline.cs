using System;
using System.Collections.Generic;

namespace DCF.Timelimits.Rules.Domain
{
    public interface ITimeline
    {
        /// <summary>
        /// Calcuated ClockStatesMap for each Timelimit type
        /// </summary>
        Dictionary<ClockTypes, ClockStates> ClockStatesMap { get; }

        Int32? FederalMax { get; set; }
        Int32? StateMax   { get; set; }
        Int32? StateMax48 { get; set; }
        Int32? CSJMax     { get; set; }
        Int32? W2TMax     { get; set; }
        Int32? TempMax    { get; set; }
        Int32? CMCMax     { get; set; }
        Int32? OPCMax     { get; set; }
        Int32? OtherMax   { get; set; }

        DateTime TimelineDate { get; set; }

        /// <summary>
        /// Current Timeline Months(s) lookup by DateTime (month)
        /// </summary>
        TimelineDictionary<TimelineMonth> Months { get; }

        /// <summary>
        /// Current Placements(s) lookup by DateTime (month)
        /// </summary>
        TimelineDictionary<List<Placement>> Placements { get; }

        /// <summary>
        /// Get Timeline months that have Any of the flags specified
        /// </summary>
        /// <param name="flags">ClockType Flags to check</param>
        /// <returns></returns>
        IEnumerable<TimelineMonth> GetTimelineMonths(ClockTypes? flags = null);

        Int32?                   GetMaxMonths(ClockTypes           flags);
        int?                     GetMaxMonthsByClock(ClockTypes    flag,      string     usedMonths);
        string                   GetMaxMonthsCount(int             stateUsed, int?       stateClockMax, int usedMonths, int? maxMonths, ClockTypes flag);
        int?                     GetMaxMonthsByClockType(ITimeline timeLine,  ClockTypes flags);
        Int32?                   GetRemainingMonths(ClockTypes     flags);
        string                   GetRemainingMonthsCount(int       usedMonths, int stateUsed, int? maxMonths, int? stateClockMax, ClockTypes flag);
        Int32?                   GetUsedMonths(ClockTypes          flags);
        string                   GetUsedMonthsCount(ClockTypes     flag);
        void                     ResetCachedData(Boolean           force = false);
        IReadOnlyList<Extension> GetExtensions(ClockTypes          flags);
        Int32?                   GetClockMax(ClockTypes            flags);
        LinkedList<Placement>    GetPlacementsLinkedList();
        void                     AddPlacement(Placement                               placement);
        void                     AddPlacements(IEnumerable<Placement>                 placements);
        void                     AddExtensionSequences(IEnumerable<ExtensionSequence> sequences);
        void                     AddTimelineMonth(TimelineMonth                       month);
        void                     AddTimelineMonths(IEnumerable<TimelineMonth>         timelineMonths);
        Placement                LastEmploymentPlacement(DateTime                     month);
        Placement                FirstEmploymentPlacement(Boolean                     includeCmc = false);
        Placement                GetPreviousPlacement(Placement                       placement,  Boolean   allowGap);
        void                     AddExtension(Int32?                                  sequenceId, Extension extension);
        void                     RemoveExtension(Int32                                sequenceId, Extension extension);
    }
}
