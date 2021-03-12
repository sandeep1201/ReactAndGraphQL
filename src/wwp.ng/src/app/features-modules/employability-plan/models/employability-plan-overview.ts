import { EnrolledProgramCode } from 'src/app/shared/enums/enrolled-program-code.enum';
import { EmployabilityPlan } from './employability-plan.model';
import { ValidationResult } from '../../../shared/models/validation-result';
import { Goal } from './goal.model';
import { ValidationManager } from '../../../shared/models/validation-manager';
import { Activity } from './activity.model';
import { ValidationCode } from '../../../shared/models/validation';
export class EmployabilityPlanOverview {
  validate(validationManager: ValidationManager, employabilityPlan: EmployabilityPlan, activities: Activity[], goals: Goal[], elapsedActivities: Activity[]) {
    const result = new ValidationResult();
    if (goals && goals.length === 0) {
      validationManager.addErrorWithFormat(ValidationCode.RequiredInformationMissing2, 'At least one goal is required.');
      result.addError('Atleast one goal');
    } else {
      if (
        employabilityPlan.enrolledProgramCd.trim().toLowerCase() === EnrolledProgramCode.ww ||
        employabilityPlan.enrolledProgramCd.trim().toLowerCase() === EnrolledProgramCode.cf ||
        employabilityPlan.enrolledProgramCd.trim().toLowerCase() === EnrolledProgramCode.tmj ||
        employabilityPlan.enrolledProgramCd.trim().toLowerCase() === EnrolledProgramCode.tj
      ) {
        if (!goals.some(i => i.goalTypeName === 'Primary Employment Goal')) {
          validationManager.addErrorWithFormat(ValidationCode.RequiredInformationMissing2, 'Primary Employment Goal type is required.');
          result.addError('Primary Employment Goal');
        } else if (!goals.find(i => i.goalTypeName === 'Primary Employment Goal' && i.goalSteps && i.goalSteps.some(gs => !gs.isGoalStepCompleted))) {
          validationManager.addErrorWithFormat(
            ValidationCode.RequiredInformationMissing2,
            'At least one Goal Step must not be marked as Completed in order for the Goal to remain on the EP.'
          );
          result.addError('Primary Employment Goal');
        }
      } else if (employabilityPlan.enrolledProgramName.toLocaleLowerCase() === 'learnfare') {
        if (!goals.some(i => i.goalTypeName === 'Education Goal')) {
          validationManager.addErrorWithFormat(ValidationCode.RequiredInformationMissing2, 'Education Goal type is required.');
          result.addError('Education Goal');
        } else if (goals.some(i => i.goalTypeName === 'Education Goal' && i.goalSteps && i.goalSteps.every(gs => gs.isGoalStepCompleted))) {
          validationManager.addErrorWithFormat(
            ValidationCode.RequiredInformationMissing2,
            'At least one Goal Step must not be marked as Completed in order for the Goal to remain on the EP.'
          );
          result.addError('Education Goal');
        }
      }
    }
    let nonElapsedActivitiesCount: number;
    if (activities && elapsedActivities) {
      nonElapsedActivitiesCount = activities.filter(val => elapsedActivities.indexOf(val) <= -1).length;
    }
    //Only Elapsed activities are present
    if (elapsedActivities && elapsedActivities.length > 0) {
      if (elapsedActivities.some(activity => activity.activityCompletionReasonId === null || activity.activityCompletionReasonId === undefined)) {
        validationManager.addErrorWithFormat(ValidationCode.RequiredInformationMissing2, `Elapsed Activities must be ended prior to submitting this EP.`);
        result.addError('Activity Validation');
      }
    }
    //Activities can't be assigned and activities are present
    if (employabilityPlan.canSaveWithoutActivity) {
      if (nonElapsedActivitiesCount > 0) {
        validationManager.addErrorWithFormat(ValidationCode.RequiredInformationMissing2, `Activities cannot be assigned when "Create EP without activities?" is "Yes"`);
        result.addError('Activity Validation');
      }
    } else if (activities) {
      // If can save without activity is true and activities are assigned
      if (activities.length === 0) {
        validationManager.addErrorWithFormat(ValidationCode.RequiredInformationMissing2, `Activities must be assigned when "Create EP without activities?" is "No"`);
        result.addError('Atleast one activity');
      }
    }
    return result;
  }
}
