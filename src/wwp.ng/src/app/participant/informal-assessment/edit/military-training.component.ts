import { AppService } from './../../../core/services/app.service';
import { Component, OnInit, OnDestroy, ViewContainerRef } from '@angular/core';
import { Subscription, forkJoin } from 'rxjs';

import { BaseInformalAssessmentSecton } from './base-informal-assessment';
import { DropDownField } from '../../../shared/models/dropdown-field';
import { InformalAssessmentEditService } from '../../../shared/services/informal-assessment-edit.service';
import { SectionComponent } from '../../../shared/interfaces/section-component';
import { FieldDataService } from '../../../shared/services/field-data.service';
import { ModalService } from 'src/app/core/modal/modal.service';
import { MilitarySection } from '../../../shared/models/military-section';
import { ModelErrors } from '../../../shared/interfaces/model-errors';
import { ParticipantService } from '../../../shared/services/participant.service';
import { ValidationManager } from '../../../shared/models/validation-manager';
import { Utilities } from '../../../shared/utilities';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-military-training-edit',
  templateUrl: 'military-training.component.html',
  styleUrls: ['./edit.component.css']
  //   viewProviders: [MODAL_DIRECTIVES]
})
export class MilitaryTrainingEditComponent extends BaseInformalAssessmentSecton implements OnInit, OnDestroy, SectionComponent {
  private sectionSub: Subscription;

  public model: MilitarySection;
  public modelErrors: ModelErrors = {}; // Note: this must be initialized to an empty object for the UI in initial state.
  public validationManager: ValidationManager = new ValidationManager(this.appService);
  public isActive = true;

  public branchDrop: DropDownField[] = [];
  public dischargeDrop: DropDownField[] = [];
  public rankDrop: DropDownField[] = [];
  public polarInputDropDown: DropDownField[] = [];
  public cached: MilitarySection;
  public outsideOfTheUsId: number;

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
      this.fdService.getMilitaryRanks().pipe(take(1)),
      this.fdService.getMilitaryDischargeTypes().pipe(take(1)),
      this.fdService.getMilitaryBranches().pipe(take(1)),
      this.fdService.getPolarInput().pipe(take(1))
    )
      .pipe(take(1))
      .subscribe(results => {
        this.initParticipantModel(results[0]);
        this.initRankDropDown(results[1]);
        this.initDischargeDropDown(results[2]);
        this.initBranchDropDown(results[3]);
        this.initPolarInputDropDown(results[4]);
        this.loadModel();
      });
  }

  private loadModel() {
    this.sectionSub = this.eiaService.getMilitarySection().subscribe(data => {
      this.initSection(data);
      // HACK: the modifiedTracker IF was added to check when loading up the main page.
      this.modifiedTrackerForcedValidation();
      this.sectionLoaded();
    });
  }

  modifiedTrackerForcedValidation() {
    if (this.eiaService.hasMilitaryValidated || this.eiaService.modifiedTracker.isMilitaryError) {
      this.isSectionValid = false;
      this.validate();
    }
  }

  ngOnDestroy() {
    if (this.sectionSub != null) {
      this.sectionSub.unsubscribe();
    }
  }

  private initSection(model: MilitarySection) {
    this.model = model;
    this.eiaService.militaryModel = model;

    this.cached = this.eiaService.lastSavedMilitaryModel;

    // Cloned string for equality checking
    this.cloneModelString = JSON.stringify(model);
  }

  private initBranchDropDown(data) {
    this.outsideOfTheUsId = Utilities.idByFieldDataName('Outside of the U.S.', data);
    this.branchDrop = data;
  }

  private initDischargeDropDown(data) {
    this.dischargeDrop = data;
  }

  private initRankDropDown(data) {
    this.rankDrop = data;
  }

  private initPolarInputDropDown(data) {
    this.polarInputDropDown = data;
  }

  checkState() {
    if (!this.isSectionValid) {
      this.validate();
    }

    this.eiaService.setModifiedModel('military', this.cloneModelString !== JSON.stringify(this.eiaService.militaryModel));
  }

  validate(): void {
    // Clear all previous errors.
    this.validationManager.resetErrors();

    // Call the model's validate method.
    const result = this.model.validate(this.validationManager, this.participantDOB.format('MM/DD/YYYY'));

    // Update our properties so the UI can bind to the results.
    this.isSectionValid = result.isValid;
    this.modelErrors = result.errors;

    if (this.isSectionValid === true) {
      this.eiaService.sectionIsNowValid('military');
    }
  }

  isValid(): boolean {
    return this.isSectionValid;
  }

  prepareToSaveWithErrors(): MilitarySection {
    // make a clone
    const model = JSON.parse(JSON.stringify(this.model)) as MilitarySection;

    // Fix any potential unsavable issues or ones which the API
    // doesn't handle very well.

    return model;
  }

  refreshModel() {
    if (this.model != null && this.eiaService.militaryModel != null) {
      this.initSection(this.eiaService.militaryModel);
      this.modifiedTrackerForcedValidation();
    }
    this.isActive = false;
    setTimeout(() => (this.isActive = true), 0);
  }
}
