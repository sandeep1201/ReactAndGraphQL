import { Serializable } from '../interfaces/serializable';
import { ValidationManager } from './validation-manager';
import { ValidationResult } from './validation-result';
import * as moment from 'moment';
import { ValidationCode } from './validation-error';
import { Utilities } from '../utilities';

export class ContactInfo implements Serializable<ContactInfo> {
    public id: number;
    public workerId: string;
    public phoneNumber: string;
    public email: string;
    public isDeleted: boolean;
    public modifiedBy: string;
    public modifiedDate: string;
    public rowVersion: string;    

  public static create(): ContactInfo {
    const contactInfo = new ContactInfo();
    contactInfo.id = 0;
    contactInfo.phoneNumber = '';
    contactInfo.email = '';
    return contactInfo;
  }

  public static clone(input: any, instance: ContactInfo) {
    instance.id = input.id == null ? 0:input.id;
    instance.workerId = input.workerId;
    instance.phoneNumber = input.phoneNumber == 0 ? '':input.phoneNumber;
    instance.email = input.email;
    instance.isDeleted = input.isDeleted;
    instance.modifiedBy = input.modifiedBy;
    instance.modifiedDate = input.modifiedDate;
    instance.rowVersion = input.rowVersion;
  }

  public deserialize(input: any) {
    ContactInfo.clone(input, this);
    return this;
  }

  validate(validationManager: ValidationManager): ValidationResult {
    const result = new ValidationResult();

    if (this.phoneNumber === null || this.phoneNumber.toString() === '') {
      Utilities.validateRequiredText(this.phoneNumber, 'phoneNumber', 'Phone', result, validationManager);
    }  
    
    if (this.email === null || this.email.toString() === '' || this.email.trim() === '') {
      Utilities.validateRequiredText(this.email, 'email', 'Email Address', result, validationManager);
    }

    if (!Utilities.isStringEmptyOrNull(this.email)) {
      Utilities.validateEmail(this.email.trim(), 'email', 'Email Address', result, validationManager);
    }
   
    return result;
  }
}
