import { FinalistAddress } from './../../../shared/models/finalist-address.model';
import { Serializable } from 'src/app/shared/interfaces/serializable';
import * as moment from 'moment';
import { Utilities } from 'src/app/shared/utilities';
import { ValidationManager, ValidationResult, ValidationCode } from 'src/app/shared/models/validation';
import { MmDdYyyyValidationContext } from 'src/app/shared/interfaces/mmDdYyyy-validation-context';
import { EAEmergencyCodes } from './ea-request-sections.enum';
import { EARequest } from './ea-request.model';

export class EARequestEmergencyTypeSection implements Serializable<EARequestEmergencyTypeSection> {
  emergencyTypeIds: number[];
  emergencyTypeNames: string[];
  emergencyTypeCodes: string[];
  emergencyDetails: string;
  eaImpendingHomelessness: EAImpendingHomelessness;
  eaHomelessness: EAHomelessness;
  eaEnergyCrisis: EAEnergyCrisis;
  requestId: number;
  modifiedBy: string;
  modifiedDate: string;
  isSubmittedViaDriverFlow: boolean;

  public static create() {
    const eaEmergencyType = new EARequestEmergencyTypeSection();
    eaEmergencyType.requestId = 0;
    eaEmergencyType.emergencyTypeIds = [];
    eaEmergencyType.eaImpendingHomelessness = new EAImpendingHomelessness();
    eaEmergencyType.eaHomelessness = new EAHomelessness();
    eaEmergencyType.eaEnergyCrisis = new EAEnergyCrisis();
    return eaEmergencyType;
  }

  public static clone(input: any, instance: EARequestEmergencyTypeSection) {
    instance.emergencyTypeIds = Utilities.deserilizeArray(input.emergencyTypeIds);
    instance.emergencyTypeNames = Utilities.deserilizeArray(input.emergencyTypeNames);
    instance.emergencyTypeCodes = Utilities.deserilizeArray(input.emergencyTypeCodes);
    instance.emergencyDetails = input.emergencyDetails;
    instance.eaImpendingHomelessness = Utilities.deserilizeChild(input.eaImpendingHomelessness, EAImpendingHomelessness);
    instance.eaHomelessness = Utilities.deserilizeChild(input.eaHomelessness, EAHomelessness);
    instance.eaEnergyCrisis = Utilities.deserilizeChild(input.eaEnergyCrisis, EAEnergyCrisis);
    instance.isSubmittedViaDriverFlow = input.isSubmittedViaDriverFlow;
    instance.modifiedBy = input.modifiedBy;
    instance.modifiedDate = input.modifiedDate;
  }

  public deserialize(input: any) {
    EARequestEmergencyTypeSection.clone(input, this);
    return this;
  }

  public validate(validationManager: ValidationManager, requestModel: EARequest): ValidationResult {
    const result = new ValidationResult();
    Utilities.validateMultiSelect(this.emergencyTypeIds, 'emergencyTypeIds', 'Emergency Type', result, validationManager);
    if (!this.emergencyDetails || this.emergencyDetails.toString() === '') {
      validationManager.addErrorWithDetail(
        ValidationCode.RequiredInformationMissing_Details,
        'Describe the emergency you have. For example, what happened and when the emergency happened.'
      );
      result.addError('emergencyDetails');
    }
    if (this.emergencyTypeCodes && this.emergencyTypeCodes.includes(EAEmergencyCodes.ImpendingHomelessness)) {
      this.eaImpendingHomelessness.validate(result, validationManager, requestModel);
    }
    if (this.emergencyTypeCodes && this.emergencyTypeCodes.includes(EAEmergencyCodes.Homelessness)) {
      this.eaHomelessness.validate(result, validationManager);
    }
    if (this.emergencyTypeCodes && this.emergencyTypeCodes.includes(EAEmergencyCodes.EnergyCrisis)) {
      this.eaEnergyCrisis.validate(result, validationManager);
    }
    return result;
  }
}

export class EAImpendingHomelessness implements Serializable<EAImpendingHomelessness> {
  haveEvictionNotice: boolean;
  dateOfEvictionNotice: string;
  difficultToPayDetails: string;
  isCurrentLandLordUnknown: boolean;
  landLordName: string;
  contactPerson: string;
  landLordPhone: string;
  landLordAddress: FinalistAddress;
  needingDifferentHomeForAbuse: boolean;
  needingDifferentHomeForRentalForeclosure: boolean;
  dateOfFamilyDeparture: string;
  isYourBuildingDecidedUnSafe: boolean;
  dateBuildingWasDecidedUnSafe: string;
  isInspectionReportAvailable: boolean;
  emergencyTypeReasonId: number;
  emergencyTypeReasonName: string;

  public static clone(input: any, instance: EAImpendingHomelessness) {
    instance.haveEvictionNotice = input.haveEvictionNotice;
    instance.dateOfEvictionNotice = input.dateOfEvictionNotice ? moment(input.dateOfEvictionNotice).format('MM/DD/YYYY') : input.dateOfEvictionNotice;
    instance.difficultToPayDetails = input.difficultToPayDetails;
    instance.isCurrentLandLordUnknown = input.isCurrentLandLordUnknown;
    instance.landLordName = input.landLordName;
    instance.contactPerson = input.contactPerson;
    instance.landLordPhone = input.landLordPhone;
    instance.landLordAddress = input.landLordAddress ? Utilities.deserilizeChild(input.landLordAddress, FinalistAddress) : new FinalistAddress();
    instance.needingDifferentHomeForAbuse = input.needingDifferentHomeForAbuse;
    instance.needingDifferentHomeForRentalForeclosure = input.needingDifferentHomeForRentalForeclosure;
    instance.dateOfFamilyDeparture = input.dateOfFamilyDeparture ? moment(input.dateOfFamilyDeparture).format('MM/DD/YYYY') : input.dateOfFamilyDeparture;
    instance.isYourBuildingDecidedUnSafe = input.isYourBuildingDecidedUnSafe;
    instance.dateBuildingWasDecidedUnSafe = input.dateBuildingWasDecidedUnSafe
      ? moment(input.dateBuildingWasDecidedUnSafe).format('MM/DD/YYYY')
      : input.dateBuildingWasDecidedUnSafe;
    instance.isInspectionReportAvailable = input.isInspectionReportAvailable;
    instance.emergencyTypeReasonId = input.emergencyTypeReasonId;
    instance.emergencyTypeReasonName = input.emergencyTypeReasonName;
  }

  public deserialize(input: any) {
    EAImpendingHomelessness.clone(input, this);
    return this;
  }

  public validate(result: ValidationResult, validationManager: ValidationManager, requestModel: EARequest) {
    const currentDate = moment(Utilities.currentDate).format('MM/DD/YYYY');
    const thirtyDaysAdd = moment(requestModel.eaDemographics.applicationDate)
      .add(30, 'days')
      .format('MM/DD/YYYY');
    const dateContext: MmDdYyyyValidationContext = {
      date: '',
      prop: 'applicationDate',
      prettyProp: 'Application Date',
      result: result,
      validationManager: validationManager,
      isRequired: true,
      maxDate: currentDate,
      maxDateAllowSame: true,
      maxDateName: currentDate,
      participantDOB: null,
      minDate: '01/01/1900',
      minDateName: '01/01/1900',
      minDateAllowSame: true
    };
    Utilities.validateRequiredYesNo(result, validationManager, this.haveEvictionNotice, 'haveEvictionNotice', 'Do you have an eviction notice or foreclosure notice?');
    if (this.haveEvictionNotice) {
      Utilities.validateMmDdYyyyDate({
        ...dateContext,
        date: this.dateOfEvictionNotice,
        prop: 'dateOfEvictionNotice',
        prettyProp: 'When did you get the eviction or foreclosure notice?',
        isRequired: this.haveEvictionNotice
      });
    }
    if (!this.difficultToPayDetails || this.difficultToPayDetails.toString() === '') {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Describe what happened to make it difficult to pay your rent or mortgage payment.');
      result.addError('difficultToPayDetails');
    }
    if (!this.isCurrentLandLordUnknown && this.landLordPhone && this.landLordPhone.length < 10) {
      validationManager.addErrorWithFormat(ValidationCode.ValueInInvalidFormat, 'Landlord Phone', '(012) 345-6789');
      result.addError('landLordPhone');
    }
    Utilities.validateRequiredYesNo(
      result,
      validationManager,
      this.needingDifferentHomeForAbuse,
      'needingDifferentHomeForAbuse',
      'Do you need a different home because of domestic abuse?'
    );
    Utilities.validateRequiredYesNo(
      result,
      validationManager,
      this.needingDifferentHomeForRentalForeclosure,
      'needingDifferentHomeForRentalForeclosure',
      'Do you need a different home because your rental housing is in foreclosure?'
    );
    if (this.needingDifferentHomeForRentalForeclosure) {
      Utilities.validateMmDdYyyyDate({
        ...dateContext,
        date: this.dateOfFamilyDeparture,
        prop: 'dateOfFamilyDeparture',
        prettyProp: 'When does your family have to leave?',
        isRequired: this.needingDifferentHomeForRentalForeclosure,
        maxDate: thirtyDaysAdd,
        maxDateName: thirtyDaysAdd
      });
    }
    Utilities.validateRequiredYesNo(
      result,
      validationManager,
      this.isYourBuildingDecidedUnSafe,
      'isYourBuildingDecidedUnSafeImpending',
      'Has a building or housing inspector or public health official decided your home is not safe to live in?'
    );
    if (this.isYourBuildingDecidedUnSafe) {
      Utilities.validateMmDdYyyyDate({
        ...dateContext,
        date: this.dateBuildingWasDecidedUnSafe,
        prop: 'dateBuildingWasDecidedUnSafeImpending',
        prettyProp: 'When did the building or housing inspector or public health official decide this?',
        isRequired: this.isYourBuildingDecidedUnSafe
      });
      Utilities.validateRequiredYesNo(
        result,
        validationManager,
        this.isInspectionReportAvailable,
        'isInspectionReportAvailableImpending',
        'Do you have a housing inspection report?'
      );
    }
  }
}

export class EAHomelessness implements Serializable<EAHomelessness> {
  inLackOfPlace: boolean;
  dateOfStart: string;
  planOnPermanentPlace: boolean;
  inShelterForDomesticAbuse: boolean;
  isYourBuildingDecidedUnSafe: boolean;
  dateBuildingWasDecidedUnSafe: string;
  isInspectionReportAvailable: boolean;
  emergencyTypeReasonId: number;
  emergencyTypeReasonName: string;

  public static clone(input: any, instance: EAHomelessness) {
    instance.inLackOfPlace = input.inLackOfPlace;
    instance.dateOfStart = input.dateOfStart ? moment(input.dateOfStart).format('MM/DD/YYYY') : input.dateOfStart;
    instance.planOnPermanentPlace = input.planOnPermanentPlace;
    instance.inShelterForDomesticAbuse = input.inShelterForDomesticAbuse;
    instance.isYourBuildingDecidedUnSafe = input.isYourBuildingDecidedUnSafe;
    instance.dateBuildingWasDecidedUnSafe = input.dateBuildingWasDecidedUnSafe
      ? moment(input.dateBuildingWasDecidedUnSafe).format('MM/DD/YYYY')
      : input.dateBuildingWasDecidedUnSafe;
    instance.isInspectionReportAvailable = input.isInspectionReportAvailable;
    instance.emergencyTypeReasonId = input.emergencyTypeReasonId;
    instance.emergencyTypeReasonName = input.emergencyTypeReasonName;
  }

  public deserialize(input: any) {
    EAHomelessness.clone(input, this);
    return this;
  }

  public validate(result: ValidationResult, validationManager: ValidationManager) {
    const currentDate = moment(Utilities.currentDate).format('MM/DD/YYYY');
    const dateContext: MmDdYyyyValidationContext = {
      date: '',
      prop: 'applicationDate',
      prettyProp: 'Application Date',
      result: result,
      validationManager: validationManager,
      isRequired: true,
      maxDate: currentDate,
      maxDateAllowSame: true,
      maxDateName: currentDate,
      participantDOB: null,
      minDate: '01/01/1900',
      minDateName: '01/01/1900',
      minDateAllowSame: true
    };
    Utilities.validateRequiredYesNo(
      result,
      validationManager,
      this.inLackOfPlace,
      'inLackOfPlace',
      'Do you lack a regular place to live or sleep in a place not meant for sleeping?'
    );
    if (this.inLackOfPlace) {
      Utilities.validateMmDdYyyyDate({
        ...dateContext,
        date: this.dateOfStart,
        prop: 'dateOfStart',
        prettyProp: 'When did this start?',
        isRequired: this.inLackOfPlace
      });
    }
    Utilities.validateRequiredYesNo(result, validationManager, this.planOnPermanentPlace, 'planOnPermanentPlace', 'Do you plan to get a permanent place to live?');
    Utilities.validateRequiredYesNo(result, validationManager, this.inShelterForDomesticAbuse, 'inShelterForDomesticAbuse', 'Are you staying in a shelter for domestic abuse?');
    Utilities.validateRequiredYesNo(
      result,
      validationManager,
      this.isYourBuildingDecidedUnSafe,
      'isYourBuildingDecidedUnSafe',
      'Has a building or housing inspector or public health official decided your home is not safe to live in?'
    );
    if (this.isYourBuildingDecidedUnSafe) {
      Utilities.validateMmDdYyyyDate({
        ...dateContext,
        date: this.dateBuildingWasDecidedUnSafe,
        prop: 'dateBuildingWasDecidedUnSafe',
        prettyProp: 'When did the building or housing inspector or public health official decide this?',
        isRequired: this.isYourBuildingDecidedUnSafe
      });
      Utilities.validateRequiredYesNo(result, validationManager, this.isInspectionReportAvailable, 'isInspectionReportAvailable', 'Do you have a housing inspection report?');
    }
  }
}

export class EAEnergyCrisis implements Serializable<EAEnergyCrisis> {
  inNeedForUtilities: boolean;
  difficultyForUtilityBill: string;
  existingAppliedHelp: string;
  haveThreat: boolean;

  public static clone(input: any, instance: EAEnergyCrisis) {
    instance.inNeedForUtilities = input.inNeedForUtilities;
    instance.difficultyForUtilityBill = input.difficultyForUtilityBill;
    instance.existingAppliedHelp = input.existingAppliedHelp;
    instance.haveThreat = input.haveThreat;
  }

  public deserialize(input: any) {
    EAEnergyCrisis.clone(input, this);
    return this;
  }

  public validate(result: ValidationResult, validationManager: ValidationManager) {
    Utilities.validateRequiredYesNo(
      result,
      validationManager,
      this.inNeedForUtilities,
      'inNeedForUtilities',
      'Does your family need financial assistance to get or keep heat, electricity, water, or sewer service?'
    );
    if (!this.difficultyForUtilityBill || this.difficultyForUtilityBill.toString() === '') {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Describe what happened to make it difficult to pay your utility bill.');
      result.addError('difficultyForUtilityBill');
    }
    if (!this.existingAppliedHelp || this.existingAppliedHelp.toString() === '') {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'What help has your family applied for already?');
      result.addError('existingAppliedHelp');
    }
    Utilities.validateRequiredYesNo(
      result,
      validationManager,
      this.haveThreat,
      'haveThreat',
      'Does your family have an immediate threat to its health and safety from an Energy Crisis?'
    );
  }
}
