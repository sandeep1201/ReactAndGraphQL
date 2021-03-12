import { Utilities } from '../utilities';
import { ValidationManager, ValidationResult, ValidationCode } from './validation';

export class FinalistAddress {
  public addressLine1: string;
  public city: string;
  public state: string;
  public zip: string;
  public fullAddress: string;
  public errorMsg: string[];
  public isValid: boolean;
  public isAddressValidated: boolean;
  public useSuggestedAddress: boolean;
  //public resubmitAddress: boolean;
  public useEnteredAddress: boolean;

  public static create(): FinalistAddress {
    const event = new FinalistAddress();
    return event;
  }

  public static clone(input: any, instance: FinalistAddress) {
    instance.addressLine1 = input.addressLine1;
    instance.city = input.city;
    instance.state = input.state;
    instance.zip = input.zip;
    instance.fullAddress = input.fullAddress;
    instance.errorMsg = Utilities.deserilizeArray(input.errorMsg);
    instance.isValid = input.isValid;
    instance.useSuggestedAddress = input.useSuggestedAddress;
    instance.isAddressValidated = true;
    instance.useEnteredAddress = input.useEnteredAddress;
  }

  public deserialize(input: any): FinalistAddress {
    FinalistAddress.clone(input, this);
    return this;
  }

  public validate(validationManager: ValidationManager): boolean {
    let isValid = true;

    if (this.addressLine1 == null || this.addressLine1.toString() === '') {
      isValid = false;
    }
    if (this.city == null || this.city.toString() === '') {
      isValid = false;
    }
    if (this.state == null || this.state.toString() === '') {
      isValid = false;
    }
    if (this.zip == null || this.zip.toString() === '') {
      isValid = false;
    }

    return isValid;
  }

  public validateSave(validationManager: ValidationManager, add = ''): ValidationResult {
    const result = new ValidationResult();
    if (this.addressLine1 == null || this.addressLine1.toString() === '') {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Address Line is required');
      result.addError('addressLine1' + add);
    }
    if (this.city == null || this.city.toString() === '') {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'City is required');
      result.addError('city' + add);
    }
    if (this.state == null || this.state.toString() === '') {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'State is required');
      result.addError('state' + add);
    }
    if (this.zip == null || this.zip.toString() === '') {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Zip is required');
      result.addError('zip' + add);
    }

    return result;
  }
}
