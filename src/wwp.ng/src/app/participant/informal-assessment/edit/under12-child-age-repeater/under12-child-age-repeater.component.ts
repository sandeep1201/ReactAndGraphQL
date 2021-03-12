import { Component, Input, forwardRef } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { BaseRepeaterComponent } from '../../../../shared/components/base-repeater-component';
import { Child } from '../../../../shared/models/child-youth-supports-section';
import { DropDownField } from '../../../../shared/models/dropdown-field';

@Component({
  selector: 'app-under12-child-age-repeater',
  templateUrl: './under12-child-age-repeater.component.html',
  styleUrls: ['./under12-child-age-repeater.component.css'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => Under12ChildAgeRepeaterComponent),
      multi: true
    }
  ]
})
export class Under12ChildAgeRepeaterComponent extends BaseRepeaterComponent<Child> implements ControlValueAccessor {
  @Input() childYouthSupportsArrangmentDrop: DropDownField;

  constructor() {
    super(Child.create);
  }
}
