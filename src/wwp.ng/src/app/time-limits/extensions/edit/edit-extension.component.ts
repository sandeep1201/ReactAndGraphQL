import { ErrorInfo } from '../../../shared/models/ErrorInfoContract';
//import { isBuiltInAccessor } from '@angular/forms/src/directives/shared';
import { DropDownMultiField } from '../../../shared/models/dropdown-multi-field';
import { coerceBooleanProperty } from '../../../shared/decorators/boolean-property';
import { EnumEx, Utilities } from '../../../shared/utilities';
import { ExtensionReason, ExtensionSequence } from '../../../shared/models/time-limits/extension';
import { Component, OnInit, OnDestroy, EventEmitter, Input, Output, ChangeDetectionStrategy } from '@angular/core';
import { Extension, ExtensionDecision, ClockTypes, ClockType, Timeline, ClockStates, ClockState } from '../../../shared/models/time-limits';
import { DropDownField } from '../../../shared/models/dropdown-field';
import { ValidationManager } from '../../../shared/models/validation';
import { ModelErrors } from '../../../shared/interfaces/model-errors';
import { Subscription, Observable, forkJoin, empty } from 'rxjs';
import { catchError, finalize, take } from 'rxjs/operators';
import * as moment from 'moment';
import { TimeLimitsService } from '../../../shared/services/timelimits.service';
// import { ClockStates } from "../../../shared/models/time-limits";
import { AppService } from 'src/app/core/services/app.service';
import { DateRange } from '../../../shared/moment-range';
import { coerceNumberProperty } from '../../../shared/decorators/number-property';
import { ClockTypeNamePipe } from '../../pipes/clock-type-name.pipe';

@Component({
  selector: 'app-edit-extension',
  templateUrl: './edit-extension.component.html',
  styleUrls: ['./edit-extension.component.css'],
  providers: [TimeLimitsService, ValidationManager],
  changeDetection: ChangeDetectionStrategy.Default
})
export class EditExtensionComponent implements OnInit, OnDestroy {
  private _modelCopy: Extension;
  private modelSub: Subscription;
  private _timeline: Timeline;
  public cachedClockType: ClockType;
  @Input() pin: string;

  @Input() get model(): Extension {
    return this._modelCopy;
  }

  set model(val: Extension) {
    if (val != null && val instanceof Extension) {
      this._modelCopy = val.clone();
      this._modelCopy.id = 0;
      this.initTimelimitsTypeDrop();
      this.clockType = val.clockType;
    } else {
      this._modelCopy = null;
    }
  }

  @Input() get timeline(): Timeline {
    return this._timeline;
  }

  set timeline(val) {
    this._timeline = val;
    if (this.timeline != null) {
      this.initTimelimitsTypeDrop();
    }
  }

  @Output() close = new EventEmitter();
  @Output() save = new EventEmitter<ExtensionSequence>();

  public isModelValid = false;
  public isSaving = false;
  public modelErrors: ModelErrors = {};
  public hadSaveError: boolean = false;
  public errorMessage: string = '';
  public canCreateForm = false;
  public extensionMonths: moment.Moment[] = [];
  public extensionDecision = ExtensionDecision;
  public ApprovalExtensionReasons: Map<number, ExtensionReason>;
  public DenialExtensionReasons: Map<number, ExtensionReason>;
  public extensionDecisions: DropDownMultiField[] = [];

  public timelimitsTypeDrop: DropDownField[];
  public denialTimelimitsTypeDrop: DropDownField[];
  public approvalReasonDrop: DropDownField[] = [];
  public denialReasonDrop: DropDownField[] = [];
  public cmcExtensionLengthDrop: DropDownField[] = [];

  get clockType(): ClockType {
    if (this.model && this.model.clockType != null) {
      return this.model.clockType;
    }
    return new ClockType(ClockTypes.None);
  }

  set clockType(val) {
    if (!this.cachedClockType) this.cachedClockType = new ClockType(coerceNumberProperty(val, 0));
    this.model.clockType = new ClockType(coerceNumberProperty(val, 0));
    this.updateExtensionDateRange();
    this.loadExtensionReasonDrops();
  }

  get exDecision() {
    return this.model.decision;
  }

  set exDecision(val) {
    this.model.decision = +val;
  }

  get approvalReason() {
    return this.model.approvalReasonId;
  }
  set approvalReason(val) {
    val = coerceNumberProperty(val);
    this.model.approvalReasonId = val ? val : null;
  }

  get denailReason() {
    return this.model.denialReasonId;
  }
  set denialReasonId(val) {
    val = coerceNumberProperty(val);
    this.model.denialReasonId = val ? val : null;
  }

  get stateOnly(): boolean {
    return this.timelimitsTypeDrop != null && this.timelimitsTypeDrop.length === 1 && this.timelimitsTypeDrop[0].id === ClockTypes.State;
  }

  public additionalClockWarnings: { clockTypeName: string; dateRange: DateRange }[] = [];

  get extensionLength() {
    return this.extensionMonths.length;
  }

  public get isCMCExtension() {
    if (this.model) {
      if (this.model.decision === ExtensionDecision.Approve && this.ApprovalExtensionReasons.has(this.model.approvalReasonId)) {
        let approvalReason = this.ApprovalExtensionReasons.get(this.model.approvalReasonId);
        return approvalReason.name.toLowerCase().trim() === 'cmc extension';
      }
    }
    return false;
  }

  public get isBackdated() {
    return this.model && this.model.isBackdated;
  }
  public set isBackdated(val: boolean) {
    val = coerceBooleanProperty(val);
    this.model.isBackdated = val;

    this.updateExtensionDateRange();
  }

  get canBackdate() {
    if (this.model && this.timeline && this.model.dateRange) {
      if (this.isBackdated) {
        return true;
      }

      if (!this.model.dateRange.start.isSame(Utilities.currentDate, 'month')) {
        return false;
      }

      let previousMonth = this.model.dateRange.start.clone().subtract(1, 'month');
      let existingExt = this.timeline
        .getExtensions(ExtensionDecision.Approve)
        .filter(x => !x.isDeleted && x.clockType.valueOf() === this.clockType.valueOf() && x.dateRange.contains(previousMonth));
      return existingExt.length < 1;
    }
  }

  get cmcExtensionLength() {
    return coerceNumberProperty(this._cmcExtensionLength);
  }

  set cmcExtensionLength(val: number) {
    this._cmcExtensionLength = coerceNumberProperty(val, 3);
  }

  // // // switch the Dates based on the time-limit type selected
  // // public startDate: moment.Moment;
  // // public endDate: moment.Moment;

  // public get discussionDate(): string {
  //   if (this.model && this.model.discussionDate) {
  //     return this.model.discussionDate.format('MM/DD/YYYY');
  //   }
  //   return '';
  // }
  // public set discussionDate(val) {
  //   let date = moment(val, 'MM/DD/YYYY');
  //   if (!val || val.toString() === '' || !date.isValid()) {
  //     this.model.discussionDate = null;
  //   }

  //   this.model.discussionDate = date;
  // }

  private clockTypeNamePipe: ClockTypeNamePipe;
  private _cmcExtensionLength: number = 3;

  constructor(private _timeLimitsService: TimeLimitsService, public validationManager: ValidationManager) {
    this.clockTypeNamePipe = new ClockTypeNamePipe();
  }

  ngOnInit() {
    let $approval = this._timeLimitsService.getExtensionReasons('Approval').pipe(take(1));

    let $denial = this._timeLimitsService.getExtensionReasons('Denial').pipe(take(1));

    forkJoin($denial, $approval)
      .pipe(take(1))
      .subscribe(arr => {
        this.DenialExtensionReasons = arr[0];
        this.ApprovalExtensionReasons = arr[1];
        // this.loadApprovalReasonDrop();
        // this.loadDenialReasonDrop();
      });

    this.cmcExtensionLengthDrop.push(new DropDownField(1, '1 Month', false));
    this.cmcExtensionLengthDrop.push(new DropDownField(2, '2 Months', false));
    this.cmcExtensionLengthDrop.push(new DropDownField(3, '3 Months', false));
  }

  initTimelimitsTypeDrop() {
    this.timelimitsTypeDrop = [];
    this.denialTimelimitsTypeDrop = [];
    let clockStates = this.timeline.clockStates;
    // If we are appending to a sequence, only allow one clock type
    if (this.model && this.model.sequenceId > 0) {
      let clockState = new ClockState(clockStates.get(this.model.clockType.valueOf()));
      if (clockState.state.CanBeExtended) {
        this.timelimitsTypeDrop.push(new DropDownField(this.clockType.valueOf(), this.clockTypeNamePipe.transform(this.clockType.valueOf()), true));
      }
      if (clockState.state.SequenceCanBeDenied || clockState.state.WillCauseGapExtension || clockState.state.CanBeExtended) {
        this.denialTimelimitsTypeDrop.push(new DropDownField(this.clockType.valueOf(), this.clockTypeNamePipe.transform(this.clockType.valueOf()), true));
      }
    } else {
      let clockTypes: ClockTypes[] = Array.from(clockStates.keys());

      clockTypes.map(key => {
        if (ClockType.any(key, ClockTypes.ExtensableTypes)) {
          if (key !== ClockTypes.State) {
            // ignore state for now
            let clockState = new ClockState(clockStates.get(key)) || new ClockState();
            if (!clockState.state.None) {
              if (clockState.state.CanBeExtended) {
                this.timelimitsTypeDrop.push(new DropDownField(key, this.clockTypeNamePipe.transform(key)));
                this.denialTimelimitsTypeDrop.push(new DropDownField(key, this.clockTypeNamePipe.transform(key)));
              }
              if (clockState.state.WillCauseGapExtension) {
                this.additionalClockWarnings.push({ clockTypeName: this.clockTypeNamePipe.transform(key), dateRange: null });
              }
            }
          }
        }
      });

      // add state timelimt type if needed
      let stateClockState = new ClockState(clockStates.get(ClockTypes.State));

      if (stateClockState.state.CanBeExtended) {
        this.timelimitsTypeDrop.push(new DropDownField(ClockTypes.State, ClockTypes[ClockTypes.State]));
      }
      if (stateClockState.state.SequenceCanBeDenied) {
        this.denialTimelimitsTypeDrop.push(new DropDownField(ClockTypes.State, ClockTypes[ClockTypes.State]));
      }
    }

    this.timelimitsTypeDrop.sort((a, b) => {
      return +a.id - +b.id;
    });

    this.denialTimelimitsTypeDrop.sort((a, b) => {
      return +a.id - +b.id;
    });

    this.initDecisionsSelector();
  }

  public initDecisionsSelector() {
    this.extensionDecisions = [];
    const approvalItem: DropDownMultiField = {
      id: ExtensionDecision.Approve,
      isSelected: false,
      disablesOthers: false,
      name: ExtensionDecision[ExtensionDecision.Approve],
      isDisabled: this.timelimitsTypeDrop.length < 1
    };

    const denialItem: DropDownMultiField = {
      id: ExtensionDecision.Deny,
      isSelected: false,
      name: ExtensionDecision[ExtensionDecision.Deny],
      disablesOthers: false,
      isDisabled: this.denialTimelimitsTypeDrop.length < 1
    };

    this.extensionDecisions.push(denialItem);
    this.extensionDecisions.push(approvalItem);
  }

  loadExtensionReasonDrops() {
    this.loadApprovalReasonDrop();
    this.loadDenialReasonDrop();
  }
  loadApprovalReasonDrop() {
    this.approvalReasonDrop = [];
    for (let el of Array.from(this.ApprovalExtensionReasons.values())) {
      if (this.clockType.any(el.validFor)) {
        this.approvalReasonDrop.push(new DropDownField(el.id, el.name));
      }
    }
    // TODO: If model has a different value the a valid one, NULL it out!
  }

  loadDenialReasonDrop() {
    this.denialReasonDrop = [];
    for (let el of Array.from(this.DenialExtensionReasons.values())) {
      if (this.clockType.any(el.validFor)) {
        this.denialReasonDrop.push(new DropDownField(el.id, el.name));
      }
    }
    // TODO: If model has a different value the a valid one, NULL it out!
  }

  updateExtensionDateRange() {
    if (this.timeline && this.model) {
      if (this.model.decision === ExtensionDecision.Approve && this.model.clockType != null && !this.model.clockType.state.None) {
        const maxMonths = this.isCMCExtension ? this.cmcExtensionLength : null;

        let isCurrentMonthTicked = false;
        this.timeline.getTicks(this.model.clockType.valueOf()).filter(tick => {
          if (tick.isCurrentMonth === true && !tick.tick.clockTypes.state.None) isCurrentMonthTicked = true;
        });
        const dateRange = this.timeline.getExtensionDateRange(this.model.clockType.valueOf(), maxMonths, isCurrentMonthTicked, this.model.isBackdated);

        if (dateRange && !dateRange.isSame(this.model.dateRange, 'months')) {
          this.model.dateRange = dateRange;
          this.extensionMonths = Array.from(this.model.dateRange.by('month'));
        }
      } else {
        this.model.dateRange = null;
      }
    }
  }

  validate() {
    // Clear all previous errors.
    this.validationManager.resetErrors();

    const result = this.model.validate(this.validationManager);
    this.isModelValid = result.isValid;
    this.modelErrors = result.errors;
  }

  saveAndExit() {
    this.validate();
    if (!this.isModelValid) {
      return false;
    }
    this.isSaving = true;
    this.hadSaveError = false;

    if (this.modelSub != null) {
      this.modelSub.unsubscribe();
      this.modelSub = null;
    }

    // Call the service to save the data.
    this.modelSub = this._timeLimitsService
      .saveExtension(this.pin, this.model)
      .pipe(take(1))
      .pipe(
        catchError((o, b) => this.handleSaveError(o, b)),
        finalize(() => (this.isSaving = false))
      )
      .subscribe(data => {
        this.hadSaveError = false;

        (<any>this.save).emit(data);
        this.exit();
        // this.initModel(data);
      });
  }

  exit() {
    this.isSaving = false;
    this.isModelValid = false;
    this.hadSaveError = false;
    this.errorMessage = null;
    // this.isBackdated = false;
    this.model = null;
    this.close.emit();
  }

  ngOnDestroy() {
    if (this.modelSub != null) {
      this.modelSub.unsubscribe();
    }
  }

  private handleSaveError(error: ErrorInfo | Error, caught: Observable<ExtensionSequence>) {
    this.hadSaveError = true;
    this.errorMessage = error.message;
    return empty();
  }
}
