import { Serializable } from '../interfaces/serializable';
import { Utilities } from '../utilities';

// TODO: Not liking the name of this object. Currently based on database name.
export class JobActionType implements Serializable<JobActionType> {
  jobActionTypes: number[];
  jobActionNames: string[];

  static create() {
    const obj = new JobActionType();
    obj.jobActionTypes = [];
    obj.jobActionNames = [];
    return obj;
  }

  deserialize(input: any) {
    // this.jobActionTypes = Utilities.deserilizeArray(input.jobActionTypes);
    // this.jobActionNames = Utilities.deserilizeArray(input.jobActionNames);

    this.jobActionTypes = [];
    this.jobActionNames = [];

    if (input.jobActionTypes == null || input.jobActionNames == null) {
      return this;
    }

    for (const ant of input.jobActionTypes) {
      if (ant != null) {
        this.jobActionTypes.push(+ant);
      }
    }

    for (const ann of input.jobActionNames) {
      if (ann != null) {
        this.jobActionNames.push(ann);
      }
    }
    return this;
  }
}

// TODO: Not liking the name of this object. Currently based on database name.
export class BarrierSubType implements Serializable<BarrierSubType> {
  barrierSubTypes: number[];
  barrierSubTypeNames: string[];

  static clone(input: any, instance: BarrierSubType) {
    instance.barrierSubTypes = Utilities.deserilizeArray(input.barrierSubTypes);
    instance.barrierSubTypeNames = Utilities.deserilizeArray(input.barrierSubTypeNames);

    return instance;
  }

  deserialize(input: any) {
    this.barrierSubTypes = [];
    if (input.barrierSubTypes != null) {
      for (const bst of input.barrierSubTypes) {
        if (!bst.hasOwnProperty('NULL')) {
          this.barrierSubTypes.push(bst);
        }
      }
    }

    this.barrierSubTypeNames = [];
    if (input.barrierSubTypeNames != null) {
      for (const bsn of input.barrierSubTypeNames) {
        if (!bsn.hasOwnProperty('NULL')) {
          this.barrierSubTypeNames.push(bsn);
        }
      }
    }

    return this;
  }
}
