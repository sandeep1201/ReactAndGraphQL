// tslint:disable: no-use-before-declare
import { Person } from './person.model';
import { Phone } from './phone.model';
import { PersonAlias } from './alias.model';
import { Utilities } from '../utilities';
import { SsnAlias } from './SsnAlias.model';
import { SsnPipe } from '../pipes/ssn.pipe';
import { ValidationManager } from '../../shared/models/validation-manager';
import { ValidationResult } from './validation-result';

import { MmYyyyValidationContext } from '../interfaces/mmYyyy-validation-context';
import { MmDdYyyyValidationContext } from '../interfaces/mmDdYyyy-validation-context';

import * as moment from 'moment';
import { DropDownField } from './dropdown-field';
import { FinalistAddress } from './finalist-address.model';

export class ClientRegistration {
  public id: number;
  public mciId: number;
  public pinNumber: number;
  public name: Person;
  public dateOfBirth: string;
  public ssn: string;
  public isNoSsn: boolean;
  public primaryPhone: Phone;
  public secondaryPhone: Phone;
  public emailAddress: string;

  public genderIndicator: string;
  public homeLanguageName: string;
  public homeLanguageId: number;
  public isInterpreterNeeded: boolean;
  public interpreterDetails: string;
  public raceEthnicity: RaceEthnicity;
  public isRefugee: boolean;
  public isPinConfidential: boolean;
  public entryDate: string;
  public refugeeEntryDateUnknown: boolean;
  public originCountryName: string;
  public countryOfOriginId: number;
  public isInTribe: boolean;
  public tribeName: string;
  public tribeId: number;
  public tribalDetails: string;
  public hasAlternativeAliases: boolean;
  public aliases: PersonAlias[];
  public deletedAliases: PersonAlias[] = [];
  public altSsns: SsnAlias[];
  public hasAlternativeSnn: boolean;

  public countyOfResidenceId: number;
  public householdAddress: FinalistAddress;
  public isHomeless: boolean;
  public isMailingSameAsHouseholdAddress: boolean;
  public mailingAddress: FinalistAddress;
  public notes: string;
  public ssnVerificationCode: string;
  public ssnVerificationCodeDescription: string;

  public eaPin: string;
  public eaRequestId: string;
  public inEAMode = false;

  constructor() {
    this.name = new Person();
    this.raceEthnicity = new RaceEthnicity();
    this.primaryPhone = new Phone();
    this.secondaryPhone = new Phone();
    this.householdAddress = new FinalistAddress();
    this.mailingAddress = new FinalistAddress();
    this.altSsns = [];
    this.aliases = [];
  }

  public static clone(input: any, instance: ClientRegistration) {
    instance.id = input.id;
    instance.mciId = input.mciId;
    instance.pinNumber = input.pinNumber;
    instance.name = Utilities.deserilizeChild(input.name, Person);
    instance.dateOfBirth = input.dateOfBirth;
    instance.primaryPhone = Utilities.deserilizeChild(input.primaryPhone, Phone);
    instance.secondaryPhone = Utilities.deserilizeChild(input.secondaryPhone, Phone);
    instance.ssn = input.ssn;
    instance.isNoSsn = input.isNoSsn;
    instance.genderIndicator = input.genderIndicator;
    instance.emailAddress = input.emailAddress;
    instance.homeLanguageName = input.homeLanguageName;
    instance.homeLanguageId = input.homeLanguageId;
    instance.isInterpreterNeeded = input.isInterpreterNeeded;
    instance.interpreterDetails = input.interpreterDetails;
    instance.raceEthnicity = Utilities.deserilizeChild(input.raceEthnicity, RaceEthnicity);
    instance.isRefugee = input.isRefugee;
    instance.isPinConfidential = input.isPinConfidential;
    instance.entryDate = input.entryDate;
    instance.refugeeEntryDateUnknown = input.refugeeEntryDateUnknown;
    instance.originCountryName = input.originCountryName;
    instance.countryOfOriginId = input.countryOfOriginId;
    instance.isInTribe = input.isInTribe;
    instance.tribeName = input.tribeName;
    instance.tribeId = input.tribeId;
    instance.tribalDetails = input.tribalDetails;
    instance.hasAlternativeAliases = input.hasAlternativeAliases;
    instance.aliases = Utilities.deserilizeChildren(input.aliases, PersonAlias, 0);
    instance.deletedAliases = Utilities.deserilizeChildren(input.deletedAliases, PersonAlias, 0);
    instance.altSsns = Utilities.deserilizeChildren(input.altSsns, SsnAlias, 0);
    instance.hasAlternativeSnn = input.hasAlternativeSnn;
    instance.countyOfResidenceId = input.countyOfResidenceId;
    instance.householdAddress = Utilities.deserilizeChild(input.householdAddress, FinalistAddress);
    instance.isHomeless = input.isHomeless;
    instance.isMailingSameAsHouseholdAddress = input.isMailingSameAsHouseholdAddress;
    instance.mailingAddress = Utilities.deserilizeChild(input.mailingAddress, FinalistAddress);
    instance.notes = input.notes;
    instance.ssnVerificationCode = input.ssnVerificationCode;
    instance.ssnVerificationCodeDescription = input.ssnVerificationCodeDescription;
  }

  public deserialize(input: any): ClientRegistration {
    ClientRegistration.clone(input, this);
    return this;
  }

  public cleanseForPost(suffixes: DropDownField[]) {
    const reg = new RegExp('-', 'g');
    const phoneReg = new RegExp('[-)( ]', 'g');

    if (this.name != null && suffixes != null) {
      for (const sd of suffixes) {
        if (sd.id === this.name.suffixTypeId) {
          this.name.suffix = sd.name;
        }
      }
    }

    // if (this.aliases != null && suffixes != null) {
    //     for (const al of this.aliases) {
    //         for (const s of suffixes) {
    //             if (+s.id === +al.suffixTypeId) {
    //                 al.suffixTypeId = null;
    //             }
    //         }
    //     }
    // }

    if (this.aliases != null) {
      for (const al of this.aliases) {
        al.suffixTypeId = null;
      }
    }

    if (this.isNoSsn === true) {
      this.ssn = null;
    }

    if (this.primaryPhone != null && this.primaryPhone.phoneNumber != null) {
      this.primaryPhone.phoneNumber = this.primaryPhone.phoneNumber.replace(phoneReg, '');
    }

    if (this.secondaryPhone != null && this.secondaryPhone.phoneNumber != null) {
      this.secondaryPhone.phoneNumber = this.secondaryPhone.phoneNumber.replace(phoneReg, '');
    }

    if (this.refugeeEntryDateUnknown) this.dateOfEntryMmYyyy = null;
  }

  set dateOfBirthMmDdYyyy(date) {
    this.dateOfBirth = Utilities.mmDdYyyyToDateTime(date);
  }

  get dateOfBirthMmDdYyyy(): string {
    return Utilities.toMmDdYyyy(this.dateOfBirth);
  }

  set dateOfEntryMmYyyy(date) {
    this.entryDate = Utilities.mmYyyyToDateTime(date);
  }

  get dateOfEntryMmYyyy(): string {
    return Utilities.toMmYyyy(this.entryDate);
  }

  get displaySsnVerificationText(): string {
    if (Utilities.isStringEmptyOrNull(this.ssnVerificationCode) && Utilities.isStringEmptyOrNull(this.ssnVerificationCodeDescription)) {
      return '';
    } else {
      return this.ssnVerificationCode + ' - ' + this.ssnVerificationCodeDescription;
    }
  }

  get isFirstNameRequired(): boolean {
    return true;
  }

  get isLastNameRequired(): boolean {
    return true;
  }

  get isDobRequired(): boolean {
    return true;
  }

  get isGenderRequired(): boolean {
    return true;
  }

  get isSsnRequired(): boolean {
    return true;
  }

  get isTribeRequired(): boolean {
    return true;
  }

  get displaySsn(): string {
    if (this.isNoSsn) {
      return 'No SSN';
    } else {
      const sp = new SsnPipe();
      return sp.transform(this.ssn);
    }
  }

  // US 1304.
  get isRefugeeRequired(): boolean {
    return true;
  }

  get isDateOfEntryRequired(): boolean {
    if (this.isRefugee) {
      return true;
    } else {
      return true;
    }
  }

  get isDateOfEntryDisabled() {
    if (this.refugeeEntryDateUnknown === true) {
      return true;
    } else {
      return false;
    }
  }

  get isCountryOriginRequired(): boolean {
    if (this.isRefugee) {
      return true;
    } else {
      return true;
    }
  }

  // US 1307: Make PIN Confidential required.
  get isPinConfidentialRequired(): boolean {
    return true;
  }

  get isMailingSameAsHouseholdAddressRequired(): boolean {
    return true;
  }

  get isHouseholdSameMailingAddressDisplayed(): boolean {
    if (!this.isHomeless) {
      return true;
    }
  }

  get isMailingAddressDisplayed(): boolean {
    if (this.isMailingSameAsHouseholdAddress === false || this.isHomeless === true) {
      return true;
    } else {
      return false;
    }
  }

  // Dont ask interpreter questions when home lang is english.
  isInterpreterNeededDisplayed(englishId: number): boolean {
    if (this.homeLanguageId == null) {
      return false;
    } else if (+this.homeLanguageId === +englishId) {
      return false;
    } else {
      return true;
    }
  }

  get isTribeDisplayed(): boolean {
    if (this.isInTribe) {
      return true;
    } else {
      return false;
    }
  }

  isTribeDetailsDisplayed(otherId: number): boolean {
    if (this.tribeId == null) {
      return false;
    } else if (+this.tribeId === +otherId && this.isInTribe) {
      return true;
    } else {
      return false;
    }
  }

  public validate(validationManager: ValidationManager, englishId: number, isPinConfidentialReadOnly: boolean, currentDateString: string, isEAMode: boolean): ValidationResult {
    const result = new ValidationResult();

    Utilities.validateRequiredText(this.name.firstName, 'firstName', 'First Name', result, validationManager);
    Utilities.validateRequiredText(this.name.lastName, 'lastName', 'Last Name', result, validationManager);

    const invalidChars = ['&', '%'];
    Utilities.validateInvalidCharInText(this.name.firstName, invalidChars, 'firstName', 'First Name', result, validationManager);
    Utilities.validateInvalidCharInText(this.name.lastName, invalidChars, 'lastName', 'Last Name', result, validationManager);

    const validMiddleChars = /[a-zA-Z]/;
    if (!Utilities.isStringEmptyOrNull(this.name.middleInitial)) {
      Utilities.validateTextWithRegEx(this.name.middleInitial, 'middleInitial', 'Middle Initial', 'a non-letter', validMiddleChars, result, validationManager);
    }

    const validNameRegex = /(^[a-zA-Z])([a-z0-9!#$`()*+,-./:;=?@[\]^_'{|}~ ]*)/;
    if (!Utilities.isStringEmptyOrNull(this.name.firstName)) {
      Utilities.validateTextWithRegEx(
        this.name.firstName,
        'firstName',
        'First Name',
        "start with a non-letter and only contain alphanumeric characters, spaces, and !#`()*+,-./:;=?@[\\]^_'}|{~$",
        validNameRegex,
        result,
        validationManager
      );
    }
    if (!Utilities.isStringEmptyOrNull(this.name.lastName)) {
      Utilities.validateTextWithRegEx(
        this.name.lastName,
        'lastName',
        'Last Name',
        "start with a non-letter and only contain alphanumeric characters, spaces, and !#`()*+,-./:;=?@[\\]^_'}|{~$",
        validNameRegex,
        result,
        validationManager
      );
    }

    if (this.isNoSsn !== true && !Utilities.validateRequiredText(this.ssn, 'ssn', 'SSN/ITIN', result, validationManager)) {
      Utilities.validateSnn(this.ssn, 'ssn', 'SSN', result, validationManager);
    }

    Utilities.validateDropDown(this.genderIndicator, 'gender', 'Gender', result, validationManager);

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

    // Dont validate when disabled.
    if (isPinConfidentialReadOnly !== true) {
      Utilities.validateRequiredYesNo(result, validationManager, this.isPinConfidential, 'isPinConfidential', 'Confidential Pin');
    }

    let allAliaseAreEmpty = true;

    // Check to see if all are empty.
    for (const al of this.aliases) {
      if (!al.isEmpty()) {
        allAliaseAreEmpty = false;
        break;
      }
    }

    const errArr = result.createErrorsArray('aliases');

    if (allAliaseAreEmpty) {
      // If all are empty validate the first one.
      if (this.aliases[0] != null) {
        const resultAliases = new ValidationResult();
        Utilities.validateInvalidCharInText(this.aliases[0].firstName, invalidChars, 'firstName', 'Alias - First Name', resultAliases, validationManager);
        Utilities.validateInvalidCharInText(this.aliases[0].lastName, invalidChars, 'lastName', 'Alias - Last Name', resultAliases, validationManager);
        Utilities.validateRequiredText(this.aliases[0].firstName, 'firstName', 'Alias - First Name', resultAliases, validationManager);
        Utilities.validateRequiredText(this.aliases[0].lastName, 'lastName', 'Alias - Last Name', resultAliases, validationManager);
        Utilities.validateTextWithRegEx(
          this.aliases[0].firstName,
          'firstName',
          'Alias - First Name',
          "start with a non-letter and only contain alphanumeric characters, spaces, and !#`()*+,-./:;=?@[\\]^_'}|{~$",
          validNameRegex,
          resultAliases,
          validationManager
        );
        Utilities.validateTextWithRegEx(
          this.aliases[0].lastName,
          'lastName',
          'Alias - Last Name',
          "start with a non-letter and only contain alphanumeric characters, spaces, and !#`()*+,-./:;=?@[\\]^_'}|{~$",
          validNameRegex,
          resultAliases,
          validationManager
        );
        Utilities.validateDropDown(this.aliases[0].alias, 'alias', 'Alias Type', resultAliases, validationManager);

        errArr.push(resultAliases.errors);
        if (resultAliases.isValid === false) {
          result.isValid = false;
        }
      }
    } else {
      for (const al of this.aliases) {
        if (!al.isEmpty()) {
          const resultAliases = new ValidationResult();
          Utilities.validateInvalidCharInText(al.firstName, invalidChars, 'firstName', 'Alias - First Name', resultAliases, validationManager);
          Utilities.validateInvalidCharInText(al.lastName, invalidChars, 'lastName', 'Alias - Last Name', resultAliases, validationManager);
          Utilities.validateRequiredText(al.firstName, 'firstName', 'Alias - First Name', resultAliases, validationManager);
          Utilities.validateRequiredText(al.lastName, 'lastName', 'Alias - Last Name', resultAliases, validationManager);
          Utilities.validateTextWithRegEx(
            al.firstName,
            'firstName',
            'Alias - First Name',
            "start with a non-letter and only contain alphanumeric characters, spaces, and !#`()*+,-./:;=?@[\\]^_'}|{~$",
            validNameRegex,
            resultAliases,
            validationManager
          );

          Utilities.validateTextWithRegEx(
            al.lastName,
            'lastName',
            'Alias - Last Name',
            "start with a non-letter and only contain alphanumeric characters, spaces, and !#`()*+,-./:;=?@[\\]^_'}|{~$",
            validNameRegex,
            resultAliases,
            validationManager
          );
          Utilities.validateDropDown(al.alias, 'alias', 'Alias Type', resultAliases, validationManager);

          const currentString = JSON.stringify(al);
          const allStrings = [];
          for (const all of this.aliases) {
            allStrings.push(JSON.stringify(all));
          }

          Utilities.validateDupData(currentString, allStrings, '', 'Duplicates', resultAliases, validationManager);
          errArr.push(resultAliases.errors);
          if (resultAliases.isValid === false) {
            result.isValid = false;
          }
        } else {
          errArr.push({});
        }
      }
    }

    // Validates only until here in case of user is EA
    if (isEAMode) {
      return result;
    }

    let allAltSsnsAreEmpty = true;

    // Check to see if all are empty.
    for (const al of this.altSsns) {
      if (!al.isEmpty()) {
        allAltSsnsAreEmpty = false;
        break;
      }
    }

    const errArrAlt = result.createErrorsArray('altSsns');

    if (allAltSsnsAreEmpty) {
      // If all are empty validate the first one.
      if (this.altSsns[0] != null) {
        const resultAltSsn = new ValidationResult();
        Utilities.validateSnn(this.altSsns[0].ssn, 'ssn', 'ITIN/Alias SSN', resultAltSsn, validationManager);
        Utilities.validateDropDown(this.altSsns[0].typeId, 'typeId', 'SSN Type', resultAltSsn, validationManager);
        errArrAlt.push(resultAltSsn.errors);
        if (resultAltSsn.isValid === false) {
          result.isValid = false;
        }
      }
    } else {
      for (const al of this.altSsns) {
        if (!al.isEmpty()) {
          const resultAltSsn = new ValidationResult();
          Utilities.validateSnn(al.ssn, 'ssn', 'ITIN/Alias SSN', resultAltSsn, validationManager);
          Utilities.validateDropDown(al.typeId, 'typeId', 'SSN Type', resultAltSsn, validationManager);
          errArrAlt.push(resultAltSsn.errors);
          if (resultAltSsn.isValid === false) {
            result.isValid = false;
          }
        } else {
          errArrAlt.push({});
        }
      }
    }

    if (this.primaryPhone.canText || (this.primaryPhone.canVoiceMail && (this.primaryPhone.phoneNumber === null || this.primaryPhone.phoneNumber.toString() === ''))) {
      Utilities.validateRequiredText(this.primaryPhone.phoneNumber, 'primaryPhone.phoneNumber', 'Primary Phone', result, validationManager);
    }

    if (this.secondaryPhone.canText || (this.secondaryPhone.canVoiceMail && (this.secondaryPhone.phoneNumber === null || this.secondaryPhone.phoneNumber.toString() === ''))) {
      Utilities.validateRequiredText(this.secondaryPhone.phoneNumber, 'secondaryPhone.phoneNumber', 'Secondary Phone', result, validationManager);
    }

    if (!Utilities.isStringEmptyOrNull(this.emailAddress)) {
      Utilities.validateEmail(this.emailAddress, 'emailAddress', 'Email Address', result, validationManager);
    }

    Utilities.validateDropDown(this.homeLanguageId, 'homeLanguageId', 'Home Language', result, validationManager);

    if (this.isInterpreterNeededDisplayed(englishId)) {
      Utilities.validateRequiredYesNo(result, validationManager, this.isInterpreterNeeded, 'isInterpreterNeeded', 'Interpreter Needed?');
    }

    Utilities.validateRequiredYesNo(result, validationManager, this.isRefugee, 'isRefugee', 'Refugee?');

    if (this.isRefugee) {
      if (this.isDateOfEntryDisabled === false) {
        // Must be after DOB.
        // Cannot be in future.
        const dateOfEntryContext: MmYyyyValidationContext = {
          date: this.dateOfEntryMmYyyy,
          prop: 'dateOfEntryMmYyyy',
          prettyProp: 'Month Of Entry ',
          result: result,
          validationManager: validationManager,
          isRequired: true,
          minDate: moment(this.dateOfBirth).format('MM/DD/YYYY'),
          minDateAllowSame: true,
          minDateName: "Participant's DOB",
          maxDate: currentDateString,
          maxDateAllowSame: true,
          maxDateName: 'Current Date',
          participantDOB: null
        };
        Utilities.validateMmYyyyDate(dateOfEntryContext);
      }

      Utilities.validateDropDown(this.countryOfOriginId, 'countryOfOriginId', 'Country of Origin', result, validationManager);
    }

    Utilities.validateDropDown(this.countyOfResidenceId, 'countyOfResidenceId', 'County of Residence', result, validationManager);

    // Check for required fields: location
    if (this.isHomeless !== true) {
      if (!this.isMailingAddressDisplayed) {
        Utilities.validateRequiredText(this.householdAddress.addressLine1, 'householdAddress-AddressLine1', 'AddressLine1', result, validationManager);
        Utilities.validateRequiredText(this.householdAddress.city, 'householdAddress-City', 'City', result, validationManager);
        Utilities.validateRequiredText(this.householdAddress.zip, 'householdAddress-Zip', 'Zip', result, validationManager);
      }
    }

    if (this.isHouseholdSameMailingAddressDisplayed) {
      Utilities.validateRequiredYesNo(
        result,
        validationManager,
        this.isMailingSameAsHouseholdAddress,
        'isMailingSameAsHouseholdAddress',
        'Is your household address the same as your mailing address?'
      );
    }

    if (this.isMailingAddressDisplayed) {
      if (this.mailingAddress) {
        Utilities.validateRequiredText(this.mailingAddress.addressLine1, 'mailingAddress-AddressLine1', 'AddressLine1', result, validationManager);
        Utilities.validateRequiredText(this.mailingAddress.city, 'mailingAddress-City', 'City', result, validationManager);
        Utilities.validateRequiredText(this.mailingAddress.zip, 'mailingAddress-Zip', 'Zip', result, validationManager);
      }
    }
    return result;
  }
}

export class RaceEthnicity {
  isAmericanIndian: boolean;
  isPacificIslander: boolean;
  isAsian: boolean;
  isWhite: boolean;
  isBlack: boolean;
  isHispanic: boolean;
  historySequenceNumber: number;

  public static clone(input: any, instance: RaceEthnicity) {
    instance.isAmericanIndian = input.isAmericanIndian;
    instance.isPacificIslander = input.isPacificIslander;
    instance.isAsian = input.isAsian;
    instance.isWhite = input.isWhite;
    instance.isBlack = input.isBlack;
    instance.isHispanic = input.isHispanic;
    instance.historySequenceNumber = input.historySequenceNumber;
  }

  public deserialize(input: any): RaceEthnicity {
    RaceEthnicity.clone(input, this);
    return this;
  }
}
