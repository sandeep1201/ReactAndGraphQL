import { Component, OnInit, OnDestroy, AfterViewInit } from '@angular/core';
import { Subscription, forkJoin } from 'rxjs';
import { BaseInformalAssessmentSecton } from './base-informal-assessment';
import { InformalAssessmentEditService } from '../../../shared/services/informal-assessment-edit.service';
import { ModalService } from 'src/app/core/modal/modal.service';
import { FieldDataService } from '../../../shared/services/field-data.service';
import { ModelErrors } from '../../../shared/interfaces/model-errors';
import { ParticipantBarrier } from '../../../shared/models/participant-barriers-app';
import { ParticipantBarrierAppService } from '../../../shared/services/participant-barrier-app.service';
import { ParticipantBarriersSection } from '../../../shared/models/participant-barriers-section';
import { SectionComponent } from '../../../shared/interfaces/section-component';
import { ValidationManager } from '../../../shared/models/validation-manager';
import { YesNoStatus } from '../../../shared/models/primitives';
import { DropDownField } from '../../../shared/models/dropdown-field';
import { Utilities } from '../../../shared/utilities';
import { ParticipantGuard } from '../../../shared/guards/participant-guard';
import { take } from 'rxjs/operators';
import { AppService } from 'src/app/core/services/app.service';

@Component({
  selector: 'app-participant-barriers-app-edit',
  templateUrl: './participant-barriers.component.html',
  styleUrls: ['./edit.component.css'],
  providers: [ParticipantBarrierAppService]
})
export class ParticipantBarriersEditComponent extends BaseInformalAssessmentSecton implements OnInit, AfterViewInit, OnDestroy, SectionComponent {
  public allParticipantBarriers: ParticipantBarrier[];
  public validationManager: ValidationManager = new ValidationManager(this.appService);
  public pin = '';
  public modelErrors: ModelErrors = {}; // Note: this must be initialized to an empty object for the UI in initial state.
  public model: ParticipantBarriersSection;
  public cached = new ParticipantBarriersSection();
  public yesNoRefusedDrop: DropDownField[] = [];
  public isSafeAppropriateToAsk = false;

  private yesId = 0;
  private pbListSub: Subscription;
  private sectionSub: Subscription;
  private pbSub: Subscription;
  public hasPBAccessBol: boolean;
  public canRequestPBAccess: boolean;
  public requestedElevatedAccess: boolean;

  constructor(
    private participantGuard: ParticipantGuard,
    private appService: AppService,
    private fdService: FieldDataService,
    private participantBarrierAppService: ParticipantBarrierAppService,
    private eiaService: InformalAssessmentEditService,
    public modalService: ModalService
  ) {
    super(modalService, eiaService);
  }

  ngOnInit() {
    this.eiaService.setSectionComponent(this);
    this.participantBarrierAppService.setPin(this.eiaService.getPin());
    this.pin = this.eiaService.getPin();
    this.pbSub = this.appService.PBSection.subscribe(res => {
      this.hasPBAccessBol = res.hasPBAccessBol;
      this.canRequestPBAccess = res.canRequestPBAccess;
      this.requestedElevatedAccess = res.requestedElevatedAccess;
      // This scenario will be handled by coreGuard.
    });

    forkJoin(this.fdService.getPolarRefused().pipe(take(1)), this.participantBarrierAppService.getParticipantBarriers().pipe(take(1)))
      .pipe(take(1))
      .subscribe(results => {
        this.initYesNoRefusedDrop(results[0]);
        this.initParticipantBarriers(results[1]);
        this.loadModel();
        this.getParticipantBarrierList();
      });
  }

  ngAfterViewInit() {
    if (this.hasPBAccessBol && this.canRequestPBAccess) {
      this.participantGuard.showModel().subscribe(res => {
        return res;
      });
    }
  }

  private loadModel() {
    this.sectionSub = this.eiaService.getParticipantBarriersSection().subscribe(data => {
      this.initSection(data);
      // HACK: the modifiedTracker IF was added to check when loading up the main page.
      this.modifiedTrackerForcedValidation();
      this.sectionLoaded();
    });
  }

  getParticipantBarrierList(): void {
    this.pbListSub = this.participantBarrierAppService.getParticipantBarriers().subscribe(participantBarriers => {
      if (participantBarriers != null) {
        this.initParticipantBarriers(participantBarriers);
      }
    });
  }

  modifiedTrackerForcedValidation() {
    if (this.eiaService.hasParticipantBarriersValidated || this.eiaService.modifiedTracker.isParticipantBarriersError) {
      this.isSectionValid = false;
      this.validate();
    }
  }

  initSection(model) {
    this.model = model;
    this.eiaService.participantBarriersModel = model;
    // We use clone to check which fields are modified.
    this.cloneModelString = JSON.stringify(model);

    this.model.isSafeAppropriateToAsk = false;

    this.cached = this.eiaService.lastSavedParticipantBarriersModel;
  }

  initParticipantBarriers(data: ParticipantBarrier[]): void {
    if (data != null) {
      this.allParticipantBarriers = data;
    }
  }

  initYesNoRefusedDrop(data: DropDownField[]): void {
    this.yesNoRefusedDrop = data;
    this.yesId = Utilities.idByFieldDataName('Yes', data);
  }

  validate(): void {
    // Clear all previous errors.
    this.validationManager.resetErrors();

    // Call the model's validate method.
    const result = this.model.validate(this.validationManager, this.yesId, this.hasPBAccessBol, this.canRequestPBAccess, this.requestedElevatedAccess);

    // Update our properties so the UI can bind to the results.
    this.isSectionValid = result.isValid;
    this.modelErrors = result.errors;

    if (this.isSectionValid === true) {
      this.eiaService.sectionIsNowValid('participant-barriers');
    }
  }

  isPhysicalHealthTakeMedicationDisplayed(): boolean {
    return this.model.isPhysicalHealthTakeMedicationDisplayed(this.yesId);
  }

  isMentalHealthTakeMedicationDisplayed(): boolean {
    return this.model.isMentalHealthTakeMedicationDisplayed(this.yesId);
  }

  isAodaTakeTreatmentDisplayed(): boolean {
    return this.model.isAodaTakeTreatmentDisplayed(this.yesId);
  }

  isDetailsRequired(yesNoRefused: YesNoStatus) {
    return this.model.isDetailsRequired(yesNoRefused, this.yesId);
  }

  onChange() {
    if (!this.isSectionValid) {
      this.validate();
    }
    this.eiaService.setModifiedModel('participant-barriers', this.cloneModelString !== JSON.stringify(this.eiaService.participantBarriersModel));
  }

  refreshModel() {
    if (this.model != null && this.eiaService.participantBarriersModel != null) {
      this.initSection(this.eiaService.participantBarriersModel);
      this.modifiedTrackerForcedValidation();
    }

    this.isSectionActive = false;

    setTimeout(() => (this.isSectionActive = true), 0);
  }

  isValid(): boolean {
    return this.isSectionValid;
  }

  prepareToSaveWithErrors(): ParticipantBarriersSection {
    // make a clone.
    const clonedModel = new ParticipantBarriersSection();
    ParticipantBarriersSection.clone(this.model, clonedModel);

    // If we need to cleanse, make sure to deserialize our clone.

    return clonedModel;
  }

  ngOnDestroy() {
    if (this.sectionSub != null) {
      this.sectionSub.unsubscribe();
    }
    if (this.pbListSub != null) {
      this.pbListSub.unsubscribe();
    }
    if (this.pbSub != null) {
      this.pbSub.unsubscribe();
    }
  }
}
