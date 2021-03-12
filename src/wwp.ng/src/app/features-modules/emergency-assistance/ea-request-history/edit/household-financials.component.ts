import { EAViewModes } from './../../models/ea-request-sections.enum';
import { FieldDataTypes } from './../../../../shared/enums/field-data-types.enum';
import { FieldDataService } from 'src/app/shared/services/field-data.service';
import { DropDownField } from './../../../../shared/models/dropdown-field';
import { EAHouseholdFinancialsSection, EAHouseholdIncomes, EAAssets, EAVehicles } from './../../models/ea-request-financials.model';
import { SectionComponent } from 'src/app/shared/interfaces/section-component';
import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { ValidationManager } from 'src/app/shared/models/validation';
import { EARequestEditService } from '../../services/ea-request-edit.service';
import { ModelErrors } from 'src/app/shared/interfaces/model-errors';
import { AppService } from 'src/app/core/services/app.service';
import { ActivatedRoute } from '@angular/router';
import { concatMap, take } from 'rxjs/operators';
import { forkJoin } from 'rxjs';
import { EARequestSections, EAStatusCodes } from '../../models/ea-request-sections.enum';
import * as moment from 'moment';
import * as _ from 'lodash';

@Component({
  selector: 'app-ea-request-edit-household-financials',
  templateUrl: './household-financials.component.html',
  styles: []
})
export class EARequestHouseholdFinancialsEditComponent implements OnInit, OnDestroy, SectionComponent {
  requestId: string;
  pin: string;
  viewMode = EAViewModes.View;
  isSectionLoaded = false;
  public isActive = true;
  model: EAHouseholdFinancialsSection = new EAHouseholdFinancialsSection();
  cachedModel: EAHouseholdFinancialsSection;
  incomeVerificationDrop: DropDownField[] = [];
  assetVerificationDrop: DropDownField[] = [];
  vehicleVerificationDrop: DropDownField[] = [];
  vehicleValueVerificationDrop: DropDownField[] = [];
  groupMembersDrop: DropDownField[] = [];
  validationManager: ValidationManager = new ValidationManager(this.appService);
  public modelErrors: ModelErrors = {};
  public isSectionValid = true;
  public isSectionModified = false;
  public hadSaveError = false;
  public hasTriedSave = false;
  public isReadOnly = false;
  public incomeBackToDate: string;
  totalVehicleAssets = 0;
  totalFinancialAssets = 0;
  totalAssets = '0.00';

  constructor(
    private requestEditService: EARequestEditService,
    private appService: AppService,
    private route: ActivatedRoute,
    private fdService: FieldDataService,
    private cdr: ChangeDetectorRef
  ) {
    scrollTo(0, 0);
  }

  ngOnInit() {
    this.requestEditService.setSectionComponent(this);
    this.isSectionLoaded = true;
    this.route.parent.params
      .pipe(
        concatMap(params => {
          this.requestId = params['id'];
          this.pin = params['pin'];
          this.viewMode = params['mode'];
          return forkJoin(this.requestEditService.getEARequest(this.pin, this.requestId), this.fdService.getFieldDataByField(FieldDataTypes.EAVerifications));
        })
      )
      .pipe(take(1))
      .subscribe(result => {
        this.initSection(result[0].eaHouseHoldFinancials);
        this.incomeVerificationDrop = result[1].filter(i => i.isIncome);
        this.assetVerificationDrop = result[1].filter(i => i.isAsset);
        this.vehicleVerificationDrop = result[1].filter(i => i.isVehicle);
        this.vehicleValueVerificationDrop = result[1].filter(i => i.isVehicleValue);
        this.modifiedTrackerForcedValidation();
        this.incomeBackToDate = moment(result[0].eaDemographics.applicationDate)
          .subtract(30, 'days')
          .toString();
        this.requestEditService.lastSavedModel.eaGroupMembers.eaGroupMembers.forEach(x => {
          if (x.isIncluded) {
            const item = new DropDownField();
            item.id = x.participantId;
            item.name = x.participantName;
            this.groupMembersDrop.push(item);
          }
        });
        this.isSectionLoaded = true;
      });
  }

  private initSection(model: EAHouseholdFinancialsSection) {
    this.model = model;
    this.requestEditService.model.eaHouseHoldFinancials = model;
    this.cachedModel = this.requestEditService.lastSavedModel.eaHouseHoldFinancials;
    this.isReadOnly =
      this.viewMode === EAViewModes.View || (this.requestEditService.lastSavedModel.statusCode !== EAStatusCodes.InProgress && !this.appService.isUserEASupervisor());
    this.setSaveButtonEnableOrDisable();
    if (model.eaHouseHoldIncomes !== null && model.eaHouseHoldIncomes.length === 0) {
      this.model.eaHouseHoldIncomes.push(EAHouseholdIncomes.create());
    }

    if (model.eaAssets !== null && model.eaAssets.length === 0) {
      this.model.eaAssets.push(EAAssets.create());
    }

    if (model.eaVehicles !== null && model.eaVehicles.length === 0) {
      this.model.eaVehicles.push(EAVehicles.create());
    }
  }

  calculateTotalAssets() {
    let totalAsset = 0;
    if (!this.model.hasNoAssets && !this.model.hasNoVehicles) {
      totalAsset = this.totalVehicleAssets + this.totalFinancialAssets;
    } else if (this.model.hasNoAssets && !this.model.hasNoVehicles) {
      totalAsset = this.totalVehicleAssets;
    } else if (!this.model.hasNoAssets && this.model.hasNoVehicles) {
      totalAsset = this.totalVehicleAssets;
    } else totalAsset = 0;
    this.totalAssets = totalAsset.toLocaleString('en-US', { minimumFractionDigits: 2 });
    this.cdr.detectChanges();
  }

  isValid(): boolean {
    return this.isSectionValid;
  }

  scrollToTop() {
    scrollTo(0, 0);
  }

  prepareToSaveWithErrors() {
    if (this.model.hasNoIncome) this.model.eaHouseHoldIncomes = [];
    if (this.model.hasNoAssets) this.model.eaAssets = [];
    if (this.model.hasNoVehicles) this.model.eaVehicles = [];

    this.model.eaHouseHoldIncomes = this.model.eaHouseHoldIncomes.filter(x => !x.isEmpty());
    this.model.eaAssets = this.model.eaAssets.filter(x => !x.isEmpty());
    this.model.eaVehicles = this.model.eaVehicles.filter(x => !x.isEmpty());
    return this.model;
  }

  openHelp() {}

  refreshModel() {
    if (this.model != null && this.requestEditService.model.eaHouseHoldFinancials != null) {
      this.initSection(this.requestEditService.model.eaHouseHoldFinancials);
      this.modifiedTrackerForcedValidation();
    }
    this.isActive = false;
    setTimeout(() => (this.isActive = true), 0);
  }

  modifiedTrackerForcedValidation() {
    if (this.requestEditService.modifiedTracker.isHouseholdFinancials.validated || this.requestEditService.modifiedTracker.isHouseholdFinancials.error) {
      this.isSectionValid = false;
      this.validate();
    }
  }

  validate(): void {
    this.validationManager.resetErrors();
    this.isSectionValid = false;
    this.modelErrors = {};
    const result = this.model.validate(this.validationManager);
    this.isSectionValid = result.isValid;
    this.modelErrors = result.errors;
    if (this.isSectionValid) {
      this.requestEditService.sectionIsNowValid(this.requestEditService.getSectionLabel(EARequestSections.Financials));
    }
    this.setSaveButtonEnableOrDisable();
  }

  setSaveButtonEnableOrDisable() {
    this.requestEditService.modifiedTracker.isSaveDisabled = this.isReadOnly || !!this.modelErrors['disableSave'];
  }

  checkState() {
    if (!this.isSectionValid) {
      this.validate();
    }
    this.requestEditService.setModifiedModel(
      this.requestEditService.getSectionLabel(EARequestSections.Financials),
      !_.isEqual(this.cachedModel, this.requestEditService.model.eaHouseHoldFinancials)
    );
  }

  ngOnDestroy() {
    this.requestEditService.modifiedTracker.isSaveDisabled = false;
  }
}
