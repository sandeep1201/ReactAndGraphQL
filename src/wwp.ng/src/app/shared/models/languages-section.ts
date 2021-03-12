// tslint:disable: no-use-before-declare
import { Serializable } from '../interfaces/serializable';
import { ValidationCode } from './validation-error';
import { ValidationManager } from './validation-manager';
import { ValidationResult } from './validation-result';
import { Utilities } from '../utilities';
import { Clearable } from '../interfaces/clearable';
import { IsEmpty } from '../interfaces/is-empty';

export class LanguagesSection implements Serializable<LanguagesSection> {
  isSubmittedViaDriverFlow: boolean;
  homeLanguageId: number;
  homeLanguageTypeId: number;
  homeLanguageName: string;
  isAbleToReadHomeLanguage: boolean;
  isAbleToWriteHomeLanguage: boolean;
  isAbleToSpeakHomeLanguage: boolean;
  isAbleToReadEnglish: boolean;
  isAbleToWriteEnglish: boolean;
  isAbleToSpeakEnglish: boolean;
  knownLanguages: KnownLanguage[] = [];
  notes: string;
  isNeedingInterpreter: boolean;
  interpreterDetails: string;
  rowVersion: string;
  assessmentRowVersion: string;
  modifiedBy: string;
  modifiedDate: string;

  /**
   *  instance is the new clone.
   *
   * @static
   * @param {*} input
   * @param {LanguagesSection} instance
   * @memberof LanguagesSection
   */
  public static clone(input: any, instance: LanguagesSection) {
    instance.isSubmittedViaDriverFlow = input.isSubmittedViaDriverFlow;
    instance.homeLanguageId = input.homeLanguageId;
    instance.homeLanguageTypeId = input.homeLanguageTypeId;
    instance.homeLanguageName = input.homeLanguageName;
    instance.isAbleToReadHomeLanguage = input.isAbleToReadHomeLanguage;
    instance.isAbleToWriteHomeLanguage = input.isAbleToWriteHomeLanguage;
    instance.isAbleToSpeakHomeLanguage = input.isAbleToSpeakHomeLanguage;
    instance.isAbleToReadEnglish = input.isAbleToReadEnglish;
    instance.isAbleToWriteEnglish = input.isAbleToWriteEnglish;
    instance.isAbleToSpeakEnglish = input.isAbleToSpeakEnglish;
    instance.knownLanguages = Utilities.deserilizeChildren(input.knownLanguages, KnownLanguage);
    instance.notes = input.notes;
    instance.isNeedingInterpreter = input.isNeedingInterpreter;
    instance.interpreterDetails = input.interpreterDetails;
    instance.rowVersion = input.rowVersion;
    instance.modifiedBy = input.modifiedBy;
    instance.modifiedDate = input.modifiedDate;
  }

  public deserialize(input: any) {
    LanguagesSection.clone(input, this);
    return this;
  }

  isHomeLangRequired(): boolean {
    if (this.homeLanguageTypeId != null || this.isAbleToReadHomeLanguage != null || this.isAbleToWriteHomeLanguage != null || this.isAbleToSpeakHomeLanguage != null) {
      return true;
    } else {
      return false;
    }
  }

  /**
   * Checks the Contact object by applying provided business rules that indicate if this
   * instance is valid.  Specific validation errors are indicated by the errors property.
   *
   * @param {ValidationManager} validationManager
   * @returns {ValidationResult}
   *
   * @memberOf Contact
   */
  public validate(validationManager: ValidationManager, englishId: number): ValidationResult {
    const result = new ValidationResult();

    Utilities.validateDropDown(this.homeLanguageTypeId, 'homeLanguageTypeId', 'What language do you use at home?', result, validationManager);

    Utilities.validateRequiredYesNo(result, validationManager, this.isAbleToReadHomeLanguage, 'isAbleToReadHomeLanguage', 'Home Language - Read');
    Utilities.validateRequiredYesNo(result, validationManager, this.isAbleToWriteHomeLanguage, 'isAbleToWriteHomeLanguage', 'Home Language - Write');
    Utilities.validateRequiredYesNo(result, validationManager, this.isAbleToSpeakHomeLanguage, 'isAbleToSpeakHomeLanguage', 'Home Language - Speak');

    if (this.isAbleToReadHomeLanguage === false && this.isAbleToWriteHomeLanguage === false && this.isAbleToSpeakHomeLanguage === false) {
      result.addError('isAbleToReadHomeLanguage');
      result.addError('isAbleToWriteHomeLanguage');
      result.addError('isAbleToSpeakHomeLanguage');
      validationManager.addError(ValidationCode.LanguageInfoIncomplete);
    }

    if (this.homeLanguageTypeId != null && this.homeLanguageTypeId !== englishId) {
      Utilities.validateRequiredYesNo(result, validationManager, this.isAbleToReadEnglish, 'isAbleToReadEnglish', 'English - Read');
      Utilities.validateRequiredYesNo(result, validationManager, this.isAbleToWriteEnglish, 'isAbleToWriteEnglish', 'English - Write');
      Utilities.validateRequiredYesNo(result, validationManager, this.isAbleToSpeakEnglish, 'isAbleToSpeakEnglish', 'English - Speak');
      // FYI -- No No No is fine for English
    }

    const knownLangErrors = result.createErrorsArray('knownLanguages');

    if (this.knownLanguages != null) {
      // Adding the homeLanguageId and englishId explicitly instead of the through new Set(iterator) because it's not supported in IE11
      const uniqueLanguages = new Set();
      uniqueLanguages.add(this.homeLanguageTypeId);
      uniqueLanguages.add(englishId);
      let langIsUnique: Boolean;
      for (const lang of this.knownLanguages) {
        if (!uniqueLanguages.has(lang.languageId)) {
          uniqueLanguages.add(lang.languageId);
          langIsUnique = true;
        } else {
          langIsUnique = false;
        }
        const valResult = lang.validate(validationManager, langIsUnique);
        knownLangErrors.push(valResult.errors);
        if (valResult.isValid === false) {
          result.isValid = false;
        }
      }
    }

    if (this.homeLanguageTypeId != null && this.homeLanguageTypeId !== englishId) {
      Utilities.validateRequiredYesNo(result, validationManager, this.isNeedingInterpreter, 'isNeedingInterpreter', 'Do you need an interpreter?');
    }

    return result;
  }
}

export class KnownLanguage implements Clearable, IsEmpty, Serializable<KnownLanguage> {
  id: number;
  languageId: number;
  languageName: string;
  canRead: boolean;
  canSpeak: boolean;
  canWrite: boolean;

  /**
   * Creates a new object suitable to be bound to the UI and passed to the API.
   * It will set the ID's to appropriate values.
   */
  public static create(): KnownLanguage {
    const x = new KnownLanguage();
    x.id = 0;
    return x;
  }

  private static graphObj(input: any, instance: KnownLanguage) {
    instance.id = input.id;
    instance.languageId = input.languageId;
    instance.languageName = input.languageName;
    instance.canRead = input.canRead;
    instance.canSpeak = input.canSpeak;
    instance.canWrite = input.canWrite;
  }

  public deserialize(input: any) {
    KnownLanguage.graphObj(input, this);
    return this;
  }

  validate(validationManager: ValidationManager, langIsUnique: Boolean): ValidationResult {
    const result = new ValidationResult();
    if (this.isRequired() === true) {
      Utilities.validateDropDown(this.languageId, 'languageId', 'List any other languages you know:', result, validationManager);
      Utilities.validateRequiredYesNo(result, validationManager, this.canRead, 'canRead', 'Other Language  - Read');
      Utilities.validateRequiredYesNo(result, validationManager, this.canWrite, 'canWrite', 'Other Language  - Write');
      Utilities.validateRequiredYesNo(result, validationManager, this.canSpeak, 'canSpeak', 'Other Language  - Speak');

      if (langIsUnique === false) {
        validationManager.addError(ValidationCode.LanguageMustBeUnique);
        result.addError('languageId');
      }

      if (this.canRead === false && this.canSpeak === false && this.canWrite === false) {
        validationManager.addError(ValidationCode.LanguageInfoIncomplete);
        result.addError('canRead');
        result.addError('canWrite');
        result.addError('canSpeak');
      }
    }
    return result;
  }

  public clear(): void {
    this.languageId = null;
    this.languageName = null;
    this.canRead = null;
    this.canSpeak = null;
    this.canWrite = null;
  }

  public isEmpty(): boolean {
    // All properties have to be null or blank to make the entire object empty.
    return this.languageId == null && this.canRead == null && this.canSpeak == null && this.canWrite == null;
  }

  isRequired(): boolean {
    if (this.languageId != null || this.canRead != null || this.canSpeak != null || this.canWrite != null) {
      return true;
    } else {
      return false;
    }
  }
}
