import * as _ from 'lodash';
import { Utilities } from '../utilities';
export class AgencyWorker {
    id: number;
    wamsId: string;
    workerId: string;
    firstName: string;
    middleInitial: string;
    lastName: string;
    organization: string;
    isActive: boolean;
  wiuid: string;

    public static clone(input: any, instance: AgencyWorker) {
        instance.id = input.id;
        instance.wamsId = input.wamsId;
        instance.workerId = input.workerId;
        instance.firstName = input.firstName;
        instance.middleInitial = input.middleInitial;
        instance.lastName = input.lastName;
        instance.organization = input.organization;
        instance.isActive = input.isActive;
    instance.wiuid = input.wiuid;
    }

    public deserialize(input: any) {
        AgencyWorker.clone(input, this);
        return this;
    }

    get fullNameTitleCase() {
        return _.startCase(_.toLower(this.firstName)) + ' ' + _.startCase(_.toLower(this.lastName));
    }

    get fullNameWithMiddleInitialTitleCase(): string {
        if (!Utilities.isStringEmptyOrNull(this.middleInitial)) {
            return _.startCase(_.toLower(this.firstName)) + ' ' + _.startCase(_.toLower(this.middleInitial)) + '. ' + _.startCase(_.toLower(this.lastName));
        } else {
            return _.startCase(_.toLower(this.firstName)) + ' ' + _.startCase(_.toLower(this.lastName));
        }
    }


}
