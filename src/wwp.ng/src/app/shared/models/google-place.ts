import { Serializable } from '../interfaces/serializable';


export class GooglePlace implements Serializable<GooglePlace> {
    placeId: string;
    formattedAddress: string;
    city: string;
    state: string;
    stateCode: string;
    country: string;
    countryCode: string;
    postalCode: string;

    private static graphObj(input: any, instance: GooglePlace) {
        instance.placeId = input.placeId;
        instance.formattedAddress = input.formattedAddress;
        instance.city = input.city;
        instance.state = input.state;
        instance.stateCode = input.stateCode;
        instance.country = input.country;
        instance.countryCode = input.countryCode;
        instance.postalCode = input.postalCode;
    }

    public deserialize(input: any) {
        GooglePlace.graphObj(input, this);
        return this;
    }
}
