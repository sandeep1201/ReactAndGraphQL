export class ActionNeeded {
  id: number;
  name: string;
  requiresDetails: boolean;
  disablesOthers: boolean;
  isSelected: boolean;
  isDisabled: boolean;

  deserialize(input: any) {
    this.id = input.id;
    this.name = input.name;
    this.requiresDetails = input.requiresDetails;
    this.disablesOthers = input.disablesOthers;
    this.isSelected = input.isSelected;
    this.isDisabled = input.isDisabled;

    return this;
  }

  isNameLong(): boolean {
    if (this.name != null && this.name.length > 30) {
      return true;
    } else {
      return false;
    }
  }
}
