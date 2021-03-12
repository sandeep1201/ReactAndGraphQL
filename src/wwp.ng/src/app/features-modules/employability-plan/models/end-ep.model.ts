import { Serializable } from '../../../shared/interfaces/serializable';
import * as moment from 'moment';
import { Goal } from './goal.model';
import { Activity } from './activity.model';
import { Utilities } from '../../../shared/utilities';
import { ValidationManager, ValidationResult } from '../../../shared/models/validation';
import { DropDownMultiField } from 'src/app/shared/models/dropdown-multi-field';

export class EndEP implements Serializable<EndEP> {
  public epId: number;
  public goals: Goal[];
  public activities: Activity[];

  public static clone(input: any, instance: EndEP) {
    instance.goals = Utilities.deserilizeChildren(input.goals, Goal, 0);
    instance.activities = Utilities.deserilizeChildren(input.activities, Activity, 0);
  }

  public static create(): EndEP {
    const endEP = new EndEP();
    return endEP;
  }

  public deserialize(input: any) {
    EndEP.clone(input, this);
    return this;
  }

  validate(
    validationManager: ValidationManager,
    goals: Goal[],
    activities: Activity[],
    epBeginDate: string,
    maxDaysCanBackDate: string,
    pullDownDates: DropDownMultiField[],
    enrolledProgramCd: string
  ): ValidationResult {
    const result = new ValidationResult();
    const goalsErrArr = result.createErrorsArray('goals');
    if (goals.length > 0) {
      goals.forEach(obj => {
        const v = obj.validate(validationManager, null, [], [], true);
        goalsErrArr.push(v.errors);
        if (v.isValid === false) {
          result.isValid = false;
        }
      });
    }
    const activitiesErrArr = result.createErrorsArray('activities');

    if (activities.length > 0) {
      activities.forEach(obj => {
        const v = obj.validate(validationManager, null, false, false, epBeginDate, true, [], [], true, true, maxDaysCanBackDate, pullDownDates, enrolledProgramCd);
        activitiesErrArr.push(v.errors);
        if (v.isValid === false) {
          result.isValid = false;
        }
      });
    }

    return result;
  }
}
