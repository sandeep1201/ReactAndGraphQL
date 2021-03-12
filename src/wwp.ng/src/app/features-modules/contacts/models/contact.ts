import * as moment from 'moment';

import { ValidationManager } from '../../../shared/models/validation-manager';
import { ValidationResult } from '../../../shared/models/validation-result';
import { AppService } from 'src/app/core/services/app.service';
import { Utilities } from '../../../shared/utilities';

/**
 * The Contact object used by the Contacts App.
 *
 * @export
 * @class Contact
 */
export class Contact {
  address: string;
  customTitle: string;
  email: string;
  faxNumber: string;
  id: number;
  isRoiSigned: boolean;
  name: string;
  notes: string;
  phoneExt: string;
  phoneNumber: string;
  roiSignedDate: string;
  rowVersion: string;
  titleTypeId: number;
  titleTypeName: string;
  modifiedBy: string;
  modifiedDate: string;
  modifiedByName: string;

  // psuedo properties
  get title(): string {
    if (this.titleTypeName) {
      if (this.titleTypeName === 'Other') {
        return this.customTitle;
      }
      return this.titleTypeName;
    }

    return '';
  }

  /**
   * Used to convert a JSON response into a proper Contact object.
   *
   * @param {*} input
   * @returns {Contact}
   *
   * @memberOf Contact
   */

  public static clone(input: any, instance: Contact) {
    instance.address = input.address;
    instance.customTitle = input.customTitle;
    instance.email = input.email;
    instance.faxNumber = input.faxNumber;
    instance.id = input.id;
    instance.isRoiSigned = input.isRoiSigned;
    instance.name = input.name;
    instance.notes = input.notes;
    instance.phoneExt = input.phoneExt;
    instance.phoneNumber = input.phoneNumber;
    instance.titleTypeId = input.titleTypeId;
    instance.roiSignedDate = input.roiSignedDate;
    instance.rowVersion = input.rowVersion;
    instance.titleTypeId = input.titleTypeId;
    instance.titleTypeName = input.titleTypeName;
    instance.modifiedBy = input.modifiedBy;
    instance.modifiedDate = input.modifiedDate;
    instance.modifiedByName = input.modifiedByName;
  }
  public deserialize(input: any): Contact {
    this.address = input.address;
    this.customTitle = input.customTitle;
    this.email = input.email;
    this.faxNumber = input.faxNumber;
    this.id = input.id;
    this.isRoiSigned = input.isRoiSigned;
    this.name = input.name;
    this.notes = input.notes;
    this.phoneExt = input.phoneExt;
    this.phoneNumber = input.phoneNumber;
    this.titleTypeId = input.titleTypeId;
    this.roiSignedDate = input.roiSignedDate;
    this.rowVersion = input.rowVersion;
    this.titleTypeId = input.titleTypeId;
    this.titleTypeName = input.titleTypeName;
    this.modifiedBy = input.modifiedBy;
    this.modifiedDate = input.modifiedDate;
    this.modifiedByName = input.modifiedByName;

    return this;
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
  public validate(validationManager: ValidationManager, otherTitleTypeId: number): ValidationResult {
    const result = new ValidationResult();

    if (!this.isTitleValid()) {
      result.addError('titleTypeId');
    }

    if (!this.isCustomTitleValid(otherTitleTypeId)) {
      result.addError('customTitle');
    }

    if (!this.isNameValid()) {
      result.addError('name');
    }

    if (this.isRoiSignedDateRequired()) {
      if (!this.isRoiDateValid()) {
        result.addError('roiSignedDate');
      }
    }

    if (this.isEmailValid()) {
      Utilities.validateEmail(this.email, 'email', 'Email', result, validationManager);
    }

    if (this.phoneNumber && this.phoneNumber.length < 10) {
      result.addError('phoneNumber');
    }

    return result;
  }

  /**
   * Checks the validity of the 'name' property.
   *
   * @returns {boolean}
   *
   * @memberOf Contact
   */
  public isNameValid(): boolean {
    let result = true;

    if (this.name == null || this.name.trim() === '') {
      result = false;
    }

    return result;
  }

  /**
   * Checks the validity of the 'title' property.
   *
   * @returns {boolean}
   *
   * @memberOf Contact
   */
  public isTitleValid(): boolean {
    let result = true;

    if (this.titleTypeId == null) {
      result = false;
      // tslint:disable-next-line:triple-equals
    } else if (this.titleTypeId == 0) {
      result = false;
    } else if (this.titleTypeId.toString() === '') {
      result = false;
    }

    return result;
  }

  /**
   * Checks if the 'customTitle' property is required.
   *
   * @param {number} otherTitleId
   * @returns {boolean}
   *
   * @memberOf Contact
   */
  public isCustomTitleRequired(otherTitleId: number): boolean {
    let result = false;

    // tslint:disable-next-line:triple-equals
    if (this.titleTypeId == otherTitleId) {
      result = true;
    }

    return result;
  }

  /**
   * Checks the validity of the 'customTitle' property.
   *
   * @param {number} otherTitleId
   * @returns {boolean}
   *
   * @memberOf Contact
   */
  public isCustomTitleValid(otherTitleId: number): boolean {
    let result = true;

    // tslint:disable-next-line:triple-equals
    if (this.titleTypeId == otherTitleId && (this.customTitle == null || this.customTitle.trim() === '')) {
      result = false;
    }

    return result;
  }

  /**
   * Indicates if the 'roiSignedDate' property is required.
   *
   * @returns {boolean}
   *
   * @memberOf Contact
   */
  public isRoiSignedDateRequired(): boolean {
    return this.isRoiSigned;
  }

  /**
   * Indicates if the 'roiSignedDate' value is valid.
   *
   * @returns {boolean}
   *
   * @memberOf Contact
   */
  public isRoiDateValid(): boolean {
    if (this.roiSignedDate == null) {
      return false;
    }

    // See if there is a valid date and be sure to use STRICT parsing
    // (using true as last parameter):
    const roiDate = moment(this.roiSignedDate, 'MM/DD/YYYY', true);

    if (!roiDate.isValid()) {
      return false;
    }

    const currentDate = Utilities.currentDate;
    const maxBackDate = Utilities.currentDate.subtract(120, 'years');

    // The ROI Date must not be in the future.
    return roiDate <= currentDate && roiDate >= maxBackDate;
  }

  /**
   * Checks the validity of the 'email' property.
   *
   * @returns {boolean}
   *
   * @memberOf Contact
   */
  public isEmailValid(): boolean {
    let result = true;

    if (this.email == null || this.email.trim() === '') {
      result = false;
    }

    return result;
  }
}
