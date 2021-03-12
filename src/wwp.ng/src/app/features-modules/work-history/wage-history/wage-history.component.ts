// tslint:disable: no-use-before-declare
import { Component, OnInit, forwardRef, Output, EventEmitter, Input, SimpleChanges, OnChanges } from '@angular/core';
import { ActivatedRoute, Router, RouterLinkWithHref } from '@angular/router';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { Subscription } from 'rxjs';
import * as moment from 'moment';

import { AppService } from './../../../core/services/app.service';
import { DropDownField } from '../../../shared/models/dropdown-field';
import { FieldDataService } from '../../../shared/services/field-data.service';
import { JobActionType } from '../../../shared/models/job-actions';
import { Utilities } from '../../../shared/utilities';
import { WageHourHistory, WorkHistoryIdentities, Employment } from '../../../shared/models/work-history-app';
import { ValidationManager } from '../../../shared/models/validation-manager';
import { ModelErrors } from '../../../shared/interfaces/model-errors';
import { BaseParticipantComponent } from '../../../shared/components/base-participant-component';
import { ParticipantService } from '../../../shared/services/participant.service';
import { Payload } from '../../../shared/models/primitives';

const noop = () => {};

export const WorkHistoryWageHistoryComponent_CONTROL_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => WorkHistoryWageHistoryComponent),
  multi: true
};

@Component({
  selector: 'app-work-history-wage-history',
  templateUrl: './wage-history.component.html',
  styleUrls: ['./wage-history.component.css'],
  providers: [WorkHistoryWageHistoryComponent_CONTROL_VALUE_ACCESSOR]
})
export class WorkHistoryWageHistoryComponent extends BaseParticipantComponent implements OnInit, ControlValueAccessor, OnChanges {
  @Output() modelChange = new EventEmitter<boolean>();
  @Input() jobBeginDate: string;
  @Input() jobEndDate: string;
  @Input() jobTypeId: number;
  @Input() isCurrentlyEmployed: boolean;
  @Input() readOnly = false;
  @Input() modelIds: WorkHistoryIdentities;
  @Input() modelWorkHistory: Employment;
  @Input() isNewRecord = false;
  @Input() allEffectiveDates: Payload;
  @Input() displayModifiedStamp = true;

  @Input() isHourlySubsidyDisabled = false;

  public model: WageHourHistory[] = [];
  public onTouchedCallback: () => void = noop;
  public onChangeCallback: (_: any) => void = noop;
  private isLoaded = false;
  private isInAutoValidation = false;

  private ptSub: Subscription;
  private priSub: Subscription;
  public payTypesDrop: DropDownField[];
  public intervalTypesDrop: DropDownField[];
  private otherPayTypeId: number;
  private indexOfOpenWageHourHistory: number;
  public validationManager: ValidationManager = new ValidationManager(this.appService);
  public modelErrors: ModelErrors = {};
  public indexOfFocusedWageHistory: number;

  public isWageHistoryInEdit = false;
  public isValid = true;
  filteredIntervalTypes: DropDownField[];

  constructor(private appService: AppService, private fdService: FieldDataService, route: ActivatedRoute, router: Router, partService: ParticipantService) {
    super(route, router, partService);
  }

  ngOnInit() {
    this.ptSub = this.fdService.getWageTypes().subscribe(data => this.initPayTypes(data));
    this.priSub = this.fdService.getIntervalTypes().subscribe(data => this.initIntervalTypes(data));
  }

  onPinInit() {
    // this.goBackUrl = '/pin/' + this.pin;
  }

  onParticipantInit() {
    // TODO: Add some Rxjs goodness to make sure both observables are complete (base class and here)
    // before setting isLoaded.
    this.isLoaded = true;
  }

  isIE(): boolean {
    const x = navigator.appVersion.indexOf('Trident') !== -1;
    return x;
  }

  initIntervalTypes(data) {
    this.intervalTypesDrop = data;
    this.filterPayRateIntervalBasedOnJobType();
  }
  initPayTypes(data) {
    this.otherPayTypeId = Utilities.idByFieldDataName('Other', data);
    this.payTypesDrop = data;
  }

  isDisabled(whh: WageHourHistory) {
    if (whh.isOpen) {
      return false;
    } else {
      return true;
    }
  }
  whichJobCategory() {
    if (this.modelWorkHistory != null) {
      return this.modelWorkHistory.whichJobCategory();
    } else {
      return false;
    }
  }

  isPayTypeHidden(whh: WageHourHistory): boolean {
    if (this.modelWorkHistory != null && this.modelIds != null) {
      return whh.isPayTypeHidden(this.modelWorkHistory.jobTypeId, this.modelIds.volunteerJobTypeId);
    }
  }

  isPayTypeDetailsRequired(whh: WageHourHistory): boolean {
    if (this.modelIds != null) {
      return whh.isPayTypeDetailsRequired(this.modelIds.otherPayTypeId);
    }
  }

  isPayRateDisabled(whh: WageHourHistory): boolean {
    if (this.modelWorkHistory != null && this.modelIds != null) {
      return whh.isPayRateDisabled(this.modelWorkHistory.jobTypeId, this.modelIds.noPayPayTypeId, this.modelIds.volunteerJobTypeId);
    }
  }

  addWageHistoryRecord() {
    if (this.model != null && this.isWageHistoryInEdit === false) {
      // If any record is open don't add new one.
      for (const m of this.model) {
        if (m.isOpen === true) {
          return;
        }
      }

      const whh = new WageHourHistory();
      const act = new JobActionType();
      if (this.model.length === 0 && this.isNewRecord === true) {
        whh.effectiveDate = this.jobBeginDate;
      }
      whh.historyPayType = act;
      whh.isOpen = true;
      this.model.push(whh);
    }
  }

  confirmWageHistoryRecord(whh: WageHourHistory) {
    whh.payRateIntervalName = Utilities.fieldDataNameById(whh.payRateIntervalId, this.intervalTypesDrop);
    const cs = this.modelWorkHistory.wageHour.calculateHourlyWage(whh);
    if (cs && cs.isCalculated) {
      whh.computedWageRateUnit = cs.units;
      whh.computedWageRateValue = cs.value;
    } else {
      whh.computedWageRateUnit = whh.payRateIntervalName;
      whh.computedWageRateValue = whh.payRate;
    }
    this.validate(whh);
    this.isInAutoValidation = true;
    if (this.isValid === true) {
      this.indexOfOpenWageHourHistory = null;
      this.isWageHistoryInEdit = false;
      this.isInAutoValidation = false;
      whh.isOpen = false;
      this.modelChange.emit(true);
    }
  }

  editWageHistoryRecord(whh: WageHourHistory) {
    if (this.isValid === true && this.isWageHistoryInEdit === false) {
      whh.isOpen = true;
      const editItemIndex = this.model.indexOf(whh);
      this.indexOfOpenWageHourHistory = editItemIndex;
      this.isWageHistoryInEdit = true;
      this.modelChange.emit(true);
    }
  }

  deleteWageHistoryRecord(whh: WageHourHistory) {
    if (whh.isOpen) {
      this.isValid = true;
      this.isWageHistoryInEdit = false;
      this.isInAutoValidation = false;
    }
    if (whh.isMovedFromCurrent) {
      whh.isDeletedFromCurrent = true;
      this.appService.wageHistory.next(whh);
    }
    const deletedItemIndex = this.model.indexOf(whh);
    this.model.splice(deletedItemIndex, 1);
    this.indexOfOpenWageHourHistory = null;
    this.modelChange.emit(false);
    this.validationManager.resetErrors();
    this.modelErrors = {};
  }

  registerModelChange(whh: WageHourHistory) {
    // this.onChangeCallback(whh);
    if (this.isInAutoValidation === true) {
      this.validate(whh);
    }
  }

  // Validation For model Object
  validate(whh: WageHourHistory) {
    // Clear all previous errors.
    this.validationManager.resetErrors();
    // Call the model's validate method.
    const result = whh.validate(
      this.validationManager,
      this.participantDOB,
      this.modelWorkHistory.jobTypeId,
      this.modelWorkHistory.isCurrentJobAtCreation,
      moment(this.jobBeginDate, 'MM/DD/YYYY'),
      moment(this.jobEndDate, 'MM/DD/YYYY'),
      this.isCurrentlyEmployed,
      this.allEffectiveDates,
      this.modelIds,
      this.isHourlySubsidyDisabled
    );

    this.isValid = result.isValid;
    this.modelErrors = result.errors;
  }

  isErrored(whh: WageHourHistory, modelName: string) {
    if (whh.isOpen) {
      return this.modelErrors[modelName];
    }
  }

  get value(): any {
    return this.model;
  }

  set value(v: any) {
    if (v !== this.model) {
      this.model = v;
      this.onChangeCallback(v);
    }
  }

  onBlur() {
    this.onTouchedCallback();
  }

  writeValue(value: any) {
    if (value !== this.model) {
      this.model = value;
      if (this.model != null) {
        this.model.sort(function(a, b) {
          const aDate = new Date(a.effectiveDate);
          const bDate = new Date(b.effectiveDate);
          return aDate > bDate ? -1 : aDate < bDate ? 1 : 0;
        });
      }
    }
  }

  registerOnChange(fn: any) {
    this.onChangeCallback = fn;
  }

  registerOnTouched(fn: any) {
    this.onTouchedCallback = fn;
  }
  filterPayRateIntervalBasedOnJobType() {
    if (
      !this.readOnly &&
      this.intervalTypesDrop &&
      this.model &&
      this.jobTypeId &&
      !!this.modelIds &&
      (this.jobTypeId === this.modelIds.tjSubsidizedId || this.jobTypeId === this.modelIds.tmjSubsidizedId)
    ) {
      this.filteredIntervalTypes = this.intervalTypesDrop.filter(i => i.name === 'Hour');
    } else {
      this.filteredIntervalTypes = null;
    }
  }
  ngOnChanges(changes: SimpleChanges) {
    for (const propName in changes) {
      if (changes.hasOwnProperty(propName)) {
        switch (propName) {
          case 'jobTypeId':
            this.filterPayRateIntervalBasedOnJobType();
            break;
          default:
            break;
        }
      }
    }
  }
}
