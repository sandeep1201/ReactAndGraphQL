import * as moment from 'moment';
import { FinalistAddress } from 'src/app/shared/models/finalist-address.model';
import { Utilities } from 'src/app/shared/utilities';

export class TJTMJEmploymentVerificationModel {
  public id: number;
  public participantId: number;
  public jobTypeId: number;
  public jobTypeName: string;
  public jobBeginDate: string;
  public jobEndDate: string;
  public jobPosition: string;
  public avgWeeklyHours: string;
  public companyName: string;
  public location: FinalistAddress;
  public agencyName: string;
  public employmentVerificationId: number;
  public isVerified: boolean;
  public modifiedBy: string;
  public modifiedDate: string;
  public createdDate: string;
  public isDisabled: boolean;
  public calculatedEmploymentDays: number;
  public numberOfDaysAtVerification: number;

  public static clone(input: any, instance: TJTMJEmploymentVerificationModel) {
    instance.id = input.id;
    instance.participantId = input.participantId;
    instance.jobTypeId = input.jobTypeId;
    instance.jobTypeName = input.jobTypeName;
    instance.jobBeginDate = input.jobBeginDate ? moment(input.jobBeginDate).format('MM/DD/YYYY') : input.jobBeginDate;
    instance.jobEndDate = input.jobEndDate ? moment(input.jobEndDate).format('MM/DD/YYYY') : input.jobEndDate;
    instance.jobPosition = input.jobPosition;
    instance.avgWeeklyHours = input.avgWeeklyHours;
    instance.companyName = input.companyName;
    instance.location = Utilities.deserilizeChild(input.location, FinalistAddress);
    instance.agencyName = input.agencyName;
    instance.employmentVerificationId = input.employmentVerificationId;
    instance.isVerified = input.isVerified;
    instance.modifiedBy = input.modifiedBy;
    instance.modifiedDate = input.modifiedDate;
    instance.createdDate = input.createdDate;
    instance.isDisabled = input.createdDate ? Utilities.currentDate.clone().isSameOrAfter(moment(input.createdDate).add(30, 'days')) : false;
    instance.numberOfDaysAtVerification = input.numberOfDaysAtVerification;
    instance.calculatedEmploymentDays = Math.abs(moment(instance.jobBeginDate).diff(input.jobEndDate ? instance.jobEndDate : Utilities.currentDate.clone(), 'days'));
  }

  public deserialize(input: any) {
    TJTMJEmploymentVerificationModel.clone(input, this);
    return this;
  }
}
