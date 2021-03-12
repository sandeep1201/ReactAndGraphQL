import { Serializable } from '../../../shared/interfaces/serializable';
import { ValidationManager } from '../../../shared/models/validation-manager';
import { ValidationResult, ValidationCode } from '../../../shared/models/validation';
import { Utilities } from '../../../shared/utilities';

export class GoalStep implements Serializable<GoalStep> {
  id: number;
  goalId: number;
  details: string;
  isGoalStepCompleted: boolean;
  isDeleted: boolean;

  goalStepCheck: boolean;

  disabledCompleteButton: boolean;

  public static clone(input: any, instance: GoalStep) {
    instance.id = input.id;
    instance.goalId = input.goalId;
    instance.details = input.details;
    instance.isGoalStepCompleted = input.isGoalStepCompleted;
    instance.isDeleted = input.isDeleted;
  }

  public static create(): GoalStep {
    const x = new GoalStep();
    x.id = 0;
    return x;
  }

  public clear(): void {
    this.id = null;
    this.goalId = null;
    // this.stepDetails = null;
  }

  public isEmpty(): boolean {
    return this.id == null && this.goalId == null && this.details == null;
  }

  public deserialize(input: any) {
    GoalStep.clone(input, this);
    return this;
  }

  validate(validationManager: ValidationManager): ValidationResult {
    const result = new ValidationResult();

    validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Goal Step Details');
    result.addError('details');

    return result;
  }
}
