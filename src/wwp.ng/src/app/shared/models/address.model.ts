import { GoogleLocation } from './google-location';

export class Address {
    public location: GoogleLocation;
    public street: string;
    public apt: string;
    public zipCode: string;
}