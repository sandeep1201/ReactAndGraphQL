import * as _ from 'lodash';

export class LearnFareFEP {
  id: number;
  wamsId: string;
  workerId: string;
  firstName: string;
  lastName: string;
  activeStatusCode: string;

  public static clone(input: any, instance: LearnFareFEP) {
    instance.id = input.id;
    instance.wamsId = input.wamsId;
    instance.workerId = input.workerId;
    instance.firstName = input.firstName;
    instance.lastName = input.lastName;
    instance.activeStatusCode = input.activeStatusCode;
  }

  public deserialize(input: any) {
    LearnFareFEP.clone(input, this);
    return this;
  }

  get fullNameTitleCase() {
    return _.startCase(_.toLower(this.firstName)) + ' ' + _.startCase(_.toLower(this.lastName))
  }
}
