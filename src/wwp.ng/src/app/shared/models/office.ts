import { Utilities } from '../utilities';

export class Office {
    public id: number;
    public agency: Agency;
    public officeNumber: number;


    public static clone(input: any, instance: Office) {
        instance.id = input.id;
        instance.agency = Utilities.deserilizeChild(input.agency, Agency);
        instance.officeNumber = input.officeNumber;
    }

    public deserialize(input: any) {
        Office.clone(input, this);
        return this;
    }
}

export class Agency {
    public id: number;
    public agencyNumber: number;
    public agencyName: string;
    public agencyCounty: string;
    public agencyCode: string;


    public static clone(input: any, instance: Agency) {
        instance.id = input.id;
        instance.agencyNumber = input.agencyNumber;
        instance.agencyName = input.agencyName;
        instance.agencyCounty = input.agencyCounty;
        instance.agencyCode = input.agencyCode;
    }

    public deserialize(input: any) {
        Agency.clone(input, this);
        return this;
    }


}
