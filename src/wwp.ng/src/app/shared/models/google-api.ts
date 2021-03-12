import { Address } from './address.model';

export class GoogleApi {
    cities: City[];
    schools: School[];
    streetAddresses: StreetAddress[];
    address: StreetAddress;

    deserialize(input: any) {
        this.cities = [];
        for (const c of input.cities) {
            this.cities.push(new City().deserialize(c));
        }

        this.schools = [];
        for (const s of input.schools) {
            this.schools.push(new School().deserialize(s));
        }

        this.streetAddresses = [];
        for (const st of input.streetAddresses) {
            this.streetAddresses.push(new StreetAddress().deserialize(st));
        }

        this.address = new StreetAddress().deserialize(input.address);

        return this;
    }
}

export class City {
    cityStateCountry: string;
    placeId: string;
    city: string;
    state: string;
    country: string;

    deserialize(input: any) {
        this.cityStateCountry = input.cityStateCountry;
        this.placeId = input.placeId;
        this.city = input.city;
        this.state = input.state;
        this.country = input.country;
        return this;
    }
}

export class School {
    name: string;
    placeId: string;

    deserialize(input: any) {
        this.name = input.name;
        this.placeId = input.streetAddress;

        return this;
    }
}

// export class College {
//     name: string;
//     streetAddress: string;

//     deserialize(input: any) {
//         this.name = input.name;
//         this.streetAddress = input.streetAddress;
//         return this;
//     }
// }

export class StreetAddress {
    formattedAddress: string;
    placeId: string;
    mainAddress: string;
    streetNumber: string;
    streetName: string;
    city: string;
    state: string;
    stateCode: string;
    country: string;
    countryCode: string;
    postalCode: string;

    deserialize(input: any) {
        this.formattedAddress = input.formattedAddress;
        this.placeId = input.placeId;
        this.mainAddress = input.mainAddress;
        this.streetNumber = input.streetNumber;
        this.streetName = input.streetName;
        this.city = input.city;
        this.state = input.state;
        this.stateCode = input.stateCode;
        this.country = input.country;
        this.countryCode = input.countryCode;
        this.postalCode = input.postalCode;

        return this;
    }
}
