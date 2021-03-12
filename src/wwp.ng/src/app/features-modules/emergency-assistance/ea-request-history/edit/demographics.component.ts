import { EAViewModes } from './../../models/ea-request-sections.enum';
import { Utilities } from 'src/app/shared/utilities';
import { FieldDataTypes } from './../../../../shared/enums/field-data-types.enum';
import { ActivatedRoute } from '@angular/router';
import { FieldDataService } from 'src/app/shared/services/field-data.service';
import { AppService } from 'src/app/core/services/app.service';
import { Component, OnInit, OnDestroy, AfterViewChecked } from '@angular/core';
import { EARequestDemographicsSection } from '../../models';
import { ValidationManager } from 'src/app/shared/models/validation';
import { ModelErrors } from 'src/app/shared/interfaces/model-errors';
import { DropDownField } from 'src/app/shared/models/dropdown-field';
import { take, concatMap } from 'rxjs/operators';
import { FinalistAddress } from 'src/app/shared/models/finalist-address.model';
import { EARequestEditService } from '../../services/ea-request-edit.service';
import { SectionComponent } from 'src/app/shared/interfaces/section-component';
import { forkJoin } from 'rxjs';
import { EARequestSections, EAStatusCodes, EAIndividualType, EAEmergencyCodes } from '../../models/ea-request-sections.enum';
import * as _ from 'lodash';

@Component({
  selector: 'app-ea-request-edit-demographics',
  templateUrl: './demographics.component.html',
  styles: []
})
export class EARequestDemographicsEditComponent implements OnInit, OnDestroy, AfterViewChecked, SectionComponent {
  requestId: string;
  pin: string;
  eaViewModes = EAViewModes;
  viewMode = this.eaViewModes.View;
  eaStatusCodes = EAStatusCodes;
  isReadOnly = false;
  isSectionLoaded = false;
  public isActive = true;
  model: EARequestDemographicsSection = new EARequestDemographicsSection();
  cachedModel: EARequestDemographicsSection;
  public countiesDrop: DropDownField[] = [];
  public initiatedMethodDrop: DropDownField[] = [];
  public bestWayToReachDrop: DropDownField[] = [
    { ...new DropDownField(), id: 'phone', name: 'Phone' },
    { ...new DropDownField(), id: 'email', name: 'Email' },
    { ...new DropDownField(), id: 'mail', name: 'Mail' }
  ];
  public isMinor = false;

  validationManager: ValidationManager = new ValidationManager(this.appService);
  public modelErrors: ModelErrors = {};
  public isSectionValid = true;
  public isApplicationDateValid = true;
  public isSectionModified = false;
  public hadSaveError = false;
  public hasTriedSave = false;
  public householdAddressValidateStatus = false;
  public mailingAddressValidateStatus = false;

  constructor(private appService: AppService, private fdService: FieldDataService, private route: ActivatedRoute, private requestEditService: EARequestEditService) {
    scrollTo(0, 0);
  }

  ngOnInit(): void {
    this.requestEditService.setSectionComponent(this);
    this.route.parent.params
      .pipe(
        concatMap(params => {
          this.requestId = params['id'];
          this.pin = params['pin'];
          this.viewMode = params['mode'];
          return forkJoin(
            this.requestEditService.getEARequest(this.pin, this.requestId),
            this.fdService.getFieldDataByField(FieldDataTypes.CountiesNumeric),
            this.fdService.getFieldDataByField(FieldDataTypes.EAInitiationMethods)
          );
        })
      )
      .pipe(take(1))
      .subscribe(result => {
        this.initSection(result[0].eaDemographics);
        this.countiesDrop = result[1];
        this.initiatedMethodDrop = result[2];
        this.modifiedTrackerForcedValidation();
        this.isSectionLoaded = true;
      });
  }

  ngAfterViewChecked() {
    this.setSaveButtonEnableOrDisable();
  }

  private initSection(model: EARequestDemographicsSection) {
    this.model = model;
    this.requestEditService.model.eaDemographics = model;
    this.cachedModel = this.requestEditService.lastSavedModel.eaDemographics;
    this.isReadOnly =
      this.viewMode === this.eaViewModes.View ||
      (this.requestId !== '0' && this.requestEditService.lastSavedModel.statusCode !== this.eaStatusCodes.InProgress && !this.appService.isUserEASupervisor());
    this.isMinor =
      Utilities.currentDate
        .clone()
        .diff(
          this.requestId === '0'
            ? this.requestEditService.participant.dateOfBirth
            : this.requestEditService.model.eaGroupMembers.eaGroupMembers.find(x => x.eaIndividualTypeCode === EAIndividualType.CaretakerRelative).participantDOB,
          'years'
        ) < 18;
  }

  isValid(): boolean {
    return this.isSectionValid;
  }

  scrollToTop() {
    scrollTo(0, 0);
  }

  updateInitiatedMethodCode($event) {
    this.model.applicationInitiatedMethodCode = Utilities.fieldDataCodeById($event, this.initiatedMethodDrop);
  }

  prepareToSaveWithErrors() {
    if (!this.model.eaDemographicsContact.householdAddress.validate(this.validationManager) || this.model.eaDemographicsContact.isHomeless) {
      this.model.eaDemographicsContact.householdAddress = new FinalistAddress();
      if (this.model.eaDemographicsContact.isHomeless) this.model.eaDemographicsContact.isMailingSameAsHouseholdAddress = null;
    }
    if (!this.model.eaDemographicsContact.mailingAddress.validate(this.validationManager) || this.model.eaDemographicsContact.isMailingSameAsHouseholdAddress) {
      this.model.eaDemographicsContact.mailingAddress = new FinalistAddress();
    }
    if (this.model.applicationInitiatedMethodCode && this.model.applicationInitiatedMethodCode !== EAEmergencyCodes.ACCESS) this.model.accessTrackingNumber = null;
    return this.model;
  }

  openHelp() {}

  refreshModel() {
    if (this.model != null && this.requestEditService.model.eaDemographics != null) {
      this.initSection(this.requestEditService.model.eaDemographics);
      this.modifiedTrackerForcedValidation();
    }
    this.isActive = false;
    setTimeout(() => (this.isActive = true), 0);
  }

  modifiedTrackerForcedValidation() {
    if (this.requestEditService.modifiedTracker.isDemographics.validated || this.requestEditService.modifiedTracker.isDemographics.error) {
      this.isSectionValid = false;
      this.validate();
    }
  }

  validate(): void {
    this.validationManager.resetErrors();
    this.isSectionValid = false;
    this.modelErrors = {};
    const applicationDateResult = this.model.validateApplicationDate(this.validationManager);
    this.modelErrors = applicationDateResult.errors;
    this.isApplicationDateValid = applicationDateResult.isValid;
    if (applicationDateResult.isValid) {
      const result = this.model.validate(this.validationManager);
      this.isSectionValid = result.isValid;
      this.modelErrors = result.errors;
      if (this.isSectionValid) {
        this.requestEditService.sectionIsNowValid(this.requestEditService.getSectionLabel(EARequestSections.Demographics));
      }
    }
    this.setSaveButtonEnableOrDisable();
  }

  setSaveButtonEnableOrDisable() {
    if (this.model.eaDemographicsContact && this.model.eaDemographicsContact.isMailingAddressDisplayed !== undefined) {
      const addressValid = this.model.eaDemographicsContact.isMailingAddressDisplayed ? this.mailingAddressValidateStatus : this.householdAddressValidateStatus;
      this.requestEditService.modifiedTracker.isSaveDisabled =
        this.isReadOnly ||
        !this.isApplicationDateValid ||
        !addressValid ||
        (this.model.eaDemographicsContact.phoneNumber && !!this.modelErrors['phoneNumber']) ||
        (this.model.eaDemographicsContact.alternatePhoneNumber && !!this.modelErrors['alternatePhoneNumber']) ||
        (this.model.eaDemographicsContact.emailAddress && !!this.modelErrors['emailAddress']);
    }
  }

  checkState() {
    if (!this.isSectionValid) {
      this.validate();
    }

    this.requestEditService.setModifiedModel(
      this.requestEditService.getSectionLabel(EARequestSections.Demographics),
      !_.isEqual(this.cachedModel, this.requestEditService.model.eaDemographics)
    );
  }

  ngOnDestroy() {
    this.requestEditService.modifiedTracker.isSaveDisabled = false;
  }
}
