import { Component, OnInit, OnDestroy, ViewContainerRef } from '@angular/core';
import { Subscription, Observable, forkJoin } from 'rxjs';
import * as _ from 'lodash';

import { AppService } from 'src/app/core/services/app.service';
import { BaseInformalAssessmentSecton } from './base-informal-assessment';
import { DropDownField } from '../../../shared/models/dropdown-field';
import { InformalAssessmentEditService } from '../../../shared/services/informal-assessment-edit.service';
import { SectionComponent } from '../../../shared/interfaces/section-component';
import { FieldDataService } from '../../../shared/services/field-data.service';
import { ModalService } from 'src/app/core/modal/modal.service';
import { ModelErrors } from '../../../shared/interfaces/model-errors';
import { ParticipantService } from '../../../shared/services/participant.service';
import { WorkProgramsSection, WorkProgram } from '../../../shared/models/work-programs-section';
import { WorkProgramSectionError } from '../../../shared/models/work-programs-section-error';
import { ValidationManager } from '../../../shared/models/validation-manager';
import { Utilities } from '../../../shared/utilities';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-work-programs-edit',
  templateUrl: 'work-programs.component.html',
  styleUrls: ['./edit.component.css']
})
export class WorkProgramsEditComponent extends BaseInformalAssessmentSecton implements OnInit, OnDestroy, SectionComponent {
  // DateBase Constants. These get set on ngInit.
  private workStatusPastId = 0;
  private workStatusCurrentId = 0;
  private workStatusWaitlistId = 0;

  // private DOB: string;
  private sectionSub: Subscription;
  public validationManager: ValidationManager = new ValidationManager(this.appService);
  private wpStatSub: Subscription;
  private wpSub: Subscription;

  public model: WorkProgramsSection;
  public workProgramsDrop: DropDownField[];
  public workProgramStatusesDrop: DropDownField[];
  private otherId: number;
  public partSub: Subscription;
  public workProgramSectionError: WorkProgramSectionError;
  public isActive = true;
  public modelErrors: ModelErrors = {};
  public cached = new WorkProgramsSection();

  constructor(
    private appService: AppService,
    private eiaService: InformalAssessmentEditService,
    private fdService: FieldDataService,
    private partService: ParticipantService,
    private viewContainer: ViewContainerRef,
    public modalService: ModalService
  ) {
    super(modalService, eiaService);
  }

  ngOnInit() {
    this.eiaService.setSectionComponent(this);

    forkJoin(
      this.partService.getParticipant(this.eiaService.getPin()).pipe(take(1)),
      this.fdService.getWorkPrograms().pipe(take(1)),
      this.fdService.getWorkProgramStatuses().pipe(take(1))
    )
      .pipe(take(1))
      .subscribe(results => {
        this.initParticipantModel(results[0]);
        this.initWorkProgramsDropDowns(results[1]);
        this.initWorkProgramStatusesDropDowns(results[2]);
        this.loadModel();
      });
  }

  private loadModel() {
    this.sectionSub = this.eiaService.getWorkProgramsSection().subscribe(data => {
      this.initSection(data);
      // HACK: the modifiedTracker IF was added to check when loading up the main page.
      this.modifiedTrackerForcedValidation();
      this.sectionLoaded();
    });
  }

  modifiedTrackerForcedValidation() {
    if (this.eiaService.hasWorkProgramsValidated || this.eiaService.modifiedTracker.isWorkProgramsError) {
      this.isSectionValid = false;
      this.validate();
    }
  }

  ngOnDestroy() {
    if (this.sectionSub != null) {
      this.sectionSub.unsubscribe();
    }
    if (this.wpStatSub != null) {
      this.wpStatSub.unsubscribe();
    }
    if (this.partSub != null) {
      this.partSub.unsubscribe();
    }
    if (this.wpSub != null) {
      this.wpSub.unsubscribe();
    }
  }

  private initWorkProgramsDropDowns(data) {
    this.otherId = Utilities.idByFieldDataName('Other', data);
    this.workProgramsDrop = data;
  }

  private initWorkProgramStatusesDropDowns(data) {
    this.workProgramStatusesDrop = data;
    this.workStatusPastId = Utilities.idByFieldDataName('Past', data);
    this.workStatusCurrentId = Utilities.idByFieldDataName('Current', data);
    this.workStatusWaitlistId = Utilities.idByFieldDataName('Waitlist', data);
  }

  private initSection(model: WorkProgramsSection) {
    this.model = model;
    if (this.model.workPrograms != null) {
      this.model.workPrograms = this.model.defaultSort(this.model.workPrograms);
    }
    // lets insert one blank record.
    this.model.workPrograms = Utilities.deserilizeChildren(model.workPrograms, WorkProgram);

    this.eiaService.workProgramsModel = model;
    // We use clone to check which fields are modified.
    this.cached = this.eiaService.lastSavedWorkProgramsModel;
    // Cloned string for equality checking
    this.cloneModelString = JSON.stringify(model);
  }

  isMmYyyyFormatCorrect(input: string): boolean {
    return /^\d{2}\/\d{4}$/.test(input);
  }

  extractMmYyyy(input: string): [number, number] {
    let val: [number, number];

    if (input != null && input !== '' && input.length === 7) {
      // Get MM & YYYY
      const mm = Number(input.substr(0, 2));
      const yyyy = Number(input.substr(3));

      // If MM not between 1 and 12, leave undefined.
      if (mm > 0 && mm <= 12) {
        val = [mm, yyyy];
      } else {
        val = null;
      }
    } else {
      val = null;
    }

    return val;
  }

  validate(): void {
    // Clear all previous errors.
    this.validationManager.resetErrors();

    // Call the model's validate method.
    const result = this.model.validate(this.validationManager, this.workProgramStatusesDrop, this.otherId, this.participantDOB);

    // Update our properties so the UI can bind to the results.
    this.isSectionValid = result.isValid;
    this.modelErrors = result.errors;
    if (this.isSectionValid === true) {
      this.eiaService.sectionIsNowValid('work-programs');
    }
  }

  isValid(): boolean {
    return this.isSectionValid;
  }

  isWorkProgramsListEmpty() {
    if (this.model != null && this.model.workPrograms != null && this.model.workPrograms.length > 0) {
      for (const wp of this.model.workPrograms) {
        if (!this.isWorkProgramEmtpy(wp)) {
          return false;
        }
      }
      return true;
    }
  }

  isWorkProgramEmtpy(wp: WorkProgram): boolean {
    // If it's null right away it is defnitely empty.
    if (wp == null) {
      return true;
    }

    // We are going to look for the fist sign this is not an empty object.
    if (wp.startDate != null && wp.startDate !== '') {
      return false;
    }
    if (wp.endDate != null && wp.endDate !== '') {
      return false;
    }
    if (wp.contactId != null && wp.contactId !== 0) {
      return false;
    }
    if (wp.location != null && wp.location.description != null && wp.location.description !== '') {
      return false;
    }
    if (wp.details != null && wp.details !== '') {
      return false;
    }
    if (wp.workProgram != null && wp.workProgram.toString().trim() !== '') {
      return false;
    }
    if (wp.workProgramName != null && wp.workProgramName !== '') {
      return false;
    }
    if (wp.workStatus != null && wp.workStatus.toString().trim() !== '') {
      return false;
    }
    if (wp.workStatusName != null && wp.workStatusName !== '') {
      return false;
    }

    // Since we got here, it must look empty;
    return true;
  }

  prepareToSaveWithErrors(): WorkProgramsSection {
    // make a clone
    const model = JSON.parse(JSON.stringify(this.model)) as WorkProgramsSection;

    if (model.isInOtherPrograms === true && model.workPrograms != null) {
      for (let wp of model.workPrograms) {
        if (this.isWorkProgramEmtpy(wp)) {
          wp = null;
        } else {
          if (wp.startDate != null && wp.startDate.length > 0) {
            const mmYyyyStart = this.extractMmYyyy(wp.startDate);
            // If we can't parse a date, then wipe it as it's not in the
            // correct format.
            if (mmYyyyStart == null) {
              wp.startDate = null;
            }
          }

          if (wp.endDate != null && wp.endDate.length > 0) {
            const mmYyyyEnd = this.extractMmYyyy(wp.endDate);
            // If we can't parse a date, then wipe it as it's not in the
            // correct format.
            if (mmYyyyEnd == null) {
              wp.endDate = null;
            }
          }
        }

        const noNulls: WorkProgram[] = [];

        for (let i = 0; i < model.workPrograms.length; i++) {
          if (model.workPrograms[i] != null) {
            noNulls.push(model.workPrograms[i]);
          }
        }

        model.workPrograms = noNulls;
      }
    }

    return model;
  }

  refreshModel() {
    if (this.model != null && this.eiaService.workProgramsModel != null) {
      this.initSection(this.eiaService.workProgramsModel);
      this.modifiedTrackerForcedValidation();
    }

    this.isActive = false;
    setTimeout(() => (this.isActive = true), 0);
  }

  checkState() {
    if (!this.isSectionValid) {
      this.validate();
    }

    this.eiaService.setModifiedModel('work-programs', this.cloneModelString !== JSON.stringify(this.eiaService.workProgramsModel));
  }
}
