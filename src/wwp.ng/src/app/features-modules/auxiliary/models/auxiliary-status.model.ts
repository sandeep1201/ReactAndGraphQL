export class AuxiliaryStatus {
  id: number;
  auxiliaryStatusTypeId: number;
  auxiliaryStatusName: string;
  auxiliaryStatusDisplayName: string;
  auxiliaryStatusDate: string;
  details: string;
  modifiedBy: string;
  modifiedDate: string;

  public static clone(input: any, instance: AuxiliaryStatus) {
    instance.id = input.id;
    instance.auxiliaryStatusTypeId = input.auxiliaryStatusTypeId;
    instance.auxiliaryStatusName = input.auxiliaryStatusName;
    instance.auxiliaryStatusDisplayName = input.auxiliaryStatusDisplayName;
    instance.auxiliaryStatusDate = input.auxiliaryStatusDate;
    instance.details = input.details;
    instance.modifiedBy = input.modifiedBy;
    instance.modifiedDate = input.modifiedDate;
  }

  public deserialize(input: any) {
    AuxiliaryStatus.clone(input, this);
    return this;
  }
}
