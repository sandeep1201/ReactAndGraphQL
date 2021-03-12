import { Serializable } from '../interfaces/serializable';
import { IsEmpty } from '../interfaces/is-empty';
import { Clearable } from '../interfaces/clearable';

export class JRContactInfo implements Clearable, IsEmpty, Serializable<JRContactInfo> {
  id: number;
  canYourPhoneNumberUsed: boolean;
  phoneNumberDetails: string;
  haveAccessToVoiceMailOrTextMessages: boolean;
  voiceOrTextMessageDetails: string;
  haveEmailAddress: boolean;
  emailAddressDetails: string;
  haveAccessDailyToEmail: boolean;
  accessEmailDailyDetails: string;

  public static clone(input: any, instance: JRContactInfo) {
    instance.id = input.id;
    instance.canYourPhoneNumberUsed = input.canYourPhoneNumberUsed;
    instance.phoneNumberDetails = input.phoneNumberDetails;
    instance.haveAccessToVoiceMailOrTextMessages = input.haveAccessToVoiceMailOrTextMessages;
    instance.voiceOrTextMessageDetails = input.voiceOrTextMessageDetails;
    instance.haveEmailAddress = input.haveEmailAddress;
    instance.emailAddressDetails = input.emailAddressDetails;
    instance.haveAccessDailyToEmail = input.haveAccessDailyToEmail;
    instance.accessEmailDailyDetails = input.accessEmailDailyDetails;
  }

  public static create(): JRContactInfo {
    const x = new JRContactInfo();
    x.id = 0;
    return x;
  }
  public deserialize(input: any) {
    JRContactInfo.clone(input, this);
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
