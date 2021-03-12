import { Serializable } from '../interfaces/serializable';
import { ValidationCode, ValidationManager, ValidationResult } from './validation';
import * as moment from 'moment';
import { Utilities } from '../utilities';
import { Clearable } from '../interfaces/clearable';
import { IsEmpty } from '../interfaces/is-empty';

export class JRWorkPreferences implements Clearable, IsEmpty, Serializable<JRWorkPreferences> {
  id: number;
  kindOfJobDetails: string;
  jobInterestDetails: string;
  trainingNeededForJobDetails: string;
  someOtherPlacesJobAvailableDetails: string;
  someOtherPlacesJobAvailableUnknown: boolean;
  situationsToAvoidDetails: string;
  beginHour: number;
  beginMinute: number;
  beginAmPm: number;
  endHour: number;
  endMinute: number;
  endAmPm: number;
  workScheduleDetails: string;
  travelTimeToWork: string;
  distanceHomeToWork: string;
  workShiftIds: number[];
  workShiftNames: string[];

  public static clone(input: any, instance: JRWorkPreferences) {
    instance.id = input.id;
    instance.kindOfJobDetails = input.kindOfJobDetails;
    instance.jobInterestDetails = input.jobInterestDetails;
    instance.trainingNeededForJobDetails = input.trainingNeededForJobDetails;
    instance.someOtherPlacesJobAvailableDetails = input.someOtherPlacesJobAvailableDetails;
    instance.someOtherPlacesJobAvailableUnknown = input.someOtherPlacesJobAvailableUnknown;
    instance.situationsToAvoidDetails = input.situationsToAvoidDetails;
    instance.beginHour = input.beginHour;
    instance.beginMinute = input.beginMinute;
    instance.beginAmPm = input.beginAmPm;
    instance.endHour = input.endHour;
    instance.endMinute = input.endMinute;
    instance.endAmPm = input.endAmPm;
    instance.workScheduleDetails = input.workScheduleDetails;
    instance.travelTimeToWork = input.travelTimeToWork;
    instance.distanceHomeToWork = input.distanceHomeToWork;
    instance.workShiftIds = Utilities.deserilizeArray(input.workShiftIds);
    instance.workShiftNames = Utilities.deserilizeArray(input.workShiftNames);
  }
  public static create(): JRWorkPreferences {
    const x = new JRWorkPreferences();
    x.id = 0;
    return x;
  }

  public clear(): void {
    this.id = null;
    // this.details = null;
  }

  public isEmpty(): boolean {
    // All properties have to be null or blank to make the entire object empty.
    return this.id == null;
  }

  public isBeginTimeRequired() {
    return !!this.beginHour || !!this.beginMinute || this.beginMinute === 0 || !!this.beginAmPm;
  }

  public isEndTimeRequired() {
    return !!this.endHour || !!this.endMinute || this.endMinute === 0 || !!this.endAmPm;
  }

  validate(validationManager: ValidationManager): ValidationResult {
    const result = new ValidationResult();
    if (this.kindOfJobDetails === null || this.kindOfJobDetails === undefined) {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Kind of Job');
      result.addError('kindOfJobDetails');
    }
    if (this.jobInterestDetails === null || this.jobInterestDetails === undefined) {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Job Interests');
      result.addError('jobInterestDetails');
    }
    if (this.trainingNeededForJobDetails === null || this.trainingNeededForJobDetails === undefined) {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Training Needed');
      result.addError('trainingNeededForJobDetails');
    }
    if (this.someOtherPlacesJobAvailableUnknown === false) {
      if (this.someOtherPlacesJobAvailableDetails === null || this.someOtherPlacesJobAvailableDetails === undefined) {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Other Places Job Available');
        result.addError('someOtherPlacesJobAvailableDetails');
      }
    } else if (this.someOtherPlacesJobAvailableDetails === null || this.someOtherPlacesJobAvailableDetails === undefined) {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Places Job Available Unkown');
      result.addError('someOtherPlacesJobAvailableUnknown');
    }
    if (this.situationsToAvoidDetails === null || this.situationsToAvoidDetails === undefined) {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Training Needed');
      result.addError('situationsToAvoidDetails');
    }

    if (this.travelTimeToWork === null || this.travelTimeToWork === undefined) {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Travel Time');
      result.addError('travelTimeToWork');
    }
    if (this.distanceHomeToWork === null || this.distanceHomeToWork === undefined) {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Distance Home to Work');
      result.addError('distanceHomeToWork');
    }

    return result;
  }

  public deserialize(input: any) {
    JRWorkPreferences.clone(input, this);
    return this;
  }
}
