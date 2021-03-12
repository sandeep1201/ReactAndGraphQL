import { ModelErrors } from '../interfaces/model-errors';

/**
 * Contains the results of the validation process.
 *
 * @export
 * @class ValidationResult
 */
export class ValidationResult {
    // We always start out assuming the validation succeeds. The validation process
    // looks for errors and will flip this property when an error is added.
    public isValid: boolean = true;

    // This is the root ModelErrors object.
    public readonly errors: ModelErrors = {};

    /**
     * Used to add a specific error to the result (root) errors property.
     *
     * @param {string} property
     *
     * @memberOf ValidationResult
     */
    public addError(property: string): void {
        // Any error causes the validation indication to be flipped.
        this.isValid = false;
        
        // A true value indicates this property has an error.
        this.errors[property] = true;
    }

    public addErrorContainingObject(property: string): ModelErrors {
        return this.errors[property] = {};
    }
    
    public addErrorForContainingObject(parent: ModelErrors, property: string): void {
        this.isValid = false;
        parent[property] = true;
    }

    /**
     * Used to add a specific error to the result on a provided (parent) ModelErrors object.
     *
     * @param {string} property
     *
     * @memberOf ValidationResult
     */
    public addErrorForParent(parent: ModelErrors, property: string): void {
        // Any error causes the validation indication to be flipped.
        this.isValid = false;

        // A true value indicates this property has an error.
        parent[property] = true;
    }

    public addErrorToParent(errors: ModelErrors, property: string): void {
        // Any error causes the validation indication to be flipped.
        this.isValid = false;

        // A true value indicates this property has an error.
        this.errors[property] = errors;
    }

    /**
     * Creates a new child ModelErrors node attached to the root errors property.
     * This is used when you have a complex (nested) object.
     *
     * @param {string} property
     * @returns {ModelErrors}
     *
     * @memberOf ValidationResult
     */
    public createChildErrors(property: string): ModelErrors {
        return this.createChildErrorsForParent(this.errors, property);
    }

    /**
     * Creates a new child ModelErrors node attached to the provided (parent) errors
     * object. This is used when you have a complex (nested) object.
     *
     * @param {ModelErrors} parent
     * @param {string} property
     * @returns {ModelErrors}
     *
     * @memberOf ValidationResult
     */
    public createChildErrorsForParent(parent: ModelErrors, property: string): ModelErrors {
        const me: ModelErrors = {};
        parent[property] = me;
        return me;
    }

    /**
     * Use this to create an array under the root ModelErrors object.
     *
     * @param {string} property
     * @returns {ModelErrors[]}
     *
     * @memberOf ValidationResult
     */
    public createErrorsArray(property: string): ModelErrors[] {
        return this.createErrorsArrayForParent(this.errors, property);
    }

    /**
     * Used to create an array under the provided parent ModelErrors object.
     *
     * @param {ModelErrors} parent
     * @param {string} property
     * @returns {ModelErrors[]}
     *
     * @memberOf ValidationResult
     */
    public createErrorsArrayForParent(parent: ModelErrors, property: string): ModelErrors[] {
        const arr: ModelErrors[] = [];
        parent[property] = arr;
        return arr;
    }

    /**
     * Creates a new ModelErrors object and adds it to the provided array.
     *
     * @param {ModelErrors[]} array
     * @returns {ModelErrors}
     *
     * @memberOf ValidationResult
     */
    public createErrorsArrayItem(array: ModelErrors[]): ModelErrors {
        const me: ModelErrors = {};
        array.push(me);
        return me;
    }

}
