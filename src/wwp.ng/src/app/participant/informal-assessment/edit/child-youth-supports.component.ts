import { AppService } from './../../../core/services/app.service';
import { Component, OnInit, OnDestroy, ViewContainerRef } from '@angular/core';
import { Subscription, forkJoin } from 'rxjs';
import { ActionNeeded } from '../../../features-modules/actions-needed/models/action-needed';
import { BaseInformalAssessmentSecton } from './base-informal-assessment';
import { ChildAndYouthSupportsSection, Child, Teen } from '../../../shared/models/child-youth-supports-section';
import { DropDownField } from '../../../shared/models/dropdown-field';
import { FieldDataService } from '../../../shared/services/field-data.service';
import { InformalAssessmentEditService } from '../../../shared/services/informal-assessment-edit.service';
import { LogService } from '../../../shared/services/log.service';
import { ModelErrors } from '../../../shared/interfaces/model-errors';
import { ModalService } from 'src/app/core/modal/modal.service';
import { ParticipantService } from '../../../shared/services/participant.service';
import { SectionComponent } from '../../../shared/interfaces/section-component';
import { ValidationManager } from '../../../shared/models/validation-manager';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-child-youth-supports-edit',
  templateUrl: 'child-youth-supports.component.html',
  styleUrls: ['./edit.component.css']
})
export class ChildYouthSupportsEditComponent extends BaseInformalAssessmentSecton implements OnInit, OnDestroy, SectionComponent {
  public actionNeededs: ActionNeeded[];
  public childChildCareArrangements: DropDownField[];
  public hasChildWelfareWorkerDrop: DropDownField[];
  public model: ChildAndYouthSupportsSection;
  public modelErrors: ModelErrors = {}; // Note: this must be initialized to an empty object for the UI in initial state.
  public validationManager: ValidationManager = new ValidationManager(this.appService);
  public cached = new ChildAndYouthSupportsSection();

  private sectionSub: Subscription;

  constructor(
    private appService: AppService,
    private eiaService: InformalAssessmentEditService,
    private fdService: FieldDataService,
    private partService: ParticipantService,
    private logService: LogService,
    viewContainer: ViewContainerRef,
    public modalService: ModalService
  ) {
    super(modalService, eiaService);
  }

  ngOnInit() {
    this.eiaService.setSectionComponent(this);

    forkJoin(
      this.partService.getParticipant(this.eiaService.getPin()).pipe(take(1)),
      this.fdService.getChildYouthSupportsActionNeeded().pipe(take(1)),
      this.fdService.getChildCareArrangements().pipe(take(1)),
      this.fdService.getPolarUnknown().pipe(take(1))
    )
      .pipe(take(1))
      .subscribe(results => {
        this.initParticipantModel(results[0]);
        this.initChildCareActionNeeded(results[1]);
        this.initChildCareArrangements(results[2]);
        this.initChildWelfareWorkerDrop(results[3]);
        this.loadModel();
      });
  }

  private loadModel() {
    this.sectionSub = this.eiaService.getChildCareSection().subscribe(data => {
      this.initSection(data);
      this.modifiedTrackerForcedValidation();
      this.sectionLoaded();
    });
  }

  modifiedTrackerForcedValidation() {
    if (this.eiaService.hasChildCareValidated || this.eiaService.modifiedTracker.isChildCareError) {
      this.isSectionValid = false;
      this.validate();
    }
  }

  ngOnDestroy() {
    if (this.sectionSub != null) {
      this.sectionSub.unsubscribe();
    }
  }

  private initSection(model: ChildAndYouthSupportsSection) {
    // Ask the service if there are server errors.
    this.model = model;
    this.eiaService.childAndYouthSupportsModel = model;

    // We use clone to check which fields are modified.
    this.cached = this.eiaService.lastSavedChildAndYouthSupportsModel;

    // Cloned string for equality checking
    this.cloneModelString = JSON.stringify(model);

    // Pushes blank object inside of repeater when model has none.
    if (model.children != null) {
      if (model.children.length === 0) {
        const x = new Child();
        x.id = 0;
        this.model.children.push(x);
      }
    }

    if (model.teens != null) {
      if (model.teens.length === 0) {
        const x = new Teen();
        x.id = 0;
        this.model.teens.push(x);
      }
    }
  }

  private initChildCareActionNeeded(data) {
    this.actionNeededs = data;
  }

  private initChildCareArrangements(data) {
    this.childChildCareArrangements = data;
  }

  private initChildWelfareWorkerDrop(data) {
    this.hasChildWelfareWorkerDrop = data;
  }

  onModelChange() {
    // See if we are in "check state"
    if (!this.isSectionValid) {
      this.validate();
    }

    this.eiaService.setModifiedModel('child-youth-supports', this.cloneModelString !== JSON.stringify(this.eiaService.childAndYouthSupportsModel));
  }

  // Validation For model Object
  validate(): void {
    // Clear all previous errors.
    this.validationManager.resetErrors();

    // Call the model's validate method.
    const result = this.model.validate(this.validationManager, this.participant, this.logService);

    // Update our properties so the UI can bind to the results.
    this.isSectionValid = result.isValid;
    this.modelErrors = result.errors;

    if (this.isSectionValid === true) {
      this.eiaService.sectionIsNowValid('child-youth-supports');
    }
  }

  // NOTE: If this method changes, the participant
  isParticipantUnder26(): boolean {
    if (this.model == null) {
      return false;
    }

    return this.model.isParticipantUnder26(this.participant);
  }

  isValid(): boolean {
    return this.isSectionValid;
  }

  prepareToSaveWithErrors(): ChildAndYouthSupportsSection {
    // make a clone
    const model = JSON.parse(JSON.stringify(this.model)) as ChildAndYouthSupportsSection;

    // Fix any potential unsavable issues.
    if (model.children != null && model.children.length > 0) {
      for (let i = 0; i < model.children.length; i++) {
        if (model.children[i].dateOfBirth != null && model.children[i].dateOfBirth !== '' && model.children[i].dateOfBirth.length !== 10) {
          model.children[i].dateOfBirth = '';
        }
      }
    }

    if (model.teens != null && model.teens.length > 0) {
      for (let i = 0; i < model.teens.length; i++) {
        if (model.teens[i].dateOfBirth != null && model.teens[i].dateOfBirth !== '' && model.teens[i].dateOfBirth.length !== 10) {
          model.teens[i].dateOfBirth = '';
        }
      }
    }

    return model;
  }

  refreshModel() {
    if (this.model != null && this.eiaService.childAndYouthSupportsModel != null) {
      this.initSection(this.eiaService.childAndYouthSupportsModel);
      this.modifiedTrackerForcedValidation();
    }

    this.isSectionActive = false;
    setTimeout(() => (this.isSectionActive = true), 0);
  }
}
