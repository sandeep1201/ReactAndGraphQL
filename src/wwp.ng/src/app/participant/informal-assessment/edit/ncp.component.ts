import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription, forkJoin } from 'rxjs';
import * as moment from 'moment';
import { ActionNeeded } from '../../../features-modules/actions-needed/models/action-needed';
import { BaseInformalAssessmentSecton } from './base-informal-assessment';
import { DropDownField } from '../../../shared/models/dropdown-field';
import { FieldDataService } from '../../../shared/services/field-data.service';
import { InformalAssessmentEditService } from '../../../shared/services/informal-assessment-edit.service';
import { ModalService } from 'src/app/core/modal/modal.service';
import { ModelErrors } from '../../../shared/interfaces/model-errors';
import { NonCustodialParentsSection, NonCustodialChild } from '../../../shared/models/non-custodial-parents-section';
import { ParticipantService } from '../../../shared/services/participant.service';
import { SectionComponent } from '../../../shared/interfaces/section-component';
import { ValidationManager } from '../../../shared/models/validation-manager';
import { take } from 'rxjs/operators';
import { AppService } from 'src/app/core/services/app.service';

@Component({
  selector: 'app-ncp-edit',
  templateUrl: 'ncp.component.html',
  styleUrls: ['./edit.component.css']
})
export class NcpEditComponent extends BaseInformalAssessmentSecton implements OnInit, OnDestroy, SectionComponent {
  private actionNeededSub: Subscription;
  private contactIntervalSub: Subscription;
  private ncpRelationshipSub: Subscription;
  private partSub: Subscription;
  private polarInputsSub: Subscription;
  private sectionSub: Subscription;
  public actionNeededs: ActionNeeded[];
  public careTakersDrop: DropDownField[] = [];
  public contactIntervalDrop: DropDownField[];
  public ncpRelationshipDrop: DropDownField[];
  public polarDrop: DropDownField[] = [];
  public isActive = true;
  public model: NonCustodialParentsSection;
  public modelErrors: ModelErrors = {}; // Note: this must be initialized to an empty object for the UI in initial state.
  public validationManager: ValidationManager = new ValidationManager(this.appService);
  public cached = new NonCustodialParentsSection();
  public pin: string;

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
    this.pin = this.eiaService.getPin();
    this.eiaService.setSectionComponent(this);

    forkJoin(
      this.partService.getParticipant(this.pin).pipe(take(1)),
      this.fdService.getNcpActionNeeded().pipe(take(1)),
      this.fdService.getNcpRelationships().pipe(take(1)),
      this.fdService.getPolarUnknown().pipe(take(1)),
      this.fdService.getContactIntervals().pipe(take(1))
    )
      .pipe(take(1))
      .subscribe(results => {
        this.initParticipantModel(results[0]);
        this.initNcpActionNeeded(results[1]);
        this.initNcpRelationships(results[2]);
        this.initPolarInputs(results[3]);
        this.initContactIntervals(results[4]);
        this.loadModel();
      });
  }

  private loadModel() {
    this.sectionSub = this.eiaService.getNonCustodialParentsSection().subscribe(data => {
      this.initSection(data);
      // HACK: the modifiedTracker IF was added to check when loading up the main page.
      this.modifiedTrackerForcedValidation();
      this.sectionLoaded();
    });
  }

  modifiedTrackerForcedValidation() {
    if (this.eiaService.hasNonCustodialParentsValidated || this.eiaService.modifiedTracker.isNonCustodialParentsError) {
      this.isSectionValid = false;
      this.validate();
    }
  }

  private initSection(model): void {
    this.model = model;
    this.eiaService.nonCustodialParentsModel = this.model;
    // We use clone to check which fields are modified.
    this.cached = this.eiaService.lastSavedNonCustodialParentsModel;
    // Cloned string for equality checking
    this.cloneModelString = JSON.stringify(this.model);
    this.initCareTakers();
  }

  private initNcpRelationships(data): void {
    this.ncpRelationshipDrop = data;
  }

  private initContactIntervals(data): void {
    this.contactIntervalDrop = data;
  }

  private initPolarInputs(data): void {
    this.polarDrop = data;
  }

  private initCareTakers(): DropDownField[] {
    return this.model.availableCaretackers;
  }

  public moveNonCustodialChild(childMoveRequest: { nonCustodialChild: NonCustodialChild; parentGuid: string }): void {
    this.model.moveNonCustodialChild(childMoveRequest.nonCustodialChild, childMoveRequest.parentGuid);
    this.onModelChange();
  }

  public areRepeatersEmpty(): boolean {
    let isDisabled = false;
    if (this.model != null && this.model.nonCustodialCaretakers != null) {
      for (const p of this.model.nonCustodialCaretakers) {
        if (p.isEmpty() === false) {
          isDisabled = true;
        }
      }
    }
    return isDisabled;
  }

  private initNcpActionNeeded(data) {
    this.actionNeededs = data;
  }

  validate(): void {
    // Clear all previous errors.
    this.validationManager.resetErrors();

    // Call the model's validate method.
    const result = this.model.validate(this.validationManager, this.polarDrop, this.ncpRelationshipDrop, this.contactIntervalDrop, this.participantDOB.format('MM/DD/YYYY'));

    // Update our properties so the UI can bind to the results.
    this.isSectionValid = result.isValid;
    this.modelErrors = result.errors;

    if (this.isSectionValid === true) {
      this.eiaService.sectionIsNowValid('non-custodial-parents');
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
    const clone = new NonCustodialParentsSection();
    NonCustodialParentsSection.clone(this.model, clone);
    this.eiaService.setModifiedModel('non-custodial-parents', JSON.stringify(this.cached) !== JSON.stringify(clone));
  }

  prepareToSaveWithErrors(): NonCustodialParentsSection {
    // make a clone
    const model = JSON.parse(JSON.stringify(this.model)) as NonCustodialParentsSection;

    // Fix any potential unsavable issues or ones which the API
    // doesn't handle very well.
    // Here we cleanse the date of birth of child.
    if (model.nonCustodialCaretakers != null) {
      for (const caretaker of model.nonCustodialCaretakers) {
        if (caretaker.nonCustodialChilds != null) {
          for (const child of caretaker.nonCustodialChilds) {
            if (!moment(child.dateOfBirth, 'MM/DD/YYYY').isValid()) {
              child.dateOfBirth = '';
            }
          }
        }
      }
    }
    return model;
  }

  refreshModel(): void {
    if (this.model != null && this.eiaService.nonCustodialParentsModel != null) {
      this.initSection(this.eiaService.nonCustodialParentsModel);
      this.modifiedTrackerForcedValidation();
    }

    this.isActive = false;
    setTimeout(() => (this.isActive = true), 0);
  }

  ngOnDestroy(): void {
    if (this.sectionSub != null) {
      this.sectionSub.unsubscribe();
    }
    if (this.actionNeededSub != null) {
      this.actionNeededSub.unsubscribe();
    }
    if (this.contactIntervalSub != null) {
      this.contactIntervalSub.unsubscribe();
    }
    if (this.ncpRelationshipSub != null) {
      this.ncpRelationshipSub.unsubscribe();
    }
    if (this.partSub != null) {
      this.partSub.unsubscribe();
    }
  }
}
