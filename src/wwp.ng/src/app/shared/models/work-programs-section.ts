// tslint:disable: no-use-before-declare
import { Serializable } from '../interfaces/serializable';
import { GoogleLocation } from './google-location';
import { ValidationManager } from './validation-manager';
import { ValidationResult } from './validation-result';
import { DropDownField } from '../../shared/models/dropdown-field';
import { Utilities } from '../utilities';
import { Validate } from '../validate';
import { ValidationCode } from './validation-error';

import { MmYyyyValidationContext } from '../interfaces/mmYyyy-validation-context';

import * as moment from 'moment';
import { AppService } from 'src/app/core/services/app.service';
import * as _ from 'lodash';

export class WorkProgramsSection implements Serializable<WorkProgramsSection> {
  isSubmittedViaDriverFlow: boolean;
  isInOtherPrograms: boolean;
  workPrograms: WorkProgram[];
  cwwFsetStatus: CwwFsetStatus;
  notes: string;
  rowVersion: string;
  modifiedBy: string;
  modifiedDate: string;

  public static clone(input: any, instance: WorkProgramsSection) {
    instance.isSubmittedViaDriverFlow = input.isSubmittedViaDriverFlow;
    instance.isInOtherPrograms = input.isInOtherPrograms;
    instance.workPrograms = Utilities.deserilizeChildren(input.workPrograms, WorkProgram, 0);
    instance.cwwFsetStatus = Utilities.deserilizeChild(input.cwwFsetStatus, CwwFsetStatus);
    instance.notes = input.notes;
    instance.rowVersion = input.rowVersion;
    instance.modifiedBy = input.modifiedBy;
    instance.modifiedDate = input.modifiedDate;
  }

  public deserialize(input: any) {
    WorkProgramsSection.clone(input, this);
    return this;
  }
  // this sorting is implemented based on this ticket https://github.com/uniqua1303/wwp/issues/2457
  public defaultSort(workPrograms: WorkProgram[]): WorkProgram[] {
    // Adding two new values endDateForSort, startDateForSort to get a valid date from 'MM/YYYY'
    const workProgramsClone = workPrograms.map(wp => {
      if (wp.endDate != null && wp.endDate.trim() !== '') {
        let endArr = wp.endDate.split('/');
        wp.endDateForSort = new Date(moment(['01', ...endArr].join('/'), 'DD/MM/YYYY').format('MM/DD/YYYY'));
      } else {
        wp.endDateForSort = null;
      }
      if (wp.startDate != null && wp.startDate.trim() !== '') {
        let strArr = wp.startDate.split('/');
        wp.startDateForSort = new Date(moment(['01', ...strArr].join('/'), 'DD/MM/YYYY').format('MM/DD/YYYY'));
      } else {
        wp.startDateForSort = null;
      }

      return wp;
    });
    let currentWP = workProgramsClone.filter(wp => wp.workStatusName === 'Current');

    const currentWPWithNullEndDate = _.orderBy(
      _.remove(currentWP, wp => wp.endDateForSort === null),
      'startDateForSort',
      'desc'
    );
    currentWP = _.orderBy(currentWP, 'startDateForSort', 'desc');
    currentWP = [...currentWPWithNullEndDate, ...currentWP];
    let waitListWP = workProgramsClone.filter(wp => wp.workStatusName === 'Waitlist');
    const bothStartandEndDateNull = _.remove(waitListWP, wp => wp.endDateForSort === null && wp.startDateForSort === null);
    const nullEndDate = _.orderBy(
      _.remove(waitListWP, wp => wp.endDateForSort === null),
      'startDateForSort',
      'desc'
    );

    waitListWP = _.orderBy(waitListWP, 'endDateForSort', 'asc');
    waitListWP = [...waitListWP, ...nullEndDate, ...bothStartandEndDateNull];
    let pastWP = workProgramsClone.filter(wp => wp.workStatusName === 'Past');
    pastWP = _.orderBy(pastWP, 'endDateForSort', 'desc');
    const sorted = [...currentWP, ...waitListWP, ...pastWP];
    return sorted;
  }

  public validate(validationManager: ValidationManager, workProgramStatusesDrop: DropDownField[], otherId: number, participantDOB: moment.Moment): ValidationResult {
    const result = new ValidationResult();

    Utilities.validateRequiredYesNo(
      result,
      validationManager,
      this.isInOtherPrograms,
      'isInOtherPrograms',
      'Have you, are you currently, or will you be involved in any other work programs?'
    );

    if (this.isInOtherPrograms === true && this.isInOtherPrograms != null) {
      const errArr = result.createErrorsArray('workPrograms');

      let allItemsAreEmpty = true;

      // Check to see if all are empty.
      for (const wp of this.workPrograms) {
        if (!wp.isEmpty()) {
          allItemsAreEmpty = false;
          break;
        }
      }

      if (allItemsAreEmpty) {
        // If all are empty validate the first one.
        if (this.workPrograms[0] != null || this.workPrograms.length === 0) {
          if (this.workPrograms.length === 0) {
            const x = WorkProgram.create();
            this.workPrograms.push(x);
          }
          const v = this.workPrograms[0].validate(validationManager, workProgramStatusesDrop, otherId, participantDOB, result);
          errArr.push(v.errors);
          if (v.isValid === false) {
            result.isValid = false;
          }
        }
      } else {
        for (const wp of this.workPrograms) {
          if (!wp.isEmpty()) {
            const v = wp.validate(validationManager, workProgramStatusesDrop, otherId, participantDOB);
            errArr.push(v.errors);
            if (v.isValid === false) {
              result.isValid = false;
            }
          } else {
            // Push empty when item is blank in order to keep correct index.
            const resultEmpty = new ValidationResult();
            errArr.push(resultEmpty.errors);
          }
        }
      }
    }
    return result;
  }
}

export class WorkProgram implements Serializable<WorkProgram> {
  id: number;
  workStatus: number;
  workStatusName: string;
  workProgram: number;
  workProgramName: string;
  startDate: string;
  endDate: string;
  contactId: number;
  endDateForSort: Date;
  startDateForSort: Date;
  location: GoogleLocation;
  details: string;

  public static create() {
    const wp = new WorkProgram();
    wp.id = 0;
    return wp;
  }

  public static clone(input: any, instance: WorkProgram) {
    instance.id = input.id;
    instance.startDate = input.startDate;
    instance.endDate = input.endDate;
    instance.contactId = input.contactId;
    instance.location = Utilities.deserilizeChild(input.location, GoogleLocation);
    instance.details = input.details;
    instance.workProgram = input.workProgram;
    instance.workProgramName = input.workProgramName;
    instance.workStatus = input.workStatus;
    instance.workStatusName = input.workStatusName;
  }

  public deserialize(input: any) {
    WorkProgram.clone(input, this);
    return this;
  }

  public clear(): void {
    this.id = null;
    this.workStatus = null;
    this.workStatusName = null;
    this.workProgram = null;
    this.workProgramName = null;
    this.startDate = null;
    this.endDate = null;
    this.contactId = null;
    this.location = new GoogleLocation();
    this.details = null;
  }

  isRequired(i: number): boolean {
    if (
      (this.startDate != null && this.startDate.length > 0) ||
      (this.endDate != null && this.endDate.length > 0) ||
      this.details != null ||
      this.workStatus != null ||
      this.workProgram != null ||
      this.location != null ||
      i === 0
    ) {
      return true;
    } else {
      return false;
    }
  }

  isStartDateRequired(workStatusPastId: number, workStatusCurrentId: number, workStatusWaitlistId: number): boolean {
    if (workStatusPastId == null && workStatusCurrentId == null && workStatusWaitlistId == null && this.workStatus == null) {
      return false;
    } else if ((this.workStatus != null && Number(this.workStatus) === workStatusCurrentId) || Number(this.workStatus) === workStatusPastId) {
      return true;
    } else if (this.workStatus != null && Number(this.workStatus) === workStatusWaitlistId) {
      return false;
    } else {
      // console.warn('Could not find if Start Date is required');
      return false;
    }
  }

  isEndDateRequired(workStatusPastId: number, workStatusCurrentId: number, workStatusWaitlistId: number): boolean {
    if (workStatusPastId == null && workStatusCurrentId == null && workStatusWaitlistId == null && this.workStatus == null) {
      return false;
    } else if (this.workStatus != null && Number(this.workStatus) === workStatusPastId) {
      return true;
    } else if ((this.workStatus != null && Number(this.workStatus) === workStatusCurrentId) || Number(this.workStatus) === workStatusWaitlistId) {
      return false;
    } else {
      // console.warn('Could not find if End Date is required');
      return false;
    }
  }

  isDetailsRequired(otherId: number): boolean {
    if (this.workProgram === otherId) {
      return true;
    } else {
      return false;
    }
  }

  private whichWorkStatusName(workProgramStatusesDrop: DropDownField[]): string {
    const pastStatusId = Utilities.idByFieldDataName('Past', workProgramStatusesDrop);
    const currentStatusId = Utilities.idByFieldDataName('Current', workProgramStatusesDrop);
    const waitlistStatusId = Utilities.idByFieldDataName('Waitlist', workProgramStatusesDrop);

    if (+this.workStatus === pastStatusId) {
      return 'past';
    }

    switch (+this.workStatus) {
      case pastStatusId:
        return 'past';
      case currentStatusId:
        return 'current';
      case waitlistStatusId:
        return 'waitlist';
      default:
        return;
    }
  }

  public validate(
    validationManager: ValidationManager,
    workProgramStatusesDrop: DropDownField[],
    otherId: number,
    participantDOB: moment.Moment,
    result?: ValidationResult
  ): ValidationResult {
    if (result == null) {
      result = new ValidationResult();
    }

    // Check for required fields: status
    Utilities.validateDropDown(this.workStatus, 'workStatus', 'Status', result, validationManager);

    // Check for required fields: work program Name
    Utilities.validateDropDown(this.workProgram, 'workProgram', 'Name', result, validationManager);

    // Check for required fields: location
    Validate.googleLocation(this.location, 'location', 'Location', result, validationManager);

    // Required when Name field is other.
    if (this.isDetailsRequired(otherId)) {
      Utilities.validateRequiredText(this.details, 'details', 'Details', result, validationManager);
    }

    let isStartDateValid = true;
    let isEndDateValid = true;
    let maxStartDate = '';
    let maxStartDateName = '';
    let maxEndDate: string = null;
    let maxEndDateName = '';
    let minEndDate: string = participantDOB.format('MM/DD/YYYY');
    const minDate = participantDOB.format('MM/DD/YYYY');
    let minEndDateName = "Participant's DOB";
    const minDateName = "Participant's DOB";

    let isStartDateRequired = false;
    let isEndDateRequired = false;

    // Switch for determing max dates.
    switch (this.whichWorkStatusName(workProgramStatusesDrop)) {
      case 'past':
        maxStartDate = Utilities.currentDate.date(1).format('MM/DD/YYYY');
        maxStartDateName = 'current date';
        isStartDateRequired = true;
        maxEndDate = Utilities.currentDate.date(1).format('MM/DD/YYYY');
        maxEndDateName = 'current date';
        isEndDateRequired = true;
        break;
      case 'current':
        maxStartDate = Utilities.currentDate.date(1).format('MM/DD/YYYY');
        maxStartDateName = 'current date';
        minEndDate = Utilities.currentDate.date(1).format('MM/DD/YYYY');
        minEndDateName = 'current date';
        isStartDateRequired = true;
        isEndDateRequired = false;
        break;
      case 'waitlist':
        break;
    }

    const startDateValidationContext: MmYyyyValidationContext = {
      date: this.startDate,
      prop: 'startDate',
      prettyProp: 'Start Date',
      result: result,
      validationManager: validationManager,
      isRequired: isStartDateRequired,
      minDate: minDate,
      minDateAllowSame: null,
      minDateName: minDateName,
      maxDate: maxStartDate,
      maxDateAllowSame: true,
      maxDateName: maxStartDateName,
      participantDOB: participantDOB
    };
    isStartDateValid = Utilities.validateMmYyyyDate(startDateValidationContext);

    const endDateValidationContext: MmYyyyValidationContext = {
      date: this.endDate,
      prop: 'endDate',
      prettyProp: 'End Date',
      result: result,
      validationManager: validationManager,
      isRequired: isEndDateRequired,
      minDate: minEndDate,
      minDateAllowSame: true,
      minDateName: minEndDateName,
      maxDate: maxEndDate,
      maxDateAllowSame: true,
      maxDateName: maxEndDateName,
      participantDOB: participantDOB
    };
    isEndDateValid = Utilities.validateMmYyyyDate(endDateValidationContext);

    if (isStartDateValid === true && isEndDateValid === true) {
      // Start must before end date.
      if (moment(this.endDate, 'MM/YYYY').date(1) < moment(this.startDate, 'MM/YYYY').date(1)) {
        validationManager.addErrorWithFormat(
          ValidationCode.DatesOutOfOrder_Date1_Date2,
          moment(this.startDate, 'MM/YYYY').format('MM/YYYY'),
          moment(this.endDate, 'MM/YYYY').format('MM/YYYY')
        );
        result.addError('startDate');
        result.addError('endDate');
      }
    }

    return result;
  }

  public isEmpty(): boolean {
    return (
      (this.workStatus == null || this.workStatus.toString() === '') &&
      (this.workProgram == null || this.workProgram.toString() === '') &&
      (this.startDate == null || this.startDate.trim() === '') &&
      (this.endDate == null || this.endDate.trim() === '') &&
      (this.location == null || this.location.description == null || this.location.description.trim() === '') &&
      (this.details == null || this.details.trim() === '') &&
      (this.contactId == null || this.contactId.toString() === '')
    );
  }

  isValid() {
    console.warn('Not Implemented');
  }
}

export class CwwFsetStatus implements Serializable<CwwFsetStatus> {
  currentStatusCode: string;
  enrollmentDate: string;
  disenrollmentDate: string;
  disenrollmentReasonCode: string;

  public deserialize(input: any): CwwFsetStatus {
    this.currentStatusCode = input.currentStatusCode;
    this.enrollmentDate = input.enrollmentDate;
    this.disenrollmentDate = input.disenrollmentDate;
    this.disenrollmentReasonCode = input.disenrollmentReasonCode;

    return this;
  }
}
