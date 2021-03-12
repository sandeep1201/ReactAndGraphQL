// tslint:disable: no-use-before-declare
import { Serializable } from '../interfaces/serializable';
import { Utilities } from '../utilities';
import { ActionNeeded } from '../../features-modules/actions-needed/models/action-needed-new';
import { ValidationManager } from './validation-manager';
import { ValidationResult } from './validation-result';
import { DropDownField } from '../../shared/models/dropdown-field';
import { MmDdYyyyValidationContext } from '../interfaces/mmDdYyyy-validation-context';
import { Guid } from '../models/guid';
import { AppService } from 'src/app/core/services/app.service';
export class NonCustodialParentsSection implements Serializable<NonCustodialParentsSection> {
  isSubmittedViaDriverFlow: boolean;
  hasChildren: boolean;
  nonCustodialCaretakers: NonCustodialCaretaker[];
  deletedNonCustodialCaretakers: NonCustodialCaretaker[];
  childSupportPayment: string;
  hasOwedChildSupport: boolean;
  hasInterestInChildServices: boolean;
  isInterestedInReferralServices: boolean;
  interestedInReferralServicesDetails: string;
  actionNeeded: ActionNeeded;
  notes: string;
  rowVersion: string;
  assessmentRowVersion: string;
  modifiedBy: string;
  modifiedDate: string;
  childSupportContactId: number;

  public static clone(input: any, instance: NonCustodialParentsSection) {
    instance.isSubmittedViaDriverFlow = input.isSubmittedViaDriverFlow;
    instance.hasChildren = input.hasChildren;
    instance.nonCustodialCaretakers = Utilities.deserilizeChildren(input.nonCustodialCaretakers, NonCustodialCaretaker);
    instance.deletedNonCustodialCaretakers = [];
    instance.childSupportPayment = input.childSupportPayment;
    instance.hasOwedChildSupport = input.hasOwedChildSupport;
    instance.hasInterestInChildServices = input.hasInterestInChildServices;
    instance.isInterestedInReferralServices = input.isInterestedInReferralServices;
    instance.interestedInReferralServicesDetails = input.interestedInReferralServicesDetails;
    instance.actionNeeded = Utilities.deserilizeChild(input.actionNeeded, ActionNeeded);
    instance.notes = input.notes;
    instance.assessmentRowVersion = input.assessmentRowVersion;
    instance.rowVersion = input.rowVersion;
    instance.modifiedBy = input.modifiedBy;
    instance.modifiedDate = input.modifiedDate;
    instance.childSupportContactId = input.childSupportContactId;
  }

  public deserialize(input: any) {
    NonCustodialParentsSection.clone(input, this);
    return this;
  }

  public moveNonCustodialChild(nonCustodialChild: NonCustodialChild, childGuid: string) {
    if (this.nonCustodialCaretakers != null) {
      for (const nnc of this.nonCustodialCaretakers) {
        // Remove nonCustodialChild.
        const index = nnc.nonCustodialChilds.indexOf(nonCustodialChild);
        if (index > -1) {
          nnc.nonCustodialChilds.splice(index, 1);
        }
        // Add Blank nonCustodialChild if none.
        if (nnc.nonCustodialChilds != null && nnc.nonCustodialChilds.length === 0) {
          const x = new NonCustodialChild();
          x.id = 0;
          nnc.nonCustodialChilds.push(x);
        }
      }

      // Add nonCustodialChild.
      for (const ncc of this.nonCustodialCaretakers) {
        if (ncc.runningGuid.toString() === childGuid) {
          if (ncc.nonCustodialChilds != null) {
            ncc.nonCustodialChilds.push(nonCustodialChild);
          }
        }
      }
    }
  }

  hasChildSupport(): boolean {
    let hasChildSupport = false;
    if (this.nonCustodialCaretakers != null) {
      for (const ncc of this.nonCustodialCaretakers) {
        if (ncc.nonCustodialChilds != null && hasChildSupport === false) {
          for (const child of ncc.nonCustodialChilds) {
            if (child.hasChildSupportOrder === true) {
              hasChildSupport = true;
              break;
            }
          }
        }
      }
    }
    return hasChildSupport;
  }

  get availableCaretackers(): DropDownField[] {
    const availableCaretackers: DropDownField[] = [];
    if (this.nonCustodialCaretakers != null) {
      const count = {};
      for (const ncc of this.nonCustodialCaretakers) {
        if (
          (ncc.firstName != null && ncc.firstName.trim() !== '') ||
          (ncc.isFirstNameUnknown != null && ncc.isFirstNameUnknown.toString() !== '' && ncc.lastName != null && ncc.lastName.trim() !== '') ||
          (ncc.isLastNameUnknown != null && ncc.isLastNameUnknown.toString() !== '')
        ) {
          const ddf = new DropDownField();
          if (ncc.runningGuid == null) {
            ncc.runningGuid = new Guid();
          }

          let firstName = '';
          let lastName = '';

          if (ncc.firstName != null && ncc.isFirstNameUnknown !== true) {
            firstName = ncc.firstName;
          }

          if (ncc.lastName != null && ncc.isLastNameUnknown !== true) {
            lastName = ncc.lastName;
          }

          if (ncc.isFirstNameUnknown === true) {
            firstName = 'Unknown';
          }

          if (ncc.isLastNameUnknown === true) {
            lastName = 'Unknown';
          }

          let name = Utilities.toTitleCase(firstName.toLowerCase()) + ' ' + Utilities.toTitleCase(lastName.toLowerCase());

          let exists = false;
          for (const act of availableCaretackers) {
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
          availableCaretackers.push(ddf);
        }
      }
    }
    return availableCaretackers;
  }

  // Validate Section.
  public validate(
    validationManager: ValidationManager,
    polarDrop: DropDownField[],
    relationshipsDrop: DropDownField[],
    contactIntervalDrop: DropDownField[],
    participantDob: string
  ): ValidationResult {
    const result = new ValidationResult();

    Utilities.validateRequiredYesNo(
      result,
      validationManager,
      this.hasChildren,
      'hasChildren',
      'Do you have any children under the age of 18 who live with another individual most of the time?'
    );

    if (this.hasChildren === true && this.nonCustodialCaretakers != null) {
      const errArr = result.createErrorsArray('nonCustodialCaretakers');

      let allNonCustodialCaretakersAreEmpty = true;

      // Check to see if all are empty.
      for (const c of this.nonCustodialCaretakers) {
        if (!c.isEmpty()) {
          allNonCustodialCaretakersAreEmpty = false;
          break;
        }
      }

      if (allNonCustodialCaretakersAreEmpty) {
        // If all are empty validate the first one.
        if (this.nonCustodialCaretakers[0] != null) {
          const v = this.nonCustodialCaretakers[0].validate(validationManager, polarDrop, relationshipsDrop, contactIntervalDrop);
          errArr.push(v.errors);
          if (v.isValid === false) {
            result.isValid = false;
          }
        }
      } else {
        for (const ncc of this.nonCustodialCaretakers) {
          if (!ncc.isEmpty()) {
            const v = ncc.validate(validationManager, polarDrop, relationshipsDrop, contactIntervalDrop);
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

    if (this.hasChildSupport() === true && this.hasChildren) {
      Utilities.validateRequiredCurrency(result, validationManager, this.childSupportPayment, 'childSupportPayment', 'How much child support are you ordered to pay each month?');
      Utilities.validateRequiredYesNo(result, validationManager, this.hasOwedChildSupport, 'hasOwedChildSupport', 'Do you owe any back child support?');
      Utilities.validateRequiredYesNo(
        result,
        validationManager,
        this.hasInterestInChildServices,
        'hasInterestInChildServices',
        'Are you interested in getting a referral for services to help you understand your child support obligations?'
      );
    }

    if (this.hasChildren) {
      Utilities.validateRequiredYesNo(
        result,
        validationManager,
        this.isInterestedInReferralServices,
        'isInterestedInReferralServices',
        'Are you interested in getting a referral for services to help with parenting or visitation?'
      );
      const anResult = this.actionNeeded.validate(validationManager);

      // TODO: wire up anResult better.
      if (anResult.isValid === false) {
        result.addError('actionNeeded');
      }
    }
    return result;
  }
}

export class NonCustodialCaretaker implements Serializable<NonCustodialCaretaker> {
  id: number;
  firstName: string;
  isFirstNameUnknown: boolean;
  lastName: string;
  isLastNameUnknown: boolean;
  nonCustodialParentRelationshipId: number;
  nonCustodialParentRelationshipName: string;
  relationshipDetails: string;
  contactIntervalId: number;
  contactIntervalName: string;
  contactIntervalDetails: string;
  isRelationshipChangeRequested: boolean;
  relationshipChangeRequestedDetails: string;
  isInterestedInRelationshipReferral: boolean;
  interestedInRelationshipReferralDetails: string;
  nonCustodialChilds: NonCustodialChild[];
  deletedNonCustodialChilds: NonCustodialChild[];
  // cached: NonCustodialCaretaker;
  // This guid may change as new children objects are added to NonCustodialOtherParent.
  runningGuid: Guid;

  private static graphObj(input: any, instance: NonCustodialCaretaker) {
    instance.id = input.id;
    instance.firstName = input.firstName;
    instance.isFirstNameUnknown = input.isFirstNameUnknown;
    instance.lastName = input.lastName;
    instance.isLastNameUnknown = input.isLastNameUnknown;
    instance.nonCustodialParentRelationshipId = input.nonCustodialParentRelationshipId;
    instance.nonCustodialParentRelationshipName = input.nonCustodialParentRelationshipName;
    instance.relationshipDetails = input.relationshipDetails;
    instance.contactIntervalId = input.contactIntervalId;
    instance.contactIntervalName = input.contactIntervalName;
    instance.contactIntervalDetails = input.contactIntervalDetails;
    instance.isRelationshipChangeRequested = input.isRelationshipChangeRequested;
    instance.relationshipChangeRequestedDetails = input.relationshipChangeRequestedDetails;
    instance.isInterestedInRelationshipReferral = input.isInterestedInRelationshipReferral;
    instance.interestedInRelationshipReferralDetails = input.interestedInRelationshipReferralDetails;
    instance.nonCustodialChilds = Utilities.deserilizeChildren(input.nonCustodialChilds, NonCustodialChild);
    instance.deletedNonCustodialChilds = [];
  }

  public static create(): NonCustodialCaretaker {
    const caretaker = new NonCustodialCaretaker();
    caretaker.id = 0;
    caretaker.nonCustodialChilds = [];
    const ncc = new NonCustodialChild();
    caretaker.nonCustodialChilds.push(ncc);
    return caretaker;
  }

  public clear(): void {
    this.firstName = null;
    this.isFirstNameUnknown = null;
    this.lastName = null;
    this.isLastNameUnknown = null;
    this.nonCustodialParentRelationshipId = null;
    this.relationshipDetails = null;
    this.contactIntervalId = null;
    this.contactIntervalDetails = null;
    this.isRelationshipChangeRequested = null;
    this.relationshipChangeRequestedDetails = null;
    this.isInterestedInRelationshipReferral = null;

    // Do not null if you want to keep original values cached.
    if (this.nonCustodialChilds != null) {
      for (const ncc of this.nonCustodialChilds) {
        ncc.clear();
      }
    }
  }

  public isEmpty(): boolean {
    // All properties have to be null or blank to make the entire object empty.
    let isEmpty =
      (this.firstName == null || this.firstName.trim() === '') &&
      (this.isFirstNameUnknown == null || this.isFirstNameUnknown === false || this.isFirstNameUnknown.toString() === '') &&
      (this.lastName == null || this.lastName.trim() === '') &&
      (this.isLastNameUnknown == null || this.isLastNameUnknown === false || this.isLastNameUnknown.toString() === '') &&
      (this.nonCustodialParentRelationshipId == null || this.nonCustodialParentRelationshipId.toString() === '') &&
      (this.relationshipDetails == null || this.relationshipDetails.trim() === '') &&
      (this.contactIntervalId == null || this.contactIntervalId.toString() === '') &&
      (this.contactIntervalDetails == null || this.contactIntervalDetails.trim() === '') &&
      (this.isRelationshipChangeRequested == null || this.isRelationshipChangeRequested.toString() === '') &&
      (this.relationshipChangeRequestedDetails == null || this.relationshipChangeRequestedDetails.trim() === '') &&
      (this.isInterestedInRelationshipReferral == null || this.isInterestedInRelationshipReferral.toString() === '');

    // Is not empty if child has data.
    if (isEmpty === true) {
      isEmpty = this.isNonCustodialChildrenEmpty();
    }

    return isEmpty;
  }
  public deserialize(input: any) {
    NonCustodialCaretaker.graphObj(input, this);
    return this;
  }

  public isRelationshipDetailsRequired(relationshipsDrop: DropDownField[]) {
    const otherRelationshipId = Utilities.idByFieldDataName('Other', relationshipsDrop);
    if (Number(this.nonCustodialParentRelationshipId) === otherRelationshipId) {
      return true;
    } else {
      return false;
    }
  }

  public isContactIntervalDetailsRequired(contactIntervalDrop: DropDownField[]) {
    const noContactId = Utilities.idByFieldDataName('No Contact', contactIntervalDrop);
    if (Number(this.contactIntervalId) === noContactId) {
      return true;
    } else {
      return false;
    }
  }

  public isContactIntervalIdHidden(relationshipsDrop: DropDownField[], value?: string) {
    const unknownRelationshipId = Utilities.idByFieldDataName('Unknown', relationshipsDrop, true);
    if (this.isFirstNameUnknown === true && this.isLastNameUnknown === true && Number(this.nonCustodialParentRelationshipId) === unknownRelationshipId) {
      return true;
    } else if (this.isFirstNameUnknown === true && this.isLastNameUnknown === true && (relationshipsDrop === [] || relationshipsDrop.length === 0) && value === 'Unknown') {
      return true;
    } else {
      return false;
    }
  }

  public isNonCustodialChildrenEmpty(): boolean {
    let isEmpty = true;
    if (this.nonCustodialChilds != null) {
      for (const ncc of this.nonCustodialChilds) {
        if (ncc.isEmpty() === false) {
          isEmpty = false;
        }
      }
    }
    return isEmpty;
  }

  // Caretaker.
  public validate(
    validationManager: ValidationManager,
    polarDrop: DropDownField[],
    relationshipsDrop: DropDownField[],
    contactIntervalDrop: DropDownField[],
    result?: ValidationResult
  ): ValidationResult {
    if (result == null) {
      result = new ValidationResult();
    }

    if (this.isFirstNameUnknown !== true) {
      Utilities.validateRequiredText(this.firstName, 'firstName', 'First Name', result, validationManager);
    }

    if (this.isLastNameUnknown !== true) {
      Utilities.validateRequiredText(this.lastName, 'lastName', 'Last Name', result, validationManager);
    }

    Utilities.validateDropDown(this.nonCustodialParentRelationshipId, 'nonCustodialParentRelationshipId', 'Relationship to Child(ren)', result, validationManager);
    if (this.isRelationshipDetailsRequired(relationshipsDrop)) {
      Utilities.validateRequiredText(this.relationshipDetails, 'relationshipDetails', 'Relationship to Child(ren) - Details', result, validationManager);
    }

    if (!this.isContactIntervalIdHidden(relationshipsDrop)) {
      Utilities.validateDropDown(this.contactIntervalId, 'contactIntervalId', 'How often do you have contact with this person?', result, validationManager);
      if (this.isContactIntervalDetailsRequired(contactIntervalDrop)) {
        Utilities.validateRequiredText(
          this.contactIntervalDetails,
          'contactIntervalDetails',
          'How often do you have contact with this person? - Details',
          result,
          validationManager
        );
      }
    }

    if (!this.isContactIntervalIdHidden(relationshipsDrop)) {
      Utilities.validateRequiredYesNo(
        result,
        validationManager,
        this.isRelationshipChangeRequested,
        'isRelationshipChangeRequested',
        'Is there anything you would like to change about your relationship with this person?'
      );
      if (this.isRelationshipChangeRequested) {
        Utilities.validateRequiredText(
          this.relationshipChangeRequestedDetails,
          'relationshipChangeRequestedDetails',
          'Is there anything you would like to change about your relationship with this person? - Details',
          result,
          validationManager
        );
      }
    }

    if (!this.isContactIntervalIdHidden(relationshipsDrop)) {
      if (this.isInterestedInRelationshipReferral) {
        Utilities.validateRequiredText(
          this.interestedInRelationshipReferralDetails,
          'interestedInRelationshipReferralDetails',
          'Would you be interested in getting a referral to help improve your relationship with this person, if this person is willing? - Details',
          result,
          validationManager
        );
      }
    }

    if (this.nonCustodialChilds != null) {
      const errArr = result.createErrorsArray('nonCustodialChilds');

      let allNonCustodialChildrenAreEmpty = true;

      // Check to see if all are empty.
      for (const child of this.nonCustodialChilds) {
        if (!child.isEmpty()) {
          allNonCustodialChildrenAreEmpty = false;
          break;
        }
      }

      if (allNonCustodialChildrenAreEmpty) {
        // If all are empty validate the first one.
        if (this.nonCustodialChilds[0] != null) {
          errArr.push(this.nonCustodialChilds[0].validate(validationManager, polarDrop, contactIntervalDrop).errors);
        }
      } else {
        for (const ncc of this.nonCustodialChilds) {
          if (!ncc.isEmpty()) {
            const v = ncc.validate(validationManager, polarDrop, contactIntervalDrop);
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

export class NonCustodialChild implements Serializable<NonCustodialChild> {
  id: number;
  firstName: string;
  lastName: string;
  dateOfBirth: string;
  hasChildSupportOrder: boolean;
  hasNameOnChildBirthRecord: boolean;
  childSupportOrderDetails: string;
  contactIntervalId: number;
  contactIntervalName: string;
  contactIntervalDetails: string;
  hasOtherAdultsPolarLookupId: number;
  hasOtherAdultsPolarLookupName: string;
  otherAdultsDetails: string;
  isRelationshipChangeRequested: boolean;
  relationshipChangeRequestedDetails: string;
  isNeedOfServicesPolarLookupId: number;
  isNeedOfServicesPolarLookupName: string;
  needOfServicesDetails: string;
  // cached: NonCustodialChild;

  private static graphObj(input: any, instance: NonCustodialChild) {
    instance.id = input.id;
    instance.firstName = input.firstName;
    instance.lastName = input.lastName;
    instance.dateOfBirth = input.dateOfBirth;
    instance.hasChildSupportOrder = input.hasChildSupportOrder;
    instance.childSupportOrderDetails = input.childSupportOrderDetails;
    instance.hasNameOnChildBirthRecord = input.hasNameOnChildBirthRecord;
    instance.contactIntervalId = input.contactIntervalId;
    instance.contactIntervalName = input.contactIntervalName;
    instance.contactIntervalDetails = input.contactIntervalDetails;
    instance.hasOtherAdultsPolarLookupId = input.hasOtherAdultsPolarLookupId;
    instance.hasOtherAdultsPolarLookupName = input.hasOtherAdultsPolarLookupName;
    instance.otherAdultsDetails = input.otherAdultsDetails;
    instance.isRelationshipChangeRequested = input.isRelationshipChangeRequested;
    instance.relationshipChangeRequestedDetails = input.relationshipChangeRequestedDetails;
    instance.isNeedOfServicesPolarLookupId = input.isNeedOfServicesPolarLookupId;
    instance.isNeedOfServicesPolarLookupName = input.isNeedOfServicesPolarLookupName;
    instance.needOfServicesDetails = input.needOfServicesDetails;
  }

  public static create(): NonCustodialChild {
    const child = new NonCustodialChild();
    child.id = 0;
    return child;
  }

  public clear(): void {
    this.firstName = null;
    this.lastName = null;
    this.dateOfBirth = null;
    this.hasChildSupportOrder = null;
    this.hasNameOnChildBirthRecord = null;
    this.childSupportOrderDetails = null;
    this.contactIntervalId = null;
    this.contactIntervalDetails = null;
    this.hasOtherAdultsPolarLookupId = null;
    this.otherAdultsDetails = null;
    this.isRelationshipChangeRequested = null;
    this.relationshipChangeRequestedDetails = null;
    this.isNeedOfServicesPolarLookupId = null;
    this.needOfServicesDetails = null;
  }

  public isEmpty(): boolean {
    // All properties have to be null or blank to make the entire object empty.
    return (
      (this.firstName == null || this.firstName.trim() === '') &&
      (this.lastName == null || this.lastName.trim() === '') &&
      (this.dateOfBirth == null || this.dateOfBirth.trim() === '') &&
      (this.hasChildSupportOrder == null || this.hasChildSupportOrder.toString() === '') &&
      (this.childSupportOrderDetails == null || this.childSupportOrderDetails.trim() === '') &&
      (this.hasNameOnChildBirthRecord == null || this.hasNameOnChildBirthRecord.toString() === '') &&
      (this.contactIntervalId == null || this.contactIntervalId.toString() === '') &&
      (this.contactIntervalDetails == null || this.contactIntervalDetails.trim() === '') &&
      (this.hasOtherAdultsPolarLookupId == null || this.hasOtherAdultsPolarLookupId.toString() === '') &&
      (this.otherAdultsDetails == null || this.otherAdultsDetails.trim() === '') &&
      (this.isRelationshipChangeRequested == null || this.isRelationshipChangeRequested.toString() === '') &&
      (this.relationshipChangeRequestedDetails == null || this.relationshipChangeRequestedDetails.trim() === '') &&
      (this.isNeedOfServicesPolarLookupId == null || this.isNeedOfServicesPolarLookupId.toString() === '') &&
      (this.needOfServicesDetails == null || this.needOfServicesDetails.trim() === '')
    );
  }

  public deserialize(input: any) {
    NonCustodialChild.graphObj(input, this);
    // this.cached = new NonCustodialChild();
    // NonCustodialChild.graphObj(input, this.cached);
    return this;
  }

  public isOtherAdultsDetailsRequired(polarDrop: DropDownField[]): boolean {
    const yesId = Utilities.idByFieldDataName('Yes', polarDrop);
    if (this.hasOtherAdultsPolarLookupId != null && Number(this.hasOtherAdultsPolarLookupId) === yesId) {
      return true;
    } else {
      return false;
    }
  }

  public isRelationshipChangeRequestedDetailsRequired(): boolean {
    if (this.isRelationshipChangeRequested === true) {
      return true;
    } else {
      return false;
    }
  }

  public isNeedOfServicesDetailsRequired(polarDrop: DropDownField[]): boolean {
    const yesId = Utilities.idByFieldDataName('Yes', polarDrop);
    if (this.isNeedOfServicesPolarLookupId != null && Number(this.isNeedOfServicesPolarLookupId) === yesId) {
      return true;
    } else {
      return false;
    }
  }

  public validate(validationManager: ValidationManager, polarDrop: DropDownField[], contactIntervalDrop: DropDownField[], result?: ValidationResult): ValidationResult {
    if (result == null) {
      result = new ValidationResult();
    }

    Utilities.validateRequiredText(this.firstName, 'firstName', 'First Name', result, validationManager);
    Utilities.validateRequiredText(this.lastName, 'lastName', 'Last Name', result, validationManager);

    const minDate = Utilities.currentDate
      .subtract(19, 'years')
      .add(1, 'day')
      .format('MM/DD/YYYY');
    const dobValidationContext: MmDdYyyyValidationContext = {
      date: this.dateOfBirth,
      prop: 'dateOfBirth',
      prettyProp: 'DOB',
      result: result,
      validationManager: validationManager,
      isRequired: true,
      minDate: minDate,
      minDateAllowSame: false,
      minDateName: 'date of ' + minDate,
      maxDate: Utilities.currentDate.format('MM/DD/YYYY'),
      maxDateAllowSame: true,
      maxDateName: 'Current Date',
      participantDOB: null
    };
    Utilities.validateMmDdYyyyDate(dobValidationContext);

    Utilities.validateRequiredYesNo(result, validationManager, this.hasChildSupportOrder, 'hasChildSupportOrder', 'Child support order?');

    if (this.hasChildSupportOrder != null && this.hasChildSupportOrder === false) {
      Utilities.validateRequiredYesNo(result, validationManager, this.hasNameOnChildBirthRecord, 'hasNameOnChildBirthRecord', 'Is your name on birth record?');
    }

    Utilities.validateDropDown(this.contactIntervalId, 'contactIntervalId', 'How often do you have contact with this child?', result, validationManager);
    Utilities.validateRequiredText(this.contactIntervalDetails, 'contactIntervalDetails', 'How often do you have contact with this child? - Details', result, validationManager);
    Utilities.validateDropDown(this.hasOtherAdultsPolarLookupId, 'hasOtherAdultsPolarLookupId', 'Are there other adults caring for this child?', result, validationManager);

    if (this.isOtherAdultsDetailsRequired(polarDrop)) {
      Utilities.validateRequiredText(this.otherAdultsDetails, 'otherAdultsDetails', 'Are there other adults caring for this child? - Details', result, validationManager);
    }

    Utilities.validateRequiredYesNo(
      result,
      validationManager,
      this.isRelationshipChangeRequested,
      'isRelationshipChangeRequested',
      'Is there anything you would like to change about your relationship with this child?'
    );

    if (this.isRelationshipChangeRequestedDetailsRequired()) {
      Utilities.validateRequiredText(
        this.relationshipChangeRequestedDetails,
        'relationshipChangeRequestedDetails',
        'Is there anything you would like to change about your relationship with this child? - Details',
        result,
        validationManager
      );
    }

    Utilities.validateDropDown(this.isNeedOfServicesPolarLookupId, 'isNeedOfServicesPolarLookupId', 'Is the child in need of any services?', result, validationManager);

    if (this.isNeedOfServicesDetailsRequired(polarDrop)) {
      Utilities.validateRequiredText(this.needOfServicesDetails, 'needOfServicesDetails', 'Is the child in need of any services? - Details', result, validationManager);
    }
    return result;
  }
}
