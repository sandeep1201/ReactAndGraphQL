export class POPClaimStatus {
  id: number;
  popClaimStatusTypeId: number;
  popClaimStatusName: string;
  popClaimStatusDisplayName: string;
  popClaimStatusDate: string;
  details: string;
  modifiedBy: string;
  modifiedDate: string;

  public static clone(input: any, instance: POPClaimStatus) {
    instance.id = input.id;
    instance.popClaimStatusTypeId = input.popClaimStatusTypeId;
    instance.popClaimStatusName = input.popClaimStatusName;
    instance.popClaimStatusDisplayName = input.popClaimStatusDisplayName;
    instance.popClaimStatusDate = input.popClaimStatusDate;
    instance.details = input.details;
    instance.modifiedBy = input.modifiedBy;
    instance.modifiedDate = input.modifiedDate;
  }

  public deserialize(input: any) {
    POPClaimStatus.clone(input, this);
    return this;
  }
}
