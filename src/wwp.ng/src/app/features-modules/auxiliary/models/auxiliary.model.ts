import { User } from './../../../shared/models/user';
// tslint:disable: radix
import { AuxiliaryStatusTypes } from './../enums/auxiliary-status-types.enums';
import { Serializable } from '../../../shared/interfaces/serializable';
import { ValidationManager, ValidationResult, ValidationCode } from 'src/app/shared/models/validation';
import { AuxiliaryStatus } from './auxiliary-status.model';
import { Utilities } from 'src/app/shared/utilities';
import { DropDownField } from 'src/app/shared/models/dropdown-field';
import * as moment from 'moment';
import * as _ from 'lodash';

export enum AuxiliaryStatusCodes {
  SystemGenerated = 'SG'
}

export class Auxiliary implements Serializable<Auxiliary> {
  id: number;
  pinNumber: number;
  participantName: string;
  participantId: number;
  caseNumber: number;
  countyNumber: number;
  countyName: string;
  officeNumber: number;
  officeName: string;
  agencyCode: string;
  programCd: string;
  subProgramCd: string;
  agSequenceNumber: number;
  participationPeriodId: number;
  participationPeriodName: string;
  participationPeriodYear: number;
  originalPayment: number;
  requestedAmount: number;
  auxiliaryReasonId: number;
  auxiliaryReasonName: string;
  auxiliaryStatusTypeId: number;
  auxiliaryStatusTypeCode: string;
  auxiliaryStatusTypeName: string;
  auxiliaryStatusTypeDisplayName: string;
  auxiliaryStatusDate: string;
  details: string;
  isSubmit: boolean;
  isWithDraw: boolean;
  createdDate: string;
  isAllowed: boolean;
  overPayAmount: string;
  recoupmentAmount: number;
  auxiliaryStatuses: AuxiliaryStatus[];
  modifiedBy: string;
  modifiedDate: string;
  wiuid: string;
  requestedUserForApprovalAndDB2: string;

  public static clone(input: any, instance: Auxiliary) {
    instance.id = input.id;
    instance.pinNumber = input.pinNumber;
    instance.participantName = Utilities.formatDisplayPersonName(input.firstName, input.middleInitial, input.lastName, input.suffixName, true);
    instance.participantId = input.participantId;
    instance.caseNumber = input.caseNumber;
    instance.countyName = input.countyName;
    instance.countyNumber = input.countyNumber;
    instance.officeNumber = input.officeNumber;
    instance.officeName = input.officeName;
    instance.agencyCode = input.agencyCode;
    instance.programCd = input.programCd;
    instance.subProgramCd = input.subProgramCd;
    instance.agSequenceNumber = input.agSequenceNumber;
    instance.participationPeriodId = input.participationPeriodId;
    instance.participationPeriodName = input.participationPeriodName;
    instance.participationPeriodYear = input.participationPeriodYear;
    instance.originalPayment = input.originalPayment;
    instance.requestedAmount = input.requestedAmount;
    instance.auxiliaryStatusTypeId = input.auxiliaryStatusTypeId;
    instance.auxiliaryStatusTypeCode = input.auxiliaryStatusTypeCode;
    instance.auxiliaryStatusTypeName = input.auxiliaryStatusTypeName;
    instance.auxiliaryStatusTypeDisplayName = input.auxiliaryStatusTypeDisplayName;
    instance.auxiliaryReasonId = input.auxiliaryReasonId;
    instance.auxiliaryReasonName = input.auxiliaryReasonName;
    instance.auxiliaryStatusDate = input.auxiliaryStatusDate;
    instance.details = input.details;
    instance.isSubmit = input.isSubmit;
    instance.isWithDraw = input.isWithDraw;
    instance.auxiliaryStatuses = Utilities.deserilizeChildren(input.auxiliaryStatuses, AuxiliaryStatus);
    instance.createdDate = input.createdDate;
    instance.isAllowed = input.isAllowed;
    instance.recoupmentAmount = input.recoupmentAmount;
    if (input.overPayAmount) instance.overPayAmount = (Math.round(input.overPayAmount * 100) / 100).toFixed(2);
    instance.modifiedBy = input.modifiedBy;
    instance.modifiedDate = input.modifiedDate;
    instance.wiuid = input.wiuid;
    instance.requestedUserForApprovalAndDB2 = input.requestedUserForApprovalAndDB2;
  }

  public deserialize(input: any) {
    Auxiliary.clone(input, this);
    return this;
  }
  public isStatusRequired(statusTypeCode: string, canApprove: boolean) {
    return (statusTypeCode === AuxiliaryStatusTypes.SUBMIT || statusTypeCode === AuxiliaryStatusTypes.REVIEW || statusTypeCode === AuxiliaryStatusTypes.APPROVE) && canApprove;
  }
  public latestPassedPullDownDate(pullDownDates, inputDate) {
    return Utilities.getPullDownDate(pullDownDates, inputDate) || null;
  }
  public splitToMonthAndDate(participationPeriod) {
    return participationPeriod.split('-').map(i =>
      i
        .split(' ')
        .filter(j => !!j)
        .join('/')
    );
  }
  public datestoConsider(participationPeriodYear, participationPeriod) {
    const months = this.splitToMonthAndDate(participationPeriod);
    if (
      +moment()
        .month(months[0].split('/')[0])
        .format('M') === 12
    ) {
      return [moment(`${months[0]}/${participationPeriodYear - 1}`).format('MM/DD/YYYY'), moment(`${months[1]}/${participationPeriodYear}`).format('MM/DD/YYYY')];
    } else {
      return [moment(`${months[0]}/${participationPeriodYear}`).format('MM/DD/YYYY'), moment(`${months[1]}/${participationPeriodYear}`).format('MM/DD/YYYY')];
    }
  }

  public monthsAndDatesForValidation(participationPeriodYear: number, pullDownDates: DropDownField[], participationPeriod: string, auxiliaryFeatureToggle) {
    return {
      datesToConsider: this.datestoConsider(participationPeriodYear, participationPeriod),
      pullDownDate: auxiliaryFeatureToggle ? this.latestPassedPullDownDate(pullDownDates, moment(this.datestoConsider(participationPeriodYear, participationPeriod)[1])) : null
    };
  }

  public checkifPullDownDatePasses(data) {
    return Utilities.currentDate.isAfter(data.pullDownDate, 'day');
  }
  public validateForPassedPullDownDate(result: ValidationResult, validationManager: ValidationManager, res) {
    if (!this.checkifPullDownDatePasses(res)) {
      validationManager.addErrorWithDetail(
        ValidationCode.ValueOutOfRange_Details,
        'An auxiliary cannot be requested for this Participation Period because the Pulldown date has not passed.'
      );
      result.addError('participationPeriodId');
    } else {
      return false;
    }
  }

  public calculateW2Amount(totalW2PaymentForPP: number): number {
    totalW2PaymentForPP = totalW2PaymentForPP ? totalW2PaymentForPP : 0;
    this.overPayAmount = this.overPayAmount ? this.overPayAmount : '0';
    return this.originalPayment - +this.overPayAmount + totalW2PaymentForPP + +this.requestedAmount;
  }
  public validateForCutOverDate(result: ValidationResult, validationManager: ValidationManager, cutOverDate: string, maxW2Payment: number, res, totalW2PaymentForPP: number) {
    if (cutOverDate !== null && cutOverDate !== '') {
      if (moment(res.datesToConsider[1]).isBefore(cutOverDate) && +this.requestedAmount > maxW2Payment) {
        validationManager.addErrorWithDetail(
          ValidationCode.ValueOutOfRange_Details,
          'The requested auxiliary payment exceeds the maximum W-2 payment amount allowed. Review the W-2 Auxiliary calculations and resubmit.'
        );
        result.addError('requestedAmount');
        result.addError('calcError');
      } else if (moment(res.datesToConsider[1]).isSameOrAfter(cutOverDate)) {
        if (this.calculateW2Amount(totalW2PaymentForPP) > maxW2Payment) {
          validationManager.addErrorWithDetail(
            ValidationCode.ValueOutOfRange_Details,
            'The addition of the requested auxiliary payment exceeds the maximum W-2 payment amount allowed. Review the W-2 Auxiliary calculations and resubmit.'
          );
          result.addError('requestedAmount');
          result.addError('calcError');
        }
      }
    } else {
      if (+this.requestedAmount > maxW2Payment) {
        validationManager.addErrorWithDetail(
          ValidationCode.ValueOutOfRange_Details,
          'The requested auxiliary payment exceeds the maximum W-2 payment amount allowed. Review the W-2 Auxiliary calculations and resubmit.'
        );
        result.addError('requestedAmount');
        result.addError('calcError');
      }
    }
  }
  public validateForEligibilityAdult(result: ValidationResult, validationManager: ValidationManager, monthsAndDates, dateForEAValidation: string) {
    if (moment(monthsAndDates.datesToConsider[0]).isSameOrBefore(dateForEAValidation)) {
      validationManager.addErrorWithDetail(
        ValidationCode.RequiredInformationMissing_Details,
        'Participant does not meet the following requirement: Must previously have been an Eligible Adult in a W-2 Assistance Group in either a WW C or WW P Category of W-2.'
      );
      result.addError('participationPeriodId');
    } else {
      validationManager.addErrorWithDetail(
        ValidationCode.RequiredInformationMissing_Details,
        'Participant does not meet the following requirement: Must currently or previously have been an Eligible Adult in a W-2 Assistance Group in either a WW C or WW P Category of W-2 at this W-2 agency.'
      );
      result.addError('participationPeriodId');
    }
  }
  calculateTotalW2Payment(allAuxs, participationPeriod, participationPeriodYear): number {
    let totalW2PaymentForPP = 0;
    const auxStatusToCompare = [AuxiliaryStatusTypes.APPROVE.valueOf(), AuxiliaryStatusTypes.SYSTEMGENERATED.valueOf()];
    const calculate = (i: Auxiliary) => {
      if (
        auxStatusToCompare.indexOf(i.auxiliaryStatusTypeCode) >= 0 &&
        i.participationPeriodName === participationPeriod &&
        i.participationPeriodYear === participationPeriodYear
      ) {
        totalW2PaymentForPP += +i.requestedAmount;
      }
    };
    if (!_.isEmpty(allAuxs)) {
      allAuxs.map(calculate);
      return totalW2PaymentForPP;
    }
  }
  canNewAuxiliaryBeCreated(allAuxs: Auxiliary[], participationPeriod: string, participationPeriodYear: number) {
    return allAuxs.some(
      aux =>
        (aux.auxiliaryStatusTypeCode === AuxiliaryStatusTypes.SUBMIT ||
          aux.auxiliaryStatusTypeCode === AuxiliaryStatusTypes.RETURN ||
          aux.auxiliaryStatusTypeCode === AuxiliaryStatusTypes.REVIEW) &&
        aux.participationPeriodName === participationPeriod &&
        aux.participationPeriodYear === participationPeriodYear
    );
  }
  // rename the method name so that it makes more sense.
  validationsForParticipationPeriod(
    validationManager: ValidationManager,
    allAuxs: Auxiliary[],
    participationPeriod,
    participationPeriodYear,
    pullDownDates,
    directlyCalled,
    auxiliaryFeatureToggle,
    result = new ValidationResult()
  ) {
    const dateForEligibilityAdultValidation = '02/13/2013';
    if (this.id === 0 && this.canNewAuxiliaryBeCreated(allAuxs, participationPeriod, participationPeriodYear)) {
      validationManager.addErrorWithDetail(
        ValidationCode.ValueOutOfRange_Details,
        'A new auxiliary cannot be requested for this Participation Period because there is currently an auxiliary in one of the following statuses: Submitted, Review in Progress, or Returned – Information Needed.'
      );
      result.addError('auxiliaryStatusTypeId');
    }
    const monthsAndDates = this.monthsAndDatesForValidation(participationPeriodYear, pullDownDates, participationPeriod, auxiliaryFeatureToggle);
    if (!!monthsAndDates.pullDownDate) {
      this.validateForPassedPullDownDate(result, validationManager, monthsAndDates);
    }
    if (this.isAllowed !== null && !this.isAllowed) {
      this.validateForEligibilityAdult(result, validationManager, monthsAndDates, dateForEligibilityAdultValidation);
    }
    if (directlyCalled) {
      return result;
    }
  }
  public validate(
    validationManager: ValidationManager,
    canApprove: boolean,
    cutOverDate: string,
    allAuxiliaries,
    participationPeriod: string,
    pullDownDates: DropDownField[],
    currentDate,
    auxiliaryFeatureToggle,
    auxiliaryStatusTypesDrop?: DropDownField[],
    isParticipationPeriodValid = true,
    user?: User
  ): ValidationResult {
    const result = new ValidationResult();

    const maxW2Payment = 673;
    Utilities.validatePropForNullAndEmptyValues(result, validationManager, this.participationPeriodId, 'participationPeriodId', 'Participant Period');
    Utilities.validatePropForNullAndEmptyValues(result, validationManager, this.participationPeriodYear, 'participationPeriodYear', 'Year');

    if (_.isEmpty(result.errors) && isParticipationPeriodValid) {
      Utilities.validatePropForNullAndEmptyValues(result, validationManager, this.originalPayment, 'originalPayment', 'Original Payment');
      Utilities.validatePropForNullAndEmptyValues(result, validationManager, this.requestedAmount, 'requestedAmount', 'W-2 Auxiliary Amount');
      Utilities.validatePropForNullAndEmptyValues(result, validationManager, this.auxiliaryReasonId, 'auxiliaryReasonId', 'Reason');
      if (
        (_.isEmpty(result.errors) || !result.errors.requestedAmount) &&
        !isNaN(this.requestedAmount) &&
        parseInt(this.requestedAmount.toString()) === 0 &&
        +this.requestedAmount === 0
      ) {
        validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, 'W-2 Auxiliary Amount cannot be $0');
        result.addError('requestedAmount');
      }

      if (this.isStatusRequired(this.auxiliaryStatusTypeCode, canApprove)) {
        if (Utilities.fieldDataCodeById(this.auxiliaryStatusTypeId, auxiliaryStatusTypesDrop, true) === '') {
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Auxiliary Status');
          result.addError('auxiliaryStatusTypeId');
        }
        if (this.auxiliaryStatusTypeId && this.requestedUserForApprovalAndDB2 === `${user.wiuid}`) {
          validationManager.addErrorWithDetail(
            ValidationCode.ValueOutOfRange_Details,
            'A worker with Auxiliary Approver security cannot update the auxiliary status for their own requests. A different worker with Auxiliary Approver security must update the status on the Auxiliary Requests page.'
          );
          result.addError('auxiliaryStatusTypeId');
        }
      }
      Utilities.validatePropForNullAndEmptyValues(result, validationManager, this.details, 'details', 'Details');
      if (!this.auxiliaryStatusTypeId || this.auxiliaryStatusTypeCode === AuxiliaryStatusTypes.RETURN) {
        const monthsAndDates = this.monthsAndDatesForValidation(this.participationPeriodYear, pullDownDates, participationPeriod, auxiliaryFeatureToggle);

        this.validationsForParticipationPeriod(
          validationManager,
          allAuxiliaries,
          participationPeriod,
          this.participationPeriodYear,
          pullDownDates,
          false,
          auxiliaryFeatureToggle,
          result
        );

        // this.validateForPassedPullDownDate(result, validationManager, monthsAndDates);
        if (!!monthsAndDates.pullDownDate && this.validateForPassedPullDownDate(result, validationManager, monthsAndDates) === false) {
          this.validateForCutOverDate(
            result,
            validationManager,
            cutOverDate,
            maxW2Payment,
            monthsAndDates,
            this.calculateTotalW2Payment(allAuxiliaries, participationPeriod, this.participationPeriodYear) || 0
          );
        }
      }
    }
    return result;
  }
}
