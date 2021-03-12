import { ValidationCode, ValidationError } from '../../shared/models/validation-error';
import { DropDownField } from '../../shared/models/dropdown-field';
export class TestingUtilities {
  static getErrorMessagesByCode(errorCode: ValidationCode, errors: ValidationError[]): string[] {
    const errorMsgs: string[] = [];

    for (const e of errors) {
      if (e.code === (errorCode as number)) {
        for (const d of e.detailItems) {
          errorMsgs.push(d);
        }
      }
    }

    return errorMsgs;
  }

  /**
   * Creates and returns by field name a new dropdown field with id set to a random number between 0 - 9999999.
   *
   * @private
   * @static
   * @param {string} name
   * @returns {DropDownField}
   *
   * @memberof TestingUtilities
   */
  private static newDropDownWithRandomId(name: string): DropDownField {
    const x = new DropDownField();
    x.id = Math.floor(Math.random() * (9999999 - 0 + 1)) + 0;
    x.name = name;
    return x;
  }

  static getDropDownByName(name: string) {
    switch (name) {
      case 'polarUnknown': {
        const polarDropdown: DropDownField[] = [];
        polarDropdown.push(this.newDropDownWithRandomId('No'));
        polarDropdown.push(this.newDropDownWithRandomId('Yes'));
        polarDropdown.push(this.newDropDownWithRandomId('Unknown'));
        return polarDropdown;
      }
      case 'relationshipNCP': {
        const polarDropdown: DropDownField[] = [];
        polarDropdown.push(this.newDropDownWithRandomId('Other Parent'));
        polarDropdown.push(this.newDropDownWithRandomId('Grandparent'));
        polarDropdown.push(this.newDropDownWithRandomId('Aunt/Uncle/Cousin'));
        polarDropdown.push(this.newDropDownWithRandomId('Adult Brother/Sister'));
        polarDropdown.push(this.newDropDownWithRandomId('Youth Out-of-Home Placement'));
        polarDropdown.push(this.newDropDownWithRandomId('Friend/Unrelated'));
        polarDropdown.push(this.newDropDownWithRandomId('Other'));
        polarDropdown.push(this.newDropDownWithRandomId('Unknown'));
        return polarDropdown;
      }
      case 'contactInterval': {
        const polarDropdown: DropDownField[] = [];
        polarDropdown.push(this.newDropDownWithRandomId('3+ times per week'));
        polarDropdown.push(this.newDropDownWithRandomId('1-2 times per week'));
        polarDropdown.push(this.newDropDownWithRandomId('1-3 times per month'));
        polarDropdown.push(this.newDropDownWithRandomId('Less than once a month'));
        polarDropdown.push(this.newDropDownWithRandomId('No Contact'));
        return polarDropdown;
      }
      default: {
        break;
      }
    }
  }
}
