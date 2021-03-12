import { ValidationResult } from '../models/validation-result';
import { ValidationManager } from '../models/validation-manager';

export interface HrsValidationContext {
  hrs: string;
  result: ValidationResult;
  validationManager: ValidationManager;
  prop: string;
  prettyProp: string;
  isRequired: boolean;
  canEnterZero?: boolean;
}
