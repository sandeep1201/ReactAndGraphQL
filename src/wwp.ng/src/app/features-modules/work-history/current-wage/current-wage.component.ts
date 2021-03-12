// tslint:disable: import-blacklist
// tslint:disable: no-use-before-declare
import { Component, OnInit, forwardRef, Input, Output, EventEmitter, OnDestroy, SimpleChanges, OnChanges } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { Subscription } from 'rxjs';

import * as moment from 'moment';

import { FieldDataService } from '../../../shared/services/field-data.service';
import { DropDownField } from '../../../shared/models/dropdown-field';
import { Employment, WageHour, WorkHistoryIdentities } from '../../../shared/models/work-history-app';
import { Utilities } from '../../../shared/utilities';
import { ModelErrors } from '../../../shared/interfaces/model-errors';

const noop = () => {};

export const WorkHistoryWageComponent_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => WorkHistoryWageComponent),
  multi: true
};

@Component({
  selector: 'app-work-history-wage',
  templateUrl: './current-wage.component.html',
  styleUrls: ['./current-wage.component.css'],
  providers: [WorkHistoryWageComponent_VALUE_ACCESSOR]
})
export class WorkHistoryWageComponent implements ControlValueAccessor, OnInit, OnChanges, OnDestroy {
  @Input() currentJobCategory: string;
  public isCurrentJobAtCreation: boolean;
  // Effective Date from the data for an already saved record
  @Input() effectiveDate: boolean;
  @Input() modelIds: WorkHistoryIdentities;
  @Input() modelWorkHistory: Employment;
  @Input() isNewRecord = false;
  @Output() modelChange = new EventEmitter();
  @Output() moveToHistory = new EventEmitter();
  @Input() modelErrors: ModelErrors = {};
  @Input() employerOfRecordSelectedValue: string;
  @Input() jobTypeId: number;
  @Input() isHourlySubsidyDisabled = true;
  private ptSub: Subscription;
  private itSub: Subscription;
  public payTypesDrop: DropDownField[];
  public intervalTypesDrop: DropDownField[];
  public model: WageHour;
  public originalModel: WageHour;
  private otherPayTypeId: number;
  private noPayPayTypeId: number;
  private isModelLoaded = false;
  public date10DaysAfterCurrent: moment.Moment;
  private onTouchedCallback: () => void = noop;
  private onChangeCallback: (_: any) => void = noop;
  public isReadOnly = false;
  filteredIntervalTypes: DropDownField[];

  constructor(private fdService: FieldDataService) {}

  ngOnInit() {
    this.ptSub = this.fdService.getWageTypes().subscribe(data => this.initPayTypes(data));
    this.itSub = this.fdService.getIntervalTypes().subscribe(data => this.initIntervalTypes(data));
    this.date10DaysAfterCurrent = Utilities.currentDate.add(10, 'days');
    if (this.currentJobCategory === 'currentJob' && this.isNewRecord === false) {
      this.isReadOnly = true;
    }
  }

  initPayTypes(data) {
    this.otherPayTypeId = Utilities.idByFieldDataName('Other', data);
    this.noPayPayTypeId = Utilities.idByFieldDataName('No Pay', data);
    this.payTypesDrop = data;
  }

  initIntervalTypes(data) {
    this.intervalTypesDrop = data;
    this.filterPayRateIntervalBasedOnJobType();
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
        this.originalModel = new WageHour();
        WageHour.clone(this.model, this.originalModel);
      }

      if (
        this.modelWorkHistory != null &&
        moment(this.modelWorkHistory.jobBeginDate, 'MM/DD/YYYY').isValid() &&
        this.model != null &&
        this.isModelLoaded === false &&
        this.isNewRecord === true
      ) {
        this.model.currentEffectiveDate = this.modelWorkHistory.jobBeginDate;

        this.isModelLoaded = true;
      } else if (
        this.modelWorkHistory != null &&
        moment(this.modelWorkHistory.jobBeginDate, 'MM/DD/YYYY').isValid() &&
        this.model != null &&
        this.isModelLoaded === false &&
        this.isNewRecord === false
      ) {
        this.isCurrentJobAtCreation = this.modelWorkHistory.isCurrentJobAtCreation;
        this.isModelLoaded = true;
      }
    }
  }

  registerOnChange(fn: any) {
    this.onChangeCallback = fn;
  }

  registerModelChange(fn: any) {
    this.modelChange.emit();
  }

  registerOnTouched(fn: any) {
    this.onTouchedCallback = fn;
  }

  isCurrentPayDetailsRequired(): boolean {
    return this.model.isCurrentPayTypeDetailsRequired(this.otherPayTypeId);
  }

  isCurrentPayRateDisabled(): boolean {
    return this.model.isCurrentPayRateDisabled(this.modelWorkHistory.jobTypeId, this.modelIds.noPayPayTypeId, this.modelIds.volunteerJobTypeId);
  }

  isPayTypeHidden(): boolean {
    // Currently the same for isCurrentPayRateDisabled.
    return this.model.isCurrentPayTypeDisabled(this.modelWorkHistory.jobTypeId, this.modelIds.volunteerJobTypeId);
  }

  whichJobCategory(): string {
    return this.currentJobCategory;
  }

  moveCurrentWageToHistory() {
    this.moveToHistory.emit();
    this.originalModel = new WageHour();
    WageHour.clone(this.model, this.originalModel);
    this.isReadOnly = false;
  }

  eraseCurrentWage() {
    this.model.eraseCurrentWage();
  }

  isPayDetailsRequired(): boolean {
    return this.model.isCurrentPayTypeDetailsRequired(this.modelIds.otherPayTypeId);
  }

  isBeginningPayDisabled(): boolean {
    return this.model.isBeginningPayDisabled(this.modelWorkHistory.jobTypeId, this.modelIds.volunteerJobTypeId, this.noPayPayTypeId);
  }

  isEndPayDisabled(): boolean {
    return this.model.isEndPayDisabled(this.modelWorkHistory.jobTypeId, this.modelIds.volunteerJobTypeId, this.noPayPayTypeId);
  }

  isUnchangedPastPayRateIndicatorDisabled(): boolean {
    if (this.model.isUnchangedPastPayRateIndicatorDisabled(this.modelWorkHistory.jobTypeId, this.modelIds.volunteerJobTypeId, this.noPayPayTypeId)) {
      return true;
    }
  }
  isTJOrTMJSubsidizedJob(jobTypeId: number, tjSubsidizedId: number, tmjSubsidizedId: number) {
    if (!!this.model && !!jobTypeId && !!tjSubsidizedId && !!tmjSubsidizedId) {
      return this.model.isTJOrTMJSubsidizedJob(jobTypeId, tjSubsidizedId, tmjSubsidizedId);
    }
  }

  filterPayRateIntervalBasedOnJobType() {
    if (this.isTJOrTMJSubsidizedJob(this.jobTypeId, this.modelIds.tjSubsidizedId, this.modelIds.tmjSubsidizedId) && this.intervalTypesDrop.length > 0) {
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

  ngOnDestroy() {
    if (this.itSub != null) {
      this.itSub.unsubscribe();
    }
    if (this.ptSub != null) {
      this.ptSub.unsubscribe();
    }
  }
}
