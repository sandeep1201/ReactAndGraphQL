import { Serializable } from '../interfaces/serializable';

export class FeatureToggle implements Serializable<FeatureToggle> {
  id: number;
  parameterName: string;
  parameterValue: string;
  modifiedDate: string;

  public static clone(input: any, instance: FeatureToggle) {
    instance.id = input.id;
    instance.parameterName = input.parameterName;
    instance.parameterValue = input.parameterValue;
    instance.modifiedDate = input.modifiedDate;
  }

  public deserialize(input: any) {
    FeatureToggle.clone(input, this);
    return this;
  }
}
