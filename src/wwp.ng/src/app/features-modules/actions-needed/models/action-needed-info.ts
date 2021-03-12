import { Serializable } from '../../../shared/interfaces/serializable';

export class ActionNeededInfo implements Serializable<ActionNeededInfo> {
  actionNeededTypes: number[];
  assistDetails: string;
  cachedOriginal: ActionNeededInfo;

  private static graphObj(input: any, instance: ActionNeededInfo) {
    if (input != null) {
      instance.actionNeededTypes = input.actionNeededTypes;
      instance.assistDetails = input.assistDetails;
    }
  }

  deserialize(input: any) {
    ActionNeededInfo.graphObj(input, this);
    this.cachedOriginal = new ActionNeededInfo();
    ActionNeededInfo.graphObj(input, this.cachedOriginal);
    return this;
  }
}
