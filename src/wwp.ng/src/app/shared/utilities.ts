// tslint:disable: deprecation
import { DropDownMultiField } from './models/dropdown-multi-field';
import { HttpHeaders } from '@angular/common/http';
import * as moment from 'moment';
import * as _ from 'lodash';
import { ActionNeeded } from '../features-modules/actions-needed/models/action-needed';
import { ActionNeededInfo } from '../features-modules/actions-needed/models/action-needed-info';
import { DropDownField } from './models/dropdown-field';
import { IsEmpty } from './interfaces/is-empty';
import { MmDdYyyyValidationContext } from './interfaces/mmDdYyyy-validation-context';
import { MmYyyyValidationContext } from './interfaces/mmYyyy-validation-context';
import { ModelErrors } from './interfaces/model-errors';
import { ValidationCode } from './models/validation-error';
import { ValidationManager } from './models/validation-manager';
import { ValidationResult } from './models/validation-result';
import { YesNoStatus } from './models/primitives';
import { IRequestOptions } from '../core/interceptors/AuthHttpClient';
import { SystemClockService } from './services/system-clock.service';
import { HrsValidationContext } from './interfaces/hrs-validation-context';

declare var $: any;

export class Utilities {
  /**
   * Returns unique from a callback.
   * ex. .filter(onlyUnique)
   * @static
   * @param {*} value
   * @param {*} index
   * @param {*} self
   * @returns
   * @memberof Utilities
   */

  public static onlyUnique(value, index, self) {
    return self.indexOf(value) === index;
  }

  public static get currentDate() {
    return SystemClockService.appDateTime;
  }

  /**
   *
   * Takes array of items and creates new array that's unique because dups are numbered.
   *
   * ex. ['a','a','b'] => ['a','a 2', 'b']
   *
   * @returns
   *
   * @memberof Utilities
   */
  public static createUniqueDropDownList(originalDropDown: DropDownField[]): DropDownField[] {
    // if (originalDropDown == null) {
    //   return;
    // }

    const originalList = [];
    for (const odd of originalDropDown) {
      originalList.push(odd.name);
    }

    const newDropDownList: DropDownField[] = [];
    const counts = {};

    // first pass, assign the counts
    for (let i = 0; i < originalList.length; i++) {
      let name = originalList[i];
      let count = counts[name];

      if (!count) {
        count = 1;
      } else {
        count++;
      }

      counts[name] = count;

      name = name + ' - ' + count;
      newDropDownList.push(name);
    }

    return newDropDownList;
  }

  public static createDropDownWithIdAsName(drops: DropDownField[]) {
    const newDd = [];
    for (const d of drops) {
      const dd = new DropDownField();
      dd.id = d.name;
      dd.name = d.name;
      newDd.push(dd);
    }
    return newDd;
  }

  /**
   *  Title case a string. ex. 'hello' -> 'Hello' and 'hello hello' -> 'Hello Hello'.
   *
   * @static
   * @param {any} str
   * @returns
   *
   * @memberOf Utilities
   */
  public static toTitleCase(str) {
    return str.replace(/\w\S*/g, function(txt) {
      return txt.charAt(0).toUpperCase() + txt.substr(1).toLowerCase();
    });
  }

  public static trimStart(character: string, string: string) {
    let startIndex = 0;

    while (string[startIndex] === character) {
      startIndex++;
    }

    return string.substr(startIndex);
  }

  /**
   * Gets a RequestOptions object that is setup with a proper authorization headers for
   * our API.
   *
   * @static
   * @returns {RequestOptions}
   *
   * @memberOf Utilities
   */
  public static getApiAuthorizedRequestOptions(): IRequestOptions {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const options = {
      headers: headers,
      withCredentials: false
    } as IRequestOptions;

    return options;
  }

  static isMmYyyyFormatCorrect(input: string): boolean {
    return /^\d{2}\/\d{4}$/.test(input);
  }

  /**
   * Formats name with first name, middle initial, last name and suffix.
   *
   * @static
   * @param {string} firstName
   * @param {string} middleInitial
   * @param {string} lastName
   * @param {string} suffix
   * @returns {string}
   * @memberof Utilities
   */
  static formatDisplayPersonName(firstName: string, middleInitial: string, lastName: string, suffix: string, toUpperCase = false): string {
    // Lowercase everything just to make sure we start with a uniformed case.
    const firstNameLower = _.toLower(firstName).trim();
    const middleInitialLower = _.toLower(middleInitial).trim();
    const lastNameLower = _.toLower(lastName).trim();

    // No suffix because case of both letters being uppercase.
    let firstNameFormatted = '';
    let middleInitialFormatted = '';
    let lastNameFormatted = '';

    // We take suffix as is but trimmed.
    let suffixFormatted = _.trim(suffix);

    if (firstNameLower.length > 0) {
      firstNameFormatted = firstNameLower.charAt(0).toUpperCase() + firstNameLower.slice(1);
    }

    if (middleInitialLower.length > 0) {
      middleInitialFormatted = middleInitialLower.charAt(0).toUpperCase() + middleInitialLower.slice(1);
    }

    if (lastNameLower.length > 0) {
      lastNameFormatted = lastNameLower.charAt(0).toUpperCase() + lastNameLower.slice(1);
    }

    if (suffix == null) {
      suffixFormatted = '';
    }

    const formattedName =
      middleInitialFormatted.length === 0
        ? `${firstNameFormatted} ${lastNameFormatted} ${suffixFormatted}`
        : `${firstNameFormatted} ${middleInitialFormatted}. ${lastNameFormatted} ${suffixFormatted}`;

    return toUpperCase ? formattedName.toUpperCase() : formattedName;
  }

  static formatPin(pin: any) {
    if (pin == null) {
      return '';
    }

    let str = pin.toString();
    if (str.length >= 10) {
      return str;
    }

    str = '000000000' + str;
    return str.substring(str.length - 10);
  }

  static parseMmYyyyIntoMmDdYyyyString(mmYyyy: string): string {
    const inputValue = mmYyyy.split('/');
    if (inputValue.length !== 2) {
      return '';
    }

    const valueMM = inputValue[0];
    const valueYYYY = inputValue[1];

    if (valueMM === '' || valueYYYY === '') {
      return '';
    }

    const valueDateString = valueMM + '-01-' + valueYYYY;

    return valueDateString;
  }

  /**
   * Returns true if date is today.
   *
   * @static
   * @param {ISO date} date
   * @returns {boolean}
   * @memberof Utilities
   */
  static isDateCurrent(date: string): boolean {
    const dateMoment = moment(date).format('MM/DD/YYYY');
    const todayMoment = Utilities.currentDate.format('MM/DD/YYYY');

    if (dateMoment === todayMoment) {
      return true;
    } else {
      return false;
    }
  }

  static idByFieldDataName(fieldName: string, fieldNames: DropDownField[], suppressWarning?: boolean, popName = 'name'): number {
    let id = 0;

    if (fieldNames == null) {
      return null;
    }

    for (const x of fieldNames) {
      if (x[popName].toLowerCase() === fieldName.toLowerCase()) {
        id = x.id;
      }
    }

    if (id === 0 && suppressWarning !== true) {
      console.warn('Method "idByFieldDataName" Could Not Find Field Id for: ' + fieldName + ' in ' + JSON.stringify(fieldNames));
    }

    return id;
  }

  static fieldDataNameById(id: number, fieldNames: DropDownField[], suppressWarning?: boolean, popName = 'name'): string {
    let fieldName = '';

    if (fieldNames == null) {
      return null;
    }

    for (const x of fieldNames) {
      if (x.id === id) {
        fieldName = x.name;
        break;
      }
    }

    if (id === 0 && suppressWarning !== true) {
      console.warn('Method "fieldDataNameById" Could Not Find Field name for: ' + id + ' in ' + JSON.stringify(fieldNames));
    }

    return fieldName;
  }

  static fieldDataNamesByIds(ids: number[], dropDownValues: DropDownField[], suppressWarning?: boolean, popName = 'name'): string[] {
    const fieldNames: string[] = [];

    if (dropDownValues == null) {
      return null;
    }

    dropDownValues.forEach(value => {
      if (ids.indexOf(value.id) >= 0) fieldNames.push(value.name);
    });

    return fieldNames;
  }

  static fieldDataIdByCode(code: string, fieldNames: DropDownField[], suppressWarning?: boolean, popName = 'name'): string {
    let fieldId = '';

    if (fieldNames == null) {
      return null;
    }

    for (const x of fieldNames) {
      if (x.code === code) {
        fieldId = x.id;
      }
    }

    if (!code && suppressWarning !== true) {
      console.warn('Method "fieldDataIdByCode" Could Not Find Field name for: ' + code + ' in ' + JSON.stringify(fieldNames));
    }

    return fieldId;
  }
  static fieldDataCodeById(id: number, fieldNames: DropDownField[], suppressWarning?: boolean, popName = 'name'): string {
    let fieldCode = '';

    if (fieldNames == null) {
      return null;
    }

    for (const x of fieldNames) {
      if (x.id === id) {
        fieldCode = x.code;
      }
    }

    if (id === 0 && suppressWarning !== true) {
      console.warn('Method "fieldDataCodeById" Could Not Find Field name for: ' + id + ' in ' + JSON.stringify(fieldNames));
    }

    return fieldCode;
  }

  static cleanseModelForApi(model: any) {
    if (model != null) {
      Object.keys(model).forEach(k => {
        if (typeof model[k] === 'string' && model[k].trim() === '') model[k] = null;
        else model[k] = model[k];
      });
    }

    return model;
  }

  static checkModelForNull(model: any) {
    let isModelNotNull = false;
    if (model != null) {
      isModelNotNull = Object.keys(model).every(k => model[k] != null);
    }

    return isModelNotNull;
  }

  static pad(num: number, size: number): string {
    let s = num + '';
    while (s.length < size) s = '0' + s;
    return s;
  }

  static toNumberOrNull(value: number) {
    if (value == null) {
      return null;
    } else {
      return +value;
    }
  }

  static isStringEmptyOrNull(str: string): boolean {
    return str == null || str.trim() === '';
  }

  static isNumberEmptyOrNull(num: number): boolean {
    return num == null || num.toString().trim() === '';
  }

  static currencyToNumber(input: any) {
    return +input.toString().replace(/[^0-9.-]+/g, '');
  }

  static toMmDdYyyy(date: string) {
    if (date != null && moment(date, moment.ISO_8601).isValid()) {
      return moment(date, moment.ISO_8601).format('MM/DD/YYYY');
    } else {
      return date;
    }
  }

  static mmDdYyyyToDateTime(dateMmDdYyyy: string) {
    if (dateMmDdYyyy != null && dateMmDdYyyy.length === 10 && moment(dateMmDdYyyy, 'MM/DD/YYYY').isValid()) {
      // ISO string is 8601.
      return moment(dateMmDdYyyy, 'MM/DD/YYYY').toISOString();
    } else {
      return dateMmDdYyyy;
    }
  }

  static toMmYyyy(date: string) {
    if (date != null && moment(date, moment.ISO_8601).isValid()) {
      return moment(date, moment.ISO_8601).format('MM/YYYY');
    } else {
      return date;
    }
  }

  static mmYyyyToDateTime(dateMmDdYyyy: string) {
    if (dateMmDdYyyy != null && dateMmDdYyyy.length === 7 && moment(dateMmDdYyyy).isValid()) {
      // ISO string is 8601.
      return moment(dateMmDdYyyy, 'MM/YYYY').toISOString();
    } else {
      return dateMmDdYyyy;
    }
  }

  static lowerCaseTrimAsNotNull(str: string): string {
    if (!str || str.trim() === '') {
      return '';
    } else {
      return str.toLocaleLowerCase().trim();
    }
  }

  static lowerCaseArrayTrimAsNotNull(objArray, prop) {
    if (!objArray) {
      return [];
    } else {
      objArray.forEach(i => {
        i[prop] = i[prop].toLocaleLowerCase().trim();
      });
      return objArray;
    }
  }

  static isArrayAllEmpty(items: IsEmpty[]): boolean {
    let isAllEmpty = true;

    if (items != null && items.length > 0) {
      for (const item of items) {
        if (!item.isEmpty()) {
          isAllEmpty = false;
          break;
        }
      }
    }

    return isAllEmpty;
  }

  static isRepeaterRowRequired(items: IsEmpty[], r: number): boolean {
    // If all rows are empty, then only the first one needs to indicate that
    // it is required.
    if (Utilities.isArrayAllEmpty(items)) {
      return r === 0;
    } else {
      // Since there is one non-empty row, we only need to indicate required
      // if this row is non-empty.
      return !items[r].isEmpty();
    }
  }

  public static getPropertybyIndexAndName(errors: any[], i: number, property: string): any {
    if (errors == null) {
      return null;
    }

    if (errors[i] == null) {
      return null;
    }

    return errors[i][property];
  }

  public static getPropertybyIdAndName(list: any[], id: number, property: string): any {
    if (list == null) {
      return null;
    }

    // if (list[id] == null) {
    //   return null;
    // }

    if (id > 0) {
      const result = list.filter(function(obj) {
        return obj.id === id;
      });
      if (result != null && result.length > 0) {
        return result[0][property];
      } else {
        return null;
      }
    }
    // return list[id][property];
  }

  static isChildRepeaterRowRequired(childItems: IsEmpty[], r: number, parentRepeaterModel: any, pr: number): boolean {
    // If all rows are empty, then only the first one needs to indicate that
    // it is required.
    if (((Utilities.isArrayAllEmpty(childItems) && pr === 0) || parentRepeaterModel.isEmpty() === false) && childItems.length === 1) {
      return r === 0;
    } else if (!parentRepeaterModel.isEmpty() && !childItems[r].isEmpty()) {
      return true;
    } else {
      // Since there is one non-empty row, we only need to indicate required
      // if this row is non-empty.
      return !childItems[r].isEmpty();
    }
  }

  // static calculateDateDuration(startDate: string, endDate: string): string {
  //     if (startDate != null && endDate != null) {

  //         let a = moment(startDate, 'MM/DD/YYYY');
  //         let b = moment(endDate, 'MM/DD/YYYY');

  //         if (a.isValid && b.isValid) {
  //             let years = a.diff(b, 'year');
  //             b.add(years, 'years');

  //             let months = a.diff(b, 'months');
  //             b.add(months, 'months');

  //             let jobDateDuration: string;
  //             if (years === 0) {
  //                 jobDateDuration = months + ' months';
  //             } else {
  //                 jobDateDuration = years + ' years, ' + months + ' months';
  //             }
  //             return jobDateDuration;
  //         }
  //     }
  // }

  public static isModelErrorsItemInvalid(errors: ModelErrors[], i: number, property: string): boolean {
    if (errors == null) {
      return false;
    }

    if (errors[i] == null) {
      return false;
    }

    return errors[i][property] != null;
  }

  public static isChildModelErrorsItemInvalid(errors: any, childRepeaterName: string, i: number, property: string): boolean {
    if (errors == null) {
      return false;
    }

    if (errors) {
      if (errors[childRepeaterName] && errors[childRepeaterName][i] && errors[childRepeaterName][i][property]) {
        return errors[childRepeaterName][i][property];
      } else {
        return false;
      }
    }
  }

  public static isSecondChildModelErrorsItemInvalid(errors: any, secondChildRepeaterName: string, i: number, property: string): boolean {
    if (errors == null) {
      return false;
    }

    if (errors) {
      if (errors[secondChildRepeaterName][i][property]) {
        return errors[secondChildRepeaterName][i][property];
      } else {
        return false;
      }
    }
  }
  public static isModelErrorsChildItemInvalid(errors: any, childRepeaterName: string, i: number, j: number, property: string): boolean {
    if (errors == null) {
      return false;
    }

    if (errors[i] == null) {
      return false;
    }
    if (errors[i]) {
      if (errors[i][childRepeaterName][j][property]) {
        return errors[i][childRepeaterName][j][property];
      } else {
        return false;
      }
    }
  }

  /**
   * Gets the Age in whole numbers for a given date of birth and date format, similar
   * to the way we communicate our age.  If you are 19 years, 4 months and 2 days it
   * will simply return 19.
   *
   * @static
   * @param {string} dateOfBirth
   * @returns {number}
   *
   * @memberOf Utilities
   */
  public static getAgeFromDateOfBirth(dateOfBirth: string): number {
    // If the DOB is not valid, we don't know the age.
    if (dateOfBirth == null || dateOfBirth.trim() === '') {
      return null;
    }

    // See if there is a valid date and be sure to use STRICT parsing
    // (using true as last parameter):
    const dob = moment(dateOfBirth);

    if (!dob.isValid()) {
      return null;
    }

    // We must check for future DOBs.  If it is in the future, we return null
    // to indicate no age.  This is because the date diff below will return 0
    // for dates in the future up to 1 year from now, similar to the way it will
    // return 0 if less than 1 year old.  So we don't want that to be confusing
    // which is why we return null for future DOBs.
    const today = Utilities.currentDate;
    if (dob > today) {
      return null;
    }

    return today.diff(dob, 'years', false);
  }

  /**
   * Gets the duration as a years and months string.  If you enter 11/06/2006 and 10/6/2010
   * return will be 3 years, 11 months.
   *
   *
   * @static
   * @param {string} startDate
   * @param {string} endDate
   * @returns {string}
   *
   * @memberOf Utilities
   */
  public static getDurationBetweenDates(startDate: string, endDate: string): string {
    const en = moment(endDate, 'MM/DD/YYYY');
    const st = moment(startDate, 'MM/DD/YYYY');

    const years = en.diff(st, 'year');
    st.add(years, 'years');

    const months = en.diff(st, 'months');
    st.add(months, 'months');

    if (years === 0) {
      if (months === 0) {
        return '<1 months';
      } else {
        return months + ' months';
      }
    } else {
      return years + ' years, ' + months + ' months';
    }
  }

  /**
   * Cleans up a date string if it's not valid.
   *
   * @static
   * @param {string} date
   * @param {string} format
   *
   * @memberOf Utilities
   */
  public static cleanseDateWithFormat(date: string, format: string): void {
    if (date != null) {
      const momDate = moment(date, format, true);
      if (!momDate.isValid()) {
        date = null;
      }
    } else {
      date = null;
    }
  }

  /**
   * Gets the Age for a given date of birth in whole numbers, similar to the way we
   * communicate our age.  If you are 19 years, 4 months and 2 days it will simply
   * return 19.
   *
   * @static
   * @param {string} dateOfBirth
   * @param {string} format
   * @returns {number}
   *
   * @memberOf Utilities
   */
  public static getAgeFromDateOfBirthWithFormat(dateOfBirth: string, format: string): number {
    // If the DOB is not valid, we don't know the age.
    if (dateOfBirth == null) {
      return null;
    }

    // See if there is a valid date and be sure to use STRICT parsing
    // (using true as last parameter):
    const dob = moment(dateOfBirth, format, true);

    if (!dob.isValid()) {
      return null;
    }

    // We must check for future DOBs.  If it is in the future, we return null
    // to indicate no age.  This is because the date diff below will return 0
    // for dates in the future up to 1 year from now, similar to the way it will
    // return 0 if less than 1 year old.  So we don't want that to be confusing
    // which is why we return null for future DOBs.
    const today = Utilities.currentDate;
    if (dob > today) {
      return null;
    }

    return today.diff(dob, 'years', false);
  }

  /**
   * Runs the validation logic for the Action Needed Info object using the
   * list of possible ActionNeeded objects and sets the results into the
   * supplied ValidationResult and ValidationManager objects.
   *
   * @static
   * @param {ActionNeededInfo} actionNeeded
   * @param {ActionNeeded[]} possibleActionNeededs
   * @param {ValidationResult} result
   * @param {ValidationManager} validationManager
   * @param {string} diffQuestion
   *
   * @memberOf Utilities
   */
  public static validateActionNeeded(
    actionNeeded: ActionNeededInfo,
    possibleActionNeededs: ActionNeeded[],
    result: ValidationResult,
    validationManager: ValidationManager,
    prettyQuestion?: string
  ): void {
    if (possibleActionNeededs == null) {
      return;
    }

    let question = 'Action Needed';
    if (prettyQuestion != null) {
      question = prettyQuestion;
    }

    if (actionNeeded == null || actionNeeded.actionNeededTypes == null || actionNeeded.actionNeededTypes.length === 0) {
      // The action needed information is required.
      result.addError('actionNeeded');
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, question);
    } else {
      // Check if details field is empty for those action neededs that require it.
      for (const ant of actionNeeded.actionNeededTypes) {
        for (const an of possibleActionNeededs) {
          if (ant === an.id && an.requiresDetails === true && (actionNeeded.assistDetails == null || actionNeeded.assistDetails.trim() === '')) {
            result.addError('actionNeededDetails');
            validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, question + ' - Details');
          }
        }
      }
    }
  }

  /**
   * Runs the validation logic for a Yes No questions (boolean) that conditionally will
   * require a Details text box.
   *
   * @static
   * @param {ValidationResult} result
   * @param {ValidationManager} validationManager
   * @param {boolean} yesNo
   * @param {string} yesNoErrorKey
   * @param {string} question
   * @param {string} details
   * @param {string} detailsErrorKey
   *
   * @memberOf Utilities
   */
  public static validateRequiredYesNoAndDetailsIfYes(
    result: ValidationResult,
    validationManager: ValidationManager,
    yesNo: boolean,
    yesNoErrorKey: string,
    question: string,
    details: string,
    detailsErrorKey: string,
    detailsQuestion?: string
  ): void {
    if (yesNo == null) {
      result.addError(yesNoErrorKey);
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, question);
    } else if (yesNo === true) {
      if (details == null || details.trim() === '') {
        result.addError(detailsErrorKey);
        if (detailsQuestion == null) {
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, question + ' - Details');
        } else {
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, detailsQuestion);
        }
      }
    }
  }

  /**
   * Runs the validation logic for a required Yes No questions (boolean).
   *
   * @static
   * @param {ValidationResult} result
   * @param {ValidationManager} validationManager
   * @param {boolean} yesNo
   * @param {string} yesNoErrorKey
   * @param {string} question
   *
   * @memberOf Utilities
   */
  public static validateRequiredYesNo(result: ValidationResult, validationManager: ValidationManager, yesNo: boolean, yesNoErrorKey: string, question: string): void {
    if (yesNo == null) {
      result.addError(yesNoErrorKey);
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, question);
    }
  }

  /**
   * Runs the validation logic for a required Yes No Refused questions.
   *
   * @static
   * @param {ValidationResult} result
   * @param {ValidationManager} validationManager
   * @param {YesNoStatus} yesNoRefused
   * @param {string} yesNoRefusedKey
   * @param {string} question
   *
   * @memberOf Utilities
   */
  public static validateYesNoRefused(
    yesNoRefused: YesNoStatus,
    yesId: number,
    result: ValidationResult,
    validationManager: ValidationManager,
    yesNoRefusedKey: string,
    question: string
  ) {
    if (yesNoRefused != null && yesNoRefused.status == null) {
      result.addError(yesNoRefusedKey);
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, question);
    } else if (yesNoRefused != null && yesNoRefused.status === yesId && (yesNoRefused.details == null || yesNoRefused.details.trim() === '')) {
      result.addError(yesNoRefusedKey + 'Details');
      // question = question.replace('?', '');
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, question + ' - Details');
    }
  }

  /**
   * Runs the validation logic for a required currency input (string).
   *
   * @static
   * @param {ValidationResult} result
   * @param {ValidationManager} validationManager
   * @param {string} currency
   * @param {string} currencyErrorKey
   * @param {string} question
   *
   * @memberOf Utilities
   */
  public static validateRequiredCurrency(result: ValidationResult, validationManager: ValidationManager, currency: string, currencyErrorKey: string, question: string): void {
    if (currency == null || currency.trim() === '' || currency.trim() === '.' || currency.trim() === '. 0') {
      result.addError(currencyErrorKey);
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, question);
    }
  }

  // WIP.
  public static validateDuplicateDataInRepeater(
    result: ValidationResult,
    validationManager: ValidationManager,
    jsonString: string,
    numberErrorKey: string,
    question: string
  ): boolean {
    const clonedStrings = [];

    if (jsonString == null && clonedStrings.indexOf(jsonString) > -1) {
      result.addError(numberErrorKey);
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, question);
      return false;
    }

    clonedStrings.push(jsonString);
    return true;
  }

  /**
   * Runs the validation logic for a required number input ().
   *
   * @static
   * @param {ValidationResult} result
   * @param {ValidationManager} validationManager
   * @param {string} currency
   * @param {string} currencyErrorKey
   * @param {string} question
   *
   * @memberOf Utilities
   */
  public static validateRequiredNumber(result: ValidationResult, validationManager: ValidationManager, number: number, numberErrorKey: string, question: string): boolean {
    if (number == null || number.toString().trim() === '') {
      result.addError(numberErrorKey);
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, question);
      return false;
    }
    return true;
  }

  // TODO: Make more generic.
  static validateRepeater(repeaterName: string, repeatedItems: any[], validationParms: any[], result: ValidationResult, validationManager: ValidationManager) {
    const errArr = result.createErrorsArray(repeaterName);
    let allFormalAssessmentsAreEmpty = true;

    // Check to see if all are empty.
    for (const fa of repeatedItems) {
      if (!fa.isEmpty()) {
        allFormalAssessmentsAreEmpty = false;
        break;
      }
    }

    if (allFormalAssessmentsAreEmpty) {
      // If all are empty validate the first one.
      if (repeatedItems[0] != null) {
        const v = repeatedItems[0].validate(validationParms[0], validationParms[1], validationParms[2], validationParms[3]);
        errArr.push(v.errors);
        if (v.isValid === false) {
          result.isValid = false;
        }
      }
    } else {
      for (const fa of repeatedItems) {
        if (!fa.isEmpty()) {
          const v = fa.validate(validationManager, validationParms[1], validationParms[2], validationParms[3]);
          errArr.push(v.errors);
          if (v.isValid === false) {
            result.isValid = false;
          }
        } else {
          // Push empty when item is blank in order to keep correct index.
          const resultEmpty = new ValidationResult();
          errArr.push(resultEmpty.errors);
        }
      }
    }
  }

  public static validatePropForNullAndEmptyValues(result: ValidationResult, validationManager: ValidationManager, prop: any, prettyProp: string, message?: string) {
    if (isUndefined(prop) || prop == null || prop.toString() === '') {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, message);
      if (message) result.addError(prettyProp);
    }
  }

  /**
   *  Returns the data type for value as a string.
   *
   * @static
   * @param {any} value
   * @returns
   * @memberof Utilities
   */
  public static detectType(value) {
    switch (typeof value) {
      case 'string':
        return 'string';
      case 'number':
        return 'number';
      case 'boolean':
        return 'boolean';
      case 'object':
        return 'object';
      default:
        return 'other';
    }
  }

  public static randomNumber() {
    return Math.floor((1 + Math.random()) * 0x10000).toString(16);
  }

  /**
   * Deserilizes child object otherwise initializes a new object.
   * childrenProp = From JSON
   * obj = Object Type
   * Returns deserilized Object
   * @static
   * @param {any} childProp
   * @param {any} obj
   *
   * @memberOf Utilities
   */
  public static deserilizeChild(childProp: any, obj: any) {
    let deserialized: any;
    if (childProp != null) {
      deserialized = new obj().deserialize(childProp);
    } else {
      // Check to see if safe to use the create function which is the favored way of initialization.
      if (typeof obj.create === 'function') {
        deserialized = obj.create();
      } else {
        deserialized = new obj();
      }
    }
    return deserialized;
  }

  /**
   * Deserilizes children objects otherwise initializes an empty array.
   * childrenProp = From JSON
   * obj = Object Type
   * Returns deserilized Array of Objects
   * @static
   * @param {any} childrenProp
   * @param {any} obj
   *
   * @memberOf Utilities
   */
  public static deserilizeChildren(childrenProp: any, obj: any, insertBlankAmount = 1) {
    // Check if API sent a null array.
    if (childrenProp == null) {
      childrenProp = [];
    }
    const deserialized: any[] = [];

    // For when API array is empty, lets initialize a blank objects and insert them.
    if (childrenProp.length === 0) {
      if (typeof obj.create === 'function') {
        // Safe to use the create function which is the favored way of initialization.
        for (let i = 0; i < insertBlankAmount; i++) {
          deserialized.push(obj.create());
        }
      } else {
        for (let i = 0; i < insertBlankAmount; i++) {
          deserialized.push(new obj());
        }
      }
      // Use what the API sent because API payload was not empty.
    } else {
      for (const c of childrenProp) {
        if (!c.hasOwnProperty('NULL')) {
          deserialized.push(new obj().deserialize(c));
        }
      }
    }
    return deserialized;
  }

  /**
   * Deserilizes array of otherwise initializes an empty array.
   *
   * @static
   * @param {any[]} numbers
   *
   * @memberOf Utilities
   */
  public static deserilizeArray(numbers: any[]) {
    if (numbers == null) {
      numbers = [];
      return numbers;
    }
    const deserialized: any[] = [];
    for (const c of numbers) {
      if (c != null) {
        deserialized.push(c);
      }
    }
    return deserialized;
  }

  /**
   * Runs the basic validations for a given date
   *
   * @static
   * @param {ValidationManager} validationManager
   * @param {ValidationResult} result
   * @param {string} date
   * @param {string} prettyErrorMsg
   * @param {string} errorKey
   *
   * @memberOf Utilities
   */
  public static dateFormatValidation(validationManager: ValidationManager, result: ValidationResult, date: string, prettyErrorMsg: string, errorKey: string) {
    const formattedInputDate = moment(date, 'MM/DD/YYYY', true);
    if (date == null || date.trim() === '') {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, prettyErrorMsg);
      result.addError(errorKey);
    } else if ((date !== null && date.length !== 10) || !(date.indexOf('/') === 2 && date.lastIndexOf('/') === 5)) {
      validationManager.addErrorWithFormat(ValidationCode.ValueInInvalidFormat_Name_FormatType, prettyErrorMsg, 'MM/DD/YYYY');
      result.addError(errorKey);
    } else if (!formattedInputDate.isValid()) {
      validationManager.addErrorWithDetail(ValidationCode.InvalidDate, 'Invalid ' + prettyErrorMsg + ' Entered');
      result.addError(errorKey);
    }
  }

  /**
   * Runs the validation logic for a year for being required, Current Format, less than DOB Year and less than current year.
   *
   * @static
   * @param {ValidationResult} result
   * @param {ValidationManager} validationManager
   * @param {number} dobYear
   * @param {number} year
   * @param {string} prettyErrorMsg
   * @param {string} errorKey
   *
   * @memberOf Utilities
   */

  // TODO: Make this more generic so it can validate any YYYY input.
  public static valididateYyyyYear(
    year: number,
    dobYear: number,
    prettyErrorMsg: string,
    errorKey: string,
    result: ValidationResult,
    validationManager: ValidationManager,
    validationCodeDob: number,
    validationCodeFuture: number,
    isRequired: boolean
  ) {
    // Year required.
    if (year == null || (year.toString().trim() === '' && isRequired === true)) {
      result.addError(errorKey);
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, prettyErrorMsg);
    } else {
      // Year with DOB and format check.
      if (year != null && year < 999) {
        result.addError(errorKey);
        validationManager.addErrorWithFormat(ValidationCode.ValueInInvalidFormat_Name_FormatType, prettyErrorMsg, 'YYYY');
      } else {
        if (year != null && year.toString() !== '' && dobYear != null && year <= dobYear) {
          result.addError(errorKey);
          validationManager.addErrorWithFormat(validationCodeDob, dobYear.toString());
          // validationManager.addErrorWithFormat(ValidationCode.EducationHistoryLastYearAttendedInvalid_ParticipantDob, dobYear.toString());
        }
        // Year against Current Date.
        const currentDate = new Date(Utilities.currentDate.format('MM/DD/YYYY'));
        const currentDateYYYY = currentDate.getFullYear();
        if (year != null && year.toString() !== '' && year > currentDateYYYY) {
          result.addError(errorKey);
          validationManager.addError(validationCodeFuture);
          // validationManager.addError(ValidationCode.EducationHistoryLastYearAttendedInFuture);
        }
      }
    }
  }

  /**
   * Creates a pretty string of the number of items on a page.
   * Used for our paging view.
   * @static
   * @param {config} result
   * @param {p} pagination
   * @param {number} max
   *
   * @memberOf Utilities
   */
  public static numberOfItemsOnCurrentPage(config, p, max: number) {
    const firstIndex = config.itemsPerPage * p.getCurrent() - (config.itemsPerPage - 1);
    let lastIndex = config.itemsPerPage * p.getCurrent();

    // Dont let the lastIndex be more than the number of items in the list being paged.
    if (lastIndex > max) {
      lastIndex = max;
    }
    const indices = firstIndex + '-' + lastIndex;
    return indices;
  }

  /**
   *  Validates a MM/YYYY date.
   *  If participantDOB is supplied date can not exceed 120 years from participant dob.
   *  Min and Max dates can be provided as MM/DD/YYYY or MM/YYYY we will default both cases to first of the month.
   *
   * @memberOf Utilities
   */
  public static validateMmYyyyDate(mmYyyyValidation: MmYyyyValidationContext, years: number = 120, showDate = false): boolean {
    let isDateValid = true;

    // Correct Dates so they are first of the month.
    if (mmYyyyValidation.minDate != null && mmYyyyValidation.minDate.length === 10) {
      const fixedDate = moment(mmYyyyValidation.minDate, 'MM/DD/YYYY').date(1);
      mmYyyyValidation.minDate = fixedDate.format('MM/DD/YYYY');
    }

    if (mmYyyyValidation.maxDate != null && mmYyyyValidation.maxDate.length === 10) {
      const fixedDate = moment(mmYyyyValidation.maxDate, 'MM/DD/YYYY').date(1);
      mmYyyyValidation.maxDate = fixedDate.format('MM/DD/YYYY');
    }

    if (mmYyyyValidation.minDate != null && mmYyyyValidation.minDate.length === 7) {
      const fixedDate = moment(mmYyyyValidation.minDate, 'MM/YYYY').date(1);
      mmYyyyValidation.minDate = fixedDate.format('MM/DD/YYYY');
    }

    if (mmYyyyValidation.maxDate != null && mmYyyyValidation.maxDate.length === 7) {
      const fixedDate = moment(mmYyyyValidation.maxDate, 'MM/YYYY').date(1);
      mmYyyyValidation.maxDate = fixedDate.format('MM/DD/YYYY');
    }

    // Set to the first of the month by using MM/YYYY.
    const inputDate = moment(mmYyyyValidation.date, 'MM/YYYY');
    if (mmYyyyValidation.isRequired === true) {
      if (mmYyyyValidation.date == null || mmYyyyValidation.date.trim() === '') {
        mmYyyyValidation.validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, mmYyyyValidation.prettyProp);
        mmYyyyValidation.result.addError(mmYyyyValidation.prop);
        isDateValid = false;
      }
    }

    if (mmYyyyValidation.date != null && mmYyyyValidation.date.trim() !== '' && mmYyyyValidation.date.length !== 7) {
      // Date must be 6 digits in two digit month, two digit day, and four digit year format. (MM/YYYY).
      mmYyyyValidation.validationManager.addErrorWithFormat(ValidationCode.ValueInInvalidFormat_Name_FormatType, mmYyyyValidation.prettyProp, 'MM/YYYY');
      mmYyyyValidation.result.addError(mmYyyyValidation.prop);
      isDateValid = false;
    } else if (inputDate.isValid()) {
      // Hook for required valid date.
      // Min date allow same month.
      const minDate = moment(mmYyyyValidation.minDate, 'MM/DD/YYYY');
      if (mmYyyyValidation.minDate != null && mmYyyyValidation.minDateAllowSame && inputDate < minDate) {
        const dd = showDate ? `${mmYyyyValidation.minDateName} ${minDate.format('MM/YYYY')}` : mmYyyyValidation.minDateName;
        mmYyyyValidation.validationManager.addErrorWithFormat(ValidationCode.DateBeforeDate_Name1_Name2, mmYyyyValidation.prettyProp, dd);
        mmYyyyValidation.result.addError(mmYyyyValidation.prop);
        isDateValid = false;
      }
      // Min date.
      if (mmYyyyValidation.minDate != null && !mmYyyyValidation.minDateAllowSame && inputDate <= minDate) {
        mmYyyyValidation.validationManager.addErrorWithFormat(ValidationCode.DateBeforeDate_Name1_Name2, mmYyyyValidation.prettyProp, mmYyyyValidation.minDateName);
        mmYyyyValidation.result.addError(mmYyyyValidation.prop);
        isDateValid = false;
      }
      // MaxDate.
      const MaxDate = moment(mmYyyyValidation.maxDate, 'MM/DD/YYYY');
      if (mmYyyyValidation.maxDate != null && !mmYyyyValidation.maxDateAllowSame && inputDate >= MaxDate) {
        mmYyyyValidation.validationManager.addErrorWithFormat(ValidationCode.DateAfterDate_Name1_Name2, mmYyyyValidation.prettyProp, mmYyyyValidation.maxDateName);
        mmYyyyValidation.result.addError(mmYyyyValidation.prop);
        isDateValid = false;
      }
      // Max Date allow same month.
      if (mmYyyyValidation.maxDate != null && mmYyyyValidation.maxDateAllowSame && inputDate > MaxDate) {
        mmYyyyValidation.validationManager.addErrorWithFormat(ValidationCode.DateAfterDate_Name1_Name2, mmYyyyValidation.prettyProp, mmYyyyValidation.maxDateName);
        mmYyyyValidation.result.addError(mmYyyyValidation.prop);
        isDateValid = false;
      }
      // If participantDOB is supplied date can not exceed 120 years from participant dob.
      if (mmYyyyValidation.participantDOB != null && inputDate > moment(mmYyyyValidation.participantDOB, 'MM/DD/YYYY').add(years, 'years')) {
        mmYyyyValidation.validationManager.addErrorWithFormat(
          ValidationCode.ValueBefore_X_PlusDOB_Name_Value_DOB,
          mmYyyyValidation.prettyProp,
          mmYyyyValidation.date,
          mmYyyyValidation.participantDOB.format('MM/DD/YYYY'),
          years.toString()
        );
        mmYyyyValidation.result.addError(mmYyyyValidation.prop);
        isDateValid = false;
      }
    } else {
      if (mmYyyyValidation.date != null && mmYyyyValidation.date.trim() !== '' && mmYyyyValidation.date.length === 7) {
        mmYyyyValidation.validationManager.addErrorWithDetail(ValidationCode.InvalidDate, 'Invalid ' + mmYyyyValidation.prettyProp + ' Entered');
        mmYyyyValidation.result.addError(mmYyyyValidation.prop);
        isDateValid = false;
      }
    }

    return isDateValid;
  }

  /**
   *  Validates a MM/DD/YYYY date.
   *  If participantDOB is supplied date can not exceed 120 years from participant dob.
   *
   * @memberOf Utilities
   */
  public static validateMmDdYyyyDate(mmDdYyyyValidation: MmDdYyyyValidationContext, years: number = 120): boolean {
    // Correct Dates so they are first of the month.
    if (mmDdYyyyValidation.minDate != null && mmDdYyyyValidation.minDate.length === 7) {
      const fixedDate = moment(mmDdYyyyValidation.minDate, 'MM/YYYY').date(1);
      mmDdYyyyValidation.minDate = fixedDate.format('MM/DD/YYYY');
    }

    if (mmDdYyyyValidation.maxDate != null && mmDdYyyyValidation.maxDate.length === 7) {
      const fixedDate = moment(mmDdYyyyValidation.maxDate, 'MM/YYYY').date(1);
      mmDdYyyyValidation.maxDate = fixedDate.format('MM/DD/YYYY');
    }

    // Date is valid unless it drops into if/else
    let isDateValid = true;
    const inputDate = moment(mmDdYyyyValidation.date, 'MM/DD/YYYY');
    if (mmDdYyyyValidation.isRequired === true) {
      if (mmDdYyyyValidation.date == null || mmDdYyyyValidation.date.trim() === '') {
        mmDdYyyyValidation.validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, mmDdYyyyValidation.prettyProp);
        mmDdYyyyValidation.result.addError(mmDdYyyyValidation.prop);
        isDateValid = false;
      }
    }
    if (mmDdYyyyValidation.date != null && mmDdYyyyValidation.date.length !== 0 && mmDdYyyyValidation.date.length !== 10) {
      //  End Date must be 8 digits in two digit month, two digit day, and four digit year format. (MM/DD/YYYY).
      mmDdYyyyValidation.validationManager.addErrorWithFormat(ValidationCode.ValueInInvalidFormat_Name_FormatType, mmDdYyyyValidation.prettyProp, 'MM/DD/YYYY');
      mmDdYyyyValidation.result.addError(mmDdYyyyValidation.prop);
      isDateValid = false;
    } else if (inputDate.isValid()) {
      // Hook for required valid date.
      // Check to see if date is no more than 120 years more than DOB.
      if (mmDdYyyyValidation.participantDOB != null && inputDate > moment(mmDdYyyyValidation.participantDOB, 'MM/DD/YYYY').add(years, 'year')) {
        mmDdYyyyValidation.validationManager.addErrorWithFormat(
          ValidationCode.ValueBefore_X_PlusDOB_Name_Value_DOB,
          mmDdYyyyValidation.prettyProp,
          inputDate.format('MM/DD/YYYY'),
          moment(mmDdYyyyValidation.participantDOB).format('MM/DD/YYYY'),
          years.toString()
        );
        mmDdYyyyValidation.result.addError(mmDdYyyyValidation.prop);
        isDateValid = false;
      }
      // Min date.
      if (mmDdYyyyValidation.minDate != null && mmDdYyyyValidation.minDateAllowSame && inputDate < moment(mmDdYyyyValidation.minDate, 'MM/DD/YYYY')) {
        mmDdYyyyValidation.validationManager.addErrorWithFormat(ValidationCode.DateBeforeDate_Name1_Name2, mmDdYyyyValidation.prettyProp, mmDdYyyyValidation.minDateName);
        mmDdYyyyValidation.result.addError(mmDdYyyyValidation.prop);
        isDateValid = false;
      }
      // Min or equal to date.
      if (mmDdYyyyValidation.minDate != null && !mmDdYyyyValidation.minDateAllowSame && inputDate < moment(mmDdYyyyValidation.minDate, 'MM/DD/YYYY')) {
        mmDdYyyyValidation.validationManager.addErrorWithFormat(ValidationCode.DateBeforeDate_Name1_Name2, mmDdYyyyValidation.prettyProp, mmDdYyyyValidation.minDateName);
        mmDdYyyyValidation.result.addError(mmDdYyyyValidation.prop);
        isDateValid = false;
      }
      if (mmDdYyyyValidation.minDate != null && !mmDdYyyyValidation.minDateAllowSame && inputDate.isSame(moment(mmDdYyyyValidation.minDate, 'MM/DD/YYYY'))) {
        mmDdYyyyValidation.validationManager.addErrorWithFormat(ValidationCode.ValueSameValue_Name1_Name2, mmDdYyyyValidation.prettyProp, mmDdYyyyValidation.minDateName);
        mmDdYyyyValidation.result.addError(mmDdYyyyValidation.prop);
        isDateValid = false;
      }
      // Min date match month.
      // if (mmDdYyyyValidation.minDate != null && mmDdYyyyValidation.minDateAllowSame && inputDate >= moment(mmDdYyyyValidation.minDate, 'MM/DD/YYYY')) {
      //   mmDdYyyyValidation.validationManager.addErrorWithFormat(ValidationCode.DateBeforeDate_Name1_Name2, mmDdYyyyValidation.prettyProp, mmDdYyyyValidation.minDateName);
      //   mmDdYyyyValidation.result.addError(mmDdYyyyValidation.prop);
      //   isDateValid = false;
      // }

      // Max Date.
      if (mmDdYyyyValidation.maxDate != null && mmDdYyyyValidation.maxDateAllowSame && inputDate > moment(mmDdYyyyValidation.maxDate, 'MM/DD/YYYY')) {
        mmDdYyyyValidation.validationManager.addErrorWithFormat(ValidationCode.DateAfterDate_Name1_Name2, mmDdYyyyValidation.prettyProp, mmDdYyyyValidation.maxDateName);
        mmDdYyyyValidation.result.addError(mmDdYyyyValidation.prop);
        isDateValid = false;
      }
      // MaxDate do not allow same.
      if (mmDdYyyyValidation.maxDate != null && !mmDdYyyyValidation.maxDateAllowSame && inputDate >= moment(mmDdYyyyValidation.maxDate, 'MM/DD/YYYY')) {
        mmDdYyyyValidation.validationManager.addErrorWithFormat(ValidationCode.DateAfterDate_Name1_Name2, mmDdYyyyValidation.prettyProp, mmDdYyyyValidation.maxDateName);
        mmDdYyyyValidation.result.addError(mmDdYyyyValidation.prop);
        isDateValid = false;
      }
    } else {
      if (mmDdYyyyValidation.date != null && mmDdYyyyValidation.date.trim() !== '' && mmDdYyyyValidation.date.length === 10) {
        mmDdYyyyValidation.validationManager.addErrorWithDetail(ValidationCode.InvalidDate, 'Invalid ' + mmDdYyyyValidation.prettyProp + ' Entered');
        mmDdYyyyValidation.result.addError(mmDdYyyyValidation.prop);
        isDateValid = false;
      }
    }

    return isDateValid;
  }

  /**
   *  Validates a MM/DD/YYYY date with a custom error msgs. Validation code is a copy of validateMmDdYyyyDate.
   *  If participantDOB is supplied date can not exceed 120 years from participant dob.
   *
   * @memberOf Utilities
   */
  public static validateMmDdYyyyDateCustomError(mmDdYyyyValidation: MmDdYyyyValidationContext, minErrorEnum: number, years: number = 120): boolean {
    // Correct Dates so they are first of the month.
    if (mmDdYyyyValidation.minDate != null && mmDdYyyyValidation.minDate.length === 7) {
      const fixedDate = moment(mmDdYyyyValidation.minDate, 'MM/YYYY').date(1);
      mmDdYyyyValidation.minDate = fixedDate.format('MM/DD/YYYY');
    }

    if (mmDdYyyyValidation.maxDate != null && mmDdYyyyValidation.maxDate.length === 7) {
      const fixedDate = moment(mmDdYyyyValidation.maxDate, 'MM/YYYY').date(1);
      mmDdYyyyValidation.maxDate = fixedDate.format('MM/DD/YYYY');
    }

    // Date is valid unless it drops into if/else
    let isDateValid = true;
    const inputDate = moment(mmDdYyyyValidation.date, 'MM/DD/YYYY');
    if (mmDdYyyyValidation.isRequired === true) {
      if (mmDdYyyyValidation.date == null || mmDdYyyyValidation.date.trim() === '') {
        mmDdYyyyValidation.validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, mmDdYyyyValidation.prettyProp);
        mmDdYyyyValidation.result.addError(mmDdYyyyValidation.prop);
        isDateValid = false;
      }
    }
    if (mmDdYyyyValidation.date != null && mmDdYyyyValidation.date.length !== 0 && mmDdYyyyValidation.date.length !== 10) {
      //  End Date must be 8 digits in two digit month, two digit day, and four digit year format. (MM/DD/YYYY).
      mmDdYyyyValidation.validationManager.addErrorWithFormat(ValidationCode.ValueInInvalidFormat_Name_FormatType, mmDdYyyyValidation.prettyProp, 'MM/DD/YYYY');
      mmDdYyyyValidation.result.addError(mmDdYyyyValidation.prop);
      isDateValid = false;
    } else if (inputDate.isValid()) {
      // Hook for required valid date.
      // Check to see if date is no more than 120 years more than DOB.
      if (mmDdYyyyValidation.participantDOB != null && inputDate > moment(mmDdYyyyValidation.participantDOB, 'MM/DD/YYYY').add(years, 'year')) {
        mmDdYyyyValidation.validationManager.addErrorWithFormat(
          ValidationCode.ValueBefore_X_PlusDOB_Name_Value_DOB,
          mmDdYyyyValidation.prettyProp,
          inputDate.format('MM/DD/YYYY'),
          moment(mmDdYyyyValidation.participantDOB).format('MM/DD/YYYY'),
          years.toString()
        );
        mmDdYyyyValidation.result.addError(mmDdYyyyValidation.prop);
        isDateValid = false;
      }
      // Min date.
      if (mmDdYyyyValidation.minDate != null && mmDdYyyyValidation.minDateAllowSame && inputDate < moment(mmDdYyyyValidation.minDate, 'MM/DD/YYYY')) {
        mmDdYyyyValidation.validationManager.addErrorWithFormat(minErrorEnum, mmDdYyyyValidation.minDate, mmDdYyyyValidation.minDateName);
        mmDdYyyyValidation.result.addError(mmDdYyyyValidation.prop);
        isDateValid = false;
      }
      // Min or equal to date.
      if (mmDdYyyyValidation.minDate != null && !mmDdYyyyValidation.minDateAllowSame && inputDate <= moment(mmDdYyyyValidation.minDate, 'MM/DD/YYYY')) {
        mmDdYyyyValidation.validationManager.addErrorWithFormat(minErrorEnum, mmDdYyyyValidation.minDate, mmDdYyyyValidation.minDateName);
        mmDdYyyyValidation.result.addError(mmDdYyyyValidation.prop);
        isDateValid = false;
      }
      // Min date match month.
      // if (mmDdYyyyValidation.minDate != null && mmDdYyyyValidation.minDateAllowSame && inputDate >= moment(mmDdYyyyValidation.minDate, 'MM/DD/YYYY')) {
      //   mmDdYyyyValidation.validationManager.addErrorWithFormat(ValidationCode.DateBeforeDate_Name1_Name2, mmDdYyyyValidation.prettyProp, mmDdYyyyValidation.minDateName);
      //   mmDdYyyyValidation.result.addError(mmDdYyyyValidation.prop);
      //   isDateValid = false;
      // }

      // Max Date.
      if (mmDdYyyyValidation.maxDate != null && mmDdYyyyValidation.maxDateAllowSame && inputDate > moment(mmDdYyyyValidation.maxDate, 'MM/DD/YYYY')) {
        mmDdYyyyValidation.validationManager.addErrorWithFormat(ValidationCode.DateAfterDate_Name1_Name2, mmDdYyyyValidation.prettyProp, mmDdYyyyValidation.maxDateName);
        mmDdYyyyValidation.result.addError(mmDdYyyyValidation.prop);
        isDateValid = false;
      }
      // MaxDate do not allow same.
      if (mmDdYyyyValidation.maxDate != null && !mmDdYyyyValidation.maxDateAllowSame && inputDate >= moment(mmDdYyyyValidation.maxDate, 'MM/DD/YYYY')) {
        mmDdYyyyValidation.validationManager.addErrorWithFormat(ValidationCode.DateAfterDate_Name1_Name2, mmDdYyyyValidation.prettyProp, mmDdYyyyValidation.maxDateName);
        mmDdYyyyValidation.result.addError(mmDdYyyyValidation.prop);
        isDateValid = false;
      }
    } else {
      if (mmDdYyyyValidation.date != null && mmDdYyyyValidation.date.trim() !== '' && mmDdYyyyValidation.date.length === 10) {
        mmDdYyyyValidation.validationManager.addErrorWithDetail(ValidationCode.InvalidDate, 'Invalid ' + mmDdYyyyValidation.prettyProp + ' Entered');
        mmDdYyyyValidation.result.addError(mmDdYyyyValidation.prop);
        isDateValid = false;
      }
    }

    return isDateValid;
  }

  /**
   *  Validate required boolean .
   *
   * @param {string} text
   * @param {string} prop
   * @param {string} prettyProp
   * @param {ValidationResult} result
   * @param {ValidationManager} validationManager
   *
   * @memberOf Utilities
   */
  public static validateRequiredBoolean(bool: boolean, prop: string, prettyProp: string, result: ValidationResult, validationManager: ValidationManager) {
    let isBoolEmpty = false;
    if (bool == null || bool.toString().trim() === '') {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, prettyProp);
      result.addError(prop);
      isBoolEmpty = true;
    }

    return isBoolEmpty;
  }

  /**
   *  Checks if text is empty/whitespace and adds to ValidationManager if it is.
   *
   * @param {string} text
   * @param {string} prop
   * @param {string} prettyProp
   * @param {ValidationResult} result
   * @param {ValidationManager} validationManager
   *
   * @memberOf Utilities
   */
  public static validateRequiredText(text: string, prop: string, prettyProp: string, result: ValidationResult, validationManager: ValidationManager) {
    let isEmpty = false;
    if (text == null || text.trim() === '') {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, prettyProp);
      result.addError(prop);
      isEmpty = true;
    }

    return isEmpty;
  }

  /**
   *  Checks if text is contains char at index and adds to ValidationManager if it does..
   *
   * @param {string} text
   * @param {string[]} invalidChars
   * @param {number} index
   * @param {string} prop
   * @param {string} prettyProp
   * @param {ValidationResult} result
   * @param {ValidationManager} validationManager
   *
   * @memberOf Utilities
   */
  public static validateInvalidCharAtIndexInText(
    text: string,
    invalidChars: string[],
    index: number,
    prop: string,
    prettyProp: string,
    result: ValidationResult,
    validationManager: ValidationManager
  ) {
    let isInvalid = false;

    if (text == null) {
      return isInvalid;
    }

    const charAtIndex = text[index];

    let prettyIndex = 'in position ' + index;

    switch (index) {
      case 0: {
        prettyIndex = 'first';
        break;
      }
      case 999: {
        prettyIndex = 'last';
        break;
      }
      default: {
        break;
      }
    }

    for (const c of invalidChars) {
      if (c === charAtIndex) {
        validationManager.addErrorWithFormat(ValidationCode.InvalidCharAtIndex, prettyProp, c, prettyIndex);
        result.addError(prop);
        isInvalid = true;
        break;
      }
    }

    return isInvalid;
  }

  /**
   *  Checks if text is contains invalid chars and adds to ValidationManager if they are any.
   *
   * @param {string} text
   * @param {string[]} invalidChars
   * @param {string} prop
   * @param {string} prettyProp
   * @param {ValidationResult} result
   * @param {ValidationManager} validationManager
   *
   * @memberOf Utilities
   */
  public static validateInvalidCharInText(text: string, invalidChars: string[], prop: string, prettyProp: string, result: ValidationResult, validationManager: ValidationManager) {
    let isInvalid = false;

    // No need to keep going if text is null.
    if (this.isStringEmptyOrNull(text)) {
      return isInvalid;
    }

    for (const char of invalidChars) {
      if (text.indexOf(char) > -1) {
        isInvalid = true;
        validationManager.addErrorWithFormat(ValidationCode.InvalidChar, prettyProp, char);
        result.addError(prop);
        isInvalid = true;
        break;
      }
    }

    return isInvalid;
  }

  public static validateDupData(
    currentString: string,
    restOfstrings: string[],
    prop: string,
    prettyProp: string,
    result: ValidationResult,
    validationManager: ValidationManager
  ): boolean {
    let isValid = false;

    if (this.isStringEmptyOrNull(currentString) || restOfstrings == null) {
      return isValid;
    }

    const numOfDups = restOfstrings.filter(x => x === currentString).length;

    if (numOfDups > 1) {
      validationManager.addErrorWithFormat(ValidationCode.DuplicateData, prettyProp);
      result.addError(prop);
      isValid = true;
    }

    return isValid;
  }

  public static validateEmail(email: string, prop: string, prettyProp: string, result: ValidationResult, validationManager: ValidationManager) {
    // ^[_A-Za-z0-9-]+(\\.[_A-Za-z0-9-]+)*@[A-Za-z0-9-]+(\\.[A-Za-z0-9]+)*(\\.[A-Za-z]{2,})$

    const re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    const isEmailValid = re.test(String(email).toLowerCase());

    if (!isEmailValid) {
      validationManager.addErrorWithFormat(ValidationCode.ValueInInvalidFormat, prettyProp, 'johndoe@example.com');
      result.addError(prop);
    }
  }

  public static validateHrs(hrsValidationContext: HrsValidationContext) {
    if (hrsValidationContext.hrs == null || hrsValidationContext.hrs.toString() === '') {
      hrsValidationContext.validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, `${hrsValidationContext.prettyProp}`);
      hrsValidationContext.result.addError(hrsValidationContext.prop);
    }
    if (hrsValidationContext.hrs !== undefined && hrsValidationContext.hrs !== null && hrsValidationContext.hrs.toString() !== '' && +hrsValidationContext.hrs > 24.0) {
      hrsValidationContext.validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, `${hrsValidationContext.prettyProp} must be less than or equal to 24.0.`);
      hrsValidationContext.result.addError(hrsValidationContext.prop);
    }
    if (hrsValidationContext.hrs !== undefined && hrsValidationContext.hrs != null && hrsValidationContext.hrs.toString() !== '' && +hrsValidationContext.hrs < 0.5) {
      if (hrsValidationContext.canEnterZero === false) {
        hrsValidationContext.validationManager.addErrorWithDetail(
          ValidationCode.ValueOutOfRange_Details,
          `${hrsValidationContext.prettyProp} must be greater than or equal to 0.5.`
        );
        hrsValidationContext.result.addError(hrsValidationContext.prop);
      }
    }
    if (hrsValidationContext.hrs && hrsValidationContext.hrs.toString() !== '' && hrsValidationContext.hrs.toString().includes('.')) {
      const hrsDecimal = hrsValidationContext.hrs.toString().split('.')[1];
      if (+hrsDecimal !== 0 && +hrsDecimal !== 5) {
        hrsValidationContext.validationManager.addErrorWithDetail(ValidationCode.InformationIncorrect, `${hrsValidationContext.prettyProp} must be whole or half Hours.`);
        hrsValidationContext.result.addError(hrsValidationContext.prop);
      } else if (hrsDecimal === '') {
        hrsValidationContext.validationManager.addErrorWithDetail(ValidationCode.InformationIncorrect, `${hrsValidationContext.prettyProp} must end with .0 or .5.`);
        hrsValidationContext.result.addError(hrsValidationContext.prop);
      }
    } else if (hrsValidationContext.hrs && hrsValidationContext.hrs.toString() !== '' && !hrsValidationContext.hrs.toString().includes('.')) {
      hrsValidationContext.validationManager.addErrorWithDetail(ValidationCode.InformationIncorrect, `${hrsValidationContext.prettyProp} must end with .0 or .5.`);
      hrsValidationContext.result.addError(hrsValidationContext.prop);
    }
  }

  /**
   * Validates a required text field.
   *
   * @static
   * @param {string} text
   * @param {string} prop
   * @param {string} prettyProp
   * @param {string} notAllowedChars
   * @param {*} regEx
   * @param {ValidationResult} result
   * @param {ValidationManager} validationManager
   * @memberof Utilities
   */
  public static validateTextWithRegEx(
    text: string,
    prop: string,
    prettyProp: string,
    allowedChars: string,
    regEx: any,
    result: ValidationResult,
    validationManager: ValidationManager
  ) {
    const isValid = regEx.test(String(text).toLowerCase());

    if (!isValid) {
      validationManager.addErrorWithFormat(ValidationCode.InvalidText, prettyProp, allowedChars);
      result.addError(prop);
    }
  }

  public static validateSnn(ssn: string, prop: string, prettyProp: string, result: ValidationResult, validationManager: ValidationManager) {
    // Validate the format.
    if (ssn == null || ssn.toString().trim() === '' || ssn.toString().length !== 9) {
      // invalid.
      validationManager.addErrorWithFormat(ValidationCode.ValueInInvalidFormat, prettyProp, 'XXX-XX-XXXX');
      result.addError(prop);
    } else {
      const firstPart = ssn.toString().substring(0, 3);
      const secondPart = ssn.toString().substring(3, 5);
      const thirdPart = ssn.toString().substring(5);

      // 1st set of 3 numbers cannot be equal to or less than 0 or 999 or 666.
      if (+firstPart <= 0 || +firstPart === 666 || +firstPart === 999) {
        // invalid.
        validationManager.addErrorWithFormat(ValidationCode.SsnValidSubset, prettyProp, firstPart, '000, 999 or 666');
        result.addError(prop);
      }

      // 2nd set of 3 numbers cannot be equal to or less than 0;
      if (+secondPart <= 0) {
        // invalid.
        validationManager.addErrorWithFormat(ValidationCode.SsnValidSubset, prettyProp, secondPart, 'less than or equal to 0');
        result.addError(prop);
      }

      // 3rd set of 3 numbers cannot be equal to or less than 0;
      if (+thirdPart <= 0) {
        // invalid.
        validationManager.addErrorWithFormat(ValidationCode.SsnValidSubset, prettyProp, thirdPart, 'less than or equal to 0');
        result.addError(prop);
      }
    }
  }

  /**
   *  Checks if select is empty and adds to ValidationManager if it is.
   *  Remember dropdowns when selected turn into strings even though the id is a number type.
   *
   * @param {any} dropDownId
   * @param {string} prop
   * @param {string} prettyProp
   * @param {ValidationResult} result
   * @param {ValidationManager} validationManager
   *
   * @memberOf Utilities
   */
  public static validateDropDown(dropDownId, prop: string, prettyProp: string, result: ValidationResult, validationManager: ValidationManager) {
    let isDropDownEmpty = false;
    if (dropDownId == null || dropDownId.toString() === '' || dropDownId === 0) {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, prettyProp);
      result.addError(prop);
      isDropDownEmpty = true;
    }

    return isDropDownEmpty;
  }

  /**
   * Checks if multi select is empty and adds to ValidationManager if it is.
   *
   * @static
   * @param {any} dropDownId
   * @param {string} prop
   * @param {string} prettyProp
   * @param {ValidationResult} result
   * @param {ValidationManager} validationManager
   * @returns
   *
   * @memberOf Utilities
   */
  public static validateMultiSelect(dropDownIds: number[], prop: string, prettyProp: string, result: ValidationResult, validationManager: ValidationManager) {
    let isDropDownEmpty = false;
    if (dropDownIds == null || dropDownIds.length === 0) {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, prettyProp);
      result.addError(prop);
      isDropDownEmpty = true;
    }

    return isDropDownEmpty;
  }

  public static sumArrayByProperty(objectArray, property): number {
    let sum = 0;
    if (objectArray) {
      sum = _.sumBy(objectArray, item => Number(item[property]));
    }
    return sum;
  }

  public static sortArrayByDate(objectArray, property, isSortByDateToggled) {
    if (!objectArray) {
      return;
    }

    const sortedObj = isSortByDateToggled ? _.orderBy(objectArray, [property], ['desc']) : _.orderBy(objectArray, [property], ['asc']);
    return sortedObj;
  }

  public static sortArrayByProperty(objectArray, property, isDesc = false) {
    if (!objectArray) {
      return;
    }

    const sortedObj = isDesc ? _.orderBy(objectArray, [property], ['desc']) : _.orderBy(objectArray, [property], ['asc']);
    return sortedObj;
  }

  public static getPullDownDate(pullDownDates: DropDownMultiField[], date: moment.Moment) {
    const checkDate = date.clone();
    let pullDownDate = null;
    const pullDownDateEntry = pullDownDates.find(i => i['benefitMonth'] === checkDate.month() + 1 && i['benefitYear'] === checkDate.year());
    if (!_.isEmpty(pullDownDateEntry)) {
      pullDownDate = pullDownDateEntry['pullDownDate'];
    }

    return pullDownDate;
  }

  public static asideSlider() {
    $(window).on('scroll', function() {
      const element = $('#sidebar');
      if (1.2 * $('#sidebar').outerHeight() < $(window).height()) {
        const scrollWindow = $(window).scrollTop(),
          size = scrollWindow - $('app-header').outerHeight() - $('app-sub-header').outerHeight() + 40;
        size > 0 ? element.css('top', size + 'px') : element.css('top', '0');
      } else element.css('top', '0');
    });
  }

  public static participationPeriodCheck(pullDownDates: DropDownMultiField[], date: moment.Moment, notConsider15th = false, considerDate = 15) {
    if (!date || !date.isValid()) return false;

    const dateForFilter = date.clone();
    const dateFilter = dateForFilter.date() > considerDate ? dateForFilter.add(1, 'M') : dateForFilter;
    const latestPullDownDate = this.getPullDownDate(pullDownDates, dateFilter);
    const pullDownDate = moment(moment(latestPullDownDate).format('MM/DD/YYYY')).parseZone();
    const currentDate = this.currentDate.clone();
    const compareCurrent = moment(currentDate.format('MM/DD/YYYY')).parseZone();
    const compareDate = moment(date.format('MM/DD/YYYY')).parseZone();
    const current15th = moment(
      currentDate
        .startOf('M')
        .add(14, 'days')
        .add(dateForFilter.month() - currentDate.month(), 'M')
        .format('MM/DD/YYYY')
    ).parseZone();

    return notConsider15th
      ? compareCurrent.isAfter(pullDownDate) && compareDate.isSameOrBefore(current15th)
      : compareCurrent.isAfter(pullDownDate) && compareDate.isBefore(current15th);
  }

  public static converObjectToArray(object, ignoreProperties) {
    const array = [];

    Object.keys(object).map(index => {
      if (ignoreProperties.indexOf(index) < 0) {
        const obj = history.state[index];
        array.push(obj);
      }
    });

    return array;
  }

  public static getRangeBetweenNums(start, end, step, includeEnd): number[] {
    if (includeEnd) return _.range(start, end + 1, step);
    else return _.range(start, end, step);
  }

  /**
   * Retuns Overlapped dates from the array of start and end date objects.
   * @static
   * @param {any} dateRanges
   *
   * @memberOf Utilities
   */
  public static datesOverlap(dateRanges): any {
    const sortedRanges = dateRanges.sort((previous, current) => {
      // get the start date from previous and current
      const previousTime = previous.start.getTime();
      const currentTime = current.start.getTime();

      // if the previous is earlier than the current
      if (previousTime < currentTime) {
        return -1;
      }

      // if the previous time is the same as the current time
      if (previousTime === currentTime) {
        return 0;
      }

      // if the previous time is later than the current time
      return 1;
    });

    const result = sortedRanges.reduce(
      function(result, current, idx, arr) {
        // get the previous range
        if (idx === 0) {
          return result;
        }
        const previous = arr[idx - 1];

        // check for any overlap
        const previousEnd = previous.end.getTime();
        const currentStart = current.start.getTime();
        const overlap = previousEnd >= currentStart;

        // store the result
        if (overlap) {
          // yes, there is overlap
          result.overlap = true;
          // store the specific ranges that overlap
          result.ranges.push({
            previous: previous,
            current: current
          });
        }

        return result;

        // seed the reduce
      },
      { overlap: false, ranges: [] }
    );

    // return the final results
    return result;
  }

  public static setObjectValues(obj, val, ignoreProperties) {
    let newObj = null;

    if (obj)
      newObj = Object.keys(obj).forEach(i => {
        if (ignoreProperties.indexOf(i) < 0) obj[i] = val;
      });

    return newObj;
  }

  public static stringIsNullOrWhiteSpace(val: string): boolean {
    return !val || val.trim() === '';
  }

  public static convertDecimalToRoundedString(val: any, scale: number): string {
    const roundingFactor = scale * 10;
    return val ? (Math.round(val * roundingFactor) / roundingFactor).toFixed(scale) : val;
  }

  public static isEndOf(value: string, checkValue: string): boolean {
    return value.substr(-checkValue.length, checkValue.length) === checkValue;
  }
}

// Collection Utilities
const _hasOwnProperty = Object.prototype.hasOwnProperty;
export const has = function(obj: any, prop: any) {
  return _hasOwnProperty.call(obj, prop);
};

/**
 * Function signature for comparing
 * <0 means a is smaller
 * = 0 means they are equal
 * >0 means a is larger
 */
export type ICompareFunction<T> = (a: T, b: T) => number;

/**
 * Function signature for checking equality
 */
export type IEqualsFunction<T> = (a: T, b: T) => boolean;

export type IFilterFunction<T> = (value: T, index?: number, array?: T[]) => boolean;

/**
 * Function signature for Iterations. Return false to break from loop
 */
export type ILoopFunction<T> = (a: T) => boolean | void;

export type IKeySelectorFunction<T, K> = (a: T) => K;

/**
 * Default function to compare element order.
 * @function
 */
export function defaultCompare<T>(a: T, b: T): number {
  if (a < b) {
    return -1;
  } else if (a === b) {
    return 0;
  } else {
    return 1;
  }
}

/**
 * Default function to test equality.
 * @function
 */
export function defaultEquals<T>(a: T, b: T): boolean {
  return a === b;
}

/**
 * Default function to convert an object to a string.
 * @function
 */
export function defaultToString(item: any): string {
  if (item === null) {
    return 'COLLECTION_NULL';
  } else if (isUndefined(item)) {
    return 'COLLECTION_UNDEFINED';
  } else if (isString(item)) {
    return '$s' + item;
  } else {
    return '$o' + item.toString();
  }
}

/**
 * Joins all the properies of the object using the provided join string
 */
export function makeString<T>(item: T, join: string = ','): string {
  if (item === null) {
    return 'COLLECTION_NULL';
  } else if (isUndefined(item)) {
    return 'COLLECTION_UNDEFINED';
  } else if (isString(item)) {
    return item.toString();
  } else {
    let toret = '{';
    let first = true;
    for (const prop in item) {
      if (has(item, prop)) {
        if (first) {
          first = false;
        } else {
          toret = toret + join;
        }
        toret = toret + prop + ':' + (<any>item)[prop];
      }
    }
    return toret + '}';
  }
}

/**
 * Checks if the given argument is a function.
 * @function
 */
export function isFunction(func: any): boolean {
  return typeof func === 'function';
}

/**
 * Checks if the given argument is undefined.
 * @function
 */
export function isUndefined(obj: any): boolean {
  return typeof obj === 'undefined';
}

/**
 * Checks if the given argument is a string.
 * @function
 */
export function isString(obj: any): boolean {
  return Object.prototype.toString.call(obj) === '[object String]';
}

/**
 * Reverses a compare function.
 * @function
 */
export function reverseCompareFunction<T>(compareFunction: ICompareFunction<T>): ICompareFunction<T> {
  if (!isFunction(compareFunction)) {
    return function(a, b) {
      if (a < b) {
        return 1;
      } else if (a === b) {
        return 0;
      } else {
        return -1;
      }
    };
  } else {
    return function(d: T, v: T) {
      return compareFunction(d, v) * -1;
    };
  }
}

/**
 * Returns an equal function given a compare function.
 * @function
 */
export function compareToEquals<T>(compareFunction: ICompareFunction<T>): IEqualsFunction<T> {
  return function(a: T, b: T) {
    return compareFunction(a, b) === 0;
  };
}

export abstract class GenericIterator<T> implements IterableIterator<T> {
  public next = (): IteratorResult<T> => {
    return { done: true, value: null };
  };

  public return? = (): IteratorResult<T> => null;
  public throw? = (): IteratorResult<T> => null;

  [Symbol.iterator](): IterableIterator<T> {
    return this;
  }
}

export class TextTransformer {
  /**
   * This just strips tags from the HTML. newlines and breaks should be preserved
   * http://locutus.io/php/strings/strip_tags/
   * @param pee
   */
  public static strip_tags(input: string, allowed: string = '') {
    allowed = (((allowed || '') + '').toLowerCase().match(/<[a-z][a-z0-9]*>/g) || []).join('');

    const tags = /<\/?([a-z][a-z0-9]*)\b[^>]*>/gi;
    const commentsAndPhpTags = /<!--[\s\S]*?-->|<\?(?:php)?[\s\S]*?\?>/gi;
    return input.replace(commentsAndPhpTags, '').replace(tags, function($0, $1) {
      return allowed.indexOf('<' + $1.toLowerCase() + '>') > -1 ? $0 : '';
    });
  }

  /**
   * Newline preservation help function for wpautop
   *
   * @since 3.1.0
   * @access private
   *
   * @param array $matches preg_replace_callback matches array
   * @return string
   */
  private static _autop_newline_preservation_helper(matches) {
    return matches[0].replace('\n', '<WPPreserveNewline />');
  }

  /**
   * JavaScript port of wpautop
   * @see: http://develop.svn.wordpress.org/trunk/src/wp-includes/formatting.php
   */

  /**
   * Replaces double line-breaks with paragraph elements.
   *
   * A group of regex replaces used to identify text formatted with newlines and
   * replace double line-breaks with HTML paragraph tags. The remaining
   * line-breaks after conversion become <<br />> tags, unless $br is set to '0'
   * or 'false'.
   *
   * @since 0.71
   *
   * @param string pee The text which has to be formatted.
   * @param bool br Optional. If set, this will convert all remaining line-breaks after paragraphing. Default true.
   * @return string Text which has been converted into correct paragraph tags.
   */
  static wpautop(pee: string, br = true) {
    const pre_tags = new Map();
    if (pee == null || pee.trim() === '') {
      return '';
    }

    pee = pee + '\n'; // just to make things a little easier, pad the end
    if (pee.indexOf('<pre') > -1) {
      const pee_parts = pee.split('</pre>');
      const last_pee = pee_parts.pop();
      pee = '';
      pee_parts.forEach(function(pee_part, index) {
        const start = pee_part.indexOf('<pre');

        // Malformed html?
        if (start === -1) {
          pee += pee_part;
          return;
        }

        const name = '<pre wp-pre-tag-' + index + '></pre>';
        pre_tags[name] = pee_part.substr(start) + '</pre>';
        pee += pee_part.substr(0, start) + name;
      });

      pee += last_pee;
    }

    pee = pee.replace(/<br \/>\s*<br \/>/, '\n\n');

    // Space things out a little
    const allblocks =
      '(?:table|thead|tfoot|caption|col|colgroup|tbody|tr|td|th|div|dl|dd|dt|ul|ol|li|pre|form|map|area|blockquote|address|math|style|p|h[1-6]|hr|fieldset|legend|section|article|aside|hgroup|header|footer|nav|figure|figcaption|details|menu|summary)';
    pee = pee.replace(new RegExp('(<' + allblocks + '[^>]*>)', 'gmi'), '\n$1');
    pee = pee.replace(new RegExp('(</' + allblocks + '>)', 'gmi'), '$1\n\n');
    pee = pee.replace(/\r\n|\r/, '\n'); // cross-platform newlines

    if (pee.indexOf('<option') > -1) {
      // no P/BR around option
      pee = pee.replace(/\s*<option'/gim, '<option');
      pee = pee.replace(/<\/option>\s*/gim, '</option>');
    }

    if (pee.indexOf('</object>') > -1) {
      // no P/BR around param and embed
      pee = pee.replace(/(<object[^>]*>)\s*/gim, '$1');
      pee = pee.replace(/\s*<\/object>/gim, '</object>');
      pee = pee.replace(/\s*(<\/?(?:param|embed)[^>]*>)\s*/gim, '$1');
    }

    if (pee.indexOf('<source') > -1 || pee.indexOf('<track') > -1) {
      // no P/BR around source and track
      pee = pee.replace(/([<\[](?:audio|video)[^>\]]*[>\]])\s*/gim, '$1');
      pee = pee.replace(/\s*([<\[]\/(?:audio|video)[>\]])/gim, '$1');
      pee = pee.replace(/\s*(<(?:source|track)[^>]*>)\s*/gim, '$1');
    }

    pee = pee.replace(/\n\n+/gim, '\n\n'); // take care of duplicates

    // make paragraphs, including one at the end
    const pees = pee.split(/\n\s*\n/);
    pee = '';
    pees.forEach(function(tinkle) {
      pee += '<p>' + tinkle.replace(/^\s+|\s+$/g, '') + '</p>\n';
    });

    pee = pee.replace(/<p>\s*<\/p>/gim, ''); // under certain strange conditions it could create a P of entirely whitespace
    pee = pee.replace(/<p>([^<]+)<\/(div|address|form)>/gim, '<p>$1</p></$2>');
    pee = pee.replace(new RegExp('<p>s*(</?' + allblocks + '[^>]*>)s*</p>', 'gmi'), '$1'); // don't pee all over a tag
    pee = pee.replace(/<p>(<li.+?)<\/p>/gim, '$1'); // problem with nested lists
    pee = pee.replace(/<p><blockquote([^>]*)>/gim, '<blockquote$1><p>');
    pee = pee.replace(/<\/blockquote><\/p>/gim, '</p></blockquote>');
    pee = pee.replace(new RegExp('<p>s*(</?' + allblocks + '[^>]*>)', 'gmi'), '$1');
    pee = pee.replace(new RegExp('(</?' + allblocks + '[^>]*>)s*</p>', 'gmi'), '$1');

    if (br) {
      pee = pee.replace(/<(script|style)(?:.|\n)*?<\/\\1>/gim, this._autop_newline_preservation_helper); // /s modifier from php PCRE regexp replaced with (?:.|\n)
      pee = pee.replace(/(<br \/>)?\s*\n/gim, '<br />\n'); // optionally make line breaks
      pee = pee.replace('<WPPreserveNewline />', '\n');
    }

    pee = pee.replace(new RegExp('(</?' + allblocks + '[^>]*>)s*<br />', 'gmi'), '$1');
    pee = pee.replace(/<br \/>(\s*<\/?(?:p|li|div|dl|dd|dt|th|pre|td|ul|ol)[^>]*>)/gim, '$1');
    pee = pee.replace(/\n<\/p>$/gim, '</p>');

    if (Object.keys(pre_tags).length) {
      pee = pee.replace(new RegExp(Object.keys(pre_tags).join('|'), 'gi'), function(matched) {
        return pre_tags[matched];
      });
    }

    return pee;
  }
}

export class EnumEx {
  static getNamesAndValues<T extends number>(e: any) {
    return EnumEx.getNames(e).map(n => ({ name: n, value: e[n] as T }));
  }

  static getNames(e: any) {
    return EnumEx.getObjValues(e).filter(v => typeof v === 'string') as string[];
  }

  static getValues<T extends number>(e: any) {
    return EnumEx.getObjValues(e).filter(v => typeof v === 'number') as T[];
  }

  private static getObjValues(e: any): (number | string)[] {
    return Object.keys(e).map(k => e[k]);
  }
}
