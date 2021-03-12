// tslint:disable: no-use-before-declare
// tslint:disable: no-shadowed-variable
import * as moment from 'moment';
import { ActionNeeded } from '../../features-modules/actions-needed/models/action-needed-new';
import { CleanseModelForApi } from '../interfaces/cleanse-model-for-api';
import { Cloneable } from '../interfaces/cloneable';
import { DropDownField } from './dropdown-field';
import { IsEmpty } from '../interfaces/is-empty';
import { Serializable } from '../interfaces/serializable';
import { Utilities } from '../utilities';
import { ValidationManager } from './validation-manager';
import { ValidationCode } from './validation-error';
import { ValidationResult } from './validation-result';
import { AppService } from 'src/app/core/services/app.service';

export class FamilyBarriersSection implements Serializable<FamilyBarriersSection>, CleanseModelForApi, Cloneable<FamilyBarriersSection> {
  // SSI/SSDI

  isSubmittedViaDriverFlow: boolean;
  hasEverAppliedSsi: boolean;
  isCurrentlyApplyingSsi: boolean;
  ssiApplicationStatusId: number;
  ssiApplicationStatusName: string;
  ssiApplicationStatusDetails: string;
  ssiApplicationDate: string;
  ssiApplicationIsAnyoneHelping: boolean;
  ssiApplicationDetails: string;
  ssiApplicationContactId: number;

  hasReceivedPastSsi: boolean;
  pastSsiDetails: string;

  hasDeniedSsi: boolean;
  deniedSsiDate: string;
  deniedSsiDetails: string;

  isInterestedInLearningMoreSsi: boolean;
  interestedInLearningMoreSsiDetails: string;

  hasAnyoneAppliedForSsi: boolean;
  isAnyoneReceivingSsi: boolean;
  anyoneReceivingSsiDetails: string;
  isAnyoneApplyingForSsi: boolean;
  anyoneApplyingForSsiDetails: string;

  // Family Needs
  hasCaretakingResponsibilities: boolean;
  hasConcernsAboutCaretakingResponsibilities: boolean;
  concernsAboutCaretakingResponsibilitiesDetails: string;

  doesHouseholdEngageInRiskyActivities: boolean;
  householdEngageInRiskyActivitiesDetails: string;
  doChildrenHaveBehaviourProblems: boolean;
  childrenHaveBehaviourProblemsDetails: string;
  areChildrenAtRiskOfSchoolSuspension: boolean;
  childrenAtRiskOfSchoolSuspensionDetails: string;
  areAnyFamilyIssuesAffectWork: boolean;
  anyFamilyIssuesAffectWorkDetails: string;
  familyBarriersReasonForPastSsiDetails: string;

  familyMembers: FamilyMember[];
  deletedFamilyMembers: FamilyMember[];
  // CWW reference data
  cwwLearnfare: CwwLearnfare[];
  cwwSocialSecurityStatus: CwwSocialSecurityStatus[];

  // Standard data
  actionNeeded: ActionNeeded;
  notes: string;
  rowVersion: string;
  assessmentRowVersion: string;
  modifiedBy: string;
  modifiedDate: string;

  get hasNonEmptyFamilyMembers(): boolean {
    if (this.familyMembers != null && this.familyMembers.length > 0) {
      for (const fm of this.familyMembers) {
        if (!fm.isEmpty()) {
          return true;
        }
      }
    }

    return false;
  }

  /**
   * Determines if the SSI Application Date field should be displayed and also required.
   *
   * @param {DropDownField[]} ssiApplicationStatuses
   * @returns {boolean}
   *
   * @memberOf FamilyBarriersSection
   */
  public isSsiApplicationDateDisplayedAndRequired(ssiApplicationStatuses: DropDownField[]): boolean {
    if (this.ssiApplicationStatusId == null || this.ssiApplicationStatusId.toString() === '' || this.ssiApplicationStatusId === 0) {
      return false;
    }

    if (ssiApplicationStatuses == null || ssiApplicationStatuses.length < 1) {
      return false;
    }

    // Lookup the name from the list of statuses using the ID.
    // NOTE: convert the property to a number using the + syntax.
    const id = +this.ssiApplicationStatusId;
    const appStatusField = ssiApplicationStatuses.find(ddf => ddf.id === id);

    if (appStatusField != null) {
      const appStatus = appStatusField.name;

      // Per US983:
      // If the Agency Worker selects any option that is "Applied" or "Appeal" - a Date Applied field will be displayed
      if (appStatus.toUpperCase().startsWith('APPEAL') || appStatus.toUpperCase().startsWith('APPLIED')) {
        return true;
      }
    }

    return false;
  }

  /**
   * Determines if the SSI Application Date field should be displayed and also required.
   *
   * @param {DropDownField[]} ssiApplicationStatuses
   * @returns {boolean}
   *
   * @memberOf FamilyBarriersSection
   */
  public isSsiApplicationIsAnyoneHelpingDisplayedAndRequired(ssiApplicationStatuses: DropDownField[]): boolean {
    // Per US983:
    // If the Agency Worker selects "Not yet submitted" or enters a Date Applied, they will be prompted with
    // the following question: "Is anyone helping you with your application?"
    if (
      (this.ssiApplicationStatusId == null || this.ssiApplicationStatusId.toString() === '' || this.ssiApplicationStatusId === 0) &&
      (this.ssiApplicationDate == null || this.ssiApplicationDate.trim() === '')
    ) {
      return false;
    }

    if (ssiApplicationStatuses == null || ssiApplicationStatuses.length < 1) {
      return false;
    }

    // Lookup the name from the list of statuses using the ID.
    // NOTE: convert the property to a number using the + syntax.
    const id = +this.ssiApplicationStatusId;
    const appStatusField = ssiApplicationStatuses.find(ddf => ddf.id === id);

    if (appStatusField != null) {
      const appStatus = appStatusField.name;

      // Per US983:
      // If the Agency Worker selects any option that is "Applied" or "Appeal" - a Date Applied field will be displayed
      if (appStatus.toLocaleLowerCase().startsWith('not yet submitted') || (this.ssiApplicationDate != null && this.ssiApplicationDate.trim().length > 0)) {
        return true;
      }
    }

    return false;
  }

  /**
   * Used to convert a JSON response into a proper FamilyBarriersSection object.
   *
   * @param {*} input
   * @returns {FamilyBarriersSection}
   *
   * @memberOf FamilyBarriersSection
   */
  public deserialize(input: any): FamilyBarriersSection {
    this.isSubmittedViaDriverFlow = input.isSubmittedViaDriverFlow;
    this.hasEverAppliedSsi = input.hasEverAppliedSsi;
    this.isCurrentlyApplyingSsi = input.isCurrentlyApplyingSsi;

    this.ssiApplicationStatusId = input.ssiApplicationStatusId;
    this.ssiApplicationStatusName = input.ssiApplicationStatusName;
    this.ssiApplicationStatusDetails = input.ssiApplicationStatusDetails;
    this.ssiApplicationDate = input.ssiApplicationDate;
    this.ssiApplicationIsAnyoneHelping = input.ssiApplicationIsAnyoneHelping;
    this.ssiApplicationDetails = input.ssiApplicationDetails;
    this.ssiApplicationContactId = input.ssiApplicationContactId;

    this.hasReceivedPastSsi = input.hasReceivedPastSsi;
    this.pastSsiDetails = input.pastSsiDetails;

    this.hasDeniedSsi = input.hasDeniedSsi;
    this.deniedSsiDate = input.deniedSsiDate;
    this.deniedSsiDetails = input.deniedSsiDetails;

    this.isInterestedInLearningMoreSsi = input.isInterestedInLearningMoreSsi;
    this.interestedInLearningMoreSsiDetails = input.interestedInLearningMoreSsiDetails;

    this.hasAnyoneAppliedForSsi = input.hasAnyoneAppliedForSsi;
    // Jared's markup had this field but it's not in US987
    // this.anyoneAppliedForSsiDetails = input.anyoneAppliedForSsiDetails;
    this.isAnyoneReceivingSsi = input.isAnyoneReceivingSsi;
    this.anyoneReceivingSsiDetails = input.anyoneReceivingSsiDetails;
    this.isAnyoneApplyingForSsi = input.isAnyoneApplyingForSsi;
    this.anyoneApplyingForSsiDetails = input.anyoneApplyingForSsiDetails;
    this.familyBarriersReasonForPastSsiDetails = input.familyBarriersReasonForPastSsiDetails;

    // Family Needs
    this.hasCaretakingResponsibilities = input.hasCaretakingResponsibilities;
    this.hasConcernsAboutCaretakingResponsibilities = input.hasConcernsAboutCaretakingResponsibilities;
    this.concernsAboutCaretakingResponsibilitiesDetails = input.concernsAboutCaretakingResponsibilitiesDetails;

    this.doesHouseholdEngageInRiskyActivities = input.doesHouseholdEngageInRiskyActivities;
    this.householdEngageInRiskyActivitiesDetails = input.householdEngageInRiskyActivitiesDetails;
    this.doChildrenHaveBehaviourProblems = input.doChildrenHaveBehaviourProblems;
    this.childrenHaveBehaviourProblemsDetails = input.childrenHaveBehaviourProblemsDetails;
    this.areChildrenAtRiskOfSchoolSuspension = input.areChildrenAtRiskOfSchoolSuspension;
    this.childrenAtRiskOfSchoolSuspensionDetails = input.childrenAtRiskOfSchoolSuspensionDetails;
    this.areAnyFamilyIssuesAffectWork = input.areAnyFamilyIssuesAffectWork;
    this.anyFamilyIssuesAffectWorkDetails = input.anyFamilyIssuesAffectWorkDetails;

    // Intialize an empty collection no matter what.
    this.familyMembers = Utilities.deserilizeChildren(input.familyMembers, FamilyMember);
    this.deletedFamilyMembers = Utilities.deserilizeChildren(input.deletedFamilyMembers, FamilyMember);
    this.cwwLearnfare = Utilities.deserilizeChildren(input.cwwLearnfare, CwwLearnfare);

    this.notes = input.notes;
    this.actionNeeded = Utilities.deserilizeChild(input.actionNeeded, ActionNeeded);
    this.rowVersion = input.rowVersion;
    this.assessmentRowVersion = input.assessmentRowVersion;
    this.modifiedBy = input.modifiedBy;
    this.modifiedDate = input.modifiedDate;

    return this;
  }

  /**
   * Checks the FamilyBarriersSection object by applying provided business rules that indicate if this
   * instance is valid.  Specific validation errors are indicated by the errors property.
   *
   * @param {ValidationManager} validationManager
   * @param {moment.Moment} participantDOB
   * @param {DropDownField[]} ssiApplicationStatuses
   * @param {ActionNeeded[]} possibleActionNeededs
   * @returns {ValidationResult}
   *
   * @memberOf FamilyBarriersSection
   */
  public validate(
    validationManager: ValidationManager,
    participantDOB: moment.Moment,
    ssiApplicationStatuses: DropDownField[],
    canAccessFamilyBarriersSsi?: boolean
  ): ValidationResult {
    const result = new ValidationResult();

    if (this.hasEverAppliedSsi == null && canAccessFamilyBarriersSsi) {
      result.addError('hasEverAppliedSsi');
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Have you ever applied for SSI or SSDI?');
    } else if (this.hasEverAppliedSsi === true && canAccessFamilyBarriersSsi) {
      if (this.isCurrentlyApplyingSsi == null) {
        result.addError('isCurrentlyApplyingSsi');
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Are you currently in the process of applying for SSI or SSDI?');
      } else if (this.isCurrentlyApplyingSsi === true) {
        if (this.ssiApplicationStatusId == null || this.ssiApplicationStatusId.toString() === '' || this.ssiApplicationStatusId === 0) {
          result.addError('ssiApplicationStatusId');
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'SSI or SSDI Application Status');
        } else if (this.isSsiApplicationDateDisplayedAndRequired(ssiApplicationStatuses)) {
          // Application date validation
          if (this.ssiApplicationDate == null) {
            result.addError('ssiApplicationDate');
            validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'SSI or SSDI Month Applied');
          } else if (this.ssiApplicationDate.length !== 7) {
            result.addError('ssiApplicationDate');
            validationManager.addErrorWithFormat(ValidationCode.ValueInInvalidFormat_Name_FormatType, 'SSI or SSDI Month Applied', 'MM/YYYY');
          } else {
            // See if there is a valid date and be sure to use STRICT parsing
            // (using true as last parameter):
            const appDate = moment(this.ssiApplicationDate, 'MM/YYYY', true);

            if (!appDate.isValid()) {
              result.addError('ssiApplicationDate');
              validationManager.addErrorWithDetail(ValidationCode.InvalidDate, 'SSI or SSDI Month Applied');
            } else {
              // The min date is the participant DOB, but we need the 1st of the month since
              // we only look at month/year.
              const minDate = participantDOB.format('YYYY-MM-01');
              const maxDate = Utilities.currentDate;

              if (appDate.isAfter(maxDate)) {
                // We can't have a date after today.
                result.addError('ssiApplicationDate');
                validationManager.addErrorWithDetail(ValidationCode.InvalidDate, 'Month Applied cannot be in the future');
              } else if (appDate.isBefore(minDate)) {
                // We can't have a date before Participant DOB.
                result.addError('ssiApplicationDate');
                validationManager.addErrorWithDetail(ValidationCode.InvalidDate, "Month Applied cannot be before the participant's date of birth");
              }
            }
          }
        }

        if (this.isSsiApplicationIsAnyoneHelpingDisplayedAndRequired(ssiApplicationStatuses)) {
          // Anyone helping?
          if (this.ssiApplicationIsAnyoneHelping == null) {
            result.addError('ssiApplicationIsAnyoneHelping');
            validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Is anyone helping you with your application?');
          } else if (this.ssiApplicationIsAnyoneHelping === true) {
            // Details
            if (this.ssiApplicationDetails == null || this.ssiApplicationDetails.trim() === '') {
              result.addError('ssiApplicationDetails');
              validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Is anyone helping you with your application? - Details');
            }
          }
        }
      }
      if (this.hasReceivedPastSsi && canAccessFamilyBarriersSsi) {
        Utilities.validateRequiredYesNoAndDetailsIfYes(
          result,
          validationManager,
          this.hasReceivedPastSsi,
          'hasReceivedPastSsi',
          'Why did you receive SSI or SSDI?',
          this.familyBarriersReasonForPastSsiDetails,
          'familyBarriersReasonForPastSsiDetails'
        );
      }

      Utilities.validateRequiredYesNoAndDetailsIfYes(
        result,
        validationManager,
        this.hasReceivedPastSsi,
        'hasReceivedPastSsi',
        'What is the reason you no longer receive SSI or SSDI?',
        this.pastSsiDetails,
        'pastSsiDetails'
      );

      if (this.hasDeniedSsi === true) {
        // Application date validation
        if (Utilities.isStringEmptyOrNull(this.deniedSsiDate)) {
          result.addError('deniedSsiDate');
          validationManager.addErrorWithDetail(
            ValidationCode.RequiredInformationMissing_Details,
            'Have you ever been denied for SSI or SSDI? – Approximate Month and Year of the Last Denial'
          );
        } else if (this.deniedSsiDate.length !== 7) {
          result.addError('deniedSsiDate');
          validationManager.addErrorWithFormat(
            ValidationCode.ValueInInvalidFormat_Name_FormatType,
            'Have you ever been denied for SSI or SSDI? – Approximate Month and Year of the Last Denial',
            'MM/YYYY'
          );
        } else {
          // See if there is a valid date and be sure to use STRICT parsing
          // (using true as last parameter):
          const appDate = moment(this.deniedSsiDate, 'MM/YYYY', true);

          if (!appDate.isValid()) {
            result.addError('deniedSsiDate');
            validationManager.addErrorWithDetail(ValidationCode.InvalidDate, 'Have you ever been denied for SSI or SSDI? – Approximate Month and Year of the Last Denial');
          } else {
            // The min date is the participant DOB, but we need the 1st of the month since
            // we only look at month/year.
            const minDate = participantDOB.format('YYYY-MM-01');
            const maxDate = Utilities.currentDate;

            if (appDate.isAfter(maxDate)) {
              // We can't have a date after today.
              result.addError('deniedSsiDate');
              validationManager.addErrorWithDetail(ValidationCode.InvalidDate, 'Approximate Month and Year of the Last Denial cannot be in the future');
            } else if (appDate.isBefore(minDate)) {
              // We can't have a date before Participant DOB.
              result.addError('deniedSsiDate');
              validationManager.addErrorWithDetail(ValidationCode.InvalidDate, "Approximate Month and Year of the Last Denial cannot be before the participant's date of birth");
            }
          }
        }
      }

      Utilities.validateRequiredYesNoAndDetailsIfYes(
        result,
        validationManager,
        this.hasDeniedSsi,
        'hasDeniedSsi',
        'Have you ever been denied for SSI or SSDI?',
        this.deniedSsiDetails,
        'deniedSsiDetails'
      );
    }
    if (canAccessFamilyBarriersSsi) {
      Utilities.validateRequiredYesNoAndDetailsIfYes(
        result,
        validationManager,
        this.isInterestedInLearningMoreSsi,
        'isInterestedInLearningMoreSsi',
        'Are you interested in learning more about the SSI/SSDI application process?',
        this.interestedInLearningMoreSsiDetails,
        'interestedInLearningMoreSsiDetails'
      );
      if (this.hasReceivedPastSsi === true) {
        Utilities.validateRequiredYesNoAndDetailsIfYes(
          result,
          validationManager,
          this.hasReceivedPastSsi,
          'hasReceivedPastSsi',
          'Why did you receive SSI or SSDI?',
          this.familyBarriersReasonForPastSsiDetails,
          'familyBarriersReasonForPastSsiDetails'
        );
      }
    }

    if (this.hasAnyoneAppliedForSsi == null) {
      result.addError('hasAnyoneAppliedForSsi');
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Has anyone in your family ever applied for SSI or SSDI?');
    } else if (this.hasAnyoneAppliedForSsi === true) {
      // Nested questions
      Utilities.validateRequiredYesNoAndDetailsIfYes(
        result,
        validationManager,
        this.isAnyoneReceivingSsi,
        'isAnyoneReceivingSsi',
        'Is anyone in your family receiving SSI or SSDI?',
        this.anyoneReceivingSsiDetails,
        'anyoneReceivingSsiDetails'
      );

      Utilities.validateRequiredYesNoAndDetailsIfYes(
        result,
        validationManager,
        this.isAnyoneApplyingForSsi,
        'isAnyoneApplyingForSsi',
        'Is anyone in your family in the process of applying for SSI or SSDI?',
        this.anyoneApplyingForSsiDetails,
        'anyoneApplyingForSsiDetails'
      );
    }

    // Family Needs
    Utilities.validateRequiredYesNo(
      result,
      validationManager,
      this.hasCaretakingResponsibilities,
      'hasCaretakingResponsibilities',
      'Do you have caretaking responsibilities for any family members in your household due to health problems or other special needs?'
    );

    // Only if they've answered yes to previous questions.
    if (this.hasCaretakingResponsibilities === true) {
      if (this.familyMembers == null || this.familyMembers.length === 0) {
        const errArr = result.createErrorsArray('familyMembers');
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'First Name');
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Last Name');
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Relationship');
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Details');

        const me = result.createErrorsArrayItem(errArr);
        result.addErrorForParent(me, 'firstName');
        result.addErrorForParent(me, 'lastName');
        result.addErrorForParent(me, 'relationshipId');
        result.addErrorForParent(me, 'details');
      } else {
        // Model errors for arrays gets a bit tricky.  We need to create an empty array to contain
        // the model errors object for each item, even if it's valid.  We do this so the indexes
        // will work when binding the front end repeaters to the model errors object.
        const errArr = result.createErrorsArray('familyMembers');

        // Check to see if at least one field is set in the repeater or if only one family member is present.
        let atLeastOneFamilyMemberIsNonEmpty = false;

        for (const child of this.familyMembers) {
          // For each family member, we need to at least create an empty model error object.
          const me = result.createErrorsArrayItem(errArr);

          if (child.isEmpty()) {
            // A totally empty form for the child is valid... we just ignore it.
          } else {
            atLeastOneFamilyMemberIsNonEmpty = true;

            if (child.firstName == null || child.firstName.trim() === '') {
              result.addErrorForParent(me, 'firstName');
              validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'First Name');
            }

            if (child.lastName == null || child.lastName.trim() === '') {
              result.addErrorForParent(me, 'lastName');
              validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Last Name');
            }

            if (child.relationshipId == null || child.relationshipId.toString() === '' || child.relationshipId === 0) {
              result.addErrorForParent(me, 'relationshipId');
              validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Relationship');
            }

            if (child.details == null || child.details.trim() === '') {
              result.addErrorForParent(me, 'details');
              validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Details');
            }
          }
        }

        if (!atLeastOneFamilyMemberIsNonEmpty) {
          // validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'One or more family members must be entered including First Name, Last Name, Relationship and Details');
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'First Name');
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Last Name');
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Relationship');
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Details');

          // Besides the generic error, we need to highlight the empty fields on the
          // first item of the repeater.  We already have created an array of modelErrors
          // so we need to grab the first one and add errors to it.
          const me = errArr[0];
          result.addErrorForParent(me, 'firstName');
          result.addErrorForParent(me, 'lastName');
          result.addErrorForParent(me, 'relationshipId');
          result.addErrorForParent(me, 'details');
        }
      }

      Utilities.validateRequiredYesNoAndDetailsIfYes(
        result,
        validationManager,
        this.hasConcernsAboutCaretakingResponsibilities,
        'hasConcernsAboutCaretakingResponsibilities',
        'Do you have concerns that these caretaking responsibilities will make it hard to participate in work activities?',
        this.concernsAboutCaretakingResponsibilitiesDetails,
        'concernsAboutCaretakingResponsibilitiesDetails'
      );
    }

    Utilities.validateRequiredYesNoAndDetailsIfYes(
      result,
      validationManager,
      this.doesHouseholdEngageInRiskyActivities,
      'doesHouseholdEngageInRiskyActivities',
      'Do any family members in your household engage in risky activities such as excessive use of drugs or alcohol, illegal activity, or gang involvement?',
      this.householdEngageInRiskyActivitiesDetails,
      'householdEngageInRiskyActivitiesDetails'
    );

    Utilities.validateRequiredYesNoAndDetailsIfYes(
      result,
      validationManager,
      this.doChildrenHaveBehaviourProblems,
      'doChildrenHaveBehaviourProblems',
      'Do any of the children in your household have other behavior problems that will affect your ability to participate in work activities?',
      this.childrenHaveBehaviourProblemsDetails,
      'childrenHaveBehaviourProblemsDetails'
    );

    Utilities.validateRequiredYesNoAndDetailsIfYes(
      result,
      validationManager,
      this.areChildrenAtRiskOfSchoolSuspension,
      'areChildrenAtRiskOfSchoolSuspension',
      'Are any of the children in your household at risk of suspension or expulsion from school?',
      this.childrenAtRiskOfSchoolSuspensionDetails,
      'childrenAtRiskOfSchoolSuspensionDetails'
    );

    Utilities.validateRequiredYesNoAndDetailsIfYes(
      result,
      validationManager,
      this.areAnyFamilyIssuesAffectWork,
      'areAnyFamilyIssuesAffectWork',
      'Are there any other issues with your family that may affect your ability to participate in work activities?',
      this.anyFamilyIssuesAffectWorkDetails,
      'anyFamilyIssuesAffectWorkDetails'
    );

    const anResult = this.actionNeeded.validate(validationManager);

    // TODO: wire up anResult better.
    if (anResult.isValid === false) {
      result.addError('actionNeeded');
    }

    return result;
  }

  /**
   * Cleanses any invalid properties of this object.  Implements the
   * CleanseModelForApi interface
   *
   * @returns {FamilyBarriersSection}
   *
   * @memberOf FamilyBarriersSection
   */
  public cleanse(): void {
    // Must be a valid date.
    Utilities.cleanseDateWithFormat(this.ssiApplicationDate, 'MM/YYYY');

    Utilities.cleanseDateWithFormat(this.deniedSsiDate, 'MM/YYYY');
  }

  /**
   * Create a clone of this object.
   *
   * @returns {FamilyBarriersSection}
   *
   * @memberOf FamilyBarriersSection
   */
  public clone<FamilyBarriersSection>() {
    return new FamilyBarriersSection().deserialize(JSON.parse(JSON.stringify(this)));
  }
}

export class FamilyMember implements IsEmpty, Serializable<FamilyMember> {
  id: number;
  firstName: string;
  lastName: string;
  relationshipId: number;
  relationshipName: string;
  details: string;
  deleteReasonId: number;

  public static create(): FamilyMember {
    const fm = new FamilyMember();
    fm.id = 0;
    return fm;
  }

  public clear() {
    this.firstName = null;
    this.lastName = null;
    this.relationshipId = null;
    this.relationshipName = null;
    this.details = null;
  }

  /**
   * Used to convert a JSON response into a proper FamilyMember object.
   *
   * @param {*} input
   * @returns {FamilyMember}
   *
   * @memberOf FamilyMember
   */
  public deserialize(input: any): FamilyMember {
    this.id = input.id;
    this.firstName = input.firstName;
    this.lastName = input.lastName;
    this.relationshipId = input.relationshipId;
    this.relationshipName = input.relationshipName;
    this.details = input.details;
    this.deleteReasonId = input.deleteReasonId;

    return this;
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
      (this.relationshipId == null || this.relationshipId.toString() === '' || this.relationshipId === 0) &&
      (this.details == null || this.details.trim() === '')
    );
  }
}

export class CwwLearnfare implements Serializable<CwwLearnfare> {
  firstName: string;
  middleInitial: string;
  lastName: string;
  birthDate: string;
  learnFareStatus: string;

  public deserialize(input: any): CwwLearnfare {
    this.firstName = input.firstName;
    this.middleInitial = input.middleInitial;
    this.lastName = input.lastName;
    this.birthDate = input.birthDate;
    this.learnFareStatus = input.learnFareStatus;

    return this;
  }
}

export class CwwSocialSecurityStatus implements Serializable<CwwSocialSecurityStatus> {
  participant: string;
  firstName: string;
  middle: string;
  lastName: string;
  dob: string;
  relationship: string;
  age: string;
  fedSsi: string;
  stateSsi: string;
  ssa: string;

  public deserialize(input: any): CwwSocialSecurityStatus {
    this.participant = input.participant;
    this.firstName = input.firstName;
    this.middle = input.middle;
    this.lastName = input.lastName;
    this.dob = input.dob;
    this.relationship = input.relationship;
    this.relationship = input.relationship;
    this.age = input.age;
    this.fedSsi = input.fedSsi;
    this.stateSsi = input.stateSsi;
    this.ssa = input.ssa;

    return this;
  }
}
