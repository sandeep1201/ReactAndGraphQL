

export class PersonName {

  firstName: string;
  middleInitial: string;
  lastName: string;
  suffix: string;


  public static clone(input: any, instance: PersonName) {
    instance.firstName = input.firstName;
    instance.middleInitial = input.middleInitial;
    instance.lastName = input.lastName;
    instance.suffix = input.suffix;
  }

  public deserialize(input: any) {
    PersonName.clone(input, this);
    return this;
  }



}
