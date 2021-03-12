// tslint:disable: no-output-on-prefix
// tslint:disable: no-use-before-declare
import { Output, EventEmitter } from '@angular/core';
import { DropDownField } from '../../../shared/models/dropdown-field';
import { Utilities } from '../../../shared/utilities';

import { MmDdYyyyValidationContext } from '../../../shared/interfaces/mmDdYyyy-validation-context';
import { ValidationManager } from '../../../shared/models/validation-manager';
import { ValidationCode } from '../../../shared/models/validation-error';
import { ValidationResult } from '../../../shared/models/validation-result';
import { Participant } from '../../../shared/models/participant';
import * as moment from 'moment';
import { AppService } from '../../../core/services/app.service';

export class ActionNeeded {
  static readonly onGoingStatus = 'onGoing';
  id: number;
  pageId: number;
  pageName: string;
  isNoActionNeeded: boolean;
  modifiedBy: string;
  modifiedDate: string;
  rowVersion: string;
  tasks: ActionNeededTask[];
  @Output() onAreAllTasksCompleted = new EventEmitter<boolean>();

  public static clone(input: any, instance: ActionNeeded) {
    instance.id = input.id;
    instance.pageId = input.pageId;
    instance.pageName = input.pageName;
    instance.isNoActionNeeded = input.isNoActionNeeded;
    instance.modifiedBy = input.modifiedBy;
    instance.modifiedDate = input.modifiedDate;
    instance.rowVersion = input.rowVersion;
    instance.tasks = Utilities.deserilizeChildren(input.tasks, ActionNeededTask, 0);
  }

  public deserialize(input: any) {
    ActionNeeded.clone(input, this);
    return this;
  }

  public validate(validationManager: ValidationManager): ValidationResult {
    const result = new ValidationResult();

    if (this.isNoActionNeeded === null || this.isNoActionNeeded === false) {
      if (this.tasks == null || this.tasks.length === 0) {
        result.addError('actionNeeded');
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Action Needed');
      } else {
        let foundOnGoingTask = false;
        for (const task of this.tasks) {
          const tskStatus = task.getTaskStatus();
          if (tskStatus === 'onGoing') {
            foundOnGoingTask = true;
            break;
          }
        }

        // Updated validation to automatically mark isNoActionNeeded true if no ongoing tasks are found per US1434
        if (!foundOnGoingTask) {
          this.onAreAllTasksCompleted.emit((this.isNoActionNeeded = true));
        }
      }
    }

    return result;
  }
}

export class ActionNeededTask {
  static readonly taskStatuses = ['onGoing', 'recentActions', 'completed', 'notCompleted'];
  id: number;
  actionNeededId: number;
  assigneeId: number;
  assigneeName: string;
  pageId: number;
  pageName: string;
  actionItemId: number;
  actionItemName: string;
  followUpTask: string;
  completionDate: string;
  isNoCompletionDate: boolean;
  dueDate: string;
  isNoDueDate: boolean;
  priorityId: number;
  priorityName: string;
  details: string;
  createdDate: string;
  completionDateUnFmt: string;
  dueDateUnFmt: string;
  modifiedDate: string;
  modifiedBy: string;
  modifiedByName: string;

  set completionDateMmDdYyyy(date) {
    if (date != null && date.length === 10 && moment(date).isValid()) {
      this.completionDate = moment(date).toISOString();
    } else {
      this.completionDate = date;
    }
    this.completionDateUnFmt = date;
  }
  get completionDateMmDdYyyy() {
    if (this.completionDateUnFmt) return this.completionDateUnFmt;
    else if (this.completionDate) return moment(this.completionDate, moment.ISO_8601).format('MM/DD/YYYY');
  }

  set dueDateMmDdYyyy(date) {
    if (date != null && date.length === 10 && moment(date).isValid()) {
      this.dueDate = moment(date).toISOString();
    } else {
      this.dueDate = date;
    }
    this.dueDateUnFmt = date;
  }
  get dueDateMmDdYyyy() {
    if (this.dueDateUnFmt) return this.dueDateUnFmt;
    else if (this.dueDate) return moment(this.dueDate, moment.ISO_8601).format('MM/DD/YYYY');
  }

  public static clone(input: any, instance: ActionNeededTask) {
    instance.id = input.id;
    instance.actionNeededId = input.actionNeededId;
    instance.assigneeId = input.assigneeId;
    instance.assigneeName = input.assigneeName;
    instance.pageId = input.pageId;
    instance.pageName = input.pageName;
    instance.actionItemId = input.actionItemId;
    instance.actionItemName = input.actionItemName;
    instance.followUpTask = input.followUpTask;
    instance.completionDate = input.completionDate;
    instance.isNoCompletionDate = input.isNoCompletionDate;
    instance.dueDate = input.dueDate;
    instance.isNoDueDate = input.isNoDueDate;
    instance.priorityId = input.priorityId;
    instance.priorityName = input.priorityName;
    instance.details = input.details;
    instance.createdDate = input.createdDate;
    instance.modifiedBy = input.modifiedBy;
    instance.modifiedDate = input.modifiedDate;
    instance.modifiedByName = input.modifiedByName;
  }

  public static create(): ActionNeededTask {
    const ant = new ActionNeededTask();
    ant.id = 0;
    // Default bools to false;
    ant.isNoCompletionDate = false;
    ant.isNoDueDate = false;
    return ant;
  }

  public deserialize(input: any) {
    ActionNeededTask.clone(input, this);
    return this;
  }

  public validate(validationManager: ValidationManager, pages: DropDownField[], actionItems: DropDownField[], participant: Participant): ValidationResult {
    const result = new ValidationResult();
    Utilities.validateDropDown(this.assigneeId, 'assigneeId', 'Assign To', result, validationManager);
    Utilities.validateDropDown(this.pageId, 'pageId', 'Page', result, validationManager);

    if (this.isActionNeededDisabled(pages) === false) {
      Utilities.validateDropDown(this.actionItemId, 'actionItemId', 'Action Item', result, validationManager);
    }

    Utilities.validateRequiredText(this.followUpTask, 'followUpTask', 'Task', result, validationManager);

    if (this.isNoDueDate !== true) {
      let minDateString = AppService.currentDate.format('MM/DD/YYYY');
      if (this.createdDate != null && this.createdDate.trim() !== '') {
        minDateString = moment(this.createdDate).format('MM/DD/YYYY');
      }

      const currentPlus6MonthsDate = AppService.currentDate.add(6, 'month').format('MM/DD/YYYY');

      let dueDateString = '';
      if (this.dueDate != null && this.dueDate.trim() !== '') {
        dueDateString = moment(this.dueDate)
          .format('MM/DD/YYYY')
          .toString();
      }

      const dueDateContext: MmDdYyyyValidationContext = {
        date: this.dueDateMmDdYyyy,
        prop: 'dueDate',
        prettyProp: 'Due Date',
        result: result,
        validationManager: validationManager,
        isRequired: true,
        minDate: minDateString,
        minDateAllowSame: true,
        minDateName: null,
        maxDate: currentPlus6MonthsDate,
        maxDateAllowSame: true,
        maxDateName: '6 Months in the future',
        participantDOB: null
      };
      Utilities.validateMmDdYyyyDate(dueDateContext);
    }

    if (this.isNoCompletionDate !== true && this.completionDate != null && this.completionDate.trim() !== '') {
      const currentDate = AppService.currentDate.format('MM/DD/YYYY');
      let completionDateString = '';
      if (this.completionDate != null && this.completionDate.trim() !== '') {
        completionDateString = moment(this.completionDate)
          .format('MM/DD/YYYY')
          .toString();
      }

      const enrolledDateString = moment(participant.enrolledDate)
        .format('MM/DD/YYYY')
        .toString();
      const completionDateContext: MmDdYyyyValidationContext = {
        date: this.completionDateMmDdYyyy,
        prop: 'completionDate',
        prettyProp: 'Completion Date',
        result: result,
        validationManager: validationManager,
        isRequired: true,
        minDate: enrolledDateString,
        minDateAllowSame: true,
        minDateName: 'Enrolled Date',
        maxDate: currentDate,
        maxDateAllowSame: true,
        maxDateName: 'Current Date',
        participantDOB: null
      };
      Utilities.validateMmDdYyyyDate(completionDateContext);
    }

    if (this.isDetailsRequired(actionItems) === true) {
      Utilities.validateRequiredText(this.details, 'details', 'Details', result, validationManager);
    }

    return result;
  }

  /**
   *  US 1274 When Page is "other", action item dropdown is disabled.
   *
   * @memberof ActionNeededTask
   */
  public isActionNeededDisabled(pages: DropDownField[]): boolean {
    if (+this.pageId === Utilities.idByFieldDataName('Other', pages)) {
      return true;
    } else {
      return false;
    }
  }

  /**
   *  US 1274 Details is required when action item is Other, Refer to Other Agency or Refer to External Agency.
   *
   * @param {DropDownField[]} actionItems
   * @returns {boolean}
   * @memberof ActionNeededTask
   */
  public isDetailsRequired(actionItems: DropDownField[]): boolean {
    if (actionItems == null || actionItems.length === 0) {
      return false;
    }

    if (
      +this.actionItemId === Utilities.idByFieldDataName('Other', actionItems, true) ||
      +this.actionItemId === Utilities.idByFieldDataName('Refer to Other Agency', actionItems, true) ||
      +this.actionItemId === Utilities.idByFieldDataName('Refer to External Agency', actionItems, true)
    ) {
      return true;
    } else {
      return false;
    }
  }

  /**
   *  Returns the status of a task. ex. 'onGoing', 'completed' or 'notCompleted'.
   *  US 1263
   * @returns {string}
   * @memberof ActionNeededTask
   */
  public getTaskStatus(): string {
    if ((this.completionDate == null || this.completionDate === '') && (this.isNoCompletionDate == null || this.isNoCompletionDate === false)) {
      return ActionNeededTask.taskStatuses[0];
    } else if (this.isTaskCompleted() === true) {
      return ActionNeededTask.taskStatuses[2];
    } else if (this.isTaskCompleted() === false) {
      return ActionNeededTask.taskStatuses[3];
    } else {
      console.warn('Task has invalid status');
      return '';
    }
  }

  public isDueDateSoon(): boolean {
    return Utilities.isDateCurrent(this.dueDate);
  }

  /**
   *  If task has due date without a completation date, its red. If the date is today, its yellow.
   *
   * @private
   * @returns {boolean}
   * @memberof ActionNeededTask
   */
  public whichColorIsDueDate(): string {
    if (this.completionDate != null && this.completionDate !== '') {
      return;
    }

    if (this.dueDate == null || this.dueDate === '') {
      return;
    }

    if (moment(this.dueDate).format('MM/DD/YYYY') === AppService.currentDate.format('MM/DD/YYYY')) {
      return 'yellow';
    } else if (moment(this.dueDate) < AppService.currentDate) {
      return 'red';
    }
    // console.warn('Could not determine due date color');
  }

  /**
   *  If task has a due date more than 30 days ago and has no completed date and did not complete
   *  is not marked "Did Not Complete", return true.
   *
   * @memberof ActionNeededTask
   */
  public isOverDueDate(): boolean {
    if (this.isRecentTask && (this.completionDate == null || this.completionDate === '') && this.isNoCompletionDate === false) {
      return true;
    } else {
      return false;
    }
  }

  /**
   *  If this task's completion date less than more than 30 days ago, return true.
   *
   * @returns {boolean}
   * @memberof ActionNeededTask
   */
  public isRecentTask(): boolean {
    const numberOfDays = 30;
    if (this.completionDate != null && moment(this.completionDate) > AppService.currentDate.subtract(numberOfDays, 'day')) {
      return true;
    } else {
      return false;
    }
  }

  /**
   *  If task has a valid(moment object) completionDate then it has been compeleted thus returns true.
   *
   * @private
   * @returns {boolean}
   * @memberof ActionNeededTask
   */
  public isTaskCompleted(): boolean {
    if (moment(this.completionDate).isValid() === true && this.isNoCompletionDate !== true) {
      return true;
    } else {
      return false;
    }
  }
}
