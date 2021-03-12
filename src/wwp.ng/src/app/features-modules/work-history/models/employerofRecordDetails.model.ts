import { GoogleLocation } from '../../../shared/models/google-location';

export class EmployerOfRecordDetails {
  public companyName: string;
  public fein: string;
  public location: GoogleLocation;
  public jobSectorId: number;
  public jobSectorName: string;
  public contactId: number;

  public deserialize(input: any) {
    this.companyName = input.companyName;
    this.fein = input.fein;
    if (input.location) {
      this.location = new GoogleLocation().deserialize(input.location);
    }
    this.jobSectorId = input.jobSectorId;
    this.jobSectorName = input.jobSectorName;
    this.contactId = input.contactId;
    return this;
  }
}
