import { Serializable } from '../interfaces/serializable';
import { GoogleLocation } from './google-location';

export class NonSelfDirectedActivity {
  public id: number;
  public businessName: string;
  public businessLocation: GoogleLocation;
  public businessPhoneNumber: number;
  public businessStreetAddress: string;
  public businessZipAddress: string;

  public deserialize(input: any) {
    this.id = input.id;
    this.businessName = input.businessName;
    if (input.businessLocation) {
      this.businessLocation = new GoogleLocation().deserialize(input.businessLocation);
    }
    this.businessPhoneNumber = input.businessPhoneNumber;
    this.businessStreetAddress = input.businessStreetAddress;
    this.businessZipAddress = input.businessZipAddress;
    return this;
  }
}
