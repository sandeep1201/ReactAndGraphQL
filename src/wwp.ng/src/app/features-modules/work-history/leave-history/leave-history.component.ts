// tslint:disable: no-use-before-declare
// tslint:disable: no-output-on-prefix
import { Component, forwardRef, OnInit, EventEmitter, Output, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { Subscription } from 'rxjs';
import * as moment from 'moment';

import { Absence } from '../../../shared/models/work-history-app';
import { AppService } from './../../../core/services/app.service';
import { DropDownField } from '../../../shared/models/dropdown-field';
import { FieldDataService } from '../../../shared/services/field-data.service';
import { Utilities } from '../../../shared/utilities';
import { ValidationManager } from '../../../shared/models/validation-manager';
import { ModelErrors } from '../../../shared/interfaces/model-errors';
import { BaseParticipantComponent } from '../../../shared/components/base-participant-component';
import { ParticipantService } from '../../../shared/services/participant.service';

const noop = () => {};

export const WorkHistoryLeaveHistoryComponent_CONTROL_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => WorkHistoryLeaveHistoryComponent),
  multi: true
};

@Component({
  selector: 'app-work-history-leave-history',
  templateUrl: './leave-history.component.html',
  styleUrls: ['./leave-history.component.css'],
  providers: [WorkHistoryLeaveHistoryComponent_CONTROL_VALUE_ACCESSOR]
})
export class WorkHistoryLeaveHistoryComponent extends BaseParticipantComponent implements ControlValueAccessor, OnInit {
  @Output() onAbsenceEdit = new EventEmitter<boolean>();
  @Output() modelChange = new EventEmitter();
  @Input() jobBeginDate: string;
  @Input() jobEndDate: string;
  @Input() isCurrentlyEmployed: boolean;
  @Input() readOnly = false;
  @Input() displayModifiedStamp = true;

  private lrSub: Subscription;
  public model: Absence[] = [];
  private isInAutoValidation = false;

  private jobAbsenceReasonsDrop: DropDownField[];
  public validationManager: ValidationManager = new ValidationManager(this.appService);
  public modelErrors: ModelErrors = {};

  private isAbsenceInEdit = false;
  private isValid = true;
  private isLoaded = false;
  private indexOfOpenAbsence: number;
  private strikeId: number;
  private otherAbsenceReasonId: number;

  constructor(private appService: AppService, private fdService: FieldDataService, route: ActivatedRoute, router: Router, partService: ParticipantService) {
    super(route, router, partService);
  }

  ngOnInit() {
    super.onInit();
    this.lrSub = this.fdService.getJobAbsenceReasons().subscribe(data => this.initJobAbsenceReasons(data));
  }

  onPinInit() {
    // this.goBackUrl = '/pin/' + this.pin;
  }

  onParticipantInit() {
    // TODO: Add some Rxjs goodness to make sure both observables are complete (base class and here)
    // before setting isLoaded.
    this.isLoaded = true;
  }

  initJobAbsenceReasons(data) {
    this.strikeId = Utilities.idByFieldDataName('Strike', data);
    this.otherAbsenceReasonId = Utilities.idByFieldDataName('Other', data);
    this.jobAbsenceReasonsDrop = data;
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

  isIE(): boolean {
    const x = navigator.appVersion.indexOf('Trident') !== -1;
    return x;
  }

  onBlur() {
    this.onTouchedCallback();
  }

  writeValue(value: any) {
    if (value !== this.model) {
      this.model = value;
      if (this.model != null) {
        this.model.sort(function(a, b) {
          const aDate = new Date(a.beginDate);
          const bDate = new Date(b.beginDate);
          return aDate > bDate ? -1 : aDate < bDate ? 1 : 0;
        });
      }
    }
  }

  registerOnChange(fn: any) {
    this.onChangeCallback = fn;
  }

  registerModelChange(ab: Absence) {
    this.modelChange.emit();
    if (this.isInAutoValidation === true) {
      this.validate(ab);
    }
  }

  registerOnTouched(fn: any) {
    this.onTouchedCallback = fn;
  }

  addAbsenceRecord(): void {
    if (this.model != null && this.isAbsenceInEdit === false) {
      // If any record is open don't add new one.
      for (const m of this.model) {
        if (m.isOpen === true) {
          return;
        }
      }
      const ab = new Absence();
      ab.isOpen = true;
      this.model.push(ab);
    }
  }

  confirmAbsenceRecord(ab: Absence): void {
    this.validate(ab);
    this.isInAutoValidation = true;
    if (this.isValid === true) {
      this.indexOfOpenAbsence = null;
      this.isAbsenceInEdit = false;
      this.isInAutoValidation = false;
      ab.isOpen = false;
      this.modelChange.emit();
    }
  }

  editAbsenceRecord(ab: Absence): void {
    if (this.isValid === true && this.isAbsenceInEdit === false) {
      ab.isOpen = true;
      const editItemIndex = this.model.indexOf(ab);
      this.indexOfOpenAbsence = editItemIndex;
      this.isAbsenceInEdit = true;
      this.onAbsenceEdit.emit(true);
    }
  }

  deleteAbsenceRecord(ab: Absence): void {
    if (ab.isOpen) {
      this.isValid = true;
      this.isAbsenceInEdit = false;
      this.isInAutoValidation = false;
    }
    const deletedItemIndex = this.model.indexOf(ab);
    this.model.splice(deletedItemIndex, 1);
    this.onAbsenceEdit.emit(false);
    this.registerModelChange(ab);
    this.validationManager.resetErrors();
    this.modelErrors = {};
  }

  isErrored(ab: Absence, modelName: string) {
    if (ab.isOpen) {
      return this.modelErrors[modelName];
    }
  }

  isDisabled(ab: Absence): boolean {
    if (ab.isOpen) {
      return false;
    } else {
      return true;
    }
  }

  isDetailsRequired(ab: Absence): boolean {
    return ab.isDetailsRequired(this.otherAbsenceReasonId);
  }

  isStrikeWarningDisplayed(): boolean {
    if (this.model != null) {
      let ret = false;
      for (const m of this.model) {
        if (this.strikeId != null && Number(m.absenceReasonId) === this.strikeId) {
          ret = true;
          break;
        } else {
          ret = false;
        }
      }
      return ret;
    }
    return null;
  }

  validate(ab: Absence) {
    // Clear all previous errors.
    this.validationManager.resetErrors();

    const result = ab.validate(
      this.validationManager,
      this.participantDOB,
      moment(this.jobBeginDate, 'MM/DD/YYYY'),
      moment(this.jobEndDate, 'MM/DD/YYYY'),
      this.isCurrentlyEmployed,
      this.otherAbsenceReasonId,
      this.model
    );

    this.isValid = result.isValid;
    this.modelErrors = result.errors;
    if (this.isValid === true) {
      this.isInAutoValidation = false;
    }
  }
}
