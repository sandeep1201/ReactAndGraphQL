import { Utilities } from './../../../shared/utilities';
import { ValidationResult, ValidationManager, ValidationCode } from 'src/app/shared/models/validation';
import * as moment from 'moment';
import { Participant } from 'src/app/shared/models/participant';
import { Employment } from 'src/app/shared/models/work-history-app';
export class WeeklyHoursWorked {
  public id: number;
  public employmentInformationId: number;
  public startDate: string;
  public hours: string;
  public details: string;
  public totalSubsidyAmount: number;
  public totalWorkSiteAmount: number;
  public isDeleted: boolean;
  public modifiedBy: string;
  public modifiedDate: string;
  public maximumLifeTimeSubsidizedHoursAllowed = 1040;

  public static clone(input: any, instance: WeeklyHoursWorked) {
    instance.id = input.id;
    instance.employmentInformationId = input.employmentInformationId;
    instance.startDate = input.startDate;
    instance.hours = input.hours ? (Math.round(input.hours * 100) / 100).toFixed(2) : input.hours;
    instance.details = input.details;
    if (input.totalSubsidyAmount && typeof input.totalSubsidyAmount === 'string') {
      input.totalSubsidyAmount = +input.totalSubsidyAmount;
    }
    instance.totalSubsidyAmount = input.totalSubsidyAmount ? input.totalSubsidyAmount.toFixed(2) : input.totalSubsidyAmount;
    if (input.totalWorkSiteAmount && typeof input.totalWorkSiteAmount === 'string') {
      input.totalWorkSiteAmount = +input.totalWorkSiteAmount;
    }
    instance.totalWorkSiteAmount = input.totalWorkSiteAmount ? input.totalWorkSiteAmount.toFixed(2) : input.totalWorkSiteAmount;
    instance.isDeleted = input.isDeleted;
    instance.modifiedBy = input.modifiedBy;
    instance.modifiedDate = input.modifiedDate;
    return this;
  }

  public isDetailsRequired() {
    if (this.hours && +this.hours < 20) {
      return true;
    } else {
      return false;
    }
  }

  public deserialize(input: any) {
    WeeklyHoursWorked.clone(input, this);
    return this;
  }

  public checkForInvalidDate(result, validationManager, inputDate: string, prop: string) {
    if (!moment(inputDate, 'MM/DD/YYYY', true).isValid()) {
      validationManager.addErrorWithDetail(ValidationCode.InvalidDate, `Invalid ${prop} Entered.`);
      result.addError('startDate');
    }
  }
  public checkIfTheDateIsFutureDate(result, validationManager, inputDate: string, prop: string, message: string) {
    if (moment(inputDate, 'MM/DD/YYYY', true).isAfter(Utilities.currentDate, 'day')) {
      validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, `The ${prop} ${message}`);
      result.addError('startDate');
    }
  }
  public checkIfDateisSundayAndNotBeforeTheSundayFromInputDate(result, validationManager, inputDate: string, prop: string, message: string) {
    if (moment(moment(this.startDate).format('MM/DD/YYYY'), 'MM/DD/YYYY').weekday() === 0) {
      if (moment(this.startDate, 'MM/DD/YYYY').isBefore(moment(moment(inputDate).format('MM/DD/YYYY'), 'MM/DD/YYYY').startOf('week'))) {
        validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, `${message} ${moment(inputDate).format('MM/DD/YYYY')}.`);
        result.addError('startDate');
      }
    } else {
      validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, `${prop} cannot be any other day other than Sunday.`);
      result.addError('startDate');
    }
  }
  public checkIfDateisSundayAndNotAfterTheSundayFromJobEndDate(result, validationManager, inputDate: string, prop: string, message: string) {
    if (moment(moment(this.startDate).format('MM/DD/YYYY'), 'MM/DD/YYYY').weekday() === 0) {
      if (moment(this.startDate, 'MM/DD/YYYY').isAfter(moment(moment(inputDate).format('MM/DD/YYYY'), 'MM/DD/YYYY').startOf('week'))) {
        validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, `${message} ${moment(inputDate).format('MM/DD/YYYY')}.`);
        result.addError('startDate');
      }
    } else {
      validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, `${prop} cannot be any other day other than Sunday.`);
      result.addError('startDate');
    }
  }

  public checkIfThereIsAlreadyAnEntryForTheWeek(
    result,
    validationManager: ValidationManager,
    weeklyHoursWorkedEntries: WeeklyHoursWorked[],
    inputDate: string,
    idOfTheRecord: number,
    prop: string,
    message: string
  ) {
    if (idOfTheRecord === 0 && weeklyHoursWorkedEntries.some(i => moment(moment(i.startDate).format('MM/DD/YYYY')).isSame(inputDate, 'day'))) {
      validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, `This ${prop} ${message}`);
      result.addError('startDate');
    }
  }
  validate(validationManager: ValidationManager, participant: Participant, employment: Employment, weeklyHoursWorkedEntries: WeeklyHoursWorked[]): ValidationResult {
    const result = new ValidationResult();
    Utilities.validatePropForNullAndEmptyValues(result, validationManager, this.startDate, 'startDate', 'Start Date');
    Utilities.validatePropForNullAndEmptyValues(result, validationManager, this.hours, 'hours', 'Hours');

    if (!result.errors.startDate) {
      this.checkForInvalidDate(result, validationManager, this.startDate, 'Start Date');
      if (!result.errors.startDate) {
        this.checkIfDateisSundayAndNotBeforeTheSundayFromInputDate(
          result,
          validationManager,
          participant.programs[0].enrollmentDate,
          'Start Date',
          'The Start Date field cannot be prior to the Sunday before the enrollment date'
        );
      }
      if (!result.errors.startDate) {
        this.checkIfDateisSundayAndNotBeforeTheSundayFromInputDate(
          result,
          validationManager,
          employment.jobBeginDate,
          'Start Date',
          'The Start Date field cannot be prior to the Sunday before the TJ/TMJ Subsidized job Begin Date'
        );
      }
      if (!result.errors.startDate) {
        this.checkIfTheDateIsFutureDate(result, validationManager, this.startDate, 'Start Date', 'field cannot be after Current Date.');
      }
      if (!result.errors.startDate) {
        this.checkIfThereIsAlreadyAnEntryForTheWeek(
          result,
          validationManager,
          weeklyHoursWorkedEntries,
          this.startDate,
          this.id,
          'Start Date',
          'already exists. You cannot have the same weekly hours worked Start Date for a job more than once.'
        );
      }
      if (!result.errors.startDate && employment.jobEndDate) {
        this.checkIfDateisSundayAndNotAfterTheSundayFromJobEndDate(
          result,
          validationManager,
          employment.jobEndDate,
          'Start Date',
          `The ${employment.jobTypeName} job End Date is ${moment(employment.jobEndDate).format('MM/DD/YYYY')}. You cannot enter Weekly Hours with a Start Date after`
        );
      }
    }
    if (!result.errors.hours) {
      if (+this.hours > 40) {
        validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, 'Hours cannot be more than 40.');
        result.addError('hours');
      }
      const decimalPartOfHours = this.hours.split('.')[1];
      if (+decimalPartOfHours !== 0 && +decimalPartOfHours !== 25 && +decimalPartOfHours !== 50 && +decimalPartOfHours !== 75) {
        validationManager.addErrorWithDetail(ValidationCode.InformationIncorrect, `"Hours" must end with one of the following values after the decimal "00","25","50" or "75".`);
        result.addError('hours');
      }
    }
    if (this.isDetailsRequired() && Utilities.stringIsNullOrWhiteSpace(this.details)) {
      validationManager.addErrorWithDetail(
        ValidationCode.ValueOutOfRange_Details,
        `Total hours worked for the week require ${employment.jobTypeName} job be at least 20.00 hours. Please explain why this was not met.`
      );
      result.addError('details');
    }
    return result;
  }
}
