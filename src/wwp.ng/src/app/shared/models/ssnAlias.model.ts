
import { Serializable } from '../interfaces/serializable';
import { Clearable } from '../interfaces/clearable';
import { IsEmpty } from '../interfaces/is-empty';


export class SsnAlias implements Serializable<SsnAlias>, Clearable, IsEmpty {

  public id: number;
  public ssn: string;
  public typeId: number;
  public details: string;

  public static create() {
    return new SsnAlias();
  }

  public static clone(input: any, instance: SsnAlias) {
    instance.id = input.id;
    instance.ssn = input.ssn;
    instance.typeId = input.typeId;
    instance.details = input.details;
  }

  public deserialize(input: any) {
    SsnAlias.clone(input, this);
    return this;
  }

  public clear() {
    this.id = null;
    this.ssn = null;
    this.typeId = null;
    this.details = null;

  }

  public isEmpty(): boolean {
    return ((this.ssn == null || this.ssn.toString() === '') &&
      (this.typeId == null || this.typeId.toString() === '') &&
      (this.details == null || this.details.trim() === ''));
  }
}
