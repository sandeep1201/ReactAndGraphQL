import { EARequest } from './ea-request.model';
import { Serializable } from 'src/app/shared/interfaces/serializable';
import { EARequestEmergencyTypeSection } from './ea-request-emergency-type.model';
import { Utilities } from 'src/app/shared/utilities';
import { ValidationManager, ValidationResult, ValidationCode } from 'src/app/shared/models/validation';
import { DropDownField } from 'src/app/shared/models/dropdown-field';
import * as moment from 'moment';
import { EAStatusCodes, EAEmergencyCodes } from './ea-request-sections.enum';

export class EAAgencySummarySection implements Serializable<EAAgencySummarySection> {
  groupSize: number;
  totalIncome: string;
  totalAssets: string;
  approvedPaymentsPast12Months: boolean;
  hasActiveIPV: boolean;
  hasPendingIPV: boolean;
  hasComment: boolean;
  eaEmergencyType: EARequestEmergencyTypeSection;
  statusId: number;
  statusName: string;
  statusReasonIds: number[];
  statusReasonNames: string[];
  statusReasonCodes: string[];
  approvedPaymentAmount: string;
  maxPaymentAmount: string;
  hasFinancialEligibilityPassed: boolean;
  eaFinancialNeeds: EAFinancialNeeds[];
  notes: string;
  requestId: number;
  modifiedBy: string;
  modifiedDate: string;
  isSubmittedViaDriverFlow: boolean;
  isSubmit = false;

  public static create() {
    const eaAgencySummary = new EAAgencySummarySection();
    eaAgencySummary.requestId = 0;
    eaAgencySummary.eaEmergencyType = new EARequestEmergencyTypeSection();
    eaAgencySummary.eaFinancialNeeds = [];
    return eaAgencySummary;
  }

  public static clone(input: any, instance: EAAgencySummarySection) {
    instance.groupSize = input.groupSize;
    instance.totalIncome = input.totalIncome;
    instance.totalAssets = input.totalAssets;
    instance.approvedPaymentsPast12Months = input.approvedPaymentsPast12Months;
    instance.hasActiveIPV = input.hasActiveIPV;
    instance.hasPendingIPV = input.hasPendingIPV;
    instance.hasComment = input.hasComment;
    instance.eaEmergencyType = Utilities.deserilizeChild(input.eaEmergencyType, EARequestEmergencyTypeSection);
    instance.eaFinancialNeeds = Utilities.deserilizeChildren(input.eaFinancialNeeds, EAFinancialNeeds, 0);
    instance.statusId = input.statusId;
    instance.statusName = input.statusName;
    instance.statusReasonIds = Utilities.deserilizeArray(input.statusReasonIds);
    instance.statusReasonNames = Utilities.deserilizeArray(input.statusReasonNames);
    instance.statusReasonCodes = Utilities.deserilizeArray(input.statusReasonCodes);
    instance.approvedPaymentAmount = input.approvedPaymentAmount;
    instance.maxPaymentAmount = input.maxPaymentAmount;
    instance.hasFinancialEligibilityPassed = input.hasFinancialEligibilityPassed;
    instance.notes = '';
    instance.requestId = input.requestId;
    instance.modifiedBy = input.modifiedBy;
    instance.modifiedDate = input.modifiedDate;
    instance.isSubmittedViaDriverFlow = input.isSubmittedViaDriverFlow;
  }

  public deserialize(input: any) {
    EAAgencySummarySection.clone(input, this);
    return this;
  }

  public validate(validationManager: ValidationManager, eaStatusDrop: DropDownField[], model: EARequest, isWorker = false, isSubmitEnabled = false): ValidationResult {
    const result = new ValidationResult();
    const errArr = result.createErrorsArray('eaFinancialNeeds');
    const statusCode = Utilities.fieldDataCodeById(this.statusId, eaStatusDrop);

    if (!(statusCode === EAStatusCodes.InProgress || statusCode === EAStatusCodes.Approved))
      Utilities.validateMultiSelect(this.statusReasonIds, 'statusReasonId', 'Status Reason', result, validationManager);

    // Approved or Pending
    if (statusCode === EAStatusCodes.Approved || statusCode === EAStatusCodes.Pending) {
      if (statusCode === EAStatusCodes.Approved) {
        if (Utilities.isStringEmptyOrNull(this.approvedPaymentAmount)) {
          result.addError('approvedPaymentAmount');
          result.addError('disableApprove');
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Approved Payment Amount');
        } else if (this.approvedPaymentAmount && Utilities.currencyToNumber(this.approvedPaymentAmount) <= 0) {
          result.addError('approvedPaymentAmount');
          result.addError('disableApprove');
          validationManager.addErrorWithDetail(ValidationCode.DuplicateDataWithNoMessage, 'Approved Payment must be greater than 0.00 when approving an EA application.');
        }
      }
      if (
        !Utilities.isStringEmptyOrNull(this.approvedPaymentAmount) &&
        this.approvedPaymentAmount &&
        Utilities.currencyToNumber(this.approvedPaymentAmount) > this.calculateLesserMaxPaymentAmount(this.eaFinancialNeeds)
      ) {
        result.addError('approvedPaymentAmount');
        result.addError('disableApprove');
        validationManager.addErrorWithDetail(ValidationCode.DuplicateDataWithNoMessage, 'Approved Payment Amount is greater than Lesser Amount.');
      }
      if (!this.hasFinancialEligibilityPassed) {
        validationManager.addErrorWithDetail(ValidationCode.DuplicateDataWithNoMessage, 'EA group does not meet financial eligibility.');
        result.addError('disableApprove');
      }
      if (!model.eaGroupMembers.eaGroupMembers.some(x => x.eaIndividualTypeName === 'Dependent Child' && x.isIncluded)) {
        validationManager.addErrorWithDetail(ValidationCode.DuplicateDataWithNoMessage, 'EA group must include at least the caretaker relative and one dependent child.');
        result.addError('disableApprove');
      }
      if (model.eaDemographics.didApplicantTakeCareOfAnyChild === false) {
        validationManager.addErrorWithDetail(
          ValidationCode.DuplicateDataWithNoMessage,
          "EA application cannot be pending or approved if No answered for Do you take care of and make decisions for either your child or a relative's child in your house ?"
        );
        result.addError('disableApprove');
      }
      if (model.eaDemographics.willTheChildStayInApplicantCare === false) {
        validationManager.addErrorWithDetail(
          ValidationCode.DuplicateDataWithNoMessage,
          'EA application cannot be pending or approved if No answered for Will this child(ren) stay in your care in the future?'
        );
        result.addError('disableApprove');
      }
      if (model.eaGroupMembers.eaGroupMembers.every(x => !(!!x.ssn || !!x.ssnExemptTypeId || (x.ssnAppliedDate && moment(x.ssnAppliedDate) >= moment().subtract(6, 'months'))))) {
        validationManager.addErrorWithDetail(ValidationCode.DuplicateDataWithNoMessage, 'Group member exists that does not meet SSN requirements.');
        result.addError('disableApprove');
      }
      if (!this.hasComment) {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'At least one EA comment must exist.');
        result.addError('disableApprove');
      }
      if (this.hasActiveIPV) {
        validationManager.addErrorWithDetail(ValidationCode.DuplicateDataWithNoMessage, 'An adult group member has an active EA IPV.');
        result.addError('disableApprove');
      }
      if (model.eaEmergencyType && model.eaEmergencyType.eaEnergyCrisis && model.eaEmergencyType.eaEnergyCrisis.haveThreat === false) {
        validationManager.addErrorWithDetail(
          ValidationCode.DuplicateDataWithNoMessage,
          'EA application for energy crisis does not meet the criteria of an immediate threat to health or safety of an EA group member.'
        );
        result.addError('disableApprove');
      }

      if (this.eaEmergencyType.emergencyTypeCodes && this.eaEmergencyType.emergencyTypeCodes.includes(EAEmergencyCodes.ImpendingHomelessness)) {
        Utilities.validateDropDown(
          this.eaEmergencyType.eaImpendingHomelessness.emergencyTypeReasonId,
          'IHLemergencyTypeReasonId',
          'Impending Homelessness Reason',
          result,
          validationManager
        );
      }
      if (this.eaEmergencyType.emergencyTypeCodes && this.eaEmergencyType.emergencyTypeCodes.includes(EAEmergencyCodes.Homelessness)) {
        Utilities.validateDropDown(this.eaEmergencyType.eaHomelessness.emergencyTypeReasonId, 'HLNemergencyTypeReasonId', 'Homelessness Reason', result, validationManager);
      }
      const errors = ['IHLemergencyTypeReasonId', 'HLNemergencyTypeReasonId'];
      if (Object.keys(result.errors).find(x => errors.includes(x))) {
        result.addError('disableApprove');
      }
    }

    if (statusCode === EAStatusCodes.Approved) {
      let atLeastOneNeedIsNonEmpty = false;
      if (this.eaFinancialNeeds == null || this.eaFinancialNeeds.length === 0) {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Amount');
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Financial Need');
        const me = result.createErrorsArrayItem(errArr);
        result.addErrorForParent(me, 'amount');
        result.addErrorForParent(me, 'financialNeedTypeId');
        result.addError('disableApprove');
      } else {
        const errArr = result.createErrorsArray('eaFinancialNeeds');
        for (const need of this.eaFinancialNeeds) {
          const me = result.createErrorsArrayItem(errArr);
          if (!need.isEmpty()) {
            atLeastOneNeedIsNonEmpty = true;
            if (Utilities.isStringEmptyOrNull(need.amount)) {
              result.addErrorForParent(me, 'amount');
              result.addError('disableApprove');
              validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Amount');
            } else if (need.amount && (isNaN(Utilities.currencyToNumber(need.amount)) || +need.amount < 0)) {
              validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, 'Amount');
              result.addErrorForParent(me, 'amount');
              result.addError('disableApprove');
            }
            if (Utilities.isNumberEmptyOrNull(need.financialNeedTypeId)) {
              result.addErrorForParent(me, 'financialNeedTypeId');
              validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Financial Need');
              result.addError('disableApprove');
            }
          }
        }
        if (!atLeastOneNeedIsNonEmpty) {
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Amount');
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Financial Need');
          const me = errArr[0];
          result.addErrorForParent(me, 'amount');
          result.addErrorForParent(me, 'financialNeedTypeId');
          result.addError('disableApprove');
        }
      }

      if (this.approvedPaymentsPast12Months) {
        validationManager.addErrorWithDetail(ValidationCode.DuplicateDataWithNoMessage, 'An adult group member has received an EA payment in the last 12 months.');
        result.addError('disableApprove');
      }
      if (isWorker && isSubmitEnabled && this.hasPendingIPV) {
        validationManager.addErrorWithDetail(
          ValidationCode.DuplicateDataWithNoMessage,
          'An adult group member has a pending EA IPV. Only a supervisor can approve the application.'
        );
        result.addError('disableApprove');
      }
    }

    // WithDrawn or Denied
    if (statusCode === EAStatusCodes.Denied || statusCode === EAStatusCodes.WithDrawn) {
      if (
        model.eaDemographics.eaDemographicsContact.isMailingSameAsHouseholdAddress
          ? !model.eaDemographics.eaDemographicsContact.householdAddress.addressLine1
          : !model.eaDemographics.eaDemographicsContact.mailingAddress.addressLine1
      ) {
        validationManager.addErrorWithDetail(
          ValidationCode.RequiredInformationMissing_Details,
          model.eaDemographics.eaDemographicsContact.isMailingSameAsHouseholdAddress ? 'Household Address' : 'Mailing Address'
        );
        result.addError('disableDeined');
      }
      if (!model.eaDemographics.eaDemographicsContact.countyOfResidenceId) {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'County');
        result.addError('disableDeined');
      }
      if ((this.statusReasonIds && !this.statusReasonIds.length) || !this.statusReasonIds) {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Status Reason');
        result.addError('disableDeined');
      }
      if (!this.hasComment) {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'At least one EA comment must exist.');
        result.addError('disableDeined');
      }
      if (this.approvedPaymentAmount && this.approvedPaymentAmount !== '' && +this.approvedPaymentAmount.toString().replace(/[^0-9.-]+/g, '') > 0) {
        validationManager.addErrorWithDetail(
          ValidationCode.DuplicateDataWithNoMessage,
          `EA application cannot be ${statusCode === EAStatusCodes.Denied ? 'denied' : 'withdrawn'} when approved payment amount is greater than $0.00.`
        );
        result.addError('disableDeined');
        result.addError('approvedPaymentAmount');
      }
    }

    return result;
  }

  calculateLesserMaxPaymentAmount(needs: EAFinancialNeeds[]): number {
    let total = 0;
    needs.forEach(x => (total += !Utilities.isStringEmptyOrNull(x.amount) ? Utilities.currencyToNumber(x.amount) : 0));
    return total < +this.maxPaymentAmount ? total : +this.maxPaymentAmount;
  }
}

export class EAFinancialNeeds implements Serializable<EAFinancialNeeds> {
  id: number;
  amount: string;
  financialNeedTypeId: number;
  financialNeedTypeName: string;

  public static create(): EAFinancialNeeds {
    const vehicle = new EAFinancialNeeds();
    vehicle.id = 0;
    return vehicle;
  }

  public static clone(input: any, instance: EAFinancialNeeds) {
    instance.id = input.id;
    instance.amount = input.amount;
    instance.financialNeedTypeId = input.financialNeedTypeId;
    instance.financialNeedTypeName = input.financialNeedTypeName;
  }

  public clear(): void {
    this.amount = null;
    this.financialNeedTypeId = null;
    this.financialNeedTypeName = null;
  }

  /**
   * Detects whether or not a financial need object is effectively empty.
   *
   * @returns {boolean}
   *
   * @memberOf financial need
   */
  public isEmpty(): boolean {
    // All properties have to be null or blank to make the entire object empty.
    return (this.amount == null || this.amount.toString() === '') && (this.financialNeedTypeId == null || this.financialNeedTypeId.toString() === '');
  }

  public deserialize(input: any) {
    EAFinancialNeeds.clone(input, this);
    return this;
  }
}
