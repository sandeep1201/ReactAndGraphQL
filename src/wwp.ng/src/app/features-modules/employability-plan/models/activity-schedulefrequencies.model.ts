import { Serializable } from '../../../shared/interfaces/serializable';
export class ActivityScheduleFrequency implements Serializable<ActivityScheduleFrequency> {
  public id: number;
  public activityScheduleId: number;
  public wkFrequencyId: number;
  public wkFrequencyName: string;
  public mrFrequencyId: any;
  public mrFrequencyName: string;
  public shortName: string;

  public static create(): ActivityScheduleFrequency {
    const x = new ActivityScheduleFrequency();
    x.id = 0;
    return x;
  }
  public static clone(input: any, instance: ActivityScheduleFrequency) {
    instance.id = input.id;
    instance.activityScheduleId = input.activityScheduleId;
    instance.wkFrequencyId = input.wkFrequencyId;
    instance.wkFrequencyName = input.wkFrequencyName;
    instance.mrFrequencyId = input.mrFrequencyId;
    instance.mrFrequencyName = input.mrFrequencyName;
    instance.shortName = input.shortName;
  }
  public deserialize(input: any) {
    ActivityScheduleFrequency.clone(input, this);
    return this;
  }

  constructor() {}
}
