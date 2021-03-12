import { Utilities } from './utilities';
import * as moment from 'moment';

import { ModelErrors } from './interfaces/model-errors';
import { ValidationCode } from './models/validation-error';
import { ValidationContext, MmYyyyValidationContext, YyyyValidationContext } from './interfaces/validation-context';
import { MmDdYyyyValidationContext } from './interfaces/mmDdYyyy-validation-context';
import { ValidationManager } from './models/validation-manager';
import { ValidationResult } from './models/validation-result';
import { AppService } from 'src/app/core/services/app.service';

export class Validate {
  /**
   * Validates a YYYY input field value against the supplied rules context.
   *
   * @static
   * @param {YyyyValidationContext} context
   * @param {ValidationManager} validationManager
   * @param {ValidationResult} result
   * @param {ModelErrors} parentModelErrors
   * @returns {boolean}
   *
   * @memberOf Validate
   */
  public static yyyyDate(context: YyyyValidationContext, validationManager: ValidationManager, result: ValidationResult, parentModelErrors?: ModelErrors): boolean {
    let isValid = true;

    if (context.yyyy != null && context.yyyy.toString() !== '') {
      // YYYY format
      if (context.yyyy < 1000) {
        validationManager.addErrorWithFormat(ValidationCode.ValueInInvalidFormat_Name_FormatType, context.prettyProp, 'YYYY');
        if (parentModelErrors == null) {
          result.addError(context.prop);
        } else {
          result.addErrorForParent(parentModelErrors, context.prop);
        }

        isValid = false;
      } else if (context.minDateParticipantDobYear != null && context.yyyy < context.minDateParticipantDobYear) {
        validationManager.addErrorWithFormat(
          ValidationCode.ValueBeforeDOB_Name_Value_DOB,
          context.prettyProp,
          context.yyyy.toString(),
          context.minDateParticipantDobYear.toString()
        );
        if (parentModelErrors == null) {
          result.addError(context.prop);
        } else {
          result.addErrorForParent(parentModelErrors, context.prop);
        }

        isValid = false;
      } else if (context.maxDateNotInFuture) {
        const currentDateYYYY = Utilities.currentDate.year();
        if (context.yyyy > currentDateYYYY) {
          validationManager.addErrorWithFormat(ValidationCode.DateAfterDate_Name1_Name2, context.prettyProp, 'Current Date');
          if (parentModelErrors == null) {
            result.addError(context.prop);
          } else {
            result.addErrorForParent(parentModelErrors, context.prop);
          }

          isValid = false;
        }
      }
    } else if (context.isRequired === true) {
      isValid = false;
    }

    return isValid;
  }

  /**
   * Validates a dropdown input field and reports any error via the ValiationManager.
   *
   * @static
   * @param {*} dropDownId
   * @param {ValidationContext} context
   * @param {ValidationManager} validationManager
   * @param {ValidationResult} result
   * @param {ModelErrors} [parentModelErrors]
   * @returns {boolean}
   *
   * @memberOf Validate
   */
  // tslint:disable-next-line:max-line-length
  public static dropDown(dropDownId: any, context: ValidationContext, validationManager: ValidationManager, result: ValidationResult, parentModelErrors?: ModelErrors): boolean {
    let isValid = true;

    // The only validation we are doing at this point is if it's required.
    if (context.isRequired) {
      if (dropDownId == null || dropDownId.toString() === '') {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, context.prettyProp);
        if (parentModelErrors == null) {
          result.addError(context.prop);
        } else {
          result.addErrorForParent(parentModelErrors, context.prop);
        }

        isValid = false;
      }
    }

    return isValid;
  }

  /**
   *  Validates a MM/YYYY date.
   *  If participantDOB is supplied date can not exceed 120 years from participant dob.
   *  Min and Max dates can be provided as MM/DD/YYYY or MM/YYYY we will default both cases to first of the month.
   *
   * @memberOf Validate
   */
  // tslint:disable-next-line:max-line-length
  public static mmYyyyDate(
    date: string,
    context: MmYyyyValidationContext,
    validationManager: ValidationManager,
    result: ValidationResult,
    parentModelErrors?: ModelErrors
  ): boolean {
    let isValid = true;

    // Correct Dates so they are first of the month.
    if (context.minDate != null && context.minDate.length === 10) {
      const fixedDate = moment(context.minDate, 'MM/DD/YYYY').date(1);
      context.minDate = fixedDate.format('MM/DD/YYYY');
    }

    if (context.maxDate != null && context.maxDate.length === 10) {
      const fixedDate = moment(context.maxDate, 'MM/DD/YYYY').date(1);
      context.maxDate = fixedDate.format('MM/DD/YYYY');
    }

    if (context.minDate != null && context.minDate.length === 7) {
      const fixedDate = moment(context.minDate, 'MM/YYYY').date(1);
      context.minDate = fixedDate.format('MM/DD/YYYY');
    }

    if (context.maxDate != null && context.maxDate.length === 7) {
      const fixedDate = moment(context.maxDate, 'MM/YYYY').date(1);
      context.maxDate = fixedDate.format('MM/DD/YYYY');
    }

    // Set to the first of the month by using MM/YYYY.
    const inputDate = moment(date, 'MM/YYYY');
    if (context.isRequired === true) {
      if (date == null || date.trim() === '') {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, context.prettyProp);
        if (parentModelErrors == null) {
          result.addError(context.prop);
        } else {
          result.addErrorForParent(parentModelErrors, context.prop);
        }

        isValid = false;
      }
    }

    if (date != null && date.trim() !== '' && date.length !== 7) {
      // Date must be 6 digits in two digit month, and four digit year format. (MM/YYYY).
      validationManager.addErrorWithFormat(ValidationCode.ValueInInvalidFormat_Name_FormatType, context.prettyProp, 'MM/YYYY');
      if (parentModelErrors == null) {
        result.addError(context.prop);
      } else {
        result.addErrorForParent(parentModelErrors, context.prop);
      }
      isValid = false;
    } else if (inputDate.isValid()) {
      // Validate against the Participant's DOB as the Min Date.
      if (context.minDateUseParticipantDob) {
        if (context.participantDob == null) {
          console.log('ERROR: participantDob has not been set when using minDateUseParticipantDob!!!');
        } else {
          // Min date allow same month.
          if (context.minDateAllowSame && inputDate < context.participantDob) {
            validationManager.addErrorWithFormat(
              ValidationCode.ValueBeforeDOB_Name_Value_DOB,
              context.prettyProp,
              inputDate.format('MM/DD/YYYY'),
              context.participantDob.format('MM/DD/YYYY')
            );
            if (parentModelErrors == null) {
              result.addError(context.prop);
            } else {
              result.addErrorForParent(parentModelErrors, context.prop);
            }
            isValid = false;
          }
          // Min date.
          if (!context.minDateAllowSame && inputDate <= context.participantDob) {
            validationManager.addErrorWithFormat(
              ValidationCode.ValueBeforeDOB_Name_Value_DOB,
              context.prettyProp,
              inputDate.format('MM/DD/YYYY'),
              context.participantDob.format('MM/DD/YYYY')
            );
            if (parentModelErrors == null) {
              result.addError(context.prop);
            } else {
              result.addErrorForParent(parentModelErrors, context.prop);
            }
            isValid = false;
          }
        }
      } else {
        // Validate against the supplied Min Date.

        // Min date allow same month.
        const minDate = moment(context.minDate, 'MM/DD/YYYY');
        if (context.minDate != null && context.minDateAllowSame && inputDate < minDate) {
          validationManager.addErrorWithFormat(ValidationCode.DateBeforeDate_Name1_Name2, context.prettyProp, context.minDateName);
          if (parentModelErrors == null) {
            result.addError(context.prop);
          } else {
            result.addErrorForParent(parentModelErrors, context.prop);
          }
          isValid = false;
        }
        // Min date.
        if (context.minDate != null && !context.minDateAllowSame && inputDate <= minDate) {
          validationManager.addErrorWithFormat(ValidationCode.DateBeforeDate_Name1_Name2, context.prettyProp, context.minDateName);
          if (parentModelErrors == null) {
            result.addError(context.prop);
          } else {
            result.addErrorForParent(parentModelErrors, context.prop);
          }
          isValid = false;
        }
      }
      // Max date.
      const maxDate = moment(context.maxDate, 'MM/DD/YYYY');
      if (context.maxDate != null && !context.maxDateAllowSame && inputDate >= maxDate) {
        validationManager.addErrorWithFormat(ValidationCode.DateAfterDate_Name1_Name2, context.prettyProp, context.maxDateName);
        if (parentModelErrors == null) {
          result.addError(context.prop);
        } else {
          result.addErrorForParent(parentModelErrors, context.prop);
        }
        isValid = false;
      }
      // Max date allow same month.
      if (context.maxDate != null && context.maxDateAllowSame && inputDate > maxDate) {
        validationManager.addErrorWithFormat(ValidationCode.DateAfterDate_Name1_Name2, context.prettyProp, context.maxDateName);
        if (parentModelErrors == null) {
          result.addError(context.prop);
        } else {
          result.addErrorForParent(parentModelErrors, context.prop);
        }
        isValid = false;
      }
    } else {
      if (date != null && date.trim() !== '' && date.length !== 7) {
        validationManager.addErrorWithDetail(ValidationCode.InvalidDate, 'Invalid ' + context.prettyProp + ' Entered');
        if (parentModelErrors == null) {
          result.addError(context.prop);
        } else {
          result.addErrorForParent(parentModelErrors, context.prop);
        }
        isValid = false;
      }
    }

    return isValid;
  }

  /**
   *  Validates a MM/DD/YYYY date.
   *  If participantDOB is supplied date can not exceed 120 years from participant dob.
   *
   * @memberOf Utilities
   */
  public static validateMmDdYyyyDate(mmDdYyyyValidation: MmDdYyyyValidationContext, years: number = 120): boolean {
    // Correct min and max MM/YYYY dates so they are first of the month.
    if (mmDdYyyyValidation.minDate != null && mmDdYyyyValidation.minDate.length === 7) {
      const fixedDate = moment(mmDdYyyyValidation.minDate, 'MM/YYYY').date(1);
      mmDdYyyyValidation.minDate = fixedDate.format('MM/DD/YYYY');
    }

    if (mmDdYyyyValidation.maxDate != null && mmDdYyyyValidation.maxDate.length === 7) {
      const fixedDate = moment(mmDdYyyyValidation.maxDate, 'MM/YYYY').date(1);
      mmDdYyyyValidation.maxDate = fixedDate.format('MM/DD/YYYY');
    }

    let isDateValid = true;

    const inputDate = moment.utc(mmDdYyyyValidation.date);
    // console.log(inputDate.date.length);
    if (mmDdYyyyValidation.isRequired === true) {
      if (mmDdYyyyValidation.date == null || mmDdYyyyValidation.date.trim() === '') {
        mmDdYyyyValidation.validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, mmDdYyyyValidation.prettyProp);
        mmDdYyyyValidation.result.addError(mmDdYyyyValidation.prop);
        isDateValid = false;
      }
    }
    if (mmDdYyyyValidation.date != null && mmDdYyyyValidation.date.length !== 0 && mmDdYyyyValidation.date.length !== 24) {
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
      if (mmDdYyyyValidation.minDate != null && !mmDdYyyyValidation.minDateAllowSame && inputDate <= moment(mmDdYyyyValidation.minDate, 'MM/DD/YYYY')) {
        mmDdYyyyValidation.validationManager.addErrorWithFormat(ValidationCode.DateBeforeDate_Name1_Name2, mmDdYyyyValidation.prettyProp, mmDdYyyyValidation.minDateName);
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
   *  Validates a ISO MM/DD/YYYY date.
   *  If participantDOB is supplied date can not exceed 120 years from participant dob.
   *
   * @memberOf Utilities
   */
  public static validateISOMmDdYyyyDate(mmDdYyyyValidation: MmDdYyyyValidationContext, years: number = 120): boolean {
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
      if (mmDdYyyyValidation.minDate != null && !mmDdYyyyValidation.minDateAllowSame && inputDate <= moment(mmDdYyyyValidation.minDate, 'MM/DD/YYYY')) {
        mmDdYyyyValidation.validationManager.addErrorWithFormat(ValidationCode.DateBeforeDate_Name1_Name2, mmDdYyyyValidation.prettyProp, mmDdYyyyValidation.minDateName);
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
   *  * Validates a required google location input field and reports any error via the ValiationManager.
   *
   * @static
   * @param {any} dropDownId
   * @param {string} prop
   * @param {string} prettyProp
   * @param {ValidationResult} result
   * @param {ValidationManager} validationManager
   * @returns
   *
   * @memberof Validate
   */
  public static googleLocation(googleLocation, prop: string, prettyProp: string, result: ValidationResult, validationManager: ValidationManager) {
    if (googleLocation == null || googleLocation.toString().trim() === '' || googleLocation.description == null || googleLocation.description.trim() === '') {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, prettyProp);
      result.addError(prop);
    }
  }

  /**
   * Validates a text input field and reports any error via the ValiationManager.
   *
   * @static
   * @param {string} text
   * @param {ValidationContext} context
   * @param {ValidationManager} validationManager
   * @param {ValidationResult} result
   * @param {ModelErrors} [parentModelErrors]
   * @returns {boolean}
   *
   * @memberOf Validate
   */
  // tslint:disable-next-line:max-line-length
  public static text(text: string, context: ValidationContext, validationManager: ValidationManager, result: ValidationResult, parentModelErrors?: ModelErrors): boolean {
    let isValid = true;

    // The only validation we are doing at this point is if it's required.
    if (context.isRequired) {
      if (text == null || text.trim() === '') {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, context.prettyProp);
        if (parentModelErrors == null) {
          result.addError(context.prop);
        } else {
          result.addErrorForParent(parentModelErrors, context.prop);
        }

        isValid = false;
      }
    }

    return isValid;
  }

  /**
   *  Checks if number is out of range.
   *  Remember dropdowns when selected turn into strings even though the id is a number type.
   *
   * @param {number} numberValue
   * @param {number} maxValue
   * @param {string} prop
   * @param {string} prettyProp
   * @param {ValidationResult} result
   * @param {ValidationManager} validationManager
   *
   * @memberOf Utilities
   */
  public static validateNumberInRange(numberValue: number, maxValue: number, prop: string, prettyProp: string, result: ValidationResult, validationManager: ValidationManager) {
    if (numberValue > maxValue) {
      validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, prettyProp);
      result.addError(prop);
      return true;
    }

    return false;
  }

  /**
   *  Checks if contact is set.
   *
   * @param {contactId} number
   * @param {string} prop
   * @param {string} prettyProp
   * @param {ValidationResult} result
   * @param {ValidationManager} validationManager
   *
   * @memberOf Utilities
   */
  public static validateRequiredContact(contactId: number, prop: string, prettyProp: string, result: ValidationResult, validationManager: ValidationManager) {
    if (contactId == null || isNaN(contactId)) {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, prettyProp);
      result.addError(prop);
      return true;
    }

    return false;
  }
}
