import { Serializable } from '../interfaces/serializable';
import { Clearable } from '../interfaces/clearable';
import { Common } from '../interfaces/common';
import { IsEmpty } from '../interfaces/is-empty';
import { Utilities } from '../utilities';
import { ValidationManager } from './validation-manager';
import { ValidationResult } from './validation-result';
import { MmDdYyyyValidationContext } from '../interfaces/mmDdYyyy-validation-context';
import * as moment from 'moment';

export class Person implements Serializable<Person>, Clearable, Common, IsEmpty {
  public id: number;
  public firstName: string;
  public middleInitial: string;
  public lastName: string;
  public suffix: string;
  public suffixTypeId: number;
  public alias: string;
  public aliasTypeId: number;
  public dateOfBirth: string;
  public snn: string;
  public isNoSsn: boolean;
  public gender: string;
  public genderId: number;

  set dateOfBirthMmDdYyyy(date) {
    this.dateOfBirth = Utilities.mmDdYyyyToDateTime(date);
  }

  get dateOfBirthMmDdYyyy() {
    return Utilities.toMmDdYyyy(this.dateOfBirth);
  }

  public static create() {
    return new Person();
  }

  public static clone(input: any, instance: Person) {
    instance.id = input.id;
    instance.firstName = input.firstName;
    instance.middleInitial = input.middleInitial;
    instance.lastName = input.lastName;
    instance.suffix = input.suffix;
    instance.suffixTypeId = input.suffixTypeId;
    instance.alias = input.alias;
    instance.aliasTypeId = input.aliasTypeId;
    instance.dateOfBirth = input.dateOfBirth;
    instance.snn = input.snn;
    instance.gender = input.gender;
    instance.genderId = input.genderId;
  }

  public deserialize(input: any) {
    Person.clone(input, this);
    return this;
  }

  public clear() {
    this.id = null;
    this.firstName = null;
    this.middleInitial = null;
    this.lastName = null;
    this.suffix = null;
    this.suffixTypeId = null;
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
      (this.suffixTypeId == null || this.suffixTypeId.toString() === '') &&
      (this.dateOfBirth == null || this.dateOfBirth.trim() === '') &&
      (this.snn == null || this.snn.toString().trim() === '') &&
      (this.genderId == null || this.genderId.toString().trim() === '') &&
      (this.gender == null || this.gender.trim() === '')
    );
  }

  public isValidForSearch(validationManager: ValidationManager, currentDateString: string) {
    const result = new ValidationResult();
    const dateOfBirthContext: MmDdYyyyValidationContext = {
      date: this.dateOfBirthMmDdYyyy,
      prop: 'dateOfBirthMmDdYyyy',
      prettyProp: 'Date of Birth',
      result: result,
      validationManager: validationManager,
      isRequired: true,
      minDate: moment(currentDateString)
        .subtract(120, 'years')
        .format('MM/DD/YYYY'),
      minDateAllowSame: true,
      minDateName:
        'date (' +
        moment(currentDateString)
          .subtract(120, 'years')
          .format('MM/DD/YYYY') +
        ')',
      maxDate: moment(currentDateString).format('MM/DD/YYYY'),
      maxDateAllowSame: true,
      maxDateName: 'date (' + moment(currentDateString).format('MM/DD/YYYY') + ')',
      participantDOB: null
    };

    Utilities.validateMmDdYyyyDate(dateOfBirthContext);

    return result;
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
}
