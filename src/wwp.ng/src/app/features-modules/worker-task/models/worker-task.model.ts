// tslint:disable: no-use-before-declare
import { Utilities } from '../../../shared/utilities';
import { ValidationManager, ValidationResult, ValidationCode } from 'src/app/shared/models/validation';
import * as moment from 'moment';
import { MmDdYyyyValidationContext } from 'src/app/shared/interfaces/mmDdYyyy-validation-context';

export class WorkerTask {
  public id: number;
  public participantId: number;
  public fullName: string;
  public pin: string;
  public workerId: number;
  public categoryId: number;
  public categoryName: string;
  public categoryCode: string;
  public participantFirstName: string;
  public participantMiddleInitial: string;
  public participantLastName: string;
  public participantSuffixName: string;
  public actionPriorityId: number;
  public actionPriorityName: string;
  public workerTaskStatusId: number;
  public workerTaskStatusCode: string;
  public workerTaskStatusName: string;
  public statusDate: string;
  public dueDate: string;
  public isSystemGenerated: boolean;
  public isDeleted: boolean;
  public taskDate: string;
  public taskDetails: string;
  public modifiedBy: string;
  public modifiedDate: string;
  public daysDifference: number;

  public static create(participantId: number) {
    const workerTask = new WorkerTask();
    workerTask.id = 0;
    workerTask.participantId = participantId;
    return workerTask;
  }

  public static clone(input: any, instance: WorkerTask) {
    instance.id = input.id;
    instance.participantId = input.participantId;
    instance.fullName = Utilities.formatDisplayPersonName(input.participantFirstName, input.participantMiddleInitial, input.participantLastName, input.participantSuffixName, true);
    instance.pin = Utilities.formatPin(input.pin);
    instance.workerId = input.workerId;
    instance.categoryId = input.categoryId;
    instance.categoryName = input.categoryName;
    instance.categoryCode = input.categoryCode;
    instance.participantFirstName = input.participantFirstName;
    instance.participantMiddleInitial = input.participantMiddleInitial;
    instance.participantLastName = input.participantLastName;
    instance.participantSuffixName = input.participantSuffixName;
    instance.actionPriorityId = input.actionPriorityId;
    instance.actionPriorityName = input.actionPriorityName;
    instance.workerTaskStatusId = input.workerTaskStatusId;
    instance.workerTaskStatusCode = input.workerTaskStatusCode;
    instance.workerTaskStatusName = input.workerTaskStatusName;
    instance.statusDate = input.statusDate ? moment(input.statusDate).format('MM/DD/YYYY') : input.statusDate;
    instance.dueDate = input.dueDate ? moment(input.dueDate).format('MM/DD/YYYY') : input.dueDate;
    instance.daysDifference = input.dueDate ? Math.ceil(moment(input.dueDate).diff(Utilities.currentDate.clone(), 'days', true)) : null;
    instance.isSystemGenerated = input.isSystemGenerated;
    instance.isDeleted = input.isDeleted;
    instance.taskDate = input.taskDate ? moment(input.taskDate).format('MM/DD/YYYY') : input.taskDate;
    instance.taskDetails = input.taskDetails;
    instance.modifiedBy = input.modifiedBy;
    instance.modifiedDate = input.modifiedDate;
  }

  public deserialize(input: any) {
    WorkerTask.clone(input, this);
    return this;
  }

  public validate(validationManager: ValidationManager, cachedModel: WorkerTask): ValidationResult {
    const result = new ValidationResult();
    const minDate =
      this.id > 0 && cachedModel.dueDate
        ? moment(cachedModel.dueDate)
            .subtract(1, 'day')
            .format('MM/DD/YYYY')
        : Utilities.currentDate.clone().format('MM/DD/YYYY');
    const add6Months = Utilities.currentDate
      .clone()
      .add(6, 'month')
      .format('MM/DD/YYYY');
    const dueDateContext: MmDdYyyyValidationContext = {
      date: this.dueDate,
      prop: 'dueDate',
      prettyProp: 'Due Date',
      result: result,
      validationManager: validationManager,
      isRequired: false,
      maxDate: add6Months,
      maxDateAllowSame: true,
      maxDateName: add6Months,
      participantDOB: null,
      minDateAllowSame: false,
      minDate: minDate,
      minDateName: minDate
    };

    Utilities.validateDropDown(this.categoryId, 'categoryId', 'Task Category', result, validationManager);

    if (Utilities.stringIsNullOrWhiteSpace(this.taskDetails)) {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Task Description');
      result.addError('taskDetails');
    }

    Utilities.validateMmDdYyyyDate(dueDateContext);

    return result;
  }
}
