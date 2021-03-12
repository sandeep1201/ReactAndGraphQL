/**
 * The Event object used by the calendar Component.
 *
 * @export
 * @class Event
 */
export class Event {
  id: number;
  name: string;
  hours: number;
  title: string;
  type: string;
  description: string;
  start: Date;
  end: Date;
  startTime: string;
  endTime: string;
  employabilityPlanId: number;
  color: string;

  public static create(): Event {
    const event = new Event();
    event.id = 0;
    return event;
  }

  public static clone(input: any, instance: Event) {
    instance.id = input.id;
    instance.title = input.title;
    instance.type = input.type;
    instance.description = input.description;
    instance.hours = input.hours;
    instance.start = input.startDate;
    instance.end = input.endDate;
    instance.startTime = input.startTime;
    instance.endTime = input.endTime;
    instance.employabilityPlanId = input.employabilityPlanId;
  }
  public deserialize(input: any): Event {
    Event.clone(input, this);
    return this;
  }
}
