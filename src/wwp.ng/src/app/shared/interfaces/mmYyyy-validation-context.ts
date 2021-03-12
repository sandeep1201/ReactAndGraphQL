import { ValidationResult } from '../models/validation-result';
import { ValidationManager } from '../models/validation-manager';
import { Moment } from 'moment';

export interface MmYyyyValidationContext {
    date: string;
    prop: string;
    prettyProp: string;
    result: ValidationResult;
    validationManager: ValidationManager;
    isRequired: boolean;
    minDate: string;
    minDateAllowSame: boolean;
    minDateName: string;
    maxDate: string;
    maxDateAllowSame: boolean;
    maxDateName: string;
    participantDOB: Moment;
}
