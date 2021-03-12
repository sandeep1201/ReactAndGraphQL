import { Serializable } from 'src/app/shared/interfaces/serializable';
import { Utilities } from 'src/app/shared/utilities';

export class ChildCareAuthorizations implements Serializable<ChildCareAuthorizations> {
  effectivePeriod: string;
  periods: Period[];

  public static clone(input: any, instance: ChildCareAuthorizations) {
    instance.effectivePeriod = input.effectivePeriod;
    instance.periods = Utilities.deserilizeChildren(input.periods, Period, 0);
  }

  public deserialize(input: any) {
    ChildCareAuthorizations.clone(input, this);
    return this;
  }
}

export class Period implements Serializable<Period> {
  month: string;
  year: number;
  children: Child[];

  public static clone(input: any, instance: Period) {
    instance.month = input.month;
    instance.year = input.year;
    instance.children = Utilities.deserilizeChildren(input.children, Child, 0);
  }

  public deserialize(input: any) {
    Period.clone(input, this);
    return this;
  }
}

export class Child implements Serializable<Child> {
  name: string;
  hours: number;

  public static clone(input: any, instance: Child) {
    instance.name = input.name;
    instance.hours = input.hours;
  }

  public deserialize(input: any) {
    Child.clone(input, this);
    return this;
  }
}
