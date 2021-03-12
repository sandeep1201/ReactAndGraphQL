import { AppService } from './../../../core/services/app.service';
// tslint:disable: use-life-cycle-interface
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription, forkJoin } from 'rxjs';
import { ActionNeeded } from '../../../features-modules/actions-needed/models/action-needed';
import { BaseInformalAssessmentSecton } from './base-informal-assessment';
import { DropDownField } from '../../../shared/models/dropdown-field';
import { FamilyBarriersSection, FamilyMember } from '../../../shared/models/family-barriers-section';
import { FieldDataService } from '../../../shared/services/field-data.service';
import { InformalAssessmentEditService } from '../../../shared/services/informal-assessment-edit.service';
import { ModalService } from 'src/app/core/modal/modal.service';
import { ModelErrors } from '../../../shared/interfaces/model-errors';
import { ParticipantService } from '../../../shared/services/participant.service';
import { SectionComponent } from '../../../shared/interfaces/section-component';
import { ValidationManager } from '../../../shared/models/validation-manager';
import { Authorization } from '../../../shared/models/authorization';
import { ParticipantGuard } from '../../../shared/guards/participant-guard';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-family-barriers-edit',
  templateUrl: 'family-barriers.component.html',
  styleUrls: ['./edit.component.css']
})
export class FamilyBarriersEditComponent extends BaseInformalAssessmentSecton implements OnInit, OnDestroy, SectionComponent {
  private sectionSub: Subscription;
  public actionNeededs: ActionNeeded[];
  public relationshipsDrop: DropDownField[];
  public model: FamilyBarriersSection;
  public modelErrors: ModelErrors = {}; // Note: this must be initialized to an empty object for the UI in initial state.
  public validationManager: ValidationManager = new ValidationManager(this.appService);

  public notYetSubmittedStatusId: number;
  public ssiApplicationStatuses: DropDownField[];
  public cached: FamilyBarriersSection;
  private fbSub: Subscription;
  public hasFBAccessBol: boolean;
  public canRequestFBAccess: boolean;
  public requestedElevatedAccess: boolean;
  get canAccessFamilyBarriersSsi(): boolean {
    return this.appService.isUserAuthorized(Authorization.canAccessFamilyBarriersSsi, null);
  }

  constructor(
    private appService: AppService,
    private fdService: FieldDataService,
    private eiaService: InformalAssessmentEditService,
    private partService: ParticipantService,
    private modalService: ModalService,
    private participantGuard: ParticipantGuard
  ) {
    super(modalService, eiaService);
  }

  ngOnInit() {
    this.eiaService.setSectionComponent(this);
    this.fbSub = this.appService.FBSection.subscribe(res => {
      this.hasFBAccessBol = res.hasFBAccessBol;
      this.canRequestFBAccess = res.canRequestFBAccess;
      this.requestedElevatedAccess = res.requestedElevatedAccess;
      // This scenario will be handled by coreGuard.
    });

    forkJoin(
      this.partService.getParticipant(this.eiaService.getPin()).pipe(take(1)),
      this.fdService.getSsiApplicationStatuses().pipe(take(1)),
      this.fdService.getRelationships().pipe(take(1)),
      this.fdService.getFamilyBarriersActionNeeded().pipe(take(1))
    )
      .pipe(take(1))
      .subscribe(results => {
        this.initParticipantModel(results[0]);
        this.initSsiApplicationStatuses(results[1]);
        this.initRelationshipsDrop(results[2]);
        this.initFamilyBarriersActionNeeded(results[3]);
        this.loadModel();
      });
  }

  ngAfterViewInit() {
    if (this.hasFBAccessBol && this.canRequestFBAccess) {
      this.participantGuard.showModelsForPHI().subscribe(res => {
        return res;
      });
    }
  }

  private loadModel() {
    this.sectionSub = this.eiaService.getFamilyBarriersSection().subscribe(data => {
      this.initSection(data);
      // HACK: the modifiedTracker IF was added to check when loading up the main page.
      this.modifiedTrackerForcedValidation();
      this.sectionLoaded();
    });
  }

  modifiedTrackerForcedValidation() {
    if (this.eiaService.hasFamilyBarriersValidated || this.eiaService.modifiedTracker.isFamilyBarriersError) {
      this.isSectionValid = false;
      this.validate();
    }
  }

  initSection(model) {
    this.model = model;
    this.eiaService.familyBarriersModel = model;
    this.cloneModelString = JSON.stringify(model);
    this.cached = this.eiaService.lastSavedFamilyBarriersModel;

    // Pushes blank object inside of repeater when model has none.
    if (model.familyMembers != null) {
      if (model.familyMembers.length === 0) {
        const x = new FamilyMember();
        x.id = 0;
        this.model.familyMembers.push(x);
      }
    }
  }

  initSsiApplicationStatuses(data) {
    this.ssiApplicationStatuses = data;
    let isFound = false;

    for (const s of data) {
      if (s.name === 'Not Yet Submitted') {
        this.notYetSubmittedStatusId = s.id;
        isFound = true;
      }
    }

    if (!isFound) {
      console.warn('Application Status: Not Yet Submitted (NOT FOUND!)');
    }
  }

  initRelationshipsDrop(data) {
    this.relationshipsDrop = data;
  }

  initFamilyBarriersActionNeeded(data) {
    this.actionNeededs = data;
  }

  isValid(): boolean {
    return this.isSectionValid;
  }

  validate(): void {
    // Clear all previous errors.
    this.validationManager.resetErrors();

    // Call the model's validate method.
    const result = this.model.validate(this.validationManager, this.participantDOB, this.ssiApplicationStatuses, this.canAccessFamilyBarriersSsi);

    // Update our properties so the UI can bind to the results.
    this.isSectionValid = result.isValid;
    this.modelErrors = result.errors;

    if (this.isSectionValid === true) {
      this.eiaService.sectionIsNowValid('family-barriers');
    }
  }

  prepareToSaveWithErrors(): FamilyBarriersSection {
    // make a clone.
    const model = new FamilyBarriersSection().deserialize(JSON.parse(JSON.stringify(this.model)));
    model.cleanse();

    return model;
  }

  onModelChange() {
    // See if we are in "check state"
    if (!this.isSectionValid) {
      this.validate();
    }

    this.eiaService.setModifiedModel('family-barriers', this.cloneModelString !== JSON.stringify(this.eiaService.familyBarriersModel));
  }

  refreshModel() {
    if (this.model != null && this.eiaService.familyBarriersModel != null) {
      this.initSection(this.eiaService.familyBarriersModel);
      this.modifiedTrackerForcedValidation();
    }

    this.isSectionActive = false;

    setTimeout(() => (this.isSectionActive = true), 0);
  }

  ngOnDestroy() {
    if (this.sectionSub != null) {
      this.sectionSub.unsubscribe();
    }
  }
}
