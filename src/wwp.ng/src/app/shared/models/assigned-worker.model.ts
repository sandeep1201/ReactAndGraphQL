
import * as _ from 'lodash';
import { Utilities } from '../utilities';


export class AssignedWorker {
    id: number;
    firstName: string;
    middleInitial: string;
    lastName: string;
    officeNumber: number;
    county: string;
    agency: string;
    activeStatusCode: string;
    wamsId: string;
    workerId: string;
  wiuid: string;
    public static clone(input: any, instance: AssignedWorker) {
        instance.id = input.id;
        instance.firstName = input.firstName;
        instance.middleInitial = input.middleInitial;
        instance.lastName = input.lastName;
        instance.activeStatusCode = input.activeStatusCode;
        instance.wamsId = input.wamsId;
        instance.workerId = input.workerId;
    instance.wiuid = input.wiuid;
    }

    public deserialize(input: any) {
        AssignedWorker.clone(input, this);
        return this;
    }

    get firstAndLastName(): string {
        if (this.firstName == null || this.lastName == null) {
            return '';
        }
        return this.firstName + ' ' + this.lastName;
    }

    get fullNameWithMiddleInitialTitleCase(): string {	    
        if (!Utilities.isStringEmptyOrNull(this.middleInitial)) {	        
            return _.startCase(_.toLower(this.firstName)) + ' ' + _.startCase(_.toLower(this.middleInitial)) + '. ' + _.startCase(_.toLower(this.lastName));	
        } else {	
            return _.startCase(_.toLower(this.firstName)) + ' ' + _.startCase(_.toLower(this.lastName));	
        }	
    }

}
