export class GenericDropDownMultiField<T> {
  id: T;
  name: string;
  isSelected: boolean;
  isDisabled: boolean;
  disablesOthers: boolean;
}


// GenericDropDownMultiField VS DropDownMultiField DIFF?
export class DropDownMultiField extends GenericDropDownMultiField<any>{
    id: any;
    name: string;
    isSelected: boolean;
    isDisabled: boolean;
    disablesOthers: boolean;
}

