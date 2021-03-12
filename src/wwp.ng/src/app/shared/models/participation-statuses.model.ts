// tslint:disable: no-shadowed-variable
import { Serializable } from '../interfaces/serializable';
import * as moment from 'moment';
import { ValidationManager } from './validation-manager';
import { ValidationResult } from './validation-result';
import { ValidationCode } from './validation';
import { EnrolledProgram } from './enrolled-program.model';
import * as _ from 'lodash';
import { Utilities } from '../utilities';
import { DropDownMultiField } from './dropdown-multi-field';
import { EnrolledProgramCode } from '../enums/enrolled-program-code.enum';

export class ParticipationStatus implements Serializable<ParticipationStatus> {
  id: number;
  participantId: number;
  pin: number;
  statusId: number;
  statusName: string;
  statusCode: string;
  details: string;
  beginDate: string;
  endDate: string;
  isCurrent: boolean;
  enrolledProgramId: number;
  enrolledProgramName: string;
  enrolledProgramCode: string;
  rowVersion: string;
  modifiedBy: string;
  modifiedDate: string;
  isCurrentError = false;

  public static clone(input: any, instance: ParticipationStatus) {
    instance.id = input.id;
    instance.participantId = input.participantId;
    instance.pin = input.pin;
    instance.statusId = input.statusId;
    instance.details = input.details;
    instance.beginDate = input.beginDate;
    instance.endDate = input.endDate;
    instance.isCurrent = input.isCurrent;
    instance.enrolledProgramId = input.enrolledProgramId;
    instance.enrolledProgramName = input.enrolledProgramName;
    instance.enrolledProgramCode = input.enrolledProgramCode;
    instance.statusCode = input.statusCode;
    instance.statusName = input.statusName;
    instance.rowVersion = input.rowVersion;
    instance.modifiedBy = input.modifiedBy;
    instance.modifiedDate = input.modifiedDate;
  }

  public deserialize(input: any) {
    ParticipationStatus.clone(input, this);
    return this;
  }
  public CurrentTATEStatus(participationStatuses: ParticipationStatus[], modelIds: any, currentProgram: EnrolledProgram) {
    if (participationStatuses && !_.isEmpty(participationStatuses)) {
      return participationStatuses.some(
        p => p.isCurrent && (p.statusId === modelIds.TAId || p.statusId === modelIds.TEId) && p.enrolledProgramId === currentProgram.enrolledProgramId
      );
    } else {
      return null;
    }
  }

  public validate(
    validationManager: ValidationManager,
    participantDOB: string,
    participationStatuses: ParticipationStatus[],
    selectedStatusCodeName,
    numberOfDaysCanBackDate,
    numberOfDaysCanBackDateForEndDate: string,
    currentlyEnrolledPrograms: EnrolledProgram[],
    moelId: number,
    currentProgram: EnrolledProgram,
    modelIds,
    pullDownDates: DropDownMultiField[],
    enrolledProgramCd: string,
    statusCd
  ): ValidationResult {
    const result = new ValidationResult();
    let overlappedPS: any;
    const dateRanges: { start: any; end: any }[] = [];
    if (participationStatuses && participationStatuses.length > 0 && currentProgram) {
      participationStatuses.forEach(i => {
        if (i.enrolledProgramId === currentProgram.enrolledProgramId && i.statusId === this.statusId && i.id !== this.id) {
          const endDate = i.isCurrent ? new Date(Utilities.currentDate.toISOString()) : new Date(i.endDate);
          dateRanges.push({ start: new Date(i.beginDate), end: new Date(endDate) });
        }
      });
      const endDate = this.isCurrent ? new Date(Utilities.currentDate.toISOString()) : new Date(this.endDate);
      dateRanges.push({ start: new Date(this.beginDate), end: new Date(endDate) });
      overlappedPS = Utilities.datesOverlap(dateRanges);
    }

    if (moelId === 0) {
      if (this.statusId == null || this.statusId.toString() === '') {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Status');
        result.addError('statusId');
      }
      Utilities.dateFormatValidation(validationManager, result, this.beginDate, 'Begin Date', 'beginDate');
      const inputDate = moment(this.beginDate, 'MM/DD/YYYY', true);
      const formattedInputDateForIE = moment(this.beginDate).format('MM/DD/YYYY');
      const DOB = moment(participantDOB).format('MM/DD/YYYY');
      const compareDate = moment(Utilities.currentDate.subtract(numberOfDaysCanBackDate, 'days').format('MM/DD/YYYY'));
      if (inputDate.isValid() && this.enrolledProgramId) {
        const currentProgram = currentlyEnrolledPrograms.find(x => +x.enrolledProgramId === +this.enrolledProgramId);
        const currentProgramEnrollmentDate = moment(currentProgram.enrollmentDate).format('MM/DD/YYYY');
        if (enrolledProgramCd && enrolledProgramCd.trim().toLowerCase() === EnrolledProgramCode.ww && Utilities.participationPeriodCheck(pullDownDates, inputDate, true)) {
          validationManager.addErrorWithFormat(ValidationCode.ParticipationPeriodCheck, 'Begin', 'first', 'current');
          result.addError('beginDate');
        }
        if (moment(currentProgramEnrollmentDate).isAfter(this.beginDate)) {
          validationManager.addErrorWithFormat(ValidationCode.DateBeforeProgramEnrollmentDate, 'Begin Date', currentProgramEnrollmentDate);
          result.addError('beginDate');
        }
        // Check Min Date.
        if (inputDate.isBefore(moment(DOB))) {
          validationManager.addErrorWithFormat(ValidationCode.ValueBeforeDOB_Name_Value_DOB, 'Begin Date', formattedInputDateForIE, DOB);
          result.addError('beginDate');
        }
        if (moment(inputDate, 'MM/DD/YYYY', true).isAfter(Utilities.currentDate.format('MM/DD/YYYY'))) {
          validationManager.addErrorWithFormat(ValidationCode.DateAfterCurrent, 'Begin Date', formattedInputDateForIE, Utilities.currentDate.format('MM/DD/YYYY'));
          result.addError('beginDate');
        }
        if (moment(inputDate, 'MM/DD/YYYY', true).isBefore(compareDate)) {
          validationManager.addErrorWithFormat(ValidationCode.DateBeforeMaxDaysCanBackDate, 'Begin Date', numberOfDaysCanBackDate);
          result.addError('beginDate');
        }
      }
    }

    if (participationStatuses && !_.isEmpty(participationStatuses) && this.statusId && this.isCurrent && this.id === 0) {
      if (
        this.statusId !== modelIds.TAId &&
        this.statusId !== modelIds.TEId &&
        participationStatuses.some(ps => ps.statusId === this.statusId && ps.isCurrent === true && ps.enrolledProgramId === currentProgram.enrolledProgramId)
      ) {
        validationManager.addErrorWithDetail(
          ValidationCode.DuplicateDataWithNoMessage,
          `Current (${selectedStatusCodeName}) status exists, end the status to add a new (${selectedStatusCodeName}) status.`
        );
        this.isCurrentError = true;
        result.addError('statusId');
      } else if (this.CurrentTATEStatus(participationStatuses, modelIds, currentProgram)) {
        this.isCurrentError = true;
        validationManager.addErrorWithDetail(ValidationCode.DuplicateDataWithNoMessage, `Current TA/TE status exists, end the status to add a new TA/TE status.`);
        result.addError('statusId');
      } else this.isCurrentError = false;
    } else this.isCurrentError = false;

    if (
      this &&
      this.beginDate &&
      moment(this.beginDate, 'MM/DD/YYYY', true).isValid() &&
      ((this.endDate && moment(this.endDate, 'MM/DD/YYYY', true).isValid()) || this.isCurrent) &&
      overlappedPS &&
      overlappedPS.ranges &&
      overlappedPS.ranges.length > 0 &&
      overlappedPS.ranges[0].previous &&
      overlappedPS.ranges[0].current &&
      overlappedPS.overlap &&
      participationStatuses &&
      !_.isEmpty(participationStatuses) &&
      this.statusId &&
      participationStatuses.some(ps => ps.statusId === this.statusId && ps.enrolledProgramId === currentProgram.enrolledProgramId) &&
      !this.isCurrentError
    ) {
      const beginDate =
        moment(overlappedPS.ranges[0].previous.start).format('MM/DD/YYYY') === moment(this.beginDate).format('MM/DD/YYYY')
          ? moment(overlappedPS.ranges[0].current.start).format('MM/DD/YYYY')
          : moment(overlappedPS.ranges[0].previous.start).format('MM/DD/YYYY');
      const endDate =
        moment(overlappedPS.ranges[0].previous.end).format('MM/DD/YYYY') === moment(this.isCurrent ? Utilities.currentDate : this.endDate).format('MM/DD/YYYY')
          ? moment(overlappedPS.ranges[0].current.end).format('MM/DD/YYYY')
          : moment(overlappedPS.ranges[0].previous.end).format('MM/DD/YYYY');
      const errMsg =
        endDate === moment(Utilities.currentDate).format('MM/DD/YYYY')
          ? `Current ${selectedStatusCodeName} status exists with Begin Date ${beginDate}. You cannot have overlapping dates for the same status.`
          : `${selectedStatusCodeName} status previously existed with Begin Date ${beginDate} and End Date  ${endDate}. You cannot have overlapping dates for the same status.`;
      validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, errMsg);
      result.addError('endDate');
      result.addError('beginDate');
    }

    // End Date is a required field.

    if (this.isCurrent !== true) {
      const inputDate = moment(this.endDate, 'MM/DD/YYYY', true);
      const formattedEndDateForIE = moment(this.endDate).format('MM/DD/YYYY');
      const DOB = moment(participantDOB).format('MM/DD/YYYY');
      const compareDate = moment(Utilities.currentDate.subtract(numberOfDaysCanBackDateForEndDate, 'days').format('MM/DD/YYYY'));
      Utilities.dateFormatValidation(validationManager, result, this.endDate, 'End Date', 'endDate');
      if (inputDate.isValid()) {
        // Check Min Date.
        if (inputDate.isBefore(participantDOB)) {
          validationManager.addErrorWithFormat(ValidationCode.ValueBeforeDOB_Name_Value_DOB, 'End Date', formattedEndDateForIE, DOB);
          result.addError('endDate');
        }
        if (moment(this.beginDate).isValid() && inputDate.isBefore(this.beginDate)) {
          validationManager.addErrorWithFormat(ValidationCode.DateBeforeDate_Name1_Name2, 'End Date', 'Begin Date');
          result.addError('endDate');
        }
        if (inputDate.isAfter(Utilities.currentDate.format('MM/DD/YYYY'))) {
          validationManager.addErrorWithFormat(ValidationCode.DateAfterCurrent, 'End Date', formattedEndDateForIE, Utilities.currentDate.format('MM/DD/YYYY'));
          result.addError('endDate');
        }
        if (moment(inputDate, 'MM/DD/YYYY', true).isBefore(compareDate)) {
          validationManager.addErrorWithFormat(ValidationCode.DateBeforeMaxDaysCanBackDate, 'End Date', numberOfDaysCanBackDateForEndDate);
          result.addError('endDate');
        }
        if (
          this.id !== 0 &&
          this.enrolledProgramName &&
          this.enrolledProgramName.trim().toLowerCase() === EnrolledProgramCode.w2name &&
          Utilities.participationPeriodCheck(pullDownDates, inputDate)
        ) {
          validationManager.addErrorWithFormat(ValidationCode.ParticipationPeriodCheck, 'End', 'last', 'prior');
          result.addError('endDate');
        }
      }
    }

    return result;
  }
}
