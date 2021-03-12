export * from './validation-error';
export * from './validation-manager';
export * from './validation-result';
import * as validation from 'lakmus';
export abstract class Validator<T>{
    constructor(protected instance: T) {

    }
    abstract validate(): validation.ValidationResult;
    protected validationResult: validation.ValidationResult = new validation.ValidationResult();

}