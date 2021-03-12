import { GoogleLocation } from '../../../shared/models/google-location';
import { Serializable } from '../../../shared/interfaces/serializable';

export class EpEmployment implements Serializable<EpEmployment> {
  modifiedBy: string;
  modifiedDate: string;
  modifiedByName: string;
  id: number;
  employmentInformationId: number;
  jobTypeId: number;
  jobTypeName: string;
  jobBeginDate: string;
  jobEndDate: string;
  jobPosition: string;
  companyName: string;
  location: GoogleLocation;
  wageHour: number;
  isSelected: boolean;

  // Front end only Properties

  public static clone(input: any, instance: EpEmployment) {
    instance.modifiedBy = input.modifiedBy;
    instance.modifiedByName = input.modifiedByName;
    instance.modifiedDate = input.modifiedDate;
    instance.id = input.id;
    instance.employmentInformationId = input.employmentInformationId;
    instance.jobTypeId = input.jobTypeId;
    instance.jobTypeName = input.jobTypeName;
    instance.jobBeginDate = input.jobBeginDate;
    instance.jobEndDate = input.jobEndDate;
    instance.jobPosition = input.jobPosition;
    instance.companyName = input.companyName;
    instance.location = new GoogleLocation().deserialize(input.location);
    instance.wageHour = input.wageHour;
    instance.isSelected = input.isSelected;
    return instance;
  }

  public deserialize(input: any) {
    EpEmployment.clone(input, this);
    return this;
  }
}
