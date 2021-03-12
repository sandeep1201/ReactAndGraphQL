import { Dictionary } from '../../../shared/dictionary';
import { Serializable } from '../../../shared/interfaces/serializable';
import { GoalStep } from './goal-step.model';
import { Utilities } from '../../../shared/utilities';
import { ValidationManager } from '../../../shared/models/validation-manager';
import { ValidationResult, ValidationCode } from '../../../shared/models/validation';
import { DropDownField } from '../../../shared/models/dropdown-field';

export class Goal implements Serializable<Goal> {
  beginDate: string;
  details: string;
  employmentPlanId: number;
  id: number;
  goalTypeId: number;
  goalTypeName: string;
  endDate: string;
  endReasonId: number;
  endReasonName: string;
  endReasonDetails: string;
  isGoalEnded: boolean;
  modifiedBy: string;
  modifiedDate: string;
  name: string;
  program: string;
  employabilityPlanId: number;
  rowVersion: string;

  showControl = true;
  goalSteps: GoalStep[];

  public static clone(input: any, instance: Goal) {
    instance.beginDate = input.beginDate;
    instance.details = input.details;
    instance.employmentPlanId = input.employmentPlanId;
    instance.id = input.id;
    instance.goalTypeId = input.goalTypeId;
    instance.goalTypeName = input.goalTypeName;
    instance.endDate = input.endDate;
    instance.endReasonId = input.endReasonId;
    instance.endReasonName = input.endReasonName;
    instance.endReasonDetails = input.endReasonDetails;
    instance.isGoalEnded = input.isGoalEnded;
    instance.modifiedBy = input.modifiedBy;
    instance.modifiedDate = input.modifiedDate;
    instance.name = input.name;
    instance.program = input.program;
    instance.employabilityPlanId = input.employabilityPlanId;
    instance.rowVersion = input.rowVersion;
    instance.goalSteps = Utilities.deserilizeChildren(input.goalSteps, GoalStep, 0);
  }

  public static create(): Goal {
    const goal = new Goal();
    goal.id = 0;
    return goal;
  }

  public deserialize(input: any) {
    Goal.clone(input, this);
    return this;
  }
  isDetailsRequired(primaryEmploymentId: number, educationGoalId: number) {
    if (Number(this.goalTypeId) === primaryEmploymentId || Number(this.goalTypeId) === educationGoalId) {
      if (this.goalSteps[0]) {
        return true;
      } else return false;
    } else {
      return false;
    }
  }

  isPrimaryStepsRequired(primaryEmploymentId: number) {
    if (Number(this.goalTypeId) === primaryEmploymentId) {
      if (this.goalSteps.length === 0 || this.goalSteps.every(i => i.isGoalStepCompleted)) {
        return true;
      }
    } else {
      return false;
    }
  }

  isEducationStepsRequired(educationGoalId: number) {
    if (Number(this.goalTypeId) === educationGoalId) {
      if (this.goalSteps.length === 0 || this.goalSteps.every(i => i.isGoalStepCompleted)) {
        return true;
      }
    } else {
      return false;
    }
  }

  isGoalStepDetailsRequired() {
    if (this.goalSteps.length > 0) {
      const details = this.goalSteps[0].details;
      if (details == null || details.trim() === '') {
        return true;
      } else {
        return false;
      }
    } else return false;
  }

  validate(
    validationManager: ValidationManager,
    goalTypeCountDictionary?: Dictionary<string, number>,
    goalTypesDrop?: DropDownField[],
    endGoalTypesDrop?: DropDownField[],
    showEndReasons?: boolean
  ): ValidationResult {
    const result = new ValidationResult();

    const primaryEmploymentId = Utilities.idByFieldDataName('Primary Employment Goal', goalTypesDrop, true);
    const secondaryEmploymentId = Utilities.idByFieldDataName('Secondary Employment Goal', goalTypesDrop, true);
    const otherProgramGoalId = Utilities.idByFieldDataName('Other Program Goal', goalTypesDrop, true);
    const longTermCareerId = Utilities.idByFieldDataName('Long Term Career Goal', goalTypesDrop, true);
    const personalGoalId = Utilities.idByFieldDataName('Personal Goal', goalTypesDrop, true);
    const educationGoalId = Utilities.idByFieldDataName('Education Goal', goalTypesDrop, true);

    let maxValidation = false;
    let maxNumberOfOtherGoalTypesAllowed = 10;
    let maxNumberOfPrimaryGoalTypesAllowed = 1;
    let maxNumberOfSecondaryGoalTypesAllowed = 1;
    let maxNumberOfEducationGoalTypesAllowed = 1;

    if (this.goalTypeId == null || this.goalTypeId.toString() === '') {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Goal Type');
      result.addError('goalTypeId');
    } else {
      if (this.goalTypeId === primaryEmploymentId && goalTypeCountDictionary.containsKey('Primary Employment Goal')) {
        if (goalTypeCountDictionary.getValue('Primary Employment Goal') + 1 > maxNumberOfPrimaryGoalTypesAllowed) {
          validationManager.addErrorWithDetail(ValidationCode.DuplicateDataWithNoMessage, 'Participant may only have one Primary Employment Goal');
          result.addError('goalTypeId');
          maxValidation = true;
        }
      }
      if (showEndReasons === true) {
        if (this.endReasonId === null || this.endReasonId === undefined) {
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Goal End Reason');
          result.addError('endReasonId');
        }
      }

      if (this.goalTypeId === secondaryEmploymentId && goalTypeCountDictionary.containsKey('Secondary Employment Goal')) {
        if (goalTypeCountDictionary.getValue('Secondary Employment Goal') + 1 > maxNumberOfSecondaryGoalTypesAllowed) {
          validationManager.addErrorWithDetail(ValidationCode.DuplicateDataWithNoMessage, 'Participant may only have one Secondary Employment Goal');
          result.addError('goalTypeId');
          maxValidation = true;
        }
      }
      if (this.goalTypeId === educationGoalId && goalTypeCountDictionary.containsKey('Education Goal')) {
        if (goalTypeCountDictionary.getValue('Education Goal') + 1 > maxNumberOfEducationGoalTypesAllowed) {
          validationManager.addErrorWithDetail(ValidationCode.DuplicateDataWithNoMessage, 'Participant may only have one Education Goal');
          result.addError('goalTypeId');
          maxValidation = true;
        }
      }

      if (this.goalTypeId === otherProgramGoalId && goalTypeCountDictionary.containsKey('Other Program Goal')) {
        if (goalTypeCountDictionary.getValue('Other Program Goal') + 1 > maxNumberOfOtherGoalTypesAllowed) {
          validationManager.addErrorWithDetail(ValidationCode.ValuesExceedRange, `Participant may only have ten Other Program Goals`);

          // validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Participant may have only up to 10 Other Program Goals');
          result.addError('goalTypeId');
        }
      }

      if (this.goalTypeId === longTermCareerId && goalTypeCountDictionary.containsKey('Long Term Career Goal')) {
        if (goalTypeCountDictionary.getValue('Long Term Career Goal') + 1 > maxNumberOfOtherGoalTypesAllowed) {
          validationManager.addErrorWithDetail(ValidationCode.ValuesExceedRange, 'Participant may only have ten Long Term Career Goals');
          result.addError('goalTypeId');
        }
      }

      if (this.goalTypeId === personalGoalId && goalTypeCountDictionary.containsKey('Personal Goal')) {
        if (goalTypeCountDictionary.getValue('Personal Goal') + 1 > maxNumberOfOtherGoalTypesAllowed) {
          validationManager.addErrorWithDetail(ValidationCode.ValuesExceedRange, 'Participant may only have ten Personal Goals');
          result.addError('goalTypeId');
        }
      }
    }

    if (this.name == null || this.name.toString() === '') {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Goal Name');
      result.addError('name');
    }

    if (this.details == null || this.details.toString() === '') {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Goal Description');
      result.addError('details');
    }

    if (!showEndReasons && this.isPrimaryStepsRequired(primaryEmploymentId)) {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Goal Steps are required for Primary Employment Goals');
      result.addError('steps');
    } else {
      if (!showEndReasons && educationGoalId && this.isEducationStepsRequired(educationGoalId)) {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Goal Steps are required for Education Goals');
        result.addError('steps');
      }
    }

    const errArr = result.createErrorsArray('goalSteps');

    if ((this.goalTypeId === primaryEmploymentId || this.goalTypeId === educationGoalId) && this.isGoalStepDetailsRequired()) {
      const v = this.goalSteps[0].validate(validationManager);
      errArr.push(v.errors);
      if (v.isValid === false) {
        result.isValid = false;
      }
    }

    return result;
  }
}
