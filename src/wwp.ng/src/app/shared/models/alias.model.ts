
import { Person } from './person.model';
import { Clearable } from '../interfaces/clearable';
import { IsEmpty } from '../interfaces/is-empty';
export class PersonAlias extends Person implements Clearable, IsEmpty {

    typeId: number;
    typeName: string;
    details: string;

    public static create(): PersonAlias {
        const obj = new PersonAlias();
        obj.id = 0;
        return obj;
    }

    public clear(): void {
        this.firstName = null;
        this.middleInitial = null;
        this.lastName = null;
        this.suffix = null;
        this.dateOfBirth = null;
        this.snn = null;
        this.gender = null;
        this.details = null;
        this.typeId = null;
        this.typeName = null;
    }


    public isEmpty(): boolean {
        // All properties have to be null or blank to make the entire object empty.
        return ((this.firstName == null || this.firstName.trim() === '') &&
            (this.middleInitial == null || this.middleInitial.trim() === '') &&
            (this.lastName == null || this.lastName.trim() === '') &&
            (this.suffix == null || this.suffix.trim() === '') &&
            (this.dateOfBirth == null || this.dateOfBirth.trim() === '') &&
            (this.snn == null || this.snn.toString() === '') &&
            (this.gender == null || this.gender.trim() === '') &&
            (this.details == null || this.details.trim() === '') &&
            (this.typeId == null || this.typeId.toString() === '') &&
            (this.typeName == null || this.typeName.toString() === '')
        );
    }


}
