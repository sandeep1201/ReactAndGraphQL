import { DropDownMultiField } from '../../shared/models/dropdown-multi-field';

export class DropDownField {
  constructor(
    public id?: any,
    public name?: string,
    public isSelected?: boolean,
    public code?: string,
    public canSelfDirect?: boolean,
    public optionCd?: string,
    public optionId?: number,
    public isIncome?: boolean,
    public isAsset?: boolean,
    public isVehicle?: boolean,
    public isVehicleValue?: boolean,
    public isSystemUseOnly?: boolean,
    public canEdit?: boolean
  ) {
    this.id = id;
    this.name = name;
    this.isSelected = isSelected;
    this.code = code;
    this.optionCd = optionCd;
    this.optionId = optionId;
    this.isIncome = isIncome;
    this.isAsset = isAsset;
    this.isVehicle = isVehicle;
    this.isVehicleValue = isVehicleValue;
    this.isSystemUseOnly = isSystemUseOnly;
    this.canEdit = canEdit;
  }

  subTypes?: DropDownMultiField[];
}
