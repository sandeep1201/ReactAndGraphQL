import { AppService } from 'src/app/core/services/app.service';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription, forkJoin } from 'rxjs';
import { BaseInformalAssessmentSecton } from './base-informal-assessment';
import { DropDownField } from '../../../shared/models/dropdown-field';
import { FieldDataService } from '../../../shared/services/field-data.service';
import { InformalAssessmentEditService } from '../../../shared/services/informal-assessment-edit.service';
import { ModalService } from 'src/app/core/modal/modal.service';
import { ModelErrors } from '../../../shared/interfaces/model-errors';
import { NonCustodialParentsReferralSection, NonCustodialReferralChild } from '../../../shared/models/non-custodial-parents-referral-section';
import { ParticipantService } from '../../../shared/services/participant.service';
import { SectionComponent } from '../../../shared/interfaces/section-component';
import { ValidationManager } from '../../../shared/models/validation-manager';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-ncpr',
  templateUrl: './ncpr.component.html'
})
export class NcprEditComponent extends BaseInformalAssessmentSecton implements OnDestroy, OnInit, SectionComponent {
  private sectionSub: Subscription;

  public careTakersDrop: DropDownField[] = [];
  public contactIntervalDrop: DropDownField[];
  public ncpRelationshipDrop: DropDownField[];
  public polarDrop: DropDownField[] = [];
  public isActive = true;
  public model: NonCustodialParentsReferralSection;
  public modelErrors: ModelErrors = {}; // Note: this must be initialized to an empty object for the UI in initial state.
  public validationManager: ValidationManager = new ValidationManager(this.appService);
  public cached = new NonCustodialParentsReferralSection();
  constructor(
    private appService: AppService,
    private eiaService: InformalAssessmentEditService,
    private partService: ParticipantService,
    private fdService: FieldDataService,
    public modalService: ModalService
  ) {
    super(modalService, eiaService);
  }

  ngOnInit() {
    this.eiaService.setSectionComponent(this);

    forkJoin(
      this.partService.getParticipant(this.eiaService.getPin()).pipe(take(1)),
      this.fdService.getContactNcprIntervals().pipe(take(1)),
      this.fdService.getPolarSkip().pipe(take(1))
    )
      .pipe(take(1))
      .subscribe(results => {
        this.initParticipantModel(results[0]);
        this.initContactIntervals(results[1]);
        this.initPolarInputs(results[2]);
        this.loadModel();
      });
  }

  private loadModel() {
    this.sectionSub = this.eiaService.getNonCustodialParentsReferralSection().subscribe(data => {
      this.initSection(data);
      // HACK: the modifiedTracker IF was added to check when loading up the main page.
      this.modifiedTrackerForcedValidation();
      this.sectionLoaded();
    });
  }

  modifiedTrackerForcedValidation() {
    if (this.eiaService.hasNonCustodialParentsReferralValidated || this.eiaService.modifiedTracker.isNonCustodialParentsReferralError) {
      this.isSectionValid = false;
      this.validate();
    }
  }

  private initContactIntervals(data): void {
    this.contactIntervalDrop = data;
  }

  private initPolarInputs(data): void {
    this.polarDrop = data;
  }

  private initSection(model): void {
    this.model = model;
    this.eiaService.nonCustodialParentsReferralModel = this.model;
    // We use clone to check which fields are modified.
    this.cached = this.eiaService.lastSavedNonCustodialParentsReferralModel;
    // Cloned string for equality checking
    this.cloneModelString = JSON.stringify(this.model);
  }

  public isOtherParentRequired() {
    if (this.polarDrop != null) {
      return this.model.isOtherParentRequired(this.polarDrop);
    }
  }

  public moveNonCustodialChild(childMoveRequest: { nonCustodialReferralChild: NonCustodialReferralChild; parentGuid: string }): void {
    this.model.moveNonCustodialChild(childMoveRequest.nonCustodialReferralChild, childMoveRequest.parentGuid);
    this.onModelChange();
  }

  public areRepeatersEmpty(): boolean {
    let isDisabled = false;
    if (this.model != null && this.model.parents != null) {
      for (const p of this.model.parents) {
        if (p.isEmpty() === false) {
          isDisabled = true;
        }
      }
    }
    return isDisabled;
  }

  validate(): void {
    // Clear all previous errors.
    this.validationManager.resetErrors();

    // Call the model's validate method.
    const result = this.model.validate(this.validationManager, this.polarDrop);

    // Update our properties so the UI can bind to the results.
    this.isSectionValid = result.isValid;
    this.modelErrors = result.errors;

    if (this.isSectionValid === true) {
      this.eiaService.sectionIsNowValid('non-custodial-parents-referral');
    }
  }

  isValid(): boolean {
    if (this.isSectionValid === false) {
      return false;
    } else {
      return true;
    }
  }

  onModelChange(): void {
    // See if we are in "check state"
    if (!this.isSectionValid) {
      this.validate();
    }

    const clone = new NonCustodialParentsReferralSection();
    NonCustodialParentsReferralSection.clone(this.eiaService.nonCustodialParentsReferralModel, clone);
    this.eiaService.setModifiedModel('non-custodial-parents-referral', JSON.stringify(this.cached) !== JSON.stringify(clone));
  }

  prepareToSaveWithErrors(): NonCustodialParentsReferralSection {
    // make a clone
    const model = JSON.parse(JSON.stringify(this.model)) as NonCustodialParentsReferralSection;

    return model;
  }

  refreshModel(): void {
    if (this.model != null && this.eiaService.nonCustodialParentsReferralModel != null) {
      this.initSection(this.eiaService.nonCustodialParentsReferralModel);
      this.modifiedTrackerForcedValidation();
    }

    this.isActive = false;
    setTimeout(() => (this.isActive = true), 0);
  }

  ngOnDestroy(): void {
    if (this.sectionSub != null) {
      this.sectionSub.unsubscribe();
    }
  }
}
