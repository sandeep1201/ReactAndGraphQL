import { Component, Input, forwardRef, } from '@angular/core';
import { BaseRepeaterComponent } from '../../../shared/components/base-repeater-component';
import { SsnAlias } from '../../../shared/models/SsnAlias.model';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { DropDownField } from '../../../shared/models/dropdown-field';

@Component({
  selector: 'app-alias-ssn-repeater',
  templateUrl: './alias-ssn-repeater.component.html',
  styleUrls: ['./alias-ssn-repeater.component.css'],
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => AliasSsnRepeaterComponent),
    multi: true
  }]
})
export class AliasSsnRepeaterComponent extends BaseRepeaterComponent<SsnAlias> implements ControlValueAccessor {

  @Input() ssnTypesDrop: DropDownField[];

  @Input() isReadOnly = false;

  public maxNumberOfItems = 2;
  constructor() { super(SsnAlias.create); }


}
