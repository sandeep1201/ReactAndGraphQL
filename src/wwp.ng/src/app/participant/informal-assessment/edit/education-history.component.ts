import { AppService } from './../../../core/services/app.service';
import { Component, OnInit, OnDestroy, ViewContainerRef, ViewChild } from '@angular/core';
import { Subscription, Observable, forkJoin } from 'rxjs';

import { BaseInformalAssessmentSecton } from './base-informal-assessment';
import { DropDownField } from '../../../shared/models/dropdown-field';
import { EducationHistorySection } from '../../../shared/models/education-history-section';
import { FieldDataService } from '../../../shared/services/field-data.service';
import { InformalAssessmentEditService } from '../../../shared/services/informal-assessment-edit.service';
import { ModalService } from 'src/app/core/modal/modal.service';
import { ModelErrors } from '../../../shared/interfaces/model-errors';
import { SectionComponent } from '../../../shared/interfaces/section-component';
import { ParticipantService } from '../../../shared/services/participant.service';
import { ValidationManager } from '../../../shared/models/validation-manager';
import { Utilities } from '../../../shared/utilities';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-education-history-edit',
  templateUrl: 'education-history.component.html',
  styleUrls: ['./edit.component.css']
})
export class EducationHistoryEditComponent extends BaseInformalAssessmentSecton implements OnInit, OnDestroy, SectionComponent {
  private dipSub: Subscription;
  public educationDiplomaTypes: DropDownField[];
  private fdSub: Subscription;
  private partSub: Subscription;
  private lastGradeSub: Subscription;
  private sectionSub: Subscription;
  public validationManager: ValidationManager = new ValidationManager(this.appService);
  public modelErrors: ModelErrors = {}; // Note: this must be initialized to an empty object for the UI in initial state.
  public diplomaId: number;
  public gedId: number;
  public hsedId: number;
  public noneId: number;
  public cached = new EducationHistorySection();

  public certificatesDrop: DropDownField[];
  public lastGradeDrop: DropDownField[];
  public model: EducationHistorySection;

  constructor(
    private appService: AppService,
    private eiaService: InformalAssessmentEditService,
    private fdService: FieldDataService,
    private partService: ParticipantService,
    viewContainer: ViewContainerRef,
    public modalService: ModalService
  ) {
    super(modalService, eiaService);
  }

  ngOnInit() {
    this.eiaService.setSectionComponent(this);

    forkJoin(
      this.partService.getParticipant(this.eiaService.getPin()).pipe(take(1)),
      this.fdService.getCertificateIssuers().pipe(take(1)),
      this.fdService.getSchoolGrades().pipe(take(1)),
      this.fdService.getEducationDiplomaTypes().pipe(take(1))
    )
      .pipe(take(1))
      .subscribe(results => {
        this.initParticipantModel(results[0]);
        this.initCertificatesDropDowns(results[1]);
        this.initLastGradeDropDowns(results[2]);
        this.initEducationDiplomaTypes(results[3]);
        this.loadModel();
      });
  }

  private loadModel() {
    this.sectionSub = this.eiaService.getEducationHistorySection().subscribe(data => {
      this.initSection(data);
      // HACK: the modifiedTracker IF was added to check when loading up the main page.
      this.modifiedTrackerForcedValidation();
      this.sectionLoaded();
    });
  }

  modifiedTrackerForcedValidation() {
    if (this.eiaService.hasEducationValidated || this.eiaService.modifiedTracker.isEducationError) {
      this.isSectionValid = false;
      this.validate();
    }
  }

  ngOnDestroy() {
    if (this.sectionSub != null) {
      this.sectionSub.unsubscribe();
    }
    if (this.fdSub != null) {
      this.fdSub.unsubscribe();
    }
    if (this.lastGradeSub != null) {
      this.lastGradeSub.unsubscribe();
    }
    if (this.dipSub != null) {
      this.dipSub.unsubscribe();
    }
    if (this.partSub != null) {
      this.partSub.unsubscribe();
    }
  }

  private initCertificatesDropDowns(data) {
    this.certificatesDrop = data;
  }

  private initLastGradeDropDowns(data) {
    this.lastGradeDrop = data;
  }

  private initEducationDiplomaTypes(data) {
    this.educationDiplomaTypes = data;
    this.diplomaId = Utilities.idByFieldDataName('Diploma', data);
    this.gedId = Utilities.idByFieldDataName('GED', data);
    this.hsedId = Utilities.idByFieldDataName('HSED', data);
    this.noneId = Utilities.idByFieldDataName('None', data);
  }

  private initSection(model: EducationHistorySection) {
    // Ask the service if there are server errors.
    this.model = model;
    this.eiaService.educationModel = model;

    // We use clone to check which fields are modified.
    this.cached = this.eiaService.lastSavedEducationModel;

    // Cloned string for equality checking
    this.cloneModelString = JSON.stringify(model);
  }

  refreshModel() {
    if (this.model != null && this.eiaService.educationModel != null) {
      this.initSection(this.eiaService.educationModel);
      this.modifiedTrackerForcedValidation();
    }

    this.isSectionActive = false;
    setTimeout(() => (this.isSectionActive = true), 0);
  }

  validate(): void {
    this.validationManager.resetErrors();

    // Call the model's validate method.
    const result = this.model.validate(this.validationManager, this.participantDOB, this.diplomaId, this.gedId, this.hsedId, this.noneId);

    // Update our properties so the UI can bind to the results.
    this.isSectionValid = result.isValid;
    this.modelErrors = result.errors;

    if (this.isSectionValid === true) {
      this.eiaService.sectionIsNowValid('education');
    }
  }

  isValid(): boolean {
    return this.isSectionValid;
  }

  prepareToSaveWithErrors(): EducationHistorySection {
    // make a clone
    const model = JSON.parse(JSON.stringify(this.model)) as EducationHistorySection;

    // Must be a 4 digit year.
    if (model.lastYearAttended < 1000) {
      model.lastYearAttended = null;
    }

    if (model.certificateYearAwarded < 1000) {
      model.certificateYearAwarded = null;
    }

    return model;
  }

  checkState() {
    if (!this.isSectionValid) {
      this.validate();
    }

    this.eiaService.setModifiedModel('education', this.cloneModelString !== JSON.stringify(this.eiaService.educationModel));
  }
}
