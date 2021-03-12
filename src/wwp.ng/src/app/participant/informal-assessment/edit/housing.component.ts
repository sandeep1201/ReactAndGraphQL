import { AppService } from './../../../core/services/app.service';
import { coerceNumber } from '../../../shared/decorators/number-property';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription, forkJoin } from 'rxjs';
import * as moment from 'moment';
import { BaseInformalAssessmentSecton } from './base-informal-assessment';
import { DropDownField } from '../../../shared/models/dropdown-field';
import { FieldDataService } from '../../../shared/services/field-data.service';
import { HousingSection, HousingHistory } from '../../../shared/models/housing-section';
import { InformalAssessmentEditService } from '../../../shared/services/informal-assessment-edit.service';
import { Participant } from '../../../shared/models/participant';
import { ParticipantService } from '../../../shared/services/participant.service';
import { SectionComponent } from '../../../shared/interfaces/section-component';
import { Utilities } from '../../../shared/utilities';
import { ValidationManager } from '../../../shared/models/validation-manager';
import { ModelErrors } from '../../../shared/interfaces/model-errors';
import { ModalService } from 'src/app/core/modal/modal.service';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-housing-edit',
  templateUrl: 'housing.component.html',
  styleUrls: ['./edit.component.css']
})
export class HousingEditComponent extends BaseInformalAssessmentSecton implements OnInit, OnDestroy, SectionComponent {
  @coerceNumber() public homelessId: number;
  private otherHousingSituationId: number;
  private isCurrentHousingPart2Valid = true;
  private isCurrentHousingValid = true;
  private sectionSub: Subscription;
  public validationManager: ValidationManager = new ValidationManager(this.appService);
  public validationManagerCurrentHousing: ValidationManager = new ValidationManager(this.appService);
  public validationManagerCurrentHousingPart2: ValidationManager = new ValidationManager(this.appService);
  public validationManagerHistoricalHousing: ValidationManager = new ValidationManager(this.appService);
  public modelErrors: ModelErrors = {}; // Note: this must be initialized to an empty object for the UI in initial state
  public CurrentModelErrors: ModelErrors = {}; // Note: this must be initialized to an empty object for the UI in initial state
  public modelErrors2: ModelErrors = {}; // Note: this must be initialized to an empty object for the UI in initial state
  public housingSituationsDrop: DropDownField[];
  public isActive = true;
  public isForcedAndEndDateVisible = false;
  public isHistoricalBeingEdited = false;
  public model: HousingSection;
  public originalModel: HousingSection;
  public participant: Participant;

  constructor(
    private appService: AppService,
    private eiaService: InformalAssessmentEditService,
    private fdService: FieldDataService,
    private partService: ParticipantService,
    public modalService: ModalService
  ) {
    super(modalService, eiaService);
  }

  ngOnInit() {
    this.eiaService.setSectionComponent(this);

    forkJoin(this.partService.getParticipant(this.eiaService.getPin()).pipe(take(1)), this.fdService.getHousingSituations().pipe(take(1)))
      .pipe(take(1))
      .subscribe(results => {
        this.initParticipantModel(results[0]);
        this.initHousingSituationsDropDowns(results[1]);
        this.loadModel();
      });
  }

  private loadModel() {
    this.sectionSub = this.eiaService.getHousingSection().subscribe(data => {
      this.initSection(data);
      // HACK: the modifiedTracker IF was added to check when loading up the main page.
      this.modifiedTrackerForcedValidation();
      this.sectionLoaded();
    });
  }

  modifiedTrackerForcedValidation() {
    if (this.eiaService.hasHousingValidated || this.eiaService.modifiedTracker.isHousingError) {
      this.isSectionValid = false;
      this.validate();
    }
  }

  private initSection(model: HousingSection) {
    this.model = model;
    this.eiaService.housingModel = model;
    this.originalModel = new HousingSection();
    this.originalModel = this.eiaService.lastSavedHousingModel;
    this.cloneModelString = JSON.stringify(model);
  }

  private initHousingSituationsDropDowns(data) {
    this.housingSituationsDrop = data;
    this.homelessId = Utilities.idByFieldDataName('Homeless (Outside of shelter)', data);
    this.otherHousingSituationId = Utilities.idByFieldDataName('Other', data);
  }

  validateCurrentHousingInitial() {
    this.isCurrentHousingValid = true;

    this.resetValidation();
    const result = this.model.validateCurrentHousingInitial(this.validationManager, this.participant.dateOfBirth, this.otherHousingSituationId, this.homelessId);
    this.modelErrors = result.errors;
    this.isCurrentHousingValid = result.isValid;
  }

  validateCurrentHousingAfterMove() {
    this.isCurrentHousingPart2Valid = true;
    this.resetValidation();
    const result = this.model.validateCurrentHousingAfterMove(this.validationManager, this.participant.dateOfBirth, this.otherHousingSituationId, this.homelessId);
    this.isCurrentHousingPart2Valid = result.isValid;
    this.modelErrors = result.errors;
  }

  isDetailsRequired() {
    return this.model.isDetailsRequired(this.otherHousingSituationId, this.homelessId);
  }

  resetValidation() {
    this.validationManager.resetErrors();
  }

  validate() {
    // Clear all previous errors.
    this.resetValidation();
    // Call the model's validate method.
    const result = this.model.validate(
      this.validationManager,
      this.participant.dateOfBirth,
      this.otherHousingSituationId,
      this.homelessId,
      this.isHistoricalBeingEdited,
      this.isForcedAndEndDateVisible
    );

    this.isSectionValid = result.isValid;
    this.modelErrors = result.errors;

    if (this.isSectionValid === true) {
      this.eiaService.sectionIsNowValid('housing');
    }
  }

  isValid(): boolean {
    if (this.isSectionValid === false || this.isCurrentHousingValid === false || this.isCurrentHousingPart2Valid === false) {
      return false;
    } else {
      return true;
    }
  }

  editState(inEdit: boolean) {
    this.isHistoricalBeingEdited = inEdit;
    this.checkState();
  }

  isForcedEvictedQuestionDisplayed(): boolean {
    return this.model.isForcedEvictedQuestionDisplayed(this.homelessId);
  }

  prepareToSaveWithErrors(): HousingSection {
    // make a clone
    const model = JSON.parse(JSON.stringify(this.model)) as HousingSection;

    // CurrentHousingBeginDate must be a valid date.
    if (model.currentHousingBeginDate != null && model.currentHousingBeginDate.length === 7) {
      const currentHousingBeginDate = moment(model.currentHousingBeginDate, 'MM/YYYY');
      if (!currentHousingBeginDate.isValid()) {
        model.currentHousingBeginDate = null;
      }
    } else {
      model.currentHousingBeginDate = null;
    }

    // CurrentHousingEndDate must be a valid date.
    if (model.currentHousingEndDate != null && model.currentHousingEndDate.length === 7) {
      const currentHousingEndDate = moment(model.currentHousingEndDate, 'MM/YYYY');
      if (!currentHousingEndDate.isValid()) {
        model.currentHousingEndDate = null;
      }
    } else {
      model.currentHousingEndDate = null;
    }

    return model;
  }

  refreshModel() {
    if (this.model != null && this.eiaService.housingModel != null) {
      this.initSection(this.eiaService.housingModel);
      this.modifiedTrackerForcedValidation();
    }

    this.isActive = false;
    setTimeout(() => (this.isActive = true), 0);
  }

  ngOnDestroy() {
    if (this.sectionSub != null) {
      this.sectionSub.unsubscribe();
    }
  }

  checkState() {
    if (!this.isSectionValid) {
      this.validate();
    }

    this.eiaService.setModifiedModel('housing', this.cloneModelString !== JSON.stringify(this.eiaService.housingModel));
  }

  checkCurrentState() {
    if (!this.isCurrentHousingValid) {
      this.validateCurrentHousingInitial();
    }
    this.eiaService.setModifiedModel('housing', this.cloneModelString !== JSON.stringify(this.eiaService.housingModel));
  }

  checkCurrentStatePart2() {
    if (!this.isCurrentHousingPart2Valid) {
      this.validateCurrentHousingAfterMove();
    }
    this.eiaService.setModifiedModel('housing', this.cloneModelString !== JSON.stringify(this.eiaService.housingModel));
  }

  // Page Methods.
  showForcedAndEndDate(beVisible: boolean) {
    if (beVisible === true) {
      this.validateCurrentHousingInitial();
      if (this.isSectionValid === false) {
        this.validate();
      }
    }

    if (this.isCurrentHousingValid === true && beVisible === true) {
      this.isForcedAndEndDateVisible = beVisible;
    }

    if (beVisible === false) {
      this.model.currentHousingEndDate = '';
      this.model.hasBeenEvicted = null;
      this.validationManager.resetErrors();
      this.modelErrors['hasBeenEvicted'] = false;
      this.modelErrors['currentHousingEndDate'] = false;
      this.modelErrors['currentHousingDetails'] = false;
      this.isForcedAndEndDateVisible = beVisible;
      if (this.isSectionValid === false) {
        this.validate();
      }
    }
  }

  eraseCurrentHousing() {
    if (this.model != null) {
      this.model.housingSituationId = null;
      this.model.currentHousingBeginDate = '';
      this.model.currentMonthlyAmount = null;
      this.model.isCurrentAmountUnknown = null;
      this.model.hasCurrentEvictionRisk = null;
      this.model.currentHousingEndDate = '';
      this.model.currentHousingDetails = null;
    }
  }

  eraseCurrentHousingPart2() {
    if (this.model != null) {
      this.model.hasBeenEvicted = null;
      this.model.currentHousingEndDate = '';
      this.model.currentHousingDetails = null;
    }
  }

  moveCurrentHousingToHistory() {
    this.validateCurrentHousingAfterMove();

    if (this.model != null && this.isCurrentHousingPart2Valid === true) {
      const hh = new HousingHistory();

      hh.id = 0;
      hh.historyType = this.model.housingSituationId;
      hh.beginDate = this.model.currentHousingBeginDate;
      hh.isAmountUnknown = this.model.isCurrentAmountUnknown;
      if (hh.isAmountUnknown !== true) {
        hh.monthlyAmount = this.model.currentMonthlyAmount;
      }
      hh.hasEvicted = this.model.hasBeenEvicted;
      hh.endDate = this.model.currentHousingEndDate;
      hh.details = this.model.currentHousingDetails;

      this.model.histories.push(hh);
      this.isForcedAndEndDateVisible = false;

      this.eraseCurrentHousing();
      this.eraseCurrentHousingPart2();
    }

    this.originalModel.housingSituationId = this.model.housingSituationId;
    this.originalModel.currentHousingBeginDate = this.model.currentHousingBeginDate;
    this.originalModel.currentHousingEndDate = this.model.currentHousingEndDate;
    this.originalModel.hasCurrentEvictionRisk = this.model.hasCurrentEvictionRisk;
    this.originalModel.currentHousingDetails = this.model.currentHousingDetails;

    if (this.isSectionValid === false) {
      this.validate();
    }
  }
}
