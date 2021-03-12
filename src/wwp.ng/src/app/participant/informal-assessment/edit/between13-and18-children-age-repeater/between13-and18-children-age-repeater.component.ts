import { Component, forwardRef } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';

import { BaseRepeaterComponent } from '../../../../shared/components/base-repeater-component';
import { Teen } from '../../../../shared/models/child-youth-supports-section';

@Component({
  selector: 'app-between13-and18-children-age-repeater',
  templateUrl: './between13-and18-children-age-repeater.component.html',
  styleUrls: ['./between13-and18-children-age-repeater.component.css'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => Between13And18ChildrenAgeRepeaterComponent),
      multi: true
    }
  ]
})
export class Between13And18ChildrenAgeRepeaterComponent extends BaseRepeaterComponent<Teen> implements ControlValueAccessor {
  constructor() {
    super(Teen.create);
  }
}
