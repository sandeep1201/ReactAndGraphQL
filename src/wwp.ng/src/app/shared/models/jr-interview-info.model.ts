import { Serializable } from '../interfaces/serializable';
import { Clearable } from '../interfaces/clearable';
import { IsEmpty } from '../interfaces/is-empty';

export class JRInterviewInfo implements Clearable, IsEmpty, Serializable<JRInterviewInfo> {
  id: number;
  lastInterviewDetails: string;
  canLookAtSocialMedia: boolean;
  canLookAtSocialMediaDetails: string;
  haveOutfit: boolean;
  haveOutfitDetails: string;

  public static clone(input: any, instance: JRInterviewInfo) {
    instance.id = input.id;
    instance.lastInterviewDetails = input.lastInterviewDetails;
    instance.canLookAtSocialMedia = input.canLookAtSocialMedia;
    instance.canLookAtSocialMediaDetails = input.canLookAtSocialMediaDetails;
    instance.haveOutfit = input.haveOutfit;
    instance.haveOutfitDetails = input.haveOutfitDetails;
  }

  public static create(): JRInterviewInfo {
    const x = new JRInterviewInfo();
    x.id = 0;
    return x;
  }
  public deserialize(input: any) {
    JRInterviewInfo.clone(input, this);
    return this;
  }
  public clear(): void {
    this.id = null;
    // this.details = null;
  }

  public isEmpty(): boolean {
    // All properties have to be null or blank to make the entire object empty.
    return this.id == null;
  }
}
