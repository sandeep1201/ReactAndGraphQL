import { DropDownField } from '../../shared/models/dropdown-field';
import { Serializable } from '../interfaces/serializable';
import { Utilities } from '../utilities';
import * as moment from 'moment';
import { ActionNeeded } from '../../features-modules/actions-needed/models/action-needed-new';
import { MmDdYyyyValidationContext } from '../interfaces/mmDdYyyy-validation-context';
import { ValidationManager } from './validation-manager';
import { ValidationResult } from './validation-result';
import { AppService } from 'src/app/core/services/app.service';
import { ValidationCode } from './validation';

export class TransportationSection implements Serializable<TransportationSection> {
  isSubmittedViaDriverFlow: boolean;
  methods: number[];
  transportationMethods: string[];
  methodDetails: string;
  isVehicleInsuredId: number;
  isVehicleInsuredName: string;
  vehicleInsuredDetails: string;
  isVehicleRegistrationCurrentId: number;
  isVehicleRegistrationCurrentName: string;
  vehicleRegistrationCurrentDetails: string;
  hasValidDriversLicense: boolean;
  driversLicenseStateId: number;
  driversLicenseStateName: string;
  driversLicenseExpirationDate: string;
  driversLicenseDetails: string;
  driversLicenseInvalidReasonId: number;
  driversLicenseInvalidReasonName: string;
  driversLicenseInvalidDetails: string;
  hadCommercialDriversLicense: boolean;
  isCommercialDriversLicenseActive: boolean;
  commercialDriversLicenseDetails: string;
  actionNeeded: ActionNeeded;
  notes: string;
  rowVersion: string;
  assessmentRowVersion: string;
  modifiedBy: string;
  modifiedDate: string;

  set driversLicenseExpirationDateMmDdYyyy(date) {
    if (date != null && date.length === 10 && moment(date).isValid()) {
      this.driversLicenseExpirationDate = moment(date).toISOString();
    } else {
      this.driversLicenseExpirationDate = date;
    }
  }
  get driversLicenseExpirationDateMmDdYyyy() {
    if (this.driversLicenseExpirationDate != null && moment(this.driversLicenseExpirationDate, moment.ISO_8601).isValid()) {
      return moment(this.driversLicenseExpirationDate, moment.ISO_8601).format('MM/DD/YYYY');
    } else {
      return this.driversLicenseExpirationDate;
    }
  }

  public static clone(input, instance: TransportationSection) {
    instance.isSubmittedViaDriverFlow = input.isSubmittedViaDriverFlow;
    instance.methods = Utilities.deserilizeArray(input.methods);
    instance.transportationMethods = Utilities.deserilizeArray(input.transportationMethods);
    instance.methodDetails = input.methodDetails;
    instance.isVehicleInsuredId = input.isVehicleInsuredId;
    instance.isVehicleInsuredName = input.isVehicleInsuredName;
    instance.vehicleInsuredDetails = input.vehicleInsuredDetails;
    instance.isVehicleRegistrationCurrentId = input.isVehicleRegistrationCurrentId;
    instance.isVehicleRegistrationCurrentName = input.isVehicleRegistrationCurrentName;
    instance.vehicleRegistrationCurrentDetails = input.vehicleRegistrationCurrentDetails;
    instance.hasValidDriversLicense = input.hasValidDriversLicense;
    instance.driversLicenseStateId = input.driversLicenseStateId;
    instance.driversLicenseStateName = input.driversLicenseStateName;
    instance.driversLicenseExpirationDate = input.driversLicenseExpirationDate;
    instance.driversLicenseDetails = input.driversLicenseDetails;
    instance.driversLicenseInvalidReasonId = input.driversLicenseInvalidReasonId;
    instance.driversLicenseInvalidReasonName = input.driversLicenseInvalidReasonName;
    instance.driversLicenseInvalidDetails = input.driversLicenseInvalidDetails;
    instance.hadCommercialDriversLicense = input.hadCommercialDriversLicense;
    instance.isCommercialDriversLicenseActive = input.isCommercialDriversLicenseActive;
    instance.commercialDriversLicenseDetails = input.commercialDriversLicenseDetails;
    instance.actionNeeded = Utilities.deserilizeChild(input.actionNeeded, ActionNeeded);
    instance.notes = input.notes;
    instance.assessmentRowVersion = input.assessmentRowVersion;
    instance.rowVersion = input.rowVersion;
    instance.modifiedBy = input.modifiedBy;
    instance.modifiedDate = input.modifiedDate;

    return instance;
  }

  public deserialize(input: any) {
    TransportationSection.clone(input, this);

    return this;
  }

  isVehicleInfoDisplayed(methodDrop: DropDownField[]): boolean {
    const personalVehicleId = Utilities.idByFieldDataName('Personal Vehicle', methodDrop);
    const borrowedVehicleId = Utilities.idByFieldDataName('Borrowed Vehicle', methodDrop);
    if (this.methods != null && (this.methods.indexOf(personalVehicleId) !== -1 || this.methods.indexOf(borrowedVehicleId) !== -1)) {
      return true;
    } else {
      return false;
    }
  }

  // BR: Required when other is selected for methods.
  isMethodDetailsRequired(methodDrop: DropDownField[]): boolean {
    const otherId = Utilities.idByFieldDataName('Other', methodDrop);
    if (this.methods != null && this.methods.indexOf(otherId) !== -1) {
      return true;
    } else {
      return false;
    }
  }

  // BR: Does not display if participant has never applied for a driver's license.
  isCdlSectionDisplayed(driverLicenseStatusesDrop: DropDownField[]): boolean {
    const neverAppliedId = Utilities.idByFieldDataName('Never Applied for a License', driverLicenseStatusesDrop);
    if (Number(this.driversLicenseInvalidReasonId) === neverAppliedId && this.hasValidDriversLicense === false) {
      return false;
    } else {
      return true;
    }
  }

  public validate(
    validationManager: ValidationManager,
    participantDob: string,
    transportationTypes: DropDownField[],
    driverLicenseStatusesDrop: DropDownField[]
  ): ValidationResult {
    const result = new ValidationResult();

    Utilities.validateMultiSelect(this.methods, 'methods', 'Which transportation methods can you use to participate in work or work activities?', result, validationManager);

    if (this.isMethodDetailsRequired(transportationTypes)) {
      Utilities.validateRequiredText(
        this.methodDetails,
        'methodDetails',
        'Which transportation methods can you use to participate in work or work activities? - Details',
        result,
        validationManager
      );
    }

    if (this.isVehicleInfoDisplayed(transportationTypes)) {
      Utilities.validateDropDown(this.isVehicleInsuredId, 'isVehicleInsuredId', 'Is the vehicle insured?', result, validationManager);
      Utilities.validateDropDown(this.isVehicleRegistrationCurrentId, 'isVehicleRegistrationCurrentId', 'Is the vehicle registration current?', result, validationManager);
    }
    Utilities.validateRequiredYesNo(result, validationManager, this.hasValidDriversLicense, 'hasValidDriversLicense', "Do you have a valid driver's license?");

    if (this.hasValidDriversLicense === true) {
      Utilities.validateDropDown(this.driversLicenseStateId, 'driversLicenseStateId', 'State Issued', result, validationManager);

      // Must be Current or a future Date.
      // Cannot be more than 120 years since DOB.
      // Required.
      const referralDateValidationContext: MmDdYyyyValidationContext = {
        date: this.driversLicenseExpirationDateMmDdYyyy,
        prop: 'driversLicenseExpirationDate',
        prettyProp: 'Expiration Date Month and Year',
        result: result,
        validationManager: validationManager,
        isRequired: true,
        minDate: moment(Utilities.currentDate).format('MM/DD/YYYY'),
        minDateAllowSame: true,
        minDateName: 'Current Date',
        maxDate: null,
        maxDateAllowSame: false,
        maxDateName: null,
        participantDOB: participantDob
      };

      Utilities.validateMmDdYyyyDate(referralDateValidationContext);
    } else if (this.hasValidDriversLicense === false) {
      Utilities.validateDropDown(this.driversLicenseInvalidReasonId, 'driversLicenseInvalidReasonId', "Why don't you have a valid driver's license?", result, validationManager);
      Utilities.validateRequiredText(
        this.driversLicenseInvalidDetails,
        'driversLicenseInvalidDetails',
        "Why don't you have a valid driver's license? - Details",
        result,
        validationManager
      );
    }

    if (this.isCdlSectionDisplayed(driverLicenseStatusesDrop) === true) {
      Utilities.validateRequiredYesNo(
        result,
        validationManager,
        this.hadCommercialDriversLicense,
        'hadCommercialDriversLicense',
        "Have you ever had a commercial driver's license (CDL)?"
      );
    }

    if (this.isCdlSectionDisplayed(driverLicenseStatusesDrop) === true && this.hadCommercialDriversLicense === true) {
      Utilities.validateRequiredYesNo(result, validationManager, this.isCommercialDriversLicenseActive, 'isCommercialDriversLicenseActive', 'Is your CDL still active?');
      if (this.isCommercialDriversLicenseActive && !this.hasValidDriversLicense) {
        validationManager.addErrorWithDetail(ValidationCode.DuplicateDataWithNoMessage, "CDL cannot be active if driver's license is not valid");
        result.addError('isCommercialDriversLicenseActive');
      }
      Utilities.validateRequiredText(
        this.commercialDriversLicenseDetails,
        'commercialDriversLicenseDetails',
        'What kind of vehicles have you operated in the past?',
        result,
        validationManager
      );
    }

    // Action needed
    const anResult = this.actionNeeded.validate(validationManager);

    // TODO: wire up anResult better.
    if (anResult.isValid === false) {
      result.addError('actionNeeded');
    }

    return result;
  }
}
