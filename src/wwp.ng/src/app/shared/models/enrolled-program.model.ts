import { AssignedWorker } from './assigned-worker.model';
import { Utilities } from '../utilities';
import { MmDdYyyyValidationContext } from '../interfaces/mmDdYyyy-validation-context';
import { ValidationResult } from './validation-result';
import { ValidationManager } from './validation-manager';
import * as moment from 'moment';
import { AppService } from 'src/app/core/services/app.service';
import { EnrolledProgramCode } from '../../shared/enums/enrolled-program-code.enum';
import { EnrolledProgramStatus } from '../../shared/enums/enrolled-program-status.enum';
import { WhyReason } from '../../shared/models/why-reasons.model';
import { LearnFareFEP } from './learnfare-fep.model';
import { HasProgramCode } from '../interfaces/program-code.interface';
import { ValidationCode } from './validation';

export class EnrolledProgram implements HasProgramCode {
  public id: number;
  public agencyCode: string;
  public agencyName: string;
  public associatedAgencyCodes: string[];
  public associatedAgencyNames: string[];
  public assignedWorker: AssignedWorker;
  public disenrollmentDate: string;
  public completionReasonId: number;
  public enrollmentDate: string;
  public officeId: number;
  public officeCounty: string;
  public officeNumber: number;
  public rfaNumber: string;
  public referralDate: string;
  public enrolledProgramId: number;
  /**
   * Program Code ex. WW.
   *
   * @type {string}
   * @memberof EnrolledProgram
   */
  public programCd: string;
  /**
   * Program name. W-2.
   *
   * @type {string}
   * @memberof EnrolledProgram
   */
  public programCode: string;
  public status: string;
  public statusDate: string;
  public subProgramCode: string;
  public isTransfer: boolean;
  public courtOrderedDate: string;
  public courtOrderedCounty: string;
  public completionReasonDetails: string;
  public contractorName: string;
  public isVoluntary: boolean;
  public learnFareFEP: LearnFareFEP;
  public caseNumber: number;

  public otherCompletionReasonId: number;

  public static getProgramCodeByName(enrolledProgramName: string): string {
    switch (
      enrolledProgramName
        .replace(/\s/g, '')
        .trim()
        .toLowerCase()
    ) {
      case 'w-2':
        return 'WW';
      case 'transitionaljobs':
        return 'TJ';
      case 'transformmilwaukeejobs':
        return 'TMJ';
      case 'learnfare':
        return 'LF';
      case 'childrenfirst':
        return 'CF';
      case 'fivecountydemonstrationproject':
        return 'FCD';
      default: {
        break;
      }
    }
  }
  public static disenrollmentValidate(model: EnrolledProgram, selectedProgramName: string, validationManager: ValidationManager, precheck?: WhyReason): ValidationResult {
    const result = new ValidationResult();

    Utilities.validateRequiredText(selectedProgramName, 'selectedProgramName', 'Program', result, validationManager);

    if (precheck == null || precheck.status === false) {
      result.isValid = false;
      return result;
    }

    if (model.isCFTmjTJFcdpProgram) {
      let minDate = '';
      let preCheckActivityMinDate = false;

      // Grab the latest for minDate.
      if (moment(precheck.activityEndDate, moment.ISO_8601) > moment(model.enrollmentDate, moment.ISO_8601)) {
        minDate = moment(precheck.activityEndDate, moment.ISO_8601).format('MM/DD/YYYY');
        preCheckActivityMinDate = true;
      } else {
        minDate = moment(model.enrollmentDate, moment.ISO_8601).format('MM/DD/YYYY');
        preCheckActivityMinDate = false;
      }
      const disenrollmentDateValidationPreCheckContext: MmDdYyyyValidationContext = {
        date: model.disenrollmentDate,
        prop: 'disenrollmentDate',
        prettyProp: 'Disenrollment Date',
        result: result,
        validationManager: validationManager,
        isRequired: true,
        minDate: minDate,
        minDateAllowSame: true,
        minDateName: minDate,
        maxDate: moment(Utilities.currentDate, moment.ISO_8601).format('MM/DD/YYYY'),
        maxDateAllowSame: true,
        maxDateName: 'Current Date',
        participantDOB: null
      };
      if (preCheckActivityMinDate) Utilities.validateMmDdYyyyDateCustomError(disenrollmentDateValidationPreCheckContext, ValidationCode.DisenrollmentPrecheckActivityError);
      else Utilities.validateMmDdYyyyDateCustomError(disenrollmentDateValidationPreCheckContext, ValidationCode.DisenrollmentPrecheckEnrollmentError);

      Utilities.validateDropDown(model.completionReasonId, 'completionReasonId', 'Completion Reason', result, validationManager);

      if (model.isDisenrollmentDetailsDisplayedForDisenrollment && model.isDisenrollmentDetailsRequiredForDisenrollment) {
        Utilities.validateRequiredText(model.completionReasonDetails, 'completionReasonDetails', 'Completion Reason - Details', result, validationManager);
      }
    }
    return result;
  }

  public static enrollmentValidate(selectedWorkerId: number, selectedProgramName: string, validationManager: ValidationManager, precheck?: boolean): ValidationResult {
    const result = new ValidationResult();

    Utilities.validateRequiredText(selectedProgramName, 'selectedProgramName', 'Program', result, validationManager);
    Utilities.validateDropDown(selectedWorkerId, 'selectedWorkerId', 'Assigned Worker', result, validationManager);

    if (precheck === false) {
      result.isValid = false;
      return result;
    }

    return result;
  }

  public static filterByStatus(programs: EnrolledProgram[], status: string): EnrolledProgram[] {
    if (programs != null) {
      const peps = [];
      for (const x of programs) {
        if (x.status != null && x.status.toLowerCase() === status.toLowerCase()) {
          peps.push(x);
        }
      }
      return peps;
    } else {
      return [];
    }
  }

  public static isStatus(program: EnrolledProgram, status: string): boolean {
    if (program == null) {
      return false;
    }

    if (program.status != null && program.status.toLowerCase() === status.toLowerCase()) {
      return true;
    }

    return false;
  }

  public static filterByStatuses(programs: EnrolledProgram[], statuses: EnrolledProgramStatus[]): EnrolledProgram[] {
    if (programs != null) {
      const peps = [];
      for (const x of programs) {
        statuses.forEach(stat => {
          if (x.status != null && x.status.toLowerCase() === stat.toLowerCase()) {
            if (peps.indexOf(x) < 0) {
              peps.push(x);
            }
          }
        });
      }
      return peps;
    } else {
      return [];
    }
  }

  public static filterByName(programs: EnrolledProgram[], programName: string): EnrolledProgram[] {
    if (programs != null) {
      const peps = [];
      for (const x of programs) {
        if (x.programCode != null && x.programCode.toLowerCase() === programName.toLowerCase()) {
          peps.push(x);
        }
      }
      return peps;
    } else {
      return [];
    }
  }

  public static filterByCode(programs: EnrolledProgram[], programCd: string): EnrolledProgram[] {
    if (programs != null) {
      const peps = [];
      for (const x of programs) {
        if (x.programCd != null && x.programCd.toLowerCase() === programCd.toLowerCase()) {
          peps.push(x);
        }
      }
      return peps;
    } else {
      return [];
    }
  }

  public static filterByAgencyCode(programs: EnrolledProgram[], agencyCode: string): EnrolledProgram[] {
    if (programs != null) {
      const peps = [];
      for (const x of programs) {
        if (x.agencyCode != null && x.agencyCode.toLowerCase() === agencyCode.toLowerCase()) {
          peps.push(x);
        }
      }
      return peps;
    } else {
      return [];
    }
  }

  public static clone(input: any, instance: EnrolledProgram) {
    instance.id = input.id;
    instance.agencyCode = input.agencyCode;
    instance.agencyName = input.agencyName;
    instance.officeId = input.officeId;
    instance.officeCounty = input.officeCounty;
    instance.officeNumber = input.officeNumber;
    instance.rfaNumber = input.rfaNumber;
    instance.referralDate = input.referralDate;
    instance.enrolledProgramId = input.enrolledProgramId;
    instance.programCd = input.programCd;
    instance.programCode = input.programCode;
    instance.enrollmentDate = input.enrollmentDate;
    instance.disenrollmentDate = input.disenrollmentDate;
    instance.completionReasonId = input.completionReasonId;
    instance.completionReasonDetails = input.completionReasonDetails;
    instance.status = input.status;
    instance.statusDate = input.statusDate;
    instance.subProgramCode = input.subProgramCode;
    instance.isTransfer = input.isTransfer;
    instance.courtOrderedDate = input.courtOrderedDate;
    instance.courtOrderedCounty = input.courtOrderedCounty;
    instance.contractorName = input.contractorName;
    instance.isVoluntary = input.isVoluntary;
    instance.caseNumber = input.caseNumber;
    instance.learnFareFEP = Utilities.deserilizeChild(input.learnFareFEP, LearnFareFEP);
    instance.assignedWorker = Utilities.deserilizeChild(input.assignedWorker, AssignedWorker);
    instance.associatedAgencyCodes = Utilities.deserilizeArray(input.associatedAgencyCodes);
    instance.associatedAgencyNames = Utilities.deserilizeArray(input.associatedAgencyNames);
  }

  public deserialize(input: any) {
    EnrolledProgram.clone(input, this);
    return this;
  }

  get statusDateMmDdYyyy() {
    const mDate = moment(this.statusDate);
    if (mDate.isValid() && this.statusDate !== '') {
      return mDate.format('MM/DD/YYYY');
    } else {
      return '';
    }
  }

  // move to util.
  get referralDateMmDdYyyy() {
    const mDate = moment(this.referralDate);
    if (mDate.isValid() && this.referralDate !== '') {
      return mDate.format('MM/DD/YYYY');
    } else {
      return '';
    }
  }

  get enrollmentDateMmDdYyyy() {
    const mDate = moment(this.enrollmentDate);
    if (mDate.isValid() && this.enrollmentDate !== '') {
      return mDate.format('MM/DD/YYYY');
    } else {
      return '';
    }
  }

  // get canReassignW2(): boolean {
  //     if (this.programCode != null && this.programCode.toLowerCase() === 'w-2'
  //         && this.status != null && this.status.toLowerCase() === 'enrolled') {
  //         return true;
  //     } else {
  //         return false;
  //     }
  // }
  // TODO: clean up when we do coenrollment.
  get canReassign(): boolean {
    if (this.status != null && this.status.toLowerCase() === 'enrolled') {
      return true;
    } else {
      return false;
    }
  }

  /**
   * Returns true for enrolled TJ, W2 and LF when agency code matches pep record.
   *
   * @param {string} userAgencyCode
   * @returns {boolean}
   * @memberof EnrolledProgram
   */
  canTransfer(userAgencyCode: string): boolean {
    // Remember we dont allow TMJ or CF transfers.
    if ((this.status != null && this.status.toLowerCase() !== EnrolledProgramStatus.enrolled) || this.isTmj || this.isCF) {
      return false;
    }

    if (this.status != null && this.status.toLowerCase() === EnrolledProgramStatus.enrolled && userAgencyCode === this.agencyCode) {
      return true;
    } else {
      return false;
    }
  }

  get canEnrollW2(): boolean {
    if (this.programCode != null && this.programCode.toLowerCase() === 'w-2' && this.status != null && this.status.toLowerCase() === 'referred') {
      return true;
    } else {
      return false;
    }
  }

  get canDisenrollW2(): boolean {
    if (this.programCode != null && this.programCode.toLowerCase() === 'w-2' && this.status != null && this.status.toLowerCase() === 'enrolled') {
      return true;
    } else {
      return false;
    }
  }

  get isPending() {
    if (this.status != null && this.status.toLocaleLowerCase() === 'pending') {
      return true;
    } else {
      return false;
    }
  }

  get isCompletionReasonRequiredForDisenrollment() {
    return true;
  }

  get isDisenrollmentDateRequiredForDisenrollment() {
    return true;
  }

  get isCompletionReasonDisplayedForDisenrollment() {
    if (this.isCFTmjTJFcdpProgram) {
      return true;
    } else {
      return false;
    }
  }

  get isDisenrollmentDetailsDisplayedForDisenrollment() {
    if (this.isCF && this.completionReasonId === this.otherCompletionReasonId) {
      return true;
    }
    if (this.isFCDP && this.completionReasonId !== null && this.completionReasonId !== undefined) {
      return true;
    } else {
      return false;
    }
  }

  get isDisenrollmentDetailsRequiredForDisenrollment() {
    if (this.isCF || this.isFCDP) {
      return true;
    } else {
      return false;
    }
  }

  get isDisenrollmentDateDisplayedForDisenrollment() {
    if (this.isCFTmjTJFcdpProgram) {
      return true;
    } else {
      return false;
    }
  }

  get isCFTmjTJProgram(): boolean {
    if (this.programCd === null) {
      return;
    }
    if (this.isCF || this.isTmj || this.isTj) {
      return true;
    } else {
      return false;
    }
  }

  get isCFTmjTJFcdpProgram(): boolean {
    if (this.programCd === null) {
      return;
    }
    if (this.isCF || this.isTmj || this.isTj || this.isFCDP) {
      return true;
    } else {
      return false;
    }
  }

  get isW2(): boolean {
    if (this.programCd != null && Utilities.lowerCaseTrimAsNotNull(this.programCd) === EnrolledProgramCode.w2) {
      return true;
    } else {
      return false;
    }
  }

  get isCF(): boolean {
    if (this.programCd != null && Utilities.lowerCaseTrimAsNotNull(this.programCd) === EnrolledProgramCode.cf) {
      return true;
    } else {
      return false;
    }
  }

  get isTmj(): boolean {
    if (this.programCd != null && Utilities.lowerCaseTrimAsNotNull(this.programCd) === EnrolledProgramCode.tmj) {
      return true;
    } else {
      return false;
    }
  }

  get isTj(): boolean {
    if (this.programCd != null && Utilities.lowerCaseTrimAsNotNull(this.programCd) === EnrolledProgramCode.tj) {
      return true;
    } else {
      return false;
    }
  }

  get isWW(): boolean {
    if (this.programCd != null && Utilities.lowerCaseTrimAsNotNull(this.programCd) === EnrolledProgramCode.ww) {
      return true;
    } else {
      return false;
    }
  }
  get isLF(): boolean {
    if (this.programCd != null && Utilities.lowerCaseTrimAsNotNull(this.programCd) === EnrolledProgramCode.lf) {
      return true;
    } else {
      return false;
    }
  }
  get isFCDP(): boolean {
    if (this.programCd != null && Utilities.lowerCaseTrimAsNotNull(this.programCd) === EnrolledProgramCode.fcdp) {
      return true;
    } else {
      return false;
    }
  }

  get isEnrolled(): boolean {
    if (this.status != null && this.status.toLowerCase() === EnrolledProgramStatus.enrolled) {
      return true;
    } else {
      return false;
    }
  }

  get isReferred(): boolean {
    if (this.status != null && this.status.toLowerCase() === EnrolledProgramStatus.referred) {
      return true;
    } else {
      return false;
    }
  }
}
