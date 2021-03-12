export class DrugScreeningStatus {
  id: number;
  drugScreeningStatusTypeId: number;
  drugScreeningStatusName: string;
  drugScreeningStatusDisplayName: string;
  drugScreeningStatusDate: string;
  details: string;
  modifiedBy: string;
  modifiedDate: string;

  public static clone(input: any, instance: DrugScreeningStatus) {
    instance.id = input.id;
    instance.drugScreeningStatusTypeId = input.StatusTypeId;
    instance.drugScreeningStatusName = input.drugScreeningStatusName;
    instance.drugScreeningStatusDisplayName = input.drugScreeningStatusDisplayName;
    instance.drugScreeningStatusDate = input.drugScreeningStatusDate;
    instance.details = input.details;
    instance.modifiedBy = input.modifiedBy;
    instance.modifiedDate = input.modifiedDate;
  }

  public deserialize(input: any) {
    DrugScreeningStatus.clone(input, this);
    return this;
  }
}
