export class User {
  private _agencyCode = '';

  username: string;
  firstName: string;
  lastName: string;
  officeName: string;
  mainFrameId: string;
  authorizations: string[];
  roles: string;
  elevatedAccessPin: string;
  wiuid: string;

  get agencyCode(): string {
    return this._agencyCode;
  }
  set agencyCode(v: string) {
    this._agencyCode = v;
  }

  get isTribalUser(): boolean {
    if (this.roles === 'Tribal User') {
      return true;
    } else {
      return false;
    }
  }
}
