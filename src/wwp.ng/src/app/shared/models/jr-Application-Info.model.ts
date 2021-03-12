import { Serializable } from '../interfaces/serializable';
import { Clearable } from '../interfaces/clearable';
import { IsEmpty } from '../interfaces/is-empty';

export class JRApplicationInfo implements Clearable, IsEmpty, Serializable<JRApplicationInfo> {
  id: number;
  canSubmitOnline: boolean;
  canSubmitOnlineDetails: string;
  haveCurrentResume: boolean;
  haveCurrentResumeDetails: string;
  haveProfessionalReference: boolean;
  haveProfessionalReferenceDetails: string;
  needDocumentLookupId: number;
  needDocumentLookupName: string;
  needDocumentDetail: string;

  public static clone(input: any, instance: JRApplicationInfo) {
    instance.id = input.id;
    instance.canSubmitOnline = input.canSubmitOnline;
    instance.canSubmitOnlineDetails = input.canSubmitOnlineDetails;
    instance.haveCurrentResume = input.haveCurrentResume;
    instance.haveCurrentResumeDetails = input.haveCurrentResumeDetails;
    instance.haveProfessionalReference = input.haveProfessionalReference;
    instance.haveProfessionalReferenceDetails = input.haveProfessionalReferenceDetails;
    instance.needDocumentLookupId = input.needDocumentLookupId;
    instance.needDocumentLookupName = input.needDocumentLookupName;
    instance.needDocumentDetail = input.needDocumentDetail;
  }

  public static create(): JRApplicationInfo {
    const x = new JRApplicationInfo();
    x.id = 0;
    return x;
  }
  public deserialize(input: any) {
    JRApplicationInfo.clone(input, this);
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
