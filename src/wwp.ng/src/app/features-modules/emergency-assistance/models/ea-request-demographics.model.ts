import { Utilities } from './../../../shared/utilities';
import { FinalistAddress } from 'src/app/shared/models/finalist-address.model';
import { Serializable } from 'src/app/shared/interfaces/serializable';
import * as moment from 'moment';
import { ValidationManager, ValidationResult, ValidationCode } from 'src/app/shared/models/validation';
import { MmDdYyyyValidationContext } from 'src/app/shared/interfaces/mmDdYyyy-validation-context';
import { EAEmergencyCodes } from './ea-request-sections.enum';

export class EARequestDemographicsSection implements Serializable<EARequestDemographicsSection> {
  applicationDate: string;
  caresCaseNumber: number;
  eaDemographicsContact: EARequestContact;
  didApplicantTakeCareOfAnyChild: boolean;
  willTheChildStayInApplicantCare: boolean;
  applicationInitiatedMethodId: number;
  applicationInitiatedMethodCode: string;
  applicationInitiatedMethodName: string;
  accessTrackingNumber: number;
  requestId: number;
  modifiedBy: string;
  modifiedDate: string;
  isSubmittedViaDriverFlow: boolean;

  public static create() {
    const eaDemographics = new EARequestDemographicsSection();
    eaDemographics.requestId = 0;
    eaDemographics.eaDemographicsContact = new EARequestContact();
    eaDemographics.eaDemographicsContact.householdAddress = new FinalistAddress();
    eaDemographics.eaDemographicsContact.mailingAddress = new FinalistAddress();
    return eaDemographics;
  }

  public static clone(input: any, instance: EARequestDemographicsSection) {
    instance.applicationDate = input.applicationDate ? moment(input.applicationDate).format('MM/DD/YYYY') : input.applicationDate;
    instance.caresCaseNumber = input.caresCaseNumber;
    instance.didApplicantTakeCareOfAnyChild = input.didApplicantTakeCareOfAnyChild;
    instance.willTheChildStayInApplicantCare = input.willTheChildStayInApplicantCare;
    instance.eaDemographicsContact = Utilities.deserilizeChild(input.eaDemographicsContact, EARequestContact);
    instance.applicationInitiatedMethodId = input.applicationInitiatedMethodId;
    instance.applicationInitiatedMethodCode = input.applicationInitiatedMethodCode;
    instance.applicationInitiatedMethodName = input.applicationInitiatedMethodName;
    instance.accessTrackingNumber = input.accessTrackingNumber;
    instance.requestId = input.requestId;
    instance.modifiedBy = input.modifiedBy;
    instance.modifiedDate = input.modifiedDate;
    instance.isSubmittedViaDriverFlow = input.isSubmittedViaDriverFlow;
    if (!instance.eaDemographicsContact.householdAddress) {
      instance.eaDemographicsContact.householdAddress = new FinalistAddress();
    }
    if (!instance.eaDemographicsContact.mailingAddress) {
      instance.eaDemographicsContact.mailingAddress = new FinalistAddress();
    }
  }

  public deserialize(input: any) {
    EARequestDemographicsSection.clone(input, this);
    return this;
  }

  public validateApplicationDate(validationManager: ValidationManager): ValidationResult {
    const result = new ValidationResult();

    const currentDate = Utilities.currentDate.clone().format('MM/DD/YYYY');
    const back180Days = Utilities.currentDate
      .clone()
      .subtract(180, 'days')
      .format('MM/DD/YYYY');
    const applicatinDateContext: MmDdYyyyValidationContext = {
      date: this.applicationDate,
      prop: 'applicationDate',
      prettyProp: 'Application Date',
      result: result,
      validationManager: validationManager,
      isRequired: true,
      maxDate: currentDate,
      maxDateAllowSame: true,
      maxDateName: currentDate,
      participantDOB: null,
      minDateAllowSame: false,
      minDate: back180Days,
      minDateName: back180Days
    };
    Utilities.validateMmDdYyyyDate(applicatinDateContext);
    return result;
  }

  public validate(validationManager: ValidationManager): ValidationResult {
    const result = new ValidationResult();
    const currentDate = Utilities.currentDate.clone().format('MM/DD/YYYY');
    const back180Days = Utilities.currentDate
      .clone()
      .subtract(180, 'days')
      .format('MM/DD/YYYY');
    const applicatinDateContext: MmDdYyyyValidationContext = {
      date: this.applicationDate,
      prop: 'applicationDate',
      prettyProp: 'Application Date',
      result: result,
      validationManager: validationManager,
      isRequired: true,
      maxDate: currentDate,
      maxDateAllowSame: true,
      maxDateName: currentDate,
      participantDOB: null,
      minDateAllowSame: false,
      minDate: back180Days,
      minDateName: back180Days
    };
    Utilities.validateMmDdYyyyDate(applicatinDateContext);
    if (Utilities.isNumberEmptyOrNull(this.applicationInitiatedMethodId)) {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Application Initiated Method.');
      result.addError(`applicationInitiatedMethodId`);
    } else if (this.applicationInitiatedMethodCode === EAEmergencyCodes.ACCESS && Utilities.isNumberEmptyOrNull(this.accessTrackingNumber)) {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'ACCESS Tracking Number.');
      result.addError(`accessTrackingNumber`);
    }
    Utilities.validateDropDown(this.eaDemographicsContact.countyOfResidenceId, 'countyOfResidenceId', 'County of Residence', result, validationManager);
    if (!this.eaDemographicsContact.isHomeless) {
      Utilities.validateRequiredYesNo(
        result,
        validationManager,
        this.eaDemographicsContact.isMailingSameAsHouseholdAddress,
        'isMailingSameAsHouseholdAddress',
        'Is your household address the same as your mailing address?'
      );
    }
    Utilities.validateRequiredYesNo(
      result,
      validationManager,
      this.didApplicantTakeCareOfAnyChild,
      'didApplicantTakeCareOfAnyChild',
      "Do you take care of and make decisions for either your child or a relative's child in your home?"
    );
    Utilities.validateRequiredYesNo(
      result,
      validationManager,
      this.willTheChildStayInApplicantCare,
      'willTheChildStayInApplicantCare',
      'Will this child(ren) stay in your care in the future?'
    );
    if (!this.eaDemographicsContact.bestWayToReach) {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Best way to reach you?');
      result.addError('bestWayToReach');
    }
    if (this.eaDemographicsContact.emailAddress) {
      Utilities.validateEmail(this.eaDemographicsContact.emailAddress, 'emailAddress', 'Applicant Email', result, validationManager);
    } else if (
      this.eaDemographicsContact.bestWayToReach &&
      this.eaDemographicsContact.bestWayToReach === 'email' &&
      (!this.eaDemographicsContact.emailAddress || this.eaDemographicsContact.emailAddress.toString() === '')
    ) {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Applicant Email');
      result.addError('emailAddress');
    }
    if (this.eaDemographicsContact.phoneNumber && this.eaDemographicsContact.phoneNumber.length < 10) {
      validationManager.addErrorWithFormat(ValidationCode.ValueInInvalidFormat, 'Applicant Phone', '(012) 345-6789');
      result.addError('phoneNumber');
    } else if (this.eaDemographicsContact.bestWayToReach && this.eaDemographicsContact.bestWayToReach === 'phone' && !this.eaDemographicsContact.phoneNumber) {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Applicant Phone');
      result.addError('phoneNumber');
    }
    if (this.eaDemographicsContact.alternatePhoneNumber && this.eaDemographicsContact.alternatePhoneNumber.length < 10) {
      validationManager.addErrorWithFormat(ValidationCode.ValueInInvalidFormat, 'Alternate Applicant Phone', '(012) 345-6789');
      result.addError('alternatePhoneNumber');
    }
    if (this.eaDemographicsContact.bestWayToReach && this.eaDemographicsContact.bestWayToReach === 'phone')
      Utilities.validateRequiredYesNo(result, validationManager, this.eaDemographicsContact.canText, 'canText', 'Can you receive messages at this number?');
    let householdResult = new ValidationResult();
    let mailingResult = new ValidationResult();
    if (!this.eaDemographicsContact.isHomeless) {
      householdResult = this.eaDemographicsContact.householdAddress.validateSave(validationManager, 'household');
      Object.keys(householdResult.errors).forEach(item => {
        result.addError(item);
      });
    }
    if (this.eaDemographicsContact.isMailingAddressDisplayed) {
      mailingResult = this.eaDemographicsContact.mailingAddress.validateSave(validationManager, 'mailing');
      Object.keys(mailingResult.errors).forEach(item => {
        result.addError(item);
      });
    }
    return result;
  }
}

export class EARequestContact implements Serializable<EARequestContact> {
  phoneNumber: string;
  canText: boolean;
  alternatePhoneNumber: string;
  canTextAlternate: boolean;
  bestWayToReach: string;
  emailAddress: string;
  countyOfResidenceId: number;
  countyOfResidenceName: string;
  isHomeless: boolean;
  isMailingSameAsHouseholdAddress: boolean;
  householdAddress: FinalistAddress;
  mailingAddress: FinalistAddress;

  public static clone(input: any, instance: EARequestContact) {
    instance.phoneNumber = input.phoneNumber;
    instance.canText = input.canText;
    instance.alternatePhoneNumber = input.alternatePhoneNumber;
    instance.canTextAlternate = input.canTextAlternate;
    instance.bestWayToReach = input.bestWayToReach;
    instance.emailAddress = input.emailAddress;
    instance.countyOfResidenceId = input.countyOfResidenceId;
    instance.countyOfResidenceName = input.countyOfResidenceName;
    instance.isHomeless = input.isHomeless;
    instance.isMailingSameAsHouseholdAddress = input.isMailingSameAsHouseholdAddress;
    instance.householdAddress = Utilities.deserilizeChild(input.householdAddress, FinalistAddress);
    instance.mailingAddress = Utilities.deserilizeChild(input.mailingAddress, FinalistAddress);
  }

  public deserialize(input: any) {
    EARequestContact.clone(input, this);
    return this;
  }

  get isMailingSameAsHouseholdAddressRequired(): boolean {
    return true;
  }

  get isHouseholdSameMailingAddressDisplayed(): boolean {
    if (!this.isHomeless) {
      return true;
    }
  }

  get isMailingAddressDisplayed(): boolean {
    return this.isMailingSameAsHouseholdAddress === false || this.isHomeless === true ? true : false;
  }
}
