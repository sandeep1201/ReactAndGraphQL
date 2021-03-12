import { Serializable } from './../../../shared/interfaces/serializable';
import { ValidationResult } from './../../../shared/models/validation-result';
import { ValidationManager, ValidationCode } from 'src/app/shared/models/validation';
import { DropDownField } from 'src/app/shared/models/dropdown-field';
import { Utilities } from 'src/app/shared/utilities';
import { ParticipationTracking } from 'src/app/shared/models/participation-tracking.model';

export class ChildrenFirstTracking extends ParticipationTracking implements Serializable<ChildrenFirstTracking> {
  id: number;
  epId: number;
  activityId: number;
  activityCd: string;
  activityName: string;
  participationDate: string;
  scheduledHours: string;
  didParticipate: boolean;
  nonParticipationReasonId: number;
  nonParticipationReasonName: string;
  nonParticipationReasonDetails: string;
  goodCauseGranted: boolean;
  goodCauseGrantedReasonId: number;
  goodCauseGrantedReasonName: string;
  goodCauseDeniedReasonId: number;
  goodCauseReasonDetails: string;
  modifiedBy: string;
  modifiedDate: string;
  canEditBasedOnDate: boolean;

  public static clone(input: any, instance: ChildrenFirstTracking) {
    instance.id = input.id;
    instance.epId = input.epId;
    instance.activityId = input.activityId;
    instance.activityCd = input.activityCd;
    instance.activityName = input.activityName;
    instance.participationDate = input.participationDate;
    instance.scheduledHours = input.scheduledHours;
    instance.didParticipate = input.didParticipate;
    instance.nonParticipationReasonId = input.nonParticipationReasonId;
    instance.nonParticipationReasonName = input.nonParticipationReasonName;
    instance.nonParticipationReasonDetails = input.nonParticipationReasonDetails;
    instance.goodCauseGranted = input.goodCauseGranted;
    instance.goodCauseGrantedReasonId = input.goodCauseGrantedReasonId;
    instance.goodCauseDeniedReasonId = input.goodCauseDeniedReasonId;
    instance.goodCauseReasonDetails = input.goodCauseReasonDetails;
    instance.canEditBasedOnDate = input.canEditBasedOnDate;
    instance.modifiedBy = input.modifiedBy;
    instance.modifiedDate = input.modifiedDate;
  }
  public deserialize(input: any) {
    ChildrenFirstTracking.clone(input, this);
    return this;
  }
  public cleanseModelForSave() {
    if (this.didParticipate === true) {
      this.nonParticipationReasonId = null;
      this.nonParticipationReasonName = null;
      this.nonParticipationReasonDetails = null;
      this.goodCauseGranted = null;
      this.goodCauseGrantedReasonId = null;
      this.goodCauseReasonDetails = null;
      this.goodCauseDeniedReasonId = null;
    }
  }

  public validate(
    validationManager: ValidationManager,
    nonParticipationReasons: DropDownField[],
    goodCauseGrantedReasons: DropDownField[],
    goodCauseDeniedReasons: DropDownField[]
  ): ValidationResult {
    const result = new ValidationResult();
    if (this.didParticipate === false) {
      if (
        this.isnonParticipationReasonsDetailsRequired(nonParticipationReasons) &&
        (this.nonParticipationReasonDetails === null || this.nonParticipationReasonDetails.trim() === '')
      ) {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Non Participation Reason Details.');
        result.addError('nonParticipationReasonDetails');
      }
      if (this.nonParticipationReasonId !== null) {
        if (this.goodCauseGranted === true) {
          this.validateGoodCauseReasonAndDetails(validationManager, result, goodCauseGrantedReasons);
          this.goodCauseDeniedReasonId = null;
          this.goodCauseDeniedReasonName = null;
          //this.validateGoodCauseHrs(validationManager, result);
        } else if (this.goodCauseGranted === false) {
          this.validateGoodCauseDeniedReasonAndDetails(validationManager, result, goodCauseDeniedReasons);
          this.goodCauseGrantedReasonId = null;
          this.goodCauseGrantedReasonName = null;
        } else {
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Good Cause Granted.');
          result.addError('goodCauseGranted');
        }
      }
    } else {
      this.cleanseModelForSave();
    }
    return result;
  }
}
