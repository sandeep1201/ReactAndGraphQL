import { EAViewModes } from './../../models/ea-request-sections.enum';
import { FieldDataTypes } from './../../../../shared/enums/field-data-types.enum';
import { Utilities } from 'src/app/shared/utilities';
import { ActivatedRoute } from '@angular/router';
import { FieldDataService } from 'src/app/shared/services/field-data.service';
import { AppService } from 'src/app/core/services/app.service';
import { SectionComponent } from 'src/app/shared/interfaces/section-component';
import { Component, OnInit, OnDestroy, AfterViewChecked } from '@angular/core';
import { EARequestEditService } from '../../services/ea-request-edit.service';
import { ValidationManager } from 'src/app/shared/models/validation';
import { ModelErrors } from 'src/app/shared/interfaces/model-errors';
import { DropDownField } from 'src/app/shared/models/dropdown-field';
import { take, concatMap } from 'rxjs/operators';
import { forkJoin } from 'rxjs';
import { EARequestSections, EAEmergencyCodes, EAStatusCodes } from '../../models/ea-request-sections.enum';
import { EARequestEmergencyTypeSection, EAImpendingHomelessness, EAHomelessness, EAEnergyCrisis } from '../../models/ea-request-emergency-type.model';
import { FinalistAddress } from 'src/app/shared/models/finalist-address.model';
import * as _ from 'lodash';

@Component({
  selector: 'app-ea-request-edit-emergency',
  templateUrl: './emergency-type.component.html',
  styles: ['.part-title {padding-bottom: .25em; border-bottom: 1px solid #ccc;}']
})
export class EARequestEmergencyTypeEditComponent implements OnInit, OnDestroy, AfterViewChecked, SectionComponent {
  requestId: string;
  pin: string;
  viewMode = EAViewModes.View;
  isSectionLoaded = false;
  isReadOnly = false;
  public isActive = true;
  model: EARequestEmergencyTypeSection = new EARequestEmergencyTypeSection();
  cachedModel: EARequestEmergencyTypeSection;
  public eaEmergencyTypesDrop: DropDownField[] = [];
  validationManager: ValidationManager = new ValidationManager(this.appService);
  public modelErrors: ModelErrors = {};
  public isSectionValid = true;
  public isSectionModified = false;
  public hadSaveError = false;
  public hasTriedSave = false;
  public landlordAddressValidateStatus = false;
  constructor(private requestEditService: EARequestEditService, private appService: AppService, private fdService: FieldDataService, private route: ActivatedRoute) {
    scrollTo(0, 0);
  }

  ngOnInit() {
    this.requestEditService.setSectionComponent(this);
    this.route.parent.params
      .pipe(
        concatMap(params => {
          this.requestId = params['id'];
          this.pin = params['pin'];
          this.viewMode = params['mode'];
          return forkJoin(this.requestEditService.getEARequest(this.pin, this.requestId), this.fdService.getFieldDataByField(FieldDataTypes.EAEmergencyTypes));
        })
      )
      .pipe(take(1))
      .subscribe(result => {
        this.initSection(result[0].eaEmergencyType);
        this.eaEmergencyTypesDrop = result[1];
        this.modifiedTrackerForcedValidation();
        this.isSectionLoaded = true;
      });
  }

  ngAfterViewChecked() {
    this.setSaveButtonEnableOrDisable();
  }

  private initSection(model: EARequestEmergencyTypeSection) {
    this.model = model;
    this.requestEditService.model.eaEmergencyType = model;
    this.cachedModel = this.requestEditService.lastSavedModel.eaEmergencyType;
    this.isReadOnly =
      this.viewMode === EAViewModes.View || (this.requestEditService.lastSavedModel.statusCode !== EAStatusCodes.InProgress && !this.appService.isUserEASupervisor());
  }

  getEmergencyCodes(emergencyTypeIds: number[]) {
    this.model.emergencyTypeCodes = [];
    emergencyTypeIds.forEach(x => this.model.emergencyTypeCodes.push(Utilities.fieldDataCodeById(x, this.eaEmergencyTypesDrop)));
  }

  isValid(): boolean {
    return this.isSectionValid;
  }

  scrollToTop() {
    scrollTo(0, 0);
  }

  prepareToSaveWithErrors() {
    if (!this.model.emergencyTypeCodes.includes(EAEmergencyCodes.ImpendingHomelessness)) {
      this.model.eaImpendingHomelessness = new EAImpendingHomelessness();
    }
    if (!this.model.emergencyTypeCodes.includes(EAEmergencyCodes.Homelessness)) {
      this.model.eaHomelessness = new EAHomelessness();
    }
    if (!this.model.emergencyTypeCodes.includes(EAEmergencyCodes.EnergyCrisis)) {
      this.model.eaEnergyCrisis = new EAEnergyCrisis();
    }

    if (this.model.eaImpendingHomelessness) {
      if (this.model.eaImpendingHomelessness.isCurrentLandLordUnknown) {
        this.model.eaImpendingHomelessness.landLordName = null;
        this.model.eaImpendingHomelessness.landLordPhone = null;
        this.model.eaImpendingHomelessness.contactPerson = null;
        this.model.eaImpendingHomelessness.landLordAddress = new FinalistAddress();
      }
      if (!this.model.eaImpendingHomelessness.haveEvictionNotice) {
        this.model.eaImpendingHomelessness.dateOfEvictionNotice = null;
      }
      if (!this.model.eaImpendingHomelessness.needingDifferentHomeForRentalForeclosure) {
        this.model.eaImpendingHomelessness.dateOfFamilyDeparture = null;
      }
      if (!this.model.eaImpendingHomelessness.isYourBuildingDecidedUnSafe) {
        this.model.eaImpendingHomelessness.dateBuildingWasDecidedUnSafe = null;
        this.model.eaImpendingHomelessness.isInspectionReportAvailable = null;
      }
    }
    if (this.model.eaHomelessness) {
      if (!this.model.eaHomelessness.inLackOfPlace) {
        this.model.eaHomelessness.dateOfStart = null;
      }
      if (!this.model.eaHomelessness.isYourBuildingDecidedUnSafe) {
        this.model.eaHomelessness.dateBuildingWasDecidedUnSafe = null;
        this.model.eaHomelessness.isInspectionReportAvailable = null;
      }
    }
    return this.model;
  }

  openHelp() {}

  refreshModel() {
    if (this.model != null && this.requestEditService.model.eaEmergencyType != null) {
      this.initSection(this.requestEditService.model.eaEmergencyType);
      this.modifiedTrackerForcedValidation();
    }
    this.isActive = false;
    setTimeout(() => (this.isActive = true), 0);
  }

  modifiedTrackerForcedValidation() {
    if (this.requestEditService.modifiedTracker.isTypeOfEmergency.validated || this.requestEditService.modifiedTracker.isTypeOfEmergency.error) {
      this.isSectionValid = false;
      this.validate();
    }
  }

  validate(): void {
    this.validationManager.resetErrors();
    this.isSectionValid = false;
    this.modelErrors = {};
    const result = this.model.validate(this.validationManager, this.requestEditService.model);
    this.isSectionValid = result.isValid;
    this.modelErrors = result.errors;
    if (this.isSectionValid) {
      this.requestEditService.sectionIsNowValid(this.requestEditService.getSectionLabel(EARequestSections.Emergency));
    }
    this.setSaveButtonEnableOrDisable();
  }

  setSaveButtonEnableOrDisable() {
    if (this.model.emergencyTypeCodes !== undefined) {
      this.requestEditService.modifiedTracker.isSaveDisabled =
        this.isReadOnly ||
        (this.model.emergencyTypeCodes.indexOf(EAEmergencyCodes.ImpendingHomelessness) >= 0 &&
          !this.model.eaImpendingHomelessness.isCurrentLandLordUnknown &&
          !this.landlordAddressValidateStatus) ||
        (this.model.emergencyTypeCodes.indexOf(EAEmergencyCodes.ImpendingHomelessness) >= 0 &&
          this.model.eaImpendingHomelessness.landLordPhone &&
          !!this.modelErrors['landLordPhone']);
    }
  }

  checkState() {
    if (!this.isSectionValid) {
      this.validate();
    }
    this.requestEditService.setModifiedModel(
      this.requestEditService.getSectionLabel(EARequestSections.Emergency),
      !_.isEqual(this.cachedModel, this.requestEditService.model.eaEmergencyType)
    );
  }

  ngOnDestroy() {
    this.requestEditService.modifiedTracker.isSaveDisabled = false;
  }
}
