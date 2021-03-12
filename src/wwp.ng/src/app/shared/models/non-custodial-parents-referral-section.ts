import { Serializable } from '../interfaces/serializable';
import { Utilities } from '../utilities';
import * as moment from 'moment';
import { ActionNeeded } from '../../features-modules/actions-needed/models/action-needed';
import { ActionNeededInfo } from '../../features-modules/actions-needed/models/action-needed-info';
import { Guid } from '../models/guid';
import { ValidationManager } from './validation-manager';
import { ValidationResult } from './validation-result';
import { DropDownField } from '../../shared/models/dropdown-field';
import { MmDdYyyyValidationContext } from '../interfaces/mmDdYyyy-validation-context';

export class CwwChild implements Serializable<CwwChild> {
  firstName: string;
  middleInitial: string;
  lastName: string;
  dateOfBirth: string;
  age: number;
  gender: string;
  relationship: string;

  get ageInYears(): number {
    return Utilities.getAgeFromDateOfBirthWithFormat(this.dateOfBirth, 'M/D/YYYY');
  }

  public deserialize(input: any): CwwChild {
    this.firstName = input.firstName;
    this.middleInitial = input.middleInitial;
    this.lastName = input.lastName;
    this.dateOfBirth = input.dateOfBirth;
    this.age = input.age;
    this.gender = input.gender;
    this.relationship = input.relationship;

    return this;
  }
}

export class NonCustodialParentsReferralSection implements Serializable<NonCustodialParentsReferralSection> {
  isSubmittedViaDriverFlow: boolean;
  id: number;
  cwwChildren: CwwChild[];
  hasChildrenId: number;
  hasChildrenName: string;
  parents: NonCustodialOtherParent[];
  deletedParents: NonCustodialOtherParent[];
  notes: string;
  rowVersion: string;
  assessmentRowVersion: string;
  modifiedBy: string;
  modifiedDate: string;

  get availableParents(): DropDownField[] {
    const availableParents: DropDownField[] = [];
    if (this.parents != null) {
      const count = {};
      for (const ncc of this.parents) {
        if ((ncc.firstName != null && ncc.firstName.trim() !== '') || (ncc.lastName != null && ncc.lastName.trim() !== '')) {
          const ddf = new DropDownField();
          if (ncc.runningGuid == null) {
            ncc.runningGuid = new Guid();
          }

          let firstName = '';
          let lastName = '';

          if (ncc.firstName != null) {
            firstName = ncc.firstName;
          }

          if (ncc.lastName != null) {
            lastName = ncc.lastName;
          }

          let name = Utilities.toTitleCase(firstName.toLowerCase()) + ' ' + Utilities.toTitleCase(lastName.toLowerCase());

          let exists = false;
          for (const act of availableParents) {
            if (act.name === name) {
              exists = true;
            }
          }

          if (exists) {
            if (!count[name]) {
              count[name] = 2;
            } else {
              count[name]++;
            }
            name += ' ' + count[name].toString();
          }

          ddf.id = ncc.runningGuid.toString();
          ddf.name = name;
          availableParents.push(ddf);
        }
      }
    }
    return availableParents;
  }

  public static clone(input: any, instance: NonCustodialParentsReferralSection) {
    instance.isSubmittedViaDriverFlow = input.isSubmittedViaDriverFlow;
    instance.id = input.id;
    instance.hasChildrenId = input.hasChildrenId;
    instance.hasChildrenName = input.hasChildrenName;
    instance.parents = Utilities.deserilizeChildren(input.parents, NonCustodialOtherParent);
    instance.deletedParents = [];
    instance.cwwChildren = Utilities.deserilizeChildren(input.cwwChildren, CwwChild, 0);
    instance.rowVersion = input.rowVersion;
    instance.assessmentRowVersion = input.assessmentRowVersion;
    instance.modifiedBy = input.modifiedBy;
    instance.modifiedDate = input.modifiedDate;
    instance.notes = input.notes;
  }

  public deserialize(input: any): NonCustodialParentsReferralSection {
    // if (input.cwwChildren != null) {
    //     this.cwwChildren = [];
    //     for (const cwwChild of input.cwwChildren) {
    //         this.cwwChildren.push(new CwwChild().deserialize(cwwChild));
    //     }
    // }

    NonCustodialParentsReferralSection.clone(input, this);
    return this;
  }

  public isOtherParentRequired(polarSkipDrop: DropDownField[]) {
    const yesId = Utilities.idByFieldDataName('Yes', polarSkipDrop);
    const noId = Utilities.idByFieldDataName('No', polarSkipDrop);
    if (Number(this.hasChildrenId) === yesId) {
      return true;
    } else {
      return false;
    }
  }

  public moveNonCustodialChild(nonCustodialChild: NonCustodialReferralChild, childGuid: string) {
    if (this.parents != null) {
      for (const p of this.parents) {
        // Remove nonCustodialChild.
        const index = p.children.indexOf(nonCustodialChild);
        if (index > -1) {
          p.children.splice(index, 1);
        }
        // Add Blank nonCustodialChild if none.
        if (p.children != null && p.children.length === 0) {
          const x = new NonCustodialReferralChild();
          p.children.push(x);
        }
      }

      // Add nonCustodialChild.
      for (const p of this.parents) {
        if (p.runningGuid.toString() === childGuid) {
          p.children.push(nonCustodialChild);
        }
      }
    }
  }

  // NonCustodialParentsReferralSection validate.
  public validate(validationManager: ValidationManager, polarSkipDrop: DropDownField[]): ValidationResult {
    const result = new ValidationResult();

    Utilities.validateDropDown(
      this.hasChildrenId,
      'hasChildrenId',
      "Do you have any children age 18 or under whose other parent isn't living with you?",
      result,
      validationManager
    );

    if (this.isOtherParentRequired(polarSkipDrop) && this.parents != null) {
      const errArr = result.createErrorsArray('parents');

      let allParentsAreEmpty = true;

      // Check to see if all are empty.
      for (const p of this.parents) {
        if (!p.isEmpty()) {
          allParentsAreEmpty = false;
          break;
        }
      }

      if (allParentsAreEmpty) {
        // If all are empty validate the first one.
        if (this.parents[0] != null) {
          const v = this.parents[0].validate(validationManager);
          errArr.push(v.errors);
          if (v.isValid === false) {
            result.isValid = false;
          }
        }
      } else {
        for (const p of this.parents) {
          if (!p.isEmpty()) {
            const v = p.validate(validationManager);
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

    return result;
  }
}

export class NonCustodialOtherParent implements Serializable<NonCustodialOtherParent> {
  id: number;
  firstName: string;
  lastName: string;
  isAvailableOrWorking: boolean;
  availableOrWorkingDetails: string;
  isInterestedInWorkProgram: boolean;
  interestedInWorkProgramDetails: string;
  isContactKnownWithParent: boolean;
  contactId: number;
  children: NonCustodialReferralChild[];
  deletedChildren: NonCustodialReferralChild[];
  rowVersion: string;
  assessmentRowVersion: string;
  modifiedBy: string;
  modifiedDate: string;

  // This guid may change as new children objects are added to NonCustodialOtherParent.
  runningGuid: Guid;

  public static create(): NonCustodialOtherParent {
    const otherParent = new NonCustodialOtherParent();
    otherParent.id = 0;
    otherParent.children = [];
    const c = new NonCustodialReferralChild();
    otherParent.children.push(c);
    return otherParent;
  }

  private static graphObj(input: any, instance: NonCustodialOtherParent) {
    instance.id = input.id;
    instance.firstName = input.firstName;
    instance.lastName = input.lastName;
    instance.isAvailableOrWorking = input.isAvailableOrWorking;
    instance.availableOrWorkingDetails = input.availableOrWorkingDetails;
    instance.isInterestedInWorkProgram = input.isInterestedInWorkProgram;
    instance.interestedInWorkProgramDetails = input.interestedInWorkProgramDetails;
    instance.isContactKnownWithParent = input.isContactKnownWithParent;
    instance.contactId = input.contactId;
    instance.children = Utilities.deserilizeChildren(input.children, NonCustodialReferralChild);
    instance.deletedChildren = [];
    instance.rowVersion = input.rowVersion;
    instance.assessmentRowVersion = input.assessmentRowVersion;
    instance.modifiedBy = input.modifiedBy;
    instance.modifiedDate = input.modifiedDate;
  }
  public deserialize(input: any) {
    NonCustodialOtherParent.graphObj(input, this);
    return this;
  }

  public clear(): void {
    this.firstName = null;
    this.lastName = null;
    this.isAvailableOrWorking = null;
    this.availableOrWorkingDetails = null;
    this.isInterestedInWorkProgram = null;
    this.interestedInWorkProgramDetails = null;
    this.isContactKnownWithParent = null;
    this.contactId = null;

    // Do not null if you want to keep original values cached.
    if (this.children != null) {
      for (const child of this.children) {
        child.clear();
      }
    }
  }

  public isNonCustodialChildrenEmpty(): boolean {
    let isEmpty = true;
    if (this.children != null) {
      for (const child of this.children) {
        if (child.isEmpty() === false) {
          isEmpty = false;
        }
      }
    }
    return isEmpty;
  }

  // NonCustodialOtherParent validate.
  public validate(validationManager: ValidationManager, result?: ValidationResult): ValidationResult {
    if (result == null) {
      result = new ValidationResult();
    }

    Utilities.validateRequiredText(this.firstName, 'firstName', 'First Name', result, validationManager);

    if (this.children != null) {
      const errArr = result.createErrorsArray('children');

      let allChildrenAreEmpty = true;

      // Check to see if all are empty.
      for (const child of this.children) {
        if (!child.isEmpty()) {
          allChildrenAreEmpty = false;
          break;
        }
      }

      if (allChildrenAreEmpty) {
        // If all are empty validate the first one.
        if (this.children[0] != null) {
          const v = this.children[0].validate(validationManager);
          errArr.push(v.errors);
          if (v.isValid === false) {
            result.isValid = false;
          }
        }
      } else {
        for (const ncc of this.children) {
          if (!ncc.isEmpty()) {
            const v = ncc.validate(validationManager);
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

    return result;
  }

  public isEmpty(): boolean {
    let isEmpty = false;
    // All properties have to be null or blank to make the entire object empty.
    isEmpty =
      (this.firstName == null || this.firstName.trim() === '') &&
      (this.lastName == null || this.lastName.trim() === '') &&
      (this.isAvailableOrWorking == null || this.isAvailableOrWorking.toString() === '') &&
      (this.availableOrWorkingDetails == null || this.availableOrWorkingDetails.trim() === '') &&
      (this.isInterestedInWorkProgram == null || this.isInterestedInWorkProgram.toString() === '') &&
      (this.interestedInWorkProgramDetails == null || this.interestedInWorkProgramDetails.trim() === '') &&
      (this.isContactKnownWithParent == null || this.isContactKnownWithParent.toString() === '') &&
      (this.contactId == null || this.contactId.toString() === '');
    // Is not empty if child has data.
    if (isEmpty === true) {
      isEmpty = this.isNonCustodialChildrenEmpty();
    }

    return isEmpty;
  }
}

export class NonCustodialReferralChild implements Serializable<NonCustodialReferralChild> {
  id: number;
  firstName: string;
  lastName: string;
  hasChildSupportOrder: boolean;
  childSupportOrderDetails: string;
  contactIntervalId: number;
  contactIntervalName: string;
  contactIntervalDetails: string;
  rowVersion: string;
  assessmentRowVersion: string;
  modifiedBy: string;
  modifiedDate: string;

  public static create(): NonCustodialReferralChild {
    const c = new NonCustodialReferralChild();
    c.id = 0;
    return c;
  }

  private static graphObj(input: any, instance: NonCustodialReferralChild) {
    instance.id = input.id;
    instance.firstName = input.firstName;
    instance.lastName = input.lastName;
    instance.hasChildSupportOrder = input.hasChildSupportOrder;
    instance.childSupportOrderDetails = input.childSupportOrderDetails;
    instance.contactIntervalId = input.contactIntervalId;
    instance.contactIntervalName = input.contactIntervalName;
    instance.contactIntervalDetails = input.contactIntervalDetails;
    instance.rowVersion = input.rowVersion;
    instance.assessmentRowVersion = input.assessmentRowVersion;
    instance.modifiedBy = input.modifiedBy;
    instance.modifiedDate = input.modifiedDate;
  }

  public deserialize(input: any) {
    NonCustodialReferralChild.graphObj(input, this);
    return this;
  }

  public clear(): void {
    this.firstName = null;
    this.lastName = null;
    this.hasChildSupportOrder = null;
    this.childSupportOrderDetails = null;
    this.contactIntervalId = null;
    this.contactIntervalDetails = null;
  }

  /**
   * Validates Child.
   *
   * @param {ValidationManager} validationManager
   * @param {ValidationResult} [result]
   * @returns {ValidationResult}
   * @memberof NonCustodialReferralChild
   */
  public validate(validationManager: ValidationManager, result?: ValidationResult): ValidationResult {
    if (result == null) {
      result = new ValidationResult();
    }

    Utilities.validateRequiredText(this.firstName, 'firstName', 'First Name', result, validationManager);

    return result;
  }

  public isOtherParentDeceased(ncprContactIntervals: DropDownField[], value?: string): boolean {
    const deceasedId = Utilities.idByFieldDataName('Other parent is deceased', ncprContactIntervals, true);
    if (+this.contactIntervalId === deceasedId || ((ncprContactIntervals === [] || ncprContactIntervals.length === 0) && value === 'Other parent is deceased')) {
      return true;
    } else {
      return false;
    }
  }

  public isEmpty(): boolean {
    // All properties have to be null or blank to make the entire object empty.
    // NOTE: This empty check also includes the ID.  If the Id has been set,
    // then this means it must be deleted to be cleared out.
    return (
      (this.id == null || this.id === 0) &&
      (this.firstName == null || this.firstName.trim() === '') &&
      (this.lastName == null || this.lastName.trim() === '') &&
      (this.hasChildSupportOrder == null || this.hasChildSupportOrder.toString() === '') &&
      (this.childSupportOrderDetails == null || this.childSupportOrderDetails.trim() === '') &&
      (this.contactIntervalId == null || this.contactIntervalId.toString() === '') &&
      (this.contactIntervalDetails == null || this.contactIntervalDetails.trim() === '')
    );
  }
}
