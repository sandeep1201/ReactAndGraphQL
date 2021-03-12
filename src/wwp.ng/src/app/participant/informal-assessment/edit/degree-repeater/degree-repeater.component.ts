import { Component, forwardRef, Input } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';

import { BaseRepeaterComponent } from '../../../../shared/components/base-repeater-component';
import { DropDownField } from '../../../../shared/models/dropdown-field';
import { PostSecondaryDegree } from '../../../../shared/models/post-secondary-education-section';

@Component({
  selector: 'app-degree-repeater',
  templateUrl: './degree-repeater.component.html',
  styleUrls: ['./degree-repeater.component.css'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => DegreeRepeaterComponent),
      multi: true
    }
  ]
})
export class DegreeRepeaterComponent extends BaseRepeaterComponent<PostSecondaryDegree> implements ControlValueAccessor {
  @Input() degreeLevelDrop: DropDownField[];
  @Input() collegesDrop: string[];

  constructor() {
    super(PostSecondaryDegree.create);
  }
}
