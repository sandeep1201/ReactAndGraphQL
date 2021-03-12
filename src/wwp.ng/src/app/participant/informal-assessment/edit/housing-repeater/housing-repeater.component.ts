import { AppService } from './../../../../core/services/app.service';
// tslint:disable: no-use-before-declare
// tslint:disable: no-output-on-prefix
import { Component, forwardRef, OnInit, Input, Output, EventEmitter, OnChanges } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { FieldDataService } from '../../../../shared/services/field-data.service';
import { DropDownField } from '../../../../shared/models/dropdown-field';
import { HousingSection, HousingHistory } from '../../../../shared/models/housing-section';
import { YesNo } from '../../../../shared/models/primitives';
import { ValidationError } from '../../../../shared/models/validation-error';
import { ValidationManager } from '../../../../shared/models/validation-manager';
import { ModelErrors } from '../../../../shared/interfaces/model-errors';
import { Utilities } from '../../../../shared/utilities';
import { ParticipantService } from '../../../../shared/services/participant.service';
import { InformalAssessmentEditService } from '../../../../shared/services/informal-assessment-edit.service';
const noop = () => {};

export const HousingHistoryRepeaterComponent_CONTROL_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => HousingRepeaterComponent),
  multi: true
};

@Component({
  selector: 'app-housing-repeater',
  templateUrl: './housing-repeater.component.html',
  styleUrls: ['./housing-repeater.component.css'],
  providers: [HousingHistoryRepeaterComponent_CONTROL_VALUE_ACCESSOR]
})
export class HousingRepeaterComponent implements OnInit, ControlValueAccessor, OnChanges {
  public evictedDrop: DropDownField[] = [];
  public HousingSection: HousingSection;
  private indexOfFocusedHousingHistory: number;
  private otherHousingSituationId: number;
  private homelessHousingSituationId: number;
  private indexOfFocusedHousingHistoryCurrency: number;
  private isHistoricalHousingEditable = false;
  public isHistoricalHousingSectionValid = true;
  public validationManager: ValidationManager = new ValidationManager(this.appService);
  public modelErrors: ModelErrors[] = [];

  // Placeholders for the callbacks which are later providesd
  // by the Control Value Accessor
  public housingHistories: HousingHistory[];

  private onChangeCallback: (_: any) => void = noop;

  private onTouchedCallback: () => void = noop;

  @Input() participantDOB: any;
  @Input() housingSituationsDrop: DropDownField[];
  @Output() onChange = new EventEmitter<boolean>();
  @Output() onHistoricalEdit = new EventEmitter<boolean>();
  @Input() validationErrors: ValidationError[] = [];

  constructor(private appService: AppService, private eiaService: InformalAssessmentEditService, private fdService: FieldDataService, private partService: ParticipantService) {}

  ngOnInit() {
    const p = new YesNo();
    this.evictedDrop = p.YesOrNo;
  }

  ngOnChanges() {
    if (this.housingSituationsDrop != null) {
      this.otherHousingSituationId = Utilities.idByFieldDataName('Other', this.housingSituationsDrop);
      this.homelessHousingSituationId = Utilities.idByFieldDataName('Homeless (Outside of shelter)', this.housingSituationsDrop);
    }
  }

  get value(): any {
    return this.housingHistories;
  }

  set value(v: any) {
    if (v !== this.housingHistories) {
      this.housingHistories = v;
      this.onChangeCallback(v);
    }
  }

  isIE(): boolean {
    const x = navigator.appVersion.indexOf('Trident') !== -1;
    return x;
  }

  // Set touched on blur
  onBlur() {
    this.onTouchedCallback();
  }

  // From ControlValueAccessor interface
  writeValue(value: any) {
    if (value !== this.housingHistories) {
      this.housingHistories = value;
    }
  }

  // From ControlValueAccessor interface
  registerOnChange(fn: any) {
    this.onChangeCallback = fn;
  }

  // From ControlValueAccessor interface
  registerOnTouched(fn: any) {
    this.onTouchedCallback = fn;
  }

  isDisabled(rowIndex: number) {
    if (rowIndex === this.indexOfFocusedHousingHistory) {
      return false;
    } else {
      return true;
    }
  }

  SetIndex(rowIndex: number) {
    this.indexOfFocusedHousingHistoryCurrency = rowIndex;
  }

  isUnknownHidden(rowIndex: number) {
    if (rowIndex === this.indexOfFocusedHousingHistoryCurrency) {
      return false;
    } else {
      return true;
    }
  }

  isEvictedDisabled(hh: HousingHistory) {
    return hh.isEvictedDisabled(this.homelessHousingSituationId);
  }

  isDetailsRequired(hh: HousingHistory) {
    return hh.isDetailsRequired(this.otherHousingSituationId);
  }

  editHistoricalHousingRecord(hh: HousingHistory) {
    //  this.validate(hh);
    if (this.isHistoricalHousingEditable === false) {
      // if (this.isHistoricalHousingSectionValid === true) {
      const editItemIndex = this.housingHistories.indexOf(hh);
      this.indexOfFocusedHousingHistory = editItemIndex;
      this.isHistoricalHousingEditable = true;
      this.onHistoricalEdit.emit(true);
    }
  }

  deleteHistoricalHousingRecord(hh: HousingHistory) {
    const deletedItemIndex = this.housingHistories.indexOf(hh);
    this.housingHistories.splice(deletedItemIndex, 1);
    this.onHistoricalEdit.emit(false);
    // We have to trigger error model changes from repeater.
    this.validate(hh);
    this.isHistoricalHousingEditable = false;
    this.checkState();
  }

  confirmAndAddHistoricalHousingRecord(hh: HousingHistory) {
    this.validate(hh);
    if (this.isHistoricalHousingSectionValid === true) {
      this.indexOfFocusedHousingHistory = null;
      this.isHistoricalHousingEditable = false;
      this.onHistoricalEdit.emit(false);
    }
  }

  addHistoricalHousingRecord() {
    if (this.housingHistories != null && this.isHistoricalHousingEditable === false) {
      const hh = new HousingHistory();
      hh.id = 0;
      this.housingHistories.push(hh);
      this.editHistoricalHousingRecord(hh);
    }
  }

  validate(hh: HousingHistory) {
    this.validationManager.resetErrors();
    this.modelErrors = [];
    this.isHistoricalHousingSectionValid = true;
    // Call the model's validate method.
    for (const hhh of this.housingHistories) {
      const result = hhh.validate(this.validationManager, this.otherHousingSituationId, this.homelessHousingSituationId, this.participantDOB.format('MM/DD/YYYY'));
      this.modelErrors.push(result.errors);
      if (result.isValid === false) {
        this.isHistoricalHousingSectionValid = false;
      }
    }

    // this.validationManagerHistoricalHousing
    // Update our properties so the UI can bind to the results.
    //this.isSectionValid = result.isValid;
  }

  isModelErrorsItemInvalid(i: number, property: string): boolean {
    const test = Utilities.isModelErrorsItemInvalid(this.modelErrors, i, property);
    return test;
  }
  isRepeaterRowRequired(i: number): boolean {
    return Utilities.isRepeaterRowRequired(this.housingHistories, i);
  }
  checkState() {
    if (this.isHistoricalHousingSectionValid === false) {
      if (this.housingHistories !== null) {
        for (const hh of this.housingHistories) {
          this.validate(hh);
        }
      }
    }
    this.onChange.emit(true);
  }
}
