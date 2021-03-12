import { Participant } from './../../../shared/models/participant';
import * as moment from 'moment';
import * as _ from 'lodash';
import { Serializable } from 'src/app/shared/interfaces/serializable';
import { ValidationManager, ValidationResult, ValidationCode } from 'src/app/shared/models/validation';
import { Utilities } from 'src/app/shared/utilities';
import { DropDownMultiField } from 'src/app/shared/models/dropdown-multi-field';
import { EnrolledProgramCode } from 'src/app/shared/enums/enrolled-program-code.enum';

export class EmployabilityPlan implements Serializable<EmployabilityPlan> {
  id: number;
  enrolledProgramId: number;
  enrolledProgramName: string;
  pepId: number;
  enrolledProgramCd: string;
  beginDate: string;
  endDate: string;
  employabilityPlanStatusTypeId: number;
  employabilityPlanStatusTypeName: string;
  canSaveWithoutActivity: boolean;
  canSaveWithoutActivityDetails: string;
  isDeleted: boolean;
  notes: string;
  rowVersion: string;
  modifiedBy: string;
  modifiedDate: string;
  createdDate: string;
  organizationId: number;
  submitDate: string;
  errorMessage: string;

  public static clone(input: any, instance: EmployabilityPlan) {
    instance.id = input.id;
    instance.enrolledProgramId = input.enrolledProgramId;
    instance.enrolledProgramName = input.enrolledProgramName;
    instance.pepId = input.pepId;
    instance.enrolledProgramCd = input.enrolledProgramCd;
    instance.organizationId = input.organizationId;
    instance.beginDate = Utilities.toMmDdYyyy(input.beginDate);
    instance.endDate = Utilities.toMmDdYyyy(input.endDate);
    instance.employabilityPlanStatusTypeId = input.employabilityPlanStatusTypeId;
    instance.employabilityPlanStatusTypeName = input.employabilityPlanStatusTypeName;
    instance.isDeleted = input.isDeleted;
    instance.notes = input.notes;
    instance.canSaveWithoutActivity = input.canSaveWithoutActivity;
    instance.canSaveWithoutActivityDetails = input.canSaveWithoutActivityDetails;
    instance.rowVersion = input.rowVersion;
    instance.modifiedBy = input.modifiedBy;
    instance.modifiedDate = input.modifiedDate;
    instance.createdDate = input.createdDate;
    instance.submitDate = Utilities.toMmDdYyyy(input.submitDate);
    instance.errorMessage = input.errorMessage;
  }

  public deserialize(input: any) {
    EmployabilityPlan.clone(input, this);
    return this;
  }

  // US2425.
  get isEnrolledProgramRequired() {
    return true;
  }

  // US2425.
  get isBackDateRequired() {
    return true;
  }

  // US2425.
  get isEpBeginDateRequired() {
    if (this.beginDate === '' || this.beginDate === undefined) {
      return true;
    } else {
      return false;
    }
  }

  // US2425.
  get isEpEndDateRequired() {
    if (this.endDate === '' || this.endDate === undefined) {
      return true;
    } else {
      return false;
    }
  }

  checkForCutOverDateValidation(
    result: ValidationResult,
    validationManager: ValidationManager,
    participant: Participant,
    canValidateCutOverDate,
    inputDate,
    isParticipantEnrolledBeforePhase2,
    subtractFromCurrentDate
  ) {
    if (participant.cutOverDate === null && canValidateCutOverDate) {
      const closest16th =
        Utilities.currentDate.date() < 16
          ? moment(`${Utilities.currentDate.month()}/16/${Utilities.currentDate.year()}`).format('MM/DD/YYYY')
          : moment(`${Utilities.currentDate.month() + 1}/16/${Utilities.currentDate.year()}`).format('MM/DD/YYYY');
      if (!inputDate.isSame(closest16th) && isParticipantEnrolledBeforePhase2) {
        validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, `Participant's first EP in WWP must have a begin date on the 16th of the month.`);
        result.addError('beginDate');
      }
      if (
        inputDate.isBefore(
          moment(
            Utilities.currentDate
              .clone()
              .subtract(isParticipantEnrolledBeforePhase2 ? 30 : subtractFromCurrentDate, 'days')
              .format('MM/DD/YYYY')
          )
        )
      ) {
        validationManager.addErrorWithFormat(ValidationCode.DateBeforeMaxDaysCanBackDate, 'EP Begin Date', isParticipantEnrolledBeforePhase2 ? '30' : subtractFromCurrentDate);
        result.addError('beginDate');
      }
    }
  }

  validate(
    validationManager: ValidationManager,
    enrollmentDate: string,
    compareBeginDate: any,
    subtractFromCurrentDate: any,
    currentEnrollmentDate: any,
    disableFields: boolean,
    pullDownDates: DropDownMultiField[],
    employabilityPlans: EmployabilityPlan[],
    participant: Participant,
    canValidateCutOverDate: boolean,
    epGoLiveDate: any,
    inProgressEps?: EmployabilityPlan[],
    currentDate?: any,
    enrolledProgramName?: string
  ): ValidationResult {
    const result = new ValidationResult();
    const compareDate = moment(
      Utilities.currentDate
        .clone()
        .subtract(subtractFromCurrentDate, 'days')
        .format('MM/DD/YYYY')
    );
    const setDate = moment(compareBeginDate).format('MM/DD/YYYY');
    const convertedcurrentEnrollmentDate = moment(currentEnrollmentDate).format('MM/DD/YYYY');
    inProgressEps.forEach(ep => {
      if (ep.enrolledProgramId === this.enrolledProgramId && this.id === 0) {
        validationManager.addErrorWithFormat(ValidationCode.EP, 'EP is currently In Progress for the same selected program.');
        result.addError('DuplicateProgramSelected');
      }
    });

    if (this.isEnrolledProgramRequired) {
      Utilities.validateDropDown(this.enrolledProgramId, 'enrolledProgramId', 'Program Type', result, validationManager);
    }
    //convert user input date to moment
    const userInputDateFormatted = moment(this.beginDate, 'MM/DD/YYYY');
    const userInputDate = moment(this.beginDate);

    if (this.isEpBeginDateRequired || !disableFields) {
      if (
        !employabilityPlans.some(x => x.enrolledProgramName.trim().toLowerCase() === EnrolledProgramCode.w2name) &&
        enrolledProgramName &&
        enrolledProgramName.trim().toLowerCase() === EnrolledProgramCode.w2name
      ) {
        const isParticipantEnrolledBeforePhase2 = moment(currentEnrollmentDate).isBefore(epGoLiveDate);
        this.checkForCutOverDateValidation(
          result,
          validationManager,
          participant,
          canValidateCutOverDate,
          userInputDateFormatted,
          isParticipantEnrolledBeforePhase2,
          subtractFromCurrentDate
        );
      }
      if (this.beginDate === null || this.beginDate === undefined || this.beginDate === '') {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'EP Begin Date');
        result.addError('beginDate');
      }
      if (this.beginDate !== '' && this.beginDate !== null && this.beginDate !== undefined && this.beginDate.length !== 10) {
        validationManager.addErrorWithFormat(ValidationCode.ValueInInvalidFormat_Name_FormatType, 'EP Begin Date', 'MM/DD/YYYY');
        result.addError('beginDate');
      } else if (this.beginDate !== null && this.beginDate !== undefined && this.beginDate !== '' && userInputDateFormatted.isValid() && !disableFields) {
        if (
          enrolledProgramName &&
          enrolledProgramName.trim().toLowerCase() === EnrolledProgramCode.w2name &&
          Utilities.participationPeriodCheck(pullDownDates, userInputDateFormatted, true)
        ) {
          validationManager.addErrorWithFormat(ValidationCode.ParticipationPeriodCheck, 'EP Begin', 'first', 'current');
          result.addError('beginDate');
        }
        if (userInputDate.isBefore(epGoLiveDate)) {
          //Begin Date cannot be before the EP Go Live Date
          validationManager.addErrorWithFormat(
            ValidationCode.DateBeforeDate_Name1_Name2,
            'EP Begin Date',
            `Phase 2 implementation date of ${moment(epGoLiveDate).format('MM/DD/YYYY')}`
          );
          result.addError('beginDate');
        } else if (userInputDateFormatted.isBefore(compareDate) && (employabilityPlans.length !== 0 || participant.cutOverDate !== null || !canValidateCutOverDate)) {
          //Begin Date cannot be beyond the max days the user can back date for this program
          validationManager.addErrorWithFormat(ValidationCode.DateBeforeMaxDaysCanBackDate, 'EP Begin Date', subtractFromCurrentDate);
          result.addError('beginDate');
        } else if (userInputDate.isAfter(currentDate)) {
          //Begin Date cannot be after the current date
          validationManager.addErrorWithFormat(ValidationCode.DateAfterDate_Name1_Name2, 'EP Begin Date', `current date of ${currentDate.format('MM/DD/YYYY')}`);
          result.addError('beginDate');
        } else if (compareBeginDate !== undefined) {
          if (userInputDate.isBefore(setDate)) {
            //Begin Date cannot be beyond the previously submitted EP begin date
            validationManager.addErrorWithFormat(ValidationCode.DateBeforeDate_Name1_Name2, 'EP Begin Date', 'Previous Submitted EP Begin Date');
            result.addError('beginDate');
          }
        }
        if (moment(convertedcurrentEnrollmentDate).isAfter(userInputDateFormatted)) {
          validationManager.addErrorWithFormat(ValidationCode.DateBeforeProgramEnrollmentDate, 'EP Begin Date', convertedcurrentEnrollmentDate);
          result.addError('beginDate');
        }
      } else if (this.beginDate !== '' && this.beginDate !== null && this.beginDate !== undefined && !userInputDateFormatted.isValid()) {
        validationManager.addErrorWithDetail(ValidationCode.InvalidDate, 'Invalid EP Begin Date Entered');
        result.addError('beginDate');
      }
    }

    const userInputEndDateFormatted = moment(this.endDate, 'MM/DD/YYYY');
    const userInputEndDate = moment(this.endDate);

    if (this.isEpEndDateRequired || !disableFields) {
      if (this.endDate === null || this.endDate === undefined || this.endDate === '') {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'EP End Date');
        result.addError('endDate');
      }
      if (this.endDate !== '' && this.endDate != null && this.endDate.length !== 10) {
        validationManager.addErrorWithFormat(ValidationCode.ValueInInvalidFormat_Name_FormatType, 'EP End Date', 'MM/DD/YYYY');
        result.addError('endDate');
      } else if (
        this.beginDate !== '' &&
        this.beginDate &&
        this.beginDate.length === 10 &&
        userInputEndDateFormatted.isValid() &&
        userInputDateFormatted.isValid() &&
        !disableFields
      ) {
        if (this.endDate !== undefined) {
          if (userInputEndDate.isSame(userInputDate)) {
            validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, 'EP End Date cannot be on or before EP Begin Date.');
            result.addError('endDate');
          }
          if (userInputEndDate.isBefore(userInputDate)) {
            validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, 'EP End Date cannot be on or before EP Begin Date.');
            result.addError('endDate');
          }
          if (userInputEndDate.isAfter(userInputDate.add(6, 'months'))) {
            validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, 'EP End Date cannot be more than six months after EP Begin Date.');
            result.addError('endDate');
          }
        }
      } else if (this.endDate !== '' && this.endDate !== null && this.endDate !== undefined && !userInputEndDateFormatted.isValid()) {
        validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, 'Invalid EP End Date Entered');
        result.addError('endDate');
      }
    }

    if (_.isNil(this.canSaveWithoutActivity)) {
      result.addError('canSaveWithoutActivity');
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Is EP being created without activities?');
    }
    if (!_.isNil(this.canSaveWithoutActivity) && this.canSaveWithoutActivity === true) {
      if (Utilities.isStringEmptyOrNull(this.canSaveWithoutActivityDetails)) {
        result.addError('canSaveWithoutActivityDetails');
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Why is EP being created without activities? - Details');
      }
    }
    return result;
  }
}
