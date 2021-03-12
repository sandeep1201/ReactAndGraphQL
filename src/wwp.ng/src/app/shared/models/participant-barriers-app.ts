import { Serializable } from '../interfaces/serializable';
import { BarrierSubType } from './job-actions';
import * as moment from 'moment';
import { MmDdYyyyValidationContext } from '../interfaces/mmDdYyyy-validation-context';
import { MmYyyyValidationContext } from '../interfaces/mmYyyy-validation-context';
import { Utilities } from '../utilities';
import { ValidationManager } from './validation-manager';
import { ValidationCode } from './validation-error';
import { ValidationResult } from './validation-result';
import { Validate } from '../validate';
import { AppService } from 'src/app/core/services/app.service';

export class ParticipantBarrier implements Serializable<ParticipantBarrier> {
  id: number;
  barrierTypeId: number;
  barrierTypeName: string;
  barrierSubType: BarrierSubType;
  onsetDate: string;
  endDate: string;
  wasClosedAtDisenrollment: boolean;
  details: string;
  contacts: number[];
  formalAssessments: FormalAssessment[];
  deletedFormalAssessments: FormalAssessment[];
  isAccommodationNeeded: boolean;
  barrierAccommodations: BarrierAccommodation[];
  deletedBarrierAccommodations: BarrierAccommodation[];
  isConverted: boolean;
  // isOpen: boolean;
  rowVersion: string;
  assessmentRowVersion: string;
  modifiedBy: string;
  modifiedDate: string;
  isDeleted: boolean;

  get isOpen(): boolean {
    if (this.endDate == null || this.endDate === '') {
      return true;
    } else if (this.endDate != null && moment(this.endDate, 'MM/YYYY').date(1) >= Utilities.currentDate.date(1).startOf('day')) {
      return true;
    } else {
      return false;
    }
  }

  public static clone(input: any, instance: ParticipantBarrier) {
    instance.id = input.id;
    instance.barrierTypeId = input.barrierTypeId;
    instance.barrierTypeName = input.barrierTypeName;
    if (input.barrierSubType != null) {
      instance.barrierSubType = new BarrierSubType().deserialize(input.barrierSubType);
    }
    instance.onsetDate = input.onsetDate;
    instance.endDate = input.endDate;
    instance.wasClosedAtDisenrollment = input.wasClosedAtDisenrollment;
    instance.details = input.details;
    instance.contacts = Utilities.deserilizeArray(input.contacts);
    instance.formalAssessments = Utilities.deserilizeChildren(input.formalAssessments, FormalAssessment);
    instance.deletedFormalAssessments = [];
    instance.isAccommodationNeeded = input.isAccommodationNeeded;
    instance.barrierAccommodations = Utilities.deserilizeChildren(input.barrierAccommodations, BarrierAccommodation);
    instance.deletedBarrierAccommodations = [];

    // if (input.endDate != null && moment(input.endDate, 'MM/YYYY') === moment()) {
    //   instance.isOpen = true;
    // } else if (input.endDate == null || input.endDate === '') {
    //   instance.isOpen = true;
    // } else {
    //   instance.isOpen = false;
    // }
    instance.rowVersion = input.rowVersion;
    instance.isConverted = input.isConverted;
    instance.assessmentRowVersion = input.assessmentRowVersion;
    instance.modifiedBy = input.modifiedBy;
    instance.modifiedDate = input.modifiedDate;
    instance.isDeleted = input.isDeleted;
  }

  get hasNonEmptyAccommodations(): boolean {
    if (this.barrierAccommodations != null && this.barrierAccommodations.length > 0) {
      for (const ba of this.barrierAccommodations) {
        if (
          (ba.accommodationId != null && ba.accommodationId.toString().trim() !== '') ||
          (ba.beginDate != null && ba.beginDate.trim() !== '') ||
          (ba.endDate != null && ba.endDate.trim() !== '') ||
          (ba.details != null && ba.details.trim() !== '')
        ) {
          return true;
        }
      }
    }

    return false;
  }

  /**
   * Use for newing up so that cache and ID are set.
   *
   * @returns
   *
   * @memberOf ParticipantBarrier
   */
  create() {
    this.id = 0;
    this.barrierSubType = new BarrierSubType();
    const ba = new BarrierAccommodation();
    this.contacts = [];
    this.barrierAccommodations = [];
    this.barrierAccommodations.push(ba);
    const fa = new FormalAssessment();
    this.formalAssessments = [];
    this.formalAssessments.push(fa);
    return this;
  }

  deserialize(input: any) {
    ParticipantBarrier.clone(input, this);
    // this.cached = new ParticipantBarrier();
    // ParticipantBarrier.graphObj(input, this.cached);
    return this;
  }

  isSubBarrierTypeDisabled(domesticViolenceId: number, aodaId: number): boolean {
    if (Number(this.barrierTypeId) === domesticViolenceId || Number(this.barrierTypeId) === aodaId) {
      return true;
    } else {
      return false;
    }
  }

  isSubBarrierTypeRequired() {
    if (this.formalAssessments != null) {
      for (const fa of this.formalAssessments) {
        if (fa.isAssessmentDateEntered()) {
          return true;
        } else {
          return false;
        }
      }
    } else {
      return false;
    }
  }

  // Formal Assessment section is diplayed unless domestic violence is selected for barrier type.
  public isFormalAssessmentDisplayed(domesticViolenceId: number): boolean {
    if (Number(this.barrierTypeId) === Number(domesticViolenceId)) {
      return false;
    } else {
      return true;
    }
  }
  public isAccommodationNeededRequired(): boolean {
    // If any of the formal assessments have an assessment date entered than isAccommodationNeeded is required.
    let isAccommodationNeededRequired = false;
    if (this.formalAssessments != null) {
      for (const fa of this.formalAssessments) {
        if (fa.isAssessmentDateEntered() === true) {
          isAccommodationNeededRequired = true;
        }
      }
    }
    return isAccommodationNeededRequired;
  }

  public validateHistorical(validationManager: ValidationManager): ValidationResult {
    const result = new ValidationResult();

    // MM/YYYY format.
    // Must be Valid.
    // Optional.
    // Min/equal: onset month.
    // Max: Past or Current Month.
    const endDateValidationContext: MmYyyyValidationContext = {
      date: this.endDate,
      prop: 'endDate',
      prettyProp: 'End Month',
      result: result,
      validationManager: validationManager,
      isRequired: false,
      minDate: this.onsetDate,
      minDateAllowSame: true,
      minDateName: 'Onset Month',
      maxDate: Utilities.currentDate.format('MM/YYYY'),
      maxDateAllowSame: true,
      maxDateName: 'Current Date',
      participantDOB: null
    };
    Utilities.validateMmYyyyDate(endDateValidationContext);

    return result;
  }

  public validate(
    validationManager: ValidationManager,
    participantDob: string,
    domesticViolenceId: number,
    aodaId: number,
    otherPCId: number,
    otherMCId: number,
    IntervalTypeDrop: any,
    IntervalTypeDayId?: number,
    IntervalTypeWeekId?: number
  ): ValidationResult {
    const result = new ValidationResult();

    // MM/YYYY format.
    // Must be Valid.
    // Required.
    // Min/equal: participantDob.
    // Max: Past or Current Month.
    const onsetDateValidationContext: MmYyyyValidationContext = {
      date: this.onsetDate,
      prop: 'onsetDate',
      prettyProp: 'Onset Month',
      result: result,
      validationManager: validationManager,
      isRequired: true,
      minDate: participantDob,
      minDateAllowSame: true,
      minDateName: "Participant's DOB (" + participantDob + ')',
      maxDate: Utilities.currentDate.format('MM/YYYY'),
      maxDateAllowSame: true,
      maxDateName: 'Current Date',
      participantDOB: null
    };
    Utilities.validateMmYyyyDate(onsetDateValidationContext);

    // MM/YYYY format.
    // Must be Valid.
    // Optional.
    // Min/equal: onset month.
    // Max: Past or Current Month.
    const endDateValidationContext: MmYyyyValidationContext = {
      date: this.endDate,
      prop: 'endDate',
      prettyProp: 'End Month',
      result: result,
      validationManager: validationManager,
      isRequired: false,
      minDate: this.onsetDate,
      minDateAllowSame: true,
      minDateName: 'Onset Month',
      maxDate: Utilities.currentDate.format('MM/YYYY'),
      maxDateAllowSame: true,
      maxDateName: 'Current Date',
      participantDOB: null
    };
    Utilities.validateMmYyyyDate(endDateValidationContext);
    if (this.barrierSubType.barrierSubTypes) {
      if (this.barrierSubType.barrierSubTypes.indexOf(otherPCId) !== -1 || this.barrierSubType.barrierSubTypes.indexOf(otherMCId) !== -1) {
        if (!this.details) {
          result.addError('barrierDetails');
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Barrier - Details');
        }
      }
    }

    Utilities.validateDropDown(this.barrierTypeId, 'barrierTypeId', 'Barrier Type', result, validationManager);

    if (!this.isSubBarrierTypeDisabled(domesticViolenceId, aodaId)) {
      if (this.isSubBarrierTypeRequired()) {
        Utilities.validateMultiSelect(this.barrierSubType.barrierSubTypes, 'barrierSubType', 'Barrier Subtype', result, validationManager);
      }
    }

    if (this.formalAssessments != null && this.isFormalAssessmentDisplayed(domesticViolenceId) === true) {
      const errArr = result.createErrorsArray('formalAssessments');
      let allFormalAssessmentsAreEmpty = true;

      // Check to see if all are empty.
      for (const fa of this.formalAssessments) {
        if (!fa.isEmpty()) {
          allFormalAssessmentsAreEmpty = false;
          break;
        }
      }

      if (allFormalAssessmentsAreEmpty) {
        // If all are empty validate the first one.
        if (this.formalAssessments[0] != null) {
          errArr.push(
            this.formalAssessments[0].validate(validationManager, this.onsetDate, participantDob, IntervalTypeDrop, IntervalTypeDayId, IntervalTypeWeekId, result).errors
          );
        }
      } else {
        for (const fa of this.formalAssessments) {
          if (!fa.isEmpty()) {
            const v = fa.validate(validationManager, this.onsetDate, participantDob, IntervalTypeDrop, IntervalTypeDayId, IntervalTypeWeekId);
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

    if (this.isAccommodationNeededRequired()) {
      Utilities.validateRequiredYesNo(result, validationManager, this.isAccommodationNeeded, 'isAccommodationNeeded', 'Are accommodations needed?');
    }

    if (this.barrierAccommodations != null && this.isAccommodationNeeded === true) {
      const errArr = result.createErrorsArray('barrierAccommodations');
      let allBarrierAccommodationsAreEmpty = true;

      // Check to see if all are empty.
      for (const ba of this.barrierAccommodations) {
        if (!ba.isEmpty()) {
          allBarrierAccommodationsAreEmpty = false;
          break;
        }
      }

      if (allBarrierAccommodationsAreEmpty) {
        // If all are empty validate the first one.
        if (this.barrierAccommodations[0] != null) {
          errArr.push(this.barrierAccommodations[0].validate(validationManager, this.onsetDate, participantDob, result).errors);
        }
      } else {
        for (const ba of this.barrierAccommodations) {
          if (!ba.isEmpty()) {
            const v = ba.validate(validationManager, this.onsetDate, participantDob);
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

export class FormalAssessment implements Serializable<FormalAssessment> {
  id: number;
  referralDate: string;
  referralDeclined: boolean;
  referralDetails: string;
  assessmentDate: string;
  assessmentNotCompleted: boolean;
  assessmentDetails: string;
  symptomId: number;
  symptomName: string;
  reassessmentRecommendedDate: string;
  isRecommendedDateNotNeeded: boolean;
  symptomDetails: string;
  hoursParticipantCanParticipate: number;
  hoursParticipantCanParticipateDetails: string;
  hoursParticipantCanParticipateIntervalId: number;
  hoursParticipantCanParticipateIntervalDesc: string;

  assessmentProviderContactId: number;

  private cachedReassessmentRecommendedDate: string;
  private cachedIsReassessmentRecommendedDatePassed: boolean;

  cached: FormalAssessment;

  get hasReassessmentRecommendedDatePassed(): boolean {
    // Handle the case where there is no reassessmentRecommendedDate
    if (this.reassessmentRecommendedDate == null) {
      return false;
    }

    // Cached value used to avoid initializing moment objects on every call.
    if (this.cachedReassessmentRecommendedDate === this.reassessmentRecommendedDate) {
      return this.cachedIsReassessmentRecommendedDatePassed;
    }
    const reassessmentRecommendedDate = moment(this.reassessmentRecommendedDate, 'MM/DD/YYYY');
    if (reassessmentRecommendedDate.isValid()) {
      this.cachedReassessmentRecommendedDate = this.reassessmentRecommendedDate;
      const currentDate = Utilities.currentDate;
      if (reassessmentRecommendedDate < currentDate) {
        this.cachedIsReassessmentRecommendedDatePassed = true;
        return true;
      } else {
        this.cachedIsReassessmentRecommendedDatePassed = false;
        return false;
      }
    } else {
      console.warn('reassessmentRecommendedDate Invalid');
      this.cachedIsReassessmentRecommendedDatePassed = false;
      return false;
    }
  }

  private static graphObj(input: any, instance: FormalAssessment) {
    instance.id = input.id;
    instance.referralDate = input.referralDate;
    instance.referralDeclined = input.referralDeclined;
    instance.referralDetails = input.referralDetails;
    instance.assessmentDate = input.assessmentDate;
    instance.assessmentNotCompleted = input.assessmentNotCompleted;
    instance.assessmentDetails = input.assessmentDetails;
    instance.symptomId = input.symptomId;
    instance.symptomName = input.symptomName;
    instance.reassessmentRecommendedDate = input.reassessmentRecommendedDate;
    instance.isRecommendedDateNotNeeded = input.isRecommendedDateNotNeeded;
    // instance.isReassessmentRecommendedDatePassed => get hasReassessmentRecommendedDatePassed().
    instance.symptomDetails = input.symptomDetails;
    instance.hoursParticipantCanParticipate = input.hoursParticipantCanParticipate;
    instance.hoursParticipantCanParticipateDetails = input.hoursParticipantCanParticipateDetails;
    instance.hoursParticipantCanParticipateIntervalId = input.hoursParticipantCanParticipateIntervalId;
    instance.hoursParticipantCanParticipateIntervalDesc = input.hoursParticipantCanParticipateIntervalDesc;
    instance.assessmentProviderContactId = input.assessmentProviderContactId;
  }

  public static create(): FormalAssessment {
    const fa = new FormalAssessment();
    fa.id = 0;
    return fa;
  }

  public clear(): void {
    this.referralDate = null;
    this.referralDeclined = null;
    this.referralDetails = null;
    this.assessmentDate = null;
    this.assessmentNotCompleted = null;
    this.assessmentDetails = null;
    this.symptomId = null;
    this.symptomName = null;
    this.reassessmentRecommendedDate = null;
    this.symptomDetails = null;
    this.assessmentProviderContactId = null;
    this.hoursParticipantCanParticipate = null;
    this.hoursParticipantCanParticipateDetails = null;
    this.hoursParticipantCanParticipateIntervalId = null;
  }

  /**
   * Detects whether or not a FormalAssessment object is effectively empty.
   * Does not check for assessmentProviderContactId or symptomName properties.
   * @returns {boolean}
   *
   * @memberOf FormalAssessment
   */
  public isEmpty(): boolean {
    // All properties have to be null or blank to make the entire object empty.
    return (
      (this.referralDate == null || this.referralDate.trim() === '') &&
      this.referralDeclined == null &&
      (this.referralDetails == null || this.referralDetails.trim() === '') &&
      (this.assessmentDate == null || this.assessmentDate.trim() === '') &&
      this.assessmentNotCompleted == null &&
      (this.assessmentDetails == null || this.assessmentDetails.trim() === '') &&
      (this.symptomId == null || this.symptomId.toString().trim() === '') &&
      (this.reassessmentRecommendedDate == null || this.reassessmentRecommendedDate.trim() === '') &&
      (this.symptomDetails == null || this.symptomDetails.trim() === '')
    );
  }

  public deserialize(input: any) {
    FormalAssessment.graphObj(input, this);
    this.cached = new FormalAssessment();
    FormalAssessment.graphObj(input, this.cached);
    return this;
  }

  public erase() {
    this.referralDate = null;
    this.referralDeclined = null;
    this.referralDetails = null;
    this.assessmentDate = null;
    this.assessmentNotCompleted = null;
    this.assessmentDetails = null;
    this.symptomId = null;
    this.symptomName = null;
    this.reassessmentRecommendedDate = null;
    this.symptomDetails = null;
    this.assessmentProviderContactId = null;
  }

  public isReferralDeclined(): boolean {
    if (this.referralDeclined === true) {
      return true;
    } else {
      return false;
    }
  }

  public isAssessmentDateEntered(): boolean {
    if (this.assessmentDate != null && this.assessmentDate.trim() !== '' && this.assessmentNotCompleted !== true) {
      return true;
    } else {
      return false;
    }
  }

  public isAssessmentNotCompleted(): boolean {
    if (this.assessmentNotCompleted === true) {
      return true;
    } else {
      return false;
    }
  }

  public isAssessmentNotNeeded(): boolean {
    if (this.isRecommendedDateNotNeeded === true) {
      return true;
    } else {
      return false;
    }
  }

  public isParticipationHoursInformationHidden(): boolean {
    if (this.isAssessmentNotCompleted() || this.isReferralDeclined()) {
      return true;
    } else {
      return false;
    }
  }
  public isParticipationHoursInformationEntered(): boolean {
    if ((this.hoursParticipantCanParticipate != null && this.hoursParticipantCanParticipate.toString().trim() !== '') || this.hoursParticipantCanParticipateIntervalId > 0) {
      if (this.hoursParticipantCanParticipateDetails == null) {
        this.hoursParticipantCanParticipateDetails = '';
      }
      return true;
    } else {
      return false;
    }
  }

  // tslint:disable-next-line:max-line-length
  public validate(
    validationManager: ValidationManager,
    onsetDate: string,
    participantDob: string,
    intervalTypeDrop: any[],
    intervalTypeDayId: number,
    intervalTypeWeekId: number,
    result?: ValidationResult
  ): ValidationResult {
    if (result == null) {
      result = new ValidationResult();
    }

    // Must be after or the same month as "onset month".
    // Must be current or past date.
    // Required.
    const referralDateValidationContext: MmDdYyyyValidationContext = {
      date: this.referralDate,
      prop: 'referralDate',
      prettyProp: 'Referral Date',
      result: result,
      validationManager: validationManager,
      isRequired: true,
      minDate: onsetDate,
      minDateAllowSame: true,
      minDateName: 'Onset Month',
      maxDate: Utilities.currentDate.format('MM/DD/YYYY'),
      maxDateAllowSame: true,
      maxDateName: 'Current Date',
      participantDOB: null
    };
    if (this.isReferralDeclined() !== true) {
      Utilities.validateMmDdYyyyDate(referralDateValidationContext);
    }

    // Must be after or the same month as "onset month".
    // Must be current or past date.
    // Not Required.
    const assessmentDateValidationContext: MmDdYyyyValidationContext = {
      date: this.assessmentDate,
      prop: 'assessmentDate',
      prettyProp: 'Assessment Date',
      result: result,
      validationManager: validationManager,
      isRequired: false,
      minDate: onsetDate,
      minDateAllowSame: true,
      minDateName: 'Onset Month',
      maxDate: Utilities.currentDate.format('MM/DD/YYYY'),
      maxDateAllowSame: true,
      maxDateName: 'Current Date',
      participantDOB: null
    };
    if (this.isAssessmentNotCompleted() !== true) {
      Utilities.validateMmDdYyyyDate(assessmentDateValidationContext);
    }

    if (this.isReferralDeclined() !== true && this.isAssessmentNotNeeded() !== true) {
      if (this.isAssessmentNotCompleted() === true) {
        Utilities.validateRequiredText(this.assessmentDetails, 'assessmentDetails', 'Assessment Date - Details', result, validationManager);
      }
    }
    if (this.isReferralDeclined() !== true && this.isAssessmentDateEntered() === true) {
      Utilities.validateDropDown(this.symptomId, 'symptomId', 'How long will the symptoms likely last?', result, validationManager);
    }
    if (this.isParticipationHoursInformationHidden() !== true && this.isParticipationHoursInformationEntered()) {
      Utilities.validateRequiredNumber(result, validationManager, this.hoursParticipantCanParticipate, 'hoursParticipantCanParticipate', 'Hours the individual can participate');

      if (this.hoursParticipantCanParticipateIntervalId === Utilities.idByFieldDataName('Day', intervalTypeDrop, true) && this.hoursParticipantCanParticipateIntervalId != null) {
        Validate.validateNumberInRange(
          +this.hoursParticipantCanParticipate,
          24,
          'hoursParticipantCanParticipate',
          'Hours per day the individual can participate cannot be more than 24',
          result,
          validationManager
        );
      } else if (this.hoursParticipantCanParticipateIntervalId == null) {
        Utilities.validateRequiredNumber(
          result,
          validationManager,
          this.hoursParticipantCanParticipateIntervalId,
          'hoursParticipantCanParticipateIntervalId',
          'Hours the individual can participate - Interval'
        );
      }
      if (this.hoursParticipantCanParticipateDetails != null && this.hoursParticipantCanParticipateDetails.trim() === '') {
        Utilities.validateRequiredText(
          this.hoursParticipantCanParticipateDetails,
          'hoursParticipantCanParticipateDetails',
          'Hours the individual can participate - Details',
          result,
          validationManager
        );
      }
    }
    // Must be after or the same month as "assessmentDate".
    // Must be no more than 120 years since DOB.
    // Required if isAssessmentDateEntered.
    const recommendedReassessmentDateValidationContext: MmDdYyyyValidationContext = {
      date: this.reassessmentRecommendedDate,
      prop: 'reassessmentRecommendedDate',
      prettyProp: 'Recommended Reassessment Date',
      result: result,
      validationManager: validationManager,
      isRequired: this.isAssessmentDateEntered(),
      minDate: this.assessmentDate,
      minDateAllowSame: true,
      minDateName: 'Assessment Date',
      maxDate: null,
      maxDateAllowSame: null,
      maxDateName: null,
      participantDOB: participantDob
    };
    if (this.isReferralDeclined() !== true && this.isAssessmentNotNeeded() !== true) {
      Utilities.validateMmDdYyyyDate(recommendedReassessmentDateValidationContext);
    }

    // Contact is required for formal assessment.
    const assessmentProviderContactIdNumber = Utilities.toNumberOrNull(this.assessmentProviderContactId);
    if (assessmentProviderContactIdNumber == null && this.isAssessmentNotCompleted() !== true && this.isReferralDeclined() !== true && this.isAssessmentDateEntered()) {
      Validate.validateRequiredContact(assessmentProviderContactIdNumber, 'assessmentProviderContactId', 'Assessment Provider', result, validationManager);
    }

    return result;
  }
}

export class BarrierAccommodation implements Serializable<BarrierAccommodation> {
  id: number;
  accommodationId: number;
  accommodationName: string;
  beginDate: string;
  endDate: string;
  details: string;
  cached: BarrierAccommodation;

  private static graphObj(input: any, instance: BarrierAccommodation) {
    instance.id = input.id;
    instance.accommodationId = input.accommodationId;
    instance.accommodationName = input.accommodationName;
    instance.beginDate = input.beginDate;
    instance.endDate = input.endDate;
    instance.details = input.details;
  }

  public static create(): BarrierAccommodation {
    const ba = new BarrierAccommodation();
    ba.id = 0;
    return ba;
  }

  public clear(): void {
    this.accommodationId = null;
    this.accommodationName = null;
    this.beginDate = null;
    this.endDate = null;
    this.details = null;
  }

  deserialize(input: any) {
    BarrierAccommodation.graphObj(input, this);
    this.cached = new BarrierAccommodation();
    BarrierAccommodation.graphObj(input, this.cached);
    return this;
  }

  erase() {
    this.accommodationId = null;
    this.accommodationName = null;
    this.beginDate = null;
    this.endDate = null;
    this.details = null;
  }

  /**
   * Detects whether or not a BarrierAccommodation object is effectively empty.
   *
   * @returns {boolean}
   *
   * @memberOf BarrierAccommodation
   */
  public isEmpty(): boolean {
    // All properties have to be null or blank to make the entire object empty.
    return (
      (this.accommodationId == null || this.accommodationId.toString().trim() === '') &&
      (this.beginDate == null || this.beginDate.trim() === '') &&
      (this.endDate == null || this.endDate.trim() === '') &&
      (this.details == null || this.details.trim() === '')
    );
  }

  validate(validationManager: ValidationManager, onsetDate, participantDob: string, result?: ValidationResult): ValidationResult {
    if (result == null) {
      result = new ValidationResult();
    }

    Utilities.validateDropDown(this.accommodationId, 'accommodationId', 'Accommodation For', result, validationManager);

    const beginDateWithValidationContext: MmDdYyyyValidationContext = {
      date: this.beginDate,
      prop: 'beginDate',
      prettyProp: 'Begin Date',
      result: result,
      validationManager: validationManager,
      isRequired: false,
      minDate: onsetDate,
      minDateAllowSame: true,
      minDateName: 'Onset Month',
      maxDate: null,
      maxDateAllowSame: null,
      maxDateName: null,
      participantDOB: participantDob
    };
    Utilities.validateMmDdYyyyDate(beginDateWithValidationContext, 150);

    // If begin date is null, make the min date for end date the DOB.
    let endDateMinDate = participantDob;
    let endDateMinDateName = "Participant's Date of Birth";
    if (!Utilities.isStringEmptyOrNull(this.beginDate)) {
      endDateMinDate = this.beginDate;
      endDateMinDateName = 'Begin Date';
    }

    const endDateWithValidationContext: MmDdYyyyValidationContext = {
      date: this.endDate,
      prop: 'endDate',
      prettyProp: 'End Date',
      result: result,
      validationManager: validationManager,
      isRequired: false,
      minDate: endDateMinDate,
      minDateAllowSame: true,
      minDateName: endDateMinDateName,
      maxDate: null,
      maxDateAllowSame: null,
      maxDateName: null,
      participantDOB: participantDob
    };
    Utilities.validateMmDdYyyyDate(endDateWithValidationContext, 150);

    // Details is required.
    Utilities.validateRequiredText(this.details, 'accommodationDetails', 'Accommodation Details', result, validationManager);

    return result;
  }
}
