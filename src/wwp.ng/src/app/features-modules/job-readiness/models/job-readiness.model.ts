import { ActionNeeded } from '../../actions-needed/models/action-needed-new';
import { JRApplicationInfo } from '../../../shared/models/jr-Application-Info.model';
import { JRContactInfo } from '../../../shared/models/jr-contact-info.model';
import { JRHistoryInfo } from '../../../shared/models/jr-history-info.model';
import { JRInterviewInfo } from '../../../shared/models/jr-interview-info.model';
import { JRWorkPreferences } from '../../../shared/models/jr-work-preferences.model';
import { Serializable } from '../../../shared/interfaces/serializable';
import { Utilities } from '../../../shared/utilities';
import { ValidationResult, ValidationManager, ValidationCode } from '../../../shared/models/validation';

export class JobReadiness implements Serializable<JobReadiness> {
  id: number;
  jrApplicationInfo: JRApplicationInfo = new JRApplicationInfo();
  jrContactInfo: JRContactInfo = new JRContactInfo();
  jrHistoryInfo: JRHistoryInfo = new JRHistoryInfo();
  jrInterviewInfo: JRInterviewInfo = new JRInterviewInfo();
  jrWorkPreferences: JRWorkPreferences = new JRWorkPreferences();
  actionNeeded: ActionNeeded;
  createdDate: string;
  modifiedBy: string;
  modifiedDate: string;

  public static clone(input: any, instance: JobReadiness) {
    instance.id = input.id;
    instance.jrApplicationInfo = Utilities.deserilizeChild(input.jrApplicationInfo, JRApplicationInfo);
    instance.jrContactInfo = Utilities.deserilizeChild(input.jrContactInfo, JRContactInfo);
    instance.jrHistoryInfo = Utilities.deserilizeChild(input.jrHistoryInfo, JRHistoryInfo);
    instance.jrInterviewInfo = Utilities.deserilizeChild(input.jrInterviewInfo, JRInterviewInfo);
    instance.jrWorkPreferences = Utilities.deserilizeChild(input.jrWorkPreferences, JRWorkPreferences);
    instance.actionNeeded = Utilities.deserilizeChild(input.actionNeeded, ActionNeeded);
    instance.createdDate = input.createdDate;
    instance.modifiedBy = input.modifiedBy;
    instance.modifiedDate = input.modifiedDate;
  }

  public deserialize(input: any) {
    JobReadiness.clone(input, this);
    return this;
  }

  validate(validationManager: ValidationManager, actionRequired: boolean): ValidationResult {
    const result = new ValidationResult();

    if (!this.jrWorkPreferences.kindOfJobDetails) {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Kind of Job');
      result.addError('kindOfJobDetails');
    }
    if (!this.jrWorkPreferences.jobInterestDetails) {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Job Interests');
      result.addError('jobInterestDetails');
    }
    if (!this.jrWorkPreferences.trainingNeededForJobDetails) {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Kind of Training');
      result.addError('trainingNeededForJobDetails');
    }
    if (!this.jrWorkPreferences.someOtherPlacesJobAvailableUnknown && !this.jrWorkPreferences.someOtherPlacesJobAvailableDetails) {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Places Job Maybe Available');
      result.addError('someOtherPlacesJobAvailableDetails');
    }
    if (!this.jrWorkPreferences.situationsToAvoidDetails) {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Situations/Jobs to avoid');
      result.addError('situationsToAvoidDetails');
    }
    if (!this.jrWorkPreferences.workShiftIds || this.jrWorkPreferences.workShiftIds.length === 0) {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Work Shift(s)');
      result.addError('workShiftIds');
    }
    if (this.jrWorkPreferences.isBeginTimeRequired()) {
      if (!this.jrWorkPreferences.beginHour) {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Begin Time Hour');
        result.addError('beginHour');
      }
      if (!this.jrWorkPreferences.beginMinute && this.jrWorkPreferences.beginMinute !== 0) {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Begin Time Minutes');
        result.addError('beginMinute');
      }
      if (!this.jrWorkPreferences.beginAmPm) {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Begin Time AM/PM');
        result.addError('beginAmPm');
      }
    }
    if (this.jrWorkPreferences.isEndTimeRequired()) {
      if (!this.jrWorkPreferences.endHour) {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'End Time Hour');
        result.addError('endHour');
      }
      if (!this.jrWorkPreferences.endMinute && this.jrWorkPreferences.endMinute !== 0) {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'End Time Minutes');
        result.addError('endMinute');
      }
      if (!this.jrWorkPreferences.endAmPm) {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'End Time AM/PM');
        result.addError('endAmPm');
      }
    }
    if (!this.jrWorkPreferences.travelTimeToWork) {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Travel Time');
      result.addError('travelTimeToWork');
    }
    if (!this.jrWorkPreferences.distanceHomeToWork) {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Distance Home to Work');
      result.addError('distanceHomeToWork');
    }

    /**  History Questions Validation **/

    if (!this.jrHistoryInfo.lastJobDetails) {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Last Job');
      result.addError('lastJobDetails');
    }
    if (!this.jrHistoryInfo.accomplishmentDetails) {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Accomplishment');
      result.addError('accomplishmentDetails');
    }
    if (!this.jrHistoryInfo.strengthDetails) {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Strengths');
      result.addError('strengthDetails');
    }
    if (!this.jrHistoryInfo.areasNeedImprove) {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Areas To Improve');
      result.addError('areasNeedImprove');
    }

    /**  Application Questions Validation **/
    if (this.jrApplicationInfo.canSubmitOnline === null || this.jrApplicationInfo.canSubmitOnline === undefined) {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Can Submit Online');
      result.addError('canSubmitOnline');
    }
    if (this.jrApplicationInfo.haveCurrentResume === null || this.jrApplicationInfo.haveCurrentResume === undefined) {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Current Resume');
      result.addError('haveCurrentResume');
    }
    if (this.jrApplicationInfo.haveProfessionalReference === null || this.jrApplicationInfo.haveProfessionalReference === undefined) {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Professional Reference');
      result.addError('haveProfessionalReference');
    }
    if (!this.jrApplicationInfo.needDocumentLookupId) {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Documents Needed For Employment');
      result.addError('needDocumentLookupId');
    }

    /**  Interview Questions Validation **/
    if (!this.jrInterviewInfo.lastInterviewDetails) {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Last Interview Details');
      result.addError('lastInterviewDetails');
    }
    if (this.jrInterviewInfo.canLookAtSocialMedia === null || this.jrInterviewInfo.canLookAtSocialMedia === undefined) {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Social Media');
      result.addError('canLookAtSocialMedia');
    }
    if (this.jrInterviewInfo.haveOutfit === null || this.jrInterviewInfo.haveOutfit === undefined) {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Have Outfit');
      result.addError('haveOutfit');
    }

    /**  Contact Questions Validation  **/
    if (this.jrContactInfo.canYourPhoneNumberUsed === null || this.jrContactInfo.canYourPhoneNumberUsed === undefined) {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Phone Number');
      result.addError('canYourPhoneNumberUsed');
    }
    if (this.jrContactInfo.haveAccessToVoiceMailOrTextMessages === null || this.jrContactInfo.haveAccessToVoiceMailOrTextMessages === undefined) {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Voicemail');
      result.addError('haveAccessToVoiceMailOrTextMessages');
    }
    if (this.jrContactInfo.haveEmailAddress === null || this.jrContactInfo.haveEmailAddress === undefined) {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Email Address');
      result.addError('haveEmailAddress');
    }
    if (this.jrContactInfo.haveAccessDailyToEmail === null || this.jrContactInfo.haveAccessDailyToEmail === undefined) {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Have access to email');
      result.addError('haveAccessDailyToEmail');
    }

    // Action needed
    if (actionRequired) {
      const anResult = this.actionNeeded.validate(validationManager);

      if (anResult.isValid === false) {
        result.addError('actionNeeded');
      }
    }

    return result;
  }
}
