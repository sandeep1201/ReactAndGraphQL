import * as moment from 'moment';
import { ActionNeeded } from '../../features-modules/actions-needed/models/action-needed-new';
import { Clearable } from '../interfaces/clearable';
import { IsEmpty } from '../interfaces/is-empty';
import { LogService } from '../services/log.service';
import { Participant } from './participant';
import { Serializable } from '../interfaces/serializable';
import { Utilities } from '../utilities';
import { Validate } from './events/validate';
import { ValidationManager } from './validation-manager';
import { ValidationCode } from './validation-error';
import { ValidationResult } from './validation-result';
import { AppService } from 'src/app/core/services/app.service';

export class Child implements Clearable, IsEmpty, Serializable<Child> {
  id: number;
  childId: number;
  firstName: string;
  lastName: string;
  dateOfBirth: string;
  careArrangementId: number;
  careArrangementName: string;
  details: string;
  isSpecialNeeds: boolean;

  /**
   * Creates a new Child suitable to be bound to the UI and passed to the API.
   * It will set the ID's to appropriate values.
   */
  public static create(): Child {
    const child = new Child();
    child.id = 0;
    child.childId = 0;

    return child;
  }

  public clear(): void {
    this.firstName = null;
    this.lastName = null;
    this.dateOfBirth = null;
    this.details = null;
    this.careArrangementId = null;
    this.careArrangementName = null;
    this.isSpecialNeeds = null;
  }

  public deserialize(input: any): Child {
    this.id = input.id;
    this.childId = input.childId;
    this.firstName = input.firstName;
    this.lastName = input.lastName;
    this.dateOfBirth = input.dateOfBirth;
    this.careArrangementId = input.careArrangementId;
    this.careArrangementName = input.careArrangementName;
    this.details = input.details;
    this.isSpecialNeeds = input.isSpecialNeeds;

    return this;
  }

  get ageInYears(): number {
    return Utilities.getAgeFromDateOfBirthWithFormat(this.dateOfBirth, 'MM/DD/YYYY');
  }

  /**
   * Detects whether or not a FamilyMember object is effectively empty.
   *
   * @returns {boolean}
   *
   * @memberOf FamilyMember
   */
  public isEmpty(): boolean {
    // All properties have to be null or blank to make the entire object empty.
    return (
      (this.firstName == null || this.firstName.trim() === '') &&
      (this.lastName == null || this.lastName.trim() === '') &&
      (this.dateOfBirth == null || this.dateOfBirth.toString() === '') &&
      (this.careArrangementId == null || this.careArrangementId.toString() === '' || this.careArrangementId === 0) &&
      (this.isSpecialNeeds == null || this.isSpecialNeeds.toString() === '') &&
      (this.details == null || this.details.trim() === '')
    );
  }

  /**
   * Checks the validity of the 'firstName' property.
   *
   * @returns {boolean}
   *
   * @memberOf Child
   */
  public isFirstNameValid(): boolean {
    let result = true;

    if (this.firstName == null || this.firstName.trim() === '') {
      result = false;
    }

    return result;
  }

  /**
   * Checks the validity of the 'lastName' property.
   *
   * @returns {boolean}
   *
   * @memberOf Child
   */
  public isLastNameValid(): boolean {
    let result = true;

    if (this.lastName == null || this.lastName.trim() === '') {
      result = false;
    }

    return result;
  }

  /**
   * Checks the validity of the 'dateOfBirth' property.
   *
   * @returns {boolean}
   *
   * @memberOf Child
   */
  public isDateOfBirthValid(): boolean {
    if (this.dateOfBirth == null) {
      return false;
    }

    // See if there is a valid date and be sure to use STRICT parsing
    // (using true as last parameter):
    const dob = moment(this.dateOfBirth, 'MM/DD/YYYY', true);

    if (!dob.isValid()) {
      return false;
    }

    const currentDate = Utilities.currentDate;

    // The ROI Date must not be in the future.
    return dob <= currentDate;
  }
}

export class Teen implements Clearable, IsEmpty, Serializable<Teen> {
  id: number;
  childId: number;
  firstName: string;
  lastName: string;
  dateOfBirth: string;
  details: string;

  /**
   * Creates a new Teen suitable to be bound to the UI and passed to the API.
   * It will set the ID's to appropriate values.
   */
  public static create(): Teen {
    const teen = new Teen();
    teen.id = 0;
    teen.childId = 0;

    return teen;
  }

  public clear(): void {
    this.firstName = null;
    this.lastName = null;
    this.dateOfBirth = null;
    this.details = null;
  }

  public deserialize(input: any): Teen {
    this.id = input.id;
    this.childId = input.childId;
    this.firstName = input.firstName;
    this.lastName = input.lastName;
    this.dateOfBirth = input.dateOfBirth;
    this.details = input.details;

    return this;
  }

  get ageInYears(): number {
    return Utilities.getAgeFromDateOfBirthWithFormat(this.dateOfBirth, 'MM/DD/YYYY');
  }

  /**
   * Detects whether or not a FamilyMember object is effectively empty.
   *
   * @returns {boolean}
   *
   * @memberOf FamilyMember
   */
  public isEmpty(): boolean {
    // All properties have to be null or blank to make the entire object empty.
    return (
      (this.firstName == null || this.firstName.trim() === '') &&
      (this.lastName == null || this.lastName.trim() === '') &&
      (this.dateOfBirth == null || this.dateOfBirth.toString() === '') &&
      (this.details == null || this.details.trim() === '')
    );
  }

  /**
   * Checks the validity of the 'firstName' property.
   *
   * @returns {boolean}
   *
   * @memberOf Teen
   */
  public isFirstNameValid(): boolean {
    let result = true;

    if (this.firstName == null || this.firstName.trim() === '') {
      result = false;
    }

    return result;
  }

  /**
   * Checks the validity of the 'lastName' property.
   *
   * @returns {boolean}
   *
   * @memberOf Teen
   */
  public isLastNameValid(): boolean {
    let result = true;

    if (this.lastName == null || this.lastName.trim() === '') {
      result = false;
    }

    return result;
  }

  /**
   * Checks the validity of the 'dateOfBirth' property.
   *
   * @returns {boolean}
   *
   * @memberOf Teen
   */
  public isDateOfBirthValid(): boolean {
    if (this.dateOfBirth == null) {
      return false;
    }

    // See if there is a valid date and be sure to use STRICT parsing
    // (using true as last parameter):
    const dob = moment(this.dateOfBirth, 'MM/DD/YYYY', true);

    if (!dob.isValid()) {
      return false;
    }

    const currentDate = Utilities.currentDate;

    // The ROI Date must not be in the future.
    return dob <= currentDate;
  }
}

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

export class CwwEligibility implements Serializable<CwwEligibility> {
  eligibilityStatus: string;
  reasonCode: string;
  description: string;
  reasonCode1: string;
  description1: string;
  reasonCode2: string;
  description2: string;

  public deserialize(input: any): CwwEligibility {
    this.eligibilityStatus = input.eligibilityStatus;
    this.reasonCode = input.reasonCode;
    this.description = input.description;
    this.reasonCode1 = input.reasonCode1;
    this.description1 = input.description1;
    this.reasonCode2 = input.reasonCode2;
    this.description2 = input.description2;

    return this;
  }

  get formattedReason(): string {
    if (this.reasonCode == null && this.description == null) {
      return '';
    }

    if (this.reasonCode == null || this.reasonCode.trim() === '') {
      return this.description.trim();
    }

    return '(' + this.reasonCode.trim() + ') ' + this.description.trim();
  }

  get formattedReason1(): string {
    if (this.reasonCode1 == null && this.description1 == null) {
      return '';
    }

    if (this.reasonCode1 == null || this.reasonCode1.trim() === '') {
      return this.description1.trim();
    }

    return '(' + this.reasonCode1.trim() + ') ' + this.description1.trim();
  }

  get formattedReason2(): string {
    if (this.reasonCode2 == null && this.description2 == null) {
      return '';
    }

    if (this.reasonCode2 == null || this.reasonCode2.trim() === '') {
      return this.description2.trim();
    }

    return '(' + this.reasonCode2.trim() + ') ' + this.description2.trim();
  }
}

/**
 * The ChildAndYouthSupportsSection object used by the Informal Assessment.
 *
 * @export
 * @class ChildAndYouthSupportsSection
 */
export class ChildAndYouthSupportsSection implements Serializable<ChildAndYouthSupportsSection> {
  isSubmittedViaDriverFlow: boolean;
  cwwChildren: CwwChild[];
  cwwEligibility: CwwEligibility;
  hasChildren: boolean;
  hasTeensWithDisabilityInNeedOfChildCare: boolean;
  children: Child[];
  teens: Teen[];
  deletedChildren: Child[];
  deletedTeens: Teen[];
  hasWicBenefits: boolean;
  isInHeadStart: boolean;
  isInAfterSchoolOrSummerProgram: boolean;
  afterSchoolOrSummerProgramNotes: string;
  isInMentoringProgram: boolean;
  mentoringProgramNotes: string;
  hasChildWelfareWorkerId: number;
  hasChildWelfareWorkerName: string;
  childWelfareWorkerPlan: string;
  childWelfareWorkerChildren: string;
  childWelfareWorkerContactId: number;
  didOrWillAgeOutOfFosterCare: boolean;
  fosterCareNotes: string;
  hasFutureChildCareChanges: boolean;
  futureChildCareChangesNotes: string;
  actionNeeded: ActionNeeded;
  notes: string;
  rowVersion: string;
  assessmentRowVersion: string;
  modifiedBy: string;
  modifiedDate: number;
  isSpecialNeedsProgramming: boolean;
  specialNeedsProgrammingDetails: string;

  // constructor(private logService: LogService) {
  // }

  // NOTE: Be sure to keep this logic in sync with that in the contract classes in .NET API.

  get hasChildUnder5(): boolean {
    if (this.children != null && this.children.length > 0) {
      for (const child of this.children) {
        if (child.ageInYears !== null && child.ageInYears < 5) {
          return true;
        }
      }
    }

    if (this.cwwChildren != null && this.cwwChildren.length > 0) {
      for (const cww of this.cwwChildren) {
        if (cww.ageInYears !== null && cww.ageInYears < 5) {
          return true;
        }
      }
    }
    return false;
  }

  get hasChildOrTeen5orOver(): boolean {
    if (this.children != null && this.children.length > 0) {
      for (const child of this.children) {
        if (child.ageInYears !== null && child.ageInYears >= 5 && child.ageInYears <= 12) {
          return true;
        }
      }
    }

    if (this.teens != null && this.teens.length > 0) {
      for (const teen of this.teens) {
        if (teen.ageInYears !== null && teen.ageInYears < 19) {
          return true;
        }
      }
    }

    // Per Business Requirements (GitHub Issue #596) we also need to look at the
    // CWW referential data.
    if (this.cwwChildren != null && this.cwwChildren.length > 0) {
      for (const cww of this.cwwChildren) {
        if (cww.ageInYears !== null && cww.ageInYears >= 5 && cww.ageInYears < 19) {
          return true;
        }
      }
    }

    return false;
  }

  get hasNonEmptyChildren(): boolean {
    if (this.children != null && this.children.length > 0) {
      for (const child of this.children) {
        if (
          (child.firstName != null && child.firstName.trim() !== '') ||
          (child.lastName != null && child.lastName.trim() !== '') ||
          (child.dateOfBirth != null && child.dateOfBirth.trim() !== '') ||
          (child.careArrangementId != null && child.careArrangementId.toString() !== '') ||
          child.isSpecialNeeds != null ||
          (child.details != null && child.details.trim() !== '')
        ) {
          return true;
        }
      }
    }

    return false;
  }

  get hasNonEmptyTeens(): boolean {
    if (this.teens != null && this.teens.length > 0) {
      for (const teen of this.teens) {
        if (
          (teen.firstName != null && teen.firstName.trim() !== '') ||
          (teen.lastName != null && teen.lastName.trim() !== '') ||
          (teen.dateOfBirth != null && teen.dateOfBirth.trim() !== '') ||
          (teen.details != null && teen.details.trim() !== '')
        ) {
          return true;
        }
      }
    }

    return false;
  }

  get isAfterSchoolOrSummerProgramNotesRequired(): boolean {
    return this.hasChildOrTeen5orOver && this.isInAfterSchoolOrSummerProgram === true;
  }

  get isMentoringProgramNotesRequired(): boolean {
    return this.hasChildOrTeen5orOver && this.isInMentoringProgram === true;
  }

  get hasSpecialNeeds(): boolean {
    if (this.children != null && this.children.length > 0) {
      for (const child of this.children) {
        if (child.isSpecialNeeds !== null && child.isSpecialNeeds === true) {
          return true;
        }
      }
    }

    return false;
  }

  get hasSpecialNeedsChild(): boolean {
    if (this.hasTeensWithDisabilityInNeedOfChildCare || (this.hasChildren === true && this.hasSpecialNeeds === true)) {
      return true;
    }
    return false;
  }

  get isSpecialNeedsProgrammingRequired(): boolean {
    return this.hasSpecialNeedsChild && this.isSpecialNeedsProgramming === true;
  }

  public static clone(input: any, instance: ChildAndYouthSupportsSection) {
    instance.isSubmittedViaDriverFlow = input.isSubmittedViaDriverFlow;

    if (input.cwwChildren != null) {
      instance.cwwChildren = [];
      for (const cwwChild of input.cwwChildren) {
        instance.cwwChildren.push(new CwwChild().deserialize(cwwChild));
      }
    }

    if (input.cwwEligibility != null) {
      instance.cwwEligibility = new CwwEligibility().deserialize(input.cwwEligibility);
    }

    instance.hasChildren = input.hasChildren;

    instance.children = Utilities.deserilizeChildren(input.children, Child, 0);

    instance.hasTeensWithDisabilityInNeedOfChildCare = input.hasTeensWithDisabilityInNeedOfChildCare;

    instance.teens = Utilities.deserilizeChildren(input.teens, Teen, 0);

    instance.deletedChildren = [];
    instance.deletedTeens = [];
    instance.isSpecialNeedsProgramming = input.isSpecialNeedsProgramming;
    instance.specialNeedsProgrammingDetails = input.specialNeedsProgrammingDetails;
    instance.hasWicBenefits = input.hasWicBenefits;
    instance.isInHeadStart = input.isInHeadStart;
    instance.isInAfterSchoolOrSummerProgram = input.isInAfterSchoolOrSummerProgram;
    instance.afterSchoolOrSummerProgramNotes = input.afterSchoolOrSummerProgramNotes;
    instance.isInMentoringProgram = input.isInMentoringProgram;
    instance.mentoringProgramNotes = input.mentoringProgramNotes;

    instance.hasChildWelfareWorkerId = input.hasChildWelfareWorkerId;
    instance.hasChildWelfareWorkerName = input.hasChildWelfareWorkerName;
    instance.childWelfareWorkerPlan = input.childWelfareWorkerPlan;
    instance.childWelfareWorkerChildren = input.childWelfareWorkerChildren;
    instance.childWelfareWorkerContactId = input.childWelfareWorkerContactId;
    instance.didOrWillAgeOutOfFosterCare = input.didOrWillAgeOutOfFosterCare;
    instance.fosterCareNotes = input.fosterCareNotes;

    instance.hasFutureChildCareChanges = input.hasFutureChildCareChanges;
    instance.futureChildCareChangesNotes = input.futureChildCareChangesNotes;

    instance.notes = input.notes;
    instance.actionNeeded = Utilities.deserilizeChild(input.actionNeeded, ActionNeeded);
    instance.rowVersion = input.rowVersion;
    instance.assessmentRowVersion = input.assessmentRowVersion;
    instance.modifiedBy = input.modifiedBy;
    instance.modifiedDate = input.modifiedDate;
  }

  /**
   * Used to convert a JSON response into a proper ChildAndYouthSupportsSection object.
   *
   * @param {*} input
   * @returns {ChildAndYouthSupportsSection}
   *
   * @memberOf ChildAndYouthSupportsSection
   */
  public deserialize(input: any): ChildAndYouthSupportsSection {
    ChildAndYouthSupportsSection.clone(input, this);

    return this;
  }

  /**
   * Checks the ChildAndYouthSupportsSection object by applying provided business rules that indicate if this
   * instance is valid.  Specific validation errors are indicated by the errors property.
   *
   * @param {ValidationManager} validationManager
   * @returns {ValidationResult}
   *
   * @memberOf ChildAndYouthSupportsSection
   */
  public validate(validationManager: ValidationManager, participant: Participant, logService: LogService): ValidationResult {
    logService.timerStart('validate');
    const val = new Validate();
    val.model = 'ChildAndYouthSupportsSection';

    const result = new ValidationResult();

    // Children section
    if (this.hasChildren == null) {
      result.addError('hasChildren');
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Do you have any children age 12 years old or under?');
    } else if (this.hasChildren === true) {
      if (this.children == null || this.children.length === 0) {
        const errArr = result.createErrorsArray('children');
        // result.addError('hasChildren');
        // validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'One or more children must be entered');
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Do you have any children age 12 years old or under? - First Name');
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Do you have any children age 12 years old or under? - Last Name');
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Do you have any children age 12 years old or under? - Date of Birth');
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Do you have any children age 12 years old or under? - Care Arrangement');
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Do you have any children age 12 years old or under? - Special Needs');

        const me = result.createErrorsArrayItem(errArr);
        result.addErrorForParent(me, 'firstName');
        result.addErrorForParent(me, 'lastName');
        result.addErrorForParent(me, 'dateOfBirth');
        result.addErrorForParent(me, 'careArrangementId');
        result.addErrorForParent(me, 'isSpecialNeeds');
      } else {
        // Model errors for arrays gets a bit tricky.  We need to create an empty array
        // to contain the model errors object for each item, even if it's valid.  We do
        // this so the indexes will work when binding the front end repeaters to the
        // model errors object.
        const errArr = result.createErrorsArray('children');

        // Check to see if at least one field is set in the repeater or if only one child is present.
        let atLeastOneChildIsNonEmpty = false;

        for (const child of this.children) {
          // For each child, we need to at least create an empty model error object.
          const me = result.createErrorsArrayItem(errArr);

          if (
            (child.firstName == null || child.firstName.trim() === '') &&
            (child.lastName == null || child.lastName.trim() === '') &&
            (child.dateOfBirth == null || child.dateOfBirth.trim() === '') &&
            (child.careArrangementId == null || child.careArrangementId.toString() === '') &&
            child.isSpecialNeeds == null &&
            (child.details == null || child.details.trim() === '')
          ) {
            // A totally empty form for the child is valid... we just ignore it.
          } else {
            atLeastOneChildIsNonEmpty = true;

            if (!child.isFirstNameValid()) {
              result.addErrorForParent(me, 'firstName');
              validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Do you have any children age 12 years old or under? - First Name');
            }

            if (!child.isLastNameValid()) {
              result.addErrorForParent(me, 'lastName');
              validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Do you have any children age 12 years old or under? - Last Name');
            }

            // Begin date of birth validation
            if (child.dateOfBirth == null) {
              result.addErrorForParent(me, 'dateOfBirth');
              validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Do you have any children age 12 years old or under? - Date of Birth');
            } else if (child.dateOfBirth.length !== 10) {
              result.addErrorForParent(me, 'dateOfBirth');
              validationManager.addErrorWithFormat(
                ValidationCode.ValueInInvalidFormat_Name_FormatType,
                'Do you have any children age 12 years old or under? - Date Of Birth',
                'MM/DD/YYYY'
              );
            } else {
              // Do date validations here.

              // See if there is a valid date and be sure to use STRICT parsing
              // (using true as last parameter):
              const dob = moment(child.dateOfBirth, 'MM/DD/YYYY', true);

              if (!dob.isValid()) {
                result.addErrorForParent(me, 'dateOfBirth');
                validationManager.addErrorWithDetail(ValidationCode.InvalidDate, 'Do you have any children age 12 years old or under? - Date of Birth');
              } else {
                const minDate = Utilities.currentDate;
                const maxDate = Utilities.currentDate.subtract(13, 'years');

                if (dob.isAfter(minDate)) {
                  // We can't have a date after today.
                  result.addErrorForParent(me, 'dateOfBirth');
                  validationManager.addError(ValidationCode.ChildCareChildDobInFuture);
                  // childrenError.dateOfBirth = true;
                } else if (dob.isBefore(maxDate)) {
                  // We can't have a date before 13 years ago.
                  result.addErrorForParent(me, 'dateOfBirth');
                  validationManager.addError(ValidationCode.ChildCareChildDobOver12);
                }
              }
            }

            if (child.careArrangementId == null || child.careArrangementId.toString() === '') {
              result.addErrorForParent(me, 'careArrangementId');
              validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Do you have any children age 12 years old or under? - Care Arrangement');
            }

            if (child.isSpecialNeeds == null) {
              result.addErrorForParent(me, 'isSpecialNeeds');
              validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Do you have any children age 12 years old or under? - Special Needs');
            }
          }
        }

        if (!atLeastOneChildIsNonEmpty) {
          // result.addError('hasChildren');
          // validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'One or more children must be entered');
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Do you have any children age 12 years old or under? - First Name');
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Do you have any children age 12 years old or under? - Last Name');
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Do you have any children age 12 years old or under? - Date of Birth');
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Do you have any children age 12 years old or under? - Care Arrangement');
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Do you have any children age 12 years old or under? - Special Needs');

          // Besides the generic error, we need to highlight the empty fields on the
          // first item of the repeater.  We already have created an array of modelErrors
          // so we need to grab the first one and add errors to it.
          const me = errArr[0];
          result.addErrorForParent(me, 'firstName');
          result.addErrorForParent(me, 'lastName');
          result.addErrorForParent(me, 'dateOfBirth');
          result.addErrorForParent(me, 'careArrangementId');
          result.addErrorForParent(me, 'isSpecialNeeds');
        }
      }
    }

    // Teens section
    if (this.hasTeensWithDisabilityInNeedOfChildCare == null) {
      result.addError('hasTeensWithDisabilityInNeedOfChildCare');
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Do you have any children age 13 to 18 with special needs in need of child care?');
    } else if (this.hasTeensWithDisabilityInNeedOfChildCare === true) {
      if (this.teens == null || this.teens.length === 0) {
        // result.addError('hasTeensWithDisabilityInNeedOfChildCare');
        // validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'One or more teens must be entered');
        const errArr = result.createErrorsArray('teens');
        const me = result.createErrorsArrayItem(errArr);
        result.addErrorForParent(me, 'firstName');
        result.addErrorForParent(me, 'lastName');
        result.addErrorForParent(me, 'dateOfBirth');

        validationManager.addErrorWithDetail(
          ValidationCode.RequiredInformationMissing_Details,
          'Do you have any children age 13 to 18 with special needs in need of child care? - First Name'
        );
        validationManager.addErrorWithDetail(
          ValidationCode.RequiredInformationMissing_Details,
          'Do you have any children age 13 to 18 with special needs in need of child care? - Last Name'
        );
        validationManager.addErrorWithDetail(
          ValidationCode.RequiredInformationMissing_Details,
          'Do you have any children age 13 to 18 with special needs in need of child care? - Date of Birth'
        );
      } else {
        // Model errors for arrays gets a bit tricky.  We need to create an empty array
        // to contain the model errors object for each item, even if it's valid.  We do
        // this so the indexes will work when binding the front end repeaters to the
        // model errors object.
        const errArr = result.createErrorsArray('teens');

        // Check to see if at least one field is set in the repeater or if only one child is present.
        let atLeastOneTeenIsNonEmpty = false;

        for (const teen of this.teens) {
          // For each child, we need to at least create an empty model error object.
          const me = result.createErrorsArrayItem(errArr);

          if (
            (teen.firstName == null || teen.firstName.trim() === '') &&
            (teen.lastName == null || teen.lastName.trim() === '') &&
            (teen.dateOfBirth == null || teen.dateOfBirth.trim() === '') &&
            (teen.details == null || teen.details.trim() === '')
          ) {
            // A totally empty form for the teen is valid... we just ignore it.
          } else {
            atLeastOneTeenIsNonEmpty = true;

            if (!teen.isFirstNameValid()) {
              result.addErrorForParent(me, 'firstName');
              validationManager.addErrorWithDetail(
                ValidationCode.RequiredInformationMissing_Details,
                'Do you have any children age 13 to 18 with special needs in need of child care? - First Name'
              );
            }

            if (!teen.isLastNameValid()) {
              result.addErrorForParent(me, 'lastName');
              validationManager.addErrorWithDetail(
                ValidationCode.RequiredInformationMissing_Details,
                'Do you have any children age 13 to 18 with special needs in need of child care? - Last Name'
              );
            }

            // Begin date of birth validation
            if (teen.dateOfBirth == null) {
              result.addErrorForParent(me, 'dateOfBirth');
              validationManager.addErrorWithDetail(
                ValidationCode.RequiredInformationMissing_Details,
                'Do you have any children age 13 to 18 with special needs in need of child care? - Date of Birth'
              );
            } else if (teen.dateOfBirth.length !== 10) {
              result.addErrorForParent(me, 'dateOfBirth');
              validationManager.addErrorWithFormat(
                ValidationCode.ValueInInvalidFormat_Name_FormatType,
                'Do you have any children age 13 to 18 with special needs in need of child care? - Date Of Birth',
                'MM/DD/YYYY'
              );
            } else {
              // Do date validations here.

              // See if there is a valid date and be sure to use STRICT parsing
              // (using true as last parameter):
              const dob = moment(teen.dateOfBirth, 'MM/DD/YYYY', true);

              if (!dob.isValid()) {
                result.addErrorForParent(me, 'dateOfBirth');
                validationManager.addErrorWithDetail(ValidationCode.InvalidDate, 'Do you have any children age 13 to 18 with special needs in need of child care? - Date of Birth');
              } else {
                const maxDate = Utilities.currentDate;

                if (dob.isAfter(maxDate)) {
                  // We can't have a date after today.
                  result.addErrorForParent(me, 'dateOfBirth');
                  validationManager.addError(ValidationCode.ChildCareChildDobInFuture);
                } else {
                  const age = teen.ageInYears;

                  // We can't have a date before 13 years ago.
                  if (age < 13) {
                    result.addErrorForParent(me, 'dateOfBirth');
                    validationManager.addError(ValidationCode.ChildCareChildDobUnder13);
                  } else if (age > 18) {
                    result.addErrorForParent(me, 'dateOfBirth');
                    validationManager.addError(ValidationCode.ChildCareChildDobOver18);
                  }
                }
              }
            }
          }
        }

        if (!atLeastOneTeenIsNonEmpty) {
          // result.addError('hasTeensWithDisabilityInNeedOfChildCare');
          // validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'One or more teens must be entered');
          const me = errArr[0];
          result.addErrorForParent(me, 'firstName');
          result.addErrorForParent(me, 'lastName');
          result.addErrorForParent(me, 'dateOfBirth');

          validationManager.addErrorWithDetail(
            ValidationCode.RequiredInformationMissing_Details,
            'Do you have any children age 13 to 18 with special needs in need of child care? - First Name'
          );
          validationManager.addErrorWithDetail(
            ValidationCode.RequiredInformationMissing_Details,
            'Do you have any children age 13 to 18 with special needs in need of child care? - Last Name'
          );
          validationManager.addErrorWithDetail(
            ValidationCode.RequiredInformationMissing_Details,
            'Do you have any children age 13 to 18 with special needs in need of child care? - Date of Birth'
          );
        }
      }
    }

    if (this.hasSpecialNeedsChild && this.isSpecialNeedsProgramming == null) {
      result.addError('isSpecialNeedsProgramming');
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Does your child or family participate in any special needs programming?');
    }

    if (this.isSpecialNeedsProgrammingRequired && (this.specialNeedsProgrammingDetails == null || this.specialNeedsProgrammingDetails.trim() === '')) {
      result.addError('specialNeedsProgrammingDetails');
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Does your child or family participate in any special needs programming? - Details');
    }

    if (this.hasChildUnder5 && this.hasWicBenefits == null) {
      result.addError('hasWicBenefits');
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Are you receiving WIC benefits?');
    }

    if (this.hasChildUnder5 && this.isInHeadStart == null) {
      result.addError('isInHeadStart');
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Does your family participate in Head Start?');
    }

    if (this.hasChildOrTeen5orOver && this.isInAfterSchoolOrSummerProgram == null) {
      result.addError('isInAfterSchoolOrSummerProgram');
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Do any of your children participate in after school or summer programs?');
    }

    if (this.isAfterSchoolOrSummerProgramNotesRequired && (this.afterSchoolOrSummerProgramNotes == null || this.afterSchoolOrSummerProgramNotes.trim() === '')) {
      result.addError('afterSchoolOrSummerProgramNotes');
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Do any of your children participate in after school or summer programs? - Details');
    }

    if (this.hasChildOrTeen5orOver && this.isInMentoringProgram == null) {
      result.addError('isInMentoringProgram');
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Do any of your children participate in mentoring programs?');
    }

    if (this.isMentoringProgramNotesRequired && (this.mentoringProgramNotes == null || this.mentoringProgramNotes.trim() === '')) {
      result.addError('mentoringProgramNotes');
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Do any of your children participate in mentoring programs? - Details');
    }

    if (this.hasChildWelfareWorkerId != null && this.hasChildWelfareWorkerId === 1) {
      if (this.childWelfareWorkerChildren == null || this.childWelfareWorkerChildren.trim() === '') {
        result.addError('childWelfareWorkerChildren');
        validationManager.addErrorWithDetail(
          ValidationCode.RequiredInformationMissing_Details,
          'Do any of your children currently have a child welfare worker? - For which child(ren)?'
        );
      }

      if (this.childWelfareWorkerPlan == null || this.childWelfareWorkerPlan.trim() === '') {
        result.addError('childWelfareWorkerPlan');
        validationManager.addErrorWithDetail(
          ValidationCode.RequiredInformationMissing_Details,
          'Do any of your children currently have a child welfare worker? - Current plan or requirements'
        );
      }
    } else if (this.hasChildWelfareWorkerId == null) {
      // This question is required.
      result.addError('hasChildWelfareWorkerId');
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Do any of your children currently have a child welfare worker?');
    }

    if (this.isParticipantUnder26(participant)) {
      if (this.didOrWillAgeOutOfFosterCare == null) {
        result.addError('didOrWillAgeOutOfFosterCare');
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Did you or will you age out of foster care?');
      } else if (this.didOrWillAgeOutOfFosterCare === true && (this.fosterCareNotes == null || this.fosterCareNotes.trim() === '')) {
        result.addError('fosterCareNotes');
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Did you or will you age out of foster care? - Details');
      }
    }

    if (this.hasFutureChildCareChanges == null) {
      result.addError('hasFutureChildCareChanges');
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Will there be any changes with your child care in the near future?');
    }

    if ((this.futureChildCareChangesNotes === null || this.futureChildCareChangesNotes.trim() === '') && this.hasFutureChildCareChanges) {
      result.addError('futureChildCareChangesNotes');
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Will there be any changes with your child care in the near future? - Details');
    }

    // Action needed
    const anResult = this.actionNeeded.validate(validationManager);

    // TODO: wire up anResult better.
    if (anResult.isValid === false) {
      result.addError('actionNeeded');
    }

    val.isValid = result.isValid;
    val.errors = result.errors;
    logService.timerEndEvent('validate', val);

    return result;
  }

  public isParticipantUnder26(participant: Participant): boolean {
    if (participant == null) {
      return null;
    }

    const age = Utilities.getAgeFromDateOfBirth(participant.dateOfBirth);

    if (age == null) {
      return null;
    }

    return age < 26;
  }
}
