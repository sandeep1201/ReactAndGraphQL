// tslint:disable: no-use-before-declare
import { Component, OnInit, forwardRef, Input, Output, EventEmitter } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';

import { Subscription } from 'rxjs';
import { FieldDataService } from '../../../shared/services/field-data.service';
import { DropDownField } from '../../../shared/models/dropdown-field';
import { ModelErrors } from '../../../shared/interfaces/model-errors';
import { WageHour, Employment, WorkHistoryIdentities } from '../../../shared/models/work-history-app';

import { Utilities } from '../../../shared/utilities';

const noop = () => {};

export const WorkHistoryPastWageComponent_CONTROL_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => WorkHistoryPastWageComponent),
  multi: true
};

@Component({
  selector: 'app-work-history-past-wage',
  templateUrl: './past-wage.component.html',
  styleUrls: ['./past-wage.component.css'],
  providers: [WorkHistoryPastWageComponent_CONTROL_VALUE_ACCESSOR]
})
export class WorkHistoryPastWageComponent implements ControlValueAccessor, OnInit {
  @Output() modelChange = new EventEmitter();
  @Input() readOnly = false;
  @Input() modelWorkHistory: Employment;
  @Input() modelIds: WorkHistoryIdentities;
  @Input() volunteerId: number;
  @Input() modelErrors: ModelErrors = {};

  private ptSub: Subscription;
  private itSub: Subscription;
  private otherPayTypeId: number;
  private noPayPayTypeId: number;
  public intervalTypesDrop: DropDownField[];
  public payTypesDrop: DropDownField[];
  public model: WageHour;
  public originalModel: WageHour = new WageHour();
  private onTouchedCallback: () => void = noop;
  private onChangeCallback: (_: any) => void = noop;

  constructor(private fdService: FieldDataService) {}

  ngOnInit() {
    this.ptSub = this.fdService.getWageTypes().subscribe(data => this.initPayTypes(data));
    this.itSub = this.fdService.getIntervalTypes().subscribe(data => this.initIntervalTypes(data));
  }

  initPayTypes(data) {
    this.otherPayTypeId = Utilities.idByFieldDataName('Other', data);
    this.noPayPayTypeId = Utilities.idByFieldDataName('No Pay', data);
    this.payTypesDrop = data;
  }

  initIntervalTypes(data) {
    this.intervalTypesDrop = data;
  }

  isPayTypeHidden(): boolean {
    // Currently the same for isCurrentPayRateDisabled.
    return this.model.isCurrentPayTypeDisabled(this.modelWorkHistory.jobTypeId, this.modelIds.volunteerJobTypeId);
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

  get value(): any {
    return this.model;
  }

  set value(v: any) {
    if (v !== this.model) {
      this.model = v;
    }
  }

  // onBlur() {
  //   this.onTouchedCallback();
  // }

  writeValue(value: any) {
    if (value !== this.model) {
      this.model = value;
      if (this.model != null) {
        this.originalModel = WageHour.create();
        WageHour.clone(this.model, this.originalModel);
      }
    }
  }

  registerOnChange(fn: any) {
    this.onChangeCallback = fn;
  }

  registerModelChange() {
    this.modelChange.emit();
  }

  registerOnTouched(fn: any) {
    this.onTouchedCallback = fn;
  }
}
