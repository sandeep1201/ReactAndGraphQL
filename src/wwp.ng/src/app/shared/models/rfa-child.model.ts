import { AppService } from 'src/app/core/services/app.service';
import { MmDdYyyyValidationContext } from '../interfaces/mmDdYyyy-validation-context';
import { Serializable } from '../interfaces/serializable';
import { Clearable } from '../interfaces/clearable';
import { Common } from '../interfaces/common';
import { IsEmpty } from '../interfaces/is-empty';
import * as moment from 'moment';
import { Utilities } from '../utilities';
import { ValidationManager, ValidationResult } from './validation';

export class RfaChild implements Serializable<RfaChild>, Clearable, Common, IsEmpty {
  public id: number;
  public firstName: string;
  public middleInitial: string;
  public lastName: string;
  public suffix: string;
  public dateOfBirth: string;
  public snn: string;
  public gender: string;
  public genderId: number;

  public static create() {
    const rfa = new RfaChild();
    rfa.id = 0;
    return rfa;
  }

  public static clone(input: any, instance: RfaChild) {
    instance.id = input.id;
    instance.firstName = input.firstName;
    instance.middleInitial = input.middleInitial;
    instance.lastName = input.lastName;
    instance.suffix = input.suffix;
    instance.dateOfBirth = input.dateOfBirth;
    instance.snn = input.snn;
    instance.gender = input.gender;
    instance.genderId = input.genderId;
  }

  public deserialize(input: any) {
    RfaChild.clone(input, this);
    return this;
  }

  public clear() {
    this.id = null;
    this.firstName = null;
    this.middleInitial = null;
    this.lastName = null;
    this.suffix = null;
    this.dateOfBirth = null;
    this.snn = null;
    this.gender = null;
    this.genderId = null;
  }

  public isEmpty(): boolean {
    return (
      (this.firstName == null || this.firstName.trim() === '') &&
      (this.middleInitial == null || this.middleInitial.toString() === '') &&
      (this.lastName == null || this.lastName.trim() === '') &&
      (this.suffix == null || this.suffix.trim() === '') &&
      (this.dateOfBirth == null || this.dateOfBirth.trim() === '') &&
      (this.snn == null || this.snn.toString().trim() === '') &&
      (this.genderId == null || this.genderId.toString().trim() === '') &&
      (this.gender == null || this.gender.trim() === '')
    );
  }

  public validate(validationManager: ValidationManager, result?: ValidationResult): ValidationResult {
    if (result == null) {
      result = new ValidationResult();
    }

    Utilities.validateRequiredText(this.firstName, 'firstName', 'First Name', result, validationManager);
    Utilities.validateRequiredText(this.lastName, 'lastName', 'Last Name', result, validationManager);

    if (this.isDateOfBirthRequired) {
      const dobContext: MmDdYyyyValidationContext = {
        date: this.dateOfBirth,
        prop: 'dateOfBirth',
        prettyProp: 'DOB',
        result: result,
        validationManager: validationManager,
        isRequired: true,
        minDate: moment(Utilities.currentDate)
          .subtract(18, 'years')
          .format('MM/DD/YYYY'),
        minDateAllowSame: true,
        minDateName: moment(Utilities.currentDate)
          .subtract(18, 'years')
          .format('MM/DD/YYYY'),
        maxDate: moment(Utilities.currentDate).format('MM/DD/YYYY'),
        maxDateAllowSame: true,
        maxDateName: 'Current Date',
        participantDOB: null
      };
      Utilities.validateMmDdYyyyDate(dobContext);
    }

    if (this.isGenderRequired) {
      Utilities.validateDropDown(this.genderId, 'genderId', 'Gender', result, validationManager);
    }

    return result;
  }

  set dobMmDdYyyy(date) {
    if (date != null && date.length === 10 && moment(date).isValid()) {
      this.dateOfBirth = moment(date).toISOString();
    } else {
      this.dateOfBirth = date;
    }
  }
  get dobMmDdYyyy() {
    if (this.dateOfBirth != null && moment(this.dateOfBirth, moment.ISO_8601).isValid()) {
      return moment(this.dateOfBirth, moment.ISO_8601).format('MM/DD/YYYY');
    } else {
      return this.dateOfBirth;
    }
  }

  // US 1321: First Name is a REQUIRED FIELD.
  get isFirstNameRequired(): boolean {
    return true;
  }

  // US 1321: Last Name is a REQUIRED FIELD.
  get isLastNameRequired(): boolean {
    return true;
  }

  // US 1321: Date Of Birth is a REQUIRED FIELD.
  get isDateOfBirthRequired(): boolean {
    return true;
  }

  // US 1321: Gender is a REQUIRED FIELD.
  get isGenderRequired(): boolean {
    return true;
  }

  get age() {
    return moment().diff(this.dateOfBirth, 'years');
  }
}
