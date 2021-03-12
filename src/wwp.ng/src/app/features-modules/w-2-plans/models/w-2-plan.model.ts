import { Serializable } from 'src/app/shared/interfaces/serializable';
import { Utilities } from 'src/app/shared/utilities';

export class W2Plan implements Serializable<W2Plan> {
  id: number;
  planNumber: number;
  participantId: number;
  planTypeId: number;
  planTypeCode: string;
  planTypeName: string;
  planStatusTypeId: number;
  planStatusTypeCode: string;
  planStatusTypeName: string;
  organizationId: number;
  organizationCode: string;
  organizationName: string;
  modifiedBy: string;
  modifiedDate: string;
  planSections: W2PlanSection[];

  public static create() {
    const w2Plan = new W2Plan();
    w2Plan.planNumber = 0;
    // May need Implementation for all Sections
    return w2Plan;
  }

  public static clone(input: any, instance: W2Plan) {
    instance.id = input.id;
    instance.planNumber = input.planNumber;
    instance.participantId = input.participantId;
    instance.planTypeId = input.planTypeId;
    instance.planTypeCode = input.planTypeCode;
    instance.planTypeName = input.planTypeName;
    instance.planStatusTypeId = input.planStatusTypeId;
    instance.planStatusTypeCode = input.planStatusTypeCode;
    instance.planStatusTypeName = input.planStatusTypeName;
    instance.organizationId = input.organizationId;
    instance.organizationCode = input.organizationCode;
    instance.organizationName = input.organizationName;
    instance.modifiedBy = input.modifiedBy;
    instance.modifiedDate = input.modifiedDate;
    instance.planSections = Utilities.deserilizeChildren(input.planSections, W2PlanSection, 0);
  }

  public deserialize(input: any) {
    W2Plan.clone(input, this);
    return this;
  }
}

export class W2PlanSection implements Serializable<W2PlanSection> {
  id: number;
  planId: number;
  planTypeId: number;
  planSectionTypeId: number;
  planSectionTypeCode: string;
  planSectionTypeName: string;
  isNotNeeded: boolean;
  shortTermPlanOfAction: string;
  longTermPlanOfAction: string;
  modifiedBy: string;
  modifiedDate: string;
  planSectionResources: W2PlanSectionResource[];

  public static create() {
    const w2Plan = new W2PlanSection();
    w2Plan.planId = 0;
    w2Plan.planSectionResources = [new W2PlanSectionResource()];
    return w2Plan;
  }

  public static clone(input: any, instance: W2PlanSection) {
    instance.id = input.id;
    instance.planId = input.planId;
    instance.planTypeId = input.planTypeId;
    instance.planSectionTypeId = input.planSectionTypeId;
    instance.planSectionTypeCode = input.planSectionTypeCode;
    instance.planSectionTypeName = input.planSectionTypeName;
    instance.isNotNeeded = input.isNotNeeded;
    instance.shortTermPlanOfAction = input.shortTermPlanOfAction;
    instance.longTermPlanOfAction = input.longTermPlanOfAction;
    instance.modifiedBy = input.modifiedBy;
    instance.modifiedDate = input.modifiedDate;
    instance.planSectionResources = Utilities.deserilizeChildren(input.planSectionResources, W2PlanSectionResource, 0);
  }

  public deserialize(input: any) {
    W2PlanSection.clone(input, this);
    return this;
  }
}

export class W2PlanSectionResource implements Serializable<W2PlanSectionResource> {
  id: number;
  planSectionId: number;
  resource: string;
  modifiedBy: string;
  modifiedDate: string;

  public static create() {
    const w2Plan = new W2PlanSectionResource();
    w2Plan.planSectionId = 0;
    return w2Plan;
  }

  public static clone(input: any, instance: W2PlanSectionResource) {
    instance.id = input.id;
    instance.planSectionId = input.planSectionId;
    instance.resource = input.resource;
    instance.modifiedBy = input.modifiedBy;
    instance.modifiedDate = input.modifiedDate;
  }

  public clear(): void {
    this.planSectionId = null;
    this.resource = null;
    this.modifiedBy = null;
    this.modifiedDate = null;
  }

  public isEmpty(): boolean {
    // All properties have to be null or blank to make the entire object empty.
    return Utilities.isNumberEmptyOrNull(this.planSectionId) && Utilities.isStringEmptyOrNull(this.resource);
  }

  public deserialize(input: any) {
    W2PlanSectionResource.clone(input, this);
    return this;
  }
}
