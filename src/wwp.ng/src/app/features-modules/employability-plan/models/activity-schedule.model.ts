// tslint:disable: radix
// tslint:disable: triple-equals
import { Serializable } from '../../../shared/interfaces/serializable';
import { ValidationManager } from '../../../shared/models/validation-manager';
import { ValidationResult } from '../../../shared/models/validation-result';
import { Utilities } from '../../../shared/utilities';
import { ValidationCode } from '../../../shared/models/validation';
import { Clearable } from '../../../shared/interfaces/clearable';
import { IsEmpty } from '../../../shared/interfaces/is-empty';
import * as moment from 'moment';
import { ActivityScheduleFrequency } from './activity-schedulefrequencies.model';

export class ActivitySchedule implements Clearable, IsEmpty, Serializable<ActivitySchedule> {
  public id: number;
  public scheduleStartDate: string;
  public isRecurring = false;
  public frequencyTypeId: string;
  public frequencyTypeName: string;
  public scheduleEndDate: string;
  public actualEndDate: string;
  public hoursPerDay: string;
  // public details: string;
  public wkFrequencyIds: any[];
  public wkFrequencyId: number;

  public mrFrequencyId: any;

  public beginHour: number;
  public beginMinute: number;
  public beginAmPm: number;
  public endHour: number;
  public endMinute: number;
  public endAmPm: number;
  public employabilityPlanId: number;
  public isTimeRequired: boolean;
  public beginTime: number;
  public endTime: number;
  activityScheduleFrequencies: ActivityScheduleFrequency[];

  public static create(): ActivitySchedule {
    const x = new ActivitySchedule();
    x.id = 0;
    return x;
  }
  public static clone(input: any, instance: ActivitySchedule) {
    instance.id = input.id;
    instance.scheduleStartDate = input.scheduleStartDate;
    instance.isRecurring = input.isRecurring;
    instance.frequencyTypeId = input.frequencyTypeId;
    instance.frequencyTypeName = input.frequencyTypeName;
    instance.scheduleEndDate = input.scheduleEndDate;
    instance.actualEndDate = input.actualEndDate;
    instance.hoursPerDay = input.hoursPerDay;
    instance.beginHour = input.beginHour;
    instance.beginMinute = input.beginMinute;
    instance.beginAmPm = input.beginAmPm;
    instance.endHour = input.endHour;
    instance.endMinute = input.endMinute;
    instance.endAmPm = input.endAmPm;
    instance.beginTime = input.beginTime;
    (instance.endTime = input.endTime), (instance.employabilityPlanId = input.employabilityPlanId);

    //instance.details = input.details;
    instance.activityScheduleFrequencies = Utilities.deserilizeChildren(input.activityScheduleFrequencies, ActivityScheduleFrequency, 0);
    if (input.activityScheduleFrequencies && input.activityScheduleFrequencies.length > 0) {
      // To-DO: Change Id's to use names instead
      if (input.frequencyTypeId == 2 || input.frequencyTypeId == 3) {
        instance.wkFrequencyIds = (input.activityScheduleFrequencies || []).map(schedule => schedule.wkFrequencyId);
        instance.wkFrequencyId = null;
      } else if (input.frequencyTypeId == 4) {
        instance.wkFrequencyIds = null;
        instance.wkFrequencyId = input.activityScheduleFrequencies.length > 0 ? input.activityScheduleFrequencies[0].wkFrequencyId : '';
      }
      //instance.wkFrequencyIds = (input.activityScheduleFrequencies || []).map(schedule => schedule.wkFrequencyId);
      //instance.wkFrequencyId = input.activityScheduleFrequencies.length > 0 ? input.activityScheduleFrequencies[0].wkFrequencyId : '';
      instance.mrFrequencyId = input.activityScheduleFrequencies.length > 0 ? input.activityScheduleFrequencies[0].mrFrequencyId : '';
    }
  }
  public deserialize(input: any) {
    ActivitySchedule.clone(input, this);
    return this;
  }
  public clear(): void {
    this.id = null;
    this.scheduleStartDate = null;
    // this.details = null;
  }

  public isEmpty(): boolean {
    // All properties have to be null or blank to make the entire object empty.
    return this.id == null && this.scheduleStartDate == null;
  }

  // Validations for activity scheduler
  validate(validationManager: ValidationManager, hours, minutes, employabilityPlanBeginDate: string, isCarriedOver: boolean): ValidationResult {
    const result = new ValidationResult();
    if (this.scheduleStartDate === null || this.scheduleStartDate === '') {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Start Date');
      result.addError('scheduleStartDate');
    }
    const minDate = employabilityPlanBeginDate;
    const ssdMinDate = moment(minDate);
    const ssd = moment(this.scheduleStartDate);
    //Dates are being parsed differently in IE so formatting the object instead of converting it to a string.
    //If we convert it to a string, we can't use isValid() function on the string to check if the date is valid.
    const formatSsd = moment(this.scheduleStartDate, 'MM/DD/YYYY');
    const inputDate = moment(this.scheduleStartDate);
    const ssdDate = moment(minDate);
    const ssd6Months = ssdDate.add(6, 'months');
    if (this.scheduleStartDate === null || this.scheduleStartDate === undefined || this.scheduleStartDate === '') {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Start Date');
      result.addError('scheduleStartDate');
    }
    if (this.scheduleStartDate !== null && this.scheduleStartDate !== undefined && this.scheduleStartDate !== '' && this.scheduleStartDate.length !== 10) {
      validationManager.addErrorWithFormat(ValidationCode.ValueInInvalidFormat_Name_FormatType, 'Start Date', 'MM/DD/YYYY');
      result.addError('scheduleStartDate');
    } else if (this.scheduleStartDate !== null && this.scheduleStartDate !== undefined && this.scheduleStartDate !== '' && formatSsd.isValid()) {
      if (inputDate.isBefore(ssdMinDate)) {
        validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, 'Start Date cannot be before EP Begin Date.');
        result.addError('scheduleStartDate');
      } else if (inputDate.isAfter(ssd6Months)) {
        validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, 'Start Date should be within 6 months of EP Begin Date.');
        result.addError('scheduleStartDate');
      }
    } else if (this.scheduleStartDate !== null && this.scheduleStartDate !== undefined && this.scheduleStartDate !== '' && !formatSsd.isValid()) {
      validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, 'Invalid Start Date Entered.');
      result.addError('scheduleStartDate');
    }
    if (this.isRecurring) {
      if (this.frequencyTypeId === null || this.frequencyTypeId === undefined) {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Frequency');
        result.addError('frequencyTypeId');
      }
      if (this.frequencyTypeId == '2' || this.frequencyTypeId == '3') {
        Utilities.validateDropDown(this.wkFrequencyIds, 'wkFrequencyIds', 'Days', result, validationManager);
      }

      const freqId = parseInt(this.frequencyTypeId);
      if (freqId === 4 && (this.mrFrequencyId == null || this.mrFrequencyId === undefined)) {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Monthly Recurrence');
        result.addError('mrFrequencyId');
      }

      if (freqId == 4 && (this.wkFrequencyId == null || this.wkFrequencyId == undefined)) {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Day for Monthly Recurrence');
        result.addError('wkFrequencyId');
      }
      // when end date is required
      if (this.scheduleEndDate == undefined || this.scheduleEndDate == null || this.scheduleEndDate == '') {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Planned End Date');
        result.addError('scheduleEndDate');
      } else if (this.scheduleEndDate !== null && this.scheduleEndDate !== undefined && this.scheduleEndDate !== '') {
        const inputDateSed = moment(this.scheduleEndDate);
        //Dates are being parsed differently in IE so formatting the object instead of converting it to a string.
        //If we convert it to a string, we can't use isValid() function on the string to check if the date is valid.
        const formatInputDateSed = moment(this.scheduleEndDate, 'MM/DD/YYYY');

        if (this.scheduleEndDate !== null && this.scheduleEndDate !== undefined && this.scheduleEndDate !== '' && this.scheduleEndDate.length != 10) {
          validationManager.addErrorWithFormat(ValidationCode.ValueInInvalidFormat_Name_FormatType, 'Planned End Date', 'MM/DD/YYYY');
          result.addError('scheduleEndDate');
        } else if (this.scheduleEndDate !== null && this.scheduleEndDate !== undefined && this.scheduleEndDate !== '' && formatInputDateSed.isValid()) {
          if (this.scheduleEndDate !== null && this.scheduleEndDate !== undefined && this.scheduleEndDate !== '' && inputDateSed.isBefore(ssdMinDate)) {
            validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, 'Planned End Date must be on or after EP Begin Date.');
            result.addError('scheduleEndDate');
          }
          if (
            this.scheduleStartDate !== null &&
            this.scheduleStartDate !== undefined &&
            this.scheduleStartDate !== '' &&
            this.scheduleEndDate !== null &&
            this.scheduleEndDate !== undefined &&
            this.scheduleEndDate !== '' &&
            inputDateSed.isSameOrBefore(this.scheduleStartDate) &&
            formatSsd.isValid()
          ) {
            validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, 'Planned End Date must be after Schedule Start Date.');
            result.addError('scheduleEndDate');
          }
        } else if (this.scheduleEndDate !== null && this.scheduleEndDate !== undefined && this.scheduleEndDate !== '' && !formatInputDateSed.isValid()) {
          validationManager.addErrorWithDetail(ValidationCode.InvalidDate, 'Invalid Planned End Date Entered.');
          result.addError('scheduleEndDate');
        }
      }
    }
    if (this.hoursPerDay == null || this.hoursPerDay == '') {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Hours Per Day');
      result.addError('hoursPerDay');
    }
    if (this.hoursPerDay !== undefined && this.hoursPerDay != null && this.hoursPerDay != '' && +this.hoursPerDay > 24.0) {
      validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, 'Hours Per Day must be less than or equal to 24.0.');
      result.addError('hoursPerDay');
    }
    if (this.hoursPerDay !== undefined && this.hoursPerDay != null && this.hoursPerDay != '' && +this.hoursPerDay < 0.5) {
      validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, 'Hours Per Day must be greater than or equal to 0.5.');
      result.addError('hoursPerDay');
    }
    if (this.hoursPerDay && this.hoursPerDay != '' && this.hoursPerDay.includes('.')) {
      const hrsDecimal = this.hoursPerDay.split('.')[1];
      if (+hrsDecimal != 0 && +hrsDecimal != 5) {
        validationManager.addErrorWithDetail(ValidationCode.InformationIncorrect, 'Hours Per Day must be whole or half hours.');
        result.addError('hoursPerDay');
      } else if (hrsDecimal == '') {
        validationManager.addErrorWithDetail(ValidationCode.InformationIncorrect, 'Hours Per Day must end with .0 or .5.');
        result.addError('hoursPerDay');
      }
    } else if (this.hoursPerDay && this.hoursPerDay != '') {
      validationManager.addErrorWithDetail(ValidationCode.InformationIncorrect, 'Hours Per Day must end with .0 or .5.');
      result.addError('hoursPerDay');
    }
    /* Schedule Begin/End TimeValidations */
    if (this.beginHour != undefined || this.beginMinute != undefined || this.endHour != undefined || this.endMinute != undefined) {
      if (this.beginHour == undefined) {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Begin Time Hour');
        result.addError('beginHour');
      }
      if (this.beginMinute == undefined) {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Begin Time Minutes');
        result.addError('beginMinute');
      }
      if (this.beginAmPm == undefined) {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Begin Time AM/PM');
        result.addError('beginAmPm');
      }
      if (this.endHour == undefined) {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'End Time Hour');
        result.addError('endHour');
      }
      if (this.endMinute == undefined) {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'End Time Minutes');
        result.addError('endMinute');
      }
      if (this.endAmPm == undefined) {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'End Time AM/PM');
        result.addError('endAmPm');
      }
      if (
        this.beginHour != undefined &&
        this.beginMinute != undefined &&
        this.endHour != undefined &&
        this.endMinute != undefined &&
        this.beginAmPm != undefined &&
        this.endAmPm != undefined
      ) {
        let beginVal: string;
        let endVal: string;
        if (this.beginAmPm === 1) {
          beginVal = 'AM';
        } else if (this.beginAmPm === 2) {
          beginVal = 'PM';
        }
        if (this.endAmPm === 1) {
          endVal = 'AM';
        } else if (this.endAmPm === 2) {
          endVal = 'PM';
        }
        const convertBeginHr = Utilities.fieldDataNameById(this.beginHour, hours);
        const convertBeginMn = Utilities.fieldDataNameById(this.beginMinute, minutes);
        const convertEndHr = Utilities.fieldDataNameById(this.endHour, hours);
        const convertEndMn = Utilities.fieldDataNameById(this.endMinute, minutes);

        const beginTimeValue = convertBeginHr + ':' + convertBeginMn + beginVal;
        const endTimeValue = convertEndHr + ':' + convertEndMn + endVal;

        if (moment(beginTimeValue, 'hh:mm A').isAfter(moment(endTimeValue, 'hh:mm A'))) {
          validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, 'Begin Time cannot be after End Time.');
          result.addError('Begin Time');
          result.addError('beginHour');
          result.addError('beginMinute');
          result.addError('beginAmPm');
        } else if (moment(endTimeValue, 'hh:mm A').isBefore(moment(beginTimeValue, 'hh:mm A'))) {
          validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, 'End Time cannot be before Begin Time.');
          result.addError('End Time');
          result.addError('endHour');
          result.addError('endMinute');
          result.addError('endAmPm');
        } else if (moment(endTimeValue, 'hh:mm A').isSame(moment(beginTimeValue, 'hh:mm A'))) {
          validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, 'End Time must be after Begin Time.');
          result.addError('End Time');
          result.addError('endHour');
          result.addError('endMinute');
          result.addError('endAmPm');
        }
      }
    }

    return result;
  }
}
