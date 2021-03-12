// tslint:disable: prefer-const

import { Utilities } from './../../../shared/utilities';
import { DropDownField } from './../../../shared/models/dropdown-field';
import { FieldDataService } from './../../../shared/services/field-data.service';
import { AppService } from 'src/app/core/services/app.service';
import { DateMmDdYyyyPipe } from './../../../shared/pipes/date-mm-dd-yyyy.pipe';
import { ModelErrors } from './../../../shared/interfaces/model-errors';
import { ParticipationTracking } from './../../../shared/models/participation-tracking.model';
import { ParticipationTrackingService } from './../services/participation-tracking.service';
import { Component, OnInit, Input } from '@angular/core';
import * as _ from 'lodash';
import { ValidationManager } from 'src/app/shared/models/validation';
import { forkJoin } from 'rxjs';
@Component({
  selector: 'app-participation-entry',
  templateUrl: './participation-entry.component.html',
  styleUrls: ['./participation-entry.component.scss']
})
export class ParticipationEntryComponent implements OnInit {
  public isLoaded = false;
  @Input() singlePTEvent: ParticipationTracking;
  @Input() pin: any;
  @Input() participantId: any;
  public model: ParticipationTracking = new ParticipationTracking();
  public cachedModel: ParticipationTracking = new ParticipationTracking();
  public modelErrors: ModelErrors = {};
  public nonParticipationHours: number;
  public isSaving = false;
  public isSectionModified = false;
  public validationManager: ValidationManager = new ValidationManager(this.appService);
  public isSectionValid = false;
  public hasTriedSave = false;
  public isHrsParticipatedSectionValid = true;
  public isCalculateDisabled = false;
  public goodCauseDeniedReasons: DropDownField[];
  public goodCauseGrantedReasons: DropDownField[];
  public nonParticipationReasons: DropDownField[];
  public inConfirmDeleteView = false;

  constructor(private ptService: ParticipationTrackingService, public appService: AppService, private fieldDataService: FieldDataService) {}

  ngOnInit() {
    ParticipationTracking.clone(this.singlePTEvent, this.model);
    const pipe = new DateMmDdYyyyPipe();
    this.model.participationDate = pipe.transform(this.model.participationDate);
    this.model.makeUpEntries.forEach((item, index) => {
      this.model.makeUpEntries[index].makeupDate = pipe.transform(item.makeupDate);
    });
    ParticipationTracking.clone(this.model, this.cachedModel);

    forkJoin(
      this.fieldDataService.getFieldDataByField('good-cause-denied-reasons', 'ww'),
      this.fieldDataService.getFieldDataByField('good-cause-granted-reasons', 'ww'),
      this.fieldDataService.getFieldDataByField('non-participation-reasons', 'ww')
    ).subscribe(res => {
      this.goodCauseDeniedReasons = res[0];
      this.goodCauseGrantedReasons = res[1];
      this.nonParticipationReasons = res[2];
      this.isLoaded = true;
    });
  }
  // This will calculate the no-participated hours and also validates the section above non-participation section
  calculateAndValidateHPSection() {
    let totalMakeupHours = 0;
    this.model.makeUpEntries.forEach(i => {
      totalMakeupHours += +i.makeupHours;
    });
    this.model.totalMakeupHours = totalMakeupHours.toFixed(1);
    this.model.participatedHours = (+this.model.reportedHours + +this.model.totalMakeupHours).toFixed(1);

    this.validateHrsParticipatedSection();

    if (this.isHrsParticipatedSectionValid) {
      this.isSectionValid = true;
      this.isCalculateDisabled = true;
      this.model.nonParticipatedHours = (+this.model.scheduledHours - (+this.model.reportedHours + +this.model.totalMakeupHours)).toFixed(1);
      this.validateSave();
    }
  }

  checkState() {
    if (!this.isHrsParticipatedSectionValid) this.validateHrsParticipatedSection();
  }

  validateHrsParticipatedSection() {
    this.validationManager.resetErrors();

    const result = this.model.validateHrsParticipatedSection(this.validationManager);

    this.isHrsParticipatedSectionValid = result.isValid;
    this.modelErrors = result.errors;

    if (this.modelErrors) this.isSectionValid = false;
  }

  enableCalculate() {
    this.isCalculateDisabled = false;
    this.isSectionValid = false;
  }
  validateSave() {
    this.isSectionModified = true;
    if (this.hasTriedSave === true) {
      this.validationManager.resetErrors();
      const result = this.model.validate(this.validationManager, this.nonParticipationReasons, this.goodCauseGrantedReasons, this.goodCauseDeniedReasons);
      this.isSectionValid = result.isValid;
      this.modelErrors = result.errors;
    }
  }

  cleanseModelForSave() {
    this.model.nonParticipationReasonId = null;
    this.model.nonParticipationReasonDetails = null;
    this.model.goodCauseGranted = null;
    this.model.goodCauseGrantedReasonId = null;
    this.model.goodCauseDeniedReasonId = null;
    this.model.goodCauseReasonDetails = null;
    this.model.goodCausedHours = null;
  }

  showunSavedChangesDialogue() {
    this.isSectionModified = true;
  }
  exit() {
    if (this.isSectionModified === true) {
      this.appService.isDialogPresent = true;
    } else {
      this.ptService.modeForParticipationEntry.next({ readOnly: false, inEditView: false });
    }
  }
  save() {
    if (this.isSectionValid) {
      this.isSaving = true;

      let cachedModel: ParticipationTracking = new ParticipationTracking();

      ParticipationTracking.clone(this.model, cachedModel);

      if (Utilities.stringIsNullOrWhiteSpace(this.model.nonParticipatedHours) || +this.model.nonParticipatedHours === 0) {
        this.cleanseModelForSave();
      }

      this.ptService.postParticipationTrackingDetails(this.pin, this.model).subscribe(
        () => this.dialogueClose(),
        error => {
          ParticipationTracking.clone(cachedModel, this.model);
          this.isSaving = false;
        }
      );
    }
  }
  dialogueClose() {
    this.ptService.modeForParticipationEntry.next({ readOnly: false, inEditView: false });
  }

  saveAndExit() {
    this.hasTriedSave = true;
    this.validateSave();
    this.save();
  }
  delete() {
    this.inConfirmDeleteView = true;
  }

  onCancelDelete() {
    this.inConfirmDeleteView = false;
  }

  onConfirmDelete() {
    this.ptService.deleteParticipationTrackingDetails(this.pin, this.participantId, this.model.id).subscribe(res => {
      this.dialogueClose();
    });

    this.inConfirmDeleteView = false;
  }
}
