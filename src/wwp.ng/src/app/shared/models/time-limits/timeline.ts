import { Utilities } from './../../utilities';
import { state } from '@angular/animations';
import { contains } from '../../arrays';
import { Dictionary } from '../../dictionary';

import * as moment from 'moment';
import { range, DateRange, rangeInterval, within } from '../../moment-range';
import { ValidationResult, ValidationManager, ValidationError } from '../../models/validation';
import * as validation from 'lakmus';
import { TimelineMonth } from './timeline-month';
import { Extension, ExtensionDecision, ExtensionSequence } from './extension';
import { Tick } from './tick';
import { ClockTypes, ClockType } from './clocktypes';
import { ClockStates, ClockState } from './clockstates';
import { EnumEx } from '../../utilities';
import { AppService } from 'src/app/core/services/app.service';
import { FeatureToggleTypes } from '../../enums/feature-toggle-types.enum';

// TODO: Add some unit tests!
export class Timeline {
  constructor(private appService: AppService) {}
  // FederalSummary: TimelimitSummary;
  // StateSummary: TimelimitSummary;
  // CSJSummary: TimelimitSummary;
  // W2TSummary: TimelimitSummary;
  // TEMPSummary: TimelimitSummary;
  // CMCSummary: TimelimitSummary;
  // OPCSummary: TimelimitSummary;
  // OtherSummary: TimelimitSummary;

  public timelineMonths: Dictionary<string, TimelineMonth> = new Dictionary<string, TimelineMonth>();
  private _clockStates = new Map<ClockTypes, ClockStates>();

  public extensionSequences: ExtensionSequence[] = [];

  public lastBatchRun: moment.Moment;

  public static getKey(date: moment.MomentInput): string {
    let keyDate = moment(date);
    return keyDate.month() + '-' + keyDate.year();
  }

  public getTimelineMonth(date: moment.MomentInput): TimelineMonth {
    return this.timelineMonths.getValue(Timeline.getKey(date)) || new TimelineMonth(date);
  }

  public setTimelineMonth(timelineMonth: TimelineMonth, clearCache = true) {
    for (let extSeq of this.extensionSequences) {
      // Add any relevant extensions (including state) into the timeline month
      if (extSeq.isDeleted || !extSeq.currentExtension || extSeq.currentExtension.decision === ExtensionDecision.Deny) {
        continue;
      }
      //TODO: PERF? if (extSeq.currentExtension.dateRange.overlaps(timelineMonth.monthRange,{adjacent:true})) {
      if (extSeq.currentExtension.dateRange.contains(timelineMonth.date)) {
        timelineMonth.extensions.push(extSeq.currentExtension);
      }
    }

    // if it doesn't have a cached orginal value at this point we will use the existing one
    timelineMonth.cachedOriginalModel = timelineMonth.clone();

    this.timelineMonths.setValue(Timeline.getKey(timelineMonth.date), timelineMonth);
    if (clearCache) {
      this.clearCachedData();
    }
  }

  public deleteTick(timelineMonth: TimelineMonth) {
    return this.timelineMonths.remove(Timeline.getKey(timelineMonth.date));
  }
  public clearMonth(date: moment.Moment) {
    return this.timelineMonths.remove(Timeline.getKey(date));
  }

  /** This method get ticks and accepts flags / filters.
   * getTicks(ClockTypes.Federal | ClockTypes.)
   */
  public getTicks(clockTypes?: ClockTypes, includedNoPlacementLimit?: boolean) {
    if (clockTypes == null) {
      return this.timelineMonths.values();
    }

    return this.timelineMonths.values().filter((month, index) => {
      if (month.tick && month.tick.clockTypes.any(clockTypes) && (includedNoPlacementLimit || !month.tick.clockTypes.any(ClockTypes.NoPlacementLimit))) {
        if (month.tick.clockTypes.any(ClockTypes.CMC) && (month.tick.clockTypes.state.State || month.tick.clockTypes.state.Federal)) {
          return true;
        } else if (!month.tick.clockTypes.any(ClockTypes.CMC)) return true;
      }
      return false;
    });
  }

  public getTicksInRange(start: moment.Moment, end: moment.Moment): TimelineMonth[] {
    let monthRangeIter = range(start, end).by('month');
    let monthArr = Array.from(monthRangeIter);
    let tMonths: TimelineMonth[] = [];
    monthArr.map((moment, index, array) => {
      let tickMonth: TimelineMonth = this.timelineMonths.getValue(Timeline.getKey(moment));
      if (tickMonth) {
        tMonths.push(tickMonth);
      }
    });
    return tMonths;
  }

  public getUsedMonths(clockType: ClockTypes) {
    let includeNoPlacementLimit = ClockType.any(clockType, ClockTypes.State) || ClockType.any(clockType, ClockTypes.Federal) || ClockType.any(clockType, ClockTypes.OTHER);
    return this.getTicks(clockType.valueOf(), includeNoPlacementLimit).length;
  }

  public getMaxMonths(clockType: ClockTypes) {
    // let stateExtension = this.getCurrentExtension(ClockTypes.State);
    // if (ClockType.any(clockType, ClockTypes.PlacementLimit) && stateExtension) {
    //   return null;
    // }

    let isCurrentMonthTicked = false;
    this.getTicks(clockType.valueOf()).filter(tick => {
      if (tick.isCurrentMonth === true && !tick.tick.clockTypes.state.None) isCurrentMonthTicked = true;
    });
    let stateExtension = this.extensionSequences
      .map(x => x.currentExtension)
      .filter(x => {
        return x && x.clockType.state.State && x.decision === ExtensionDecision.Approve && x.isDeleted === false && !x.hasElapsed;
      });

    // if (ClockType.any(clockType, ClockTypes.PlacementLimit) && stateExtension && stateExtension.length) {
    //   return null;
    // }

    let clockExtensions = this.extensionSequences
      .filter(x => {
        let currentExt = x.currentExtension;
        if (!x.isDeleted && currentExt && currentExt.clockType.eql(clockType) && currentExt.decision === ExtensionDecision.Approve) {
          return !currentExt.hasElapsed;
        }
      })
      .map(x => x.currentExtension);

    let extensions = clockExtensions.concat(stateExtension);

    // let extFutureMonths: number = 0;
    let clockMax = this.GetClockMax(clockType);
    let max = this.GetClockMax(clockType);

    let additionalMaxMonths = 0;

    if (max != null) {
      let usedMonths = this.getUsedMonths(clockType);
      // let remaining = (max - usedMonths) > 0 ? (max - usedMonths) : 0;

      max = usedMonths;

      if (extensions && extensions.length) {
        let endDates = extensions.map(x => x.dateRange.end);
        let maxEnd = moment.max(...endDates);
        let dateRange = new DateRange(Utilities.currentDate, maxEnd);
        let dateRangeMonths = Array.from(dateRange.by('month'));
        const currentMonth = Utilities.currentDate.clone().startOf('month');

        for (let month of dateRangeMonths) {
          if (max < clockMax) {
            max++;
          } else {
            // if (month.isSame(currentMonth, 'month')) {
            //   // check if we ticked for the current month or not, add if not ticked
            //   const tMonth = this.getTimelineMonth(currentMonth);
            //   if (tMonth && tMonth.tick.clockTypes.state.None) {
            //     max++;
            //     continue;
            //   }
            // }

            // let overlappingExtensions = extensions.filter(x => x.dateRange.contains(month));
            let overlappingExtensions = extensions.filter(x => x.dateRange.end.isSameOrAfter(month, 'month'));
            if (overlappingExtensions && overlappingExtensions.length > 0) {
              max++;
            }
          }
        }

        if (isCurrentMonthTicked) max--;
        // if max is less then clockMax after adding extension months,

        //   for (let extension of extensions) {

        //     const extMonths = Array.from(extension.dateRange.by('month'));
        //     const currentMonth = Utilities.currentDate.clone().startOf('month');
        //     for (let month of extMonths) {

        //       // 1. if extension month is in the future add it to the max
        //       if (month.isAfter(currentMonth, 'month')) {
        //         max++;
        //       }

        //       // 2. Check if we should include the current month
        //       if (month.isSame(currentMonth, 'month')) {
        //         // check if we ticked for the current month or not, add if not ticked
        //         const tMonth = this.getTimelineMonth(currentMonth);
        //         if (tMonth && tMonth.tick.clockTypes.state.None) {
        //           max++;
        //         }
        //       }
        //     }
        //   }
        // }
        // // max += extFutureMonths > 0 ? extFutureMonths : 0;

        // // if used is greater then max, add the difference. Only after adding extension months
        // if (usedMonths > clockMax) {
        //   max += usedMonths - clockMax;
        // }
      }
      if (max < clockMax) {
        max = clockMax;
      }
    }
    return max;
  }

  public getRemainingMonths(clockType: ClockTypes) {
    let maxMonths = this.getMaxMonths(clockType);
    let usedMonths = this.getUsedMonths(clockType);
    let remaining = maxMonths == null ? -1 : maxMonths - usedMonths;
    return remaining < 0 ? null : remaining;
  }

  // public get deniableExtensionClocks() {

  //   let clockStates = this.clockStates;
  //   let deniableClocks: ClockTypes[] = [];

  //   EnumEx.getValues(ClockTypes).map(cType => {
  //     let clockState = clockStates.get(cType);
  //     if (clockState === ClockStates.CanBeExtended || clockState == ClockStates.Warn || clockState == ClockStates.Danger) {
  //       return true;
  //     } else if (clockState === ClockStates.None) {
  //       // check if we have a current denail
  //     }
  //   });

  //   return deniableClocks;
  // }

  public get clockStates(): Map<ClockTypes, ClockStates> {
    if (this._clockStates == null) {
      this._clockStates = new Map<ClockTypes, ClockStates>();

      let stateExtension = this.getCurrentExtension(ClockTypes.State);
      let stateDenied = stateExtension && stateExtension.decision === ExtensionDecision.Deny;
      let stateApproved = stateExtension && stateExtension.decision === ExtensionDecision.Approve;
      let stateClockMax = this.GetClockMax(ClockTypes.State);
      let stateUsed = this.getStateTicks().length;
      let stateRemaining = stateClockMax - stateUsed > 0 ? stateClockMax - stateUsed : 0;
      let isOverStateMax = stateRemaining < 1;

      EnumEx.getValues(ClockTypes).map(cType => {
        let clockExtension = cType === ClockTypes.State ? stateExtension : this.getCurrentExtension(cType);
        let clockDenied = clockExtension && clockExtension.decision === ExtensionDecision.Deny;
        let clockApproved = clockExtension && clockExtension.decision === ExtensionDecision.Approve;
        let clockType = new ClockType(cType);
        let clockState = ClockStates.None;
        let clockTypeMax = this.GetClockMax(cType);

        if (clockTypeMax != null) {
          //let count = this.getTicks(clockType.valueOf()).length;
          let remainingMonths = this.getRemainingMonths(cType);

          let isWarn: boolean;
          let isDanger: boolean;
          let canBeExtended: boolean;
          let willCauseGapExtension: boolean;
          let canBeDenied: boolean;

          // Transition Logic Start
          const isInTransitionPeriod = this.getIsInOrAfterTransition(
            Utilities.currentDate.clone(),
            this.appService.getFeatureToggleValue(FeatureToggleTypes.StateMax48MonthStartDate)
          );
          const maxWarningMonths = isInTransitionPeriod ? 18 : 6;
          // Transition Logic End

          if (clockType.state.State) {
            isWarn = (!stateApproved && remainingMonths <= maxWarningMonths) || (stateApproved && remainingMonths <= 3);
            isDanger = (!stateApproved && remainingMonths <= 4) || (stateApproved && remainingMonths <= 1);
            canBeExtended = isDanger || isWarn || stateDenied;
            willCauseGapExtension = false;
            canBeDenied = stateExtension != null || canBeExtended;
          } else {
            isWarn = ((!clockApproved && remainingMonths <= 7) || (clockApproved && remainingMonths <= 3)) && !isOverStateMax;
            isDanger = ((!clockApproved && remainingMonths <= 4) || (clockApproved && remainingMonths <= 1)) && !isOverStateMax;
            let stateCountAfterRemainingUsed = remainingMonths + stateUsed;

            if (stateRemaining <= maxWarningMonths) {
              canBeExtended = false;
              isWarn = isWarn && !stateApproved;
              isDanger = isDanger && !stateApproved;
              canBeDenied = isWarn || isDanger || clockExtension != null;
            } else {
              willCauseGapExtension = stateCountAfterRemainingUsed > stateClockMax - 6 && stateCountAfterRemainingUsed <= stateClockMax;
              canBeExtended = isWarn || isDanger;
              canBeDenied = isWarn || isDanger || clockExtension != null;
            }

            // // not a state clock, but state is extended so we will hide any other warnings
            // let stateCountAfterRemainingUsed = remainingMonths + stateUsed;
            // let willEndInGapPeriod = stateCountAfterRemainingUsed > (stateClockMax - 6) && stateCountAfterRemainingUsed <= stateClockMax;

            // willCauseGapExtension = willEndInGapPeriod;

            // isWarn = ((!clockApproved && remainingMonths <= 6) || (clockApproved && remainingMonths <= 3)) && !isOverStateMax;
            // isDanger = ((!clockApproved && remainingMonths <= 4) || (clockApproved && remainingMonths <= 1)) && !isOverStateMax;

            // if (willEndInGapPeriod && stateExtension) {
            //   isWarn = isWarn && !stateApproved;
            //   isDanger = isWarn && isDanger;
            // } else if (willEndInGapPeriod) {
            //   canBeExtended = (isWarn || isDanger) && stateCountAfterRemainingUsed < stateClockMax;
            // }else{
            //   canBeExtended = (isWarn || isDanger) && !stateApproved || clockDenied;
            // }

            // canBeDenied = clockExtension != null || canBeExtended;
          }

          //clockState = isDanger ? ClockStates.Danger : (isWarn ? ClockStates.Warn : (canBeExtended ? ClockStates.CanBeExtended : ClockStates.None));
          clockState = this.addFlagsIf(isDanger, clockState, ClockStates.Danger);
          clockState = this.addFlagsIf(isWarn, clockState, ClockStates.Warn);
          clockState = this.addFlagsIf(canBeExtended, clockState, ClockStates.CanBeExtended);
          clockState = this.addFlagsIf(willCauseGapExtension, clockState, ClockStates.WillCauseGapExtension);
          clockState = this.addFlagsIf(canBeDenied, clockState, ClockStates.SequenceCanBeDenied);
        }

        this._clockStates.set(cType, clockState);
      });
    }
    return this._clockStates;
  }

  private addFlagsIf(shouldAdd: boolean, clockState: ClockStates, flags: ClockStates) {
    if (shouldAdd) {
      clockState |= flags; //Add flag(s)
    } else {
      clockState &= ~flags; //Remove Flag(s)
    }
    return clockState;
  }

  public get timelineState() {
    let hasWarning = false;
    let hasDanger = false;
    let canBeExtended = false;

    let states = Array.from(this.clockStates.values());
    let clockState = ClockStates.None;
    states.map(x => {
      hasDanger = hasDanger || ClockState.any(x, ClockStates.Danger);
      hasWarning = hasWarning || ClockState.any(x, ClockStates.Warn);
      canBeExtended = canBeExtended || ClockState.any(x, ClockStates.CanBeExtended);
    });
    clockState = this.addFlagsIf(hasDanger, clockState, ClockStates.Danger);
    clockState = this.addFlagsIf(hasWarning, clockState, ClockStates.Warn);
    clockState = this.addFlagsIf(canBeExtended, clockState, ClockStates.CanBeExtended);
    return new ClockState(clockState);
  }

  public getFederalTicks() {
    return this.timelineMonths.values().filter((val, index) => {
      return val.tick && val.tick.clockTypes && val.tick.clockTypes.has(ClockTypes.Federal);
    });
  }

  public getStateTicks() {
    return this.timelineMonths.values().filter((val, index) => {
      return val.tick && val.tick.clockTypes && val.tick.clockTypes.has(ClockTypes.State);
    });
  }

  /**
   *
   *
   * @param {moment.MomentInput} date
   * @param {ExtensionDecision} [extensionDecision]
   * @returns {Extension[]}
   * @description get extensions that cover a the date period. Will only return the latest decision in the sequence(s)
   * @memberOf Timeline
   *
   */
  public getExtensions(extensionDecision?: ExtensionDecision): Extension[] {
    if (this.extensionSequences.length < 1) {
      return [];
    }

    //  from sequences, merge into single array, and filter
    let exSequenceExtensions = this.extensionSequences
      .filter(x => {
        return !x.isDeleted;
      }) // get non-deleted sequences
      .map(x => x.extensions); // get their extensions

    let exts: Extension[] = [];
    // NOTE: if there is more then one? Reduce to latest decisions in the highest sequence
    if (exSequenceExtensions.length > 1) {
      exts = exSequenceExtensions.reduce((x, y) => {
        return x.concat(y);
      }); // merge into a single array
    } else if (exSequenceExtensions.length === 1) {
      exts = exSequenceExtensions[0];
    }

    return (
      exts
        // .map(x => x.extensions) // get their extensions
        // .reduce((x, y) => { return x.concat(y); }) // merge into a single array
        .filter(x => {
          // filter by extension decision if provided
          return extensionDecision == null || x.decision === extensionDecision;
        })
    );
  }

  // public getCurrentStateExtension():ExtensionSequence {
  //     let stateExtSequence:ExtensionSequence = null;
  //     let stateExts: ExtensionSequence[] = this.extensionSequences.filter(x => {
  //         return !x.isDeleted && x.clockType === ClockTypes.State
  //               && x.currentExtension != null && !x.currentExtension.hasElapsed
  //     });
  //     // TODO: What do we do if there is more then one? Reduce to latest decision and throw error?
  //     // System should prevent this, but it shouldn't break if it happens...
  //   if (stateExts.length > 1) {
  //     console.warn(`Too many extension sequences for time limit type: ${ClockTypes[this.clockTypeFlag]}. Please delete one. I'm going to use the "last" one `);
  //     // TODO: Figure out what to do here. Redirect to extension screen with error message?
  //     stateExtSequence = stateExts.reduce((a, b) => {
  //       if (a.currentExtension.createdDate.isAfter(b.currentExtension.createdDate)) {
  //         return a;
  //       } else {
  //         return b;
  //       }
  //     });
  //   }
  //     return stateExtSequence ||  stateExts[0];
  // }

  public getCurrentExtension(clockTypes: ClockTypes): Extension {
    let clockType = new ClockType(clockTypes);
    let ext: Extension = null;
    let exts: Extension[] = this.extensionSequences
      .filter(x => {
        let currentExt = x.currentExtension;
        return !x.isDeleted && clockType.eql(x.clockType) && currentExt != null && !currentExt.hasElapsed;
      })
      .map(x => x.currentExtension);

    // NOTE: if there is more then one? Reduce to latest decisions in the highest sequence
    if (exts.length > 1) {
      ext = exts.reduce((a, b) => {
        if (a.decisionDate.isAfter(b.decisionDate)) {
          return a;
        } else {
          return b;
        }
      });
    }
    return ext || exts[0];
  }

  // public getCurrentExtensions() {
  //     return this.extensions.filter((ex, index) => {
  //         return ex.dateRange.end.isSameOrAfter(Utilities.currentDate, 'month');
  //     });
  // }

  federalMax: number;
  stateMax: number = 60;
  stateMax48: number = 48;
  CSJMax: number = 24;
  W2TMax: number = 24;
  TEMPMax: number = 24;
  CMCMax: number;
  OPCMax: number;
  otherMax: number;

  public GetClockMax(clockType: ClockTypes) {
    let cType = new ClockType(clockType);

    if (cType.eql(ClockTypes.Federal)) {
      return this.federalMax;
    }
    if (cType.eql(ClockTypes.State)) {
      return this.getIsInOrAfterTransition(Utilities.currentDate.clone(), this.appService.getFeatureToggleValue(FeatureToggleTypes.StateMax48MonthStartDate), true)
        ? this.stateMax48
        : this.stateMax;
    }
    if (cType.eql(ClockTypes.CSJ)) {
      return this.CSJMax;
    }
    if (cType.eql(ClockTypes.W2T)) {
      return this.W2TMax;
    }
    if (cType.eql(ClockTypes.TEMP)) {
      return this.TEMPMax;
    }
    if (cType.eql(ClockTypes.CMC)) {
      return this.CMCMax;
    }
    if (cType.eql(ClockTypes.OPC)) {
      return this.OPCMax;
    }
    if (cType.eql(ClockTypes.OTHER)) {
      return this.otherMax;
    }
    return null;
  }

  public clearCachedData() {
    this._clockStates = null;
  }

  public clone(): Timeline {
    return Object.assign(new Timeline(this.appService), this);
  }

  public getExtensionLength(clockType: ClockTypes, maxExtensionMonths = 6) {
    let extLength = 0;
    let extensionDateRange = this.getExtensionDateRange(clockType, maxExtensionMonths);
    if (extensionDateRange) {
      const extMonths = Array.from(extensionDateRange.by('month'));
      extLength = extMonths.length;
    }
    return extLength;
  }

  // public getExtensionLength(clockType: ClockTypes) {
  //   let extLength = 0;
  //   let clockMax = this.GetClockMax(clockType);
  //   if(!clockMax){
  //     console.warn("Extension length invalid for ClockType flag(s) ("+ClockType.toString(clockType)+"). method: getExtensionLength")
  //     return;
  //   }
  //
  //   let stateUsed = this.getStateTicks().length;
  //   let stateClockMax = this.GetClockMax(ClockTypes.State);
  //
  //   // If we are checking a weird type or already over the lifetime limit, do not extend other clocks, only state.
  //   if(clockType !== ClockTypes.State && stateUsed >= stateClockMax){
  //     extLength = 0;
  //   }else{
  //     extLength = 6;
  //
  //     if(clockType !== ClockTypes.State){
  //       // Determine if remaining ticks or extension would push us into "warning" on the state clock.
  //       // if so, calculate extension length to equal the date we think the state clock with hit it's max
  //
  //       let stateRemaining = this.getRemainingMonths(ClockTypes.State);
  //       let clockRemaining = this.getRemainingMonths(clockType);
  //       let stateUsedAfterClockAndExt = stateUsed + clockRemaining + 6;
  //       if(stateUsedAfterClockAndExt >= stateClockMax - 6){
  //         extLength = stateRemaining - clockRemaining;
  //       }
  //     }
  //   }
  //
  //   return extLength;
  // }

  getStateOnlyExtensionDateRange() {
    let startDate = Utilities.currentDate.clone();
    let stateClockState = this.clockStates.get(ClockTypes.State);

    if (ClockState.any(stateClockState, ClockStates.Danger | ClockStates.Warn)) {
      // if we are already in a warning/danger, start the DateRange now
      startDate.startOf('month');
    } else {
      // Otherwise figure out when we would be within 6 of the max and start it then
      let stateUsed = this.getStateTicks().length;
      let stateClockMax = this.GetClockMax(ClockTypes.State);
      let monthsTillWarning = stateClockMax - 6 - stateUsed;

      startDate.add(monthsTillWarning, 'months').startOf('month');
    }

    return new DateRange(startDate);
  }

  getExtensionDateRange(clockType: ClockTypes, maxExtensionLength = 6, isCurrentMonthTicked = false, isBackDated = false): DateRange {
    maxExtensionLength = maxExtensionLength == null ? 6 : maxExtensionLength;
    let isExtensible = ClockType.any(clockType, ClockTypes.ExtensableTypes);

    let clockMax = this.GetClockMax(clockType);

    if (!isExtensible || clockMax == null) {
      return null; // SKIP if it isn't extensible or there is not TIME limit for the clock type
    }

    let remainingMonths = this.getRemainingMonths(clockType);

    if (remainingMonths == null) {
      return null; // Skip when remaining months is ignored
    }

    let stateMax = this.GetClockMax(ClockTypes.State);
    let stateUsed = this.getStateTicks().length;
    let stateRemaining = stateMax - stateUsed;

    if (clockType !== ClockTypes.State) {
      //check if we only need a state extensions || a state extensions would cover the remaining time on the ClockType
      if (stateUsed > stateMax || stateRemaining <= remainingMonths) {
        return null;
      }
    }

    // Transition Logic End
    let calculatedStartDate = this.getTransitionPeriodStartDate(
      Utilities.currentDate.clone(),
      this.appService.getFeatureToggleValue(FeatureToggleTypes.StateMax48MonthStartDate),
      stateUsed,
      remainingMonths,
      clockType,
      isCurrentMonthTicked,
      isBackDated
    );
    // Transition Logic End

    let startDate =
      calculatedStartDate !== null
        ? moment(calculatedStartDate)
        : Utilities.currentDate
            .clone()
            .startOf('month')
            .add(remainingMonths, 'months');
    // we have to subtract one, because adding 1 month, will cause the iterator to iterate the first and second month (2)
    // IE. 01/01/2017 + 6 months  = 01/01-2017-07/01/2017. DateRange.duration() is 6-months, but the iterator will iterate 7
    // jan,feb,march,april,may,jun,july. We only want jan, feb, march, april, may, jun
    let endDate = startDate
      .clone()
      .add(maxExtensionLength - 1, 'months')
      .startOf('month');

    if (isCurrentMonthTicked && calculatedStartDate == null) {
      startDate.add(1, 'month');
      endDate.add(1, 'month');
    }

    if (isBackDated && calculatedStartDate == null) {
      startDate.subtract(1, 'month');
      endDate.subtract(1, 'month');
    }

    let dateRange = new DateRange(startDate, endDate);

    // if (clockType !== ClockTypes.State) {
    //   let stateOnlyExtensionDateRange = this.getStateOnlyExtensionDateRange();
    //   if (stateRemaining > 0 && stateOnlyExtensionDateRange.overlaps(dateRange)) {
    //     // we must use a gap to get up to stateMax. Extension can end the month before the state extension starts (-1)
    //     dateRange.end = Utilities.currentDate.clone().startOf('month').add(stateRemaining - 1, 'months');
    //   }
    // }

    return dateRange;
  }

  getIsInOrAfterTransition(currentDate: moment.Moment, stateMax48MonthStartDate: string, isAfter = false) {
    return isAfter
      ? currentDate.isSameOrAfter(stateMax48MonthStartDate)
      : currentDate.isBetween(moment(stateMax48MonthStartDate).subtract(6, 'months'), stateMax48MonthStartDate, undefined, '[)');
  }

  getTransitionPeriodStartDate(
    currentDate: moment.Moment,
    stateMax48MonthStartDate: string,
    stateUsed: number,
    remaining: number,
    clockType: ClockTypes,
    isCurrentMonthTicked = false,
    isBackDated = false
  ) {
    let calculatedStartDate = null;
    const currentExtensionDecision = this.getCurrentExtension(clockType);
    if (
      this.getIsInOrAfterTransition(currentDate, stateMax48MonthStartDate) &&
      clockType === ClockTypes.State &&
      (currentExtensionDecision === undefined || currentExtensionDecision.decision !== ExtensionDecision.Approve)
    ) {
      const stateMax48MonthStart = moment(stateMax48MonthStartDate).startOf('month');
      if (stateUsed < 54) {
        const calculated48MonthDate = currentDate.startOf('month').add(this.stateMax48 - stateUsed, 'months');
        if (isCurrentMonthTicked) calculated48MonthDate.add(1, 'month');
        if (isBackDated) calculated48MonthDate.subtract(1, 'month');
        calculatedStartDate = calculated48MonthDate.isAfter(stateMax48MonthStart) ? calculated48MonthDate : stateMax48MonthStart;
      } else {
        const calculated60MonthStartDate = currentDate.startOf('month').add(remaining, 'months');
        if (isCurrentMonthTicked) calculated60MonthStartDate.add(1, 'month');
        if (isBackDated) calculated60MonthStartDate.subtract(1, 'month');
        calculatedStartDate = calculated60MonthStartDate.isBefore(stateMax48MonthStart) ? calculated60MonthStartDate : stateMax48MonthStart;
      }
    }
    return calculatedStartDate;
  }
}
