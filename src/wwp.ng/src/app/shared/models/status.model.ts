

export class Status {
  pinNumber: number;

  errorMessages: string[];


  public static clone(input: any, instance: Status) {
    instance.pinNumber = input.pinNumber;
    instance.errorMessages = input.errorMessages;
  }

  public deserialize(input: any) {
    Status.clone(input, this);
    return this;
  }

}
