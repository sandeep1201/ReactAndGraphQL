import { POPClaimStatusTypes } from './../enums/pop-claim-status-types.enum';
import { ValidationResult, ValidationCode } from './../../../shared/models/validation';
import { ValidationManager } from 'src/app/shared/models/validation-manager';
import { Utilities } from 'src/app/shared/utilities';
import { Serializable } from 'src/app/shared/interfaces/serializable';
import { POPClaimStatus } from './pop-claim-status.model';
import { POPClaimEmployment } from './pop-claim-employment.model';
import * as moment from 'moment';
import * as _ from 'lodash';

export class POPClaim implements Serializable<POPClaim> {
  id: number;
  participantId: number;
  pinNumber: number;
  participantName: string;
  popClaimTypeId: number;
  popClaimTypeCode: string;
  popClaimTypeName: string;
  organizationId: number;
  agencyCode: string;
  agencyName: string;
  activityCode: string;
  activityBeginDate: string;
  activityEndDate: string;

  claimPeriodBeginDate: string;
  claimEffectiveDate: string;
  isDeleted: boolean;
  modifiedBy: string;
  modifiedDate: string;
  claimStatus: string;
  claimStatusDate: string;
  details: string;
  isSubmit: boolean;
  claimStatusTypeId: number;
  claimStatusTypeCode: string;
  claimStatusTypeName: string;
  claimStatusTypeDisplayName: string;
  popClaimStatuses: POPClaimStatus[];
  popClaimEmployments: POPClaimEmployment[];
  canUsePEBDForCPBD: boolean;

  public static clone(input: any, instance: POPClaim) {
    instance.id = input.id;
    instance.participantId = input.participantId;
    instance.pinNumber = input.pinNumber;
    instance.participantName = input.participantName;
    instance.organizationId = input.organizationId;
    instance.agencyCode = input.agencyCode;
    instance.agencyName = input.agencyName;
    instance.activityCode = input.activityCode;
    instance.activityBeginDate = input.activityBeginDate;
    instance.activityEndDate = input.activityEndDate;
    instance.popClaimTypeId = input.popClaimTypeId;
    instance.popClaimTypeName = input.popClaimTypeName;
    instance.claimPeriodBeginDate = input.claimPeriodBeginDate;
    instance.claimEffectiveDate = input.claimEffectiveDate;
    instance.isDeleted = input.isDeleted;
    instance.modifiedBy = input.modifiedBy;
    instance.modifiedDate = input.modifiedDate;
    instance.claimStatusDate = input.claimStatusDate;
    instance.claimStatusTypeCode = input.claimStatusTypeCode;
    instance.claimStatusTypeId = input.claimStatusTypeId;
    instance.claimStatusTypeName = input.claimStatusTypeName;
    instance.claimStatusTypeDisplayName = input.claimStatusTypeDisplayName;
    instance.popClaimStatuses = Utilities.deserilizeChildren(input.popClaimStatuses, POPClaimStatus);
    instance.popClaimEmployments = Utilities.deserilizeChildren(input.popClaimEmployments, POPClaimEmployment);
  }
  public deserialize(input: any) {
    POPClaim.clone(input, this);
    return this;
  }

  validate(validationManager: ValidationManager, isFromEditView: boolean, isHighWagePOPClaimSelectedbool = false, filteredStatusDropDown: any[]): ValidationResult {
    const result = new ValidationResult();
    const maxDaysAnEffectiveDateCanBeInThePast = 90;
    const primaryEmploymentWindowInDays = 180;
    const consecutiveDaysForClaimPeriodDate = 31;
    Utilities.validatePropForNullAndEmptyValues(result, validationManager, this.popClaimTypeId, 'popClaimTypeId', 'POP Claim Type');
    if (Object.keys(this.popClaimEmployments[0]).length > 0) {
      const errArr = result.createErrorsArray('employments');
      if (isHighWagePOPClaimSelectedbool) {
        if (!this.popClaimEmployments.some(emp => emp.isPrimary)) {
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'One of the employments must be marked as the Primary Employment.');
          result.addError('primaryEmploymentCheck');
        }
      } else {
        if (!this.popClaimEmployments.some(emp => emp.isSelected)) {
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'At least one employment must be selected.');
          result.addError('selectionCheck');
        }
        if (!this.popClaimEmployments.some(emp => emp.isSelected && emp.isPrimary)) {
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'One of the selected employments must be marked as the Primary Employment.');
          result.addError('primaryEmploymentCheck');
        }
      }

      let v: ValidationResult;
      this.popClaimEmployments.forEach(emp => {
        v = emp.validate(validationManager, isHighWagePOPClaimSelectedbool);
        errArr.push(v.errors);
        if (v.isValid === false) {
          result.isValid = false;
        }
      });
    }
    if (isFromEditView) {
      Utilities.validatePropForNullAndEmptyValues(
        result,
        validationManager,
        this.canUsePEBDForCPBD,
        'canUsePEBDForCPBD',
        'Use the Primary Employment Begin Date as the Claim Period Begin Date?'
      );
    }
    Utilities.validatePropForNullAndEmptyValues(result, validationManager, this.claimPeriodBeginDate, 'claimPeriodBeginDate', 'Claim Period Begin Date');
    this.validateClaimPeriodDate(
      result,
      validationManager,
      this.claimPeriodBeginDate,
      this.popClaimEmployments.filter(emp => emp.isPrimary)[0],
      primaryEmploymentWindowInDays,
      consecutiveDaysForClaimPeriodDate
    );
    Utilities.validatePropForNullAndEmptyValues(result, validationManager, this.claimEffectiveDate, 'claimEffectiveDate', 'Claim Effective Date');
    if (
      filteredStatusDropDown.some(status => status.id === this.claimStatusTypeId) &&
      this.claimStatusTypeCode === POPClaimStatusTypes.SUBMIT &&
      moment(this.claimEffectiveDate).isBefore(Utilities.currentDate) &&
      moment(Utilities.currentDate).diff(this.claimEffectiveDate, 'days') > maxDaysAnEffectiveDateCanBeInThePast
    ) {
      validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, 'Claim must be submitted within 90 days of the Claim Effective Date.');
      result.addError('claimEffectiveDate');
    }
    if (
      filteredStatusDropDown.some(status => status.id === this.claimStatusTypeId) &&
      this.claimStatusTypeCode === POPClaimStatusTypes.APPROVE &&
      moment(this.claimEffectiveDate).isBefore(Utilities.currentDate) &&
      moment(Utilities.currentDate).diff(this.claimEffectiveDate, 'days') > maxDaysAnEffectiveDateCanBeInThePast
    ) {
      validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, 'Claim must be approved within 90 days of the Claim Effective Date.');
      result.addError('claimEffectiveDate');
    }
    if (filteredStatusDropDown.length > 0) {
      if (!filteredStatusDropDown.some(status => status.id === this.claimStatusTypeId)) {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Claim Status Type');
        result.addError('claimStatusTypeId');
      }
    }
    Utilities.validatePropForNullAndEmptyValues(result, validationManager, this.details, 'details', 'Details');

    return result;
  }
  validateClaimPeriodDate(
    result: ValidationResult,
    validationManager: ValidationManager,
    claimPeriodBeginDate: string,
    primaryPOPEmployment: POPClaimEmployment,
    primaryEmploymentWindowInDays: number,
    consecutiveDaysForClaimPeriodDate: number
  ) {
    if (!_.isEmpty(primaryPOPEmployment)) {
      const primaryEmploymentBeginDate = moment(primaryPOPEmployment.jobBeginDate);
      const lastDaytoConsiderForprimaryEmploymentBeginDate = moment(primaryPOPEmployment.jobBeginDate).add(primaryEmploymentWindowInDays, 'days');
      const lastDaytoConsiderForClaimPeriodBeginDate = moment(claimPeriodBeginDate).add(consecutiveDaysForClaimPeriodDate, 'days');
      if (
        !moment(claimPeriodBeginDate).isBetween(primaryEmploymentBeginDate, lastDaytoConsiderForprimaryEmploymentBeginDate) &&
        !moment(lastDaytoConsiderForClaimPeriodBeginDate).isBetween(primaryEmploymentBeginDate, lastDaytoConsiderForprimaryEmploymentBeginDate)
      ) {
        validationManager.addErrorWithDetail(
          ValidationCode.ValueOutOfRange_Details,
          'Claim Period Begin Date must be a consecutive 31 day period within 180 days of the Primary Employment Begin Date.'
        );
        result.addError('claimPeriodBeginDate');
      }
    }
  }
}
