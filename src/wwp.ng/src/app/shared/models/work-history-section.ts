import { DropDownField } from '../../shared/models/dropdown-field';
import { Serializable } from '../interfaces/serializable';
import { ValidationCode } from './validation-error';
import { ValidationManager } from './validation-manager';
import { ValidationResult } from './validation-result';
import { Employment } from './work-history-app';
import { Utilities } from '../utilities';

export class WorkHistorySection implements Serializable<WorkHistorySection> {
  isSubmittedViaDriverFlow: boolean;
  employmentStatusTypeId: number;
  employmentStatusTypeName: string;
  preventionFactorIds: number[];
  preventionFactorNames: string[];
  nonFullTimeDetails: string;
  hasVolunteered: boolean;
  notes: string;
  rowVersion: string;
  modifiedBy: string;
  modifiedDate: string;
  hasCareerAssessment: number;
  hasCareerAssessmentName: string;
  hasCareerAssessmentNotes: string;

  cached: WorkHistorySection;

  public static clone(input: any, instance: WorkHistorySection) {
    instance.isSubmittedViaDriverFlow = input.isSubmittedViaDriverFlow;
    instance.employmentStatusTypeId = input.employmentStatusTypeId;
    instance.employmentStatusTypeName = input.employmentStatusTypeName;
    instance.preventionFactorIds = Utilities.deserilizeArray(input.preventionFactorIds);
    instance.preventionFactorNames = Utilities.deserilizeArray(input.preventionFactorNames);
    instance.nonFullTimeDetails = input.nonFullTimeDetails;
    instance.hasVolunteered = input.hasVolunteered;
    instance.notes = input.notes;
    instance.rowVersion = input.rowVersion;
    instance.modifiedBy = input.modifiedBy;
    instance.modifiedDate = input.modifiedDate;
    instance.hasCareerAssessment = input.hasCareerAssessment;
    instance.hasCareerAssessmentName = input.hasCareerAssessmentName;
    instance.hasCareerAssessmentNotes = input.hasCareerAssessmentNotes;
  }

  public deserialize(input: any) {
    WorkHistorySection.clone(input, this);
    this.cached = new WorkHistorySection();
    WorkHistorySection.clone(input, this.cached);
    return this;
  }

  isEmployedOrVolunteerdRequired(workStatusDropDown: DropDownField[]): boolean {
    const unemployedId = Utilities.idByFieldDataName('Unemployed', workStatusDropDown);
    if (+this.employmentStatusTypeId === unemployedId) {
      return true;
    } else {
      return false;
    }
  }

  arePreventingFactorsRequired(workStatusDropDown: DropDownField[]): boolean {
    const unemployedId = Utilities.idByFieldDataName('Unemployed', workStatusDropDown);
    const partTimeId = Utilities.idByFieldDataName('Part-Time', workStatusDropDown);
    if (+this.employmentStatusTypeId === unemployedId || +this.employmentStatusTypeId === partTimeId) {
      return true;
    } else {
      return false;
    }
  }

  arePreventingFactorsDisplayed(workStatusDropDown: DropDownField[]): boolean {
    return this.arePreventingFactorsRequired(workStatusDropDown);
  }

  isNonFullTimeDetailsRequired(possibleActionNeededs: DropDownField[]): boolean {
    const otherId = Utilities.idByFieldDataName('Other', possibleActionNeededs);
    const personalChoiceId = Utilities.idByFieldDataName('Personal Choice', possibleActionNeededs);
    if (this.preventionFactorIds != null && (this.preventionFactorIds.indexOf(otherId) > -1 || this.preventionFactorIds.indexOf(personalChoiceId) > -1)) {
      return true;
    } else {
      return false;
    }
  }

  validate(
    validationManager: ValidationManager,
    workStatusDropDown: DropDownField[],
    possibleActionNeededs: DropDownField[],
    employments: Employment[],
    polarDrop: DropDownField[],
    showCareerFeature: boolean,
    whReadOnly: boolean
  ): ValidationResult {
    const fullTimeId = Utilities.idByFieldDataName('Full-Time', workStatusDropDown);
    const partTimeId = Utilities.idByFieldDataName('Part-Time', workStatusDropDown);
    const otherId = Utilities.idByFieldDataName('Other', possibleActionNeededs);
    let yesId: number;
    if (polarDrop) yesId = Utilities.idByFieldDataName('Yes', polarDrop);

    const result = new ValidationResult();

    Utilities.validateDropDown(this.employmentStatusTypeId, 'employmentStatusTypeId', 'Employment Status', result, validationManager);

    if (this.isEmployedOrVolunteerdRequired(workStatusDropDown) && employments != null && employments.length < 1) {
      Utilities.validateRequiredYesNo(result, validationManager, this.hasVolunteered, 'hasVolunteered', 'Have you ever been employed or performed volunteer work?');
      if (this.hasVolunteered === true) {
        this.addErrorForNoEmployments(validationManager, employments, result, whReadOnly);
      }
    }

    if (this.arePreventingFactorsRequired(workStatusDropDown)) {
      Utilities.validateMultiSelect(this.preventionFactorIds, 'preventionFactorIds', 'What is keeping you from working full-time?', result, validationManager);

      if (this.isNonFullTimeDetailsRequired(possibleActionNeededs)) {
        Utilities.validateRequiredText(this.nonFullTimeDetails, 'nonFullTimeDetails', 'Details', result, validationManager);
      }
    }

    // US 1162.
    if (this.employmentStatusTypeId != null && (this.employmentStatusTypeId === fullTimeId || this.employmentStatusTypeId === partTimeId)) {
      this.addErrorForNoEmployments(validationManager, employments, result, whReadOnly);
    }

    if (showCareerFeature) {
      if (!this.hasCareerAssessment) {
        const polarIds = [];
        Utilities.validateMultiSelect(polarIds, 'hasCareerAssessment', 'Have you completed a career assessment?', result, validationManager);
      }

      if (yesId && this.hasCareerAssessment === yesId && (!this.hasCareerAssessmentNotes || this.hasCareerAssessmentNotes === ''))
        Utilities.validateRequiredText(this.hasCareerAssessmentNotes, 'hasCareerAssessmentNotes', 'Have you completed a career assessment? - Details', result, validationManager);
    }

    return result;
  }

  // US 1162.
  private addErrorForNoEmployments(validationManager: ValidationManager, employments: Employment[], result: ValidationResult, whReadOnly: boolean) {
    if ((employments == null || employments.length < 1) && !whReadOnly) {
      validationManager.addError(ValidationCode.WorkHistoryNoEmployments);
      result.addError('workHistory');
    }
  }
}
