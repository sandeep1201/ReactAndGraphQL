import { Utilities } from '../../../shared/utilities';
import { Serializable } from '../../../shared/interfaces/serializable';
import { Clearable } from '../../../shared/interfaces/clearable';
import { IsEmpty } from '../../../shared/interfaces/is-empty';
import { ValidationManager, ValidationCode } from '../../../shared/models/validation';
import { ValidationResult } from '../../../shared/models/validation-result';

export class SupportiveService implements Clearable, IsEmpty, Serializable<SupportiveService> {
  id: number;
  employabilityPlanId: number;
  supportiveServiceTypeId: number;
  supportiveServiceTypeName: string;
  details: string;
  modifiedBy: string;
  modifiedDate: string;

  /**
   * Creates a new object suitable to be bound to the UI and passed to the API.
   * It will set the ID's to appropriate values.
   */
  public static create(): SupportiveService {
    const x = new SupportiveService();
    x.id = 0;
    return x;
  }

  public static clone(input: any, instance: SupportiveService) {
    instance.id = input.id;
    instance.employabilityPlanId = input.employabilityPlanId;
    instance.supportiveServiceTypeId = input.supportiveServiceTypeId;
    instance.supportiveServiceTypeName = input.supportiveServiceTypeName;
    instance.details = input.details;
  }

  public clear(): void {
    this.id = null;
    this.employabilityPlanId = null;
    this.details = null;
  }

  public isEmpty(): boolean {
    // All properties have to be null or blank to make the entire object empty.
    return this.id == null && this.employabilityPlanId == null && this.details == null;
  }

  public deserialize(input: any) {
    SupportiveService.clone(input, this);
    return this;
  }

  public validate(validationManager: ValidationManager, otherSupportiveServiceTypeId: number): ValidationResult {
    const result = new ValidationResult();

    Utilities.validateDropDown(this.supportiveServiceTypeId, 'supportiveServiceTypeId', 'Service', result, validationManager);

    if (this.isDetailsRequired(otherSupportiveServiceTypeId)) {
      if (this.details == null || this.details.trim() === '') {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Details');
        result.addError('details');
      }
    }

    return result;
  }

  isDetailsRequired(otherSupportiveServiceTypeId: number) {
    if (Number(this.supportiveServiceTypeId) === otherSupportiveServiceTypeId) {
      return true;
    } else {
      return false;
    }
  }
}
