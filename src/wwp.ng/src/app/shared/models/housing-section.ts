// tslint:disable: no-use-before-declare
import { Serializable } from '../interfaces/serializable';
import { ValidationCode } from './validation-error';
import { ValidationManager } from './validation-manager';
import { ValidationResult } from './validation-result';
import { ActionNeeded } from '../../features-modules/actions-needed/models/action-needed-new';
import * as moment from 'moment';
import { IsEmpty } from '../interfaces/is-empty';
import { Utilities, has } from '../utilities';
import { MmYyyyValidationContext } from '../interfaces/mmYyyy-validation-context';
import { AppService } from 'src/app/core/services/app.service';

export class CwwHousing implements Serializable<CwwHousing> {
  address: string;
  city: string;
  state: string;
  zip: string;
  subsidized: string;
  rentObligation: string;

  deserialize(input: any) {
    this.address = input.address;
    this.city = input.city;
    this.state = input.state;
    this.zip = input.zip;
    this.subsidized = input.subsidized;
    this.rentObligation = input.rentObligation;

    return this;
  }
}

export class HousingSection implements Serializable<HousingSection> {
  isSubmittedViaDriverFlow: boolean;
  housingSituationId: number;
  housingSituationName: string;
  currentHousingBeginDate: string;
  currentHousingEndDate: string;
  hasCurrentEvictionRisk: boolean;
  currentHousingDetails: string;
  currentMonthlyAmount: string;
  isCurrentAmountUnknown: boolean;
  hasBeenEvicted: boolean;
  hasUtilityDisconnectionRisk: boolean;
  utilityDisconnectionRiskNotes: string;
  hasDifficultyWorking: boolean;
  difficultyWorkingNotes: string;
  actionNeeded: ActionNeeded;
  histories: HousingHistory[];
  housingNotes: string;
  cwwHousing: CwwHousing[];
  rowVersion: string;
  modifiedBy: string;
  modifiedDate: string;

  static clone(input: any, instance: HousingSection) {
    instance.isSubmittedViaDriverFlow = input.isSubmittedViaDriverFlow;
    instance.housingSituationId = input.housingSituationId;
    instance.housingSituationName = input.housingSituationName;
    instance.currentHousingBeginDate = input.currentHousingBeginDate;
    instance.currentHousingEndDate = input.currentHousingEndDate;
    instance.hasCurrentEvictionRisk = input.hasCurrentEvictionRisk;
    instance.currentHousingDetails = input.currentHousingDetails;
    instance.currentMonthlyAmount = input.currentMonthlyAmount;
    instance.isCurrentAmountUnknown = input.isCurrentAmountUnknown;
    instance.hasBeenEvicted = input.hasBeenEvicted;
    instance.hasUtilityDisconnectionRisk = input.hasUtilityDisconnectionRisk;
    instance.utilityDisconnectionRiskNotes = input.utilityDisconnectionRiskNotes;
    instance.hasDifficultyWorking = input.hasDifficultyWorking;
    instance.difficultyWorkingNotes = input.difficultyWorkingNotes;
    instance.actionNeeded = Utilities.deserilizeChild(input.actionNeeded, ActionNeeded);
    instance.histories = Utilities.deserilizeChildren(input.histories, HousingHistory, 0);
    instance.housingNotes = input.housingNotes;
    instance.cwwHousing = Utilities.deserilizeChildren(input.cwwHousing, CwwHousing, 0);
    instance.rowVersion = input.rowVersion;
    instance.modifiedBy = input.modifiedBy;
    instance.modifiedDate = input.modifiedDate;

    return instance;
  }

  public deserialize(input: any) {
    HousingSection.clone(input, this);
    return this;
  }

  public validate(
    validationManager: ValidationManager,
    participantDob: string,
    otherHousingSituationId: number,
    homelessId: number,
    isHistoricalBeingEdited = false,
    isForcedAndEndDateVisible = false
  ): ValidationResult {
    const result = new ValidationResult();

    if (isForcedAndEndDateVisible === false) {
      this.validateCurrentHousingInitial(validationManager, participantDob, otherHousingSituationId, homelessId, result);
    }

    if (isForcedAndEndDateVisible === true) {
      this.validateCurrentHousingAfterMove(validationManager, participantDob, otherHousingSituationId, homelessId, result);
    }

    if (isHistoricalBeingEdited === true) {
      // TODO: Re-add this after demo.
      validationManager.addErrorWithDetail(ValidationCode.HistoryTableConfirm, 'Housing History');
    }
    Utilities.validateRequiredYesNo(
      result,
      validationManager,
      this.hasUtilityDisconnectionRisk,
      'hasUtilityDisconnectionRisk',
      'Are you at risk of having a utility disconnected?'
    );

    if (this.hasUtilityDisconnectionRisk === true) {
      Utilities.validateRequiredText(
        this.utilityDisconnectionRiskNotes,
        'utilityDisconnectionRiskNotes',
        'Are you at risk of having a utility disconnected? - Details',
        result,
        validationManager
      );
    }

    Utilities.validateRequiredYesNo(
      result,
      validationManager,
      this.hasDifficultyWorking,
      'hasDifficultyWorking',
      'Does your current housing situation make it hard to work or participate in work activities?'
    );

    if (this.hasDifficultyWorking === true) {
      Utilities.validateRequiredText(
        this.difficultyWorkingNotes,
        'difficultyWorkingNotes',
        'Does your current housing situation make it hard to work or participate in work activities? - Details',
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

  validateCurrentHousingAfterMove(
    validationManager: ValidationManager,
    participantDob: string,
    otherHousingSituationId: number,
    homelessId: number,
    result?: ValidationResult
  ): ValidationResult {
    if (result == null) {
      result = new ValidationResult();
    }

    // validationManager.resetErrors();

    // Second part of current housing.
    if (this.hasBeenEvicted == null && this.isForcedEvictedQuestionDisplayed(homelessId) === true) {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Were you evicted or forced to move?');

      result.addError('hasBeenEvicted');
    }

    const currentDate = Utilities.currentDate.day(1).format('MM/DD/YYYY');

    // Must be after DOB.
    // Cannot be more than 120 years since DOB.
    // Cannot be in future.
    const endDateContext: MmYyyyValidationContext = {
      date: this.currentHousingEndDate,
      prop: 'currentHousingEndDate',
      prettyProp: 'End Date',
      result: result,
      validationManager: validationManager,
      isRequired: true,
      minDate: this.currentHousingBeginDate,
      minDateAllowSame: true,
      minDateName: 'Begin Date',
      maxDate: currentDate,
      maxDateAllowSame: true,
      maxDateName: 'Current Date',
      participantDOB: null
    };
    Utilities.validateMmYyyyDate(endDateContext, 120, true);

    if (this.isDetailsRequired(otherHousingSituationId, homelessId)) {
      Utilities.validateRequiredText(this.currentHousingDetails, 'currentHousingDetails', 'Current housing details', result, validationManager);
    }

    return result;
  }

  validateCurrentHousingInitial(
    validationManager: ValidationManager,
    participantDob: string,
    otherHousingSituationId: number,
    homelessId: number,
    result?: ValidationResult
  ): ValidationResult {
    if (result == null) {
      result = new ValidationResult();
    }

    Utilities.validateDropDown(this.housingSituationId, 'housingSituationId', 'Housing Situation', result, validationManager);

    const currentDate = Utilities.currentDate.day(1).format('MM/DD/YYYY');

    // Must be after DOB.
    // Cannot be more than 120 years since DOB.
    // Cannot be in future.
    const beginDateContext: MmYyyyValidationContext = {
      date: this.currentHousingBeginDate,
      prop: 'currentHousingBeginDate',
      prettyProp: 'Begin Date',
      result: result,
      validationManager: validationManager,
      isRequired: true,
      minDate: moment(participantDob).format('MM/DD/YYYY'),
      minDateAllowSame: false,
      minDateName: "Participant's DOB",
      maxDate: currentDate,
      maxDateAllowSame: true,
      maxDateName: 'Current Date',
      participantDOB: null
    };
    Utilities.validateMmYyyyDate(beginDateContext);

    // Only check amount if unknown is not selected.
    if (this.isCurrentAmountUnknown == null || this.isCurrentAmountUnknown === false) {
      Utilities.validateRequiredCurrency(result, validationManager, this.currentMonthlyAmount, 'currentMonthlyAmount', 'Monthly Amount');
    }

    if (this.hasCurrentEvictionRisk == null && homelessId !== Number(this.housingSituationId)) {
      Utilities.validateRequiredYesNo(result, validationManager, this.hasCurrentEvictionRisk, 'hasCurrentEvictionRisk', 'Are you at risk of being evicted or forced to move?');
    }

    if (this.isDetailsRequired(otherHousingSituationId, homelessId)) {
      Utilities.validateRequiredText(this.currentHousingDetails, 'currentHousingDetails', 'Details', result, validationManager);
    }
    return result;
  }

  /**
   *  Required when "Are you at risk of being evicted or forced to move?" is set to yes or housingSituationId is to other.
   *
   * @param {number} otherHousingSituationId
   * @param {(number)} homelessId
   * @returns
   * @memberof HousingSection
   */
  isDetailsRequired(otherHousingSituationId: number, homelessId: number) {
    if (
      (this != null && this.hasCurrentEvictionRisk === true) ||
      (this != null && Number(this.housingSituationId) === otherHousingSituationId) ||
      (this != null && this.hasBeenEvicted === true)
    ) {
      return true;
    } else {
      return false;
    }
  }

  isForcedEvictedQuestionDisplayed(homelessId: number): boolean {
    if (Number(this.housingSituationId) === homelessId) {
      return false;
    }
    return true;
  }
}

export class HousingHistory implements IsEmpty, Serializable<HousingHistory> {
  id: number;
  historyType: number;
  historyTypeName: string;
  beginDate: string;
  endDate: string;
  hasEvicted: any;

  get _hasEvicted(): any {
    if (this.hasEvicted === 'true' || this.hasEvicted === true) {
      return true;
    } else if (this.hasEvicted === 'false' || this.hasEvicted === false) {
      return false;
    } else {
      return null;
    }
  }

  monthlyAmount: string;
  isAmountUnknown: boolean;
  details: string;

  deserialize(input: any) {
    this.id = input.id;
    this.historyType = input.historyType;
    this.historyTypeName = input.historyTypeName;
    this.beginDate = input.beginDate;
    this.endDate = input.endDate;
    this.hasEvicted = input.hasEvicted;
    this.monthlyAmount = input.monthlyAmount;
    this.isAmountUnknown = input.isAmountUnknown;
    this.details = input.details;

    return this;
  }

  /**
   * Detects whether or not a HousingSection object is effectively empty.
   *
   * @returns {boolean}
   *
   * @memberOf HousingSection
   */
  public isEmpty(): boolean {
    // All properties have to be null or blank to make the entire object empty.
    return (
      (this.historyType == null || this.historyType.toString() === '') &&
      (this.beginDate == null || this.beginDate.toString() === '') &&
      (this.endDate == null || this.endDate.toString() === '') &&
      this.hasEvicted == null &&
      (this.monthlyAmount == null || this.monthlyAmount.toString() === '') &&
      (this.isAmountUnknown == null || this.isAmountUnknown.toString() === '') &&
      (this.details == null || this.details.toString() === '')
    );
  }

  isDetailsRequired(otherHousingSituationId: number): boolean {
    if (Number(this.historyType) === otherHousingSituationId || this._hasEvicted === true) {
      return true;
    } else {
      return false;
    }
  }

  isEvictedDisabled(homelessHousingSituationId: number) {
    if (Number(this.historyType) === homelessHousingSituationId) {
      return true;
    } else {
      return false;
    }
  }

  public validate(validationManager: ValidationManager, otherHousingSituationId: number, homelessHousingSituationId: number, participantDob: string): ValidationResult {
    const result = new ValidationResult();

    Utilities.validateDropDown(this.historyType, 'historyType', 'Housing Situation', result, validationManager);

    const currentDate = Utilities.currentDate.day(1).format('MM/DD/YYYY');

    // Must be after DOB.
    // Cannot be in future.
    const beginDateContext: MmYyyyValidationContext = {
      date: this.beginDate,
      prop: 'beginDate',
      prettyProp: 'Begin Date',
      result: result,
      validationManager: validationManager,
      isRequired: true,
      minDate: moment(participantDob).format('MM/DD/YYYY'),
      minDateAllowSame: false,
      minDateName: "Participant's DOB",
      maxDate: currentDate,
      maxDateAllowSame: true,
      maxDateName: 'Current Date',
      participantDOB: null
    };
    Utilities.validateMmYyyyDate(beginDateContext);

    // Must be after DOB.
    // Cannot be in future.
    const endDateContext: MmYyyyValidationContext = {
      date: this.endDate,
      prop: 'endDate',
      prettyProp: 'End Date',
      result: result,
      validationManager: validationManager,
      isRequired: true,
      minDate: this.beginDate,
      minDateAllowSame: true,
      minDateName: 'Begin Date',
      maxDate: currentDate,
      maxDateAllowSame: true,
      maxDateName: 'Current Date',
      participantDOB: null
    };
    Utilities.validateMmYyyyDate(endDateContext);

    if (this.isEvictedDisabled(homelessHousingSituationId) === false) {
      Utilities.validateRequiredBoolean(this._hasEvicted, 'hasEvicted', 'Evicted?', result, validationManager);
    }

    if (this.isAmountUnknown !== true) {
      Utilities.validateRequiredCurrency(result, validationManager, this.monthlyAmount, 'monthlyAmount', 'Monthly Amount');
    }

    if (this.isDetailsRequired(otherHousingSituationId)) {
      Utilities.validateRequiredText(this.details, 'details', 'Details', result, validationManager);
    }

    return result;
  }
}
