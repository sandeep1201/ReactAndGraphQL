import { DateMmDdYyyyPipe } from 'src/app/shared/pipes/date-mm-dd-yyyy.pipe';
import { AppService } from 'src/app/core/services/app.service';
import { WeeklyHoursWorkedService } from './../../services/weekly-hours-worked.service';
import { WeeklyHoursWorked } from './../../models/weekly-hours-worked.model';
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { ValidationManager } from 'src/app/shared/models/validation';
import { ModelErrors } from 'src/app/shared/interfaces/model-errors';
import { Employment } from 'src/app/shared/models/work-history-app';
import { Participant } from 'src/app/shared/models/participant';
import { WhyReason } from 'src/app/shared/models/why-reasons.model';
import * as _ from 'lodash';

@Component({
  selector: 'app-hourly-entry-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.scss']
})
export class HourlyEntryEditComponent implements OnInit {
  public cachedWeeklyHoursWorkedEntry: WeeklyHoursWorked = new WeeklyHoursWorked();
  public isLoaded = false;

  public weeklyHoursWorkedEntry: WeeklyHoursWorked;
  @Input() weeklyHoursWorkedEntries: WeeklyHoursWorked[];
  @Input() inputModel: any;
  @Input() employment: Employment;
  @Output() cancel = new EventEmitter<any>();
  @Input() pin: string;
  @Input() participant: Participant;
  public isSaving = false;
  public isSectionValid = true;
  public isReadOnly = false;
  public isSectionModified = false;
  public hasTriedSave: boolean;
  public hadSaveError = false;
  public validationManager: ValidationManager = new ValidationManager(this.appService);
  public modelErrors: ModelErrors = {};
  public precheck: WhyReason = new WhyReason();
  public isDetailsRequiredBol = false;
  public isDialogPresent = false;

  public maximumLifeTimeSubsidizedHoursAllowed = Employment.maximumLifeTimeSubsidizedHoursAllowed;

  constructor(private weeklyHoursWorkedService: WeeklyHoursWorkedService, private appService: AppService) {}
  ngOnInit() {
    if (this.inputModel && this.inputModel.id === 0) {
      this.weeklyHoursWorkedEntry = new WeeklyHoursWorked();
      this.weeklyHoursWorkedEntry.id = this.inputModel.id;
      this.weeklyHoursWorkedEntry.employmentInformationId = this.employment.id;
      WeeklyHoursWorked.clone(this.weeklyHoursWorkedEntry, this.cachedWeeklyHoursWorkedEntry);
    } else {
      const dateMmDdYyyyPipe = new DateMmDdYyyyPipe();
      this.weeklyHoursWorkedEntry = Object.assign(new WeeklyHoursWorked(), this.inputModel);
      this.weeklyHoursWorkedEntry.startDate = dateMmDdYyyyPipe.transform(this.inputModel.startDate);
      WeeklyHoursWorked.clone(this.weeklyHoursWorkedEntry, this.cachedWeeklyHoursWorkedEntry);
      this.isReadOnly = this.appService.isStateStaff;
    }
    this.isLoaded = true;
  }

  public isDetailsRequired() {
    this.isDetailsRequiredBol = this.weeklyHoursWorkedEntry.hours && +this.weeklyHoursWorkedEntry.hours < 20;
  }

  calculateTotalSubsidyHours() {
    let totalHours = 0;
    this.weeklyHoursWorkedEntries.forEach(entry => (totalHours += +entry.hours));
    return totalHours;
  }

  checkforTotalHoursWarning() {
    if (this.hasTriedSave) {
      if (this.calculateTotalSubsidyHours() >= this.maximumLifeTimeSubsidizedHoursAllowed) {
        this.precheck.canSaveWithWarnings = true;
        this.precheck.warnings = ['The Subsidized Worker has reached the maximum allowed Subsidized Hours.'];
      }
    }
  }

  exitweeklyHoursWorkedEntryEditIgnoreChanges() {
    this.cancel.emit(false);
  }
  cancelOutOfDialogueView() {
    this.isDialogPresent = false;
  }

  exit() {
    this.cancel.emit(false);
  }
  saveAndExit() {
    this.hasTriedSave = true;
    this.checkforTotalHoursWarning();
    if (_.isEmpty(this.precheck)) {
      this.validate();
      this.save();
    }
  }
  saveWithWarningAndExit() {
    this.hasTriedSave = true;
    this.validate();
    this.save();
  }

  save() {
    if (this.isSectionValid) {
      this.hadSaveError = false;
      this.isSaving = true;
      this.weeklyHoursWorkedService.saveWeeklyHoursWorkedEntry(this.pin, this.weeklyHoursWorkedEntry).subscribe(
        res => {
          this.isSaving = false;
          this.cancel.emit({ afterPostCallRes: res });
        },
        error => {
          this.isSaving = false;
          this.hadSaveError = true;
        }
      );
    }
  }

  public validate(e?: string) {
    this.isSectionModified = true;
    if (this.hasTriedSave === true) {
      this.validationManager.resetErrors();
      const result = this.weeklyHoursWorkedEntry.validate(this.validationManager, this.participant, this.employment, this.weeklyHoursWorkedEntries);
      this.isSectionValid = result.isValid;
      this.modelErrors = result.errors;
      if (this.isSectionValid) this.hasTriedSave = false;
      if (this.modelErrors) {
        this.isSaving = false;
      }
    }
  }
}
