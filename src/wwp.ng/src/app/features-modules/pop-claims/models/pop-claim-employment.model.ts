import { Serializable } from '../../../shared/interfaces/serializable';
import { ValidationManager } from 'src/app/shared/models/validation-manager';
import { ValidationResult } from 'src/app/shared/models/validation-result';
import { Utilities } from 'src/app/shared/utilities';
import { ValidationCode } from 'src/app/shared/models/validation-error';
import { Clearable } from 'src/app/shared/interfaces/clearable';
import { IsEmpty } from 'src/app/shared/interfaces/is-empty';

export class POPClaimEmployment implements Serializable<POPClaimEmployment> {
  modifiedBy: string;
  modifiedDate: string;
  modifiedByName: string;
  id: number;
  employmentInformationId: number;
  jobTypeId: number;
  jobTypeName: string;
  jobBeginDate: string;
  jobEndDate: string;
  jobPosition: string;
  companyName: string;
  hoursWorked: number;
  earnings: number;
  isPrimary: boolean;
  isSelected: boolean;
  averageWeeklyHours: number;
  deletedReasonId: number;
  startingWage: number;
  startingWageUnit: string;

  public static clone(input: any, instance: POPClaimEmployment) {
    instance.modifiedBy = input.modifiedBy;
    instance.modifiedByName = input.modifiedByName;
    instance.modifiedDate = input.modifiedDate;
    instance.id = input.id;
    instance.employmentInformationId = input.employmentInformationId;
    instance.jobTypeId = input.jobTypeId;
    instance.jobTypeName = input.jobTypeName;
    instance.jobBeginDate = input.jobBeginDate;
    instance.jobEndDate = input.jobEndDate;
    instance.jobPosition = input.jobPosition;
    instance.companyName = input.companyName;
    instance.hoursWorked = input.id ? input.hoursWorked : undefined;
    instance.earnings = input.id ? input.earnings : undefined;
    instance.isPrimary = input.isPrimary || false;
    instance.isSelected = input.isSelected || false;
    instance.averageWeeklyHours = input.averageWeeklyHours;
    instance.deletedReasonId = input.deletedReasonId;
    instance.startingWage = input.startingWage;
    instance.startingWageUnit = input.startingWageUnit;

    return instance;
  }

  public deserialize(input: any) {
    POPClaimEmployment.clone(input, this);
    return this;
  }

  public clear(): void {
    this.id = null;
  }

  public isEmpty(): boolean {
    // All properties have to be null or blank to make the entire object empty.
    return this.id == null;
  }

  public validate(validationManager: ValidationManager, isHighWagePOPClaimSelectedbool: boolean): ValidationResult {
    const result = new ValidationResult();
    if (this.isSelected && !isHighWagePOPClaimSelectedbool && Utilities.isNumberEmptyOrNull(this.hoursWorked)) {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Hours Worked');
      result.addError('hoursWorked');
    }
    if (this.isSelected && !isHighWagePOPClaimSelectedbool && Utilities.isNumberEmptyOrNull(this.earnings)) {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Earnings');
      result.addError('earnings');
    }
    return result;
  }
}
