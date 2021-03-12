using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using DCF.Common.Configuration;
using DCF.Common.Dates;
using DCF.Common.Extensions;
using DCF.Core;
using EnumsNET;

namespace DCF.Timelimits.Rules.Domain
{
    public class Timeline : ITimeline
    {
        public virtual Int32? FederalMax { get; set; } = null;
        public virtual Int32? StateMax   { get; set; } = 60;
        public virtual Int32? StateMax48 { get; set; } = 48;
        public virtual Int32? CSJMax     { get; set; } = 24;
        public virtual Int32? W2TMax     { get; set; } = 24;
        public virtual Int32? TempMax    { get; set; } = 24;
        public virtual Int32? CMCMax     { get; set; } = null;
        public virtual Int32? OPCMax     { get; set; } = null;
        public virtual Int32? OtherMax   { get; set; } = null;

        private DateTime _timelineDate;

        public DateTime TimelineDate
        {
            get { return this._timelineDate; }
            set
            {
                // Flag as dirty if we set it to a different day
                if (!this._timelineDate.IsSame(value, DateTimeUnit.Day))
                {
                    this._isDirty = true;
                }

                this._timelineDate = value;
            }
        }

        public Timeline() : this(ApplicationContext.Current)
        {
        }

        public Timeline(ApplicationContext context)
        {
            this._timelineDate = context.Date;
            //this.Months.ItemChanged += Months_ItemChanged;
            //this.Extensions.ItemChanged += Extensions_ItemChanged;
            //this.Placements.ItemChanged += Placements_ItemChanged;
        }


        #region EventHandlers

        //private void Extensions_ItemChanged(DictionaryItemChangedEventArgs<DateTime, IEnumerable<Extension>> obj)
        //{
        //    this._isDirty = true;
        //}

        //private void Months_ItemChanged(DictionaryItemChangedEventArgs<DateTime, TimelineMonth> dictionaryItemChangedEventArgs)
        //{
        //    this._isDirty = true;
        //}

        //private void Placements_ItemChanged(DictionaryItemChangedEventArgs<DateTime, IEnumerable<Placement>> obj)
        //{
        //    // We don't care I don't think...
        //}

        #endregion

        private Dictionary<ClockTypes, ClockStates> _clockStates = new Dictionary<ClockTypes, ClockStates>();
        private Boolean                             _isDirty     = true;

        #region private readonly caches of calculated values

        private readonly ConcurrentDictionary<ClockTypes, IEnumerable<TimelineMonth>> _monthsMap          = new ConcurrentDictionary<ClockTypes, IEnumerable<TimelineMonth>>();
        private readonly ConcurrentDictionary<ClockTypes, IEnumerable<Extension>>     _extensionsMap      = new ConcurrentDictionary<ClockTypes, IEnumerable<Extension>>();
        private readonly ConcurrentDictionary<ClockTypes, Int32?>                     _clockMaxMonthsMap  = new ConcurrentDictionary<ClockTypes, Int32?>();
        private readonly ConcurrentDictionary<ClockTypes, Int32?>                     _clockUsedMonthsMap = new ConcurrentDictionary<ClockTypes, Int32?>();

        private readonly ConcurrentDictionary<ClockTypes, Int32?> _clockRemainingMonthsMap = new ConcurrentDictionary<ClockTypes, Int32?>();
        //private readonly ConcurrentDictionary<ClockTypes, IEnumerable<DateTime>> _extensionsMonthMap = new ConcurrentDictionary<ClockTypes, IEnumerable<DateTime>>();

        #endregion

        private List<ExtensionSequence> _extensionsSequence = new List<ExtensionSequence>();
        //private ApplicationContext _applicationContext;


        /// <summary>
        /// Current Timeline Months(s) lookup by DateTime (month)
        /// </summary>
        public virtual TimelineDictionary<TimelineMonth> Months { get; } = new TimelineDictionary<TimelineMonth>();

        /// <summary>
        /// Current Placements(s) lookup by DateTime (month)
        /// </summary>
        public virtual TimelineDictionary<List<Placement>> Placements { get; } = new TimelineDictionary<List<Placement>>();

        /// <summary>
        /// Calcuated ClockStatesMap for each Timelimit type
        /// </summary>
        Dictionary<ClockTypes, ClockStates> ITimeline.ClockStatesMap
        {
            get
            {
                this.ResetCachedData();
                return this._clockStates;
            }
        }

        public virtual void ResetCachedData(Boolean force = false)
        {
            if (this._isDirty || force)
            {
                this._clockStates?.Clear();
                this._monthsMap?.Clear();
                this._extensionsMap?.Clear();
                this._clockMaxMonthsMap?.Clear();
                this._clockUsedMonthsMap?.Clear();
                this._clockRemainingMonthsMap?.Clear();
                //this._extensionsMonthMap?.Clear();

                var flags = FlagEnums.GetAllFlags<ClockTypes>().GetFlags().ToList();
                flags.Add(ClockTypes.TRIBAL | ClockTypes.OTF);
                flags.Add(ClockTypes.Other);
                flags.Add(ClockTypes.TEMP);

                foreach (var clocktype in flags)
                {
                    this._extensionsMap.TryAdd(clocktype, this.GetExtensionsByClockType(clocktype));
                    this._monthsMap.TryAdd(clocktype, this.GetMonthsByClockTypes(clocktype));
                    this._clockMaxMonthsMap.TryAdd(clocktype, this.GetMaxMonthsByClockType(clocktype));
                    this._clockUsedMonthsMap.TryAdd(clocktype, this.GetUsedMonthsByClockTypes(clocktype));
                    this._clockRemainingMonthsMap.TryAdd(clocktype, this.GetRemainingMonthsByClockType(clocktype));
                    //this._extensionsMonthMap.TryAdd(clocktype, this.GetExtensionMonthsByClockType(clocktype));
                }

                this._clockStates = this.GetClockStates();
            }

            this._isDirty = false;
        }


        /// <summary>
        /// Get Timeline months that have Any of the flags specified
        /// </summary>
        /// <param name="flags">ClockType Flags to check</param>
        /// <returns></returns>
        IEnumerable<TimelineMonth> ITimeline.GetTimelineMonths(ClockTypes? flags = null)
        {
            this.ResetCachedData();
            IEnumerable<TimelineMonth> months;
            if (flags.HasValue)
            {
                this._monthsMap.TryGetValue(flags.Value, out months);
            }
            else
            {
                months = this._monthsMap.SelectMany(x => x.Value);
            }

            return months ?? new List<TimelineMonth>();
        }

        Int32? ITimeline.GetUsedMonths(ClockTypes flags)
        {
            this.ResetCachedData();
            Int32? used;
            this._clockUsedMonthsMap.TryGetValue(flags, out used);
            return used;
        }

        public string GetUsedMonthsCount(ClockTypes flag)
        {
            var   months                  = Months?.Where(x => x.Value.Date.IsSameOrBefore(_timelineDate, DateTimeUnit.Month)).Select(x => x.Value).ToList();
            bool? includeNoPlacementLimit = null;
            if (!flag.HasAnyFlags(ClockTypes.OtherTypes))
                includeNoPlacementLimit = flag.HasAnyFlags(ClockTypes.State | ClockTypes.Federal | ClockTypes.Other);
            else
                if (flag.HasAnyFlags(ClockTypes.NoPlacementLimit))
                    includeNoPlacementLimit = true;
            var filtered = months?.Where(x =>
                                         {
                                             if (!x.ClockTypes.HasFlag(flag) || (includeNoPlacementLimit == false && x.ClockTypes.HasAnyFlags(ClockTypes.NoPlacementLimit))) return false;
                                             if (x.ClockTypes.HasAnyFlags(ClockTypes.CMC) && (((x.ClockTypes & ClockTypes.State) != 0 || (x.ClockTypes & ClockTypes.Federal) != 0)))
                                             {
                                                 return true;
                                             }

                                             return !x.ClockTypes.HasAnyFlags(ClockTypes.CMC);
                                         }).ToList();

            if (filtered == null || filtered.Count == 0) return "0";

            var used = filtered.Count.ToString();
            return used;
        }

        public virtual IReadOnlyList<Extension> GetExtensions(ClockTypes flags)
        {
            this.ResetCachedData();
            return this._extensionsMap.GetOrAdd(flags, this.GetExtensionsByClockType).ToList().AsReadOnly();
        }

        // TODO: Unit Test this!
        Int32? ITimeline.GetMaxMonths(ClockTypes flags)
        {
            this.ResetCachedData();
            Int32? max;
            this._clockMaxMonthsMap.TryGetValue(flags, out max);
            return max;
        }
        //List<DateTime> ITimeline.GetExtensionMonths(ClockTypes flags)
        //{
        //    this.ResetCachedData();
        //    IEnumerable<DateTime> extensionMonths;
        //    this._extensionsMonthMap.TryGetValue(flags, out extensionMonths);
        //    return (extensionMonths ?? new List<DateTime>()).ToList();
        //}

        Int32? ITimeline.GetRemainingMonths(ClockTypes flags)
        {
            this.ResetCachedData();
            Int32? remaining;
            this._clockRemainingMonthsMap.TryGetValue(flags, out remaining);
            return remaining;
        }

        public virtual Int32? GetClockMax(ClockTypes flags)
        {
            switch (flags)
            {
                case ClockTypes.State:
                    return ApplicationContext.IsInTransitionPeriod ? this.StateMax : this.StateMax48;
                case ClockTypes.CSJ:
                    return this.CSJMax;
                case ClockTypes.W2T:
                    return this.W2TMax;
                case ClockTypes.TMP:
                case ClockTypes.TNP:
                case ClockTypes.TEMP:
                    return this.TempMax;
                case ClockTypes.Federal:
                    return this.FederalMax;
                case ClockTypes.CMC:
                    return this.CMCMax;
                case ClockTypes.OPC:
                    return this.OPCMax;
                case ClockTypes.Other:
                    return this.OtherMax;
                default:
                    return null;
            }
        }

        private Int32? GetMaxMonthsByClockType(ClockTypes flags)
        {
            Int32? max      = this.GetClockMax(flags);
            Int32? clockMax = max;

            if (!max.HasValue)
            {
                return null;
            }

            var usedMonths = this.GetUsedMonthsByClockTypes(flags).GetValueOrDefault(0);
            max = usedMonths;

            var stateExtension = this.GetExtensionsByClockType(ClockTypes.State).Where(x => x.DateRange.HasValue).ToList();
            ;

            var clockExtensions = this.GetExtensionsByClockType(flags).Where(x => x.DateRange.HasValue).ToList();
            ;

            var extensions = new List<Extension>();
            extensions.AddRange(stateExtension);
            extensions.AddRange(clockExtensions);

            if (extensions.Any())
            {
                var maxEndDate = extensions.Select(x => x.DateRange.Value.End).GetMax(x => x);
                // Must use current date for adding extension months to MAX, not timeline date (it'll add too many)
                if (ApplicationContext.Current.Date.IsSameOrBefore(maxEndDate, DateTimeUnit.Month))
                {
                    var dateRange = new DateTimeRange(ApplicationContext.Current.Date, maxEndDate);

                    foreach (var month in dateRange.By(DateTimeUnits.Months))
                    {
                        if (max < clockMax)
                        {
                            max++;
                        }
                        else
                        {
                            TimelineMonth tMonth;
                            var           usedMonth               = this.Months.TryGetValue(month, out tMonth);
                            var           hasOverlappingExtension = extensions.Any(x => x.DateRange.Value.End.IsSameOrAfter(month, DateTimeUnit.Month));
                            if (hasOverlappingExtension && !usedMonth)
                            {
                                max++;
                            }
                        }
                    }
                }
            }

            if (max < clockMax)
            {
                max = clockMax;
            }


            return max;
        }

        public int? GetMaxMonthsByClock(ClockTypes flag, string usedMonths)
        {
            var max      = GetClockMax(flag);
            var clockMax = max;

            if (!max.HasValue)
            {
                return null;
            }

            var stateExtension = _extensionsSequence.Select(s =>
                                                                s.GetAllExtensions()
                                                                 .Where(i => i != null && i.ClockType.HasAnyFlags(ClockTypes.State) && i.ExtensionDecision.HasAnyFlags(ExtensionDecision.Approve)
                                                                             && !i.HasElapsed)
                                                                 .GetMax(i => i.DecisionDate)).ToList();

            var clockExtensions = _extensionsSequence.Select(s =>
                                                                 s.GetAllExtensions()
                                                                  .Where(i => i != null && i.ClockType.HasAnyFlags(flag) && i.ExtensionDecision.HasAnyFlags(ExtensionDecision.Approve)
                                                                              && !i.HasElapsed)
                                                                  .GetMax(i => i.DecisionDate)).ToList();

            var extensions = new List<Extension>();
            extensions.AddRange(stateExtension);
            extensions.AddRange(clockExtensions);

            max = int.Parse(usedMonths);

            if (extensions.Any(i => i != null))
            {
                var maxEndDate      = extensions.Select(i => i.DateRange.Value.End).GetMax(i => i);
                var dateRange       = new DateTimeRange(ApplicationContext.Current.Date, maxEndDate);
                var dateRangeMonths = dateRange.By(DateTimeUnits.Months).ToArray();

                foreach (var month in dateRangeMonths)
                {
                    if (max < clockMax)
                    {
                        max++;
                    }
                    else
                    {
                        var hasOverlappingExtension = extensions.Any(x => x.DateRange.Value.End.IsSameOrAfter(month, DateTimeUnit.Month));
                        if (hasOverlappingExtension)
                        {
                            max++;
                        }
                    }
                }
            }

            if (max < clockMax)
                max = clockMax;

            return max;
        }

        public string GetMaxMonthsCount(int stateUsed, int? stateClockMax, int usedMonths, int? maxMonths, ClockTypes flag)
        {
            var    clockMax = GetClockMax(flag);
            string maxClockMonths;

            if (flag == ClockTypes.PlacementLimit && stateUsed >= stateClockMax)
                maxClockMonths = "-";
            else
                if (flag == ClockTypes.PlacementLimit && usedMonths >= clockMax)
                    maxClockMonths = "-";
                else
                    if (flag == ClockTypes.State)
                        maxClockMonths = stateUsed > stateClockMax ? "-" : maxMonths.ToString();
                    else
                        maxClockMonths = (clockMax == null || usedMonths >= clockMax) ? "-" : maxMonths.ToString();

            return maxClockMonths;
        }

        public int? GetMaxMonthsByClockType(ITimeline timeLine, ClockTypes flags)
        {
            var max      = GetClockMax(flags);
            var clockMax = GetClockMax(flags);

            if (!max.HasValue)
            {
                return null;
            }

            var usedMonths = int.Parse(GetUsedMonthsCount(flags));
            max = usedMonths;

            var isCurrentMonthTicked = timeLine.GetTimelineMonths(flags).OrderByDescending(i => i.Date).FirstOrDefault()?.IsCurrentMonth ?? false;

            var stateExtension  = GetExtensionsByClockType(ClockTypes.State).Where(x => x.DateRange.HasValue).ToList();
            var clockExtensions = GetExtensionsByClockType(flags).Where(x => x.DateRange.HasValue).ToList();

            var extensions = new List<Extension>();
            extensions.AddRange(stateExtension);
            extensions.AddRange(clockExtensions);

            if (extensions.Any())
            {
                var maxEndDate = extensions.Where(i => i.DateRange.HasValue).Select(i => i.DateRange.Value.End).GetMax(x => x);
                var dateRange  = new DateTimeRange(ApplicationContext.Current.Date, maxEndDate);

                foreach (var month in dateRange.By(DateTimeUnits.Months))
                {
                    if (max < clockMax)
                    {
                        max++;
                    }
                    else
                    {
                        var hasOverlappingExtension = extensions.Any(i => i.DateRange.HasValue && i.DateRange.Value.End.IsSameOrAfter(month, DateTimeUnit.Month));
                        if (hasOverlappingExtension)
                            max++;
                    }
                }
            }

            if (isCurrentMonthTicked)
                max--;

            if (max < clockMax)
            {
                max = clockMax;
            }


            return max;
        }

        private List<DateTime> GetExtensionMonthsByClockType(ClockTypes flags)
        {
            var extensionMonths = new List<DateTime>();

            var clockExtensions      = this.GetExtensionsByClockType(flags).ToList();
            var stateExtensions      = this.GetExtensionsByClockType(ClockTypes.State).ToList();
            var stateExtensionMonths = stateExtensions.Where(x => x.DateRange.HasValue).SelectMany(x => x.DateRange.Value.By(DateTimeUnits.Months).Select(y => y.Date)).ToList();
            var clockExtensionMonths = flags == ClockTypes.State ? new List<DateTime>() : clockExtensions.Where(x => x.DateRange.HasValue).SelectMany(x => x.DateRange.Value.By(DateTimeUnits.Months).Select(y => y.Date)).ToList();
            if (stateExtensionMonths.Any() || clockExtensionMonths.Any())
            {
                // TODO: filter out extension if clock will close before starting extension

                // Create DateRange from now till the end of the last applicable extension
                var endDate   = stateExtensionMonths.Any() ? stateExtensionMonths.Max() : clockExtensionMonths.Max();
                var dateRange = new DateTimeRange(this.TimelineDate, endDate);
                foreach (var extMonth in dateRange.By(DateTimeUnits.Months))
                {
                    if (stateExtensionMonths.Any(x => x.IsSame(extMonth, DateTimeUnit.Month)) || clockExtensionMonths.Any(x => x.IsSame(extMonth, DateTimeUnit.Month)))
                    {
                        extensionMonths.Add(extMonth);
                    }
                }
            }

            return extensionMonths;
        }

        private List<Extension> GetExtensionsByClockType(ClockTypes flags)
        {
            return this._extensionsSequence.Select(s =>
                                                       s.GetAllExtensions()
                                                        .Where(x => !x.HasElapsed)
                                                        .GetMax(x => x.DecisionDate)
                                                  ).Where(x => x != null && x.ClockType.HasAnyFlags(flags)).ToList();
        }


        private Int32? GetUsedMonthsByClockTypes(ClockTypes clocktype)
        {
            return this.GetMonthsByClockTypes(clocktype)?.Count();
        }

        private IReadOnlyList<TimelineMonth> GetMonthsByClockTypes(ClockTypes clocktype)
        {
            var months = this.Months?.Where(x => x.Value.Date.IsSameOrBefore(this._timelineDate, DateTimeUnit.Month)).Select(x => x.Value).ToList();
            var filtered = months.Where(x =>
                                        {
                                            var monthClockTypes = x.ClockTypes;
                                            if (clocktype.HasAnyFlags(ClockTypes.PlacementLimit | ClockTypes.TJB) && monthClockTypes.HasAnyFlags(ClockTypes.NoPlacementLimit))
                                            {
                                                return false;
                                            }
                                            else
                                            {
                                                return monthClockTypes.HasAnyFlags(clocktype);
                                            }
                                        });
            return filtered.ToList().AsReadOnly();
        }

        private Int32? GetRemainingMonthsByClockType(ClockTypes flags)
        {
            var maxMonths  = this.GetMaxMonthsByClockType(flags);
            var usedMonths = this.GetUsedMonthsByClockTypes(flags);
            var remaining  = maxMonths == null ? -1 : maxMonths - usedMonths;
            return remaining < 0 ? null : remaining;
        }

        public string GetRemainingMonthsCount(int usedMonths, int stateUsed, int? maxMonths, int? stateClockMax, ClockTypes flag)
        {
            var remaining = maxMonths == null ? -1 : maxMonths - usedMonths;

            var    clockMax = GetClockMax(flag);
            string remainingClockMonths;

            if (flag == ClockTypes.PlacementLimit && stateUsed >= stateClockMax)
                remainingClockMonths = "-";
            else
                if (flag == ClockTypes.PlacementLimit && usedMonths >= clockMax)
                    remainingClockMonths = remaining < 0 ? "-" : remaining.ToString();
                else
                    if (flag == ClockTypes.State)
                        remainingClockMonths = remaining < 0 ? "-" : remaining.ToString();
                    else
                        remainingClockMonths = remaining < 0 ? "-" : remaining.ToString();

            return remainingClockMonths;
        }

        private Dictionary<ClockTypes, ClockStates> GetClockStates()
        {
            var clockStatesMap = new Dictionary<ClockTypes, ClockStates>();
            var stateExtension = this._extensionsMap.GetOrAdd(ClockTypes.State, this.GetExtensionsByClockType).GetMax(x => x.DecisionDate); //this.GetExtensions(ClockTypes.State).GetMax(x => x.DecisionDate);
            var stateDenied    = stateExtension?.ExtensionDecision == ExtensionDecision.Deny;
            var stateApproved  = stateExtension?.ExtensionDecision == ExtensionDecision.Approve;
            var stateClockMax  = this.GetClockMax(ClockTypes.State);
            var stateUsed      = this._monthsMap.GetOrAdd(ClockTypes.State, this.GetMonthsByClockTypes).Count(); //this.GetUsedMonthsByClockTypes(ClockTypes.State);
            var stateRemaining = (stateClockMax - stateUsed > 0) ? stateClockMax - stateUsed : 0;
            var isOverStateMax = stateRemaining < 1;

            foreach (var clockType in ClockTypes.ExtensableTypes.GetFlags())
            {
                var clockTypeMax = this.GetClockMax(clockType);
                var clockState   = ClockStates.None;

                if (clockTypeMax.HasValue)
                {
                    var clockExtension = clockType == ClockTypes.State ? stateExtension : this._extensionsMap.GetOrAdd(clockType, this.GetExtensionsByClockType).GetMax(x => x.DecisionDate);
                    var clockApproved  = clockExtension?.ExtensionDecision == ExtensionDecision.Approve;

                    var remainingMonths = this.GetRemainingMonthsByClockType(clockType); //TODO: Add a cache for this calculation

                    var isWarn   = false;
                    var isDanger = false;
                    //var canBeExtended = false;
                    //var canBeDenied = false;
                    var outOfTicks = remainingMonths < 1;

                    if (clockType.HasAnyFlags(ClockTypes.State))
                    {
                        isWarn   = (!stateApproved && remainingMonths <= (ApplicationContext.IsInTransitionPeriod ? 18 : 6)) || (stateApproved && remainingMonths <= 3);
                        isDanger = (!stateApproved && remainingMonths <= 4) || (stateApproved && remainingMonths <= 1);
                        //canBeExtended = isDanger || isWarn || stateDenied;
                        //canBeDenied = stateExtension != null || canBeExtended;
                    }
                    else
                    {
                        isWarn   = ((!clockApproved && remainingMonths <= 6) || (clockApproved && remainingMonths <= 3)) && !isOverStateMax;
                        isDanger = ((!clockApproved && remainingMonths <= 4) || (clockApproved && remainingMonths <= 1)) && !isOverStateMax;

                        if (stateRemaining <= (ApplicationContext.IsInTransitionPeriod ? 18 : 6))
                        {
                            //canBeExtended = false;
                            isWarn   = isWarn   && !stateApproved;
                            isDanger = isDanger && !stateApproved;
                            //canBeDenied = (isWarn || isDanger) || clockExtension != null;
                        }

                        //else
                        //{
                        //canBeExtended = isWarn || isDanger;
                        //canBeDenied = (isWarn || isDanger) || clockExtension != null;
                        //}
                    }

                    clockState = isDanger ? clockState.CombineFlags(ClockStates.Danger) : clockState;
                    clockState = isWarn ? clockState.CombineFlags(ClockStates.Warn) : clockState;
                    clockState = outOfTicks ? clockState.CombineFlags(ClockStates.OutOfTicks) : clockState;
                }

                clockStatesMap.Add(clockType, clockState);
            }

            return clockStatesMap;
        }

        private Dictionary<ClockTypes, ClockStates> GetClockStatesOld()
        {
            var clockStatesMap  = new Dictionary<ClockTypes, ClockStates>();
            var stateUsedMonths = this.GetUsedMonthsByClockTypes(ClockTypes.State);
            //var stateClockMax = this.GetClockMax(ClockTypes.State);
            var stateExtension = this.GetExtensionsByClockType(ClockTypes.State).GetMax(x => x.DecisionDate);


            foreach (var clockType in ClockTypes.ExtensableTypes.GetFlags())
            {
                var clockState = ClockStates.None;
                if (clockType.Equals(ClockTypes.TMP) || clockType.Equals(ClockTypes.TNP))
                {
                    continue;
                }

                var isWarn                    = false;
                var isDanger                  = false;
                var isOutOfTicks              = false;
                var isInExtension             = false;
                var isInStateExtensionGap     = false;
                var isCoveredByStateExtension = false;

                var clockExtension  = this.GetExtensionsByClockType(clockType).GetMax(x => x.DecisionDate);
                var clockMax        = this.GetClockMax(clockType);
                var remainingMonths = this.GetRemainingMonthsByClockType(clockType);

                if (!clockType.Equals(ClockTypes.State) && stateExtension?.ExtensionDecision == ExtensionDecision.Approve)
                {
                    // If Remaining months will put state clock within 6 months of State clock max
                    isInStateExtensionGap     = clockExtension?.ExtensionDecision == ExtensionDecision.Approve && stateUsedMonths + remainingMonths <= clockMax && stateUsedMonths + remainingMonths >= clockMax - 6;
                    isCoveredByStateExtension = isInStateExtensionGap || stateUsedMonths                                          + remainingMonths > clockMax;
                }

                isWarn        = (!isInStateExtensionGap && !isCoveredByStateExtension) && ((clockExtension == null && remainingMonths <= 7) || (clockExtension?.ExtensionDecision != ExtensionDecision.Deny && remainingMonths <= 3));
                isDanger      = (isWarn && ((clockExtension == null && remainingMonths <= 4) || (clockExtension?.ExtensionDecision != ExtensionDecision.Deny && remainingMonths <= 1)));
                isOutOfTicks  = remainingMonths < 1;
                isInExtension = clockExtension != null && clockExtension.HasStarted && !clockExtension.HasElapsed;

                // Add Flags
                if (isWarn) clockState.CombineFlags(ClockStates.Warn);
                if (isDanger) clockState.CombineFlags(ClockStates.Danger);
                if (isOutOfTicks) clockState.CombineFlags(ClockStates.OutOfTicks);
                if (isInExtension) clockState.CombineFlags(ClockStates.InExtension);
                if (isInStateExtensionGap) clockState.CombineFlags(ClockStates.InStateExtensionGap);
                if (isCoveredByStateExtension) clockState.CombineFlags(ClockStates.CoveredByStateExtension);

                clockStatesMap.Add(clockType, clockState);
            }

            return clockStatesMap;
        }

        public virtual LinkedList<Placement> GetPlacementsLinkedList()
        {
            var sortedPlacements = this.Placements.SelectMany(x => x.Value).Distinct().ToList();
            sortedPlacements.InsertionSort(x => x.DateRange.Start);
            var linkedList = new LinkedList<Placement>();
            foreach (var placement in sortedPlacements)
            {
                linkedList.AddLast(placement);
            }

            return linkedList;
        }

        public void AddPlacement(Placement placement)
        {
            this.AddPlacements(new List<Placement> { placement });
        }

        public void AddPlacements(IEnumerable<Placement> placements)
        {
            foreach (var placement in placements)
            {
                DateTimeRange monthRange = placement.IsOpen ? new DateTimeRange(placement.DateRange.Start, this._timelineDate.Date.EndOf(DateTimeUnit.Month)) : placement.DateRange;

                var months = monthRange.By(DateTimeUnits.Months).ToList();

                foreach (var month in months)
                {
                    this.Placements.AddOrUpdate(month, new List<Placement> { placement }, (k, v) =>
                                                                                          {
                                                                                              v.Add(placement);
                                                                                              return v;
                                                                                          });
                }
            }

            this._isDirty = true;
        }

        public void AddExtensionSequences(IEnumerable<ExtensionSequence> sequences)
        {
            var extensionSequences = sequences as ExtensionSequence[] ?? sequences.ToArray();
            this._extensionsSequence.AddRange(extensionSequences);

            foreach (var extSequence in extensionSequences)
            {
                var extension = extSequence.CurrentExtension;
                if (extension != null)
                {
                    this._extensionsMap.AddOrUpdate(extSequence.ClockType, new List<Extension> { extension }, (k, v) =>
                                                                                                              {
                                                                                                                  v.ToList().Add(extension);
                                                                                                                  return v;
                                                                                                              });
                }
            }

            this._isDirty = true;
        }

        public void AddTimelineMonth(TimelineMonth month)
        {
            this.Months.AddOrUpdate(month.Date, month, (k, v) => month);
            this._isDirty = true;
        }

        public void AddTimelineMonths(IEnumerable<TimelineMonth> timelineMonths)
        {
            foreach (var month in timelineMonths)
            {
                this.AddTimelineMonth(month);
            }
        }

        //TODO: Add unit tests for this, so we dont' have to guess/Test that the rules engine is getting the right value
        public Placement LastEmploymentPlacement(DateTime month)
        {
            List<Placement> monthPlacements;
            this.Placements.TryGetValue(month, out monthPlacements);
            var placements = monthPlacements?.Where(x => x.PlacementType.GetValueOrDefault().HasAnyFlags(ClockTypes.PlacementTypes));

            var p = placements?.GetMax(x => x.DateRange.End);
            return p;
        }

        public Placement FirstEmploymentPlacement(Boolean includeCmc = false)
        {
            var placementFilter = includeCmc ? ClockTypes.PlacementTypes : ClockTypes.PlacementTypes.RemoveFlags(ClockTypes.CMC);
            var monthPlacements = this.Placements.SelectMany(x => x.Value).Distinct().ToList();
            var placements      = monthPlacements?.Where(x => x.PlacementType.GetValueOrDefault().HasAnyFlags(placementFilter));

            var p = placements?.GetMin(x => x.DateRange.End);
            return p;
        }

        public Placement GetPreviousPlacement(Placement placement, Boolean allowGap)
        {
            var current           = this.GetPlacementsLinkedList().Find(placement);
            var previousPlacement = new Placement(ClockTypes.None, DateTime.Now, DateTime.MaxValue);
            while (previousPlacement.PlacementType.GetValueOrDefault() == ClockTypes.None)
            {
                // If we don't have a current placement or previouse placement get out
                if (current?.Value == null || current?.Previous?.Value == null)
                {
                    break;
                }

                var currentClockType  = current.Value.PlacementType.GetValueOrDefault();
                var previousClockType = current.Previous.Value.PlacementType.GetValueOrDefault();

                // Same as no placement
                if (currentClockType == ClockTypes.None || previousClockType == ClockTypes.None)
                {
                    break;
                }

                var previous = current.Previous;


                // if they aren't adjacent by day, then doesn't count as previous. Break Out
                if (!allowGap && !previous.Value.DateRange.Adjacent(current.Value.DateRange, DateTimeUnits.Days))
                {
                    break;
                }

                //If seperate placements, but we moved into the same placement, keep looking
                if (currentClockType == previousClockType)
                {
                    current = previous;
                    continue;
                }

                // gap Allowed || or no gap and we found a different previouse placement, so use previous placement
                previousPlacement = previous.Value;
            }

            return previousPlacement;
        }

        public void AddExtension(Int32? sequenceId, Extension extension)
        {
            var extensionSequence = this._extensionsSequence.FirstOrDefault(x => x.ClockType == extension.ClockType && x.SequenceId == sequenceId.GetValueOrDefault());
            if (extensionSequence == null)
            {
                extensionSequence = new ExtensionSequence(1, new List<Extension>());
                this._extensionsSequence.Add(extensionSequence);
            }

            extensionSequence.Extensions.Add(extension);
            this._isDirty = true;
        }

        public void RemoveExtension(Int32 sequenceId, Extension extension)
        {
            var extensionSequence = this._extensionsSequence.FirstOrDefault(x => x.ClockType == extension.ClockType && x.SequenceId == sequenceId);
            if (extensionSequence != null)
            {
                var matchingExt = extensionSequence.Extensions.FirstOrDefault(x =>
                                                                                  x.ClockType == extension.ClockType && x.DecisionDate.IsSame(extension.DecisionDate, DateTimeUnit.Minute) && x.ExtensionDecision == extension.ExtensionDecision);

                extensionSequence.Extensions.Remove(matchingExt);
            }

            this._isDirty = true;
        }
    }
}
