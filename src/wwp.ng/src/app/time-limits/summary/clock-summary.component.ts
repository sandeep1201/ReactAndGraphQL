import { AppService } from './../../core/services/app.service';
import { DateRange, range } from '../../shared/moment-range';
import { Component, OnInit, Input, HostBinding, ChangeDetectionStrategy } from '@angular/core';
import { ClockType, ClockTypes, Extension, ExtensionDecision, ExtensionSequence, Timeline, ClockState, ClockStates } from '../../shared/models/time-limits';
import * as moment from 'moment';

@Component({
  selector: 'app-clock-summary',
  templateUrl: './clock-summary.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  styleUrls: ['./clock-summary.component.css']
})
export class ClockSummaryComponent implements OnInit {
  ClockTypes = ClockTypes;
  ExtensionDecision = ExtensionDecision;

  _timeline: Timeline;
  private isLoaded: boolean = false;
  private _clockStates: Map<ClockTypes, ClockStates>;
  @Input() get timeline(): Timeline {
    return this._timeline;
  }

  set timeline(val) {
    if (val) {
      this._timeline = val;
      this.loadData();
    }
  }

  @Input() name: string;

  @Input()
  get clockTypeFlag() {
    return this._clockTypeFlags;
  }

  set clockTypeFlag(clockTypes: ClockTypes) {
    this._clockTypeFlags = clockTypes;
    this.clockType = new ClockType(this._clockTypeFlags);
  }

  clockType: ClockType;

  usedMonths: number = null;

  maxMonths: number = null;

  remainingMonths: number = null;

  private _clockTypeFlags: ClockTypes;

  @HostBinding('class.active') get isActive(): boolean {
    return +this.usedMonths > 0;
  }

  @HostBinding('class.extension') get hasExtensions() {
    return this.extension != null && this.extension.decision === ExtensionDecision.Approve;
  }

  @HostBinding('class.ext-denied') get hasDeniedExtension() {
    return this.extension != null && this.extension.decision === ExtensionDecision.Deny;
  }

  @HostBinding('class.ext-danger') isPassedExtensionDeadline = false;

  // @HostBinding('class.ext-danger') get isPassedExtensionDeadline() {
  //   // TODO: Move to Timeline. Make warn/danger limits configurable
  //   if (this.timeline) {
  //     if (this.clockType.state.State) {
  //       return !this.stateExtension && this.remainingMonths < 6 || this.stateExtension && this.stateExtension.hasStarted && this.remainingMonths < 4;
  //     } else {
  //       if (!this.stateExtension && this.maxMonths) {
  //         let stateTicks = this.timeline.getStateTicks();
  //         let stateUsed = stateTicks.length;
  //         let stateRemaining = this.timeline.stateMax - stateUsed;
  //
  //         return stateRemaining > 6 && (!this.extension && this.remainingMonths < 6 || this.extension && this.extension.hasStarted && this.remainingMonths < 4);
  //       } else {
  //         return false;
  //       }
  //     }
  //   }
  //   return false;
  // }

  @HostBinding('class.ext-warn') public isApproachingExtensionDeadline = false;
  // @HostBinding('class.ext-warn') get isApproachingExtensionDeadline() {
  //   // TODO: Move to Timeline. Make warn/danger limits configurable
  //   if (this.timeline) {
  //     if (this.clockType.state.State) {
  //       return !this.stateExtension && this.remainingMonths <= 6 || this.stateExtension && this.stateExtension.hasStarted && this.remainingMonths <= 4;
  //     } else {
  //       if (!this.stateExtension && this.maxMonths) {
  //         let stateTicks = this.timeline.getStateTicks();
  //         let stateUsed = stateTicks.length;
  //         let stateRemaining = this.timeline.stateMax - stateUsed;
  //
  //         return stateRemaining > 6 && (!this.extension && this.remainingMonths <= 6 || this.extension && this.extension.hasStarted && this.remainingMonths <= 4);
  //       } else {
  //         return false;
  //       }
  //     }
  //   }
  //   return false;
  // }
  public canBeExtended = false;
  public extension: Extension;

  public stateExtension: Extension;

  OTFCount = 0;

  TribalCount = 0;

  TJBCount = 0;

  JOBSCount = 0;

  NoPlacementLimitCount = 0;

  constructor() {}

  ngOnInit() {
    this.isLoaded = true;
    this.loadData();
  }

  // getClockCount(clockType: ClockTypes, includedNoPlacementLimit:boolean = false) {
  //   return this.timeline ? this.timeline.getTicks(clockType,includedNoPlacementLimit).length : 0;
  // }

  private loadData() {
    if (this._clockTypeFlags != null) {
      this.initExtensions();
      this.initCounts();
    }
  }

  private initExtensions() {
    if (this.isLoaded) {
      this.initClockExtension();
      this.initStateExtension();
    }
  }

  private initCounts() {
    this._clockStates = this.timeline.clockStates;
    let clockState = new ClockState(this._clockStates.get(this._clockTypeFlags));
    this.isApproachingExtensionDeadline = clockState.state.Warn;
    this.isPassedExtensionDeadline = clockState.state.Danger;
    this.usedMonths = this.timeline.getUsedMonths(this.clockTypeFlag);
    let stateUsed = this.timeline.getUsedMonths(ClockTypes.State);
    let stateClockMax = this.timeline.GetClockMax(ClockTypes.State);
    let clockMax = this.timeline.GetClockMax(this._clockTypeFlags);
    let maxMonths = this.timeline.getMaxMonths(this.clockTypeFlag);

    if (this.clockType.state.PlacementLimit && stateUsed >= stateClockMax) {
      this.remainingMonths = null;
      this.maxMonths = null;
    } else if (this.clockType.state.PlacementLimit && this.usedMonths >= clockMax) {
      this.remainingMonths = this.timeline.getRemainingMonths(this.clockTypeFlag);
      this.maxMonths = null;
    } else if (this.clockType.state.State) {
      this.remainingMonths = this.timeline.getRemainingMonths(this.clockTypeFlag);
      this.maxMonths = stateUsed > stateClockMax ? null : maxMonths;
    } else {
      this.remainingMonths = this.timeline.getRemainingMonths(this.clockTypeFlag);
      this.maxMonths = !clockMax || this.usedMonths >= clockMax ? null : maxMonths;
    }

    this.canBeExtended = clockState.state.CanBeExtended;

    if (this.clockType.any(ClockTypes.OTHER)) {
      this.OTFCount = this.timeline.getTicks(ClockTypes.OTF).length;
      this.TribalCount = this.timeline.getTicks(ClockTypes.TRIBAL).length;
      this.TJBCount = this.timeline.getTicks(ClockTypes.TJB).length;
      this.JOBSCount = this.timeline.getTicks(ClockTypes.JOBS).length;
      this.NoPlacementLimitCount = this.timeline.getTicks(ClockTypes.NoPlacementLimit, true).length;
    }
  }

  private initClockExtension() {
    this.extension = this.timeline.getCurrentExtension(this._clockTypeFlags);
    // let extension: Extension = null;
    // let extSequence: ExtensionSequence = null;
    // let extSequences = this.timeline.extensionSequences.filter(a => {
    //     let currEx = a.currentExtension;
    //     return currEx != null && !currEx.isDeleted
    //       && currEx.clockType.any(this.clockTypeFlag)
    //       && (!currEx.hasElapsed || currEx.decision === ExtensionDecision.Deny)
    //   });
    //
    // if (extSequences.length > 1) {
    //   console.warn(`Too many extension sequences for time limit type: ${ClockTypes[this.clockTypeFlag]}. Please delete one. I'm going to use the "last" one `);
    //   // TODO: Figure out what to do here. Redirect to extension screen with error message?
    //   extSequence = extSequences.reduce((a, b) => {
    //     if (a.currentExtension.createdDate.isAfter(b.currentExtension.createdDate)) {
    //       return a;
    //     } else {
    //       return b;
    //     }
    //   });
    // }
    // this.extension = extSequences.map(x => x.currentExtension)[0];
  }

  private initStateExtension() {
    if (this.clockType.state.State) {
      this.stateExtension = this.extension;
    } else {
      this.stateExtension = this.timeline.getCurrentExtension(ClockTypes.State);
    }
  }
}
