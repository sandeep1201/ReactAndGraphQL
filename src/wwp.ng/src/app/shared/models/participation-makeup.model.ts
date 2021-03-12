import { Serializable } from '../../shared/interfaces/serializable';
import { ValidationManager, ValidationResult, ValidationCode } from 'src/app/shared/models/validation';
import { HrsValidationContext } from 'src/app/shared/interfaces/hrs-validation-context';
import { Utilities } from 'src/app/shared/utilities';
import * as moment from 'moment';
export class MakeUpEntries implements Serializable<MakeUpEntries> {
  public id: number;
  public participantId: number;
  public makeupDate: string;
  public makeupHours: string;

  public static clone(input: any, instance: MakeUpEntries) {
    instance.id = input.id;
    instance.participantId = input.participantId;
    instance.makeupDate = input.makeupDate;
    instance.makeupHours = input.makeupHours;
  }
  public static create(): MakeUpEntries {
    const makeUpEntries = new MakeUpEntries();
    makeUpEntries.id = 0;
    return makeUpEntries;
  }

  public deserialize(input: any) {
    MakeUpEntries.clone(input, this);
    return this;
  }
  public clear(): void {
    this.makeupDate = null;
    this.makeupHours = null;
  }
  public isEmpty(): boolean {
    // All properties have to be null or blank to make the entire object empty.
    return (this.makeupDate == null || this.makeupDate.trim() === '') && this.makeupHours == null && (this.makeupHours == null || this.makeupHours.toString().trim() === '');
  }

  public validate(validationManager: ValidationManager, participationDate: any) {
    const result = new ValidationResult();
    const currentDate = Utilities.currentDate;
    const ptDate = moment(new Date(participationDate));
    const WeekStartdate = moment(ptDate).startOf('week');
    const WeekEnddate = moment(ptDate).endOf('week');
    const inputDate = moment(this.makeupDate, 'MM/DD/YYYY', true);

    const hrsValidationContext: HrsValidationContext = {
      hrs: this.makeupHours,
      result: result,
      validationManager: validationManager,
      prop: 'makeupHours',
      prettyProp: 'Make-Up Hours',
      isRequired: true,
      canEnterZero: false
    };
    Utilities.validateHrs(hrsValidationContext);
    Utilities.dateFormatValidation(validationManager, result, this.makeupDate, 'Date', 'makeupDate');
    if (inputDate.isBefore(WeekStartdate) || inputDate.isAfter(WeekEnddate)) {
      validationManager.addErrorWithDetail(
        ValidationCode.ValueOutOfRange_Details,
        'The date entered for Make-Up Hours must be in the same calendar week (Sun - Sat) as the original activity date.'
      );
      result.addError('makeupDate');
    }
    if (inputDate.isAfter(currentDate)) {
      validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, 'The date entered for Make-Up Hours cannot be a future date.');
      result.addError('makeupDate');
    }
    return result;
  }
}
