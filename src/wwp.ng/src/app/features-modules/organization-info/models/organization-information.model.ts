// tslint:disable: no-use-before-declare
import { FinalistAddress } from '../../../shared/models/finalist-address.model';
import { Utilities } from '../../../shared/utilities';
import { ValidationManager, ValidationResult, ValidationCode } from 'src/app/shared/models/validation';
import * as moment from 'moment';

export class OrganizationInformation {
  public id: number;
  public programId: number;
  public programName: string;
  public organizationId: number;
  public organizationName: string;
  public locations: FinalistLocation[];
  public modifiedBy: string;
  public modifiedDate: string;

  public static clone(input: any, instance: OrganizationInformation) {
    instance.id = input.id;
    instance.programId = input.programId;
    instance.programName = input.programName;
    instance.organizationId = input.organizationId;
    instance.organizationName = input.organizationName;
    instance.locations = Utilities.deserilizeChildren(input.locations, FinalistLocation, 0);
    instance.modifiedBy = input.modifiedBy;
    instance.modifiedDate = input.modifiedDate;
  }

  public static create(): OrganizationInformation {
    const obj = new OrganizationInformation();
    return obj;
  }

  public deserialize(input: any) {
    OrganizationInformation.clone(input, this);
    return this;
  }
}

export class FinalistLocation {
  public id: number;
  public finalistAddress: FinalistAddress;
  public effectiveDate: string;
  public endDate: string;

  public static clone(input: any, instance: FinalistLocation) {
    instance.id = input.id;
    instance.finalistAddress = Utilities.deserilizeChild(input.finalistAddress, FinalistAddress);
    instance.effectiveDate = input.effectiveDate;
    instance.endDate = input.endDate;
  }

  public static create(): FinalistLocation {
    const obj = new FinalistLocation();
    obj.id = 0;
    return obj;
  }

  public deserialize(input: any) {
    FinalistLocation.clone(input, this);
    return this;
  }

  public validateSave(validationManager: ValidationManager, orgModel: OrganizationInformation, isDisabled: boolean): ValidationResult {
    const result = new ValidationResult();
    const effDate = moment(this.effectiveDate, 'MM/DD/YYYY', true);
    const endDate = moment(this.endDate, 'MM/DD/YYYY', true);

    if (this.effectiveDate == null || this.effectiveDate.toString() === '') {
      validationManager.addErrorWithDetail(ValidationCode.InvalidDate, 'Effective Date is required.');
      result.addError('effectiveDate');
    }
    if (this.effectiveDate && !effDate.isValid()) {
      validationManager.addErrorWithDetail(ValidationCode.InvalidDate, 'Effective Date must be in MM/DD/YYYY format.');
      result.addError('effectiveDate');
    }

    const currentDate = Utilities.currentDate;

    if (isDisabled === false && this.effectiveDate && effDate.isValid() && moment.duration(moment(this.effectiveDate).diff(currentDate)).asDays() > 60) {
      validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, 'Effective Date can be up to 60 days in the future based on the current date.');
      result.addError('effectiveDate');
    }

    if (isDisabled === false && this.effectiveDate && effDate.isValid() && moment.duration(moment(currentDate).diff(this.effectiveDate)).asDays() > 1) {
      validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, 'Effective Date must be on or after current date.');
      result.addError('effectiveDate');
    }

    if (this.endDate && endDate.isValid() && moment.duration(moment(currentDate).diff(this.endDate)).asDays() > 1) {
      validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, 'End Date must be on or after current date.');
      result.addError('endDate');
    } else if (this.endDate && !endDate.isValid()) {
      validationManager.addErrorWithDetail(ValidationCode.InvalidDate, 'End Date must be in MM/DD/YYYY format.');
      result.addError('endDate');
    }

    if (this.effectiveDate && effDate.isValid() && this.endDate && endDate.isValid() && endDate.diff(effDate, 'days') < 0) {
      validationManager.addErrorWithDetail(ValidationCode.InvalidDate, 'End Date must be after Effective Date.');
      result.addError('endDate');
    }

    if (orgModel && orgModel.locations && this.effectiveDate && effDate.isValid()) {
      let isValid = true;
      orgModel.locations.forEach(loc => {
        if (
          this.id !== loc.id &&
          ((effDate.diff(loc.effectiveDate, 'days') >= 0 &&
            (moment(loc.endDate).diff(effDate, 'days') >= 0 || !moment(loc.endDate).isValid()) &&
            (!moment(endDate).isValid() || moment(loc.endDate).diff(effDate, 'days') >= 0 || !moment(loc.endDate).isValid())) ||
            (this.id !== loc.id && moment(loc.effectiveDate).diff(effDate, 'days') >= 0 && (endDate.diff(loc.effectiveDate, 'days') >= 0 || !moment(endDate).isValid())))
        ) {
          isValid = false;
        }
      });
      if (!isValid) {
        validationManager.addErrorWithDetail(ValidationCode.InvalidDate, 'Date fields of a location cannot overlap the date fields of another location.');
        if (isDisabled === false) {
          result.addError('effectiveDate');
          result.addError('endDate');
        } else {
          result.addError('endDate');
        }
      }
    }

    return result;
  }

  public validate(validationManager: ValidationManager): boolean {
    let isValid = false;

    if (!(this.effectiveDate == null || this.effectiveDate.toString() === '')) {
      isValid = true;
    }

    return isValid;
  }

  public clear(): void {
    this.finalistAddress = null;
    this.effectiveDate = null;
    this.endDate = null;
  }

  public isEmpty(): boolean {
    return !this.finalistAddress && (this.effectiveDate === null || this.effectiveDate.trim() === '') && (this.endDate === null || this.endDate.trim() === '');
  }
}
