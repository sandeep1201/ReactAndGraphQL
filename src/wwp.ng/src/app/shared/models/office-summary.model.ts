export class OfficeSummary {
    public id: number;
    public officeNumber: number;
    public officeName: string;
    public countyNumber: number;
    public countyName: string;
    public organizationCode: string;
    public organizationName: string;
    public programShortName; string;


    public static clone(input: any, instance: OfficeSummary) {
        instance.id = input.id;
        instance.officeNumber = input.officeNumber;
        instance.officeName = input.officeName;
        instance.countyNumber = input.countyNumber;
        instance.countyName = input.countyName;
        instance.organizationCode = input.organizationCode;
        instance.organizationName = input.organizationName;
        instance.programShortName = input.programShortName;
    }

    public deserialize(input: any) {
        OfficeSummary.clone(input, this);
        return this;
    }
}
