import { AppService } from './../../../core/services/app.service';
import { Component, OnInit, OnDestroy, ViewContainerRef } from '@angular/core';
import { Subscription, forkJoin } from 'rxjs';
import * as moment from 'moment';

import { ActionNeeded } from '../../../features-modules/actions-needed/models/action-needed';
import { BaseInformalAssessmentSecton } from './base-informal-assessment';
import { DropDownField } from '../../../shared/models/dropdown-field';
import { FieldDataService } from '../../../shared/services/field-data.service';
import { InformalAssessmentEditService } from '../../../shared/services/informal-assessment-edit.service';
import { LegalIssuesSection, CriminalCharge, CourtDate } from '../../../shared/models/legal-issues-section';
import { ModalService } from 'src/app/core/modal/modal.service';
import { ModelErrors } from '../../../shared/interfaces/model-errors';
import { Participant } from '../../../shared/models/participant';
import { ParticipantService } from '../../../shared/services/participant.service';
import { SectionComponent } from '../../../shared/interfaces/section-component';
import { ValidationManager } from '../../../shared/models/validation-manager';
import { Utilities } from '../../../shared/utilities';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-legal-issues-edit',
  templateUrl: 'legal-issues.component.html',
  styleUrls: ['./edit.component.css']
})
export class LegalIssuesEditComponent extends BaseInformalAssessmentSecton implements OnInit, OnDestroy, SectionComponent {
  private sectionSub: Subscription;

  public participant: Participant;
  public model: LegalIssuesSection;
  public isActive = true;
  public modelErrors: ModelErrors = {}; // Note: this must be initialized to an empty object for the UI in initial state.
  public validationManager: ValidationManager = new ValidationManager(this.appService);

  actionNeededs: ActionNeeded[];
  pendingTypesDrop: DropDownField[] = [];

  dischargeDrop: DropDownField[] = [];
  rankDrop: DropDownField[] = [];
  isHasTrainingInvalid = false;
  isBranchInvalid = false;
  isRankInvalid = false;
  isYearsInvalid = false;
  isDischargeInvalid = false;

  public cached = new LegalIssuesSection();

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
      this.fdService.getLegaIssuesActionNeeded().pipe(take(1)),
      this.fdService.getPendingTypes().pipe(take(1))
    )
      .pipe(take(1))
      .subscribe(results => {
        this.initParticipantModel(results[0]);
        this.initLegalIssuesActionNeeded(results[1]);
        this.initPendingTypesDropDown(results[2]);
        this.loadModel();
      });
  }

  private loadModel() {
    this.sectionSub = this.eiaService.getLegalIssuesSection().subscribe(data => {
      this.initSection(data);
      // HACK: the modifiedTracker IF was added to check when loading up the main page.
      this.modifiedTrackerForcedValidation();
      this.sectionLoaded();
    });
  }

  modifiedTrackerForcedValidation() {
    if (this.eiaService.hasLegalIssuesValidated || this.eiaService.modifiedTracker.isLegalIssuesError) {
      this.isSectionValid = false;
      this.validate();
    }
  }

  private initLegalIssuesActionNeeded(data) {
    this.actionNeededs = data;
  }

  private initPendingTypesDropDown(data) {
    this.pendingTypesDrop = data;
  }

  ngOnDestroy() {
    if (this.sectionSub != null) {
      this.sectionSub.unsubscribe();
    }
  }

  private initSection(model: LegalIssuesSection) {
    // Ask the service if there are server errors.

    this.model = model;
    this.eiaService.legalIssuesModel = model;

    // We use clone to check which fields are modified.
    this.cached = this.eiaService.lastSavedLegalIssuesModel;

    // Cloned string for equality checking
    this.cloneModelString = JSON.stringify(model);

    // Pushes blank object inside of repeater when model has none.
    if (model.convictions != null) {
      if (model.convictions.length === 0) {
        const x = new CriminalCharge();
        x.id = 0;
        this.model.convictions.push(x);
      }
    }

    if (model.pendings != null) {
      if (model.pendings.length === 0) {
        const x = new CriminalCharge();
        x.id = 0;
        this.model.pendings.push(x);
      }
    }

    if (model.courtDates != null) {
      if (model.courtDates.length === 0) {
        const x = new CourtDate();
        x.id = 0;
        this.model.courtDates.push(x);
      }
    }
  }

  public isConvictionsRepeaterEmpty(): boolean {
    let isDisabled = false;
    if (this.model != null && this.model.convictions != null) {
      for (const p of this.model.convictions) {
        if (p.isEmpty() === false) {
          isDisabled = true;
        }
      }
    }
    return isDisabled;
  }

  public isCourtDatesRepeaterEmpty(): boolean {
    let isDisabled = false;
    if (this.model != null && this.model.courtDates != null) {
      for (const p of this.model.courtDates) {
        if (p.isEmpty() === false) {
          isDisabled = true;
        }
      }
    }
    return isDisabled;
  }

  public isPendingsRepeaterEmpty(): boolean {
    let isDisabled = false;
    if (this.model != null && this.model.pendings != null) {
      for (const p of this.model.pendings) {
        if (p.isEmpty() === false) {
          isDisabled = true;
        }
      }
    }
    return isDisabled;
  }

  checkState() {
    if (!this.isSectionValid) {
      this.validate();
    }

    this.eiaService.setModifiedModel('legal-issues', this.cloneModelString !== JSON.stringify(this.eiaService.legalIssuesModel));
  }

  validate(): void {
    // Assume it's valid to start
    this.isSectionValid = true;

    // Reset the errors.
    this.validationManager.resetErrors();

    const result = this.model.validate(this.validationManager, this.participantDOB.format('MM/DD/YYYY'));

    // Update our properties so the UI can bind to the results.
    this.isSectionValid = result.isValid;
    this.modelErrors = result.errors;

    if (this.isSectionValid === true) {
      this.eiaService.sectionIsNowValid('legal-issues');
    }
  }

  isValid(): boolean {
    return this.isSectionValid;
  }

  prepareToSaveWithErrors(): LegalIssuesSection {
    // make a clone
    const model = JSON.parse(JSON.stringify(this.model)) as LegalIssuesSection;

    for (let i = 0; i < model.convictions.length; i++) {
      if (model.convictions[i].date != null && model.convictions[i].date !== '') {
        const valueDateString = Utilities.parseMmYyyyIntoMmDdYyyyString(model.convictions[i].date);
        const inputDate = moment(valueDateString, 'MM-DD-YYYY');

        if (!inputDate.isValid()) {
          model.convictions[i].date = '';
        }
      }
    }

    for (let i = 0; i < model.pendings.length; i++) {
      if (model.pendings[i].date != null && model.pendings[i].date !== '') {
        const valueDateString = Utilities.parseMmYyyyIntoMmDdYyyyString(model.pendings[i].date);
        const inputDate = moment(valueDateString, 'MM-DD-YYYY');

        if (!inputDate.isValid()) {
          model.pendings[i].date = '';
        }
      }
    }

    for (let i = 0; i < model.courtDates.length; i++) {
      if (model.courtDates[i].date != null && model.courtDates[i].date !== '') {
        const inputDate = moment(model.courtDates[i].date, 'MM-DD-YYYY');

        if (!inputDate.isValid()) {
          model.courtDates[i].date = '';
        }
      }
    }

    return model;
  }

  refreshModel() {
    if (this.model != null && this.eiaService.legalIssuesModel != null) {
      this.initSection(this.eiaService.legalIssuesModel);
      this.modifiedTrackerForcedValidation();
    }

    this.isActive = false;
    setTimeout(() => (this.isActive = true), 0);
  }
}
