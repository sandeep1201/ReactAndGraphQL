// tslint:disable: import-blacklist
// tslint:disable: no-shadowed-variable
import { Component, OnInit, OnDestroy, ViewContainerRef } from '@angular/core';
import { Subscription, Observable, forkJoin } from 'rxjs';
import { AppService } from 'src/app/core/services/app.service';
import { BaseInformalAssessmentSecton } from './base-informal-assessment';
import { DropDownField } from '../../../shared/models/dropdown-field';
import { Employment } from '../../../shared/models/work-history-app';
import { FieldDataService } from '../../../shared/services/field-data.service';
import { InformalAssessmentEditService } from '../../../shared/services/informal-assessment-edit.service';
import { ModalService } from 'src/app/core/modal/modal.service';
import { ModelErrors } from '../../../shared/interfaces/model-errors';
import { SectionComponent } from '../../../shared/interfaces/section-component';
import { Utilities } from '../../../shared/utilities';
import { ValidationManager } from '../../../shared/models/validation-manager';
import { WorkHistorySection } from '../../../shared/models/work-history-section';
import { WorkHistoryAppService } from '../../../shared/services/work-history-app.service';
import { RfaService } from '../../../shared/services/rfa.service';
import { ParticipantService } from '../../../shared/services/participant.service';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-work-history-gatepost-edit',
  templateUrl: 'work-history.component.html',
  styleUrls: ['./edit.component.css'],
  providers: [RfaService]
})
export class WorkHistoryGatepostEditComponent extends BaseInformalAssessmentSecton implements OnInit, OnDestroy, SectionComponent {
  public isActive = true;
  public model: WorkHistorySection;
  public employmentPreventionFactorsDrop: DropDownField[];
  public workHistoryEmploymentStatuses: DropDownField[];
  public modelErrors: ModelErrors = {};
  public fullTimeId: number;
  public partTimeId: number;
  public unemployedId: number;
  public cached = new WorkHistorySection();
  public employments: Employment[] = [];
  public hasEmploymentsLoaded = false;
  public originalEmployments: Employment[] = [];
  private sectionSub: Subscription;
  public pin: string;
  public polarDrop: DropDownField[] = [];
  public yesId: number;
  public isCareerAssessmentNoteRequired = false;
  public validationManager: ValidationManager = new ValidationManager(this.appService);
  public showCareerFeature = false;
  public whReadOnly = false;
  public isReportColapsed = true;
  public viewRpeort = false;
  public hasOnlyFcdp = false;

  constructor(
    private appService: AppService,
    private eiaService: InformalAssessmentEditService,
    private fdService: FieldDataService,
    private whaService: WorkHistoryAppService,
    viewContainer: ViewContainerRef,
    public modalService: ModalService,
    private partService: ParticipantService
  ) {
    super(modalService, eiaService);
  }

  ngOnInit() {
    this.eiaService.setSectionComponent(this);
    this.whaService.setPin(this.eiaService.getPin());
    this.pin = this.eiaService.getPin();
    this.showCareerFeature = this.appService.getFeatureToggleDate('CareerAndJobReadiness');

    forkJoin(
      this.partService.getParticipant(this.pin).pipe(take(1)),
      this.fdService.getEmploymentPreventionFactors().pipe(take(1)),
      this.fdService.getWorkHistoryEmploymentStatuses().pipe(take(1)),
      this.fdService.getPolarUnknown().pipe(take(1)),
      this.eiaService.getWorkHistorySection().pipe(take(1))
    )
      .pipe(take(1))
      .subscribe(results => {
        this.initParticipantModel(results[0]);
        this.initEmploymentPreventionFactors(results[1]);
        this.initWorkHistoryEmploymentStatuses(results[2]);
        this.initPolarInputs(results[3]);
        this.loadModel(results[4]);
      });
  }

  private loadModel(data) {
    this.initSection(data);
    this.modifiedTrackerForcedValidation();
    this.sectionLoaded();
  }

  private employmentsLoaded() {
    if (this.eiaService.hasWorkHistoryValidated || this.eiaService.modifiedTracker.isWorkHistoryError) {
      this.isSectionValid = false;
      this.validate();
    }
  }

  modifiedTrackerForcedValidation() {
    if (this.eiaService.hasWorkHistoryValidated || this.eiaService.modifiedTracker.isWorkHistoryError) {
      this.isSectionValid = false;
      this.validate();
    }
  }

  initSection(model) {
    this.model = model;
    this.eiaService.workHistoryModel = model;

    // We use clone to check which fields are modified.
    this.cached = this.eiaService.lastSavedWorkHistoryModel;
    // Cloned string for equality checking
    this.cloneModelString = JSON.stringify(model);
    this.whReadOnly = !this.appService.isUserAuthorizedToEditWorkHistory(this.participant);
    this.viewRpeort = this.appService.isUserAuthorizedToViewKidsWHReport(this.participant);
    const programCds = [];
    if (this.participant) {
      this.participant.programs.forEach(i => {
        if (i.status !== 'Referred') programCds.push(i.programCd);
      });
      this.hasOnlyFcdp = this.appService.checkForFCDP(programCds);
    }
    this.isCareerAssessmentNotesRequired();
  }

  initEmploymentPreventionFactors(data) {
    this.employmentPreventionFactorsDrop = data;
  }

  // TODO revist this.
  initWorkHistoryEmploymentStatuses(data) {
    this.workHistoryEmploymentStatuses = data;
    this.unemployedId = Utilities.idByFieldDataName('Unemployed', data);
    this.partTimeId = Utilities.idByFieldDataName('Part-Time', data);
    this.fullTimeId = Utilities.idByFieldDataName('Full-Time', data);
  }

  private initPolarInputs(data): void {
    this.polarDrop = data;
    this.yesId = Utilities.idByFieldDataName('Yes', this.polarDrop);
  }

  toggleCollapse() {
    this.isReportColapsed = !this.isReportColapsed;
  }

  ngOnDestroy() {
    if (this.sectionSub != null) {
      this.sectionSub.unsubscribe();
    }
  }

  validate(): void {
    // Clear all previous errors.
    this.validationManager.resetErrors();

    // Call the model's validate method.
    const result = this.model.validate(
      this.validationManager,
      this.workHistoryEmploymentStatuses,
      this.employmentPreventionFactorsDrop,
      this.employments,
      this.polarDrop,
      this.showCareerFeature,
      this.whReadOnly
    );

    // Update our properties so the UI can bind to the results.
    this.isSectionValid = result.isValid;
    this.modelErrors = result.errors;

    if (this.isSectionValid === true) {
      this.eiaService.sectionIsNowValid('work-history');
    }
  }

  isValid(): boolean {
    return this.isSectionValid;
  }

  prepareToSaveWithErrors(): WorkHistorySection {
    // make a clone
    const model = JSON.parse(JSON.stringify(this.model)) as WorkHistorySection;

    // Fix any potential unsavable issues or ones which the API
    // doesn't handle very well.
    return model;
  }

  refreshModel() {
    if (this.model != null && this.eiaService.workHistoryModel != null) {
      this.initSection(this.eiaService.workHistoryModel);
      this.modifiedTrackerForcedValidation();
    }

    this.isActive = false;
    setTimeout(() => (this.isActive = true), 0);
  }

  checkState() {
    if (!this.isSectionValid) {
      this.validate();
    }

    this.eiaService.setModifiedModel('work-history', this.cloneModelString !== JSON.stringify(this.eiaService.workHistoryModel));
  }

  isCareerAssessmentNotesRequired() {
    if (this.model && this.model.hasCareerAssessment === this.yesId) this.isCareerAssessmentNoteRequired = true;
    else this.isCareerAssessmentNoteRequired = false;
  }

  employmentsUpdated(event) {
    this.employments = event;
  }
}
