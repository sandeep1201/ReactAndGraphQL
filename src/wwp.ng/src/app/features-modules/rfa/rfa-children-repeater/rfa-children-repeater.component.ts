import { Component, Input, forwardRef } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { BaseRepeaterComponent } from '../../../shared/components/base-repeater-component';
import { RfaChild } from '../../../shared/models/rfa-child.model';
import { DropDownField } from '../../../shared/models/dropdown-field';

@Component({
  selector: 'app-rfa-children-repeater',
  templateUrl: './rfa-children-repeater.component.html',
  styleUrls: ['./rfa-children-repeater.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => RfaChildrenRepeaterComponent),
      multi: true
    }
  ]
})
export class RfaChildrenRepeaterComponent extends BaseRepeaterComponent<RfaChild> implements ControlValueAccessor {
  @Input() genderDrop: DropDownField[] = [];

  @Input() isReadOnly: boolean;

  constructor() {
    super(RfaChild.create);
  }
}
