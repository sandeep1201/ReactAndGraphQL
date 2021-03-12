// tslint:disable: no-use-before-declare
import * as moment from 'moment';
import { Clearable } from '../interfaces/clearable';
import { GoogleLocation } from './google-location';
import { IsEmpty } from '../interfaces/is-empty';
import { YyyyValidationContext } from '../interfaces/validation-context';
import { MmYyyyValidationContext } from '../../shared/interfaces/mmYyyy-validation-context';
import { Serializable } from '../interfaces/serializable';
import { Utilities } from '../utilities';
import { Validate } from '../validate';
import { ValidationCode } from './validation-error';
import { ValidationManager } from './validation-manager';
import { ValidationResult } from './validation-result';
import { AppService } from 'src/app/core/services/app.service';

export class PostSecondaryEducationSection implements Serializable<PostSecondaryEducationSection> {
  isSubmittedViaDriverFlow: boolean;
  hasAttendedCollege: boolean;
  postSecondaryColleges: PostSecondaryCollege[];
  hasDegree: boolean;
  postSecondaryDegrees: PostSecondaryDegree[];
  isWorkingOnLicensesOrCertificates: boolean;
  postSecondaryLicenses: PostSecondaryLicense[];
  postSecondaryNotes: string;
  rowVersion: string;
  modifiedBy: string;
  modifiedDate: string;

  public static clone(input: any, instance: PostSecondaryEducationSection) {
    instance.isSubmittedViaDriverFlow = input.isSubmittedViaDriverFlow;
    instance.hasAttendedCollege = input.hasAttendedCollege;
    instance.postSecondaryColleges = Utilities.deserilizeChildren(input.postSecondaryColleges, PostSecondaryCollege);
    instance.hasDegree = input.hasDegree;
    instance.postSecondaryDegrees = Utilities.deserilizeChildren(input.postSecondaryDegrees, PostSecondaryDegree);
    instance.isWorkingOnLicensesOrCertificates = input.isWorkingOnLicensesOrCertificates;
    instance.postSecondaryLicenses = Utilities.deserilizeChildren(input.postSecondaryLicenses, PostSecondaryLicense);
    instance.postSecondaryNotes = input.postSecondaryNotes;
    instance.rowVersion = input.rowVersion;
    instance.modifiedBy = input.modifiedBy;
    instance.modifiedDate = input.modifiedDate;
  }

  deserialize(input) {
    PostSecondaryEducationSection.clone(input, this);
    return this;
  }

  get hasNonEmptyColleges(): boolean {
    return !Utilities.isArrayAllEmpty(this.postSecondaryColleges);
  }

  get hasNonEmptyDegrees(): boolean {
    return !Utilities.isArrayAllEmpty(this.postSecondaryDegrees);
  }

  get hasNonEmptyLicenses(): boolean {
    return !Utilities.isArrayAllEmpty(this.postSecondaryLicenses);
  }

  /**
   * Validates the current state of the PostSecondaryEducationSection object.
   *
   * @param {ValidationManager} validationManager
   * @param {moment.Moment} participantDOB
   * @returns {ValidationResult}
   *
   * @memberOf PostSecondaryEducationSection
   */
  public validate(validationManager: ValidationManager, participantDob: moment.Moment): ValidationResult {
    const result = new ValidationResult();

    if (this.hasAttendedCollege == null) {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Have you attended, or are you currently attending a college or university?');
      result.addError('hasAttendedCollege');
    } else if (this.hasAttendedCollege === true) {
      if (this.postSecondaryColleges == null || this.postSecondaryColleges.length === 0) {
        const errArr = result.createErrorsArray('postSecondaryColleges');
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'College/University Location');
        // Decided to only show this if the Location is set since it's disabled if Location is empty.
        // validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'College/University Name');
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'College/University Graduated');

        const me = result.createErrorsArrayItem(errArr);
        result.addErrorForParent(me, 'collegeLocation');
        result.addErrorForParent(me, 'collegeName');
        result.addErrorForParent(me, 'hasGraduated');
      } else {
        // Model errors for arrays gets a bit tricky.  We need to create an empty array
        // to contain the model errors object for each item, even if it's valid.  We do
        // this so the indexes will work when binding the front end repeaters to the
        // model errors object.
        const errArr = result.createErrorsArray('postSecondaryColleges');

        // Check to see if at least one field is set in the repeater or if only one child is present.
        let atLeastOneCollegeIsNonEmpty = false;

        for (const college of this.postSecondaryColleges) {
          // For each child, we need to at least create an empty model error object.
          const me = result.createErrorsArrayItem(errArr);

          if (college.isEmpty()) {
            // A totally empty form for the child is valid... we just ignore it.
          } else {
            atLeastOneCollegeIsNonEmpty = true;

            if (!college.isLocationValid()) {
              result.addErrorForParent(me, 'collegeLocation');
              validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'College/University Location');
            } else if (college.name == null || college.name.trim() === '') {
              // Using an else if above because the Location is required before enabling the name.
              result.addErrorForParent(me, 'collegeName');
              validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'College/University Name');
            }

            if (college.hasGraduated == null) {
              result.addErrorForParent(me, 'graduated');
              validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'College/University Graduated');
            }

            if (college.isCurrentlyAttending === true) {
              // Checking this option is always valid... no need to check the actual
              // value of the YYYY field since it will be cleared in this case.
            } else {
              const yyyyContext: YyyyValidationContext = {
                yyyy: college.lastYearAttended,
                prop: 'lastYearAttended',
                prettyProp: 'Last Year Attended',
                isRequired: false,
                maxDateNotInFuture: true,
                minDateParticipantDobYear: participantDob.year()
              };

              Validate.yyyyDate(yyyyContext, validationManager, result, me);
            }
          }
        }

        if (!atLeastOneCollegeIsNonEmpty) {
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'College/University Location');
          // Decided to only show this if the Location is set since it's disabled if Location is empty.
          // validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'College/University Name');
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'College/University Graduated');
          const me = errArr[0];
          result.addErrorForParent(me, 'collegeLocation');
          result.addErrorForParent(me, 'collegeName');
          result.addErrorForParent(me, 'graduated');
        }
      }

      // Also check Degree fields if they've indicated they've attended college.
      if (this.hasDegree == null) {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Do you have any degrees?');
        result.addError('hasDegree');
      } else if (this.hasDegree === true) {
        if (this.postSecondaryDegrees == null || this.postSecondaryDegrees.length === 0) {
          const errArr = result.createErrorsArray('postSecondaryDegrees');
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Degree Name');
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Degree Level');
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Degree College');
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Degree Year Attained');

          const me = result.createErrorsArrayItem(errArr);

          result.addErrorForParent(me, 'degreeName');
          result.addErrorForParent(me, 'degreeType');
          result.addErrorForParent(me, 'degreeCollege');
          result.addErrorForParent(me, 'yearAttained');
        } else {
          // Model errors for arrays gets a bit tricky.  We need to create an empty array
          // to contain the model errors object for each item, even if it's valid.  We do
          // this so the indexes will work when binding the front end repeaters to the
          // model errors object.
          const errArr = result.createErrorsArray('postSecondaryDegrees');

          // Check to see if at least one field is set in the repeater or if only one child is present.
          let atLeastOneDegreeIsNonEmpty = false;

          for (const degree of this.postSecondaryDegrees) {
            // For each child, we need to at least create an empty model error object.
            const me = result.createErrorsArrayItem(errArr);

            if (degree.isEmpty()) {
              // A totally empty form for the child is valid... we just ignore it.
            } else {
              atLeastOneDegreeIsNonEmpty = true;

              if (degree.name == null || degree.name.trim() === '') {
                result.addErrorForParent(me, 'degreeName');
                validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Degree Name');
              }

              if (degree.type == null || degree.type.toString() === '') {
                result.addErrorForParent(me, 'degreeType');
                validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Degree Level');
              }

              if (degree.college == null || degree.college.trim() === '') {
                result.addErrorForParent(me, 'degreeCollege');
                validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Degree College');
              }
              if (degree.yearAttained == null || degree.yearAttained.toString() === '') {
                result.addErrorForParent(me, 'yearAttained');
                validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Year Attained');
              }

              const yyyyContext: YyyyValidationContext = {
                yyyy: degree.yearAttained,
                prop: 'yearAttained',
                prettyProp: 'Year Attained',
                isRequired: true,
                maxDateNotInFuture: true,
                minDateParticipantDobYear: participantDob.year()
              };

              Validate.yyyyDate(yyyyContext, validationManager, result, me);
            }
          }

          if (!atLeastOneDegreeIsNonEmpty) {
            validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Degree Name');
            validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Degree Level');
            validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Degree College');
            validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Degree Year Attained');
            const me = errArr[0];
            result.addErrorForParent(me, 'degreeName');
            result.addErrorForParent(me, 'degreeType');
            result.addErrorForParent(me, 'degreeCollege');
            result.addErrorForParent(me, 'yearAttained');
          }
        }
      }
    }

    if (this.isWorkingOnLicensesOrCertificates == null) {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Do you have or are you working towards any licenses or certificates?');
      result.addError('isWorkingOnLicensesOrCertificates');
    } else if (this.isWorkingOnLicensesOrCertificates === true) {
      if (this.postSecondaryLicenses == null || this.postSecondaryLicenses.length === 0) {
        const errArr = result.createErrorsArray('postSecondaryLicenses');
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'License Type');
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'License Name');
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'License Valid in WI');
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'License Issuer');

        const me = result.createErrorsArrayItem(errArr);

        result.addErrorForParent(me, 'licenseType');
        result.addErrorForParent(me, 'licenseName');
        result.addErrorForParent(me, 'licenseValid');
        result.addErrorForParent(me, 'licenseIssuer');
      } else {
        // Model errors for arrays gets a bit tricky.  We need to create an empty array
        // to contain the model errors object for each item, even if it's valid.  We do
        // this so the indexes will work when binding the front end repeaters to the
        // model errors object.
        const errArr = result.createErrorsArray('postSecondaryLicenses');

        // Check to see if at least one field is set in the repeater or if only one child is present.
        let atLeastOneLicenseIsNonEmpty = false;

        for (const license of this.postSecondaryLicenses) {
          // For each child, we need to at least create an empty model error object.
          const me = result.createErrorsArrayItem(errArr);

          if (license.isEmpty()) {
            // A totally empty form for the child is valid... we just ignore it.
          } else {
            atLeastOneLicenseIsNonEmpty = true;

            Validate.dropDown(
              license.licenseType,
              {
                prop: 'licenseType',
                prettyProp: 'License Type',
                isRequired: true
              },
              validationManager,
              result,
              me
            );

            Validate.text(
              license.name,
              {
                prop: 'licenseName',
                prettyProp: 'License Name',
                isRequired: true
              },
              validationManager,
              result,
              me
            );

            Validate.dropDown(
              license.polarLookupId,
              {
                prop: 'licenseValid',
                prettyProp: 'License Valid in WI',
                isRequired: true
              },
              validationManager,
              result,
              me
            );

            Validate.text(
              license.issuer,
              {
                prop: 'licenseIssuer',
                prettyProp: 'License Issuer',
                isRequired: true
              },
              validationManager,
              result,
              me
            );

            let isYearAttainedValid = true;
            if (license.isInProgress === true) {
              // Checking this option is always valid... no need to check the actual value.
            } else {
              const attainedContext: MmYyyyValidationContext = {
                date: license.attainedDate,
                prop: 'licenseDateAttained',
                prettyProp: 'Month and Year Attained',
                isRequired: false,
                result: result,
                validationManager: validationManager,
                minDate: participantDob.format('MM/DD/YYYY'),
                minDateAllowSame: true,
                minDateName: "Participant's DOB",
                maxDate: Utilities.currentDate.format('MM/YYYY'),
                maxDateAllowSame: true,
                maxDateName: 'Current Date',
                participantDOB: participantDob
              };
              if (Utilities.validateMmYyyyDate(attainedContext) === false) {
                result.addErrorForParent(me, 'licenseDateAttained');
                isYearAttainedValid = false;
              }
            }

            let isYearExpirationValid = true;
            if (license.doesNotExpire === true) {
              // Checking this option is always valid... no need to check the actual value.
            } else {
              const expirationContext: MmYyyyValidationContext = {
                date: license.expiredDate,
                prop: 'licenseDateExpired',
                prettyProp: 'Expiration Date Month and Year',
                isRequired: false,
                result: result,
                validationManager: validationManager,
                minDate: participantDob.format('MM/DD/YYYY'),
                minDateAllowSame: true,
                minDateName: "Participant's DOB",
                maxDate: null,
                maxDateAllowSame: true,
                maxDateName: '',
                participantDOB: participantDob
              };
              if (Utilities.validateMmYyyyDate(expirationContext) === false) {
                result.addErrorForParent(me, 'licenseDateExpired');
                isYearExpirationValid = false;
              }
            }

            if (isYearExpirationValid === true && isYearExpirationValid === true && license.doesNotExpire !== true && license.isInProgress !== true) {
              const expirationContext: MmYyyyValidationContext = {
                date: license.expiredDate,
                prop: 'licenseDateExpired',
                prettyProp: 'Expiration Date Month and Year',
                isRequired: false,
                result: result,
                validationManager: validationManager,
                minDate: license.attainedDate,
                minDateAllowSame: true,
                minDateName: 'Attained Date',
                maxDate: null,
                maxDateAllowSame: false,
                maxDateName: '',
                participantDOB: null
              };
              if (Utilities.validateMmYyyyDate(expirationContext) === false) {
                result.addErrorForParent(me, 'licenseDateExpired');
              }
            }
          }
        }
        if (!atLeastOneLicenseIsNonEmpty) {
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'License Type');
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'License Name');
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'License Valid in WI');
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'License Issuer');
          const me = errArr[0];
          result.addErrorForParent(me, 'licenseType');
          result.addErrorForParent(me, 'licenseName');
          result.addErrorForParent(me, 'licenseValid');
          result.addErrorForParent(me, 'licenseIssuer');
        }
      }
    }

    return result;
  }
}

export class PostSecondaryCollege implements Clearable, IsEmpty, Serializable<PostSecondaryCollege> {
  id: number;
  name: string;
  location: GoogleLocation;
  address: string;
  hasGraduated: boolean;
  lastYearAttended: number;
  isCurrentlyAttending: boolean;
  semesters: number;
  credits: number;
  details: string;

  /**
   * Creates a new object suitable to be bound to the UI and passed to the API.
   * It will set the ID's to appropriate values.
   */
  public static create(): PostSecondaryCollege {
    const college = new PostSecondaryCollege();
    college.id = 0;

    return college;
  }

  public clear(): void {
    this.name = null;
    this.address = null;
    this.location = null;
    this.hasGraduated = null;
    this.lastYearAttended = null;
    this.semesters = null;
    this.credits = null;
    this.details = null;
    this.isCurrentlyAttending = null;
  }

  deserialize(input: any) {
    this.id = input.id;
    this.name = input.name;
    this.location = Utilities.deserilizeChild(input.location, GoogleLocation);
    this.address = input.address;
    this.hasGraduated = input.hasGraduated;
    this.lastYearAttended = input.lastYearAttended;
    this.semesters = input.semesters;
    this.credits = input.credits;
    this.details = input.details;
    this.isCurrentlyAttending = input.isCurrentlyAttending;

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
      this.location == null &&
      (this.name == null || this.name.trim() === '') &&
      (this.hasGraduated == null || this.hasGraduated.toString() === '') &&
      (this.lastYearAttended == null || this.lastYearAttended.toString() === '') &&
      (this.address == null || this.address.trim() === '') &&
      (this.semesters == null || this.semesters.toString() === '') &&
      (this.credits == null || this.credits.toString() === '') &&
      (this.details == null || this.details.trim() === '') &&
      (this.isCurrentlyAttending == null || this.isCurrentlyAttending.toString() === '')
    );
  }

  public isLocationValid(): boolean {
    let result = true;

    if (this.location == null || this.location.isEmpty()) {
      result = false;
    }

    return result;
  }

  public isNameValid(): boolean {
    let result = true;

    if (this.name == null) {
      result = false;
    }

    return result;
  }
}

export class PostSecondaryDegree implements Clearable, IsEmpty, Serializable<PostSecondaryDegree> {
  id: number;
  name: string;
  type: number;
  typeName: string;
  college: string;
  yearAttained: number;

  /**
   * Creates a new object suitable to be bound to the UI and passed to the API.
   * It will set the ID's to appropriate values.
   */
  public static create(): PostSecondaryDegree {
    const degree = new PostSecondaryDegree();
    degree.id = 0;

    return degree;
  }

  public clear(): void {
    this.name = null;
    this.type = null;
    this.typeName = null;
    this.college = null;
    this.yearAttained = null;
  }

  deserialize(input: any) {
    this.id = input.id;
    this.name = input.name;
    this.type = input.type;
    this.typeName = input.typeName;
    this.college = input.college;
    this.yearAttained = input.yearAttained;

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
      (this.name == null || this.name.trim() === '') &&
      (this.type == null || this.type.toString() === '') &&
      (this.typeName == null || this.typeName.trim() === '') &&
      (this.college == null || this.college.trim() === '') &&
      (this.yearAttained == null || this.yearAttained.toString() === '')
    );
  }

  public isCollegeNameValid(): boolean {
    let result = true;

    if (this.name == null) {
      result = false;
    }

    return result;
  }

  public isTypeValid(): boolean {
    let result = true;

    if (this.name == null) {
      result = false;
    }

    return result;
  }

  public isYearAttainedValid(): boolean {
    let result = true;

    if (this.name == null) {
      result = false;
    }

    return result;
  }
}

export class PostSecondaryLicense implements Clearable, IsEmpty, Serializable<PostSecondaryLicense> {
  id: number;
  licenseType: number;
  licenseTypeName: string;
  name: string;
  polarLookupId: number;
  validInWi: string;
  issuer: string;
  expiredDate: string;
  attainedDate: string;
  doesNotExpire: boolean;
  isInProgress: boolean;

  /**
   * Creates a new object suitable to be bound to the UI and passed to the API.
   * It will set the ID's to appropriate values.
   */
  public static create(): PostSecondaryLicense {
    const license = new PostSecondaryLicense();
    license.id = 0;

    return license;
  }

  public clear(): void {
    this.name = null;
    this.licenseType = null;
    this.licenseTypeName = null;
    this.polarLookupId = null;
    this.validInWi = null;
    this.issuer = null;
    this.expiredDate = null;
    this.attainedDate = null;
    this.doesNotExpire = null;
    this.isInProgress = null;
  }

  deserialize(input: any) {
    this.id = input.id;
    this.licenseType = input.licenseType;
    this.licenseTypeName = input.licenseTypeName;
    this.name = input.name;
    this.polarLookupId = input.polarLookupId;
    this.validInWi = input.validInWi;
    this.issuer = input.issuer;
    this.expiredDate = input.expiredDate;
    this.attainedDate = input.attainedDate;
    this.doesNotExpire = input.doesNotExpire;
    this.isInProgress = input.isInProgress;

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
      (this.name == null || this.name.trim() === '') &&
      (this.licenseType == null || this.licenseType.toString() === '') &&
      (this.licenseTypeName == null || this.licenseTypeName.toString() === '') &&
      (this.polarLookupId == null || this.polarLookupId.toString() === '') &&
      (this.validInWi == null || this.validInWi.toString() === '') &&
      (this.issuer == null || this.issuer.toString() === '') &&
      (this.expiredDate == null || this.expiredDate.toString() === '') &&
      (this.attainedDate == null || this.attainedDate.toString() === '') &&
      (this.doesNotExpire == null || this.doesNotExpire.toString() === '') &&
      (this.isInProgress == null || this.isInProgress.toString() === '')
    );
  }
  public isLicenseTypeValid(): boolean {
    let result = true;

    if (this.licenseType == null) {
      result = false;
    }

    return result;
  }

  public isValidInWiValid(): boolean {
    let result = true;

    if (this.name == null) {
      result = false;
    }

    return result;
  }
}
