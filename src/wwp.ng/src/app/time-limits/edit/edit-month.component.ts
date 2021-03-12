import { ErrorInfo } from '../../shared/models/ErrorInfoContract';
import { coerceNumberProperty } from '../../shared/decorators/number-property';
import { Authorization } from '../../shared/models/authorization';
import { Participant } from '../../shared/models/participant';
import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnChanges, OnDestroy, OnInit, Output, SimpleChange, SimpleChanges } from '@angular/core';
import { Extension, ExtensionDecision, ClockTypes, ClockType, TimelineMonth, Timeline, Tick, ReasonForChange } from '../../shared/models/time-limits';
import { states } from '../../shared/models/us-states';
import { DateRange } from '../../shared/moment-range';
import { DropDownField } from '../../shared/models/dropdown-field';
import { ValidationManager } from '../../shared/models/validation';
import { ModelErrors } from '../../shared/interfaces/model-errors';
import { Observable, Subject, Subscription, empty } from 'rxjs';
import { distinctUntilChanged, catchError, finalize, take, debounceTime } from 'rxjs/operators';
import * as moment from 'moment';
import { AppService } from './../../core/services/app.service';
import { TimeLimitsService } from '../../shared/services/timelimits.service';
import { EnumEx, TextTransformer, Utilities } from '../../shared/utilities';
import { ClockTypeNamePipe } from '../pipes/clock-type-name.pipe';

@Component({
  selector: 'app-edit-month',
  templateUrl: './edit-month.component.html',
  styleUrls: ['./edit-month.component.css'],
  changeDetection: ChangeDetectionStrategy.Default
})
export class EditMonthComponent implements OnInit, OnDestroy {
  @Input()
  pin: string;
  @Output()
  close = new EventEmitter();
  @Output()
  save = new EventEmitter<TimelineMonth[]>();
  @Input()
  get model(): TimelineMonth {
    return this._modelCopy;
  }
  set model(val: TimelineMonth) {
    if (val instanceof TimelineMonth) {
      this.originalModel = val.cachedOriginalModel;
      this._modelCopy = val.clone();

      // clear out reason for change and details. They must be new for each edit
      this._modelCopy.reasonForChange = null;
      this._modelCopy.reasonForChangeDetails = null;
      this._modelCopy.cachedOriginalModel.reasonForChange = null;
      this._modelCopy.cachedOriginalModel.reasonForChangeDetails = null;
      this.UpdateTimelimitTypesDrop();
    } else {
      this._modelCopy = val;
      this.selectedMonths.length = 0;
      this.selectedMonths = [];
    }
  }
  @Input()
  get timeline(): Timeline {
    return this._timeline;
  }

  set timeline(val: Timeline) {
    this._timeline = val;
    this.updateExcludedMonths();
  }
  @Input()
  participant: Participant;

  public ClockTypes = ClockTypes;
  public states = states;
  public NOPlacementLimitRange = new DateRange('2009-11-01', '2011-12-31');

  public cmcOpcFederalOnlyStart = moment('2016-08-01');

  public ReasonForChange = ReasonForChange;
  public isModelValid = false;
  public isSaving = false;
  public modelErrors: ModelErrors = {};
  public timelimitsTypeDrop: DropDownField[] = [];
  public statesDrop: DropDownField[] = [];
  public selectedMonths: moment.Moment[] = [];
  // public reasonDetailsEmitter = new Subject<string>();
  // public reasonDetailsSub: Subscription;
  public notesEmitter = new Subject<string>();
  public notesSub: Subscription;
  public excludedMonths: moment.Moment[] = [];
  public get canCountTowardPlacementLimit() {
    return this.model && !this.NOPlacementLimitRange.contains(this._modelCopy.date);
  }
  private reasonsForChange: ReasonForChange[] = [];
  private subtractReasonsForChange: ReasonForChange[] = [];
  private addReasonsForChange: ReasonForChange[] = [];
  private _modelCopy: TimelineMonth;
  private originalModel: TimelineMonth;
  public hadSaveError: boolean = false;
  public errorMessage: string = '';
  private modelSub: Subscription;
  private _timeline: Timeline;
  private clockTypeNamePipe: ClockTypeNamePipe;
  constructor(private _timeLimitsService: TimeLimitsService, private appService: AppService, private validationManager: ValidationManager) {
    this.clockTypeNamePipe = new ClockTypeNamePipe();
  }

  get flagsAreChanged() {
    let result = !(this.model.tick == (this.originalModel.tick.valueOf()));
    return result || (this.selectedMonths && this.selectedMonths.length > 0);
  }

  get reasonsForChangeDrop(): DropDownField[] {
    let reasons: DropDownField[] = [];
    var activeReasonsForChange = this.model.tick.tickType === ClockTypes.None ? this.subtractReasonsForChange : this.addReasonsForChange;
    for (let reason of activeReasonsForChange) {
      reasons.push(new DropDownField(reason.id, reason.name));
    }
    return reasons;
  }

  get isPlacement() {
    return this.canCountTowardPlacementLimit && this.model.tick.clockTypes.state.PlacementLimit;
  }

  get isState() {
    return this.model.tick.clockTypes.state.State;
  }

  get isFederal() {
    return this.model.tick.clockTypes.state.Federal;
  }

  get isCMC() {
    return this.model.tick.clockTypes.state.CMC;
  }
  get canTogglePlacement() {
    return false;
    // return this.canCountTowardPlacementLimit && this.model.tick.clockTypes.any(ClockTypes.PlacementLimit)
  }

  get canToggleState() {
    return this.model.tick.clockTypes.any(ClockTypes.CMC | ClockTypes.OPC) && this.model.date.isSameOrAfter(this.cmcOpcFederalOnlyStart, 'month');
  }

  get canToggleFederal() {
    const isAuthorized =
      (this.participant.isParticipantServedByAgency(this.appService.user.agencyCode) || this.appService.user.agencyCode === 'DCF') &&
      this.appService.isUserAuthorizedToEditTimeLimits(this.participant) &&
      this.appService.isUserAuthorized(Authorization.timeLimitsFederalToggle, this.participant);
    return isAuthorized && !this.model.tick.clockTypes.state.OTF && !this.model.tick.clockTypes.state.None;
  }

  get isOTF() {
    const returnVal = this.model.tick.clockTypes.any(ClockTypes.OTF) && this.model.reasonForChange === 1; // TODO. Figure out a way not to hardcode this
    return returnVal;
  }

  get clockType() {
    return this.model.tick.tickType;
  }

  get notes() {
    if (this.model.tick.notes) {
      return TextTransformer.strip_tags(this.model.tick.notes);
    } else {
      return '';
    }
  }

  get minEditDate() {
    let w2startStartDate = TimeLimitsService.w2StartDate.clone();

    if (this.participant && this.participant.dateOfBirth && moment(this.participant.dateOfBirth).isValid()) {
      const participantDob = moment(this.participant.dateOfBirth);
      const participant18thBirthday = participantDob.add(18, 'years').startOf('month');
      const max = moment.max(w2startStartDate, participant18thBirthday);
      return max;
    } else {
      return w2startStartDate;
    }
  }

  ngOnInit() {
    this.initStatesDrop();
    this._timeLimitsService
      .getReasonsForChange()
      .pipe(
      take(1))
      .subscribe(x => {
        this.reasonsForChange = x.filter(x => x.isRequired && !x.isDeleted);
        this.subtractReasonsForChange = x.filter(x => x.isRequired && !x.isDeleted && x.code && x.code.toUpperCase().startsWith('S'));
        this.addReasonsForChange = x.filter(x => x.isRequired && !x.isDeleted && x.code && !x.code.toUpperCase().startsWith('S'));
        
      });
    this.notesSub = this.notesEmitter
    .pipe(
      debounceTime(500))
      .pipe(distinctUntilChanged())
      .subscribe(notes => this.model.tick.notes = notes);     
    
    this.UpdateTimelimitTypesDrop();  
    
  }

  UpdateTimelimitTypesDrop() {
    this.timelimitsTypeDrop = [];

    this.timelimitsTypeDrop.push(new DropDownField(ClockTypes.None, ClockTypes[ClockTypes.None]));

    if (this.model && this.model.date.isSame(Utilities.currentDate, 'month')) {
      if (this.model && this.model.cachedOriginalModel && !this.model.cachedOriginalModel.tick.clockTypes.state.None && !this.model.cachedOriginalModel.tick.clockTypes.state.OTF) {
        this.timelimitsTypeDrop.push(
          new DropDownField(this.model.cachedOriginalModel.tick.tickType, this.clockTypeNamePipe.transform(this.model.cachedOriginalModel.tick.tickType))
        );
      }
      this.timelimitsTypeDrop.push(new DropDownField(ClockTypes.OTF, ClockTypes[ClockTypes.OTF]));
    } else {
      EnumEx.getValues(ClockTypes).map(x => {
        if (ClockType.IsSingleFlag(x) && ClockType.any(x, ClockTypes.CreateableTypes) && this.isValidClockTypeForDate(x)) {
          this.timelimitsTypeDrop.push(new DropDownField(x, this.clockTypeNamePipe.transform(x)));
        }
      });
    }

    this.timelimitsTypeDrop.sort((a, b) => {
      if (a.id < b.id) return -1;
      if (a.id > b.id) return 1;
      return 0;
    });
  }

  updateExcludedMonths() {
    if (this.timeline) {
      this.excludedMonths = this.timeline.timelineMonths
        .values()
        .filter(x => !x.tick.clockTypes.state.None && !x.isDeleted)
        .map(x => x.date);
    }
  }

  private initStatesDrop() {
    this.statesDrop = [];
    for (let i = 0; i < states.length; i++) {
      const state = states[i];
      if (state.toLowerCase() === 'wisconsin') {
        continue;
      }
      this.statesDrop.push(new DropDownField(i + 1, state));
    }
  }

  setClockType(val: ClockType) {
    let cType = val ? <ClockTypes>val.valueOf() : ClockTypes.None;

    if (this.model.cachedOriginalModel && this.model.cachedOriginalModel.tick.tickType === cType) {
      this.model.tick.clockTypes = this.model.cachedOriginalModel.tick.clockTypes;
    } else {
      // set federal indicator based on "Normal status"
      if (!ClockType.any(cType, ClockTypes.TEMP) && ClockType.any(cType, ClockTypes.PlacementLimit | ClockTypes.CMC | ClockTypes.OPC | ClockTypes.OTHER)) {
        cType |= ClockTypes.Federal; //Add Federal flag
      }

      //set state indicator based on "Normal status"
      if (!ClockType.eql(cType, ClockTypes.None)) {
        cType |= ClockTypes.State;
      }

      if (!this.canCountTowardPlacementLimit && ClockType.any(cType, ClockTypes.PlacementLimit)) {
        cType |= ClockTypes.NoPlacementLimit; //Add NoPlacement Limit flag
      }
    }

    this.model.tick.clockTypes = new ClockType(cType);
    
    // setTimeout(_=>{
    //   this.validate();
    // },10)
  }

  togglePlacementIndicator(val: boolean) {
    let cType = this.model.tick.clockTypes.valueOf();
    if (val) {
      cType |= ClockTypes.NoPlacementLimit; //Add placement flag
    } else {
      cType &= ~ClockTypes.NoPlacementLimit; //Remove placement flag
    }
  }

  toggleStateIndicator(val: boolean) {
    let cType: ClockTypes = this.model.tick.clockTypes.valueOf();
    if (val) {
      cType |= ClockTypes.State; //Add State flag
    } else {
      cType &= ~ClockTypes.State; //Remove State Flag
    }
    this.model.tick.clockTypes = new ClockType(cType);
  }

  toggleFederalIndicator(val: boolean) {
    let cType: ClockTypes = this.model.tick.clockTypes.valueOf();
    if (val) {
      cType |= ClockTypes.Federal; //Add Federal flag
    } else {
      cType &= ~ClockTypes.Federal; //Remove Federal Flag
    }
    this.model.tick.clockTypes = new ClockType(cType);
  }

  // reasonDetailsChanged(details: string) {
  //   this.reasonDetailsEmitter.next(details);
  // }

  notesChanged(notes: string) {
    this.notesEmitter.next(notes);
  }

  public get canSave() {
    if (this.model) {
      if (this.flagsAreChanged) {
        let timeLimitType = this.model.tick.clockTypes.val;
        let reasonChange = this.model.reasonForChange;
        let reasonChangeDetails = this.model.reasonForChangeDetails;
        if(((timeLimitType==0 && this.model.reasonForChange != 0) || timeLimitType) && reasonChange && reasonChangeDetails)
        {
          return this.isModelValid;
        }
        else
        {
          return false;
        }
      }else {
        return false;
      }
    }
    return false;
  }

  ngOnDestroy() {
    if (this.modelSub != null) {
      this.modelSub.unsubscribe();
    }
    if (this.notesSub != null) {
      this.notesSub.unsubscribe();
    }
    this.exit();
  }

  saveAndExit() {
    this.isSaving = true;
    this.hadSaveError = false;

    if (this.modelSub != null) {
      this.modelSub.unsubscribe();
      this.modelSub = null;
    }
    let timelineMonths: TimelineMonth[] = [this.model];
    let saveObservable: Observable<TimelineMonth[]>;
    //save months, then emit original so timeline bindings refresh
    if (this.isOTF && this.selectedMonths.length > 0) {
      this.selectedMonths.map(x => {
        const modelCopy = this.model.clone();
        modelCopy.date = x.startOf('month');
        timelineMonths.push(modelCopy);
      });
      // Call the service to save the data.
      saveObservable = this._timeLimitsService.saveMonths(this.pin, timelineMonths);
    } else {
      // Call the service to save the data.
      saveObservable = this._timeLimitsService.saveMonth(this.pin, this.model);
    }
    this.modelSub = saveObservable.pipe(take(1))
      .pipe(
        catchError((o, b) => this.handleSaveError(o, b)),
        finalize(() => this.isSaving = false)
      )
      .subscribe(months => {
        // TODO: Emit actually month(s) from server
        (<any>this.save).emit(months);
        // this.save.emit(timelineMonths);
        this.exit();
      });
  }

  private handleSaveError(error: ErrorInfo | Error, caught: Observable<TimelineMonth[]>) {
    this.hadSaveError = true;
    this.errorMessage = error.message;
    return empty();
  }

  exit() {
    this.hadSaveError = false;
    this.errorMessage = null;
    this.isModelValid = false;
    this.isSaving = false;
    this.selectedMonths.length = 0;
    this.close.emit();
  }

  validate() {
    const result = this.model.validate(this.validationManager);
    this.isModelValid = result.isValid;
    let isStateOrFederal = this.model.tick.clockTypes.state.Federal || this.model.tick.clockTypes.state.State;
    if (this.model.tick.tickType === ClockTypes.CMC && !isStateOrFederal) this.isModelValid = false;
    this.modelErrors = result.errors;
  }

  private isValidClockTypeForDate(clockType: ClockTypes) {
    if (!this.model || !this.model.date || this.model.tick.clockTypes.any(clockType)) {
      return true;
    }

    if (ClockType.has(clockType, ClockTypes.JOBS)) {
      return this.model.date.isBetween('1996-10-01', '1998-03-31', 'month', '()');
    }
    if (ClockType.has(clockType, ClockTypes.TJB)) {
      return this.model.date.isSameOrBefore('2014-07-31', 'month');
    }
    if (ClockType.any(clockType, ClockTypes.TEMP)) {
      return this.model.date.isSameOrAfter('2016-03-01', 'month');
    }

    return true;
  }
}
