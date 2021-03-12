import { Serializable } from '../../shared/interfaces/serializable';
import { ValidationResult } from './validation-result';
import { ValidationManager, ValidationCode } from 'src/app/shared/models/validation';
import { MakeUpEntries } from './participation-makeup.model';
import { Utilities } from 'src/app/shared/utilities';
import { HrsValidationContext } from 'src/app/shared/interfaces/hrs-validation-context';
import { DropDownField } from 'src/app/shared/models/dropdown-field';

export class ParticipationTracking implements Serializable<ParticipationTracking> {
  id: number;
  epId: number;
  activityId: number;
  activityCd: string;
  activityName: string;
  canEditBasedOnOrg: boolean;
  participationDate: string;
  scheduledHours: string;
  reportedHours: string;
  totalMakeupHours: string;
  participatedHours: string;
  nonParticipatedHours: string;
  goodCausedHours: string;
  nonParticipationReasonId: number;
  nonParticipationReasonCd: string;
  nonParticipationReasonName: string;
  nonParticipationReasonDetails: string;
  goodCauseGranted: boolean;
  goodCauseReasonCd: string;
  goodCauseGrantedReasonId: number;
  goodCauseGrantedReasonName: string;
  goodCauseDeniedReasonId: number;
  goodCauseDeniedReasonName: string;
  goodCauseReasonDetails: string;
  placementTypeId: number;
  placementTypeName: string;
  placementTypeCd: string;
  formalAssessmentExists: boolean;
  hoursSanctionable: boolean;
  sanctionableHours: String;
  modifiedBy: string;
  modifiedDate: string;
  canEditBasedOnDate: boolean;
  isProcessed: boolean;
  processedDate: string;
  makeUpEntries: MakeUpEntries[];

  public static clone(input: any, instance: ParticipationTracking) {
    instance.id = input.id;
    instance.epId = input.epId;
    instance.activityId = input.activityId;
    instance.activityCd = input.activityCd;
    instance.activityName = input.activityName;
    instance.canEditBasedOnOrg = input.canEditBasedOnOrg;
    instance.participationDate = input.participationDate;
    instance.scheduledHours = input.scheduledHours;
    instance.reportedHours = input.reportedHours;
    instance.totalMakeupHours = input.totalMakeupHours;
    instance.participatedHours = input.participatedHours;
    instance.nonParticipatedHours = input.nonParticipatedHours;
    instance.goodCausedHours = input.goodCausedHours;
    instance.nonParticipationReasonId = input.nonParticipationReasonId;
    instance.nonParticipationReasonCd = input.nonParticipationReasonCd;
    instance.nonParticipationReasonName = input.nonParticipationReasonName;
    instance.nonParticipationReasonDetails = input.nonParticipationReasonDetails;
    instance.goodCauseGranted = input.goodCauseGranted;
    instance.goodCauseGrantedReasonId = input.goodCauseGrantedReasonId;
    instance.goodCauseReasonCd = input.goodCauseReasonCd;
    instance.goodCauseGrantedReasonName = input.goodCauseGrantedReasonName;
    instance.goodCauseDeniedReasonId = input.goodCauseDeniedReasonId;
    instance.goodCauseDeniedReasonName = input.goodCauseDeniedReasonName;
    instance.goodCauseReasonDetails = input.goodCauseReasonDetails;
    instance.placementTypeId = input.placementTypeId;
    instance.placementTypeName = input.placementTypeName;
    instance.placementTypeCd = input.placementTypeCd;
    instance.formalAssessmentExists = input.formalAssessmentExists;
    instance.hoursSanctionable = input.hoursSanctionable;
    if (instance.hoursSanctionable) {
      instance.sanctionableHours = (+input.nonParticipatedHours - +input.goodCausedHours).toFixed(1);
    } else {
      instance.sanctionableHours = (0).toFixed(1);
    }
    instance.canEditBasedOnDate = input.canEditBasedOnDate;
    instance.isProcessed = input.isProcessed;
    instance.processedDate = input.processedDate;
    instance.makeUpEntries = Utilities.deserilizeChildren(input.makeUpEntries, MakeUpEntries, 0);
    instance.modifiedBy = input.modifiedBy;
    instance.modifiedDate = input.modifiedDate;
  }
  public deserialize(input: any) {
    ParticipationTracking.clone(input, this);
    return this;
  }

  public isnonParticipationReasonsDetailsRequired(nonParticipationReasons: DropDownField[]) {
    if (this.nonParticipationReasonId === Utilities.idByFieldDataName('XX - Other Reason', nonParticipationReasons)) {
      return true;
    } else {
      return false;
    }
  }
  public isgoodCauseReasonsDetailsRequired(dropDown: DropDownField[]) {
    if (this.goodCauseDeniedReasonId === Utilities.idByFieldDataName('XX - Reason Does Not Meet Good Cause Policy', dropDown)) {
      return true;
    } else if (this.goodCauseGrantedReasonId === Utilities.idByFieldDataName(`ZZ - Other Circumstances Beyond Participant's Control`, dropDown)) {
      return true;
    } else {
      return false;
    }
  }

  public hoursParticipatedValidation(result, validationManager: ValidationManager) {
    const hrsValidationContext: HrsValidationContext = {
      hrs: this.reportedHours,
      result: result,
      validationManager: validationManager,
      prop: 'reportedHours',
      prettyProp: 'Hours Participated',
      isRequired: true,
      canEnterZero: true
    };
    Utilities.validateHrs(hrsValidationContext);
    if (this.reportedHours !== null || this.totalMakeupHours !== null) {
      if (+this.reportedHours > +this.scheduledHours) {
        validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, 'The total Hours Participated and Make-Up Hours cannot be more than the Scheduled Hours.');
        result.addError('reportedHours');
      }
      if (+this.totalMakeupHours + +this.reportedHours > +this.scheduledHours) {
        validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, 'The total Hours Participated and Make-Up Hours cannot be more than the Scheduled Hours.');
        result.addError('reportedHours');
      }
    }
    if (this.makeUpEntries != null) {
      const errArr = result.createErrorsArray('makeUpEntries');
      for (const pmh of this.makeUpEntries) {
        const v = pmh.validate(validationManager, this.participationDate);
        errArr.push(v.errors);
        if (v.isValid === false) {
          result.isValid = false;
        }
      }
    }
  }
  public validateHrsParticipatedSection(validationManager: ValidationManager) {
    const result = new ValidationResult();
    this.hoursParticipatedValidation(result, validationManager);
    return result;
  }
  public cleanseModelForSave() {
    if (this.nonParticipatedHours === null || this.nonParticipatedHours.trim() === '') {
      this.nonParticipationReasonId = null;
      this.nonParticipationReasonDetails = null;
      this.nonParticipationReasonDetails = null;
      this.goodCauseGranted = null;
      this.goodCauseGrantedReasonId = null;
      this.goodCauseReasonDetails = null;
      this.goodCauseDeniedReasonId = null;
      this.goodCausedHours = null;
    }
  }
  public validateGoodCauseReasonAndDetails(validationManager, result, goodCauseGrantedReasons) {
    if (!this.goodCauseGrantedReasonId) {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Good Cause Granted Reason');
      result.addError('goodCauseGrantedReasonId');
    }
    if (this.isgoodCauseReasonsDetailsRequired(goodCauseGrantedReasons) && (this.goodCauseReasonDetails === null || this.goodCauseReasonDetails.trim() === '')) {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Good Cause Reason Details');
      result.addError('goodCauseReasonDetails');
    }
  }
  public validateGoodCauseDeniedReasonAndDetails(validationManager, result, goodCauseDeniedReasons) {
    if (!this.goodCauseDeniedReasonId) {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Good Cause Denied Reason');
      result.addError('goodCauseDeniedReasonId');
    }
    if (this.isgoodCauseReasonsDetailsRequired(goodCauseDeniedReasons) && (this.goodCauseReasonDetails === null || this.goodCauseReasonDetails.trim() === '')) {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Good Cause Reason Details');
      result.addError('goodCauseReasonDetails');
    }
  }
  public validateGoodCauseHrs(validationManager, result) {
    const goodCauseHrsContext: HrsValidationContext = {
      hrs: this.goodCausedHours,
      result: result,
      validationManager: validationManager,
      prop: 'goodCausedHours',
      prettyProp: 'Good Cause Hours',
      isRequired: true,
      canEnterZero: false
    };
    Utilities.validateHrs(goodCauseHrsContext);
  }

  public validate(
    validationManager: ValidationManager,
    nonParticipationReasons: DropDownField[],
    goodCauseGrantedReasons: DropDownField[],
    goodCauseDeniedReasons: DropDownField[]
  ): ValidationResult {
    const result = new ValidationResult();
    if (+this.nonParticipatedHours > 0) {
      if (
        this.isnonParticipationReasonsDetailsRequired(nonParticipationReasons) &&
        (this.nonParticipationReasonDetails === null || this.nonParticipationReasonDetails.trim() === '')
      ) {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Non-Participation Reason Details');
        result.addError('nonParticipationReasonDetails');
      }
      if (this.nonParticipationReasonId !== null) {
        if (this.goodCauseGranted === true) {
          this.validateGoodCauseReasonAndDetails(validationManager, result, goodCauseGrantedReasons);
          this.validateGoodCauseHrs(validationManager, result);
          this.goodCauseDeniedReasonId = null;
          this.goodCauseDeniedReasonName = null;
          if (!result.errors.goodCausedHours) {
            if (+this.goodCausedHours > +this.nonParticipatedHours) {
              validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, 'The number of Good Cause Hours cannot be more than Non-Participation Hours.');
              result.addError('goodCausedHours');
            }
          }
        } else if (this.goodCauseGranted === false) {
          this.validateGoodCauseDeniedReasonAndDetails(validationManager, result, goodCauseDeniedReasons);
          this.goodCauseGrantedReasonId = null;
          this.goodCauseGrantedReasonName = null;
          this.goodCausedHours = null;
        } else {
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Good Cause Granted');
          result.addError('goodCauseGranted');
        }
      } else {
        this.goodCauseGranted = null;
        this.goodCauseGrantedReasonId = null;
        this.goodCauseDeniedReasonId = null;
        this.goodCauseDeniedReasonName = null;
        this.goodCauseGrantedReasonName = null;
      }
    }
    this.cleanseModelForSave();
    return result;
  }
}
