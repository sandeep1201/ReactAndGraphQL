import * as _ from 'lodash';
import { PersonAlias } from './alias.model';
import { PersonName } from './person-name.model';
import { MmDdYyyyValidationContext } from '../interfaces/mmDdYyyy-validation-context';
import { SsnPipe } from '../pipes/ssn.pipe';
import { PadPipe } from '../pipes/pad.pipe';
import { ValidationManager } from './validation-manager';
import { ValidationResult } from './validation-result';
import { Utilities } from '../utilities';

import * as moment from 'moment';

export class MciParticipantSearch {
    id: number;
    name: PersonName;
    dateOfBirth: string;
    ssn: string;
    isNoSsn: boolean;
    gender: string;
    aliases: PersonAlias[];

    set dateOfBirthMmDdYyyy(date) {
        this.dateOfBirth = Utilities.mmDdYyyyToDateTime(date);
    }

    get dateOfBirthMmDdYyyy() {
        return Utilities.toMmDdYyyy(this.dateOfBirth);
    }

    public static clone(input: any, instance: MciParticipantSearch) {
        instance.id = input.id;
        instance.name = Utilities.deserilizeChild(input.name, PersonName);
        instance.dateOfBirth = input.dateOfBirth;
        instance.ssn = input.ssn;
        instance.isNoSsn = input.isNoSsn;
        instance.gender = input.gender;
        instance.aliases = Utilities.deserilizeChildren(input.aliases, PersonAlias, 0);
    }

    public deserialize(input: any) {
        MciParticipantSearch.clone(input, this);
        return this;
    }


    public isValidForSearch(validationManager: ValidationManager, currentDateString: string): ValidationResult {
        const result = new ValidationResult();
        Utilities.validateRequiredText(this.name.firstName, 'firstName', 'First Name', result, validationManager);
        Utilities.validateRequiredText(this.name.lastName, 'lastName', 'Last Name', result, validationManager);

        const invalidChars = ['&', '%'];
        Utilities.validateInvalidCharInText(this.name.firstName, invalidChars, 'firstName', 'First Name', result, validationManager);
        Utilities.validateInvalidCharInText(this.name.lastName, invalidChars, 'lastName', 'Last Name', result, validationManager);

        const validMiddleChars = /[a-zA-Z]/;
        if (!Utilities.isStringEmptyOrNull(this.name.middleInitial)) {
            Utilities.validateTextWithRegEx(this.name.middleInitial, 'middleInitial', 'Middle Initial', 'a letter', validMiddleChars, result, validationManager);
        }

        const validNameRegex = /(^[a-zA-Z])([a-z0-9!#$`()*+,-./:;=?@[\]^_'{|}~ ]*)/;
        if (!Utilities.isStringEmptyOrNull(this.name.firstName)) {
            Utilities.validateTextWithRegEx(this.name.firstName,
                'firstName', 'First Name', 'start with a letter and only contain alphanumeric characters, spaces, and !#`()*+,-.\/:;=?@[\\]^_\'}|{~$',
                validNameRegex, result, validationManager);
        }
        if (!Utilities.isStringEmptyOrNull(this.name.lastName)) {
            Utilities.validateTextWithRegEx(this.name.lastName, 'lastName', 'Last Name', 'start with a letter and only contain alphanumeric characters, spaces, and !#`()*+,-.\/:;=?@[\\]^_\'}|{~$',
                validNameRegex, result, validationManager);
        }

        if (this.isNoSsn !== true && !Utilities.validateRequiredText(this.ssn, 'ssn', 'SSN/ITIN', result, validationManager)) {
            Utilities.validateSnn(this.ssn, 'ssn', 'SSN', result, validationManager);
        }

        Utilities.validateDropDown(this.gender, 'gender', 'Gender', result, validationManager);

        const dateOfBirthContext: MmDdYyyyValidationContext = {
            date: this.dateOfBirthMmDdYyyy,
            prop: 'dateOfBirthMmDdYyyy',
            prettyProp: 'Date of Birth',
            result: result,
            validationManager: validationManager,
            isRequired: true,
            minDate: moment(currentDateString).subtract(120, 'years').format('MM/DD/YYYY'),
            minDateAllowSame: true,
            minDateName: 'date (' + moment(currentDateString).subtract(120, 'years').format('MM/DD/YYYY') + ')',
            maxDate: moment(currentDateString).format('MM/DD/YYYY'),
            maxDateAllowSame: true,
            maxDateName: 'date (' + moment(currentDateString).format('MM/DD/YYYY') + ')',
            participantDOB: null
        };
        Utilities.validateMmDdYyyyDate(dateOfBirthContext);

        let allItemsAreEmpty = true;

        // Check to see if all are empty.
        for (const al of this.aliases) {
            if (!al.isEmpty()) {
                allItemsAreEmpty = false;
                break;
            }
        }

        const errArr = result.createErrorsArray('aliases');

        if (allItemsAreEmpty) {
            // If all are empty validate the first one.
            if (this.aliases[0] != null) {
                const resultAliases = new ValidationResult();
                Utilities.validateInvalidCharInText(this.aliases[0].firstName, invalidChars, 'firstName', 'Alias - First Name', resultAliases, validationManager);
                Utilities.validateInvalidCharInText(this.aliases[0].lastName, invalidChars, 'lastName', 'Alias - Last Name', resultAliases, validationManager);
                Utilities.validateRequiredText(this.aliases[0].firstName, 'firstName', 'Alias - First Name', resultAliases, validationManager);
                Utilities.validateRequiredText(this.aliases[0].lastName, 'lastName', 'Alias - Last Name', resultAliases, validationManager);
                Utilities.validateTextWithRegEx(this.aliases[0].firstName,
                    'firstName', 'Alias - First Name', 'start with a non-letter and only contain alphanumeric characters, spaces, and !#`()*+,-.\/:;=?@[\\]^_\'}|{~$',
                    validNameRegex, resultAliases, validationManager);
                Utilities.validateTextWithRegEx(this.aliases[0].lastName,
                    'lastName', 'Alias - Last Name', 'start with a non-letter and only contain alphanumeric characters, spaces, and !#`()*+,-.\/:;=?@[\\]^_\'}|{~$',
                    validNameRegex, resultAliases, validationManager);
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
                    Utilities.validateTextWithRegEx(al.firstName,
                        'firstName', 'Alias - First Name', `start with a letter and only contain alphanumeric characters, spaces, and !#'()*+,-,/:;=?@[]^_'}|{~$`,
                        validNameRegex, resultAliases, validationManager);
                    Utilities.validateTextWithRegEx(al.lastName,
                        'lastName', 'Alias - Last Name', `start with a letter and only contain alphanumeric characters, spaces, and !#'()*+,-,/:;=?@[]^_'}|{~$`,
                        validNameRegex, resultAliases, validationManager);
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

        return result;
    }


    public getCleansedSearchModel(model: MciParticipantSearch): MciParticipantSearch {
        const modelCloned = new MciParticipantSearch();
        MciParticipantSearch.clone(model, modelCloned);
        if (modelCloned.isNoSsn) {
            modelCloned.ssn = null;
        }
        return modelCloned;
    }

    get fullNameWithMiddleInitialSuffixTitleCase(): string {
        if (this.name != null) {
            return Utilities.formatDisplayPersonName(this.name.firstName, this.name.middleInitial, this.name.lastName, this.name.suffix);
        }
    }

    get displaySsn(): string {
        if (this.isNoSsn) {
            return 'No SSN';
        } else {
            // Pad first before formatting.
            const pp = new PadPipe();
            const paddedSsn = pp.transform(this.ssn, 9);
            const sp = new SsnPipe();
            return sp.transform(paddedSsn);
        }
    }

}
