import { Serializable } from '../interfaces/serializable';


export class GoogleLocation implements Serializable<GoogleLocation> {
    description: string;
    googlePlaceId: string;
    city: string;
    state: string;
    country: string;
    fullAddress: string;
    addressPlaceId: string;
    zipAddress: string;

    // Not a part of google.
    aptUnit: string;

    public static create(): GoogleLocation {
        const item = new GoogleLocation();
        return item;
    }

    private static graphObj(input: any, instance: GoogleLocation) {
        instance.description = input.description;
        instance.googlePlaceId = input.googlePlaceId;
        instance.city = input.city;
        instance.state = input.state;
        instance.country = input.country;
        instance.fullAddress = input.fullAddress;
        instance.zipAddress = input.zipAddress;
        instance.aptUnit = input.aptUnit;
    }

    public deserialize(input: any) {
        GoogleLocation.graphObj(input, this);
        return this;
    }

    /**
     * Detects whether or not a FamilyMember object is effectively empty.
     *
     * @returns {boolean}
     *
     * @memberOf FamilyMember
     */
    public isEmpty(): boolean {
        // All properties have to be null or blank to make the entire object empty.
        if (
            (this.city == null) &&
            (this.state == null) &&
            (this.country == null) &&
            (this.googlePlaceId == null)
        ) {
            return true;
        }
        return false;
    }

}
