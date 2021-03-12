import { AppService } from './../../core/services/app.service';

import { ValidationCode, ValidationError } from './validation-error';
import { Injectable } from '@angular/core';
import * as validation from 'lakmus';
import * as moment from 'moment';


@Injectable()
export class ValidationManager {

    public errors: ValidationError[] = [];

    constructor(private appService: AppService) { }

    public resetErrors(): void {
        // Clear out the detail items first.
        for (let ve of this.errors) {
            if (ve.detailItems != null) {
                ve.detailItems.length = 0;
            } else {
                ve.detailItems = [];
            }
        }

        // Now reset the array.
        if (this.errors != null) {
            this.errors.length = 0;
        } else {
            this.errors = [];
        }
    }

    public addError(code: ValidationCode) {
        let existing = this.errors.find(ve => ve.code === code as number);

        if (existing == null) {
            // Make a copy of this so we don't modify the one held in AppService.
            let ve = JSON.parse(JSON.stringify(this.appService.getValidationError(code)));
            // We must update the formatted property as that isn't defined/set in the JSON.
            // For these simple errors, it's just the same as the message.
            ve.formatted = ve.message;
            this.errors.push(ve);
        }
    }

    public addErrorWithDetail(code: ValidationCode, detail: string) {
        let existing = this.errors.find(ve => ve.code === code as number);

        if (existing == null) {
            // Make a copy of this so we don't modify the one held in AppService.
            existing = JSON.parse(JSON.stringify(this.appService.getValidationError(code)));
            // We must update the formatted property as that isn't defined/set in the JSON.
            // For these simple errors, it's just the same as the message.
            existing.formatted = existing.message;
            this.errors.push(existing);
        }

        // Now make sure the detailItems array contains this detail string.
        if (existing.detailItems == null) {
            existing.detailItems = [];
        }

        let existingDetail = existing.detailItems.find(di => di === detail);

        if (existingDetail == null) {
            existing.detailItems.push(detail);
        }
    }

    public addErrorWithFormat(code: ValidationCode, ...values: string[]): void {
        let errorStringified = JSON.stringify(this.appService.getValidationError(code));

        for (const v of values) {
            errorStringified += v;
        }

        let existing = this.errors.find(ve => ve.code + ve.message === errorStringified as string);

        if (existing == null) {
            // Make a copy of this so we don't modify the one held in AppService.
            let x = this.appService.getValidationError(code);
            let ve = JSON.parse(JSON.stringify(x));
            ve.formatted = this.format(ve.message, ...values);
            this.errors.push(ve);
        } else {
            // If one already exists, we'll update it with these latest value.
            existing.formatted = this.format(existing.message, ...values);
        }
    }

    private format(template: string, ...args: string[]): string {
        try {
            let result;
            if (template) {
                result = template.substring(0, template.length);
                for (let i = 0, len = args.length; i < len; i++) {
                    result = result.replace('{' + i + '}', args[i]);
                }
            } else {
                for (let i = 0, len = args.length; i < len; i++) {
                    result = args[i];
                }
            }
            return result;
        } catch (e) {
            return '';
        }
    }
}


