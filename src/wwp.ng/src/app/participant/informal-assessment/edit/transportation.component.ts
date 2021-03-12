import { Component, OnInit, OnDestroy, ViewContainerRef } from '@angular/core';
import { BaseInformalAssessmentSecton } from './base-informal-assessment';
import { Subscription, forkJoin } from 'rxjs';

import { ActionNeeded } from '../../../features-modules/actions-needed/models/action-needed';
import { AppService } from 'src/app/core/services/app.service';
import { DropDownField } from '../../../shared/models/dropdown-field';
import { InformalAssessmentEditService } from '../../../shared/services/informal-assessment-edit.service';
import { FieldDataService } from '../../../shared/services/field-data.service';
import { ModalService } from 'src/app/core/modal/modal.service';

import { ParticipantService } from '../../../shared/services/participant.service';
import { TransportationSection } from '../../../shared/models/transportation-section';
import { ValidationManager } from '../../../shared/models/validation-manager';
import { ModelErrors } from '../../../shared/interfaces/model-errors';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-transportation-edit',
  templateUrl: './transportation.component.html',
  styleUrls: ['./edit.component.css']
})
export class TransportationEditComponent extends BaseInformalAssessmentSecton implements OnInit, OnDestroy {
  private sectionSub: Subscription;

  public actionNeededs: ActionNeeded[];
  public isActive = true;
  public isLoaded = false;
  public model: TransportationSection;
  public originalModel: TransportationSection;
  public statesDrop: DropDownField[];
  public transportationTypes: DropDownField[];
  public driverLicenseStatusesDrop: DropDownField[];
  public polarDrop: DropDownField[] = [];
  public modelErrors: ModelErrors = {}; // Note: this must be initialized to an empty object for the UI in initial state.
  public validationManager: ValidationManager = new ValidationManager(this.appService);

  constructor(
    private appService: AppService,
    private eiaService: InformalAssessmentEditService,
    private partService: ParticipantService,
    private fdService: FieldDataService,
    public modalService: ModalService,
    viewContainer: ViewContainerRef
  ) {
    super(modalService, eiaService);
  }

  ngOnInit() {
    this.eiaService.setSectionComponent(this);

    forkJoin(
      this.partService.getParticipant(this.eiaService.getPin()).pipe(take(1)),
      this.fdService.getTransportationTypes().pipe(take(1)),
      this.fdService.getPolarUnknown().pipe(take(1)),
      this.fdService.getTransportationActionNeeded().pipe(take(1)),
      this.fdService.getDriverLicenseStates().pipe(take(1)),
      this.fdService.getDriverLicenseStatuses().pipe(take(1))
    )
      .pipe(take(1))
      .subscribe(results => {
        this.initParticipantModel(results[0]);
        this.initTransportationTypes(results[1]);
        this.initPolarInputs(results[2]);
        this.initTransportationActionNeeded(results[3]);
        this.initStatesDrop(results[4]);
        this.initDriverLicenseStatusesDrop(results[5]);
        this.loadModel();
      });
  }

  private loadModel() {
    this.sectionSub = this.eiaService.getTransportationSection().subscribe(data => {
      this.initSection(data);
      // HACK: the modifiedTracker IF was added to check when loading up the main page.
      this.modifiedTrackerForcedValidation();
      this.sectionLoaded();
    });
  }

  modifiedTrackerForcedValidation() {
    if (this.eiaService.hasTransportationValidated || this.eiaService.modifiedTracker.isTransportationError) {
      this.isSectionValid = false;
      this.validate();
    }
  }

  initSection(model) {
    this.model = model;
    this.eiaService.transportationSectionModel = model;

    this.originalModel = this.eiaService.lastSavedTransportationSectionModel;

    // Cloned string for equality checking
    this.cloneModelString = JSON.stringify(model);
  }

  private initTransportationActionNeeded(data) {
    this.actionNeededs = data;
  }
  initTransportationTypes(data) {
    this.transportationTypes = data;
  }

  initStatesDrop(data) {
    this.statesDrop = data;
  }

  initDriverLicenseStatusesDrop(data) {
    this.driverLicenseStatusesDrop = data;
  }

  private initPolarInputs(data): void {
    this.polarDrop = data;
  }

  refreshModel(): void {
    if (this.model != null && this.eiaService.transportationSectionModel != null) {
      this.initSection(this.eiaService.transportationSectionModel);
      this.modifiedTrackerForcedValidation();
    }

    this.isActive = false;
    setTimeout(() => (this.isActive = true), 0);
  }

  validate(): void {
    // Clear all previous errors.
    this.validationManager.resetErrors();

    // Call the model's validate method.
    const result = this.model.validate(this.validationManager, this.participantDOB.format('MM/DD/YYYY'), this.transportationTypes, this.driverLicenseStatusesDrop);

    // Update our properties so the UI can bind to the results.
    this.isSectionValid = result.isValid;
    this.modelErrors = result.errors;

    if (this.isSectionValid === true) {
      this.eiaService.sectionIsNowValid('transportation');
    }
  }

  isValid(): boolean {
    return this.isSectionValid;
  }

  prepareToSaveWithErrors(): TransportationSection {
    // make a clone
    const model = JSON.parse(JSON.stringify(this.model)) as TransportationSection;

    // Fix any potential unsavable issues or ones which the API
    // doesn't handle very well.

    return model;
  }

  onModelChange(): void {
    // See if we are in "check state"
    if (!this.isSectionValid) {
      this.validate();
    }

    this.eiaService.setModifiedModel('transportation', this.cloneModelString !== JSON.stringify(this.eiaService.transportationSectionModel));
  }

  ngOnDestroy() {
    if (this.sectionSub != null) {
      this.sectionSub.unsubscribe();
    }
  }
}
