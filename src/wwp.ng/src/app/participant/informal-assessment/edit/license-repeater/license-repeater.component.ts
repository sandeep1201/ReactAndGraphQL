import { Component, forwardRef, Input } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';

import { BaseRepeaterComponent } from '../../../../shared/components/base-repeater-component';
import { DropDownField } from '../../../../shared/models/dropdown-field';
import { PostSecondaryLicense } from '../../../../shared/models/post-secondary-education-section';

@Component({
  selector: 'app-license-repeater',
  templateUrl: './license-repeater.component.html',
  styleUrls: ['./license-repeater.component.css'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => LicenseRepeaterComponent),
      multi: true
    }
  ]
})
export class LicenseRepeaterComponent extends BaseRepeaterComponent<PostSecondaryLicense> implements ControlValueAccessor {
  @Input() licenseValidDrop: DropDownField;
  @Input() licenseTypeDrop: DropDownField;

  constructor() {
    super(PostSecondaryLicense.create);
  }
}
