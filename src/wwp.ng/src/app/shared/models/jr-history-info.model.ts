import { Serializable } from '../interfaces/serializable';
import { Clearable } from '../interfaces/clearable';
import { IsEmpty } from '../interfaces/is-empty';

export class JRHistoryInfo implements Clearable, IsEmpty, Serializable<JRHistoryInfo> {
  id: number;
  lastJobDetails: string;
  accomplishmentDetails: string;
  strengthDetails: string;
  areasNeedImprove: string;

  public static clone(input: any, instance: JRHistoryInfo) {
    instance.id = input.id;
    instance.lastJobDetails = input.lastJobDetails;
    instance.accomplishmentDetails = input.accomplishmentDetails;
    instance.strengthDetails = input.strengthDetails;
    instance.areasNeedImprove = input.areasNeedImprove;
  }

  public static create(): JRHistoryInfo {
    const x = new JRHistoryInfo();
    x.id = 0;
    return x;
  }
  public deserialize(input: any) {
    JRHistoryInfo.clone(input, this);
    return this;
  }
  public clear(): void {
    this.id = null;
    // this.details = null;
  }

  public isEmpty(): boolean {
    // All properties have to be null or blank to make the entire object empty.
    return this.id == null;
  }
}
