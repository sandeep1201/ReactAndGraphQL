import * as moment from 'moment';

import { DropDownField } from '../../shared/models/dropdown-field';
import { GoogleLocation } from './google-location';
import { Serializable } from '../interfaces/serializable';
import { ValidationCode } from './validation-error';
import { ValidationManager } from './validation-manager';
import { ValidationResult } from './validation-result';
import { Utilities } from '../utilities';
import { Validate } from '../validate';
import { AppService } from 'src/app/core/services/app.service';

export class EducationHistorySection implements Serializable<EducationHistorySection> {
  isSubmittedViaDriverFlow: boolean;
  diploma: number;
  diplomaName: string;
  location: GoogleLocation;
  city: string;
  country: string;
  fullAddress: string;
  schoolName: string;
  notes: string;
  certificateYearAwarded: number;
  lastYearAttended: number;
  isCurrentlyEnrolled: boolean;
  hasEducationPlan: boolean;
  educationPlanDetails: string;
  lastGradeCompleted: number;
  lastGradeCompletedName: string;
  gedHsedStatus: boolean;
  hasEverGoneToSchool: boolean;
  issuingAuthorityCode: number;
  issuingAuthorityName: string;
  rowVersion: string;
  modifiedBy: string;
  modifiedDate: string;

  public static clone(input: any, instance: EducationHistorySection) {
    instance.isSubmittedViaDriverFlow = input.isSubmittedViaDriverFlow;
    instance.diploma = input.diploma;
    instance.diplomaName = input.diplomaName;
    instance.location = Utilities.deserilizeChild(input.location, GoogleLocation);
    instance.city = input.city;
    instance.country = input.country;
    instance.fullAddress = input.fullAddress;
    instance.schoolName = input.schoolName;
    instance.notes = input.notes;
    instance.certificateYearAwarded = input.certificateYearAwarded;
    instance.lastYearAttended = input.lastYearAttended;
    instance.isCurrentlyEnrolled = input.isCurrentlyEnrolled;
    instance.hasEducationPlan = input.hasEducationPlan;
    instance.educationPlanDetails = input.educationPlanDetails;
    instance.lastGradeCompleted = input.lastGradeCompleted;
    instance.lastGradeCompletedName = input.lastGradeCompletedName;
    instance.gedHsedStatus = input.gedHsedStatus;
    instance.hasEverGoneToSchool = input.hasEverGoneToSchool;
    instance.issuingAuthorityCode = input.issuingAuthorityCode;
    instance.issuingAuthorityName = input.issuingAuthorityName;
    instance.rowVersion = input.rowVersion;
    instance.modifiedBy = input.modifiedBy;
    instance.modifiedDate = input.modifiedDate;
  }

  public deserialize(input: any) {
    EducationHistorySection.clone(input, this);
    return this;
  }

  public isLocationValid(): boolean {
    let result = true;

    if (this.location == null || this.location.isEmpty()) {
      result = false;
    }

    return result;
  }
  isLastGradeCompletedDisplayed(educationDiplomaTypes: DropDownField[]): boolean {
    const diplomaId = Utilities.idByFieldDataName('Diploma', educationDiplomaTypes);
    const gedId = Utilities.idByFieldDataName('Ged', educationDiplomaTypes);
    const hsedId = Utilities.idByFieldDataName('HSED', educationDiplomaTypes);
    const noneId = Utilities.idByFieldDataName('None', educationDiplomaTypes);
    if (+this.diploma === gedId || +this.diploma === hsedId) {
      return true;
    } else if (+this.diploma === noneId && this.hasEverGoneToSchool === true) {
      return true;
    } else {
      return false;
    }
  }

  isCurrentlyEnrolledDisplayed(educationDiplomaTypes: DropDownField[]): boolean {
    const noneId = Utilities.idByFieldDataName('None', educationDiplomaTypes);
    if (+this.diploma === noneId) {
      return true;
    } else {
      return false;
    }
  }

  isTestScoresAppDisplayed(educationDiplomaTypes: DropDownField[]): boolean {
    const noneId = Utilities.idByFieldDataName('None', educationDiplomaTypes);
    if (+this.diploma === noneId) {
      return true;
    } else {
      return false;
    }
  }

  isSchoolLocationDisplayed(educationDiplomaTypes: DropDownField[]): boolean {
    const diplomaId = Utilities.idByFieldDataName('Diploma', educationDiplomaTypes);
    const gedId = Utilities.idByFieldDataName('Ged', educationDiplomaTypes);
    const hsedId = Utilities.idByFieldDataName('HSED', educationDiplomaTypes);
    const noneId = Utilities.idByFieldDataName('None', educationDiplomaTypes);
    if (+this.diploma === diplomaId || +this.diploma === gedId || +this.diploma === hsedId) {
      return true;
    } else if (+this.diploma === noneId && this.hasEverGoneToSchool === true) {
      return true;
    } else {
      return false;
    }
  }

  isStateIssuedDisplayed(educationDiplomaTypes: DropDownField[]): boolean {
    const gedId = Utilities.idByFieldDataName('GED', educationDiplomaTypes);
    const hsedId = Utilities.idByFieldDataName('HSED', educationDiplomaTypes);
    if (+this.diploma === gedId || +this.diploma === hsedId) {
      return true;
    } else {
      return false;
    }
  }

  validate(validationManager: ValidationManager, participantDOB: moment.Moment, diplomaId: number, gedId: number, hsedId: number, noneId: number): ValidationResult {
    // Assume it's valid to start
    const result = new ValidationResult();
    const dobYYYY = participantDOB.year();

    // Also need the current date in year format.
    const currentDate = new Date(Utilities.currentDate.format('MM/DD/YYYY'));
    const currentDateYYYY = currentDate.getFullYear();

    if (this.diploma == null) {
      result.addError('diploma');
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'What is your high school graduation status?');
    } else if (this.diploma === diplomaId) {
      // Diploma

      // Check for required fields: location
      Validate.googleLocation(this.location, 'schoolLocation', 'Location', result, validationManager);

      if (this.isLocationValid()) {
        if (this.schoolName == null || this.schoolName === '') {
          result.addError('schoolName');
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'School Name');
        }
      }

      // Year Attended Validation required.
      Utilities.valididateYyyyYear(
        this.lastYearAttended,
        dobYYYY,
        'Last Year Attended',
        'lastYearAttended',
        result,
        validationManager,
        ValidationCode.EducationHistoryLastYearAttendedInvalid_ParticipantDob,
        ValidationCode.EducationHistoryLastYearAttendedInFuture,
        true
      );
    } else if (this.diploma === gedId || this.diploma === hsedId) {
      // GED/HSED

      // Check for required fields: location
      Validate.googleLocation(this.location, 'schoolLocation', 'Location', result, validationManager);

      if (this.isLocationValid()) {
        if (this.schoolName == null || this.schoolName === '') {
          result.addError('schoolName');
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'School Name');
        }
      }

      // Year Attended Validation required.
      Utilities.valididateYyyyYear(
        this.lastYearAttended,
        dobYYYY,
        'Last Year Attended',
        'lastYearAttended',
        result,
        validationManager,
        ValidationCode.EducationHistoryLastYearAttendedInvalid_ParticipantDob,
        ValidationCode.EducationHistoryLastYearAttendedInFuture,
        true
      );

      if (this.lastGradeCompleted == null || this.lastGradeCompleted.toString() === '') {
        result.addError('lastGradeCompleted');
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Last Grade Completed');
      }

      if (this.issuingAuthorityCode == null || this.issuingAuthorityCode.toString() === '') {
        result.addError('issuingAuthorityCode');
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'State Issued');
      }

      // Certificate Year Validation.
      if (this.certificateYearAwarded != null && this.certificateYearAwarded.toString() !== '' && this.certificateYearAwarded <= dobYYYY) {
        result.addError('certificateYearAwarded');
        validationManager.addErrorWithFormat(ValidationCode.ValueBeforeDOB_Name_Value_DOB, 'Year Awarded', this.certificateYearAwarded.toString(), dobYYYY.toString());
      }

      if (this.certificateYearAwarded != null && this.certificateYearAwarded.toString() !== '' && this.certificateYearAwarded > currentDateYYYY) {
        let whichCert = 'GED';
        if (this.diploma === hsedId) {
          whichCert = 'HSED';
        }
        result.addError('certificateYearAwarded');
        validationManager.addErrorWithFormat(ValidationCode.EducationHistoryYearAwardedAfterCurrentYear_GedOrHsed, whichCert);
      }
    } else if (this.diploma === noneId) {
      // None

      if (this.hasEverGoneToSchool == null) {
        result.addError('hasEverGoneToSchool');
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Have you ever attended school?');
      } else if (this.hasEverGoneToSchool === true) {
        // Check for required fields: location
        Validate.googleLocation(this.location, 'schoolLocation', 'Location', result, validationManager);

        if (this.isLocationValid()) {
          if (this.schoolName == null || this.schoolName === '') {
            result.addError('schoolName');
            validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'School Name');
          }
        }

        if (this.lastGradeCompleted == null || this.lastGradeCompleted.toString() === '') {
          result.addError('lastGradeCompleted');
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Last Grade Completed');
        }

        // Year Attended Validation required.
        Utilities.valididateYyyyYear(
          this.lastYearAttended,
          dobYYYY,
          'Last Year Attended',
          'lastYearAttended',
          result,
          validationManager,
          ValidationCode.EducationHistoryLastYearAttendedInvalid_ParticipantDob,
          ValidationCode.EducationHistoryLastYearAttendedInFuture,
          true
        );

        if (this.isCurrentlyEnrolled == null) {
          result.addError('isCurrentlyEnrolled');
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Currently enrolled?');
        } else if (this.isCurrentlyEnrolled === true) {
          Utilities.validateRequiredYesNo(result, validationManager, this.hasEducationPlan, 'hasEducationPlan', 'Do you currently have an individualized education plan (IEP)?');

          if (this.hasEducationPlan === true) {
            Utilities.validateRequiredText(
              this.educationPlanDetails,
              'educationPlanDetails',
              'Do you currently have an individualized education plan (IEP)? - Details',
              result,
              validationManager
            );
          }
        }
      }
    }

    return result;
  }
}
