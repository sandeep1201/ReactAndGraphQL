// tslint:disable: member-ordering
// tslint:disable: no-use-before-declare
import { Utilities } from '../utilities';
import { Serializable } from '../interfaces/serializable';
import { JobActionType } from './job-actions';
import { EmployerOfRecordDetails } from '../../features-modules/work-history/models/employerofRecordDetails.model';
import { EnrolledProgram } from './enrolled-program.model';
import { GoogleLocation } from './google-location';
import { ValidationCode } from './validation-error';
import { ValidationManager } from './validation-manager';
import { ValidationResult } from './validation-result';
import { TextDetail, CalculatedString, Payload, Trivalue } from './primitives';
import { Validate } from '../validate';
import * as moment from 'moment';
import * as _ from 'lodash';
import { DropDownField } from './dropdown-field';
import { JobTypeName } from 'src/app/shared/enums/job-type-name.enum';
import { EnrolledProgramStatus } from '../enums/enrolled-program-status.enum';

export class WorkHistoryIdentities {
  // Job Types
  tempCustodialParentUnsubsidizedId: number;
  tempCustodialParentSubsidizedId: number;

  tempNonCustodialParentUnsubsidizedId: number;
  tempNonCustodialParentSubsidizedId: number;
  tmjUnsubsidizedId: number;
  tjUnsubsidizedId: number;
  tjSubsidizedId: number;
  tmjSubsidizedId: number;
  unSubsidizedId: number;
  staffingAgencyId: number;
  selfEmployedId: number;

  volunteerJobTypeId: number;
  jobFoundWorkerAssistedId: number;
  jobFoundOtherWorkProgramId: number;
  otherPayTypeId: number;
  noPayPayTypeId: number;

  // Leaving Reason DropDown.
  leavingReasonFiredId: number;
  leavingReasonPermanentlyLaidOffId: number;
  leavingReasonQuitId: number;
  leavingReasonOtherId: number;
  employerOfRecordTypes: any;
  otherWorkProgramId: number;
}

export class Employment implements Serializable<Employment> {
  rowVersion: string;
  modifiedBy: string;
  modifiedDate: string;
  modifiedByName: string;
  id: number;
  jobTypeId: number;
  employmentProgramTypeName: string;
  jobTypeName: string;
  jobBeginDate: string;
  jobEndDate: string;
  isCurrentlyEmployed: boolean;
  isConverted: boolean;
  isVerified: boolean;
  jobPosition: string;
  companyName: string;
  fein: string;
  location: GoogleLocation;
  contactId: number;
  jobDuties: TextDetail[];
  jobSectorId: number;
  jobSectorName: string;
  workerId: string;
  workProgramId: number;
  expectedScheduleDetails: string;
  jobAction: JobActionType;
  jobFoundMethodId: number;
  jobFoundMethodName: string;
  jobFoundMethodDetails: string;
  leavingReasonId: number;
  leavingReasonName: string;
  leavingReasonDetails: string;
  wageHour: WageHour;
  absences: Absence[];

  totalSubsidizedHours: number;
  notes: string;
  deleteReasonId: number;
  deleteReasonName: string;
  employerOfRecordId: number;
  employerOfRecordDetails: EmployerOfRecordDetails;
  isCurrentJobAtCreation: boolean;

  // Front end only Properties
  jobDateDuration: string;
  showDel = true;
  hasEmploymentOnEp: boolean;
  isSelected = false;
  streetAddress: string;
  zipAddress: string;
  isEndDateValid = true;

  static readonly maximumLifeTimeSubsidizedHoursAllowed = 1040;

  public static clone(input: any, instance: Employment) {
    instance.rowVersion = input.rowVersion;
    instance.modifiedBy = input.modifiedBy;
    instance.modifiedByName = input.modifiedByName;
    instance.modifiedDate = input.modifiedDate;
    instance.id = input.id;
    instance.jobTypeId = input.jobTypeId;
    instance.employmentProgramTypeName = input.employmentProgramTypeName;
    instance.jobTypeName = input.jobTypeName;
    instance.jobBeginDate = input.jobBeginDate;
    instance.jobEndDate = input.jobEndDate;
    instance.isCurrentlyEmployed = input.isCurrentlyEmployed;
    instance.isConverted = input.isConverted;
    instance.isVerified = input.isVerified;
    instance.jobPosition = input.jobPosition;
    instance.companyName = input.companyName;
    instance.fein = input.fein;
    instance.location = new GoogleLocation().deserialize(input.location);
    instance.contactId = input.contactId;
    instance.jobDuties = Utilities.deserilizeChildren(input.jobDuties, TextDetail);
    instance.jobSectorId = input.jobSectorId;
    instance.jobSectorName = input.jobSectorName;
    instance.workerId = input.workerId;
    instance.workProgramId = input.workProgramId;
    instance.expectedScheduleDetails = input.expectedScheduleDetails;
    instance.jobAction = new JobActionType().deserialize(input.jobAction);
    instance.jobFoundMethodId = input.jobFoundMethodId;
    instance.jobFoundMethodName = input.jobFoundMethodName;
    instance.jobFoundMethodDetails = input.jobFoundMethodDetails;
    instance.leavingReasonId = input.leavingReasonId;
    instance.leavingReasonName = input.leavingReasonName;
    instance.leavingReasonDetails = input.leavingReasonDetails;
    instance.wageHour = new WageHour().deserialize(input.wageHour);
    instance.absences = Utilities.deserilizeChildren(input.absences, Absence, 0);
    instance.totalSubsidizedHours = input.totalSubsidizedHours;
    instance.notes = input.notes;
    instance.employerOfRecordId = input.employerOfRecordId;
    instance.deleteReasonId = input.deleteReasonId;
    instance.deleteReasonName = input.deleteReasonName;
    if (input.employerOfRecordDetails !== null) {
      instance.employerOfRecordDetails = new EmployerOfRecordDetails().deserialize(input.employerOfRecordDetails);
    }
    instance.isCurrentJobAtCreation = input.isCurrentJobAtCreation;
    instance.hasEmploymentOnEp = input.hasEmploymentOnEp;
    instance.isSelected = input.isSelected;
    instance.streetAddress = input.streetAddress;
    instance.zipAddress = input.zipAddress;
    return instance;
  }

  public deserialize(input: any) {
    Employment.clone(input, this);
    return this;
  }

  get allEffectiveDates(): Payload {
    const dates = new Payload();
    if (this.wageHour != null && this.wageHour.wageHourHistories != null) {
      for (const wwh of this.wageHour.wageHourHistories) {
        const tv: Trivalue = { first: wwh.effectiveDate, second: false, third: null };
        dates.trivalue.push(tv);
        dates.stringArray.push(wwh.effectiveDate);
      }
    }
    if (this.wageHour != null) {
      const tv: Trivalue = { first: this.wageHour.currentEffectiveDate, second: true, third: null };
      dates.trivalue.push(tv);
      dates.stringArray.push(this.wageHour.currentEffectiveDate);
    }
    return dates;
  }
  public isEmployerOfRecordRequired(tempCustodialParentSubsidizedId: number, tempNonCustodialParentSubsidizedId: number, tmjSubsidizedId: number, tjSubsidizedId: number) {
    if (
      Number(this.jobTypeId) === tempCustodialParentSubsidizedId ||
      Number(this.jobTypeId) === tempNonCustodialParentSubsidizedId ||
      Number(this.jobTypeId) === tmjSubsidizedId ||
      Number(this.jobTypeId) === tjSubsidizedId
    ) {
      return true;
    } else {
      return false;
    }
  }
  public isFEINRequired(tjSubsidizedId: number, tmjSubsidizedId: number): boolean {
    return Number(this.jobTypeId) === tjSubsidizedId || Number(this.jobTypeId) === tmjSubsidizedId;
  }
  public isAddressRequired(
    tempCustodialParentUnsubsidizedId: number,
    tempNonCustodialParentUnsubsidizedId: number,
    tmjUnsubsidizedId: number,
    tjUnsubsidizedId: number,
    tempCustodialParentSubsidizedId: number,
    tempNonCustodialParentSubsidizedId: number,
    tmjSubsidizedId: number,
    tjSubsidizedId: number
  ): boolean {
    if (
      Number(this.jobTypeId) === tempCustodialParentUnsubsidizedId ||
      Number(this.jobTypeId) === tempNonCustodialParentUnsubsidizedId ||
      Number(this.jobTypeId) === tmjUnsubsidizedId ||
      Number(this.jobTypeId) === tjUnsubsidizedId ||
      Number(this.jobTypeId) === tempCustodialParentSubsidizedId ||
      Number(this.jobTypeId) === tempNonCustodialParentSubsidizedId ||
      Number(this.jobTypeId) === tmjSubsidizedId ||
      Number(this.jobTypeId) === tjSubsidizedId
    ) {
      return true;
    } else {
      return false;
    }
  }
  public isZipCodeRequired(
    tempCustodialParentUnsubsidizedId: number,
    tempNonCustodialParentUnsubsidizedId: number,
    tmjUnsubsidizedId: number,
    tjUnsubsidizedId: number,
    tempCustodialParentSubsidizedId: number,
    tempNonCustodialParentSubsidizedId: number,
    tmjSubsidizedId: number,
    tjSubsidizedId: number
  ): boolean {
    if (
      Number(this.jobTypeId) === tempCustodialParentUnsubsidizedId ||
      Number(this.jobTypeId) === tempNonCustodialParentUnsubsidizedId ||
      Number(this.jobTypeId) === tmjUnsubsidizedId ||
      Number(this.jobTypeId) === tjUnsubsidizedId ||
      Number(this.jobTypeId) === tempCustodialParentSubsidizedId ||
      Number(this.jobTypeId) === tempNonCustodialParentSubsidizedId ||
      Number(this.jobTypeId) === tmjSubsidizedId ||
      Number(this.jobTypeId) === tjSubsidizedId
    ) {
      return true;
    } else {
      return false;
    }
  }

  public isPrivatePublicRequired(
    tempCustodialParentUnsubsidizedId: number,
    tempNonCustodialParentUnsubsidizedId: number,
    tmjUnsubsidizedId: number,
    tjUnsubsidizedId: number,
    tempCustodialParentSubsidizedId: number,
    tempNonCustodialParentSubsidizedId: number,
    tmjSubsidizedId: number,
    tjSubsidizedId: number
  ): boolean {
    if (
      Number(this.jobTypeId) === tempCustodialParentUnsubsidizedId ||
      Number(this.jobTypeId) === tempNonCustodialParentUnsubsidizedId ||
      Number(this.jobTypeId) === tmjUnsubsidizedId ||
      Number(this.jobTypeId) === tjUnsubsidizedId ||
      Number(this.jobTypeId) === tempCustodialParentSubsidizedId ||
      Number(this.jobTypeId) === tempNonCustodialParentSubsidizedId ||
      Number(this.jobTypeId) === tmjSubsidizedId ||
      Number(this.jobTypeId) === tjSubsidizedId
    ) {
      return true;
    } else {
      return false;
    }
  }

  // public isLocatedInTMIAreaRequired(tmjUnsubsidizedId: number, tmjSubsidizedId: number): boolean {
  //   if (Number(this.jobTypeId) === tmjUnsubsidizedId || Number(this.jobTypeId) === tmjSubsidizedId) {
  //     return true;
  //   } else {
  //     return false;
  //   }
  // }

  public isWorkerIdRequired(jobFoundWorkerAssistedId: number, jobStatus: string): boolean {
    if (Number(this.jobFoundMethodId) === jobFoundWorkerAssistedId && jobStatus === 'currentJob') {
      return true;
    } else {
      return false;
    }
  }

  public isPayRateDisabledAndNotRequired(volunteerId: number): boolean {
    if (Number(this.jobTypeId) === volunteerId) {
      return true;
    } else {
      return false;
    }
  }

  public isJobFoundDetailsRequired(otherWorkProgramId: number, jobFoundOtherWorkProgramId): boolean {
    if (Number(this.workProgramId) === otherWorkProgramId && Number(this.jobFoundMethodId === jobFoundOtherWorkProgramId)) {
      return true;
    } else {
      return false;
    }
  }

  public isLeavingReasonsRequired(jobStatus: string): boolean {
    if (this.jobEndDate != null && this.jobEndDate.trim() !== '' && jobStatus === 'currentJob' && this.isCurrentlyEmployed !== true) {
      return true;
    } else if (jobStatus === 'pastJob') {
      return true;
    } else {
      return false;
    }
  }

  public isLeavingReasonsDetailsRequired(
    leavingReasonFiredId: number,
    leavingReasonPermanentlyLaidOffId: number,
    leavingReasonQuitId: number,
    leavingReasonOtherId: number
  ): boolean {
    if (
      Number(this.leavingReasonId) === leavingReasonFiredId ||
      Number(this.leavingReasonId) === leavingReasonPermanentlyLaidOffId ||
      Number(this.leavingReasonId) === leavingReasonQuitId ||
      Number(this.leavingReasonId) === leavingReasonOtherId
    ) {
      return true;
    } else {
      return false;
    }
  }

  public isBeginningPayDisabled(volunteerId: number): boolean {
    if (Number(this.jobTypeId) === volunteerId) {
      return true;
    } else {
      return false;
    }
  }

  public isEndingPayDisabled(volunteerId: number): boolean {
    if (Number(this.jobTypeId) === volunteerId) {
      return true;
    } else {
      return false;
    }
  }

  public isCurrentJob() {
    if (this.isCurrentlyEmployed === true) {
      return true;
    }
  }
  public isPastJob(endDateM) {
    if (endDateM != null && (this.isCurrentlyEmployed === false || this.isCurrentlyEmployed == null)) {
      return true;
    }
  }

  public whichJobCategory(): string {
    if (this.isCurrentJob()) {
      return 'currentJob';
    } else if (this.isPastJob(this.jobEndDate)) {
      return 'pastJob';
    } else {
      return null;
    }
  }

  public static canAddAndShowTMJTJSubsidizedHours(emp, participant, appService, hasEditAuth) {
    // checkReadOnlyAccess will check for enrolled and disenrolled program and also the most recent enrolled program user has access to.
    if (emp.length) {
      return (
        (appService.isStateStaff || !appService.checkReadOnlyAccess(participant, [EnrolledProgramStatus.enrolled, EnrolledProgramStatus.disenrolled], hasEditAuth)) &&
        emp.some(e => e.jobTypeName === JobTypeName.tmjSubsidised || e.jobTypeName === JobTypeName.tjSubsidised)
      );
    } else {
      return (
        (appService.isStateStaff || !appService.checkReadOnlyAccess(participant, [EnrolledProgramStatus.enrolled, EnrolledProgramStatus.disenrolled], hasEditAuth)) &&
        (emp.jobTypeName === JobTypeName.tmjSubsidised || emp.jobTypeName === JobTypeName.tjSubsidised) &&
        emp.deleteReasonId === null
      );
    }
  }
  public validateBeginDateForJobType(programs, validationManager, result, jobtype, programType) {
    programs.forEach(program => {
      {
        const enrollDate = moment(new Date(program.enrollmentDate).toISOString());
        const disenrollDate = program.disenrollmentDate ? moment(new Date(program.disenrollmentDate).toISOString()) : program.disenrollmentDate;
        const inputDate = moment(new Date(this.jobBeginDate).toISOString());

        if (inputDate.isBefore(enrollDate)) {
          validationManager.addErrorWithFormat(ValidationCode.DateBeforeEnrollmentDate, jobtype, programType, moment(enrollDate, moment.ISO_8601).format('MM/DD/YYYY'));
          result.addError('jobBeginDate');
        }
        if (disenrollDate) {
          if (inputDate.isAfter(disenrollDate)) {
            validationManager.addErrorWithFormat(ValidationCode.DateBeforeDisenrollmentDate, jobtype, programType, moment(disenrollDate, moment.ISO_8601).format('MM/DD/YYYY'));
            result.addError('jobBeginDate');
          }
        }
      }
    });
  }

  public validateEndDateForJobType(programs, validationManager, result, jobtype, programType) {
    programs.forEach(program => {
      const disenrollmentDate = program.disenrollmentDate ? moment(new Date(program.disenrollmentDate).toISOString()) : program.disenrollmentDate;
      const inputDate = moment(new Date(this.jobEndDate).toISOString());
      if (disenrollmentDate) {
        if (inputDate.isAfter(disenrollmentDate)) {
          validationManager.addErrorWithFormat(ValidationCode.DateAfterDisenrollmentDate, jobtype, programType, moment(disenrollmentDate, moment.ISO_8601).format('MM/DD/YYYY'));
          result.addError('jobEndDate');
        }
      }
    });
  }

  public validateEndDate(result, validationManager, modelIds, allPrograms, participantDOB) {
    // End Date is a required field.
    if ((this.jobEndDate == null || this.jobEndDate.trim() === '') && this.isCurrentlyEmployed !== true) {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'End Date');
      result.addError('jobEndDate');
      this.isEndDateValid = false;
    } else if (this.isCurrentlyEmployed !== true) {
      const inputDate = moment(this.jobEndDate, 'MM/DD/YYYY', true);
      //  End Date must be 8 digits in two digit month, two digit day, and four digit year format. (MM/DD/YYYY).
      if (this.jobEndDate.length !== 10) {
        validationManager.addErrorWithFormat(ValidationCode.ValueInInvalidFormat_Name_FormatType, 'End Date', 'MM/DD/YYYY');
        result.addError('jobEndDate');
        this.isEndDateValid = false;
      } else if (inputDate.isValid()) {
        // Check Min Date.
        if (inputDate < participantDOB) {
          validationManager.addErrorWithFormat(ValidationCode.ValueBeforeDOB_Name_Value_DOB, 'End Date', inputDate.format('MM/DD/YYYY'), participantDOB.format('MM/DD/YYYY'));
          result.addError('jobEndDate');
          this.isEndDateValid = false;
        }
        // End Date validation Based on JobType
        if (this.jobTypeId === modelIds.tjSubsidizedId) {
          const tjEnrolledprograms = allPrograms.filter(program => program.isTj);
          if (tjEnrolledprograms == null) {
            validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, 'Participant never enrolled in TJ program');
            result.addError('jobEndDate');
            this.isEndDateValid = false;
          } else {
            this.validateEndDateForJobType(tjEnrolledprograms, validationManager, result, 'TJ (Subsidized)', 'Tj');
          }
        }

        if (this.jobTypeId === modelIds.tmjSubsidizedId) {
          const tmjEnrolledprograms = allPrograms.filter(program => program.isTmj);
          if (tmjEnrolledprograms == null) {
            validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, 'Participant never enrolled in TMJ program');
            result.addError('jobEndDate');
            this.isEndDateValid = false;
          } else {
            this.validateEndDateForJobType(tmjEnrolledprograms, validationManager, result, 'TMJ (Subsidized)', 'TMJ');
          }
        }
        if (this.jobTypeId === modelIds.tempCustodialParentSubsidizedId) {
          const W2Enrolledprograms = allPrograms.filter(program => program.isWW);
          if (W2Enrolledprograms == null) {
            validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, 'Participant never enrolled in W2 program');
            result.addError('jobEndDate');
            this.isEndDateValid = false;
          } else {
            this.validateEndDateForJobType(W2Enrolledprograms, validationManager, result, 'TEMP Custodial Parent (Subsidized)', 'W-2');
          }
        }
        if (this.jobTypeId === modelIds.tempNonCustodialParentSubsidizedId) {
          const W2Enrolledprograms = allPrograms.filter(program => program.isWW);
          if (W2Enrolledprograms == null) {
            validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, 'Participant never enrolled in W2 program');
            result.addError('jobEndDate');
            this.isEndDateValid = false;
          } else {
            this.validateEndDateForJobType(W2Enrolledprograms, validationManager, result, 'TEMP Non-Custodial Parent (Subsidized)', 'W-2');
          }
        }

        let isEndDateBeforeEffective = false;
        if (this.allEffectiveDates != null && this.allEffectiveDates.stringArray != null && (this.whichJobCategory() !== 'pastJob' || this.isCurrentJobAtCreation)) {
          for (const eDate of this.allEffectiveDates.stringArray) {
            if (inputDate < moment(eDate, 'MM/DD/YYYY')) {
              isEndDateBeforeEffective = true;
            }
          }
        }

        if (isEndDateBeforeEffective) {
          validationManager.addErrorWithFormat(ValidationCode.DateBeforeDate_Name1_Name2, 'End Date', 'Effective Date');
          result.addError('jobEndDate');
          this.isEndDateValid = false;
        }

        const currentDate = Utilities.currentDate;
        // Must be a past or current date.
        if (inputDate.isAfter(currentDate)) {
          validationManager.addErrorWithFormat(ValidationCode.DateAfterCurrent, 'End Date', inputDate.format('MM/DD/YYYY'));
          result.addError('jobEndDate');
          this.isEndDateValid = false;
        }
      } else {
        validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, 'Invalid End Date Entered');
        result.addError('jobEndDate');
        this.isEndDateValid = false;
      }
    }
  }

  public validateJustEndDate(result: ValidationResult, validationManager: ValidationManager) {
    const currentDate = Utilities.currentDate;
    const inputDate = moment(this.jobEndDate, 'MM/DD/YYYY', true);
    const jobBeginDate = moment(this.jobBeginDate, 'MM/DD/YYYY');

    if ((this.jobEndDate == null || this.jobEndDate.trim() === '') && this.isCurrentlyEmployed !== true) {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'End Date');
      result.addError('jobEndDate');
      this.isEndDateValid = false;
    } else if (this.isCurrentlyEmployed !== true && this.jobEndDate !== null && this.jobEndDate.length !== 10) {
      validationManager.addErrorWithFormat(ValidationCode.ValueInInvalidFormat_Name_FormatType, 'End Date', 'MM/DD/YYYY');
      result.addError('jobEndDate');
      this.isEndDateValid = false;
    }
    // Must be a past or current date.
    else if (inputDate.isValid()) {
      if (inputDate >= currentDate) {
        validationManager.addErrorWithFormat(ValidationCode.DateAfterCurrent, 'End Date', inputDate.format('MM/DD/YYYY'));
        result.addError('jobEndDate');
        this.isEndDateValid = false;
      }

      if (jobBeginDate > inputDate) {
        validationManager.addErrorWithFormat(ValidationCode.DateBeforeDate_Name1_Name2, 'End Date', 'Begin Date');
        result.addError('jobEndDate');
      }
    } else {
      validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, 'Invalid End Date Entered');
      result.addError('jobEndDate');
      this.isEndDateValid = false;
    }

    return result;
  }

  // TODO: Pass a interface for Ids;
  public validate(
    validationManager: ValidationManager,
    participantDOB: moment.Moment,
    enrollmentDate: string,
    disenrollmentDate: string,
    modelIds: WorkHistoryIdentities,
    allPrograms: EnrolledProgram[],
    originalModel?: Employment,
    employerOfRecordSelectedValue?: string,
    jobFoundOtherWorkProgramId?: number,
    maxHourlySubsidy?: string
  ): ValidationResult {
    const result = new ValidationResult();

    // Job Type is a required field.
    if (this.jobTypeId == null || this.jobTypeId.toString() === '') {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Job Type');
      result.addError('jobTypeId');
    } else if (this.jobTypeId && this.isVerified && this.jobTypeId !== originalModel.jobTypeId) {
      validationManager.addErrorWithDetail(
        ValidationCode.DuplicateDataWithNoMessage,
        'This job is verified through TJ/TMJ 60 Day Employment Verification. The job type cannot be changed.'
      );
      result.addError('jobTypeId');
    }
    // we already have a check for enrollment the user cannot select tjSubsidised if the job being edited is tmjSubsidised because we check if the user is enrolled in tj program and there is no way the user can co-enroll in tj and tmj
    if (
      this.totalSubsidizedHours > 0 &&
      (originalModel.jobTypeName === JobTypeName.tjSubsidised || originalModel.jobTypeName === JobTypeName.tmjSubsidised) &&
      !(this.jobTypeId === modelIds.tjSubsidizedId || this.jobTypeId === modelIds.tmjSubsidizedId)
    ) {
      validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, 'This job type already has weekly hours worked added. A job type change cannot be made.');
      result.addError('jobTypeId');
    }
    let isBeginDateValid = true;

    // Begin Date is a required field.
    if (this.jobBeginDate == null || this.jobBeginDate.trim() === '') {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Begin Date');
      result.addError('jobBeginDate');
      isBeginDateValid = false;
    } else {
      const inputDate = moment(this.jobBeginDate, 'MM/DD/YYYY', true);
      //  End Date must be 8 digits in two digit month, two digit day, and four digit year format. (MM/DD/YYYY).
      if (this.jobBeginDate.length !== 10) {
        validationManager.addErrorWithFormat(ValidationCode.ValueInInvalidFormat_Name_FormatType, 'Begin Date', 'MM/DD/YYYY');
        result.addError('jobBeginDate');
        isBeginDateValid = false;
      } else if (inputDate.isValid()) {
        // Check Min Date.
        if (inputDate < participantDOB) {
          validationManager.addErrorWithFormat(ValidationCode.ValueBeforeDOB_Name_Value_DOB, 'Begin Date', inputDate.format('MM/DD/YYYY'), participantDOB.format('MM/DD/YYYY'));
          result.addError('jobBeginDate');
          isBeginDateValid = false;
        }
        const currentDatePlus10Days = Utilities.currentDate.add(10, 'days');
        // Must be a past date or up to 10 days in the future.
        if (inputDate > currentDatePlus10Days) {
          validationManager.addErrorWithFormat(ValidationCode.DateAfterDuration, 'Begin Date', inputDate.format('MM/DD/YYYY'), '10 days');
          result.addError('jobBeginDate');
          isBeginDateValid = false;
        }
        // Begin Date validation Based on JobType
        if (this.jobTypeId === modelIds.tjSubsidizedId) {
          const tjEnrolledprograms = allPrograms.filter(program => program.isTj);
          if (tjEnrolledprograms == null || tjEnrolledprograms.length === 0) {
            validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, 'Participant was not enrolled in the program of the selected job type');
            result.addError('jobTypeId');
            result.isValid = false;
          } else {
            this.validateBeginDateForJobType(tjEnrolledprograms, validationManager, result, 'TJ (Subsidized)', 'TJ');
          }
        }
        if (this.jobTypeId === modelIds.tmjSubsidizedId) {
          const tmjEnrolledprograms = allPrograms.filter(program => program.isTmj);
          if (tmjEnrolledprograms == null || tmjEnrolledprograms.length === 0) {
            validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, 'Participant was not enrolled in the program of the selected job type');
            result.addError('jobTypeId');
            result.isValid = false;
          } else {
            this.validateBeginDateForJobType(tmjEnrolledprograms, validationManager, result, 'TMJ (Subsidized)', 'TMJ');
          }
        }
        if (this.jobTypeId === modelIds.tempCustodialParentSubsidizedId) {
          const W2Enrolledprograms = allPrograms.filter(program => program.isWW);
          if (W2Enrolledprograms == null || W2Enrolledprograms.length === 0) {
            validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, 'Participant was not enrolled in the program of the selected job type');
            result.addError('jobTypeId');
            result.isValid = false;
          } else {
            this.validateBeginDateForJobType(W2Enrolledprograms, validationManager, result, 'TEMP Custodial Parent (Subsidized)', 'W-2');
          }
        }
        if (this.jobTypeId === modelIds.tempNonCustodialParentSubsidizedId) {
          const W2Enrolledprograms = allPrograms.filter(program => program.isWW);
          if (W2Enrolledprograms == null || W2Enrolledprograms.length === 0) {
            validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, 'Participant was not enrolled in the program of the selected job type');
            result.addError('jobTypeId');
            result.isValid = false;
          } else {
            this.validateBeginDateForJobType(W2Enrolledprograms, validationManager, result, 'TEMP Non-Custodial Parent (Subsidized)', 'W-2');
          }
        }
      } else {
        validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, 'Invalid Begin Date Entered');
        result.addError('jobBeginDate');
        isBeginDateValid = false;
      }
    }
    this.validateEndDate(result, validationManager, modelIds, allPrograms, participantDOB);

    if (isBeginDateValid && this.isEndDateValid && this.isCurrentlyEmployed !== true) {
      // If both begin and end dates are invalid.
      const jobBeginDate = moment(this.jobBeginDate, 'MM/DD/YYYY');
      const jobEndDate = moment(this.jobEndDate, 'MM/DD/YYYY');
      if (jobBeginDate > jobEndDate) {
        validationManager.addErrorWithFormat(ValidationCode.DateBeforeDate_Name1_Name2, 'End Date', 'Begin Date');
        result.addError('jobBeginDate');
        result.addError('jobEndDate');
      }
    }
    if (this.isEmployerOfRecordRequired(modelIds.tempCustodialParentSubsidizedId, modelIds.tempNonCustodialParentSubsidizedId, modelIds.tmjSubsidizedId, modelIds.tjSubsidizedId)) {
      if (this.employerOfRecordId == null) {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Employer Of Record');
        result.addError('emoployerOfRecordId');
      }
      if (this.whichJobCategory() === 'currentJob') {
        if (this.wageHour.currentHourlySubsidyRate == null || this.wageHour.currentHourlySubsidyRate.trim() === '') {
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Wage/Hour - Current Hourly Subsidy');
          result.addError('currentHourlySubsidyRate');
        } else if (+this.wageHour.currentHourlySubsidyRate > +maxHourlySubsidy) {
          validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, `Hourly subsidy can't exceed $${maxHourlySubsidy}`);
          result.addError('currentHourlySubsidyRate');
        }
      }
    }
    if (Number(this.employerOfRecordId) === Utilities.idByFieldDataName('Other', modelIds.employerOfRecordTypes)) {
      const me = result.addErrorContainingObject('employerOfRecordDetails');
      if (this.employerOfRecordDetails.companyName == null || this.employerOfRecordDetails.companyName.trim() === '') {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Employer Of Record Details - Company/Organization Name');
        result.addErrorForContainingObject(me, 'companyName');
      }
      if (this.isFEINRequired(modelIds.tjSubsidizedId, modelIds.tmjSubsidizedId) && Utilities.stringIsNullOrWhiteSpace(this.employerOfRecordDetails.fein)) {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Employer Of Record Details - FEIN');
        result.addErrorForContainingObject(me, 'fein');
      }
      if (
        this.employerOfRecordDetails.location == null ||
        ((this.employerOfRecordDetails.location.googlePlaceId == null || this.employerOfRecordDetails.location.googlePlaceId === '') &&
          (this.employerOfRecordDetails.location.city == null || this.employerOfRecordDetails.location.city.trim() === ''))
      ) {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Employer Of Record Details - Location');
        result.addErrorForContainingObject(me, 'location');
      }
      if (
        this.employerOfRecordDetails.location.fullAddress == null ||
        this.employerOfRecordDetails.location.fullAddress == null ||
        this.employerOfRecordDetails.location.fullAddress.trim() === ''
      ) {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Employer Of Record Details - Street Address');
        result.addErrorForContainingObject(me, 'streetAddress');
      }
      if (
        this.employerOfRecordDetails.location == null ||
        this.employerOfRecordDetails.location.zipAddress == null ||
        this.employerOfRecordDetails.location.zipAddress.trim() === ''
      ) {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Employer Of Record Details - Zip');
        result.addErrorForContainingObject(me, 'zipAddress');
      }
      if (this.employerOfRecordDetails.jobSectorId == null) {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Employer Of Record Details - Private / Public');
        result.addErrorForContainingObject(me, 'jobSectorId');
      }
    }

    // Job Position is a required field.
    if (this.jobPosition == null || this.jobPosition.trim() === '') {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Job Position');
      result.addError('jobPosition');
    }

    // Company/Organization Name is a required field.
    if (this.companyName == null || this.companyName.trim() === '') {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Company/Organization Name');
      result.addError('companyName');
    }
    if (
      Number(this.employerOfRecordId) !== Utilities.idByFieldDataName('Other', modelIds.employerOfRecordTypes) &&
      this.isFEINRequired(modelIds.tjSubsidizedId, modelIds.tmjSubsidizedId) &&
      Utilities.stringIsNullOrWhiteSpace(this.fein)
    ) {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'FEIN');
      result.addError('fein');
    }

    //  Location is a required field.  Either it is empty, or the google place ID is empty and the city is empty.
    Validate.googleLocation(this.location, 'location', 'Location', result, validationManager);

    if (
      this.isAddressRequired(
        modelIds.tempCustodialParentUnsubsidizedId,
        modelIds.tempNonCustodialParentUnsubsidizedId,
        modelIds.tmjUnsubsidizedId,
        modelIds.tjUnsubsidizedId,
        modelIds.tempCustodialParentSubsidizedId,
        modelIds.tempNonCustodialParentSubsidizedId,
        modelIds.tmjSubsidizedId,
        modelIds.tjSubsidizedId
      )
    ) {
      if (this.location.fullAddress == null || this.location.fullAddress == null || this.location.fullAddress.trim() === '') {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Street Address');
        result.addError('streetAddress');
      }
    }

    if (
      this.isZipCodeRequired(
        modelIds.tempCustodialParentUnsubsidizedId,
        modelIds.tempNonCustodialParentUnsubsidizedId,
        modelIds.tmjUnsubsidizedId,
        modelIds.tjUnsubsidizedId,
        modelIds.tempCustodialParentSubsidizedId,
        modelIds.tempNonCustodialParentSubsidizedId,
        modelIds.tmjSubsidizedId,
        modelIds.tjSubsidizedId
      )
    ) {
      if (this.location == null || this.location.zipAddress == null || this.location.zipAddress.trim() === '') {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Zip');
        result.addError('zipAddress');
      }
    }

    if (this.jobDuties != null) {
      let isJobDutiesEmpty = true;
      for (const jd of this.jobDuties) {
        if (jd.details != null && jd.details.trim() !== '') {
          isJobDutiesEmpty = false;
          break;
        }
      }
      if (isJobDutiesEmpty) {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Job Duties');
        result.addError('jobDuties');
      }
    }

    if (
      this.isPrivatePublicRequired(
        modelIds.tempCustodialParentUnsubsidizedId,
        modelIds.tempNonCustodialParentUnsubsidizedId,
        modelIds.tmjUnsubsidizedId,
        modelIds.tjUnsubsidizedId,
        modelIds.tempCustodialParentSubsidizedId,
        modelIds.tempNonCustodialParentSubsidizedId,
        modelIds.tmjSubsidizedId,
        modelIds.tjSubsidizedId
      )
    ) {
      if (this.jobSectorId == null) {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Private/Public');
        result.addError('jobSectorId');
      }
    }

    // if (this.isLocatedInTMIAreaRequired(modelIds.tmjUnsubsidizedId, modelIds.tmjSubsidizedId)) {
    //   if (this.isLocatedTmiArea == null) {
    //     validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Located in TMI Area?');
    //     result.addError('isLocatedTmiArea');
    //   }
    // }
    //  Benefits Offered is required for In-Program Jobs.
    if (this.whichJobCategory() === 'currentJob') {
      if (this.jobAction == null || this.jobAction.jobActionTypes == null || this.jobAction.jobActionTypes.length === 0) {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Benefits Offered');
        result.addError('jobAction');
      }
    }

    //  "Details" is a required field only when "Other Work Program" is selected.
    if (this.isJobFoundDetailsRequired(modelIds.otherWorkProgramId, jobFoundOtherWorkProgramId)) {
      if (this.jobFoundMethodDetails == null || this.jobFoundMethodDetails.trim() === '') {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Details');
        result.addError('jobFoundMethodDetails');
      }
    }

    if (this.jobFoundMethodId === jobFoundOtherWorkProgramId && this.workProgramId == null) {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Work Program');
      result.addError('workProgramId');
    }

    // How was this job found?" is displayed and required for Currentjobs and employerOfRecordSelectedValue is differnt from 'Other'.
    if (this.whichJobCategory() === 'currentJob' && employerOfRecordSelectedValue !== 'Other') {
      if (this.jobFoundMethodId == null || this.jobFoundMethodId.toString() === '') {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'How was this job found?');
        result.addError('jobFoundMethodId');
      }
    }

    if (this.isWorkerIdRequired(modelIds.jobFoundWorkerAssistedId, this.whichJobCategory()) && employerOfRecordSelectedValue !== 'Other') {
      if (this.workerId == null || this.workerId.trim() === '') {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Worker ID');
        result.addError('workerId');
      }
    }

    // Reason for Leaving is a required field for past jobs.
    if (this.isLeavingReasonsRequired(this.whichJobCategory())) {
      if (this.leavingReasonId == null || this.leavingReasonId.toString() === '') {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Reason for Leaving');
        result.addError('leavingReasonId');
      }

      if (
        this.isLeavingReasonsDetailsRequired(modelIds.leavingReasonFiredId, modelIds.leavingReasonPermanentlyLaidOffId, modelIds.leavingReasonQuitId, modelIds.leavingReasonOtherId)
      ) {
        if (this.leavingReasonDetails == null || this.leavingReasonDetails.trim() === '') {
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Leaving Reason Details');
          result.addError('leavingReasonDetails');
        }
      }
    }

    if (this.whichJobCategory() === 'currentJob') {
      this.wageHour.validate(
        validationManager,
        participantDOB,
        this.jobBeginDate,
        this.jobEndDate,
        'currentJob',
        this.isCurrentlyEmployed,
        this.jobTypeId,
        this.allEffectiveDates,
        modelIds,
        this.isCurrentJobAtCreation,
        result
      );
    }

    if (this.whichJobCategory() === 'pastJob') {
      this.wageHour.validate(
        validationManager,
        participantDOB,
        this.jobBeginDate,
        this.jobEndDate,
        'pastJob',
        this.isCurrentlyEmployed,
        this.jobTypeId,
        this.allEffectiveDates,
        modelIds,
        this.isCurrentJobAtCreation,
        result
      );
    }

    if (this.whichJobCategory() === 'currentJob') {
      // Need to check every item in repeater and see if any are open.
      for (const wwh of this.wageHour.wageHourHistories) {
        if (wwh.isOpen) {
          validationManager.addError(ValidationCode.HistoryTableConfirm);
          result.addError('wageHourHistories');
        }
      }

      for (const ab of this.absences) {
        if (ab.isOpen) {
          validationManager.addError(ValidationCode.HistoryTableConfirm);
          result.addError('absences');
        }
      }
    }
    return result;
  }
}

export class WageHour implements Serializable<WageHour> {
  currentEffectiveDate: string;
  currentPayType: JobActionType;
  currentPayTypeDetails: string;
  currentAverageWeeklyHours: string;
  currentPayRate: string;
  currentPayRateIntervalId: number;
  currentPayRateIntervalName: string;
  pastBeginPayRate: string;
  pastBeginPayRateIntervalId: number;
  pastBeginPayRateIntervalName: string;
  pastEndPayRate: string;
  pastEndPayRateIntervalId: number;
  pastEndPayRateIntervalName: string;
  currentHourlySubsidyRate: string;
  computedCurrentWageRateUnit: string;
  computedCurrentWageRateValue: string;
  computedPastEndWageRateUnit: string;
  computedPastEndWageRateValue: string;
  computedDB2WageRateUnit: string;
  computedDB2WageRateValue: string;

  unchangedPastPayRateIndicator: boolean;
  workSiteContribution: string;
  wageHourHistories: WageHourHistory[];

  public static clone(input: any, instance: WageHour) {
    instance.currentEffectiveDate = input.currentEffectiveDate;
    instance.currentPayType = Utilities.deserilizeChild(input.currentPayType, JobActionType);
    instance.currentPayTypeDetails = input.currentPayTypeDetails;
    instance.currentAverageWeeklyHours = input.currentAverageWeeklyHours;
    instance.currentPayRate = input.currentPayRate;
    instance.currentPayRateIntervalId = input.currentPayRateIntervalId;
    instance.currentPayRateIntervalName = input.currentPayRateIntervalName;
    instance.pastBeginPayRate = input.pastBeginPayRate;
    instance.pastBeginPayRateIntervalId = input.pastBeginPayRateIntervalId;
    instance.pastBeginPayRateIntervalName = input.pastBeginPayRateIntervalName;
    instance.pastEndPayRate = input.pastEndPayRate;
    instance.pastEndPayRateIntervalId = input.pastEndPayRateIntervalId;
    instance.pastEndPayRateIntervalName = input.pastEndPayRateIntervalName;
    instance.currentHourlySubsidyRate = input.currentHourlySubsidyRate;
    instance.computedCurrentWageRateUnit = input.computedCurrentWageRateUnit;
    instance.computedCurrentWageRateValue = input.computedCurrentWageRateValue;
    instance.computedPastEndWageRateUnit = input.computedPastEndWageRateUnit;
    instance.computedPastEndWageRateValue = input.computedPastEndWageRateValue;
    instance.computedDB2WageRateUnit = input.computedDB2WageRateUnit;
    instance.computedDB2WageRateValue = input.computedDB2WageRateValue;
    instance.unchangedPastPayRateIndicator = input.unchangedPastPayRateIndicator;
    instance.workSiteContribution = input.workSiteContribution;
    instance.wageHourHistories = Utilities.deserilizeChildren(input.wageHourHistories, WageHourHistory, 0);

    return instance;
  }

  public deserialize(input: any) {
    WageHour.clone(input, this);
    return this;
  }

  public static create() {
    const obj = new WageHour();
    obj.currentPayType = new JobActionType();
    return obj;
  }

  calculateHourlyWage(whh?: WageHourHistory): CalculatedString {
    let amount: number;
    let payTypeIntervalName: string;
    let hours: string;
    if (this.currentPayRate !== null || this.pastBeginPayRate !== null || whh) {
      if (whh && whh.payRate != null && whh.payRate.trim() !== '' && whh.payRateIntervalId != null) {
        amount = Number(whh.payRate.replace(/\,/g, ''));
        payTypeIntervalName = whh.payRateIntervalName;
        hours = whh.averageWeeklyHours;
      } else if (
        this.unchangedPastPayRateIndicator !== true &&
        this.currentPayRate != null &&
        this.currentPayRate.trim() !== '' &&
        this.currentPayRateIntervalName != null &&
        this.currentPayRateIntervalName.trim() !== ''
      ) {
        amount = Number(this.currentPayRate.replace(/\,/g, ''));
        payTypeIntervalName = this.currentPayRateIntervalName;
        hours = this.currentAverageWeeklyHours;
      } else if (this.pastEndPayRate != null && this.pastEndPayRate.trim() !== '' && this.pastEndPayRateIntervalName != null && this.pastEndPayRateIntervalName.trim() !== '') {
        amount = Number(this.pastEndPayRate.replace(/\,/g, ''));
        payTypeIntervalName = this.pastEndPayRateIntervalName;
        hours = this.currentAverageWeeklyHours;
      } else if (
        this.unchangedPastPayRateIndicator === true &&
        this.pastBeginPayRate != null &&
        this.pastBeginPayRate.trim() !== '' &&
        this.pastBeginPayRateIntervalName != null &&
        this.pastBeginPayRateIntervalName.trim() !== ''
      ) {
        amount = Number(this.pastBeginPayRate.replace(/\,/g, ''));
        payTypeIntervalName = this.pastBeginPayRateIntervalName;
        hours = this.currentAverageWeeklyHours;
      } else {
        return null;
      }
    } else {
      return null;
    }

    const cs = this.calculateHourlyWageValue(payTypeIntervalName, hours, amount);

    return cs;
  }

  calculateHourlyWageForDB2(intervalTypes: DropDownField[]) {
    let amount: number;
    let payTypeIntervalName: string;
    let hours: string;

    if (this.wageHourHistories && this.wageHourHistories.length > 0) {
      const sorted = _.orderBy<WageHourHistory>(
        this.wageHourHistories,
        [
          function(o) {
            return new Date(o.effectiveDate);
          }
        ],
        ['asc']
      )[0];

      const intervalName =
        sorted.payRateIntervalName && sorted.payRateIntervalName !== '' ? sorted.payRateIntervalName : Utilities.fieldDataNameById(sorted.payRateIntervalId, intervalTypes);

      if (sorted.payRate && sorted.payRate.trim() !== '') {
        amount = Number(sorted.payRate.replace(/\,/g, ''));
        payTypeIntervalName = intervalName;
        hours = sorted.averageWeeklyHours;
      }
    } else if (this.pastBeginPayRate && this.pastBeginPayRate.trim() !== '') {
      amount = Number(this.pastBeginPayRate.replace(/\,/g, ''));
      payTypeIntervalName = this.pastBeginPayRateIntervalName;
      hours = this.currentAverageWeeklyHours;
    } else if (this.currentPayRate && this.currentPayRate.trim() !== '') {
      amount = Number(this.currentPayRate.replace(/\,/g, ''));
      payTypeIntervalName = this.currentPayRateIntervalName;
      hours = this.currentAverageWeeklyHours;
    }

    const cs =
      payTypeIntervalName && payTypeIntervalName.trim() !== '' && hours && hours.trim() !== '' && amount ? this.calculateHourlyWageValue(payTypeIntervalName, hours, amount) : null;

    return cs;
  }

  calculateHourlyWageValue(payTypeIntervalName: string, hours: string, amount: number) {
    let durationDivisor: number;
    let isCalculable = false;

    switch (payTypeIntervalName.toLowerCase()) {
      case 'hour':
        durationDivisor = 1;
        isCalculable = false;
        break;
      case 'day':
        durationDivisor = 1;
        isCalculable = false;
        break;
      case 'week':
        durationDivisor = Number(hours);
        isCalculable = true;
        break;
      case 'semi-monthly':
        amount = (2 * amount) / 4.3;
        durationDivisor = Number(hours);
        isCalculable = true;
        break;
      case 'bi-weekly':
        amount = (2.15 * amount) / 4.3;
        durationDivisor = Number(hours);
        isCalculable = true;
        break;
      case 'month':
        amount = amount / 4.3;
        durationDivisor = Number(hours);
        isCalculable = true;
        break;
      case 'year':
        durationDivisor = 1;
        isCalculable = false;
        break;
      case 'irregular':
        durationDivisor = 1;
        isCalculable = false;
        break;
    }

    const cs = new CalculatedString();
    if (isCalculable === true) {
      const calculatedHourlyWage = amount / durationDivisor;
      const calculatedHourlyWageFixed = calculatedHourlyWage.toFixed(2);
      cs.value = calculatedHourlyWageFixed;
      cs.units = 'Hour';
      cs.isCalculated = true;
    } else {
      cs.value = amount.toFixed(2);
      cs.units = payTypeIntervalName;
      cs.isCalculated = false;
    }

    return cs;
  }

  // isPayRateDisabled(noPayPayTypeId: number): boolean {
  //     if (this.currentPayType != null && this.currentPayType.jobActionTypes.indexOf(noPayPayTypeId) > -1) {
  //         return true;
  //     } else {
  //         return false;
  //     }
  // }

  isBeginningPayDisabled(jobTypeId: number, volunteerJobTypeId: number, noPayPayTypeId: number): boolean {
    if (
      Number(jobTypeId) === volunteerJobTypeId ||
      (this.currentPayType != null && this.currentPayType.jobActionTypes != null && this.currentPayType.jobActionTypes.indexOf(noPayPayTypeId) !== -1)
    ) {
      return true;
    } else {
      return false;
    }
  }
  isEmployerOfRecordRequired(
    tempCustodialParentSubsidizedId: number,
    tempNonCustodialParentSubsidizedId: number,
    tmjSubsidizedId: number,
    tjSubsidizedId: number,
    jobTypeId: number
  ) {
    if (
      Number(jobTypeId) === tempCustodialParentSubsidizedId ||
      Number(jobTypeId) === tempNonCustodialParentSubsidizedId ||
      Number(jobTypeId) === tmjSubsidizedId ||
      Number(jobTypeId) === tjSubsidizedId
    ) {
      return true;
    } else {
      return false;
    }
  }

  isTJOrTMJSubsidizedJob(jobTypeId: number, tjSubsidizedId: number, tmjSubsidizedId: number) {
    if (!!jobTypeId && !!tjSubsidizedId && !!tmjSubsidizedId) {
      if (Number(jobTypeId === tjSubsidizedId || Number(jobTypeId) === tmjSubsidizedId)) {
        return true;
      } else {
        return false;
      }
    }
  }

  isEndPayDisabled(jobTypeId: number, volunteerJobTypeId: number, noPayPayTypeId: number): boolean {
    if (
      Number(jobTypeId) === volunteerJobTypeId ||
      (this.currentPayType != null && this.currentPayType.jobActionTypes != null && this.currentPayType.jobActionTypes.indexOf(noPayPayTypeId) !== -1)
    ) {
      return true;
    } else if (
      Number(jobTypeId) !== volunteerJobTypeId &&
      this.currentPayType != null &&
      this.currentPayType.jobActionTypes != null &&
      this.currentPayType.jobActionTypes.indexOf(noPayPayTypeId) === -1 &&
      this.unchangedPastPayRateIndicator === true
    ) {
      return true;
    } else {
      return false;
    }
  }

  isCurrentPayRateDisabled(jobTypeId: number, noPayPayTypeId: number, volunteerJobTypeId: number): boolean {
    if (
      Number(jobTypeId) === volunteerJobTypeId ||
      (this.currentPayType != null && this.currentPayType.jobActionTypes != null && this.currentPayType.jobActionTypes.indexOf(noPayPayTypeId) > -1)
    ) {
      return true;
    } else {
      return false;
    }
  }

  isCurrentPayTypeDisabled(jobTypeId: number, volunteerJobTypeId: number) {
    if (Number(jobTypeId) === volunteerJobTypeId) {
      return true;
    } else {
      return false;
    }
  }

  isUnchangedPastPayRateIndicatorDisabled(jobTypeId: number, volunteerJobTypeId: number, noPayPayTypeId: number): boolean {
    if (
      Number(jobTypeId) === volunteerJobTypeId ||
      (this.currentPayType != null && this.currentPayType.jobActionTypes != null && this.currentPayType.jobActionTypes.indexOf(noPayPayTypeId) !== -1)
    ) {
      return true;
    } else if (
      Number(jobTypeId) !== volunteerJobTypeId &&
      this.currentPayType != null &&
      this.currentPayType.jobActionTypes != null &&
      this.currentPayType.jobActionTypes.indexOf(noPayPayTypeId) === -1 &&
      this.unchangedPastPayRateIndicator === true
    ) {
      return false;
    } else {
      return false;
    }
  }

  isCurrentPayTypeDetailsRequired(otherPayTypeId: number): boolean {
    if (this.currentPayType != null && this.currentPayType.jobActionTypes != null && this.currentPayType.jobActionTypes.indexOf(otherPayTypeId) !== -1) {
      return true;
    } else {
      return false;
    }
  }

  eraseCurrentWage() {
    this.currentEffectiveDate = null;
    this.currentPayType.jobActionTypes = [];
    this.currentPayTypeDetails = null;
    this.currentAverageWeeklyHours = null;
    this.currentPayRate = null;
    this.currentPayRateIntervalId = null;
    this.currentPayRateIntervalName = null;
    this.currentHourlySubsidyRate = null;
    this.workSiteContribution = null;
  }

  validate(
    validationManager: ValidationManager,
    participantDOB: moment.Moment,
    jobBeginDate: string,
    jobEndDate: string,
    programStatus: string,
    isCurrentlyEmployed: boolean,
    jobTypeId: number,
    otherEffectiveDates: Payload,
    modelIds: WorkHistoryIdentities,
    isCurrentJobAtCreation: boolean,
    result?: ValidationResult
  ): ValidationResult {
    if (result === undefined) {
      result = new ValidationResult();
    }

    // Current Job and In Program Validation.
    if (programStatus === 'currentJob' || isCurrentJobAtCreation) {
      let currentEffectiveDateIsValid = true;
      //  Effective Date is a required field.
      if (this.currentEffectiveDate == null || this.currentEffectiveDate.trim() === '') {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Effective Date');
        result.addError('currentEffectiveDate');
        currentEffectiveDateIsValid = false;
      } else {
        const inputDate = moment(this.currentEffectiveDate, 'MM/DD/YYYY');
        //  Effective Date must be 8 digits in two digit month, two digit day, and four digit year format. (MM/DD/YYYY).
        if (this.currentEffectiveDate.length !== 10) {
          validationManager.addErrorWithFormat(ValidationCode.ValueInInvalidFormat_Name_FormatType, 'Effective Date', 'MM/DD/YYYY');
          result.addError('currentEffectiveDate');
          currentEffectiveDateIsValid = false;
        } else if (inputDate.isValid()) {
          // Check Min Date.
          if (inputDate < participantDOB) {
            validationManager.addErrorWithFormat(
              ValidationCode.ValueBeforeDOB_Name_Value_DOB,
              'Effective Date',
              inputDate.format('MM/DD/YYYY'),
              participantDOB.format('MM/DD/YYYY')
            );
            result.addError('currentEffectiveDate');
            currentEffectiveDateIsValid = false;
          }

          const currentDatePlus10Days = Utilities.currentDate.add(10, 'days');
          // Check Max Date.
          if (inputDate > currentDatePlus10Days) {
            validationManager.addErrorWithFormat(ValidationCode.DateAfterDuration, 'Effective Date', inputDate.format('MM/DD/YYYY'), '10 Days');
            result.addError('currentEffectiveDate');
            currentEffectiveDateIsValid = false;
          }

          // Remove this effective date before checking for dups.
          const index = otherEffectiveDates.stringArray.indexOf(inputDate.format('MM/DD/YYYY'));
          otherEffectiveDates.stringArray.splice(index, 1);
          // Check for any dups.
          for (const eDate of otherEffectiveDates.stringArray) {
            if (moment(eDate, 'MM/DD/YYYY').format('MM/DD/YYYY') === inputDate.format('MM/DD/YYYY')) {
              validationManager.addErrorWithFormat(ValidationCode.HistoryTableDuplicate, 'Effective Dates');
              result.addError('currentEffectiveDate');
            }
            if (moment(eDate, 'MM/DD/YYYY').isAfter(inputDate.format('MM/DD/YYYY'))) {
              validationManager.addErrorWithFormat(ValidationCode.DateBeforeDate_Name1_Name2, 'Current Effective Date', 'History Effective Date');
              result.addError('currentEffectiveDate');
            }
          }
        } else {
          validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, 'Invalid Date Effective Entered');
          result.addError('currentEffectiveDate');
        }
      }

      const beginDateM = moment(jobBeginDate, 'MM/DD/YYYY');
      if (currentEffectiveDateIsValid && beginDateM.isValid()) {
        if (moment(this.currentEffectiveDate, 'MM/DD/YYYY') < beginDateM) {
          validationManager.addErrorWithFormat(ValidationCode.DateBeforeDate_Name1_Name2, 'Effective Date', 'Job Begin Date');
          result.addError('currentEffectiveDate');
        }
      }

      // Pay Types is a required field.
      if (!this.isCurrentPayTypeDisabled(jobTypeId, modelIds.volunteerJobTypeId)) {
        if ((this.currentPayType != null && this.currentPayType.jobActionTypes == null) || this.currentPayType.jobActionTypes.length === 0) {
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Pay Types');
          result.addError('currentPayType');
        } else if (this.currentPayType.jobActionTypes.indexOf(modelIds.otherPayTypeId) > -1 && (this.currentPayTypeDetails == null || this.currentPayTypeDetails.trim() === '')) {
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Pay Type Details');
          result.addError('currentPayTypeDetails');
        }
      }

      // Average Weekly Hours is a required field.
      if (this.currentAverageWeeklyHours == null || this.currentAverageWeeklyHours.toString().trim() === '') {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Average Weekly Hours');
        result.addError('currentAverageWeeklyHours');
      } else if (Number(this.currentAverageWeeklyHours) > 120) {
        validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, 'Average Weekly Hours must be 120 or less');
        result.addError('currentAverageWeeklyHours');
      }
      if (this.currentAverageWeeklyHours != null && isNaN(Number(this.currentAverageWeeklyHours.replace(/\,/g, '')))) {
        validationManager.addErrorWithFormat(ValidationCode.InvalidChar, 'Current Average Weekly Hours', this.currentAverageWeeklyHours);
        result.addError('currentAverageWeeklyHours');
      }

      if (!this.isCurrentPayRateDisabled(jobTypeId, modelIds.noPayPayTypeId, modelIds.volunteerJobTypeId)) {
        // Pay Rate is a required field.
        if (this.currentPayRate == null || Number(this.currentPayRate) <= 0) {
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Pay Rate');
          result.addError('currentPayRate');
        }
        if (this.currentPayRate != null && isNaN(Number(this.currentPayRate.replace(/\,/g, '')))) {
          validationManager.addErrorWithFormat(ValidationCode.InvalidChar, 'Current Pay Rate', this.currentPayRate);
          result.addError('currentPayRate');
        }

        if (this.currentPayRateIntervalId == null || this.currentPayRateIntervalId.toString() === '') {
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Pay Rate Interval');
          result.addError('currentPayRateIntervalId');
        }
      }

      if (
        this.isEmployerOfRecordRequired(
          modelIds.tempCustodialParentSubsidizedId,
          modelIds.tempNonCustodialParentSubsidizedId,
          modelIds.tmjSubsidizedId,
          modelIds.tjSubsidizedId,
          jobTypeId
        )
      ) {
        if (this.currentHourlySubsidyRate == null || this.currentHourlySubsidyRate.trim() === '') {
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Wage/Hour - Current Hourly Subsidy');
          result.addError('currentHourlySubsidyRate');
        }
      }
      if (this.isTJOrTMJSubsidizedJob(jobTypeId, modelIds.tjSubsidizedId, modelIds.tmjSubsidizedId)) {
        if (this.workSiteContribution == null || this.workSiteContribution.trim() === '') {
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Wage/Hour - Work Site Contribution');
          result.addError('workSiteContribution');
        }
        if (this.currentPayRate && this.currentHourlySubsidyRate && this.workSiteContribution) {
          if (+this.currentHourlySubsidyRate + +this.workSiteContribution > +this.currentPayRate) {
            validationManager.addErrorWithDetail(
              ValidationCode.ValueOutOfRange_Details,
              `The total of "Work Site Contribution" and "Hourly Subsidy" cannot be greater than the "Pay Rate"`
            );
            result.addError('workSiteContribution');
          }
        }
      }
    }

    if (programStatus === 'pastJob') {
      // Past Job.
      // Pay Types is a required field.
      if (!this.isCurrentPayRateDisabled(jobTypeId, modelIds.noPayPayTypeId, modelIds.volunteerJobTypeId)) {
        if ((this.currentPayType != null && this.currentPayType.jobActionTypes == null) || this.currentPayType.jobActionTypes.length === 0) {
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Pay Types');
          result.addError('currentPayType');
        }
      }

      if (this.isCurrentPayTypeDetailsRequired(modelIds.otherPayTypeId)) {
        if (this.currentPayTypeDetails == null || this.currentPayTypeDetails.trim() === '') {
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Pay Type Details');
          result.addError('currentPayTypeDetails');
        }
      }

      // Average Weekly Hours is a required field.
      if (this.currentAverageWeeklyHours == null || this.currentAverageWeeklyHours.toString().trim() === '') {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Average Weekly Hours');
        result.addError('currentAverageWeeklyHours');
      } else if (Number(this.currentAverageWeeklyHours) > 120) {
        validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, 'Average Weekly Hours must be 120 or less');
        result.addError('currentAverageWeeklyHours');
      }

      if (!this.isBeginningPayDisabled(jobTypeId, modelIds.volunteerJobTypeId, modelIds.noPayPayTypeId)) {
        if ((this.pastBeginPayRate == null || Number(this.pastBeginPayRate) <= 0) && !isCurrentJobAtCreation) {
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Begin Pay Rate');
          result.addError('pastBeginPayRate');
        }

        if ((this.pastBeginPayRateIntervalId == null || this.pastBeginPayRateIntervalId.toString() === '') && !isCurrentJobAtCreation) {
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Begin Pay Rate Interval');
          result.addError('pastBeginPayRateIntervalId');
        }
      }

      if (!this.isEndPayDisabled(jobTypeId, modelIds.volunteerJobTypeId, modelIds.noPayPayTypeId)) {
        if ((this.pastEndPayRate == null || Number(this.pastEndPayRate) <= 0) && !isCurrentJobAtCreation) {
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'End Pay Rate');
          result.addError('pastEndPayRate');
        }
        if (
          (this.pastEndPayRateIntervalId == null || (this.pastEndPayRateIntervalId.toString() === '' && this.unchangedPastPayRateIndicator !== true)) &&
          !isCurrentJobAtCreation
        ) {
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'End Pay Rate Interval');
          result.addError('pastEndPayRateIntervalId');
        }
      }
    }
    return result;
  }
}

export class WageHourHistory implements Serializable<WageHourHistory> {
  id: number;
  hourlySubsidyRate: string;
  effectiveDate: string;
  payTypeDetails: string;
  averageWeeklyHours: string;
  payRate: string;
  payRateIntervalId: number;
  payRateIntervalName: string;
  historyPayType: JobActionType;
  rowVersion: string;
  computedWageRateUnit: string;
  computedWageRateValue: string;
  modifiedBy: string;
  modifiedDate: string;

  // Used to detect if this object is open in repeater.
  isOpen: boolean;
  isMovedFromCurrent = false;
  isDeletedFromCurrent = false;
  workSiteContribution: string;

  public static clone(input: any, instance: WageHourHistory) {
    instance.id = input.id;
    instance.hourlySubsidyRate = input.hourlySubsidyRate;
    instance.effectiveDate = input.effectiveDate;
    instance.payTypeDetails = input.payTypeDetails;
    instance.computedWageRateUnit = input.computedWageRateUnit;
    instance.computedWageRateValue = input.computedWageRateValue;
    instance.averageWeeklyHours = input.averageWeeklyHours;
    instance.payRate = input.payRate;
    instance.payRateIntervalId = input.payRateIntervalId;
    instance.payRateIntervalName = input.payRateIntervalName;
    instance.historyPayType = new JobActionType().deserialize(input.historyPayType);
    instance.rowVersion = input.rowVersion;
    instance.modifiedBy = input.modifiedBy;
    instance.modifiedDate = input.modifiedDate;
    instance.isOpen = false;
    instance.workSiteContribution = input.workSiteContribution;
    return instance;
  }

  public deserialize(input: any) {
    WageHourHistory.clone(input, this);
    return this;
  }

  isPayTypeHidden(jobTypeId: number, volunteerJobTypeId: number): boolean {
    if (Number(jobTypeId) === volunteerJobTypeId) {
      return true;
    } else {
      return false;
    }
  }

  isPayTypeDetailsRequired(otherPayTypeId: number) {
    if (this.historyPayType != null && this.historyPayType.jobActionTypes != null && this.historyPayType.jobActionTypes.indexOf(otherPayTypeId) > -1) {
      return true;
    } else {
      return false;
    }
  }

  isPayRateDisabled(jobTypeId: number, noPayPayTypeId: number, volunteerJobTypeId: number): boolean {
    if (
      Number(jobTypeId) === volunteerJobTypeId ||
      (this.historyPayType != null && this.historyPayType.jobActionTypes != null && this.historyPayType.jobActionTypes.indexOf(noPayPayTypeId) !== -1)
    ) {
      return true;
    } else {
      return false;
    }
  }
  isTJOrTMJSubsidizedJob(jobTypeId: number, tjSubsidizedId: number, tmjSubsidizedId: number) {
    if (!!jobTypeId && !!tjSubsidizedId && !!tmjSubsidizedId) {
      if (Number(jobTypeId === tjSubsidizedId || Number(jobTypeId) === tmjSubsidizedId)) {
        return true;
      } else {
        return false;
      }
    }
  }

  public validate(
    validationManager: ValidationManager,
    participantDOB: moment.Moment,
    jobTypeId: number,
    isCurrentJobAtCreation: boolean,
    jobBeginDate: moment.Moment,
    jobEndDate: moment.Moment,
    isCurrentlyEmployed: boolean,
    otherEffectiveDates: Payload,
    modelIds: WorkHistoryIdentities,
    isHourlySubsidyDisabled: boolean
  ): ValidationResult {
    const result = new ValidationResult();

    //  Effective Date is a required field.
    if (this.effectiveDate == null || this.effectiveDate.trim() === '') {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Effective Date');
      result.addError('effectiveDate');
    } else {
      const inputDate = moment(this.effectiveDate, 'MM/DD/YYYY');
      //  Effective Date must be 8 digits in two digit month, two digit day, and four digit year format. (MM/DD/YYYY).
      if (this.effectiveDate.length !== 10) {
        validationManager.addErrorWithFormat(ValidationCode.ValueInInvalidFormat_Name_FormatType, 'Effective Date', 'MM/DD/YYYY');
        result.addError('effectiveDate');
      } else if (inputDate.isValid()) {
        // Check Min Date.
        if (inputDate < jobBeginDate) {
          validationManager.addErrorWithFormat(ValidationCode.DateBeforeDate_Name1_Name2, 'Effective Date', 'Job Begin Date');
          result.addError('effectiveDate');
        }
        if (inputDate < participantDOB) {
          validationManager.addErrorWithFormat(ValidationCode.ValueBeforeDOB_Name_Value_DOB, 'Effective Date', inputDate.format('MM/DD/YYYY'), participantDOB.format('MM/DD/YYYY'));
          result.addError('effectiveDate');
        }

        // Check Max Date.
        const currentDatePlus10Days = Utilities.currentDate.add(10, 'days');
        if (inputDate > currentDatePlus10Days) {
          validationManager.addErrorWithFormat(ValidationCode.DateAfterDuration, 'Effective Date', inputDate.format('MM/DD/YYYY'), '10 Days');
          result.addError('effectiveDate');
        }

        if (inputDate > jobEndDate && isCurrentlyEmployed !== true) {
          validationManager.addErrorWithFormat(ValidationCode.DateAfterDate_Name1_Name2, 'Effective Date', 'Job End Date');
          result.addError('effectiveDate');
        }

        // make sure history effective date is before current effective date.
        for (const eDate of otherEffectiveDates.trivalue) {
          if (eDate.second === true && moment(eDate.first, 'MM/DD/YYYY') < inputDate) {
            validationManager.addErrorWithFormat(ValidationCode.DateAfterDate_Name1_Name2, 'History Effective Date', 'Current Effective Date');
            result.addError('effectiveDate');
          }
        }

        // Remove this effective date before checking for dups.
        const index = otherEffectiveDates.stringArray.indexOf(inputDate.format('MM/DD/YYYY'));
        otherEffectiveDates.stringArray.splice(index, 1);
        // Check for any dups.
        for (const eDate of otherEffectiveDates.stringArray) {
          if (moment(eDate, 'MM/DD/YYYY').format('MM/DD/YYYY') === inputDate.format('MM/DD/YYYY')) {
            validationManager.addErrorWithFormat(ValidationCode.HistoryTableDuplicate, 'Effective Dates');
            result.addError('effectiveDate');
          }
        }
      } else {
        validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, 'Invalid Effective Date Entered');
        result.addError('effectiveDate');
      }
    }

    if (!this.isPayTypeHidden(jobTypeId, modelIds.volunteerJobTypeId)) {
      // Pay Types is a required field.
      if ((this.historyPayType != null && this.historyPayType.jobActionTypes == null) || this.historyPayType.jobActionTypes.length === 0) {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Pay Types');
        result.addError('historyPayType');
      }
      // If Other pay type is selected, details is required.
      if (this.isPayTypeDetailsRequired(modelIds.otherPayTypeId) && (this.payTypeDetails == null || this.payTypeDetails.trim() === '')) {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Pay Type Details');
        result.addError('payTypeDetails');
      }
    }
    if (!this.isPayRateDisabled(jobTypeId, modelIds.noPayPayTypeId, modelIds.volunteerJobTypeId)) {
      //  Pay Rate is a required field.
      if (this.payRate == null || Number(this.payRate) === 0) {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Pay Rate');
        result.addError('payRate');
      }
      if (this.payRate != null && isNaN(Number(this.payRate.replace(/\,/g, '')))) {
        validationManager.addErrorWithFormat(ValidationCode.InvalidChar, 'Pay Rate', this.payRate);
        result.addError('payRate');
      }
      if (this.payRateIntervalId == null || this.payRateIntervalId.toString() === '') {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Pay Rate Interval');
        result.addError('payRateIntervalId');
      }
    }

    // Average Weekly Hours is a required field.
    if (this.averageWeeklyHours == null || this.averageWeeklyHours.toString().trim() === '') {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Average Weekly Hours');
      result.addError('averageWeeklyHours');
    } else if (this.averageWeeklyHours != null && isNaN(Number(this.averageWeeklyHours.replace(/\,/g, '')))) {
      validationManager.addErrorWithFormat(ValidationCode.InvalidChar, 'Average Weekly Hours', this.averageWeeklyHours);
      result.addError('currentAverageWeeklyHours');
    } else if (Number(this.averageWeeklyHours) > 120) {
      validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, 'Average Weekly Hours must be 120 or less');
      result.addError('averageWeeklyHours');
    }
    if (isCurrentlyEmployed && !isHourlySubsidyDisabled) {
      if (this.hourlySubsidyRate == null || this.hourlySubsidyRate.toString().trim() === '') {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Wage/Hour - Hourly Subsidy');
        result.addError('hourlySubsidyRate');
      }
      if (this.isTJOrTMJSubsidizedJob(jobTypeId, modelIds.tjSubsidizedId, modelIds.tmjSubsidizedId)) {
        if (this.workSiteContribution == null || this.workSiteContribution.toString().trim() === '') {
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Wage/Hour - Work Site Contribution');
          result.addError('workSiteContribution');
        }
        if (this.payRate && this.hourlySubsidyRate && this.workSiteContribution) {
          if (+this.hourlySubsidyRate + +this.workSiteContribution > +this.payRate) {
            validationManager.addErrorWithDetail(
              ValidationCode.ValueOutOfRange_Details,
              `The total of "Work Site Contribution" and "Hourly Subsidy" cannot be greater than the "Pay Rate"`
            );
            result.addError('workSiteContribution');
          }
        }
      }
    }

    return result;
  }
}

export class Absence implements Serializable<Absence> {
  id: number;
  beginDate: string;
  endDate: string;
  absenceReasonId: number;
  details: string;
  modifiedBy: string;
  modifiedDate: string;
  isOpen: boolean;
  isEndDateValid = true;

  public static clone(input: any, instance: Absence) {
    instance.id = input.id;
    instance.beginDate = input.beginDate;
    instance.endDate = input.endDate;
    instance.absenceReasonId = input.absenceReasonId;
    instance.details = input.details;
    instance.modifiedBy = input.modifiedBy;
    instance.modifiedDate = input.modifiedDate;
    instance.isOpen = false;
    return instance;
  }

  public deserialize(input: any) {
    Absence.clone(input, this);
    return this;
  }

  isDetailsRequired(otherAbsenceReasonId: number) {
    if (Number(this.absenceReasonId) === otherAbsenceReasonId) {
      return true;
    } else {
      return false;
    }
  }

  validate(
    validationManager: ValidationManager,
    participantDOB: moment.Moment,
    jobBeginDate: moment.Moment,
    jobEndDate: moment.Moment,
    isCurrentlyEmployed: boolean,
    otherAbsenceReasonId: number,
    absences: Absence[]
  ): ValidationResult {
    const result = new ValidationResult();

    let isBeginDateValid = true;
    //  Begin Date is a required field.
    if (this.beginDate == null || this.beginDate.trim() === '') {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Leave of Absence Begin Date');
      result.addError('beginDate');
      isBeginDateValid = false;
    } else {
      const inputDate = moment(this.beginDate, 'MM/DD/YYYY');
      if (this.beginDate.length !== 10) {
        validationManager.addErrorWithFormat(ValidationCode.ValueInInvalidFormat_Name_FormatType, 'Leave of Absence Begin Date', 'MM/DD/YYYY');
        result.addError('beginDate');
        isBeginDateValid = false;
      } else if (inputDate.isValid()) {
        // Check Min Date.
        if (inputDate.isBefore(participantDOB)) {
          validationManager.addErrorWithFormat(
            ValidationCode.ValueBeforeDOB_Name_Value_DOB,
            'Leave of Absence Begin Date',
            inputDate.format('MM/DD/YYYY'),
            participantDOB.format('MM/DD/YYYY')
          );
          result.addError('beginDate');
          isBeginDateValid = false;
        }
        if (inputDate.isBefore(jobBeginDate)) {
          validationManager.addErrorWithFormat(ValidationCode.DateBeforeDate_Name1_Name2, 'Leave of Absence Begin Date', 'Job Begin Date ' + jobBeginDate.format('MM/DD/YYYY'));
          result.addError('beginDate');
          isBeginDateValid = false;
        }
        // Check Max Date.
        if (inputDate.isSameOrAfter(jobEndDate) && isCurrentlyEmployed !== true) {
          validationManager.addErrorWithFormat(ValidationCode.DateAfterDate_Name1_Name2, 'Leave of Absence Begin Date', 'Job End Date');
          result.addError('beginDate');
          isBeginDateValid = false;
        }
      } else {
        validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, 'Invalid Leave of Absence Begin Date Entered');
        result.addError('beginDate');
        isBeginDateValid = false;
      }
    }

    let overlappedLOA: any;
    const dateRanges: { start: any; end: any }[] = [];
    if (absences) {
      absences.forEach(i => {
        if (i.isOpen !== this.isOpen) {
          const endDate = !i.endDate ? new Date(Utilities.currentDate.toISOString()) : new Date(i.endDate);
          dateRanges.push({ start: new Date(i.beginDate), end: new Date(endDate) });
        }
      });
      const end = !this.endDate ? new Date(Utilities.currentDate.toISOString()) : new Date(this.endDate);
      dateRanges.push({ start: new Date(this.beginDate), end: new Date(end) });
      overlappedLOA = Utilities.datesOverlap(dateRanges);

      if (
        this.beginDate &&
        moment(this.beginDate, 'MM/DD/YYYY', true).isValid() &&
        ((this.endDate && moment(this.endDate, 'MM/DD/YYYY', true).isValid()) || this.isOpen) &&
        overlappedLOA &&
        overlappedLOA.ranges &&
        overlappedLOA.ranges.length > 0 &&
        overlappedLOA.ranges[0].previous &&
        overlappedLOA.ranges[0].current &&
        overlappedLOA.overlap
      ) {
        const beginDate =
          moment(overlappedLOA.ranges[0].previous.start).format('MM/DD/YYYY') === moment(this.beginDate).format('MM/DD/YYYY')
            ? moment(overlappedLOA.ranges[0].current.start).format('MM/DD/YYYY')
            : moment(overlappedLOA.ranges[0].previous.start).format('MM/DD/YYYY');
        const endDate =
          moment(overlappedLOA.ranges[0].previous.end).format('MM/DD/YYYY') === moment(!this.endDate ? Utilities.currentDate : this.endDate).format('MM/DD/YYYY')
            ? moment(overlappedLOA.ranges[0].current.end).format('MM/DD/YYYY')
            : moment(overlappedLOA.ranges[0].previous.end).format('MM/DD/YYYY');
        const errMsg =
          endDate === moment(Utilities.currentDate).format('MM/DD/YYYY')
            ? `Current Leave of Absence record exists with Begin Date ${beginDate}. You cannot have overlapping dates for any Leave of Absence record.`
            : `Leave of Absence record previously exists with Begin Date ${beginDate} and End Date ${endDate}.You cannot have overlapping dates for any Leave of Absence record.`;
        validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, errMsg);
        result.addError('endDate');
        result.addError('beginDate');
      }
    }

    //  End Date is a required field.
    if (this.endDate) {
      const inputDate = moment(this.endDate, 'MM/DD/YYYY');
      if (this.endDate && this.endDate.length !== 10) {
        validationManager.addErrorWithFormat(ValidationCode.ValueInInvalidFormat_Name_FormatType, 'Leave of Absence End Date', 'MM/DD/YYYY');
        result.addError('endDate');
        this.isEndDateValid = false;
      } else if (inputDate.isValid()) {
        // Check Min Date.
        if (inputDate.isBefore(participantDOB)) {
          validationManager.addErrorWithFormat(
            ValidationCode.ValueBeforeDOB_Name_Value_DOB,
            'Leave of Absence End Date',
            inputDate.format('MM/DD/YYYY'),
            participantDOB.format('MM/DD/YYYY')
          );
          result.addError('endDate');
          this.isEndDateValid = false;
        }
        if (inputDate.isBefore(moment(this.beginDate, 'MM/DD/YYYY'))) {
          validationManager.addErrorWithFormat(ValidationCode.DateBeforeDate_Name1_Name2, 'Leave of Absence End Date', 'Begin Date');
          result.addError('endDate');
          this.isEndDateValid = false;
        }
        // Check Max Date.
        if (inputDate.isAfter(jobEndDate) && isCurrentlyEmployed !== true) {
          validationManager.addErrorWithFormat(ValidationCode.DateAfterDate_Name1_Name2, 'Leave of Absence End Date', 'Job End Date');
          result.addError('endDate');
          this.isEndDateValid = false;
        }
        if (participantDOB !== null && inputDate > moment(participantDOB, 'MM/DD/YYYY').add(150, 'years')) {
          validationManager.addErrorWithFormat(
            ValidationCode.ValueBefore_X_PlusDOB_Name_Value_DOB,
            'Leave of Absence End Date',
            inputDate.format('MM/DD/YYYY'),
            moment(participantDOB).format('MM/DD/YYYY'),
            '150'
          );
          result.addError('endDate');
          this.isEndDateValid = false;
        }
      } else {
        validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, 'Invalid LOA End Date Entered');
        result.addError('endDate');
        this.isEndDateValid = false;
      }
    }

    if (this.endDate && isBeginDateValid && this.isEndDateValid) {
      // If both begin and end dates are invalid.
      const beginDate = moment(this.beginDate, 'MM/DD/YYYY');
      const endDate = moment(this.endDate, 'MM/DD/YYYY');
      if (beginDate > endDate) {
        validationManager.addErrorWithFormat(ValidationCode.DateBeforeDate_Name1_Name2, 'Leave of Absence End Date', 'Leave of Absence Begin Date');
        result.addError('beginDate');
        result.addError('endDate');
      } else if (beginDate.toISOString() === endDate.toISOString()) {
        validationManager.addErrorWithFormat(ValidationCode.ValueSameValue_Name1_Name2, 'Leave of Absence End Date', 'Leave of Absence Begin Date');
        result.addError('beginDate');
        result.addError('endDate');
      }
    }

    //  Reason is a required field.
    if (this.absenceReasonId == null || this.absenceReasonId.toString() === '') {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Leave of Absence Reason');
      result.addError('absenceReasonId');
    }

    if (this.isDetailsRequired(otherAbsenceReasonId)) {
      if (this.details == null || this.details.trim() === '') {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Leave of Absence Details');
        result.addError('details');
      }
    }

    return result;
  }
}
