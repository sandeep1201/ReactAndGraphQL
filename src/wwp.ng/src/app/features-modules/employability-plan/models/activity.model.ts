import { DropDownMultiField } from './../../../shared/models/dropdown-multi-field';
import { Serializable } from '../../../shared/interfaces/serializable';
import * as moment from 'moment';
import { ValidationManager } from '../../../shared/models/validation-manager';
import { ValidationResult } from '../../../shared/models/validation-result';
import { Utilities } from '../../../shared/utilities';
import { ValidationCode } from '../../../shared/models/validation';
import { NonSelfDirectedActivity } from '../../../shared/models/nonSelfDirectedActivity.model';
import { ActivitySchedule } from './activity-schedule.model';
import { DropDownField } from '../../../shared/models/dropdown-field';
import { WhyReason } from '../../../shared/models/why-reasons.model';
import { EnrolledProgramCode } from '../../../shared/enums/enrolled-program-code.enum';
import { EmployabilityPlanService } from '../services/employability-plan.service';
import { EmployabilityPlan } from './employability-plan.model';

export class Activity implements Serializable<Activity> {
  id: number;
  employabilityPlanId: number;
  activityTypeId: number;
  activityTypeName: string;
  activityTypeCode: string;
  description: string;
  activityLocationName: string;
  activityLocationId: number;
  nonSelfDirectedActivity: NonSelfDirectedActivity;
  contacts: number[];
  additionalInformation: string;
  tempCompletionReasonId: any;
  activitySchedules: ActivitySchedule[];

  minDate: string;
  employabilityPlan: EmployabilityPlan;
  endDate: string;
  isCarriedOver: boolean;
  activityCompletionReasonId: number;
  activityCompletionReasonName: string;
  activityCompletionReasonCode: string;

  rowVersion: string;
  modifiedBy: string;
  modifiedDate: string;
  minStartDate: string;
  maxEndDate: string;
  UEId: string;
  URId: string;
  UCId: string;
  program: string;
  endActivity: boolean;
  public upfrontActivityExists: boolean;
  public onlyUpfrontActivities: boolean;
  private employabilityPlanService: EmployabilityPlanService;
  private activityForCalculatingDates: Activity[];
  public static create(): Activity {
    const x = new Activity();
    x.id = 0;
    return x;
  }

  public static clone(input: any, instance: Activity) {
    instance.id = input.id;
    instance.employabilityPlanId = input.employabilityPlanId;
    instance.activityTypeId = input.activityTypeId;
    instance.activityTypeName = input.activityTypeName;
    instance.activityTypeCode = input.activityTypeCode;
    instance.description = input.description;
    instance.activityLocationId = input.activityLocationId;
    if (input.nonSelfDirectedActivity != null) {
      instance.nonSelfDirectedActivity = new NonSelfDirectedActivity().deserialize(input.nonSelfDirectedActivity);
    }

    instance.contacts = Utilities.deserilizeArray(input.contacts);
    instance.additionalInformation = input.additionalInformation;
    instance.activityCompletionReasonId = input.activityCompletionReasonId;
    instance.activityCompletionReasonName = input.activityCompletionReasonName;
    instance.activityCompletionReasonCode = input.activityCompletionReasonCode;
    instance.endDate = input.endDate;
    instance.isCarriedOver = input.isCarriedOver;
    instance.rowVersion = input.rowVersion;
    instance.modifiedBy = input.modifiedBy;
    instance.modifiedDate = input.modifiedDate;
    instance.activitySchedules = Utilities.deserilizeChildren(input.activitySchedules, ActivitySchedule, 0);
    instance.minStartDate = input.minStartDate;
    instance.maxEndDate = input.maxEndDate;
    instance.program = input.program;
    instance.endActivity = input.endActivity;
  }

  public deserialize(input: any) {
    Activity.clone(input, this);
    return this;
  }

  public clear(): void {
    this.id = null;
    this.employabilityPlanId = null;
  }

  public isEmpty(): boolean {
    // All properties have to be null or blank to make the entire object empty.
    return this.id == null && this.employabilityPlanId == null;
  }
  public anyUpfrontActivities(activities, modelIds, enrolledProgramCode) {
    if (activities && activities.some(activity => !activity.activityCompletionReasonId && this.upfrontActivity(activity, modelIds, enrolledProgramCode))) {
      this.upfrontActivityExists = true;
    }
  }
  public onlyUpfrontActivitiesCheck(activities, modelIds, enrolledProgramCode) {
    if (activities && activities.every(activity => !activity.activityCompletionReasonId && this.upfrontActivity(activity, modelIds, enrolledProgramCode))) {
      this.onlyUpfrontActivities = true;
    }
  }
  public openActivities(activities: Activity[]) {
    return activities.some(activity => !activity.activityCompletionReasonId);
  }
  public upfrontActivity(activity: Activity, modelIds: any, enrolledProgramCode) {
    return (
      (enrolledProgramCode.trim().toLocaleLowerCase() === (EnrolledProgramCode.ww || EnrolledProgramCode.w2) && activity.activityTypeId === modelIds.UEId) ||
      activity.activityTypeId === modelIds.URId ||
      activity.activityTypeId === modelIds.UCId
    );
  }

  precheck(activities, modelIds): WhyReason {
    const res = new WhyReason();
    if (
      activities &&
      this.openActivities(activities) &&
      !this.onlyUpfrontActivities &&
      (this.activityTypeId === modelIds.UCId || this.activityTypeId === modelIds.UEId || this.activityTypeId === modelIds.URId)
    ) {
      res.status = false;
      res.errors = ['Up-front activity assignment is not allowed, participant has/had other activities assigned during their current enrollment.'];
      return res;
    }
    if (this.upfrontActivityExists && !(this.activityTypeId === modelIds.UCId || this.activityTypeId === modelIds.UEId || this.activityTypeId === modelIds.URId)) {
      res.status = false;
      res.errors = ['All Up-front activities must be ended before assigning other activities.'];
      return res;
    }
  }

  validate(
    validationManager: ValidationManager,
    modelIds?: any,
    subsequenEp?: boolean,
    initialEp?: boolean,
    epBeginDate?: string,
    showEndActivityFields?: boolean,
    hours?: DropDownField[],
    minutes?: DropDownField[],
    activityForDateCalculation?: any,
    endEp?: boolean,
    maxDaysCanBackDate?: string,
    pullDownDates?: DropDownMultiField[],
    enrolledProgramCd?: string
  ): ValidationResult {
    const result = new ValidationResult();
    if (!endEp) {
      if (this.activityTypeId == null || this.activityTypeId.toString() === '') {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Activity');
        result.addError('activityTypeId');
      }
      if (this.description == null || this.description.toString() === '') {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Activity Description');
        result.addError('description');
      }
      if (this.activityLocationId == null || this.activityLocationId.toString() === '') {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Activity Location');
        result.addError('activityLocationId');
      }
      if (this.activityLocationId != null) {
        if ((this.activityLocationId === modelIds.onSiteId || this.activityLocationId === modelIds.offSiteId) && this.nonSelfDirectedActivity) {
          const se = result.addErrorContainingObject('nonSelfDirectedActivity');
          if (!this.nonSelfDirectedActivity.businessName || this.nonSelfDirectedActivity.businessName == null || this.nonSelfDirectedActivity.businessName.toString() === '') {
            validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Business Name');
            result.addErrorForContainingObject(se, 'businessName');
          }
        }
      }

      const errArr = result.createErrorsArray('activitySchedule');
      if (this.activitySchedules) {
        let v: any;
        this.activitySchedules.forEach(obj => {
          v = obj.validate(validationManager, hours, minutes, epBeginDate, this.isCarriedOver);
          errArr.push(v.errors);
          if (v.isValid === false) {
            result.isValid = false;
          }
        });
        if (v.isValid && this.id === 0) {
          /**** ****/
          //FInd a spot to put the logic below so that it doesn't exist in the model
          /**** ****/
          let setMaxDateFlag: Boolean;
          let scheduleStartDates = [];
          // Push all schedule start dates to an aray
          activityForDateCalculation.activitySchedules.forEach(activitySchedule => {
            scheduleStartDates.push(new Date(activitySchedule.scheduleStartDate));
          });
          // Get the max start date from the array
          activityForDateCalculation.minStartDate = moment(new Date(Math.min.apply(null, scheduleStartDates)).toString()).format('MM/DD/YYYY');
          // If the activity has not ended (Activity doesn't have a completion reason)
          let scheduleEndDates = [];
          if (!activityForDateCalculation.activityCompletionReasonId) {
            // If every schedule in the activity is recurring
            if (activityForDateCalculation.activitySchedules.every(schedule => schedule.isRecurring === true)) {
              // Push all the schedule end date to an array
              activityForDateCalculation.activitySchedules.forEach(schedule => {
                scheduleEndDates.push(new Date(schedule.scheduleEndDate));
              });
            }
            // If all the schedules are non-recurring in an activity
            else if (activityForDateCalculation.activitySchedules.every(schedule => schedule.isRecurring === false)) {
              // Push all schedule start dates to an array
              activityForDateCalculation.activitySchedules.forEach(schedule => {
                scheduleEndDates.push(new Date(schedule.scheduleStartDate));
              });
            }
            // If the activity has a combination fo recurring and non-recurring schedules
            else {
              scheduleEndDates = [];
              scheduleStartDates = [];
              // Set a flag
              setMaxDateFlag = true;
              activityForDateCalculation.activitySchedules.forEach(schedule => {
                scheduleStartDates.push(new Date(schedule.scheduleStartDate));
                // Push all schedule end dates to array
                if (schedule.scheduleEndDate !== null) scheduleEndDates.push(new Date(schedule.scheduleEndDate));
              });
            }
            // If flag set go into this logic to calculate max date from all the start dates and end dates of all the schedules in an activity
            if (setMaxDateFlag) {
              activityForDateCalculation.maxEndDate = moment(new Date(Math.max.apply(null, scheduleEndDates)).toString()).format('MM/DD/YYYY');
              const maxStartDate = moment(new Date(Math.max.apply(null, scheduleStartDates)).toString()).format('MM/DD/YYYY');
              activityForDateCalculation.maxEndDate = moment.max(moment(maxStartDate), moment(activityForDateCalculation.maxEndDate)).format('MM/DD/YYYY');
            }
            // Else just calculate the max end date of the schedules
            else {
              activityForDateCalculation.maxEndDate = moment(new Date(Math.max.apply(null, scheduleEndDates)).toString()).format('MM/DD/YYYY');
            }
          }
        }
      }
    }
    if (showEndActivityFields) {
      const formatEpBeginDateToMomentL = moment(epBeginDate).format('L');
      const formatEpBeginDate = moment(formatEpBeginDateToMomentL);
      const formatendDate = moment(this.endDate, 'MM/DD/YYYY');
      const checkEndDate = moment(this.endDate).format('MM/DD/YYYY');
      const formatMinStartDate = moment(this.minStartDate, 'MM/DD/YYYY');
      const checkMinStartDate = moment(this.minStartDate).format('MM/DD/YYYY');
      let compareDate = null;
      if (maxDaysCanBackDate) compareDate = moment(Utilities.currentDate.subtract(+maxDaysCanBackDate, 'days').format('MM/DD/YYYY'));
      if ((this.endActivity || !Utilities.isNumberEmptyOrNull(this.activityCompletionReasonId)) && (this.endDate === null || this.endDate === undefined || this.endDate === '')) {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Actual End Date');
        result.addError('endDate');
      }
      if (this.endDate !== null && this.endDate !== undefined && this.endDate !== '' && this.endDate.length !== 10) {
        validationManager.addErrorWithFormat(ValidationCode.ValueInInvalidFormat_Name_FormatType, 'Actual End Date', 'MM/DD/YYYY');
        result.addError('endDate');
      } else if (this.endDate !== null && formatendDate.isValid() && formatEpBeginDate.isValid() && checkEndDate !== 'Invalid date' && checkMinStartDate !== 'Invalid date') {
        if (formatendDate.isAfter(moment(Utilities.currentDate))) {
          validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, 'Actual End Date must be on or prior to Current Date.');
          result.addError('endDate');
        }
        if (this.isCarriedOver) {
          if (formatendDate.isBefore(moment(formatEpBeginDate).subtract(1, 'days'))) {
            validationManager.addErrorWithDetail(
              ValidationCode.ValueOutOfRange_Details,
              'Actual End Date can be one (1) day prior to the EP’s Begin Date, up to the current date.'
            );
            result.addError('endDate');
          }
        } else if (formatendDate.isBefore(formatMinStartDate)) {
          validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, 'Actual End Date must be on or after Activity Start Date.');
          result.addError('endDate');
        } else if (!endEp && compareDate && formatendDate.isBefore(compareDate)) {
          validationManager.addErrorWithFormat(ValidationCode.DateBeforeMaxDaysCanBackDate, 'Actual End Date', maxDaysCanBackDate);
          result.addError('endDate');
        }
        if (
          endEp &&
          enrolledProgramCd &&
          enrolledProgramCd.trim().toLowerCase() === EnrolledProgramCode.ww &&
          (endEp || this.isCarriedOver) &&
          Utilities.participationPeriodCheck(pullDownDates, formatendDate) &&
          this.maxEndDate &&
          formatendDate.isBefore(this.maxEndDate)
        ) {
          validationManager.addErrorWithFormat(ValidationCode.ParticipationPeriodCheck, 'Actual End', 'last', 'prior');
          result.addError('endDate');
        }
      }

      if (this.endDate !== null && this.endDate !== undefined && this.endDate !== '' && this.endDate.length !== 10) {
        validationManager.addErrorWithFormat(ValidationCode.ValueInInvalidFormat_Name_FormatType, 'Actual End Date', 'MM/DD/YYYY');
        result.addError('endDate');
      }
      // When date is not null, not undefined, not empty but date is invalid throw the following error message

      if (this.endDate !== null && this.endDate !== undefined && this.endDate !== '' && !formatendDate.isValid()) {
        validationManager.addErrorWithDetail(ValidationCode.InvalidDate, 'Invalid Actual End Date Entered.');
        result.addError('endDate');
      }

      if (this.endActivity && this.activityCompletionReasonId === null) {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Completion Reason');
        result.addError('activityCompletionReasonId');
      }
    }
    return result;
  }
}
