import { Component, forwardRef } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';

import { BaseRepeaterComponent } from '../../../../shared/components/base-repeater-component';
import { PostSecondaryCollege } from '../../../../shared/models/post-secondary-education-section';

@Component({
  selector: 'app-college-repeater',
  templateUrl: './college-repeater.component.html',
  styleUrls: ['./college-repeater.component.css'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => CollegeRepeaterComponent),
      multi: true
    }
  ]
})
export class CollegeRepeaterComponent extends BaseRepeaterComponent<PostSecondaryCollege> implements ControlValueAccessor {
  constructor() {
    super(PostSecondaryCollege.create);
  }
}
